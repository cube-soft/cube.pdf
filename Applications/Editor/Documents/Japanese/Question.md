CubePDF Utility のよくある質問 (FAQ)
====

Copyright © 2013 CubeSoft, Inc.  
support@cube-soft.jp  
https://www.cube-soft.jp/cubepdfutility/

## はじめに

この記事は [CubePDF Utility](https://www.cube-soft.jp/cubepdfutility/) について、よくある質問 (FAQ) をまとめたものです。CubePDF Utility の使用方法については、[CubePDF Utility ユーザーマニュアル](https://docs.cube-soft.jp/entry/cubepdfutility) も参照下さい。

### Windows のどのバージョンで動作しますか？

CubePDF Utility の動作対象となる Windows は、**サポート期限の有効な Windows** としています。2021 年現在では Windows 8.1 以降が対象となります。サポート期限の切れた Windows に関しては、動作するかどうかの保証はありません。また、動作しなくなった場合、そのためだけの修正を行う事もありませんので、ご了承下さい。

Windows 10 や Windows 11 に関しては、利用するフレームワークやライブラリを鑑みても、基本的には特定のバージョンでのみ動作しなくなるような現象が発生する可能性は低いと考えています。そのため、何らかの固有の問題が発生した場合のみ告知する事とし、それ以外の場合には、バージョン毎に動作するかどうかを明示する予定はありません。尚、最終的には、ご自身でインストールする事によって動作および利用の可否を決定して下さい。

Windows Server に関しては、Windows 10 と同様の理由で、基本的には問題なく動作すると予想しています。また、実際に数多くの動作報告も頂いています。ただ、開発環境側で十分なテストができない事もあり、何らかの Server 特有の問題が発生した時に検証や解決を行う事ができない場合があります。その点、ご了承下さい。

### 必要なランタイムやフレームワーク等はありますか？

CubePDF Utility は .NET Framework を用いて開発しています。そのため、CubePDF Utility を使用するためには .NET Framework 3.5 以降がインストールされている必要があります（4.5.2 以降を強く推奨）。.NET Framework は、現在では、ほとんどの場合においてインストール済のはずですが、もしインストールが必要になった場合 [Download .NET Framework](https://dotnet.microsoft.com/download/dotnet-framework) からダウンロードして下さい。

尚、CubePDF Utility は、インストールされている .NET Framework のバージョンによってインストールする実行ファイルが異なります。そのため、.NET Framework のバージョンを更新した場合、CubePDF Utility も上書きインストールする事をお勧めします。

### どうやってバージョンアップすれば良いですか？

CubePDF Utility のバージョンアップを行う場合は、最初にインストールした時と同様に [CubePDF Utility のダウンロードページ](https://www.cube-soft.jp/cubepdfutility/) から最新バージョンのインストーラーをダウンロードし、再度実行して下さい。

### インストールオプションを教えて下さい

CubePDF Utility のインストーラは [Inno Setup](http://www.jrsoftware.org/isinfo.php) と言う開発用ソフトウェアを用いて作成されています。Inno Setup が提供するインストールオプションの一覧については [Setup Command Line Parameters](http://www.jrsoftware.org/ishelp/index.php?topic=setupcmdline) を参照下さい。

### 実行時にネットワーク通信が発生しますか？

CubePDF Utility は、実行時に CubePDF Utility 自体が意図的にネットワーク通信を行う事はありません。CubePDF Utility に関連するネットワーク通信は、PC 起動時に実行されるアップデート確認のみです。アップデート確認では、CubePDF Utility、Windows、.NET Framework のバージョン番号を送信します。このアップデート確認を無効にしたい場合、CubePDF Utility「その他」タブの「設定」ボタンをクリックして表示されるダイアログで**コンピュータの起動時にアップデートを確認する** の項目を無効にして下さい。

### 使用中にエラーが発生しました

CubePDF Utility は ```C:\Users\＜ユーザ名＞\AppData\Local\CubeSoft\CubePdfUtility2\Log``` フォルダに実行ログを出力しています。何らかの問題が発生した時は、これらのログを添付して support@cube-soft.jp までご連絡下さい（＜ユーザ名＞の箇所は、ログオン中のユーザ名に置き換えて下さい）。
