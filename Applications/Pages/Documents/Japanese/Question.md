CubePDF Page でよくある質問
====

Copyright © 2013 CubeSoft, Inc.  
GNU Affero General Public License version 3 (AGPLv3)  
support@cube-soft.jp  
https://www.cube-soft.jp/cubepdfpage/

### はじめに

この記事は [CubePDF Page](https://www.cube-soft.jp/cubepdfpage/) について、よくある質問 (FAQ) をまとめたものです。CubePDF Page の使用方法については、[CubePDF Page ユーザーマニュアル](https://docs.cube-soft.jp/entry/cubepdf-utility) も参照下さい。

### Windows のどのバージョンで動作しますか？

CubePDF Page の動作対象となる Windows は、**サポート期限の有効な Windows** としています。サポート期限の切れた Windows に関しては、動作するかどうかの保証はありません。また、動作しなくなった場合、そのためだけの修正を行う事もありませんので、ご了承下さい。

Windows 10 や Windows 11 に関しては、利用するフレームワークやライブラリを鑑みても、基本的には特定のバージョンでのみ動作しなくなるような現象が発生する可能性は低いと考えています。そのため、何らかの固有の問題が発生した場合のみ告知する事とし、それ以外の場合には、バージョン毎に動作するかどうかを明示する予定はありません。尚、最終的には、ご自身でインストールする事によって動作および利用の可否を決定して下さい。

Windows Server に関しては、Windows 10 や Windows 11 と同様の理由で、基本的には問題なく動作すると予想しています。また、実際に数多くの動作報告も頂いています。ただ、開発環境側で十分なテストができない事もあり、何らかの Server 特有の問題が発生した時に検証や解決を行う事ができない場合があります。その点、ご了承下さい。

### 必要なランタイムやフレームワーク等はありますか？

CubePDF Page は .NET Framework を用いて開発しています。そのため、CubePDF Page を使用するためには .NET Framework 3.5 以降がインストールされている必要があります（4.7 以降を強く推奨）。.NET Framework は、現在では、ほとんどの場合においてインストール済のはずですが、もしインストールが必要になった場合 [Download .NET Framework](https://dotnet.microsoft.com/download/dotnet-framework) からダウンロードして下さい。

尚、CubePDF Page は、インストールされている .NET Framework のバージョンによってインストールする実行ファイルが異なります。そのため、.NET Framework のバージョンを更新した場合、CubePDF Page も上書きインストールする事をお勧めします。

### インストールオプションを教えて下さい

CubePDF Page のインストーラは [Inno Setup](http://www.jrsoftware.org/isinfo.php) と言う開発用ソフトウェアを用いて作成されています。Inno Setup が提供するインストールオプションの一覧については [Setup Command Line Parameters](http://www.jrsoftware.org/ishelp/index.php?topic=setupcmdline) を参照下さい。

### 実行時にネットワーク通信が発生しますか？

CubePDF Page は、実行時に CubePDF Page 自体が意図的にネットワーク通信を行う事はありません。CubePDF Page に関連するネットワーク通信は、PC 起動時に実行されるアップデート確認のみです。アップデート確認では、CubePDF Page、Windows、.NET Framework のバージョン番号を送信します。このアップデート確認を無効にしたい場合、CubePDF Page の設定ダイアログで **コンピュータの起動時にアップデートを確認する** の項目を無効にして下さい。

### 使用中にエラーが発生しました

CubePDF Page は ```C:\Users\%UserName%\AppData\Local\CubeSoft\CubePdfPage\Log``` フォルダに実行ログを出力しています。何らかの問題が発生した時は、これらのログを添付して support@cube-soft.jp までご連絡下さい（%UserName% の箇所は、ログオン中のユーザ名に置き換えて下さい）。
