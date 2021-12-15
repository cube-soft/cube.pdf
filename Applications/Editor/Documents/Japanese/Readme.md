CubePDF Utility ユーザーマニュアル
====

Copyright © 2013 CubeSoft, Inc.  
support@cube-soft.jp  
https://www.cube-soft.jp/cubepdfutility/

## 基本的な使い方

CubPDF Utility は、既存の PDF ファイルに対してページ挿入や削除、文書プロパティ、各種セキュリティ機能などの設定を変更するためのソフトウェアです。

![CubePDF Utility メイン画面](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdfutility/overview.ja.png)

CubPDF Utility を使用するためにはまず、編集したい PDF ファイルを開きます。PDF ファイルを開く方法は、下記の 3 通りが用意されています。

1. メイン画面中央または **ファイル** メニューに表示される **最近開いたファイル** から選択
2. **開く** ボタンをクリックして表示されるダイアログから選択
3. メイン画面へ編集したい PDF ファイルをドラッグ&ドロップ

選択した PDF ファイルに対して既にセキュリティ機能が設定されている場合、管理用パスワードを入力するためのダイアログボックスが表示されますので、正しいパスワードを入力して下さい。

![パスワード入力画面](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdfutility/doc/v1/ja/open-02.png)

PDF ファイルを開き、必要な操作を終えたら、最後に **保存（上書き保存）** または **名前を付けて保存** ボタンを押して PDF への編集は完了です。

![保存](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdfutility/doc/v1/ja/save.png)

### PDF ファイルのパスワードについて

PDF ファイルには、閲覧および事前に許可された操作のみが可能となる **閲覧用パスワード** と、全ての操作を実行する事ができる **管理用パスワード** の 2 種類が存在します。

CubePDF Utility は PDF ファイルを編集すると言う性質上、開く際には常に **管理用パスワード** を要求します。そのため、**閲覧用パスワード** の入力では該当 PDF ファイルを開く事ができませんのでご注意下さい。また、パスワードを入力せずに開く事ができる PDF ファイルであっても、**管理用パスワード** のみが設定されている事もあります。該当 PDF ファイルを編集する場合、PDF ファイルの作成者に **管理用パスワード** を尋ねる等の対応をお願いします。

## 編集操作の一覧

### 挿入

![挿入](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdfutility/doc/v1/ja/insert-01.png)

CubePDF Utility では、PDF ファイルに対して、下記のファイルを挿入する事ができます。

* PDF ファイル
* ビットマップ画像ファイル (PNG, JPEG, BMP, TIFF)

ファイルを挿入するには、上部メニューまたはコンテキストメニューに表示される下記 4 種類のメニューを用いて挿入位置を決定して下さい。

* **挿入** または **選択位置の後に挿入**
* **先頭に挿入**
* **末尾に挿入**
* **詳細を設定して挿入**

**詳細を設定して挿入** を選択した場合、挿入位置や挿入するファイルを詳細に設定するための下記専用ダイアログが表示されます。それ以外のメニューを選択した場合、ファイルを選択するためのダイアログが表示されます。

![詳細を設定して挿入](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdfutility/doc/v1/ja/insert-02.png)

この他、CubePDF Utility では PDF ファイルのページ単位で挿入する事もできます。ページ単位で挿入するには、まず挿入元と挿入先、2 つの PDF ファイルを両方とも CubePDF Utility で開きます。そして、挿入したいページのサムネイル画像をドラッグ&ドロップする事で挿入操作が完了します。

![ドラッグ&ドロップによる挿入](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdfutility/doc/v1/ja/insert-03.png)

### 削除

![削除](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdfutility/doc/v1/ja/remove-01.png)

**削除** メニューでは、開いた PDF ファイルの一部のページを削除する事ができます。**選択ページを削除**（**削除** も同様）メニューを選択した場合、メイン画面上で現在選択状態となっているページを削除します。また、**詳細を設定して削除** メニューを選択した場合、下記の専用ダイアログが表示されます。

![詳細を設定して削除](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdfutility/doc/v1/ja/remove-02.png)

**対象ページ** には削除するページ範囲を記載します。使用可能な文字は、数字、","（コンマ）、および "-"（ハイフン）となります（例. 1,2,4,7-9）。

### 抽出

![抽出](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdfutility/doc/v1/ja/extract-01.png)

CubePDF Utility では、PDF ファイル中の任意のページを抽出して、下記のファイル形式で保存する事ができます。

* PDF
* PNG（ビットマップ画像ファイル）

ただし、**選択ページを抽出** （**抽出** も同様）メニューを選択した場合、保存形式は PDF となります。PNG 形式で抽出する場合、**詳細を設定して抽出** メニューを選択後に表示される下記専用ダイアログで必要な設定を行って下さい。

![詳細を設定して抽出](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdfutility/doc/v1/ja/extract-02.png)

詳細ダイアログの **出力ファイル** には、抽出した結果を保存するパスを設定します。**ファイルタイプ** は、**PDF** と **PNG** の 2 種類が選択できるので、必要な形式を選択します。PNG を設定した場合、DPI を設定する事ができます。この値は、PDF ファイルの内容をビットマップ形式に変換する際のサイズ（幅、および高さ）の決定に用います。例えば、A4 サイズの PDF ファイルを変換する場合、DPI の設定値に応じたサイズは下記のようになります。

*  72dpi ...  595 ×  842
*  96dpi ...  793 x 1122
* 144dpi ... 1190 x 1684
* 300dpi ... 2479 x 3508

**対象ページ** は、**選択ページ**、**全てのページ**、**範囲を指定** の中から選択して下さい。**範囲を指定** した場合、下部のテキストボックスに抽出するページ範囲を記載します。使用可能な文字は、数字、","（コンマ）、および "-"（ハイフン）となります（例. 1,2,4,7-9）。

**1 ページ毎に個別のファイルとして保存** オプションを有効にした場合、例えば 10 ページの PDF ファイルを抽出すると 10 個の PDF ファイルが生成されます。この際、生成される PDF ファイルのファイル名は、**出力ファイル** で指定したものに数字を付与したものとなります。例えば、Sample.pdf と言うファイル名を指定した場合、実際に生成される PDF ファイルは Sample-01.pdf、Sample-02.pdf、のようになります。尚、ファイルタイプとして PNG を指定した場合も同様の動作となります。

### ページ順序の変更

![ページ順序の変更](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdfutility/doc/v1/ja/reorder.png)

**前へ** および **後へ** メニューは、メイン画面上で現在選択状態となっているページの順序をそれぞれ1 ページだけ、前、または後ろに移動させます。ページ順序はドラッグ&ドロップ操作によっても変更する事ができます。変更したいページを選択した状態で、それらのページを変更位置までドラッグ&ドロップ操作で移動させます。

### 回転

![回転](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdfutility/doc/v1/ja/rotate.png)

**左90度** および **右90度** メニューは、メイン画面上で現在選択状態となっているページをそれぞれ左方向、または右方向に 90 度だけ回転させます。

### 文書プロパティ

![文書プロパティ](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdfutility/doc/v1/ja/metadata.png)

**文書プロパティ** メニューでは、タイトルや作成者などの情報を登録する事ができます。ここで登録した情報は、Adobe Acrobat Reader DC などの PDF 閲覧ソフトのプロパティ画面で閲覧する事ができます。また、**ページレイアウト** の項目は、PDF 閲覧ソフトで開いた時の表示方法を変更する事ができます。

### セキュリティ

![セキュリティ](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdfutility/doc/v1/ja/encryption.png)

**セキュリティ** メニューでは、PDF ファイルにパスワード等を設定する事ができます。パスワードを設定する場合は、まず **PDF ファイルをパスワードで保護する** の項目を有効にし、**管理用パスワード** とパスワード確認の項目に同じパスワードを 2 回 入力してください。暗号化方式は、現在 **40bit RC4**、**128bit RC4**、**128bit AES**、**256bit AES** の 4 種類に対応しています。

次に、**操作** の項目で、ユーザに許可・制限する操作を指定します。設定可能な項目は以下の通りです。

* PDF ファイルを開く時にパスワードを要求する
* 印刷を許可する
* テキストや画像のコピーを許可する
* ページの挿入、回転、削除を許可する
* アクセシビリティのための内容の抽出を許可する
* フォームへの入力を許可する
* 注釈の追加、編集を許可する

尚、**PDF ファイルを開く時にパスワードを要求する** の項目を有効にする際、**管理用パスワードと共用する** の項目も有効にすると、CubePDF Utility は、閲覧用パスワードに管理用パスワードと同じものを設定します。

ただし、**管理用パスワードと閲覧用パスワードを共有した場合、PDF 閲覧ソフトによっては、印刷やコピー操作等の制限が正常に機能しない事があります**。これは、PDF 閲覧ソフトが管理用パスワードで PDF ファイルを開いたと認識するためと予想されます。そのため、CubePDF Utilityでは、管理用パスワードと共用した場合、その他の操作に関する許可設定を受け付けないように設計しています。

また、管理用パスワードで PDF ファイルを開いたと認識された場合、閲覧用パスワードの除去も含めた全ての PDF 編集が可能になります。閲覧用パスワードを管理用パスワードと共用する場合、これらの動作を十分に理解した上でご利用下さい。

## その他の設定

### メイン画面の表示に関する設定

![表示](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdfutility/doc/v1/ja/view.png)

メイン画面の **その他** タブでは、CubePDF Utility の表示方法に関する設定を変更できます。**拡大**、**縮小**、およびピクセル数の選択ボックスでは、サムネイル画像の表示サイズを変更できます。尚、変更直後は、変更前のサムネイル画像がそのまま利用されているため、表示がぼやける事があります。その際は、**更新** ボタンを押して下さい。

**枠線のみ** ボタンは、サムネイル画像は非表示となり、その代わりにページ数分だけの枠線が表示されます。この機能は、主にメモリ消費量の抑制を目的として利用されます。

### バージョン情報

![バージョン情報](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdfutility/doc/v1/ja/version.png)

メイン画面の **その他** タブにある **設定** ボタンを押すと、バージョン情報および関連項目の設定画面が表示されます。**バージョン情報** では、CubePDF Utility のバージョン情報が表示されます。その下にある **起動時にアップデートを確認する** の項目を有効にすると、CubePDF Utility のバージョンアップ時にパソコンの右下に通知されます。アップデートの確認は、パソコンの起動時に実行されます。また、アップデートの確認のために送信する情報は、CubePDF, Windows, .NET Framework それぞれのバージョン番号です。

**表示言語** では、CubePDF Utility メイン画面のメニュー等の表示言語を設定する事ができます。対応言語は英語と日本語の 2 種類です。また、表示言語で自動を選択した場合、Windows の言語設定に応じてどちらかの言語が自動的に選択されます。

## ショートカットキーの一覧

CubePDF Utility で有効なキーボードのショートカットキーは、以下の通りです。

* Ctrl + O ... PDF ファイルを開く
* Ctrl + S ... 上書き保存
* Ctrl + Shit + S ... 名前を付けて保存
* Ctrl + W ... PDF ファイルを閉じる
* Ctrl + Q ... アプリケーションを終了
* Ctrl + I or Insert ... 選択位置に PDF, PNG, JPEG, BMP ファイルを挿入
* Ctrl + Shift +I ... 詳細を設定して挿入
* Ctrl + D or Delete ... 選択ページを削除
* Ctrl + Shift + D ... 範囲を指定して削除
* Ctrl + E ... 選択ページを抽出
* Ctrl + Shift + E ... 詳細を設定して抽出
* Ctrl + B ... 選択ページを 1 ページ分前に移動
* Ctrl + F ... 選択ページを 1 ページ分後ろに移動
* Ctrl + L ... 選択ページを左 90 度回転
* Ctrl + R ... 選択ページを右 90 度回転
* Ctrl + M ... 文書プロパティ編集画面を表示
* Ctrl + K ... セキュリティ編集画面を表示
* Ctrl + A ... 全て選択
* Ctrl + Z ... 元に戻す
* Ctrl + Y ... やり直し
* Ctrl + + ... サムネイル画像サイズを拡大
* Ctrl + - ... サムネイル画像サイズを縮小

## CubePDF Utility のアンインストール

CubePDF Utility をアンインストールするには、まず、設定のアプリと機能（Windows 8 以降）または、コントロールパネルのプログラムのアンインストールを選択します。そして、表示される画面で CubePDF Utility のアイコンを選択してアンインストールの項目を実行して下さい。

![アンインストール（設定）](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdfutility/doc/v1/ja/uninstall-01.png)
![アンインストール（コントロールパネル）](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdfutility/doc/v1/ja/uninstall-02.png)

## CubePDF Utility で問題が発生した場合

CubePDF Utility は、```C:\Users\%UserName%\AppData\Local\CubeSoft\CubePdfUtility2\Log``` フォルダに実行ログを出力しています。問題が発生した時は、これらのログを添付して support@cube-soft.jp までご連絡お願いします（%UserName% の箇所は、ログオン中のユーザ名に置き換えて下さい）。