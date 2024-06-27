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
/// Text
///
/// <summary>
/// Represents text properties used by the CubePDF Page.
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
    public string Menu_Setting => Get();
    public string Menu_Metadata => Get();
    public string Menu_Merge => Get();
    public string Menu_Split => Get();
    public string Menu_Add => Get();
    public string Menu_Up => Get();
    public string Menu_Down => Get();
    public string Menu_Remove => Get();
    public string Menu_Clear => Get();
    public string Menu_Preview => Get();
    #endregion

    #region Columns
    public string Column_Filename => Get();
    public string Column_Filetype => Get();
    public string Column_Filesize => Get();
    public string Column_Pages => Get();
    public string Column_Date => Get();
    #endregion

    #region Setting window
    public string Setting_Window => Get();
    public string Setting_Tab => Get();
    public string Setting_Version => Get();
    public string Setting_Options => Get();
    public string Setting_Temp => Get();
    public string Setting_Language => Get();
    public string Setting_Others => Get();
    public string Setting_Shrink => Get();
    public string Setting_KeepOutline => Get();
    public string Setting_AutoSort => Get();
    public string Setting_CheckUpdate => Get();
    #endregion

    #region Metadata window
    public string Metadata_Window => Get();
    public string Metadata_Tab => Get();
    public string Metadata_Title => Get();
    public string Metadata_Author => Get();
    public string Metadata_Subject => Get();
    public string Metadata_Keyword => Get();
    public string Metadata_Creator => Get();
    public string Metadata_Version => Get();
    public string Metadata_Layout => Get();

    // Menus for Metadata tab (ComboBox, CheckBox, ...)
    public string Metadata_SinglePage => Get();
    public string Metadata_OneColumn => Get();
    public string Metadata_TwoPageLeft => Get();
    public string Metadata_TwoPageRight => Get();
    public string Metadata_TwoColumnLeft => Get();
    public string Metadata_TwoColumnRight => Get();
    #endregion

    #region Security window (tab)
    public string Security_Tab => Get();
    public string Security_OwnerPassword => Get();
    public string Security_UserPassword => Get();
    public string Security_ConfirmPassword => Get();
    public string Security_Operations => Get();

    // Menus for Security tab (ComboBox, CheckBox, ...)
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

    #region Password window
    public string Password_Window => Get();
    public string Password_Show => Get();
    #endregion

    #region Titles for other dialogs
    public string Window_Add => Get();
    public string Window_Merge => Get();
    public string Window_Split => Get();
    public string Window_Temp => Get();
    #endregion

    #region Error messages
    public string Error_OwnerPassword => Get();
    public string Error_UserPassword => Get();
    #endregion

    #region Warning messages
    public string Warn_Password => Get();
    #endregion

    #region File filters
    public string Filter_All => Get();
    public string Filter_Support => Get();
    public string Filter_Pdf => Get();
    #endregion
}
