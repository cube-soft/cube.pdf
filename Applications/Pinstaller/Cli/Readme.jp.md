CubePinstaller
====

Copyright © 2010 CubeSoft, Inc.  
support@cube-soft.jp  
https://www.cube-soft.jp/cubepdf/

## はじめに

CubePinstaller は、プリンタのインストールおよびアンインストールを実行するための
コマンドライン型アプリケーションです。CubePinstaller を使用するためには、
Microsoft .NET Framework 3.5 以降がインストールされている
必要があります。Microsoft .NET Framework 3.5 は、以下の URL からダウンロードして下さい。

* Microsoft .NET Framework 4.5.2  
  https://www.microsoft.com/ja-JP/download/details.aspx?id=42643
* Microsoft .NET Framework 3.5  
  https://www.microsoft.com/ja-jp/download/details.aspx?id=22

## 利用方法

```
CubePinstaller.exe JSON /Command COMMAND [OPTIONS] 
```

CubePinstaller の必須パラメータは *JSON* および *COMMAND* の 2 種類です。

*JSON* には、インストールまたはアンインストールするプリンタ構成を記載した
JSON 形式のファイルへのパスを記載します。

*COMMAND* には、下記の 3 種類の中から一つを指定します。

* **Install**  
  指定された構成でプリンタをインストールします。
  既にインストールされている項目に関しては、処理をスキップします。
* **Uninstall**  
  指定された構成でプリンタをアンインストールします。
* **Reinstall**  
  指定されたプリンタ等をいったんアンインストールした後、再度インストールを実行します。

*OPTIONS* に指定可能なオプションは下記の通りです。

* **/Resource DIRECTORY**  
  JSON ファイルに記載される、インストールに必要な各種ファイルが存在する
  ディレクトリへのパスを指定します。
* **/Relative**  
  コマンドライン上で指定するパスを CubePinstaller.exe が存在するディレクトリ
  からの相対パスとして認識します。
* **/Force**  
  JSON ファイルに記載されたプリンタドライバやポートモニタに依存する全ての要素を
  強制的にアンインストールします。このオプションを指定しない場合、対象となる
  プリンタドライバ等が他のプリンタに使用されているなどの理由で、アンインストールに
  失敗する事があります。
* **/Retry COUNT**  
  プリンタ等のインストールまたはアンインストールに失敗した時に再試行する回数を
  指定します。
* **/Timeout SECOND**  
  プリンタ等のインストールまたはアンインストール実行時のタイムアウト時間の初期値を
  秒単位で指定します。実際のタイムアウト時間は、実行に失敗する度に等倍します。
  例えば 30 を指定した場合、実際のタイムアウト時間は 30 秒、60 秒、90 秒となります。

CubePinstaller の実行コマンド例は下記の通りです。

```
CubePinstaller.exe CubePrinter.json
    /Command Reinstall
    /Force
    /Relative
    /Resource Printers
    /Retry 6
    /Timeout 30
```