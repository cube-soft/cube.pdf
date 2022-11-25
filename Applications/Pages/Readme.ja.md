CubePDF Page
====

Copyright © 2013 CubeSoft, Inc.  
GNU Affero General Public License version 3 (AGPLv3)  
support@cube-soft.jp  
https://www.cube-soft.jp/cubepdfpage/

## はじめに

CubePDF Page は、PDF ファイルおよび画像ファイル（BMP, JPEG, PNG, GIF, TIFF) を結合・分割するためのソフトウェアです。

CubePDF Page を使用するためには、.NET Framework 3.5 以降がインストールされている必要があります（4.7 以降を推奨）。
.NET Framework は、以下の URL からダウンロードして下さい。

* Download .NET Framework  
  https://dotnet.microsoft.com/download/dotnet-framework

## 使用方法

結合、または分割したいファイルを追加ボタンやドラッグ&ドロップを通じて、ファイル一覧に登録します。
その後、結合または分割ボタンを押すとファイルの保存場所を指定するダイアログが表示されるので、保存場所を入力して下さい。

PDF ファイルの結合を行う場合、ファイル一覧の上から順に結合されます。
ファイル一覧での表示順序に関しては、ファイルの追加後に上へ/下へボタンで変更する事ができます。

PDF ファイルの分割では、追加された PDF ファイルの全ページを 1 ページずつ別ファイルとして分割して保存します。
ある特定のページ群のみを抽出したい場合、いったん全ページを分割した後に必要なページのみを再結合して下さい。

## ショートカットキー一覧

CubePDF Page で有効なキーボードのショートカットキーは、以下の通りです。

* Ctrl + M ... ファイルの結合を実行
* Ctrl + S ... ファイルの分割を実行
* Ctrl + E ... 文書プロパティおよびセキュリティ編集画面を表示
* Ctrl + H ... CubePDF Page の設定画面を表示
* Ctrl + Q ... アプリケーションを終了
* Ctrl + O ... PDF, PNG, JPEG, BMP ファイルを追加するダイアログを表示
* Ctrl + A ... ファイル一覧の全ての項目を選択
* Ctrl + R ... 選択中のファイルを関連付けられているアプリケーションで開く
* Ctrl + K or Ctrl + 上矢印 ... 選択ファイルを 1 つ上に移動
* Ctrl + J or Ctrl + 下矢印 ... 選択ファイルを 1 つ下に移動
* Ctrl + D or Delete ... 選択ファイルを一覧から削除
* Ctrl + Shift + D ... 全てのファイルを一覧から削除

## 問題が発生した場合

CubePDF Page は、以下のフォルダに実行ログを出力しています。  
```C:\Users\%UserName\AppData\Local\CubeSoft\CubePdfPage\Log```  
問題が発生した時は、これらのログを添付して support@cube-soft.jp までご連絡お願いいたします。
※ %UserName% は、ログイン中のユーザ名に置換して下さい。  

## 利用ライブラリ

CubePDF Page は、以下のライブラリを利用しています。
それぞれのライブラリについては、記載した URL から取得することができます。

* iText7 (net47) or iTextSharp (net35)
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

* 2022-12-01 version 4.1.0
    - iText7 を 7.2.4 に更新
* 2022-07-11 version 4.0.3
    - iText7 を 7.2.3 に更新
* 2022-04-18 version 4.0.2
    - iText7 を 7.2.2 に更新
* 2022-03-30 version 4.0.1
    - 全角数字の含まれるファイルを指定した時にエラーとなる不具合を修正
* 2022-03-25 version 4.0.0
    - 推奨動作環境を .NET Framework 4.7 以降に変更
    - iText7 を 7.2.1 に更新
    - 文書プロパティおよびセキュリティ設定のための画面を新設
    - 重複リソースの削除に関する設定を追加
    - 結合元 PDF ファイルのしおり情報に関する設定を追加
    - 作業フォルダーに関する設定を追加
    - メイン画面の表示言語に関する設定を追加
    - ファイル追加時、ファイル名に数字が含まれる場合の並び順を変更
* 2022-01-07 version 3.6.1
    - 画像ファイル結合時にファイルサイズが必要以上に増大する不具合を修正
    - iText7 を 7.1.17 に更新
* 2021-10-26 version 3.6.0
    - PDF 結合時にしおりの階層構造が崩れる不具合を修正
    - 更新通知機能を改善
* 2021-07-09 version 3.5.0
    - iText7 への移行を含む内部処理の修正
* 2021-05-21 version 3.1.1
    - 複数項目を選択して上へ・下へボタンを押した時の不具合を修正
    - iTextSharp 5.5.13.2 に更新
* 2021-01-22 version 3.1.0
    - 複数ファイルおよびフォルダー選択時の追加順序を修正
    - プログラム引数に追加ファイルを指定できるように修正
* 2020-11-13 version 3.0.0
    - 結合・分割処理の実装を CubePDF Utility に追随
    - 処理完了後のメッセージを削除
    - ログ出力用ライブラリを log4net から NLog に変更
* 2016-04-05 version 2.0.1
    - しおり（ブックマーク）の位置がずれる不具合を修正
    - 上書き保存が失敗する不具合を修正
    - マウスのドラッグ&ドロップによる項目の移動を実装
* 2015-12-28 version 2.0.0
    - 画像ファイルの結合に対応
    - 結合・分割処理の実装を CubePDF Utility と統一するように修正
    - GUI の修正
* 2013-02-25 version 1.0.0
    - 最初の公開バージョン