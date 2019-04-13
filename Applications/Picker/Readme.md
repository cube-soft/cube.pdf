CubePDF ImagePicker
====

Copyright (c) 2015 CubeSoft, Inc.

Copyright © 2010 CubeSoft, Inc.  
GNU Affero General Public License version 3 (AGPLv3)  
support@cube-soft.jp  
https://www.cube-soft.jp/cubepdf/

## はじめに

CubePDF ImagePicker は、PDF ファイルから画像を抽出するためのソフトウェアです。

CubePDF ImagePicker を使用するためには、.NET Framework 3.5 以降がインストールされている必要があります（4.5.2 以降を推奨）。
.NET Framework は、以下の URL からダウンロードして下さい。

* .NET Downloads for Linux, macOS, and Windows  
  https://dotnet.microsoft.com/download

## 使用方法

CubePDF ImagePicker を実行すると、コンピュータ画面の右上にフォーム（メイン画面）が表示されますので、
画像を抽出したい PDF ファイルをドラッグ&ドロップして下さい。
画像の抽出処理が終了すると、「プレビュー」と「全て保存」の 2 つの操作が可能になります。
「全て保存」では、ドラッグ&ドロップされた PDF ファイルから抽出した全ての画像を保存します。
「プレビュー」では、抽出した画像の一覧が表示されますので、必要な画像を選択し「選択項目を保存」ボタンを押して下さい。

### 利用ライブラリ

CubePDF ImagePicker は、以下のライブラリを利用しています。
それぞれのライブラリについては、記載した URL から取得することができます。

* iTextSharp
    - GNU Affero General Public License
    - https://itextpdf.com/
    - https://www.nuget.org/packages/iTextSharp/
* log4net
    - Apache License, Version 2.0
    - https://logging.apache.org/log4net/
    - https://www.nuget.org/packages/log4net/
* AsyncBridge (.NET Framework 3.5)
    - MIT License
    - https://omermor.github.io/AsyncBridge/
    - https://www.nuget.org/packages/AsyncBridge

## バージョン履歴

* 2016/MM/DD version 1.0.1
    - 画像抽出時に透過情報が失われる不都合を修正
* 2015/11/09 version 1.0.0
    - 最初の公開バージョン