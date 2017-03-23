# CubePDF Page

Copyright (c) 2013 CubeSoft, Inc.

* License: GNU Affero General Public License version 3 (AGPLv3)
* Mail: support@cube-soft.jp
* Web: http://www.cube-soft.jp/cubepdfpage/

## はじめに

CubePDF Page は、PDF ファイルおよび画像ファイル（BMP, JPEG, PNG, GIF, TIFF) を
結合、または分割するためのソフトウェアです。

CubePDF Page を使用するためには、Microsoft .NET Framework 3.5 以上がインストール
されている必要があります。.NET Framework 3.5 は、以下の URL からダウンロードして下さい。
http://www.microsoft.com/ja-jp/download/details.aspx?id=22

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
* Ctrl + J : 選択中のファイルを下に移動（Ctrl + 下矢印も同様）
* Ctrl + K : 選択中のファイルを上に移動（Ctrl + 上矢印も同様）
* Ctrl + M : 結合操作を実行
* Ctrl + O : ファイルをファイルリストに追加
* Ctrl + R : 選択中のファイルを関連付けられているアプリケーションで開く（プレビュー）
* Ctrl + S : 分割操作を実行

## 利用ライブラリ

CubePDF Page は、以下のライブラリを利用しています。
それぞれのライブラリについては、記載した URL から取得することができます。

* iTextSharp
 - GNU Affero General Public License
 - http://itextpdf.com/
 - https://www.nuget.org/packages/iTextSharp/

* log4net
 - Apache License, Version 2.0
 - http://logging.apache.org/log4net/
 - https://www.nuget.org/packages/log4net/

* AsyncBridge
 - MIT License
 - http://omermor.github.io/AsyncBridge/
 - https://www.nuget.org/packages/AsyncBridge.Net35

## バージョン履歴

* 2016/04/05 version 2.0.1
 - しおり（ブックマーク）の位置がずれる不都合を修正
 - 上書き保存が失敗する不都合を修正
 - マウスのドラッグ&ドロップによる項目の移動を実装
 
* 2015/12/28 version 2.0.0
 - 画像ファイルの結合に対応
 - 結合・分割処理の実装を CubePDF Utility と統一するように修正
 - GUI の修正
 
* 2013/02/25 version 1.0.0
 - 最初の公開バージョン
