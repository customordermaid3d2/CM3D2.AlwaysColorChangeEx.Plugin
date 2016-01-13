# CM3D2.AlwaysColorChange.Plugin

パーツごとに好きに色変更できるようにするプラグインです。  
まだまだ不具合が残っている可能性があります。

オリジナルの[kf-cm3d2](https://github.com/kf-cm3d2)さんとは別人による勝手な改造版です。

---
## ■ 導入方法
#### 前提条件  
UnityInjectorが導入済みであること。

#### インストール  

[Release](./Release)ページから、対応するバージョンのzipファイルをダウンロードし、
解凍後、UnityInjectorフォルダ以下をプラグインフォルダにコピーしてください。


#### 自分でコンパイルする場合  
compile.batをサンプルで用意しました。  
レジストリが正しく設定されていればそのままコンパイルできます。  
もし、レジストリの状態と異なるCM3D2ディレクトリを使いたい場合は、
cmpile.batファイルの以下のパスを設定して、バッチを実行してください。
* CM3D2_DIR=C:\KISS\CM3D2  

## ■ 機能概要
#### 対象シーン
 * エディット
 * 夜伽
 * ダンス
 * ADVパート  

で動作します。

##### 機能説明
* マスククリア  
  maskItemで消去されているアイテムを表示します。
   マスクアイテム選択画面で、各スロットのマスク可否を指定してください。  
   適用ボタンで反映。

  表示後は、個別にα値変更で不要なものを消してください。
* ノード表示切替え  
  ノード（身体の部位）の表示・非表示を切り替えられます。  
  適用ボタンで反映。
* 保存  
  現在の衣装・設定値をプリセットとして保存します。  
  プリセット適用で呼び出すことがます。  
  - マスククリアを有効にする  
    プリセット適用時、マスククリアを適用するようにします。（未実装）
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
  pngはファイル選択ダイアログで選択できる範囲なら適用可能です。

## ■ 使用方法
##### メニュー操作
F12キーでメニューが開きます。  
キーを変更したい場合は、
* Config/AlwaysColroChange.ini

のToggleWindowキーで指定してください。  
[iniファイル](./#iniファイル)の項目を参照

![GUI](http://i.imgur.com/sMm5lo9.jpg  "GUI")

パーツを選択し、RGBAをスライダーで変更することで色が変更されます。  
A(Alpha)は透過可能なシェーダでのみ変更可能です。


##### ◆ マスクアイテム選択画面
![Imgur](http://i.imgur.com/AAxzyMv.png,"マスクアイテム選択画面")

本画面では、チェックが入っている=マスク対象(アイテム非表示)です。
###### ○各ボタンの説明
  * 同期：現在の表示状態に、チェックボタンを合わせる  
  * すべてON：チェックボタンをすべてオン  
  * すべてOFF：チェックボタンをすべてオフ  
  * 適用：チェックボタンに合わせてマスクを適用  
  * クリア：すべてのマスクをクリアする
  * 戻す　：元のマスク状態に戻す  

###### ○アイテムの表示状態
  * 表示中：マスクされていない表示状態
  * 非表示：下着モードやヌードモード等、モードによって非表示化されている状態
  * マスク：マスクされ、非表示の状態
  * 未読込：アイテムが装着されていないなど、対象スロットが読込まれていない状態

##### ◆ 表示ノード選択画面
![Imgur](http://i.imgur.com/9kF6zs4.png, "表示ノード選択画面")

本画面では、チェックが入っている=表示対象(アイテム表示)です。
###### ○各ボタンの説明
  * 同期：現在の表示状態に、チェックボタンを合わせる  
  　　　　表示アイテムの状態も同期します。
  * すべてON：チェックボタンをすべてオン  
  * すべてOFF：チェックボタンをすべてオフ  
  * 適用：チェックボタンに合わせて表示ノードの状態を変更

###### ○アイテムの表示状態
  * 表示中：ノードが表示されている状態
  * 非表示：ノードが非表示の状態  
※ 本画面ではノードの表示状態はリアルタイムには変化しません。  
　 最新の状態に更新する場合は「同期」ボタンを押下してください。

##### ◆ menu/mate保存の仕様について
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
  - αは元々透過対応シェーダーのものしか効かない  
  - .modには非対応  
  他にも不具合あるかもしれません。

##### Config/AlwaysColorChange.ini
iniファイルの項目を以下に示す。  
ただし、すべて[Config]セクションとする。

| キー                 |型      |デフォルト | 説明            
|----------------------|-------|:---------:|:----------------
|ToggleWindow          |KeyCode| F12       | メニュー表示キー
|PresetPath            |string | --        | プリセット保存先パス
|SliderShininessMax    | float |  20       | Shininessスライダの最大値
|SliderShininessMin    | float |   0       | Shininessスライダの最小値
|SliderOutlineWidthMax | float |   0.1     | OutlineWidthスライダの最大値
|SliderOutlineWidthMin | float |   0       | OutlineWidthスライダの最小値
|SliderRimPowerMax     | float | 100       | RimPowerスライダの最大値
|SliderRimPowerMin     | float |   0       | RimPowerスライダの最小値
|SliderRimShiftMax     | float |   5       | RimShiftスライダの最大値
|SliderRimShiftMin     | float |  -5       | RimShiftスライダの最小値
|SliderHiRateMax       | float |   1       | HiRateスライダの最大値
|SliderHiRateMin       | float |   0       | HiRateスライダの最小値
|SliderHiPowMax        | float |  50       | HiPowスライダの最大値
|SliderHiPowMin        | float |   0.001   | HiPowスライダの最小値
|SliderFloatVal1Max    | float | 300       | FloatValue1スライダの最大値
|SliderFloatVal1Min    | float |   0       | FloatValue1スライダの最小値
|SliderFloatVal2Max    | float |  15       | FloatValue2スライダの最大値
|SliderFloatVal2Min    | float | -15       | FloatValue2スライダの最小値
|SliderFloatVal3Max    | float |   1       | FloatValue3スライダの最大値
|SliderFloatVal3Min    | float |   0       | FloatValue3スライダの最小値


## ■ 権利
ソースの改変・再配布等は自由にしていただいてかまいません。  
むしろやりたいこと・それを超える機能を実装してくれる人歓迎。
(オリジナル原文）


#### □ TODO （やるかも）
 * menu/mate保存機能の拡張  
  * シェーダ変更を行っているアイテムの場合  
  mate・modelファイルのシェーダを書き換えたファイルを出力
  * テクスチャ変更を行っているアイテムの場合  
  対応するtexファイル出力
  * pmat出力
 * 表示ノード画面
  ノードの構造を見やすく表示。グループごとに選択可能とする等々
 * UNDO機能追加
 * コード修正
  クラス構造整理（ビュー、モデルの分離等）リファクタ
  NGUI化は…無理かも？
 * 複数メイド/男への対応
 * 保存プリセットの管理
  名前を指定して削除等

## ■ 更新履歴
#### 0.0.5.1
* 機能拡張 (**Enhance**)
  * テクスチャ変更画面から、調整されたテクスチャのpng保存に対応(暫定版)
    (出力先は MOD/ACC/Export/以下。ファイル名はオリジナルをベースに時刻付与)
* その他(**Misc**)
  * シーン切替え時のデータ削除をなくし、  
    代わりにメイド変更時にデータ削除するよう修正
  * コード修正 (model, mateのシェーダ変更準備等)
  * README修正

#### 0.0.5.0
* 新規機能 (**New**)
  * マスクアイテム選択画面
* 機能拡張(**Enhance**)
  * ノード表示切り替え画面
    * 「すべてOFF」「同期」ボタン追加
    * 現在の状態を表示
  * テクスチャ変更画面
    * ToonRampとShadowRateToonで標準texが選択可能になった
  * preset保存
    * FloatValue1,2,3に対応
* 不具合修正(**Fixed**)
  * カラーチェンジ画面
    * 一部アイテムのカラーチェンジが出来ない問題を修正  
　  ( 前髪, 耳, リボン, 手持ちアイテム )
  * iniファイル
    * iniファイル未指定時のデフォルト値のミス修正
  * テクスチャ変更画面
    * シェーダ変更のテクスチャ数増加時に例外が発生していた問題を修正
    * 表示アイテムが多数あった場合にスクロールバーが表示されない問題を修正
    * RenderTexを削除
* その他(**Misc**)
  * 表示アイテム間のマージン値を調整（縮小）
  * 一部文字列を色付け・文字サイズ調整
  * iniファイルのデフォルト値変更(FloatVal2を-15~15に変更)
* 制限事項(**Limited**)
  * プリセット保存では、マスク状態の保存未対応

#### 0.0.4.6
* 強制カラーチェンジ画面で、各マテリアルのシェーダを選択可能に変更  
　　(代わりに、アルファ値を1未満にするとシェーダを自動で置換える機能を除去)
 * カラーチェンジの項目にfloatValue1,2,3追加  
   * iniファイルにスライダー範囲キーを追加
 * RenderTexを追加
 * RimColorとOutlineColorのalpha値を非表示に
 * 透過シェーダのみアルファ値スライダを表示するように変更
* UIのサイズ微修正
* 各種コード修正

#### 0.0.4.5
* 「テクスチャ変更」画面の処理軽量化
* 各種コード修正

#### 0.0.4.4
* 不具合修正
  * テクスチャ変更画面で、modelの異なる衣装に変更すると発生するエラーを修正
  * テクスチャ変更で色変更が出来なくなっていた問題を修正  
  (0.0.4.3で混入した不具合)
* スライダーの範囲をiniで指定可能にした
* エラーダイアログを一部追加 (modファイルのmate変更)
*  プリセット適用処理を変更  
 （既存は同一マテリアル名で置換していたが、マテリアル番号による判定に変更）

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
 * 字下げスタイルを勝手に変更
  等

↑勝手に改造版

---
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
