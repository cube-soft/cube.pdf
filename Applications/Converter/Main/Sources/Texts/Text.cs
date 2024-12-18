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
using System.Threading;
using Cube.Globalization;

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
internal class Text() : LocalizableText(Make, new EnglishText())
{
    #region Properties

    #region General tab
    public string General_Tab => Get();
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
    public string General_Open => Get();
    public string General_OpenDirectory => Get();
    public string General_None => Get();
    public string General_UserProgram => Get();
    #endregion

    #region Metadata tab
    public string Metadata_Tab => Get();
    public string Metadata_Title => Get();
    public string Metadata_Author => Get();
    public string Metadata_Subject => Get();
    public string Metadata_Keyword => Get();
    public string Metadata_Creator => Get();
    public string Metadata_Layout => Get();

    // Menus for Metadata tab (ComboBox, CheckBox, ...)
    public string Metadata_SinglePage => Get();
    public string Metadata_OneColumn => Get();
    public string Metadata_TwoPageLeft => Get();
    public string Metadata_TwoPageRight => Get();
    public string Metadata_TwoColumnLeft => Get();
    public string Metadata_TwoColumnRight => Get();
    #endregion

    #region Security tab
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

    #region Misc tab
    public string Misc_Tab => Get();
    public string Misc_About => Get();
    public string Misc_Language => Get();

    // Menus for Misc tab (ComboBox, CheckBox, ...)
    public string Misc_CheckUpdate => Get();
    #endregion

    #region Menus
    public string Menu_Convert => Get();
    public string Menu_Cancel => Get();
    public string Menu_Save => Get();
    #endregion

    #region Titles for dialogs
    public string Window_Source => Get();
    public string Window_Destination => Get();
    public string Window_UserProgram => Get();
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

    #endregion

    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// Make
    ///
    /// <summary>
    /// Makes a text group with the specified Language value.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private static TextGroup Make(Language src) => src switch
    {
        Language.Auto     => Make(Locale.GetDefaultLanguage()),
        Language.English  => new EnglishText(),
        Language.German   => new GermanText(),
        Language.Japanese => new JapaneseText(),
        Language.Russian  => new RussianText(),
        Language.SimplifiedChinese => new SimplifiedChineseText(),
        _ => default,
    };

    /* --------------------------------------------------------------------- */
    ///
    /// OnReset
    ///
    /// <summary>
    /// Occurs when the Reset method is invoked.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    protected override void OnReset(Language src)
    {
        base.OnReset(src);

        var ci = src.ToCultureInfo();
        Thread.CurrentThread.CurrentCulture = ci;
        Thread.CurrentThread.CurrentUICulture = ci;
        Properties.Resources.Culture = ci;
    }

    #endregion
}
