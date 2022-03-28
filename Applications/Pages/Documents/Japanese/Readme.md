CubePDF Page ユーザーマニュアル
====

Copyright © 2013 CubeSoft, Inc.  
GNU Affero General Public License version 3 (AGPLv3)  
support@cube-soft.jp  
https://www.cube-soft.jp/cubepdfpage/

## 基本的な使い方

![CubePDF Page メイン画面](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdfpage/overview.ja.png)

CubPDF Page は、既存の PDF ファイルや画像ファイルを結合、または分割するためのソフトウェアです。結合・分割後の PDF ファイルに対して文書プロパティや各種セキュリティ機能などの設定を追加する事もできます。CubPDF Page を使用するためにはまず、結合・分割したい PDF ファイルや画像ファイルをメイン画面のファイル一覧に追加します。PDF ファイルを追加する方法は、下記の 2 通りが用意されています。

1. **開く** ボタンをクリックして表示されるダイアログから選択
2. メイン画面のファイル一覧へ結合・分割したい PDF ファイルまたは画像ファイルをドラッグ&ドロップ

選択した PDF ファイルに対して既にセキュリティ機能が設定されている場合、管理用パスワードを入力するためのダイアログボックスが表示されますので、正しいパスワードを入力して下さい。

![パスワード入力画面](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdfpage/doc/v2/ja/password.png)

PDF ファイル等を追加し、必要な操作を終えたら、最後に下記のどちらかのボタンをクリックして完了です。

* **結合**  
  複数の PDF ファイルを 1 つの PDF ファイルに結合します。PDF ファイルに加えて、PNG、JPEG、BMP、TIFF 形式の画像ファイルも併せて 1 つの PDF ファイルとして結合してする事ができます。
* **分割**  
  複数ページからなる PDF ファイルを 1ページ毎の PDF ファイルに分割します。

### PDF ファイルのパスワードについて

PDF ファイルには、閲覧および事前に許可された操作のみが可能となる **閲覧用パスワード** と、全ての操作を実行する事ができる **管理用パスワード** の 2 種類が存在します。

CubePDF Page は PDF ファイルを編集すると言う性質上、開く際には常に **管理用パスワード** を要求します。そのため、**閲覧用パスワード** の入力では該当 PDF ファイルを開く事ができませんのでご注意下さい。また、パスワードを入力せずに開く事ができる PDF ファイルであっても、**管理用パスワード** のみが設定されている事もあります。該当 PDF ファイルを編集する場合、PDF ファイルの作成者に **管理用パスワード** を尋ねる等の対応をお願いします。

### PDF ファイルのページ単位での編集

CubePDF Page ではページ順序の入れ替え、特定ページの抽出や削除と言ったページ単位での編集を直接実行する方法はありませんが、結合・分割機能の併用によって、間接的に実現する事ができます。尚、既存の PDF ファイルに対して詳細な操作を行いたい場合、[CubePDF Utility](https://www.cube-soft.jp/cubepdfutility/) の利用もご検討下さい。

## 編集操作の一覧

### メイン画面

![CubePDF Page メイン画面](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdfpage/doc/v2/ja/main.png)

CubePDF Page メイン画面に表示されているボタン等の機能は下記の通りです。

* **ファイル一覧**  
  メイン画面の左側には、結合または分割対象となるファイル一覧が表示されます。PDF ファイルまたは画像ファイル (BMP, PNG, JPEG, GIF, TIFF) を登録する事ができます。表示されているファイル名をダブルクリックすると、該当ファイルが関連付けられているアプリケーションで開きます。
* **追加**  
  ファイル一覧に新しい PDF ファイルまたは画像ファイルを追加するためのダイアログを表示します。
* **上へ**, **下へ**  
  ファイル一覧で現在選択されている項目の順序を 1 つだけ上、または下へ移動します。CubePDF Page は、ファイル一覧に表示されている順序で PDF ファイルまたは画像ファイルを結合します。
* **削除**  
  ファイル一覧で選択されている項目を結合・分割対象から削除します。
* **すべて削除**  
  ファイル一覧のすべての項目を結合・分割対象から削除します。
* **文書プロパティ**  
  結合・分割後の PDF ファイルに対して、文書プロパティや各種セキュリティ機能などの設定を行います。
* **結合**  
  ファイル一覧に登録されたファイルを 1 つの PDF ファイルとして結合します。
* **分割**  
  ファイル一覧に登録された全てのファイルを 1 ページずつ、個別の PDF ファイルに分割します。
* **終了**  
  CubePDF Page を終了します。

### 文書プロパティ

![文書プロパティ](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdfpage/doc/v2/ja/metadata.png)

メイン画面で **文書プロパティ** ボタンをクリックすると、上記のような画面が表示され、タイトルや作成者などの情報を登録する事ができます。ここで登録した情報は、Adobe Acrobat Reader DC などの PDF 閲覧ソフトのプロパティ画面で閲覧する事ができます。また、**ページレイアウト** の項目は、PDF 閲覧ソフトで開いた時の表示方法を変更する事ができます。

### セキュリティ

![セキュリティ](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdfpage/doc/v2/ja/encryption.png)

**文書プロパティ** 画面では、**文書プロパティ** タブに加えて **セキュリティ** タブも表示されます。**セキュリティ** タブでは、PDF ファイルにパスワード等を設定する事ができます。パスワードを設定する場合は、まず **PDF ファイルをパスワードで保護する** の項目を有効にし、**管理用パスワード** とパスワード確認の項目に同じパスワードを 2 回 入力してください。

次に、**操作** の項目で、ユーザに許可・制限する操作を指定します。設定可能な項目は以下の通りです。

* PDF ファイルを開く時にパスワードを要求する
* 印刷を許可する
* テキストや画像のコピーを許可する
* ページの挿入、回転、削除を許可する
* アクセシビリティのための内容の抽出を許可する
* フォームへの入力を許可する
* 注釈の追加、編集を許可する

尚、**PDF ファイルを開く時にパスワードを要求する** の項目を有効にする際、**管理用パスワードと共用する** の項目も有効にすると、CubePDF Page は、閲覧用パスワードに管理用パスワードと同じものを設定します。

ただし、**管理用パスワードと閲覧用パスワードを共有した場合、PDF 閲覧ソフトによっては、印刷やコピー操作等の制限が正常に機能しない事があります**。これは、PDF 閲覧ソフトが管理用パスワードで PDF ファイルを開いたと認識するためと予想されます。そのため、CubePDF Page では、管理用パスワードと共用した場合、その他の操作に関する許可設定を受け付けないように設計しています。

また、管理用パスワードで PDF ファイルを開いたと認識された場合、閲覧用パスワードの除去も含めた全ての PDF 編集が可能になります。閲覧用パスワードを管理用パスワードと共用する場合、これらの動作を十分に理解した上でご利用下さい。

## その他の設定およびバージョン情報

![設定](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdfpage/doc/v2/ja/settings.png)

メイン画面のヘッダ部分に表示されている **CubePDF Page** をクリックすると CubePDF Page のバージョン情報および関連する設定画面が表示されます。

### アプリケーション設定

**設定** タブでは、CubePDF Page の挙動に関するいくつかの設定を変更する事ができます。

**重複リソースを削除してファイルサイズを削減する** の項目を有効にすると、複数の PDF ファイルを結合する際にフォント情報などのリソースが重複する場合、できるだけ重複部分を削除してファイルサイズを削減するよう試みます。現在、この機能を有効にした場合、一部の注釈等において結合前と異なる見た目になる事例が報告されています。そのような現象に遭遇した時には、この項目を無効にする事も検討して下さい。

**結合元 PDF ファイルのしおり情報を維持する** の項目を有効にすると、結合元のそれぞれのしおり情報も結合した上で最終的な PDF ファイルを生成します。現在、CubePDF Utility および CubePDF Page において、PDF ファイルの結合に失敗する事例がいくつか報告されていますが、それらの多くはしおり情報が原因になっているようです。そのような現象に遭遇した時には、この項目を無効にする事も検討して下さい。

**作業フォルダー** には、CubePDF Page が結合・分割処理を実行する際に、一時ファイル等を作成するフォルダーを指定します。CubePDF Page は、初期設定では保存先フォルダーを作業フォルダーとしても利用しますが、保存先フォルダーがネットワークに接続された別の端末である場合、処理速度が著しく低下する可能性があります。このような場合には、あらかじめ作業フォルダーを決めておく事によって、そのような現象を回避できます。尚、初期設定のままで問題ない場合、この項目は空欄にして下さい。

**表示言語** では、CubePDF Page メイン画面のメニュー等の表示言語を設定する事ができます。対応言語は English （英語）と Japanese （日本語）の 2 種類です。また、表示言語で Auto を選択した場合、Windows の言語設定に応じてどちらかの言語が自動的に選択されます。

**起動時にアップデートを確認する** の項目を有効にすると、CubePDF Page のバージョンアップ時にパソコンの右下に通知されます。アップデートの確認は、パソコンの起動時に実行されます。また、アップデートの確認のために送信する情報は、CubePDF Page, Windows, .NET Framework それぞれのバージョン番号です。

### バージョン情報

![バージョン情報](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdfpage/doc/v2/ja/version.png)

**バージョン情報** タブでは、CubePDF Page のバージョン情報が表示されます。

## ショートカットキーの一覧

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

## CubePDF Page のアンインストール

CubePDF Page をアンインストールするには、まず、設定のアプリと機能（Windows 8 以降）または、コントロールパネルのプログラムのアンインストールを選択します。そして、表示される画面で CubePDF Page のアイコンを選択してアンインストールの項目を実行して下さい。

![アンインストール（設定）](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdfpage/doc/v2/ja/uninstall-01.png)
![アンインストール（コントロールパネル）](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdfpage/doc/v2/ja/uninstall-02.png)

## CubePDF Page で問題が発生した場合

CubePDF Page は、```C:\Users\%UserName%\AppData\Local\CubeSoft\CubePdfPage\Log``` フォルダに実行ログを出力しています。問題が発生した時は、これらのログを添付して support@cube-soft.jp までご連絡お願いします（%UserName% の箇所は、ログオン中のユーザ名に置き換えて下さい）。