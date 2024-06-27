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
/// EnglishText
///
/// <summary>
/// Represents the English texts used by CubePDF Page.
/// </summary>
///
/* ------------------------------------------------------------------------- */
internal class EnglishText() : Globalization.TextGroup(new()
{
    // Menus
    { nameof(Text.Menu_Ok), "OK" },
    { nameof(Text.Menu_Cancel), "Cancel" },
    { nameof(Text.Menu_Exit), "Exit" },
    { nameof(Text.Menu_Setting), "Settings" },
    { nameof(Text.Menu_Metadata), "Metadata" },
    { nameof(Text.Menu_Merge), "Merge" },
    { nameof(Text.Menu_Split), "Split" },
    { nameof(Text.Menu_Add), "Add" },
    { nameof(Text.Menu_Up), "Up" },
    { nameof(Text.Menu_Down), "Down" },
    { nameof(Text.Menu_Remove), "Remove" },
    { nameof(Text.Menu_Clear), "Clear" },
    { nameof(Text.Menu_Preview), "Preview" },

    // Columns for Main window
    { nameof(Text.Column_Filename), "Filename" },
    { nameof(Text.Column_Filetype), "Type" },
    { nameof(Text.Column_Filesize), "Filesize" },
    { nameof(Text.Column_Pages), "Pages" },
    { nameof(Text.Column_Date), "Last updated" },

    // Labels for Setting window
    { nameof(Text.Setting_Window), "CubePDF Page Settings" },
    { nameof(Text.Setting_Tab), "Settings" },
    { nameof(Text.Setting_Version), "Version" },
    { nameof(Text.Setting_Options), "Options" },
    { nameof(Text.Setting_Temp), "Temp" },
    { nameof(Text.Setting_Language), "Language" },
    { nameof(Text.Setting_Others), "Others" },
    { nameof(Text.Setting_Shrink), "Reduce duplicated resources" },
    { nameof(Text.Setting_KeepOutline), "Keep bookmarks of source PDF files" },
    { nameof(Text.Setting_AutoSort), "Sort selected files automatically" },
    { nameof(Text.Setting_CheckUpdate), "Check for updates on startup" },

    // Labels for Metadata window
    { nameof(Text.Metadata_Window), "PDF Metadata" },
    { nameof(Text.Metadata_Tab), "Summary" },
    { nameof(Text.Metadata_Title), "Title" },
    { nameof(Text.Metadata_Author), "Author" },
    { nameof(Text.Metadata_Subject), "Subject" },
    { nameof(Text.Metadata_Keyword), "Keywords" },
    { nameof(Text.Metadata_Creator), "Creator" },
    { nameof(Text.Metadata_Version), "Version" },
    { nameof(Text.Metadata_Layout), "Layout" },

    // Menus for Metadata window (ComboBox, CheckBox, RadioButton, ...)
    { nameof(Text.Metadata_SinglePage), "Single page" },
    { nameof(Text.Metadata_OneColumn), "One column" },
    { nameof(Text.Metadata_TwoPageLeft), "Two page (left)" },
    { nameof(Text.Metadata_TwoPageRight), "Two page (right)" },
    { nameof(Text.Metadata_TwoColumnLeft), "Two column (left)" },
    { nameof(Text.Metadata_TwoColumnRight), "Two column (right)" },

    // Labels for Security window
    { nameof(Text.Security_Tab), "Security" },
    { nameof(Text.Security_OwnerPassword), "Password" }, // Omit "Owner" due to space limitation.
    { nameof(Text.Security_UserPassword), "Password" }, // Omit "User" due to space limitation.
    { nameof(Text.Security_ConfirmPassword), "Confirm" },
    { nameof(Text.Security_Operations), "Operations" },

    // Menus for Security window (ComboBox, CheckBox, RadioButton, ...)
    { nameof(Text.Security_Enable), "Encrypt the PDF with password" },
    { nameof(Text.Security_OpenWithPassword), "Open with password" },
    { nameof(Text.Security_SharePassword), "Use owner password" },
    { nameof(Text.Security_AllowPrint), "Allow printing" },
    { nameof(Text.Security_AllowCopy), "Allow copying text and images" },
    { nameof(Text.Security_AllowModify), "Allow inserting and removing pages" },
    { nameof(Text.Security_AllowAccessibility), "Allow using contents for accessibility" },
    { nameof(Text.Security_AllowForm), "Allow filling in forms" },
    { nameof(Text.Security_AllowAnnotation), "Allow creating and editing annotations" },

    // Labels for Password window
    { nameof(Text.Password_Window), "Enter the owner password" },
    { nameof(Text.Password_Show), "Show password" },

    // Titles for other dialogs
    { nameof(Text.Window_Add), "Select source file" },
    { nameof(Text.Window_Merge), "Save merged file as" },
    { nameof(Text.Window_Split), "Select a folder to save the split files." },
    { nameof(Text.Window_Temp), "Select a temp folder. If you do not specify a temp folder, the same folder as the source files will be used." },

    // Error messages
    { nameof(Text.Error_OwnerPassword), "Owner password is empty or does not match the confirmation. Please check your password and confirmation again" },
    { nameof(Text.Error_UserPassword), "User password is empty or does not match the confirmation. Please check the user password or enable the Use owner password checkbox." },

    // Warning messages
    { nameof(Text.Warn_Password), "{0} is protected. Enter the owner password to edit." },

    // File filters
    { nameof(Text.Filter_All), "All files" },
    { nameof(Text.Filter_Support), "All supported files" },
    { nameof(Text.Filter_Pdf), "PDF files" },
});
