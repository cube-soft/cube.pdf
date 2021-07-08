CubePDF Page
====

Copyright © 2013 CubeSoft, Inc.  
GNU Affero General Public License version 3 (AGPLv3)  
support@cube-soft.jp  
https://www.cube-soft.jp/cubepdfpage/

## はじめに

CubePDF Page は、PDF ファイルおよび画像ファイル（BMP, JPEG, PNG, GIF, TIFF) を
結合、または分割するためのソフトウェアです。

CubePDF Page を使用するためには、.NET Framework 3.5 以降がインストールされている必要があります（4.5.2 以降を推奨）。
.NET Framework は、以下の URL からダウンロードして下さい。

* Download .NET Framework  
  https://dotnet.microsoft.com/download/dotnet-framework

## 使用方法

結合、または分割したいファイルを「ファイルを追加」ボタンやドラッグ&ドロップを通じて、
ファイルリストに登録します。その後、「結合」または「分割」ボタンを押すとファイルの
保存場所を指定するダイアログが表示されるので、保存場所を入力します。

PDF ファイルの結合を行う場合、ファイルリストの上から順に結合されます。ファイルリストに
登録されている順序については、登録後、「上へ/下へ」ボタンやマウスのドラッグ操作で
変更する事ができます。

PDF ファイルを分割については、CubePDF Page は登録された PDF ファイルの全ページを
1 ページずつ別ファイルとして分割する、と言う処理を行います。ある特定のページ群のみを
抽出したい場合は、いったん全ページを分割した後に必要なページのみを再結合する、と言う
操作を行って下さい。

## ショートカットキー一覧

CubePDF Page で有効なキーボードのショートカットキーは、以下の通りです。

* Ctrl + A : 全て選択
* Ctrl + D : 選択中のファイルをファイルリストから削除（Ctrl + Shift + D ですべて削除）
* Ctrl + H : 「CubePDF Page について」を表示
* Ctrl + M : 結合操作を実行
* Ctrl + O : ファイルをファイルリストに追加
* Ctrl + R : 選択中のファイルを関連付けられているアプリケーションで開く（プレビュー）
* Ctrl + S : 分割操作を実行
* Ctrl + 下矢印 : 選択中のファイルを下に移動
* Ctrl + 上矢印 : 選択中のファイルを上に移動

## 利用ライブラリ

CubePDF Page は、以下のライブラリを利用しています。
それぞれのライブラリについては、記載した URL から取得することができます。

* iText7 (net45) or iTextSharp (net35)
    - GNU Affero General Public License
    - https://itextpdf.com/
    - https://www.nuget.org/packages/itext7/
    - https://www.nuget.org/packages/iTextSharp/
* NLog
    - 3-clause BSD License
    - https://nlog-project.org/
    - https://www.nuget.org/packages/NLog/
* AsyncBridge (net35)
    - MIT License
    - https://omermor.github.io/AsyncBridge/
    - https://www.nuget.org/packages/AsyncBridge.Net35/

## バージョン履歴

* 2021-07-09 version 3.5.0
    - iText7 への移行を含む内部処理の修正
* 2021-05-21 version 3.1.1
    - 複数項目を選択して上へ・下へボタンを押した時の不都合を修正
    - iTextSharp 5.5.13.2 に更新
* 2021-01-22 version 3.1.0
    - 複数ファイルおよびフォルダー選択時の追加順序を修正
    - プログラム引数に追加ファイルを指定できるように修正
* 2020-11-13 version 3.0.0
    - 結合・分割処理の実装を CubePDF Utility に追随
    - 処理完了後のメッセージを削除
    - ログ出力用ライブラリを log4net から NLog に変更
* 2016-04-05 version 2.0.1
    - しおり（ブックマーク）の位置がずれる不都合を修正
    - 上書き保存が失敗する不都合を修正
    - マウスのドラッグ&ドロップによる項目の移動を実装
* 2015-12-28 version 2.0.0
    - 画像ファイルの結合に対応
    - 結合・分割処理の実装を CubePDF Utility と統一するように修正
    - GUI の修正
* 2013-02-25 version 1.0.0
    - 最初の公開バージョン