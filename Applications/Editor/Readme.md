CubePDF Utility
====

Copyright © 2013 CubeSoft, Inc.  
GNU Affero General Public License version 3 (AGPLv3)  
support@cube-soft.jp  
https://www.cube-soft.com/cubepdfutility/

## Overview

CubePDF Utility is a PDF editor which can insert, remove, move, rotate pages, add or modify some metadata (PDF version, title, author, subject, keywords, creator, page layout), and encryption settings through GUI.

CubePDF Utility requires .NET Framework 3.5 or later (4.7 or later recommended).  
The download links are as follows:

* Download .NET Framework  
  https://dotnet.microsoft.com/download/dotnet-framework

## How to use

First, open a PDF file that you want to edit.
Note that input the owner password in the displayed dialog if the specified PDF file is encrypted.
Then make some changes in the main window and finally click the Save or Overwrite button.

The available operations in the CubePDF Utility are as follows:

1. Page editing
    - Insert
    - Remove
    - Extract
    - Change page order
    - Rotate
2. PDF metadata
    - Title
    - Author
    - Subject (Subtitle)
    - Keywords
    - Creator
    - PDF version
    - Page layout (a.k.a viewer preferences)
3. Security (Encryption)
    - Owner password
    - User password
    - Allow printing
    - Allow copying text and images
    - Allow inserting and removing pages
    - Allow using contents for accessibility
    - Allow filling in form fields
    - Allow creating and editing annotations

## Keyboard shortcuts

* Ctrl + O ... Open a PDF file
* Ctrl + S ... Overwrite the current PDF file
* Ctrl + Shift + S ... Save as a new file
* Ctrl + W ... Close the current PDF file
* Ctrl + Q ... Exit the application
* Ctrl + I or Insert ... Insert PDF/PNG/JPEG/BMP files at the selected position
* Ctrl + Shift + I ... Insert at other position
* Ctrl + D or Delete ... Remove the selected pages
* Ctrl + Shift + D ... Remove other pages
* Ctrl + E ... Extract the selected pages
* Ctrl + Shift + E ... Extract with other settings
* Ctrl + B ... Move the selected pages one page previous
* Ctrl + F ... Move the selected pages one page forward
* Ctrl + L ... Rotate the selected pages 90 degrees left
* Ctrl + R ... Rotate the selected pages 90 degrees right
* Ctrl + M ... Show the dialog to edit the PDF metadata
* Ctrl + K ... Show the dialog to edit the security (encryption)
* Ctrl + A ... Select all pages
* Ctrl + Z ... Undo
* Ctrl + Y ... Redo
* Ctrl + + ... Zoom in
* Ctrl + - ... Zoom out

## Support

CubePDF Utility outputs the log to the following directory.  
```C:\Users\(UserName *1)\AppData\Local\CubeSoft\CubePdfUtility2\Log```  
When you have some troubles, please contact support@cube-soft.jp along with these log files.

The application saves backup of edited files to the following directory during the past 30 days.  
```C:\Users\(UserName *1)\AppData\Local\CubeSoft\CubePdfUtility2\(Date *2)```  
When you lost files in some unexpected errors, please recover them from the backup.

*1 ... logged-on username  
*2 ... editing date

## Dependencies

Dependencies of the CubePDF Utility are as follows.

* iText (net47) or iTextSharp (net35)
    - GNU Affero General Public License
    - https://itextpdf.com/
    - https://www.nuget.org/packages/itext/
    - https://www.nuget.org/packages/iTextSharp/
* PDFium
    - 3-clause BSD License
    - https://pdfium.googlesource.com/pdfium/
    - https://github.com/cube-soft/Cube.Native.Pdfium
    - https://www.nuget.org/packages/Cube.Native.Pdfium.Lite/
* NLog
    - 3-clause BSD License
    - https://nlog-project.org/
    - https://www.nuget.org/packages/NLog/
* Fluent.Ribbon (net47)
    - MIT License
    - https://fluentribbon.github.io/
    - https://www.nuget.org/packages/Fluent.Ribbon/
* GongSolutions.WPF.DragDrop
    - 3-clause BSD License
    - https://github.com/punker76/gong-wpf-dragdrop
    - https://www.nuget.org/packages/gong-wpf-dragdrop/
* AsyncBridge (net35)
    - MIT License
    - https://omermor.github.io/AsyncBridge/
    - https://www.nuget.org/packages/AsyncBridge.Net35/

## History

* 2025-04-11 version 4.2.4
    - Update PDFium to Chromium 135 compatible.
* 2025-03-11 version 4.2.3
    - Update iText to 9.1.0.
    - Update PDFium to Chromium 134 compatible.
* 2025-02-17 version 4.2.2
    - Update PDFium to Chromium 133 compatible.
* 2025-01-16 version 4.2.1
    - Update PDFium to Chromium 132 compatible.
* 2024-12-03 version 4.2.0
    - Update iText to 9.0.0.
    - Update PDFium to Chromium 131 compatible.
* 2024-10-23 version 4.1.1
    - Update PDFium to Chromium 130 compatible.
* 2024-09-27 version 4.1.0
    - Update PDFium to Chromium 129 compatible.
    - Add Russian as a display language.
* 2024-08-23 version 4.0.2
    - Update PDFium to Chromium 128 compatible.
* 2024-07-29 version 4.0.1
    - Update iText to 8.0.5.
    - Update PDFium to Chromium 127 compatible.
* 2024-06-21 version 4.0.0
    - Improve processing of i18n support
    - Add Simplified Chinese as a display language.
    - Fix auto-sorting of selected files
    - Add setting for auto-sorting of selected files
    - Update iText to 8.0.4.
    - Update PDFium to Chromium 126 compatible.
* 2024-05-16 version 3.0.2
    - Update PDFium to Chromium 125 compatible.
* 2024-04-18 version 3.0.1
    - Update PDFium to Chromium 124 compatible.
* 2024-04-03 version 3.0.0
    - Fix issues with backup functionality.
    - Add settings for auto-deleting old backup files.
* 2024-03-22 version 2.6.2
    - Update PDFium to Chromium 123 compatible.
* 2024-02-22 version 2.6.1
    - Add an option to enable/disable automatic sorting of selected files.
    - Update iText to 8.0.3.
    - Update PDFium to Chromium 122 compatible.
* 2024-01-23 version 2.6.0
    - Improve I/O processing.
    - Update PDFium to Chromium 121 compatible.
* 2023-12-14 version 2.5.4
    - Update PDFium to Chromium 120 compatible.
* 2023-11-13 version 2.5.3
    - Fix to allow insertion of Tiff files.
    - Update iText to 8.0.2.
    - Update PDFium to Chromium 119 compatible.
* 2023-10-16 version 2.5.2
    - Update PDFium to Chromium 118 compatible.
* 2023-09-19 version 2.5.1
    - Update PDFium to Chromium 117 compatible.
* 2023-08-18 version 2.5.0
    - Fix sub-windows to be resizable.
    - Improve implementation for copying and moving files.
    - Update iText to 8.0.1.
    - Update PDFium to Chromium 116 compatible.
* 2023-07-19 version 2.4.1
    - Update PDFium to Chromium 115 compatible.
* 2023-06-15 version 2.4.0
    - Change the initial settings of temp directory to the system's default value.
    - Update iText to 8.0.0.
    - Update PDFium to Chromium 114 compatible.
    - Improve some implementations.
* 2023-05-10 version 2.3.0
    - Update PDFium to Chromium 113 compatible.
    - Improve some implementations.
* 2023-04-07 version 2.2.2
    - Update PDFium to Chromium 112 compatible.
* 2023-03-08 version 2.2.1
    - Update PDFium to Chromium 111 compatible.
* 2023-02-09 version 2.2.0
    - Add German as a display language. (Thanks Roy)
    - Adjust GUI layout.
    - Update PDFium to Chromium 110 compatible.
    - Update iText7 to 7.2.5.
* 2023-01-11 version 2.1.1
    - Update PDFium to Chromium 109 compatible.
* 2022-12-01 version 2.1.0
    - Update PDFium to Chromium 108 compatible.
    - Update iText7 to 7.2.4.
* 2022-11-01 version 2.0.5
    - Update PDFium to Chromium 107 compatible.
* 2022-09-30 version 2.0.4
    - Update PDFium to Chromium 106 compatible.
* 2022-08-31 version 2.0.3
    - Update PDFium to Chromium 105 compatible.
* 2022-08-04 version 2.0.2
    - Update PDFium to Chromium 104 compatible.
    - Update iText7 to 7.2.3.
* 2022-06-23 version 2.0.1
    - Update PDFium to Chromium 103 compatible.
    - Fix an issue that files were not inserted in the order specified in the "Insert at other position" dialog.
* 2022-06-01 version 2.0.0
    - Change recommended environment to .NET Framework 4.7 or later.
    - Update PDFium to Chromium 102 compatible.
    - Update iText7 to 7.2.2.
    - Add settings for deleting duplicate resources.
    - Add settings for bookmark information in the source PDF files.
    - Add settings for displaying recently used PDF files.
    - Add settings for the backup folder.
    - Add settings for the temporary folder.
    - Add function to insert PDF files at the end by Ctrl + Drag&Drop.
    - Change the order of files to be added when the file name contains numbers.
    - Remove splash window.
* 2022-05-10 version 1.6.7
    - Update PDFium to Chromium 101 compatible.
* 2022-04-01 version 1.6.6
    - Update PDFium to Chromium 100 compatible.
* 2022-03-01 version 1.6.5
    - Update PDFium to Chromium 99 compatible.
* 2022-02-07 version 1.6.4
    - Update PDFium to Chromium 98 compatible.
* 2022-01-07 version 1.6.3
    - Fix an issue that increases the file size unnecessarily when merging image files.
    - Update PDFium to Chromium 97 compatible.
    - Update iText7 to 7.1.17.
* 2021-11-19 version 1.6.2
    - Fix problems with thumbnails not being displayed when adding image files.
    - Update PDFium to Chromium 96 compatible.
* 2021-10-26 version 1.6.1
    - Fix to preserve nested outlines when merging PDFs.
    - Fix the update notification program.
    - Update PDFium to Chromium 95 compatible.
* 2021-09-24 version 1.6.0
    - Fix bugs that CubePDF Utility fails to start.
    - Fix behaviors when opening other than *.pdf.
    - Update PDFium to Chromium 94 compatible.
* 2021-08-31 version 1.5.2
    - Fix bugs that PDF pages cannot be Drag&Drop between windows.
    - Fix bugs that the undo process cannot be performed under certain conditions when moving with Drag&Drop.
    - Fix bugs that the file is not registered in the list of recently files when opened by Drag&Drop.
    - Update PDFium to Chromium 93 compatible.
* 2021-08-06 version 1.5.1
    - Add help menu.
    - Update PDFium to Chromium 92 compatible.
* 2021-07-09 version 1.5.0
    - Migrate to iText7 and refactor the implementation.
    - Migrate to Microsoft.Xaml.Behaviors.Wpf.
* 2021-06-09 version 1.0.2
    - Update PDFium to Chromium 91 compatible.
* 2021-04-27 version 1.0.1
    - Update PDFium to Chromium 90 compatible.
    - Update iTextSharp to 5.5.13.2.
* 2021-03-16 version 1.0.0
    - Update PDFium to Chromium 89 compatible.
* 2021-02-03 version 0.6.5β
    - Fix errors when launching the CubePDF utility with an encrypted PDF file.
    - Update PDFium to Chromium 88 compatible.
* 2020-12-01 version 0.6.4β
    - Update PDFium to Chromium 87 compatible.
* 2020-10-16 version 0.6.3β
    - Update PDFium to Chromium 86 compatible.
* 2020-09-08 version 0.6.2β
    - Update PDFium to Chromium 85 compatible.
* 2020-07-28 version 0.6.1β
    - Update PDFium to Chromium 84 compatible.
* 2020-06-09 version 0.6.0β
    - Fix to draw form fields.
    - Change log4net to NLog.
    - Update PDFium to Chromium 83 compatible.
* 2020-04-21 version 0.5.7β
    - Update PDFium to Chromium 81 compatible.
* 2020-04-07 version 0.5.6β
    - Update PDFium to Chromium 80 compatible.
* 2019-10-24 version 0.5.5β
    - Fix page order of the extracted PDF file.
    - Update PDFium to Chromium 77 compatible.
* 2019-08-02 version 0.5.4β
    - Add menu of "Extract with other settings".
    - Fix the problem that thumbnail images are ugly.
    - Fix to draw annotations in thumbnail images.
    - Improve memory consumption.
    - Update PDFium to Chromium 76 compatible.
* 2019-04-15 version 0.5.3β
    - Fix errors when showing icons that are associated with PDF files.
    - Fix a problem when selecting with Ctrl-click.
* 2018-12-10 version 0.5.2β
    - Fix a problem that encrypted PDF file cannot be opened in 32bit edition.
    - Fix the layout of the preview window.
    - Fix to display thumbnail images correctly in high DPI settings.
    - Fix to display the number of selected items in the status bar.
    - Fix to use associated file icon.
    - Add a keyboard shortcut of Ctrl+Q that exits the application.
* 2018-10-25 version 0.5.1β
    - Add menu of "Insert at other position".
    - Fix to insert PDF pages from another process by Drag&Drop operation.
    - Fix to insert image files (PNG, JPEG, BMP) as PDF pages.
    - Fix to confirm the owner password when opening All encrypted files.
    - Fix not to clear the selection when clicking the scrollbar.
* 2018-09-26 version 0.5.0β
    - Fix many features.
    - Fix to use PDFium as a rendering engine.
    - Add settings for selecting the display language (Auto, English, and Japanese).
* 2013-05-20 version 0.1.0β
    - First release version.
