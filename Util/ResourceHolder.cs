﻿using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace CM3D2.AlwaysColorChangeEx.Plugin.Util {
    /// <summary>
    /// リソースのロードユーティリティ
    /// </summary>
    public sealed class ResourceHolder {
        private static readonly ResourceHolder INSTANCE = new ResourceHolder();
        public static ResourceHolder Instance {
            get {
                return INSTANCE;
            }
        }
        private static readonly FileUtilEx outUtil = FileUtilEx.Instance;
        private readonly Assembly asmbl = Assembly.GetExecutingAssembly();
        private ResourceHolder() {}
        private Texture2D dirImage;
        private Texture2D fileImage;
        private Texture2D pictImage;
        private Texture2D copyImage;
        private Texture2D pasteImage;
        private Texture2D plusImage;
        private Texture2D minusImage;
        private Texture2D checkonImage;
        private Texture2D checkoffImage;
        public Texture2D PictImage {
            get {
                if (pictImage == null) pictImage = LoadTex("picture");
                return pictImage;
            }
        }
        public Texture2D FileImage {
            get {
                if (fileImage == null) fileImage = LoadTex("file");
                return fileImage;
            }
        }
        public Texture2D DirImage {
            get {
                if (dirImage == null) dirImage = LoadTex("folder");
                return dirImage;
            }
        }
        public Texture2D CopyImage {
            get {
                if (copyImage == null) copyImage = LoadTex("copy");
                return copyImage;
            }
        }
        public Texture2D PasteImage {
            get {
                if (pasteImage == null) pasteImage = LoadTex("paste");
                return pasteImage;
            }
        }
        public Texture2D PlusImage {
            get {
                if (plusImage == null) plusImage = LoadTex("plus");
                return plusImage;
            }
        }
        public Texture2D MinusImage {
            get { return minusImage ?? (minusImage = LoadTex("minus")); }
        }
        public Texture2D CheckonImage {
            get { return checkonImage ?? (checkonImage = LoadTex("checkon")); }
        }
        public Texture2D CheckoffImage {
            get { return checkoffImage ?? (checkoffImage = LoadTex("checkoff")); }
        }

        private GUIContent checkon;
        public GUIContent Checkon {
            get { return checkon ?? (checkon = new GUIContent(CheckonImage)); }
        }
        private GUIContent checkoff;
        public GUIContent Checkoff {
            get { return checkoff ?? (checkoff = new GUIContent(CheckoffImage)); }
        }

        private Texture2D LoadTex(string name) {
            try {
                using (var fs = asmbl.GetManifestResourceStream(name + ".png")) {
                    var tex2d = outUtil.LoadTexture(fs);
                    tex2d.name = name;
                    LogUtil.Debug("resource file image loaded :", name);
                    return tex2d;
                }
            } catch(Exception e) {
                LogUtil.Log("アイコンリソースのロードに失敗しました。空として扱います", name, e);
                return new Texture2D(2, 2);
            }
        }

        internal byte[] LoadBytes(string path) {
            try {
                var buffer = new byte[8192];
                using (var fs = asmbl.GetManifestResourceStream(path)) {
                    if (fs != null) {
                        using (var ms = new MemoryStream((int) fs.Length)) {
                            int read;
                            while ((read = fs.Read(buffer, 0, buffer.Length)) > 0) {
                                ms.Write(buffer, 0, read);
                            }
                            return ms.ToArray();
                        }
                    }
                }
            } catch(Exception e) {
                LogUtil.Log("リソースのロードに失敗しました。path=", path, e);
                throw;
            }

            return new byte[0];
        }

        public void Clear() {
            if (pictImage != null)  UnityEngine.Object.DestroyImmediate(pictImage);
            if (dirImage  != null)  UnityEngine.Object.DestroyImmediate(dirImage);
            if (fileImage != null)  UnityEngine.Object.DestroyImmediate(fileImage);
            if (copyImage != null)  UnityEngine.Object.DestroyImmediate(copyImage);
            if (pasteImage != null) UnityEngine.Object.DestroyImmediate(pasteImage);
            if (plusImage  != null) UnityEngine.Object.DestroyImmediate(plusImage);
            if (minusImage != null) UnityEngine.Object.DestroyImmediate(minusImage);
            if (checkonImage != null) UnityEngine.Object.DestroyImmediate(checkonImage);
            if (checkoffImage != null) UnityEngine.Object.DestroyImmediate(checkoffImage);
            pictImage = null;
            dirImage  = null;
            fileImage = null;
            copyImage = null;
            pasteImage = null;
            plusImage  = null;
            minusImage = null;
            checkonImage = null;
            checkoffImage = null;
        }
    }
}
