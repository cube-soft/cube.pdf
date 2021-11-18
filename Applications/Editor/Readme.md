CubePDF Utility
====

Copyright © 2013 CubeSoft, Inc.  
GNU Affero General Public License version 3 (AGPLv3)  
support@cube-soft.jp  
https://www.cube-soft.jp/cubepdfutility/

## Overview

CubePDF Utility is a PDF editor which can insert, remove, move, rotate pages, add or modify some metadata (PDF version, title, author, subject, keywords, creator, page layout), and encryption settings through GUI.

CubePDF Utility requires .NET Framework 3.5 or later (4.5.2 or later recommended).  
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

* iText7 (net45) or iTextSharp (net35)
    - GNU Affero General Public License
    - https://itextpdf.com/
    - https://www.nuget.org/packages/itext7/
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
* Fluent.Ribbon (net45)
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
