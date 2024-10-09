/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
/* ------------------------------------------------------------------------- */
namespace Cube.Pdf.Converter;

/* ------------------------------------------------------------------------- */
///
/// JapaneseText
///
/// <summary>
/// Represents the Japanese texts used by CubePDF.
/// </summary>
///
/* ------------------------------------------------------------------------- */
internal class JapaneseText() : Globalization.TextGroup(new()
{
    // Labels for General tab
    { nameof(Text.General_Tab), "一般" },
    { nameof(Text.General_Source), "入力ファイル" },
    { nameof(Text.General_Destination), "出力ファイル" },
    { nameof(Text.General_Format), "ファイルタイプ" },
    { nameof(Text.General_Color), "カラーモード" },
    { nameof(Text.General_Resolution), "解像度" },
    { nameof(Text.General_Orientation), "ページの向き" },
    { nameof(Text.General_Options), "オプション" },
    { nameof(Text.General_PostProcess), "ポストプロセス" },

    // Menus for General tab (ComboBox, CheckBox, ...)
    { nameof(Text.General_Overwrite), "上書き" },
    { nameof(Text.General_MergeHead), "先頭に結合" },
    { nameof(Text.General_MergeTail), "末尾に結合" },
    { nameof(Text.General_Rename), "リネーム" },
    { nameof(Text.General_Auto), "自動" },
    { nameof(Text.General_Rgb), "RGB" },
    { nameof(Text.General_Grayscale), "グレースケール" },
    { nameof(Text.General_Monochrome), "白黒" },
    { nameof(Text.General_Portrait), "縦" },
    { nameof(Text.General_Landscape), "横" },
    { nameof(Text.General_Jpeg), "PDF ファイル中の画像を JPEG 形式で圧縮する" },
    { nameof(Text.General_Open), "開く" },
    { nameof(Text.General_OpenDirectory), "フォルダーを開く" },
    { nameof(Text.General_None), "何もしない" },
    { nameof(Text.General_UserProgram), "その他" },

    // Labels for Metadata tab
    { nameof(Text.Metadata_Tab), "文書プロパティ" },
    { nameof(Text.Metadata_Title), "タイトル" },
    { nameof(Text.Metadata_Author), "作成者" },
    { nameof(Text.Metadata_Subject), "サブタイトル" },
    { nameof(Text.Metadata_Keyword), "キーワード" },
    { nameof(Text.Metadata_Creator), "変換ソフト" },
    { nameof(Text.Metadata_Layout), "ページレイアウト" },

    // Menus for Metadata tab (ComboBox, CheckBox, ...)
    { nameof(Text.Metadata_SinglePage), "単一ページ" },
    { nameof(Text.Metadata_OneColumn), "連続ページ" },
    { nameof(Text.Metadata_TwoPageLeft), "見開きページ（左綴じ）" },
    { nameof(Text.Metadata_TwoPageRight), "見開きページ（右綴じ）" },
    { nameof(Text.Metadata_TwoColumnLeft), "連続見開きページ（左綴じ）" },
    { nameof(Text.Metadata_TwoColumnRight), "連続見開きページ（右綴じ）" },

    // Labels for Security tab
    { nameof(Text.Security_Tab), "セキュリティ" },
    { nameof(Text.Security_OwnerPassword), "管理用パスワード" },
    { nameof(Text.Security_UserPassword), "閲覧用パスワード" },
    { nameof(Text.Security_ConfirmPassword), "パスワード確認" },
    { nameof(Text.Security_Operations), "操作" },

    // Menus for Security tab (ComboBox, CheckBox, ...)
    { nameof(Text.Security_Enable), "PDF ファイルをパスワードで保護する" },
    { nameof(Text.Security_OpenWithPassword), "PDF ファイルを開く時にパスワードを要求する" },
    { nameof(Text.Security_SharePassword), "管理用パスワードと共用する" },
    { nameof(Text.Security_AllowPrint), "印刷を許可する" },
    { nameof(Text.Security_AllowCopy), "テキストや画像のコピーを許可する" },
    { nameof(Text.Security_AllowModify), "ページの挿入、回転、削除を許可する" },
    { nameof(Text.Security_AllowAccessibility), "アクセシビリティのための内容の抽出を許可する" },
    { nameof(Text.Security_AllowForm), "フォームへの入力を許可する" },
    { nameof(Text.Security_AllowAnnotation), "注釈の追加、編集を許可する" },

    // Labels for Misc tab
    { nameof(Text.Misc_Tab), "その他" },
    { nameof(Text.Misc_About), "バージョン情報" },
    { nameof(Text.Misc_Language), "表示言語" },

    // Menus for Misc tab (ComboBox, CheckBox, ...)
    { nameof(Text.Misc_CheckUpdate), "起動時にアップデートを確認する" },

    // Buttons
    { nameof(Text.Menu_Convert), "変換" },
    { nameof(Text.Menu_Cancel), "キャンセル" },
    { nameof(Text.Menu_Save), "設定を保存" },

    // Titles for dialogs
    { nameof(Text.Window_Source), "入力ファイルを選択" },
    { nameof(Text.Window_Destination), "名前を付けて保存" },
    { nameof(Text.Window_UserProgram), "変換完了時に実行するプログラムを選択" },

    // Error messages
    { nameof(Text.Error_Source), "入力ファイルが指定されていません。正常な手順で CubePDF が実行されたかどうか確認して下さい。" },
    { nameof(Text.Error_Digest), "入力ファイルのハッシュ値が一致しません。入力ファイルが破損したか、または改ざんされた可能性があります。" },
    { nameof(Text.Error_Ghostscript), "Ghostscript API による変換中にエラーが発生しました。({0:D})" },
    { nameof(Text.Error_InvalidChars), "ファイル名に次の文字を使用する事はできません。" },
    { nameof(Text.Error_OwnerPassword), "パスワードが入力されていないか、または確認欄と一致しません。パスワードおよびパスワード確認欄を確認して下さい。" },
    { nameof(Text.Error_UserPassword), "閲覧用パスワードが入力されていないか、または確認欄と一致しません。パスワードを再度ご確認するか、「管理用パスワードと共用する」の項目を有効にして下さい。" },
    { nameof(Text.Error_MergePassword), "結合対象として指定された PDF ファイルの管理用パスワードをセキュリティタブに入力して下さい。" },
    { nameof(Text.Error_PostProcess), "変換処理は正常に完了しましたが、ポストプロセスの実行に失敗しました。ファイルの関連付けやユーザープログラムの設定を確認して下さい。" },

    // Warning/Confirm messages
    { nameof(Text.Warn_Exist), "{0} は既に存在します。" },
    { nameof(Text.Warn_Overwrite), "上書きしますか？" },
    { nameof(Text.Warn_MergeHead), "先頭に結合しますか？" },
    { nameof(Text.Warn_MergeTail), "末尾に結合しますか？" },
    { nameof(Text.Warn_Metadata), "タイトル、作成者、サブタイトル、キーワードのいずれかの項目が入力されています。これらの内容は次回以降、CubePDF 起動時の初期設定として利用されます。設定を保存しても良いですか？" },

    // File filters
    { nameof(Text.Filter_All), "すべてのファイル" },
    { nameof(Text.Filter_Pdf), "PDF ファイル" },
    { nameof(Text.Filter_Ps), "PS ファイル" },
    { nameof(Text.Filter_Eps), "EPS ファイル" },
    { nameof(Text.Filter_Bmp), "BMP ファイル" },
    { nameof(Text.Filter_Png), "PNG ファイル" },
    { nameof(Text.Filter_Jpeg), "JPEG ファイル" },
    { nameof(Text.Filter_Tiff), "TIFF ファイル" },
    { nameof(Text.Filter_Exe), "実行可能なファイル" },
});