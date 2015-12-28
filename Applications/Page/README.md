# CubePDF Page

Copyright (c) 2013 CubeSoft, Inc.

* 開発・配布：株式会社キューブ・ソフト
* License: GNU Affero General Public License version 3 (AGPLv3)
* Mail: support@cube-soft.jp
* Web: http://www.cube-soft.jp/cubepdf/

## What's this

CubePDF Page は、PDF ファイルおよび画像ファイルを結合、または分割するためのソフトウェアです。

CubePDF Page を使用するためには、Microsoft .NetFramework 3.5 がインストールされている必要があります。
Microsoft .NetFramework 3.5 は、以下の URL からダウンロードして下さい。
http://www.microsoft.com/ja-jp/download/details.aspx?id=22

## 使用方法

結合、または分割したいファイルを「ファイルを追加」ボタンやドラッグ&ドロップを通じて、ファイルリストに登録します。
その後、「結合」または「分割」ボタンを押すとファイルの保存場所を指定するダイアログが表示されるので、保存場所を入力します。

PDF ファイルの結合を行う場合、ファイルリストの上から順に結合されます。ファイルリストに登録されている順序については、
登録後、「上へ/下へ」ボタンやマウスのドラッグ操作で変更する事ができます。

PDF ファイルを分割については、CubePDF Page は登録された PDF ファイルの全ページを 1 ページずつ別ファイルとして分割する、
と言う処理を行います。ある特定のページ群のみを抽出したい場合は、いったん全ページを分割した後に必要なページのみを再結合する、
と言う操作を行って下さい。

## 使用ライブラリ

CubePDF Page は、以下のライブラリを利用しています。
それぞれのライブラリについては、記載した URL から取得することができます。

* iTextSharp
  URL: http://sourceforge.net/projects/itextsharp/
  GNU Affero General Public License ( http://www.gnu.org/licenses/agpl.html )

## バージョン履歴

* 2015/12/28 version 2.0.0
 - 画像ファイルの結合に対応
 - 結合・分割処理の実装を CubePDF Utility と統一するように修正
 - GUI の修正
 
* 2013/02/25 version 1.0.0
 - 最初の公開バージョン
