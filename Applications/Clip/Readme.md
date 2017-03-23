# CubePDF Clip

Copyright (c) 2017 CubeSoft, Inc.

* License: GNU Affero General Public License version 3 (AGPLv3)
* Mail: support@cube-soft.jp
* Web: http://www.cube-soft.jp/

## はじめに

CubePDF Clip は、PDF ファイルにテキストファイルや画像ファイルなどの
別のファイルを添付するためのソフトウェアです。

CubePDF Clip を使用するためには、Microsoft .NET Framework 3.5 以上がインストール
されている必要があります。.NET Framework 3.5 は、以下の URL からダウンロードして下さい。
http://www.microsoft.com/ja-jp/download/details.aspx?id=22

## 使用方法

まず、添付元の PDF ファイルを「...」ボタンやドラッグ&ドロップを通じて選択します。
次に、選択された PDF ファイルに添付したいファイルを「追加」ボタンまたは
ドラッグ&ドロップを通じてファイルリストに登録します。尚、選択された PDF ファイルに
添付されているファイルが存在する場合、読み込み時にファイルリストに追加されます。
最後に、「保存」ボタンを押すと、選択された PDF ファイルに対して各種ファイルを
添付して上書き保存されます。

PDF ファイルから既に添付されているファイルを削除したい場合は、該当のファイル名を
選択後「削除」ボタンを押して下さい。また、間違って添付ファイルを削除したり
した場合は「リセット」ボタンを押すと、PDF ファイルが読み込まれた直後の添付状況に
リセットされます。

## 注意

PDF ファイルにファイルを添付する方法は、以下の 2 通りが存在します。

1. コンテンツとしてファイルを添付
2. 注釈としてファイルを添付

これらの内、CubePDF Clip で編集可能なのは 1. の「コンテンツとしてファイルを添付」
となります。2. の「注釈としてファイルを添付」および注釈として添付されたファイルを
削除する事はできません。

参考: PDF 文書にファイルを添付する方法
https://helpx.adobe.com/jp/acrobat/kb/4566.html

## ショートカットキー一覧

CubePDF Clip で有効なキーボードのショートカットキーは、以下の通りです。

* Ctrl + D : 選択中のファイルをファイルリストから削除
* Ctrl + H : 「CubePDF Clip について」を表示
* Ctrl + N : 添付ファイルを選択する画面を表示
* Ctrl + O : 添付元となる PDF ファイルを選択する画面を表示
* Ctrl + R : PDF ファイル読み込み直後の添付状況にリセット
* Ctrl + S : 選択された PDF ファイルに対して添付処理を実行して上書き保存

## 利用ライブラリ

CubePDF Clip は、以下のライブラリを利用しています。
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

* 2017/03/24 version 1.0.0
    - 最初の公開バージョン