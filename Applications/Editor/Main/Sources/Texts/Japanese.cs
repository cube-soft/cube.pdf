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
namespace Cube.Pdf.Editor;

/* ------------------------------------------------------------------------- */
///
/// JapaneseText
///
/// <summary>
/// Represents the Japanese texts used by CubePDF Utility.
/// </summary>
///
/* ------------------------------------------------------------------------- */
internal class JapaneseText() : Globalization.TextGroup(new()
{
    // Menus. Note that Menu_*_Long values are used for tooltips.
    { nameof(Text.Menu_Ok), "OK" },
    { nameof(Text.Menu_Cancel), "キャンセル" },
    { nameof(Text.Menu_Exit), "終了" },
    { nameof(Text.Menu_File), "ファイル" },
    { nameof(Text.Menu_Edit), "編集" },
    { nameof(Text.Menu_Misc), "その他" },
    { nameof(Text.Menu_Help), "ヘルプ" },
    { nameof(Text.Menu_Setting), "設定" },
    { nameof(Text.Menu_Preview), "プレビュー" },
    { nameof(Text.Menu_Metadata), "プロパティ" },
    { nameof(Text.Menu_Metadata_Long), "文書プロパティ" },
    { nameof(Text.Menu_Security), "セキュリティ" },
    { nameof(Text.Menu_Open), "開く" },
    { nameof(Text.Menu_Close), "閉じる" },
    { nameof(Text.Menu_Save), "保存" },
    { nameof(Text.Menu_Save_Long), "上書き保存" },
    { nameof(Text.Menu_Save_As), "名前を付けて保存" },
    { nameof(Text.Menu_Redraw), "更新" },
    { nameof(Text.Menu_Undo), "元に戻す" },
    { nameof(Text.Menu_Redo), "なり直し" },
    { nameof(Text.Menu_Select), "選択" },
    { nameof(Text.Menu_Select_All), "すべて選択" },
    { nameof(Text.Menu_Select_Flip), "選択の切り替え" },
    { nameof(Text.Menu_Select_Clear), "選択を解除" },
    { nameof(Text.Menu_Insert), "挿入" },
    { nameof(Text.Menu_Insert_Long), "選択位置の後に挿入" },
    { nameof(Text.Menu_Insert_Head), "先頭に挿入" },
    { nameof(Text.Menu_Insert_Tail), "末尾に挿入" },
    { nameof(Text.Menu_Insert_Custom), "詳細を設定して挿入" },
    { nameof(Text.Menu_Extract), "抽出" },
    { nameof(Text.Menu_Extract_Long), "選択ページを抽出" },
    { nameof(Text.Menu_Extract_Custom), "詳細を設定して抽出" },
    { nameof(Text.Menu_Remove), "削除" },
    { nameof(Text.Menu_Remove_Long), "選択ページを削除" },
    { nameof(Text.Menu_Remove_Custom), "範囲を指定して削除" },
    { nameof(Text.Menu_Move_Back), "前へ" },
    { nameof(Text.Menu_Move_Forth), "後へ" },
    { nameof(Text.Menu_Rotate_Left), "左90度" },
    { nameof(Text.Menu_Rotate_Right), "右90度" },
    { nameof(Text.Menu_Zoom_In), "拡大" },
    { nameof(Text.Menu_Zoom_Out), "縮小" },
    { nameof(Text.Menu_Frame), "枠線のみ" },
    { nameof(Text.Menu_Recent), "最近開いたファイル" },

    // Setting window
    { nameof(Text.Setting_Window), "CubePDF Utility 設定" },
    { nameof(Text.Setting_Tab), "設定" },
    { nameof(Text.Setting_Version), "バージョン" },
    { nameof(Text.Setting_Options), "保存オプション" },
    { nameof(Text.Setting_Backup), "バックアップ" },
    { nameof(Text.Setting_Backup_Enable), "バックアップ機能を有効にする" },
    { nameof(Text.Setting_Backup_Clean), "古いバックアップファイルを自動的に削除する" },
    { nameof(Text.Setting_Temp), "作業フォルダー" },
    { nameof(Text.Setting_Language), "表示言語" },
    { nameof(Text.Setting_Others), "その他" },
    { nameof(Text.Setting_Shrink), "重複リソースを削除してファイルサイズを削減する" },
    { nameof(Text.Setting_KeepOutline), "PDF ファイルのしおり情報を維持する" },
    { nameof(Text.Setting_Recent), "最近開いたファイルを表示する" },
    { nameof(Text.Setting_CheckUpdate), "起動時にアップデートを確認する" },

    // Metadata window
    { nameof(Text.Metadata_Window), "文書プロパティ" },
    { nameof(Text.Metadata_Summary), "概要" },
    { nameof(Text.Metadata_Detail), "詳細情報" },
    { nameof(Text.Metadata_Title), "タイトル" },
    { nameof(Text.Metadata_Author), "作成者" },
    { nameof(Text.Metadata_Subject), "サブタイトル" },
    { nameof(Text.Metadata_Keyword), "キーワード" },
    { nameof(Text.Metadata_Version), "PDF バージョン" },
    { nameof(Text.Metadata_Layout), "ページレイアウト" },
    { nameof(Text.Metadata_Creator), "アプリケーション" },
    { nameof(Text.Metadata_Producer), "PDF 変換" },
    { nameof(Text.Metadata_Filename), "ファイル名" },
    { nameof(Text.Metadata_Filesize), "ファイルサイズ" },
    { nameof(Text.Metadata_CreationTime), "作成日時" },
    { nameof(Text.Metadata_LastWriteTime), "最終更新日時" },
    { nameof(Text.Metadata_SinglePage), "単一ページ" },
    { nameof(Text.Metadata_OneColumn), "連続ページ" },
    { nameof(Text.Metadata_TwoPageLeft), "見開きページ（左綴じ）" },
    { nameof(Text.Metadata_TwoPageRight), "見開きページ（右綴じ）" },
    { nameof(Text.Metadata_TwoColumnLeft), "連続見開きページ（左綴じ）" },
    { nameof(Text.Metadata_TwoColumnRight), "連続見開きページ（右綴じ）" },

    // Security window
    { nameof(Text.Security_Window), "セキュリティ設定" },
    { nameof(Text.Security_OwnerPassword), "管理用パスワード" },
    { nameof(Text.Security_UserPassword), "閲覧用パスワード" },
    { nameof(Text.Security_ConfirmPassword), "パスワード確認" },
    { nameof(Text.Security_Method), "暗号化方式" },
    { nameof(Text.Security_Operations), "操作" },
    { nameof(Text.Security_Enable), "PDF ファイルをパスワードで保護する" },
    { nameof(Text.Security_OpenWithPassword), "PDF ファイルを開く時にパスワードを要求する" },
    { nameof(Text.Security_SharePassword), "管理用パスワードと共用する" },
    { nameof(Text.Security_AllowPrint), "印刷を許可する" },
    { nameof(Text.Security_AllowCopy), "テキストや画像のコピーを許可する" },
    { nameof(Text.Security_AllowModify), "ページの挿入、回転、削除を許可する" },
    { nameof(Text.Security_AllowAccessibility), "アクセシビリティのための内容の抽出を許可する" },
    { nameof(Text.Security_AllowForm), "フォームへの入力を許可する" },
    { nameof(Text.Security_AllowAnnotation), "注釈の追加、編集を許可する" },

    // Insert window
    { nameof(Text.Insert_Window), "詳細を設定して挿入" },
    { nameof(Text.Insert_Menu_Add), "追加" },
    { nameof(Text.Insert_Menu_Up), "上へ" },
    { nameof(Text.Insert_Menu_Down), "下へ" },
    { nameof(Text.Insert_Menu_Remove), "削除" },
    { nameof(Text.Insert_Menu_Clear), "全て削除" },
    { nameof(Text.Insert_Menu_Preview), "プレビュー" },
    { nameof(Text.Insert_Position), "挿入位置" },
    { nameof(Text.Insert_Position_Select), "選択ページの直後" },
    { nameof(Text.Insert_Position_Head), "先頭" },
    { nameof(Text.Insert_Position_Tail), "末尾" },
    { nameof(Text.Insert_Position_Custom), "指定ページの直後" },
    { nameof(Text.Insert_Column_Filename), "ファイル名" },
    { nameof(Text.Insert_Column_Filetype), "種類" },
    { nameof(Text.Insert_Column_Filesize), "ファイルサイズ" },
    { nameof(Text.Insert_Column_LastWriteTime), "最終更新日時" },

    // Extract window
    { nameof(Text.Extract_Window), "詳細を設定して抽出" },
    { nameof(Text.Extract_Destination), "出力ファイル" },
    { nameof(Text.Extract_Format), "ファイルタイプ" },
    { nameof(Text.Extract_Page), "総ページ数" },
    { nameof(Text.Extract_Target), "対象ページ" },
    { nameof(Text.Extract_Target_Select), "選択ページ" },
    { nameof(Text.Extract_Target_All), "全てのページ" },
    { nameof(Text.Extract_Target_Custom), "範囲を指定" },
    { nameof(Text.Extract_Options), "オプション" },
    { nameof(Text.Extract_Split), "1ページ毎に個別のファイルとして保存" },

    // Remove window
    { nameof(Text.Remove_Window), "詳細を設定して削除" },
    { nameof(Text.Remove_Page), "総ページ数" },
    { nameof(Text.Remove_Target), "対象ページ" },

    // Password window

    // Titles for other dialogs
    { nameof(Text.Window_Open), "ファイルを開く" },
    { nameof(Text.Window_Save), "名前を付けて保存" },
    { nameof(Text.Window_Backup), "バックアップフォルダーを選択して下さい。" },
    { nameof(Text.Window_Temp), "作業フォルダーを選択して下さい。作業フォルダーを明示しない場合、開いた PDF ファイルと同じフォルダーが使用されます。" },
    { nameof(Text.Window_Preview), "{0} ({1}/{2}ページ)" },
    { nameof(Text.Window_Password), "管理用パスワードを入力" },

    // Error messages
    { nameof(Text.Error_Open), "ファイルが PDF 形式ではないか、またはデータが破損している可能性があります。" },
    { nameof(Text.Error_Metadata), "文書プロパティの取得に失敗しました。" },
    { nameof(Text.Error_Range), "指定範囲の解析に失敗しました。不適切な文字が含まれていないか確認して下さい。" },

    // Warning messages
    { nameof(Text.Warn_Password), "{0} はパスワードで保護されています。編集するためには管理用パスワードを入力して下さい。" },
    { nameof(Text.Warn_Overwrite), "PDF の内容が編集されています。上書き保存しますか？" },

    // Other messages
    { nameof(Text.Message_Loading), "{0} を開いています ..." },
    { nameof(Text.Message_Saving), "{0} に保存しています ..." },
    { nameof(Text.Message_Saved), "{0} に保存しました" },
    { nameof(Text.Message_Pages), "{0} ページ" },
    { nameof(Text.Message_Total), "全 {0} ページ" },
    { nameof(Text.Message_Selection), "{0} 個の項目を選択" },
    { nameof(Text.Message_Range), "例. 1,2,4-7,9" },
    { nameof(Text.Message_Byte), "バイト" },
    { nameof(Text.Message_Dpi), "dpi" },

    // File filters
    { nameof(Text.Filter_All), "すべてのファイル" },
    { nameof(Text.Filter_Insertable), "挿入可能なファイル" },
    { nameof(Text.Filter_Extractable), "保存可能なファイル" },
    { nameof(Text.Filter_Pdf), "PDF ファイル" },
});
