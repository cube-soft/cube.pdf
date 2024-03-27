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

using Cube.Globalization;

/* ------------------------------------------------------------------------- */
///
/// EnglishText
///
/// <summary>
/// Represents the English texts used by CubePDF.
/// </summary>
///
/* ------------------------------------------------------------------------- */
static class EnglishText
{
    public static TextGroup Get() => new()
    {
        // Labels for General tab
        { nameof(Text.General_Header), "General" },
        { nameof(Text.General_Source), "Source" },
        { nameof(Text.General_Destination), "Destination" },
        { nameof(Text.General_Format), "Format" },
        { nameof(Text.General_Color), "Color" },
        { nameof(Text.General_Resolution), "Resolution" },
        { nameof(Text.General_Orientation), "Orientation" },
        { nameof(Text.General_Options), "Options" },
        { nameof(Text.General_PostProcess), "Post Process" },

        // Menus for General tab (ComboBox, CheckBox, RadioButton, ...)
        { nameof(Text.General_Overwrite), "Overwrite" },
        { nameof(Text.General_MergeHead), "Merge at the beginning" },
        { nameof(Text.General_MergeTail), "Merge at the end" },
        { nameof(Text.General_Rename), "Rename" },
        { nameof(Text.General_Auto), "Auto" },
        { nameof(Text.General_Rgb), "RGB" },
        { nameof(Text.General_Grayscale), "Grayscale" },
        { nameof(Text.General_Monochrome), "Monochrome" },
        { nameof(Text.General_Portrait), "Portrait" },
        { nameof(Text.General_Landscape), "Landscape" },
        { nameof(Text.General_Jpeg), "Compress PDF images as JPEG" },
        { nameof(Text.General_Linearization), "Optimize PDF for fast Web view" },
        { nameof(Text.General_Open), "Open" },
        { nameof(Text.General_OpenDirectory), "Open directory" },
        { nameof(Text.General_None), "None" },
        { nameof(Text.General_UserProgram), "Others" },

        // Labels for Metadata tab
        { nameof(Text.Metadata_Header), "Metadata" },
        { nameof(Text.Metadata_Title), "Title" },
        { nameof(Text.Metadata__Author), "Author" },
        { nameof(Text.Metadata__Subject), "Subject" },
        { nameof(Text.Metadata__Keyword), "Keywords" },
        { nameof(Text.Metadata__Creator), "Creator" },
        { nameof(Text.Metadata__ViewOption), "Layout" },

        // Menus for Metadata tab (ComboBox, CheckBox, RadioButton, ...)
        { nameof(Text.Metadata_SinglePage), "Single page" },
        { nameof(Text.Metadata_OneColumn), "One column" },
        { nameof(Text.Metadata_TwoPageLeft), "Two page (left)" },
        { nameof(Text.Metadata_TwoPageRight), "Two page (right)" },
        { nameof(Text.Metadata_TwoColumnLeft), "Two column (left)" },
        { nameof(Text.Metadata_TwoColumnRight), "Two column (right)" },

        // Labels for Security tab
        { nameof(Text.Security_Header), "Security" },
        { nameof(Text.Security_OwnerPassword), "Password" }, // Omit "Owner" due to space limitation.
        { nameof(Text.Security_UserPassword), "Password" }, // Omit "User" due to space limitation.
        { nameof(Text.Security_ConfirmPassword), "Confirm" },
        { nameof(Text.Security_Operations), "Operations" },

        // Menus for Security tab (ComboBox, CheckBox, RadioButton, ...)
        { nameof(Text.Security_Enable), "Encrypt the PDF with password" },
        { nameof(Text.Security_OpenWithPassword), "Open with password" },
        { nameof(Text.Security_SharePassword), "Use owner password" },
        { nameof(Text.Security_AllowPrint), "Allow printing" },
        { nameof(Text.Security_AllowCopy), "Allow copying text and images" },
        { nameof(Text.Security_AllowModify), "Allow inserting and removing pages" },
        { nameof(Text.Security_AllowAccessibility), "Allow using contents for accessibility" },
        { nameof(Text.Security_AllowForm), "Allow filling in forms" },
        { nameof(Text.Security_AllowAnnotation), "Allow creating and editing annotations" },

        // Labels for Misc tab
        { nameof(Text.Misc_Header), "Others" },
        { nameof(Text.Misc_About), "About" },
        { nameof(Text.Misc_Language), "Language" },

        // Menus for Misc tab (ComboBox, CheckBox, RadioButton, ...)
        { nameof(Text.MIsc_CheckUpdate), "Check for updates on startup" },

        // Buttons
        { nameof(Text.Button_Convert), "Convert" },
        { nameof(Text.Button_Cancel), "Cancel" },
        { nameof(Text.Button_Save), "Save Settings" },

        // Titles for dialogs
        { nameof(Text.Title_Source), "Select source file" },
        { nameof(Text.Title_Destination), "Save as" },
        { nameof(Text.Title_UserProgram), "Select user program" },

        // Error messages
        { nameof(Text.Error_Source), "Input file not specified. Please check if CubePDF was executed through the normal procedure." },
        { nameof(Text.Error_Digest), "Message digest of the source file does not match." },
        { nameof(Text.Error_Ghostscript), "Ghostscript error ({0:D})" },
        { nameof(Text.Error_InvalidChars), "Path cannot contain any of the following characters." },
        { nameof(Text.Error_OwnerPassword), "Owner password is empty or does not match the confirmation. Please check your password and confirmation again." },
        { nameof(Text.Error_UserPassword), "User password is empty or does not match the confirmation. Please check the user password or enable the \"Use owner password\" checkbox." },
        { nameof(Text.Error_MergePassword), "Set the owner password of the PDF file to be merged in the Security tab." },
        { nameof(Text.Error_PostProcess), "Post-process failed, although the conversion was completed. Check the file association settings or the user program is correct." },

        // Warning/Confirm messages
        { nameof(Text.Warn_Exist), "{0} already exists." },
        { nameof(Text.Warn_Overwrite), "Do you want to overwrite the file?" },
        { nameof(Text.Warn_MergeHead), "Do you want to merge at the beginning of the existing file?" },
        { nameof(Text.Warn_MergeTail), "Do you want to merge at the end of the existing file?" },
        { nameof(Text.Warn_Metadata), "Some value is entered in the Title, Author, Subject, or Keywords field. Do you want to save the settings?" },

        // File filters
        { nameof(Text.Filter_All), "All files" },
        { nameof(Text.Filter_Pdf), "PDF files" },
        { nameof(Text.Filter_Ps), "PS files" },
        { nameof(Text.Filter_Eps), "EPS files" },
        { nameof(Text.Filter_Bmp), "BMP files" },
        { nameof(Text.Filter_Png), "PNG files" },
        { nameof(Text.Filter_Jpeg), "JPEG files" },
        { nameof(Text.Filter_Tiff), "TIFF files" },
        { nameof(Text.Filter_Exe), "Executable files" },
    };
}
