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
/// Text
///
/// <summary>
/// Represents text properties used by the CubePDF.
/// </summary>
///
/* ------------------------------------------------------------------------- */
internal class Text : TextBase
{
    #region General tab
    public string General_Header => Get();
    public string General_Source => Get();
    public string General_Destination => Get();
    public string General_Format => Get();
    public string General_Color => Get();
    public string General_Resolution => Get();
    public string General_Orientation => Get();
    public string General_Options => Get();
    public string General_PostProcess => Get();

    // Menus for General tab (ComboBox, CheckBox, ...)
    public string General_Overwrite => Get();
    public string General_MergeHead => Get();
    public string General_MergeTail => Get();
    public string General_Rename => Get();
    public string General_Auto => Get();
    public string General_Rgb => Get();
    public string General_Grayscale => Get();
    public string General_Monochrome => Get();
    public string General_Portrait => Get();
    public string General_Landscape => Get();
    public string General_Jpeg => Get();
    public string General_Linearization => Get();
    public string General_Open => Get();
    public string General_OpenDirectory => Get();
    public string General_None => Get();
    public string General_UserProgram => Get();
    #endregion

    #region Metadata tab
    public string Metadata_Header => Get();
    public string Metadata_Title => Get();
    public string Metadata__Author => Get();
    public string Metadata__Subject => Get();
    public string Metadata__Keyword => Get();
    public string Metadata__Creator => Get();
    public string Metadata__ViewOption => Get();

    // Menus for Metadata tab (ComboBox, CheckBox, ...)
    public string Metadata_SinglePage => Get();
    public string Metadata_OneColumn => Get();
    public string Metadata_TwoPageLeft => Get();
    public string Metadata_TwoPageRight => Get();
    public string Metadata_TwoColumnLeft => Get();
    public string Metadata_TwoColumnRight => Get();
    #endregion

    #region Security tab
    public string Security_Header => Get();
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

    #region Misc tab
    public string Misc_Header => Get();
    public string Misc_About => Get();
    public string Misc_Language => Get();

    // Menus for Misc tab (ComboBox, CheckBox, ...)
    public string MIsc_CheckUpdate => Get();
    #endregion

    #region Buttons
    public string Button_Convert => Get();
    public string Button_Cancel => Get();
    public string Button_Save => Get();
    #endregion

    #region Titles for dialogs
    public string Title_Source => Get();
    public string Title_Destination => Get();
    public string Title_UserProgram => Get();
    #endregion

    #region Error messages
    public string Error_Source => Get();
    public string Error_Digest => Get();
    public string Error_Ghostscript => Get();
    public string Error_InvalidChars => Get();
    public string Error_OwnerPassword => Get();
    public string Error_UserPassword => Get();
    public string Error_MergePassword => Get();
    public string Error_PostProcess => Get();
    #endregion

    #region Warning/Confirm messages
    public string Warn_Exist => Get();
    public string Warn_Overwrite => Get();
    public string Warn_MergeHead => Get();
    public string Warn_MergeTail => Get();
    public string Warn_Metadata => Get();
    #endregion

    #region File filters
    public string Filter_All => Get();
    public string Filter_Pdf => Get();
    public string Filter_Ps => Get();
    public string Filter_Eps => Get();
    public string Filter_Bmp => Get();
    public string Filter_Png => Get();
    public string Filter_Jpeg => Get();
    public string Filter_Tiff => Get();
    public string Filter_Exe => Get();
    #endregion
}
