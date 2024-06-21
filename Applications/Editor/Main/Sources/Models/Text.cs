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
/// Text
///
/// <summary>
/// Represents text properties used by the CubePDF Utility.
/// </summary>
///
/* ------------------------------------------------------------------------- */
internal class Text : TextBase
{
    // ReSharper disable InconsistentNaming
    #region Menus
    public string Menu_Ok => Get();
    public string Menu_Cancel => Get();
    public string Menu_Exit => Get();
    public string Menu_File => Get();
    public string Menu_Edit => Get();
    public string Menu_Misc => Get();
    public string Menu_Help => Get();
    public string Menu_Setting => Get();
    public string Menu_Preview => Get();
    public string Menu_Metadata => Get();
    public string Menu_Metadata_Long => Get();
    public string Menu_Security => Get();
    public string Menu_Open => Get();
    public string Menu_Close => Get();
    public string Menu_Save => Get();
    public string Menu_Save_Long => Get();
    public string Menu_Save_As => Get();
    public string Menu_Redraw => Get();
    public string Menu_Undo => Get();
    public string Menu_Redo => Get();
    public string Menu_Select => Get();
    public string Menu_Select_All => Get();
    public string Menu_Select_Flip => Get();
    public string Menu_Select_Clear => Get();
    public string Menu_Insert => Get();
    public string Menu_Insert_Long => Get();
    public string Menu_Insert_Head => Get();
    public string Menu_Insert_Tail => Get();
    public string Menu_Insert_Custom => Get();
    public string Menu_Extract => Get();
    public string Menu_Extract_Long => Get();
    public string Menu_Extract_Custom => Get();
    public string Menu_Remove => Get();
    public string Menu_Remove_Long => Get();
    public string Menu_Remove_Custom => Get();
    public string Menu_Move_Back => Get();
    public string Menu_Move_Forth => Get();
    public string Menu_Rotate_Left => Get();
    public string Menu_Rotate_Right => Get();
    public string Menu_Zoom_In => Get();
    public string Menu_Zoom_Out => Get();
    public string Menu_Frame => Get();
    public string Menu_Recent => Get();
    #endregion

    #region Setting window
    public string Setting_Window => Get();
    public string Setting_Tab => Get();
    public string Setting_Version => Get();
    public string Setting_Options => Get();
    public string Setting_Backup => Get();
    public string Setting_Backup_Enable => Get();
    public string Setting_Backup_Clean => Get();
    public string Setting_Temp => Get();
    public string Setting_Language => Get();
    public string Setting_Others => Get();
    public string Setting_Shrink => Get();
    public string Setting_KeepOutline => Get();
    public string Setting_Recent => Get();
    public string Setting_AutoSort => Get();
    public string Setting_CheckUpdate => Get();
    #endregion

    #region Metadata window
    public string Metadata_Window => Get();
    public string Metadata_Summary => Get();
    public string Metadata_Detail => Get();
    public string Metadata_Title => Get();
    public string Metadata_Author => Get();
    public string Metadata_Subject => Get();
    public string Metadata_Keyword => Get();
    public string Metadata_Version => Get();
    public string Metadata_Layout => Get();
    public string Metadata_Creator => Get();
    public string Metadata_Producer => Get();
    public string Metadata_Filename => Get();
    public string Metadata_Filesize => Get();
    public string Metadata_CreationTime => Get();
    public string Metadata_LastWriteTime => Get();
    public string Metadata_SinglePage => Get();
    public string Metadata_OneColumn => Get();
    public string Metadata_TwoPageLeft => Get();
    public string Metadata_TwoPageRight => Get();
    public string Metadata_TwoColumnLeft => Get();
    public string Metadata_TwoColumnRight => Get();
    #endregion

    #region Security window
    public string Security_Window => Get();
    public string Security_OwnerPassword => Get();
    public string Security_UserPassword => Get();
    public string Security_ConfirmPassword => Get();
    public string Security_Method => Get();
    public string Security_Operations => Get();
    public string Security_Enable => Get();
    public string Security_OpenWithPassword => Get();
    public string Security_SharePassword => Get();
    public string Security_AllowPrint => Get();
    public string Security_AllowCopy => Get();
    public string Security_AllowModify => Get();
    public string Security_AllowAccessibility => Get();
    public string Security_AllowForm => Get();
    public string Security_AllowAnnotation => Get();
    #endregion

    #region Insert window
    public string Insert_Window => Get();
    public string Insert_Menu_Add => Get();
    public string Insert_Menu_Up => Get();
    public string Insert_Menu_Down => Get();
    public string Insert_Menu_Remove => Get();
    public string Insert_Menu_Clear => Get();
    public string Insert_Menu_Preview => Get();
    public string Insert_Position => Get();
    public string Insert_Position_Select => Get();
    public string Insert_Position_Head => Get();
    public string Insert_Position_Tail => Get();
    public string Insert_Position_Custom => Get();
    public string Insert_Column_Filename => Get();
    public string Insert_Column_Filetype => Get();
    public string Insert_Column_Filesize => Get();
    public string Insert_Column_LastWriteTime => Get();
    #endregion

    #region Extract window
    public string Extract_Window => Get();
    public string Extract_Destination => Get();
    public string Extract_Format => Get();
    public string Extract_Page => Get();
    public string Extract_Target => Get();
    public string Extract_Target_Select => Get();
    public string Extract_Target_All => Get();
    public string Extract_Target_Custom => Get();
    public string Extract_Options => Get();
    public string Extract_Split => Get();
    #endregion

    #region Remove window
    public string Remove_Window => Get();
    public string Remove_Page => Get();
    public string Remove_Target => Get();
    #endregion

    #region Titles for other dialogs
    public string Window_Open => Get();
    public string Window_Save => Get();
    public string Window_Backup => Get();
    public string Window_Temp => Get();
    public string Window_Preview => Get();
    public string Window_Password => Get();
    #endregion

    #region Error messages
    public string Error_Open => Get();
    public string Error_Backup => Get();
    public string Error_Metadata => Get();
    public string Error_Range => Get();
    #endregion

    #region Warning messages
    public string Warn_Password => Get();
    public string Warn_Overwrite => Get();
    #endregion

    #region Other messages
    public string Message_Loading => Get();
    public string Message_Saving => Get();
    public string Message_Saved => Get();
    public string Message_Pages => Get();
    public string Message_Total => Get();
    public string Message_Selection => Get();
    public string Message_Range => Get();
    public string Message_Byte => Get();
    public string Message_Dpi => Get();
    #endregion

    #region File filters
    public string Filter_All => Get();
    public string Filter_Insertable => Get();
    public string Filter_Extractable => Get();
    public string Filter_Pdf => Get();
    #endregion
}
