# CM3D2.AlwaysColorChange.Plugin

パーツごとに好きに色変更できるようにするプラグインです。  
まだ実験段階です。  

  オリジナルの[kf-cm3d2](https://github.com/kf-cm3d2)さんとは別人による勝手な改造版です。  


## 改造点
#### 0.0.4.3
* iniファイル追加 (KeyCode設定用)
* MOD出力時の処理を修正
 * menuファイルの出力ミスを一部修正
 * materialファイルが稀に出力されない問題を一部修正
* 保存XMLのデータを一部修正
* ダンス(scarlet leap)でも可能に
* カラー変更機能
 * \_HiPowや\_HiRate、\_HiTexに対応
 * シェーダに応じて、変更不可能なスライダーを非表示化
 * 表示されていないパーツのボタンをdisabledに
 * 各種スライダーの値範囲を変更(Shininess,RimShiftの負値対応)
* UI関係
 * 表示するボタンの間隔を縮小
 * ホイール操作のカメラ拡大縮小処理について他のMODへの影響が出ないように抑制
 * ファイル選択について前回選択時のディレクトリが選択されない問題を修正
* ソース
 * クラス構造を勝手に改造（途中）
 * 字下げスタイルをK&Rに変更（これは個人的な好みで本来のC\#のスタイルではありません。すみません）
  等


以下はオリジナルのREADMEです。

---

## 導入方法
#### 前提条件  
UnityInjectorが導入済みであること。

#### インストール  
Download ZIPボタンを押してzipファイルをダウンロードします。  
zipファイルの中にあるUnityInjectorフォルダを、CM3D2フォルダにコピーしてください。

#### 自分でコンパイルする場合  
compile.batをサンプルで用意しました。  
* CM3D2_Managed=C:\KISS\CM3D2_KAIZOU\CM3D2x64_Data\Managed  
* C:\Windows\Microsoft.NET\Framework\v3.5\csc  
を自分の環境に合わせて変更し実行すればできるはず。

## 機能概要
#### 対象シーン
* エディット
* 夜伽
* ダンス
* ADVパート

で動作する（はず）です。

#### 使用方法
###### メニュー操作
F12キーでメニューが開きます。  
キーアサインを変更したい場合は、ソースを編集・コンパイルしてください。  

![GUI](http://i.imgur.com/sMm5lo9.jpg  "GUI")

パーツを選択し、RGBAをスライダーで変更後、適用ボタンで色が変更されるはずです。  
パーツの元の色によっては変わらないかもしれません。  
Aを1未満にすると、無理やりTransシェーダーを適用して透過します。  

###### 機能説明
* マスククリア  
  maskItemで消去されているアイテムをすべて表示します。  
  表示後は、個別にα値変更で不要なものを消してください。
* ノード表示切り替え  
  ノード（身体の部位）の表示・非表示を切り替えられます。  
  適用ボタンで反映。
* 保存  
  現在の衣装・設定値をプリセットとして保存します。  
  プリセット適用で呼び出すことがます。  
  - マスククリアを有効にする  
    プリセット適用時、マスククリアを適用するようにします。
  - 身体も保存する  
    髪型・顔等も保存します。（プリセットの服／体と同等）
* プリセット適用  
  保存したプリセットを選択・適用します。
* menu/mate保存  
  現在のスロットの設定値をmenu/mateファイルに出力します。  
* テクスチャ変更  
  変更できるだけで、保存等はできません。インターフェースも適当です。  
  texはMod以下ないしSybaris/deflac等で読み込めるフォルダ内に存在する必要があります。  
  arc内のもファイル名を直接入力して[適]ボタンで適用できます。  
  pngはファイル選択ダイアログで選択できる範囲ならどこでもいけるはず。  

###### menu/mate保存の仕様について
現在は以下のような仕様となっています。
* 保存先  
  Mod/ACC以下（標準Modで読み込めます）
* ファイル名  
  保存ダイアログが開くので、そこでファイル名等変更します。  
  既に同名のメニューおよびマテリアルファイルが存在する場合、保存できません。  
  アイコン・モデルファイルは別で、変更がなければそのまま、変更した場合は変更したファイル名でコピーして保存します。  
  適宜コピーしたファイルを編集してください。
  ※標準Modがパス関係なく読み込めるので、種類ごとにフォルダ分けしません。
* priority  
  9999としているので、各カテゴリの後ろのほうに追加されると思います。  
* わかっていること
  - freeカラー対応の素材（髪、肌等）は非対応  
    もともと変更できるのだから、不要かと・・・
  - αは元々透過対応シェーダーのものしか効かない  
    非対応に適用するにはmodelも変更する必要がある＆maskItem・node消去もあわせてやらないと・・・  
	仕組み的には実装可能なので、今後できれば。
  - .modには非対応  
    .modのフォーマット自体まだまだ変わりそうなので様子見かな・・・

  他にも不具合あるかもしれません。

#### 今後やりたいこと  
必要なファイル編集等だいたいわかってきたし、ソースも実験的に拡張してきてグチャグチャなので、整理してMod製造支援ツールに分岐しようかと。  
イメージとしては、
1. ベースとしたいアイテム選択、使われているファイル(menu,mate,model,tex等)をMod以下の任意のフォルダに指定したファイル名（prefix指定かな？）でコピー
2. テクスチャをpngないしtexで用意しておき、読み込んで色味等調整
3. additem、maskitem、node消去等も画面上でON/OFF切り替えて確認しながら調整
4. shaderも登録されているものから選択できれば。
5. 最終結果を保存

みたいなことができたらな、と。  
機能的にはほぼ網羅しているので、あとはがんばるだけ・・・なはず。


#### 権利とか？
ソースの改変・再配布等は自由にしていただいてかまいません。  
むしろやりたいこと・それを超える機能を実装してくれる人歓迎。

##更新履歴

#### 0.0.4.0
* テクスチャの色の変更処理をマージ
* RenderQueue変更をテスト（保存はされません）。.pmatを生成するようにする予定。

#### 0.0.3.5
* texture変更テスト中

#### 0.0.3.4
* menu書き換え不具合修正

#### 0.0.3.2
* menu/mate出力時、Modelもコピーするように

#### 0.0.3.1
* menu/mate出力時、ファイル名・名前・説明等変更できるように

#### 0.0.3.0
* menu/mate出力に暫定対応

#### 0.0.2.5
* Shadow, Rim, Outline, Shininessも変更可能に

#### 0.0.2.0
* プリセット保存・読み込みできるように

#### 0.0.1.0
* maskItemで非表示になっているアイテムを全部表示できるように
* node消去を部位別に切り替えできるように

#### 0.0.0.2
* マテリアルごとに色変更できるように
* 適用ボタンを押さなくてもスライダー変更でリアルタイムに適用

#### 0.0.0.1
* 初版（実験版）
