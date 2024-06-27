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
namespace Cube.Pdf.Pages;

/* ------------------------------------------------------------------------- */
///
/// JapaneseText
///
/// <summary>
/// Represents the Japanese texts used by CubePDF Page.
/// </summary>
///
/* ------------------------------------------------------------------------- */
internal class JapaneseText() : Globalization.TextGroup(new()
{
    // Menus
    { nameof(Text.Menu_Ok), "OK" },
    { nameof(Text.Menu_Cancel), "キャンセル" },
    { nameof(Text.Menu_Exit), "終了" },
    { nameof(Text.Menu_Setting), "設定" },
    { nameof(Text.Menu_Metadata), "文書プロパティ" },
    { nameof(Text.Menu_Merge), "結合" },
    { nameof(Text.Menu_Split), "分割" },
    { nameof(Text.Menu_Add), "追加" },
    { nameof(Text.Menu_Up), "上へ" },
    { nameof(Text.Menu_Down), "下へ" },
    { nameof(Text.Menu_Remove), "削除" },
    { nameof(Text.Menu_Clear), "すべて削除" },
    { nameof(Text.Menu_Preview), "プレビュー" },

    // Columns for Main window
    { nameof(Text.Column_Filename), "ファイル名" },
    { nameof(Text.Column_Filetype), "種類" },
    { nameof(Text.Column_Filesize), "サイズ" },
    { nameof(Text.Column_Pages), "ページ数" },
    { nameof(Text.Column_Date), "更新日時" },

    // Labels for Setting window
    { nameof(Text.Setting_Window), "CubePDF Page 設定" },
    { nameof(Text.Setting_Tab), "設定" },
    { nameof(Text.Setting_Version), "バージョン" },
    { nameof(Text.Setting_Options), "オプション" },
    { nameof(Text.Setting_Temp), "作業フォルダー" },
    { nameof(Text.Setting_Language), "表示言語" },
    { nameof(Text.Setting_Others), "その他" },
    { nameof(Text.Setting_Shrink), "重複リソースを削除してファイルサイズを削減する" },
    { nameof(Text.Setting_KeepOutline), "結合元 PDF ファイルのしおり情報を維持する" },
    { nameof(Text.Setting_AutoSort), "選択ファイルを自動的に並び変える" },
    { nameof(Text.Setting_CheckUpdate), "起動時にアップデートを確認する" },

    // Labels for Metadata window
    { nameof(Text.Metadata_Window), "文書プロパティ" },
    { nameof(Text.Metadata_Tab), "概要" },
    { nameof(Text.Metadata_Title), "タイトル" },
    { nameof(Text.Metadata_Author), "作成者" },
    { nameof(Text.Metadata_Subject), "サブタイトル" },
    { nameof(Text.Metadata_Keyword), "キーワード" },
    { nameof(Text.Metadata_Creator), "アプリケーション" },
    { nameof(Text.Metadata_Version), "バージョン" },
    { nameof(Text.Metadata_Layout), "ページレイアウト" },

    // Menus for Metadata window (ComboBox, CheckBox, RadioButton, ...)
    { nameof(Text.Metadata_SinglePage), "単一ページ" },
    { nameof(Text.Metadata_OneColumn), "連続ページ" },
    { nameof(Text.Metadata_TwoPageLeft), "見開きページ (左綴じ)" },
    { nameof(Text.Metadata_TwoPageRight), "見開きページ (右綴じ)" },
    { nameof(Text.Metadata_TwoColumnLeft), "連続見開きページ (左綴じ)" },
    { nameof(Text.Metadata_TwoColumnRight), "連続見開きページ (右綴じ)" },

    // Labels for Security window
    { nameof(Text.Security_Tab), "セキュリティ" },
    { nameof(Text.Security_OwnerPassword), "管理用パスワード" },
    { nameof(Text.Security_UserPassword), "閲覧用パスワード" },
    { nameof(Text.Security_ConfirmPassword), "パスワード確認" },
    { nameof(Text.Security_Operations), "操作" },

    // Menus for Security window (ComboBox, CheckBox, RadioButton, ...)
    { nameof(Text.Security_Enable), "PDF ファイルをパスワードで保護する" },
    { nameof(Text.Security_OpenWithPassword), "PDF ファイルを開く時にパスワードを要求する" },
    { nameof(Text.Security_SharePassword), "管理用パスワードと共用する" },
    { nameof(Text.Security_AllowPrint), "印刷を許可する" },
    { nameof(Text.Security_AllowCopy), "テキストや画像のコピーを許可する" },
    { nameof(Text.Security_AllowModify), "ページの挿入、回転、削除を許可する" },
    { nameof(Text.Security_AllowAccessibility), "アクセシビリティのための内容の抽出を許可する" },
    { nameof(Text.Security_AllowForm), "フォームへの入力を許可する" },
    { nameof(Text.Security_AllowAnnotation), "注釈の追加、編集を許可する" },

    // Labels for Password window
    { nameof(Text.Password_Window), "管理用パスワードを入力して下さい" },
    { nameof(Text.Password_Show), "パスワードを表示" },

    // Titles for other dialogs
    { nameof(Text.Window_Add), "ファイルを追加" },
    { nameof(Text.Window_Merge), "結合したファイルの保存" },
    { nameof(Text.Window_Split), "分割したファイルを保存するフォルダーを選択して下さい。" },
    { nameof(Text.Window_Temp), "作業フォルダーを選択して下さい。作業フォルダーを明示しない場合、結合・分割元ファイルと同じフォルダーが使用されます。" },

    // Error messages
    { nameof(Text.Error_OwnerPassword), "パスワードが入力されていないか、または確認欄と一致しません。パスワードおよびパスワード確認欄を再度ご確認下さい。" },
    { nameof(Text.Error_UserPassword), "閲覧用パスワードが入力されていないか、または確認欄と一致しません。パスワードを再度ご確認するか、「管理用パスワードと共用する」の項目を有効にして下さい。" },

    // Warning messages
    { nameof(Text.Warn_Password), "{0} はパスワードで保護されています。編集するためには管理用パスワードを入力して下さい。" },

    // File filters
    { nameof(Text.Filter_All), "すべてのファイル" },
    { nameof(Text.Filter_Support), "追加可能なファイル" },
    { nameof(Text.Filter_Pdf), "PDF ファイル" },
});
