﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UnityEngine;
using CM3D2.AlwaysColorChangeEx.Plugin;
using CM3D2.AlwaysColorChangeEx.Plugin.UI;
using CM3D2.AlwaysColorChangeEx.Plugin.Util;

namespace CM3D2.AlwaysColorChangeEx.Plugin.Data {
    /// <summary>
    /// マテリアルの変更情報を扱うデータクラス.
    /// スライダー操作中のデータを保持する.
    /// </summary>
    public class ACCMaterial {
        internal static Settings settings = Settings.Instance;

        public ACCMaterial Original {get; private set;}
        public Renderer renderer;
        public Material material;
        public string name;
        public ShaderType type;

        public EditValue renderQueue  = new EditValue(2000f, EditRange.renderQueue);

        public EditColor[] editColors;
        public EditValue[] editVals;

        public string rqEdit;
        protected ACCMaterial(ShaderType type) {
            this.type = type;
            InitType();
        }

        // 編集前と後でオブジェクトを分ける場合用（未使用）
        public ACCMaterial(ACCMaterial src) {
            Original = src;
            renderer = src.renderer;
            material = src.material;
            name = src.name;
            //this.shader = src.shader;
            //this.type1 = src.type1;
            type = src.type;

            renderQueue = src.renderQueue;
            
            // TODO 配列の中身はディープコピーとする 
            editColors = src.editColors;
            editVals = src.editVals;
        }

        
        public ACCMaterial(Material m, Renderer r = null) {
            material = m;
            renderer = r;
            name = m.name;
            type = ShaderType.Resolve(m.shader.name);
            if (type == ShaderType.UNKNOWN) {
                return;
                //throw new Exception("ShaderType has not found:" + m.shader.name);
            }

            renderQueue.Set( m.renderQueue );

            rqEdit = renderQueue.ToString();
            
            InitType();
        }
        private void InitType() {
            // Color生成
            var colProps = type.colProps;
            editColors = new EditColor[colProps.Length];
            for (var i=0; i<colProps.Length; i++) {
                var colProp = colProps[i];
                var ec = new EditColor(null, colProp.colorType);
                if (material != null) {
                    ec.Set( material.GetColor(colProps[i].propId) );
                } else {
                    ec.Set( colProps[i].defaultVal );
                }
                editColors[i] = ec;
            }

            // float生成
            var props = type.fProps;
            editVals = new EditValue[props.Length];
            for (var i=0; i<props.Length; i++) {
                var val = props[i].defaultVal;
                if (material != null) {
                    val = material.GetFloat(props[i].propId);
                }
                editVals[i] = new EditValue(val, props[i].range);
            }
        }

        private Color GetColor(int i) {
            if (i < editColors.Length) {
                return editColors[i].val.HasValue ? editColors[i].val.Value : type.colProps[i].defaultVal;
            }
            return Color.white;
        }

        public void ChangeShader(string shaderName, int shaderIdx=-1) {
            var shader = Shader.Find(shaderName);
            if (shader == null) return;

            var rq = material.renderQueue;
            material.shader = shader;
            material.renderQueue = rq;
            var type1 = (shaderIdx != -1) ? ShaderType.Resolve(shaderIdx) : ShaderType.Resolve(shaderName);
            if (type1 == ShaderType.UNKNOWN) return;

            Update(type1);
            LogUtil.Debug("selected shader updated");

            //} else {
            //    // var script = CustomShaderHolder.ShaderScripts[shaderIdx];
            //    // var matType = CustomShaderHolder.customMat[shaderIdx];
            //    // material = new Material(script);
            //    // // http://forum.unity3d.com/threads/setting-renderer-materials-by-index-doesnt-work.59150/
            //    // // 一見無駄な処理だが、materialsに直接代入しなければ反映されない（Unity）
            //    // var mats = edited.renderer.materials;
            //    // mats[matIdx] = material;
            //    // renderer.materials = mats;
            //    //
            //    // Update(matType);
            //    
            //    Shader selected;
            //    if (CustomShaderHolder.customShader.TryGetValue(shaderIdx, out selected)) {
            //        material.shader = selected;
            //        var matType = CustomShaderHolder.customMat[shaderIdx];
            //        Update(matType);
            //        LogUtil.Debug("shader udated.", shaderName);
            //    } else {
            //        LogUtil.Debug("shader not found.", shaderName);
            //    }
        }

        public void Update(ShaderType sdrType) {
            if (type == sdrType) return;
            // TODO 変更前のマテリアルから設定値をロード


            // 同一長の場合でも更新（Alphaの有無が異なるケースがある）
            var colProps = sdrType.colProps;
            var createdColors = new EditColor[colProps.Length];
            for (var i=0; i<colProps.Length; i++) {
                var colProp = colProps[i];
                if (i < editColors.Length && editColors[i].val.HasValue) {
                    // カラータイプが異なる場合は、インスタンスを作り直して色をコピー
                    if (editColors[i].type == colProp.colorType) {
                        createdColors[i] = editColors[i];
                    } else {
                        createdColors[i] = new EditColor(editColors[i].val, colProp.colorType); 
                    }
                } else {
                    var ec = new EditColor(null, colProp.colorType);
                    if (material != null) {
                        ec.Set( material.GetColor(colProps[i].propId) );
                    } else {
                        ec.Set( (Original != null)? Original.GetColor(i): colProps[i].defaultVal );
                    }
                    createdColors[i] = ec;
                }
            }
            editColors = createdColors;
            
            
            var props = sdrType.fProps;
            var createdVals = new EditValue[props.Length];
            for (var i=0; i<props.Length; i++) {
                var val = (material != null)? material.GetFloat(props[i].propId) : props[i].defaultVal;
                createdVals[i] = new EditValue(val, props[i].range);
            }
            editVals = createdVals;

            // テクスチャ情報の初期化
            foreach (var texProp in sdrType.texProps) {
                // セットしてないテクスチャは空テクスチャをセット
                if (material != null && !material.HasProperty(texProp.keyName)) {
                    material.SetTexture(texProp.propId, new Texture());
                }
            }

            type = sdrType;
        }

//        public void ReflectTo(Material m) {
//            m.SetFloat("_SetManualRenderQueue", renderQueue.val);
//            m.renderQueue = (int)renderQueue.val;
//
//            if (type1.hasColor) {
//                m.SetColor(PROP_COLOR, color.val.Value);
//            }
//            if (type1.isLighted) {
//                m.SetColor(PROP_SHADOWC, shadowColor.val.Value);
//                m.SetFloat("_Shininess", shininess.val);
//            }
//            if (type1.isOutlined) {
//                m.SetColor(PROP_OUTLINEC, outlineColor.val.Value);
//                m.SetFloat("_OutlineWidth", outlineWidth.val);
//            }
//            if (type1.isToony) {
//                m.SetColor(PROP_RIMC, rimColor.val.Value);
//                m.SetFloat("_RimPower", rimPower.val);
//                m.SetFloat("_RimShift", rimShift.val);
//            }
//            if (type1.isHair) {
//                m.SetFloat("_HiRate", hiRate.val);
//                m.SetFloat("_HiPow", hiPow.val);
//            }
//            if (type1.isHair) {
//                m.SetFloat("_HiRate", hiRate.val);
//                m.SetFloat("_HiPow", hiPow.val);
//            }
//            if (type1.hasFloat1) {
//                m.SetFloat("_FloatValue1", floatVal1.val);
//            }
//            if (type1.hasFloat2) {
//                m.SetFloat("_FloatValue2", floatVal2.val);
//            }
//            if (type1.hasFloat3) {
//                m.SetFloat("_FloatValue3", floatVal3.val);
//            }
//            if (type1.hasCutoff) {
//                m.SetFloat("_Cutoff", cutoff.val);
//            }
//        }

        public bool HasChanged(ACCMaterial mate) {
            // 同一シェーダを想定
            if (editColors.Where((t, i) => t.val != mate.editColors[i].val).Any()) {
                return true;
            }

            return editVals.Where((t, i) => !NumberUtil.Equals(t.val, mate.editVals[i].val)).Any();
        }
                
        public void SetColor(string propName, Color c) {
            try {
                var propKey = (PropKey)Enum.Parse(typeof(PropKey), propName);
                for (var i=0; i<type.colProps.Length; i++) {
                    var colProp = type.colProps[i];
                    if (colProp.key != propKey) continue;

                    editColors[i].Set ( c );
                    return;
                }
                LogUtil.Debug("propName mismatched:", propName);
            } catch(Exception e) {
                LogUtil.Debug("unsupported propName found:", propName, e);
            }
        }

        public void SetFloat(string propName, float f) {
            try {
                var propKey = (PropKey)Enum.Parse(typeof(PropKey), propName);
                for (var i=0; i<type.fProps.Length; i++) {
                    var prop = type.fProps[i];
                    if (prop.key != propKey) continue;

                    editVals[i].Set( f );
                    return;
                }
                LogUtil.Debug("propName mismatched:", propName);
            } catch(Exception e) {
                LogUtil.Debug("unsupported propName found:", propName, e);
            }
        }
//        public bool hasChanged(ACCMaterial mate) {
//            // 同一シェーダを想定
//            if (type.hasColor) {
//                if (color != mate.color) return true;
//            }
//            if (type.isLighted) {
//                if (shadowColor != mate.shadowColor) return true;
//                if (!NumberUtil.Equals(shininess.val, mate.shininess.val)) return true;
//            }
//            if (type.isOutlined) {
//                if (outlineColor != mate.outlineColor) return true;
//                if (!NumberUtil.Equals(outlineWidth.val, mate.outlineWidth.val)) return true;
//            }
//            if (type.isToony) {
//                if (rimColor != mate.rimColor) return true;
//                if (!NumberUtil.Equals(rimPower.val, mate.rimPower.val) || !NumberUtil.Equals(rimShift.val, mate.rimShift.val)) return true;
//            }
//            if (type.isHair) {
//                if (!NumberUtil.Equals(hiRate.val, mate.hiRate.val) || !NumberUtil.Equals(hiPow.val, mate.hiPow.val)) return true;
//            }
//            if (type.hasFloat1) {
//                if (!NumberUtil.Equals(floatVal1.val, mate.floatVal1.val)) return true;
//            }
//            if (type.hasFloat2) {
//                if (!NumberUtil.Equals(floatVal2.val, mate.floatVal2.val)) return true;
//            }
//            if (type.hasFloat3) {
//                if (!NumberUtil.Equals(floatVal3.val, mate.floatVal3.val)) return true;
//            }
//            if (type.hasCutoff) {
//                if (!NumberUtil.Equals(cutoff.val, mate.cutoff.val)) return true;
//            }
//            return false;
//        }
//        public bool ShaderChanged() {
//            return original != null && (shader != original.shader);
//        }
    }
    /// <summary>
    /// エクスポート機能用の機能を拡張したデータクラス
    /// </summary>
    public class ACCMaterialEx : ACCMaterial {
        private static readonly FileUtilEx OUT_UTIL = FileUtilEx.Instance;
        public readonly Dictionary<PropKey, ACCTextureEx> texDic = new Dictionary<PropKey, ACCTextureEx>(5);
        public string name1;
        public string name2;
        private ACCMaterialEx(ShaderType type) : base(type) { }


        public static ACCMaterialEx Load(string file) {

            bool onBuffer;
            using ( var reader = new BinaryReader(FileUtilEx.Instance.GetStream(file, out onBuffer), Encoding.UTF8)) {
                var header = reader.ReadString(); // header
                if (onBuffer || reader.BaseStream.Position > 0) {
                    if (header == FileConst.HEAD_MATE) {
                        return Load(reader);
                    }
                    var msg = LogUtil.Log("指定されたファイルのヘッダが不正です。", header, file);
                    throw new ACCException(msg.ToString());
                }
            }
            // arc内のファイルがロードできない場合の回避策: Sybaris 0410向け対策. 一括読み込み
            // バッファサイズより大きく、かつ最初からの読み込みが出来なくなったケース
            using (var reader = new BinaryReader(new MemoryStream(FileUtilEx.Instance.LoadInternal(file), false), Encoding.UTF8)) {
                var header = reader.ReadString(); // hader
                if (header == FileConst.HEAD_MATE) {
                    return Load(reader);
                }
                var msg = LogUtil.Log("指定されたファイルのヘッダが不正です。", header, file);
                throw new ACCException(msg.ToString());
            }
        }
        private static ACCMaterialEx Load(BinaryReader reader) {
            var version = reader.ReadInt32();
            var name1 = reader.ReadString();
            var name2 = reader.ReadString();
            var shaderName1 = reader.ReadString();
            var shaderType = ShaderType.Resolve(shaderName1);

            var created = new ACCMaterialEx(shaderType) {
                name1 = name1,
                name2 = name2
            };

            //created.shader = created.type;
            //created.type1 = ShaderMapper.resolve(shaderName1);
            //created.shader = created.type1.shader;
            
            var shaderName2 = reader.ReadString();

            while(true) {
                var type = reader.ReadString();
                if (type == "end") break;

                var propName = reader.ReadString();
                switch (type) {
                    case "tex":
                        var sub = reader.ReadString();
                        switch (sub) {
                        case "tex2d":
                            
                            var tex = new ACCTextureEx(propName);
                            tex.editname = reader.ReadString();
                            tex.txtpath  = reader.ReadString();
                            tex.texOffset.x = reader.ReadSingle();
                            tex.texOffset.y = reader.ReadSingle();
                            tex.texScale.x = reader.ReadSingle();
                            tex.texScale.y = reader.ReadSingle();                                
                            created.texDic[tex.propKey] = tex;
                            break;
                        case "null":
                            break;
                        case "texRT":
                            reader.ReadString();
                            reader.ReadString();
                            break;
                    }
                    break;
                case "col":
                case "vec":
                    var c = new Color(reader.ReadSingle(), reader.ReadSingle(),
                                      reader.ReadSingle(), reader.ReadSingle());
                    created.SetColor(propName, c);
                    break;
                case "f":
                    var f = reader.ReadSingle();
                    created.SetFloat(propName, f);
                    break;
                }
            }
            return created;           
        }

        public static void Write(string filepath, ACCMaterialEx mate) {
            using ( var writer = new BinaryWriter(File.OpenWrite(filepath)) ) {
                Write(writer, mate);
            }
        }

        public static void Write(BinaryWriter writer, ACCMaterialEx mate) {
            writer.Write(FileConst.HEAD_MATE);
            writer.Write(1000); // version

            writer.Write(mate.name1);
            writer.Write(mate.name2);

            var shaderName1 = mate.type.name;
            writer.Write(shaderName1);
            var shaderName2 = ShaderType.GetShader2(shaderName1);
            writer.Write(shaderName2);

            // tex
            foreach (var texProp in mate.type.texProps) {
                if (texProp.key == PropKey._RenderTex) {
                    writer.Write("null");
                } else {
                    writer.Write("tex2d");
                    var tex = mate.texDic[texProp.key];
                    writer.Write( tex.editname );
                    writer.Write( tex.txtpath );

                    OUT_UTIL.Write(writer, tex.texOffset);
                    OUT_UTIL.Write(writer, tex.texScale);
                }
            }
            // col
            for (var i=0; i<mate.editColors.Length; i++) {
                var colProp = mate.type.colProps[i];
                var eColor = mate.editColors[i];

                // PropType.col
                writer.Write(colProp.type.ToString());
                writer.Write(colProp.keyName);
                OUT_UTIL.Write(writer, eColor.val.Value);
            }
            // f
            for (var i=0; i<mate.editVals.Length; i++) {
                var prop = mate.type.fProps[i];
                var eVal = mate.editVals[i];

                // PropType.f
                writer.Write(prop.type.ToString());
                writer.Write(prop.keyName);
                writer.Write(eVal.val);
            }
        }
    }
}
