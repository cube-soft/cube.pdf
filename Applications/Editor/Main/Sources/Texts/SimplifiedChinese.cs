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
/// SimplifiedChineseText
///
/// <summary>
/// Represents the Simplified Chinese texts used by CubePDF Utility.
/// </summary>
///
/* ------------------------------------------------------------------------- */
internal class SimplifiedChineseText() : Globalization.TextGroup(new()
{
    // Menus. Note that Menu_*_Long values are used for tooltips.
    { nameof(Text.Menu_Ok), "OK" },
    { nameof(Text.Menu_Cancel), "Cancel" },
    { nameof(Text.Menu_Exit), "Exit" },
    { nameof(Text.Menu_File), "File" },
    { nameof(Text.Menu_Edit), "Edit" },
    { nameof(Text.Menu_Misc), "Others" },
    { nameof(Text.Menu_Help), "Help" },
    { nameof(Text.Menu_Setting), "Settings" },
    { nameof(Text.Menu_Preview), "Preview" },
    { nameof(Text.Menu_Metadata), "Metadata" },
    { nameof(Text.Menu_Metadata_Long), "PDF document metadata" },
    { nameof(Text.Menu_Security), "Security" },
    { nameof(Text.Menu_Open), "Open" },
    { nameof(Text.Menu_Close), "Close" },
    { nameof(Text.Menu_Save), "Save" },
    { nameof(Text.Menu_Save_Long), "Save" },
    { nameof(Text.Menu_Save_As), "Save as" },
    { nameof(Text.Menu_Redraw), "Refresh" },
    { nameof(Text.Menu_Undo), "Undo" },
    { nameof(Text.Menu_Redo), "Redo" },
    { nameof(Text.Menu_Select), "Select" },
    { nameof(Text.Menu_Select_All), "Select all" },
    { nameof(Text.Menu_Select_Flip), "Invert selection" },
    { nameof(Text.Menu_Select_Clear), "Deselect all" },
    { nameof(Text.Menu_Insert), "Insert" },
    { nameof(Text.Menu_Insert_Long), "Insert behind selected position" },
    { nameof(Text.Menu_Insert_Head), "Insert at the beginning" },
    { nameof(Text.Menu_Insert_Tail), "Insert at the end" },
    { nameof(Text.Menu_Insert_Custom), "Insert at other position" },
    { nameof(Text.Menu_Extract), "Extract" },
    { nameof(Text.Menu_Extract_Long), "Extract the selected pages" },
    { nameof(Text.Menu_Extract_Custom), "Extract with other settings" },
    { nameof(Text.Menu_Remove), "Remove" },
    { nameof(Text.Menu_Remove_Long), "Remove the selected pages" },
    { nameof(Text.Menu_Remove_Custom), "Remove other pages" },
    { nameof(Text.Menu_Move_Back), "Back" },
    { nameof(Text.Menu_Move_Forth), "Forth" },
    { nameof(Text.Menu_Rotate_Left), "Left" },
    { nameof(Text.Menu_Rotate_Right), "Right" },
    { nameof(Text.Menu_Zoom_In), "ZoomIn" },
    { nameof(Text.Menu_Zoom_Out), "ZoomOut" },
    { nameof(Text.Menu_Frame), "Frame only" },
    { nameof(Text.Menu_Recent), "Recent files" },

    // Setting window
    { nameof(Text.Setting_Window), "CubePDF Utility Settings" },
    { nameof(Text.Setting_Tab), "Settings" },
    { nameof(Text.Setting_Version), "Version" },
    { nameof(Text.Setting_Options), "Save options" },
    { nameof(Text.Setting_Backup), "Backup" },
    { nameof(Text.Setting_Backup_Enable), "Enable backup" },
    { nameof(Text.Setting_Backup_Clean), "Delete old backup files automatically" },
    { nameof(Text.Setting_Temp), "Temp" },
    { nameof(Text.Setting_Language), "Language" },
    { nameof(Text.Setting_Others), "Others" },
    { nameof(Text.Setting_Shrink), "Reduce duplicated resources" },
    { nameof(Text.Setting_KeepOutline), "Keep bookmarks of the source PDF file" },
    { nameof(Text.Setting_Recent), "Show recently used files" },
    { nameof(Text.Setting_CheckUpdate), "Check for updates on startup" },

    // Metadata window
    { nameof(Text.Metadata_Window), "PDF Metadata" },
    { nameof(Text.Metadata_Summary), "Summary" },
    { nameof(Text.Metadata_Detail), "Details" },
    { nameof(Text.Metadata_Title), "Title" },
    { nameof(Text.Metadata_Author), "Author" },
    { nameof(Text.Metadata_Subject), "Subject" },
    { nameof(Text.Metadata_Keyword), "Keywords" },
    { nameof(Text.Metadata_Version), "Version" },
    { nameof(Text.Metadata_Layout), "Layout" },
    { nameof(Text.Metadata_Creator), "Creator" },
    { nameof(Text.Metadata_Producer), "Producer" },
    { nameof(Text.Metadata_Filename), "Filename" },
    { nameof(Text.Metadata_Filesize), "Filesize" },
    { nameof(Text.Metadata_CreationTime), "Creation" },
    { nameof(Text.Metadata_LastWriteTime), "Last updated" },
    { nameof(Text.Metadata_SinglePage), "Single page" },
    { nameof(Text.Metadata_OneColumn), "One column" },
    { nameof(Text.Metadata_TwoPageLeft), "Two page (left)" },
    { nameof(Text.Metadata_TwoPageRight), "Two page (right)" },
    { nameof(Text.Metadata_TwoColumnLeft), "Two column (left)" },
    { nameof(Text.Metadata_TwoColumnRight), "Two column (right)" },

    // Security window
    { nameof(Text.Security_Window), "Security" },
    { nameof(Text.Security_OwnerPassword), "Password" }, // Omit "Owner" due to space limitation.
    { nameof(Text.Security_UserPassword), "Password" }, // Omit "User" due to space limitation.
    { nameof(Text.Security_ConfirmPassword), "Confirm" },
    { nameof(Text.Security_Method), "Method" },
    { nameof(Text.Security_Operations), "Operations" },
    { nameof(Text.Security_Enable), "Encrypt the PDF with password" },
    { nameof(Text.Security_OpenWithPassword), "Open with password" },
    { nameof(Text.Security_SharePassword), "Use owner password" },
    { nameof(Text.Security_AllowPrint), "Allow printing" },
    { nameof(Text.Security_AllowCopy), "Allow copying text and images" },
    { nameof(Text.Security_AllowModify), "Allow inserting and removing pages" },
    { nameof(Text.Security_AllowAccessibility), "Allow using contents for accessibility" },
    { nameof(Text.Security_AllowForm), "Allow filling in forms" },
    { nameof(Text.Security_AllowAnnotation), "Allow creating and editing annotations" },

    // Insert window
    { nameof(Text.Insert_Window), "Insertion Details" },
    { nameof(Text.Insert_Menu_Add), "Add" },
    { nameof(Text.Insert_Menu_Up), "Up" },
    { nameof(Text.Insert_Menu_Down), "Down" },
    { nameof(Text.Insert_Menu_Remove), "Remove" },
    { nameof(Text.Insert_Menu_Clear), "Clear" },
    { nameof(Text.Insert_Menu_Preview), "Preview" },
    { nameof(Text.Insert_Position), "Insert position" },
    { nameof(Text.Insert_Position_Select), "Selected position" },
    { nameof(Text.Insert_Position_Head), "Beginning" },
    { nameof(Text.Insert_Position_Tail), "End" },
    { nameof(Text.Insert_Position_Custom), "Behind the number of" },
    { nameof(Text.Insert_Column_Filename), "Filename" },
    { nameof(Text.Insert_Column_Filetype), "Type" },
    { nameof(Text.Insert_Column_Filesize), "Filesize" },
    { nameof(Text.Insert_Column_LastWriteTime), "Last updated" },

    // Extract window
    { nameof(Text.Extract_Window), "Extraction Details" },
    { nameof(Text.Extract_Destination), "Save path" },
    { nameof(Text.Extract_Format), "Format" },
    { nameof(Text.Extract_Page), "Page count" },
    { nameof(Text.Extract_Target), "Target pages" },
    { nameof(Text.Extract_Target_Select), "Selected pages" },
    { nameof(Text.Extract_Target_All), "All pages" },
    { nameof(Text.Extract_Target_Custom), "Specified range" },
    { nameof(Text.Extract_Options), "Options" },
    { nameof(Text.Extract_Split), "Save as a separate file per page" },

    // Remove window
    { nameof(Text.Remove_Window), "Removal Details" },
    { nameof(Text.Remove_Page), "Page count" },
    { nameof(Text.Remove_Target), "Target pages" },

    // Password window

    // Titles for other dialogs
    { nameof(Text.Window_Open), "Open file" },
    { nameof(Text.Window_Save), "Save as" },
    { nameof(Text.Window_Backup), "Select a backup folder." },
    { nameof(Text.Window_Temp), "Select a temp folder. If you do not specify a temp folder, the same folder as the source files will be used." },
    { nameof(Text.Window_Preview), "{0} ({1}/{2} page)" },
    { nameof(Text.Window_Password), "Enter the owner password" },

    // Error messages
    { nameof(Text.Error_Open), "File is not in PDF format or corrupted." },
    { nameof(Text.Error_Metadata), "Failed to get PDF metadata." },
    { nameof(Text.Error_Range), "Failed to parse the removal range." },

    // Warning messages
    { nameof(Text.Warn_Password), "Please enter the owner password for opening and editing {0}." },
    { nameof(Text.Warn_Overwrite), "PDF is modified. Do you want to overwrite?" },

    // Other messages
    { nameof(Text.Message_Loading), "Loading {0} ..." },
    { nameof(Text.Message_Saving), "Saving as {0} ..." },
    { nameof(Text.Message_Saved), "Saved as {0}" },
    { nameof(Text.Message_Pages), "{0} pages" },
    { nameof(Text.Message_Total), "{0} pages" },
    { nameof(Text.Message_Selection), "{0} pages selected" },
    { nameof(Text.Message_Range), "e.g. 1,2,4-7,9" },
    { nameof(Text.Message_Byte), "Bytes" },
    { nameof(Text.Message_Dpi), "dpi" },

    // File filters
    { nameof(Text.Filter_All), "All files" },
    { nameof(Text.Filter_Insertable), "All insertable files" },
    { nameof(Text.Filter_Extractable), "All supported files" },
    { nameof(Text.Filter_Pdf), "PDF files" },
});
