CubePDF の基本的な使用方法について
====

Copyright © 2010 CubeSoft, Inc.  
GNU Affero General Public License version 3 (AGPLv3)  
support@cube-soft.jp  
https://www.cube-soft.jp/cubepdf/

[:contents]

## CubePDF の使い方が分かりません

![CubePDF の使用方法](https://github.com/cube-soft/Cube.Pdf/blob/master/Applications/Converter/Assets/01.ja.png?raw=true)

CubePDF は「仮想プリンター」と呼ばれる類のソフトウェアです。まず、PDF に変換したい内容を Google Chrome や Microsoft Word など適当なアプリケーションで開きます。次に、それらのアプリケーションの「印刷」メニューを選択し、プリンター一覧の中から CubePDF を選択して実行します。そうすると CubePDF のメイン画面が表示されるので、必要な設定を行った後に変換ボタンをクリックすれば完了です。

## デスクトップに CubePDF のアイコンがありません

CubePDF は仮想プリンターと呼ばれる類のソフトウェアなので、一般的なアプリケーションのように、デスクトップやスタートメニュー等から直接 CubePDF を 実行したり、PDF ファイル等に関連付けたりして利用する事はできません。そのため、デスクトップに CubePDF のショートカット（アイコン）が作成される事もありません。

## PDF ファイルが開けません

CubePDF は PDF ファイル等への変換機能のみを提供するソフトウェアです。そのため、変換後のファイルを閲覧するには別途
アプリケーションが必要となります。例えば、PDF ファイルを閲覧するアプリケーションとしては
[Adobe Acrobat Reader DC](https://get.adobe.com/jp/reader/) 等が挙げられます。

![ポストプロセスを何もしないに変更](https://github.com/cube-soft/Cube.Pdf/blob/master/Applications/Converter/Assets/Question.01.ja.png?raw=true)

また、変換したファイルを開くためのアプリケーションがご利用の PC にインストールされていない場合、CubePDF 実行時にエラーとなる事があります。この場合、メイン画面の「ポストプロセス」の項目を「何もしない」に変更して回避して下さい。

## PDF ファイルのファイルサイズが突然大きくなりました

![ポストプロセスを何もしないに変更](https://github.com/cube-soft/Cube.Pdf/blob/master/Applications/Converter/Assets/Question.03.ja.png?raw=true)

PDF ファイルへの変換については、一見同じような内容に見えても、印刷を実行するアプリケーションの設定や内容自体の細かな差によりファイルサイズが大きく変化する場合があります。

しかし、同じアプリケーションから同じ内容を印刷したにも関わらず、変換される PDF ファイルのファイルサイズに大きな差がある場合、CubePDF で画像を JPEG 形式に圧縮したかどうかの違いである可能性が高いと予想されます。この場合には、メイン画面「その他」タブにある「PDF ファイル中の画像を JPEG 形式で圧縮する」の項目が有効であるかを確認して下さい。

## PDF ファイルの結合に失敗しました

![PDF ファイルがパスワードによって保護されている場合](https://github.com/cube-soft/Cube.Pdf/blob/master/Applications/Converter/Assets/Question.04.ja.png?raw=true)

PDF ファイルの結合に失敗した場合、考えられる可能性の一つとして結合先の PDF ファイルがパスワードによって保護されている事が挙げられます。PDF ファイルは、パスワード無しで開く事ができても、パスワードによって保護されている場合があります。例えば、Adobe Acrobat Reader で PDF ファイルを開いた時に「保護」と言う表示がある場合、その PDF ファイルはパスワードによって保護されています。この場合、CubePDF メイン画面「セキュリティ」タブで、結合先 PDF ファイルの「**管理用パスワード**」を入力する必要があります。

![PDF ファイルが他のアプリケーションで開かれている場合](https://github.com/cube-soft/Cube.Pdf/blob/master/Applications/Converter/Assets/Question.05.ja.png?raw=true)

また、上記のように「ファイルが別のプロセスで使用されているため、プロセスはファイルにアクセスできません」と言うエラーメッセージが表示された場合、結合先 PDF ファイルが何らかのアプリケーションで開かれている可能性が高いと予想されます。この場合は、PDF ファイルを開いていないか再度ご確認をお願いします。

## PDF ファイルを編集できますか？

CubePDF は、変換時に他の PDF ファイルに結合したり、文書プロパティやパスワード等の設定を行うことはできますが、基本的には PDF 等に変換するためのソフトウェアとなっています。既に存在する PDF ファイルに対して、結合、分割、回転等のページ単位の編集を行いたい場合、[CubePDF Utility](https://www.cube-soft.jp/cubepdfutility/) や [CubePDF Page](https://www.cube-soft.jp/cubepdfpage/) をご利用下さい。

## 実行時にネットワーク通信が発生しますか？

CubePDF は、実行時に CubePDF 自体が意図的にネットワーク通信を行う事はありません。CubePDF に関連するネットワーク通信は、PC 起動時に実行されるアップデート確認のみです。アップデート確認では、CubePDF、Windows、.NET Framework のバージョン番号を送信します。このアップデート確認を無効にしたい場合、メイン画面「その他」タブの「起動時にアップデートを確認する」項目を無効にした上で、左下にある「設定を保存」ボタンをクリックして下さい。

![アップデート確認の無効方法](https://github.com/cube-soft/Cube.Pdf/blob/master/Applications/Converter/Assets/Question.02.ja.png?raw=true)

尚、過去にお問い合わせ頂いた問題については、[CubePDF 実行時に発生する通信について](https://clown.cube-soft.jp/entry/2011/10/26/upnp) をご覧ください。これに関しては Windows 自体が行っているものであり CubePDF 側で制御する事はできないと認識しています。

## Windows のどのバージョンで動作しますか？

CubePDF の動作対象となる Windows は、**サポート期限の有効な Windows** としています。2019 年現在では Windows 7 以降が対象となります。サポート期限の切れた Windows に関しては、動作するかどうかの保証はありません。また、動作しなくなった場合、そのためだけの修正を行う事もありませんので、ご了承下さい。

Windows 10 に関しては、利用するフレームワークやライブラリを鑑みても、基本的には特定のバージョンでのみ動作しなくなるような現象が発生する可能性は低いと考えています。そのため、何らかの固有の問題が発生した場合のみ告知する事とし、それ以外の場合には、バージョン毎に動作するかどうかを明示する予定はありません。尚、最終的には、ご自身でインストールする事によって動作および利用の可否を決定して下さい。

Windows Server に関しては、Windows 10 と同様の理由で、基本的には問題なく動作すると予想しています。また、実際に数多くの動作報告も頂いています。ただ、開発環境側で十分なテストができない事もあり、何らかの Server エディション特有の問題が発生した時に検証や解決を行う事ができない場合があります。その点、ご了承下さい。

## 必要なランタイムやフレームワーク等はありますか？

CubePDF は .NET Framework を用いて開発しています。そのため、CubePDF を使用するためには .NET Framework 3.5 以降がインストールされている必要があります（4.5.2 以降を強く推奨）。.NET Framework は、現在では、ほとんどの場合においてインストール済のはずですが、もしインストールが必要になった場合 [Download .NET Framework](https://dotnet.microsoft.com/download/dotnet-framework) からダウンロードして下さい。

また、CubePDF は仮想プリンターを構築する際に、Windows の標準プリンタードライバーである PScript5 を利用します。そのため、ご利用の PC に該当モジュール（pscript5.dll および ps5ui.dll）が存在しない場合、インストールに失敗します。インストールに失敗した場合、```C:\ProgramData\CubeSoft\CubePDF\Log\CubeVpc.log``` を添付の上、support@cube-soft.jp までご連絡をお願いします。

## どうやってバージョンアップすれば良いですか？

CubePDF のバージョンアップを行う場合は、最初にインストールした時と同様に [CubePDF のダウンロードページ](https://www.cube-soft.jp/cubepdf/) から最新バージョンのインストーラーをダウンロードし、再度実行して下さい。

## インストールオプションを教えて下さい

CubePDF のインストーラは [Inno Setup](http://www.jrsoftware.org/isinfo.php) と言う開発用ソフトウェアを用いて作成されています。Inno Setup が提供するインストールオプションの一覧については [Setup Command Line Parameters](http://www.jrsoftware.org/ishelp/index.php?topic=setupcmdline) を参照下さい。

## GUI を非表示にして変換を自動化できますか？

いいえ。CubePDF のメイン画面を非表示にする設定は存在しません。有償で提供している [CubePDF Customize](https://www.cube-soft.jp/cubevp/) では、メイン画面の非表示を含めユーザがプログラミングする事によって、より柔軟なカスタム仮想プリンターを作成する事ができます。もし良ければ、こちらもご検討下さい。