CubePDF Page
====

Copyright © 2013 CubeSoft, Inc.  
GNU Affero General Public License version 3 (AGPLv3)  
support@cube-soft.jp  
https://www.cube-soft.com/cubepdfpage/

## Overview

CubePDF Page is a software for merging and splitting PDF files and image files (BMP, JPEG, PNG, GIF, TIFF).

CubePDF Page requires .NET Framework 3.5 or later (4.7 or later recommended).  
The download links are as follows:

* Download .NET Framework  
  https://dotnet.microsoft.com/download/dotnet-framework


## How to use

Add files to be merged or split to the file list by using the Add button or drag&drop.
Then press the Merge or Split button and a dialog box will appear to specify where to save the file.

When merging PDF files, they are merged from the top of the file list.
The order in which files are displayed in the file list can be changed by clicking the Up or Down button after the file is added.

When the Split button is pressed, CubePDF Page will split all pages of the added PDF file into separate files, one page at a time.
If you want to extract only a specific group of pages, split all pages once and then re-merge only the pages you need.

## Keyboard shortcuts

* Ctrl + M ... Merge PDF files
* Ctrl + S ... Split PDF files
* Ctrl + E ... Display PDF metadata and encryption settings window
* Ctrl + H ... Display CubePDF Page settings window
* Ctrl + Q ... Exit the application
* Ctrl + O ... Display dialog to add PDF, PNG, JPEG, BMP files
* Ctrl + A ... Select all items in the file list
* Ctrl + R ... Open the selected file in the associated application
* Ctrl + K or Ctrl + Up ... Move selected files down up level
* Ctrl + J or Ctrl + Down ... Move selected files down one level
* Ctrl + D or Delete ... Delete selected files from the list
* Ctrl + Shift + D ... Delete all files from the list

## Support

CubePDF Page outputs the log to the following directory.  
```C:\Users\%UserName%\AppData\Local\CubeSoft\CubePdfPage\Log```  
When you have some troubles, please contact support@cube-soft.jp along with these log files.

## Dependencies

Dependencies of the CubePDF Page are as follows.

* iText (net47) or iTextSharp (net35)
    - GNU Affero General Public License
    - https://itextpdf.com/
    - https://www.nuget.org/packages/itext/
    - https://www.nuget.org/packages/iTextSharp/
* NLog
    - 3-clause BSD License
    - https://nlog-project.org/
    - https://www.nuget.org/packages/NLog/
* AsyncBridge (net35)
    - MIT License
    - https://omermor.github.io/AsyncBridge/
    - https://www.nuget.org/packages/AsyncBridge.Net35/

## History

* 2024-07-31 version 5.0.1
    - Update iText to 8.0.5.
* 2024-06-27 version 5.0.0
    - Improve processing of i18n support
    - Add Simplified Chinese as a display language.
    - Fix auto-sorting of selected files
    - Add setting for auto-sorting of selected files
    - Update iText to 8.0.4.
* 2024-02-21 version 4.4.0
    - Improve I/O processing.
    - Add an option to enable/disable automatic sorting of selected files.
    - Update iText to 8.0.3.
* 2023-11-01 version 4.3.2
    - Update iText to 8.0.2.
* 2023-08-21 version 4.3.1
    - Update iText to 8.0.1.
    - Improve some implementations.
* 2023-06-06 version 4.3.0
    - Change the initial settings of temp directory to the system's default value.
    - Update iText to 8.0.0.
    - Improve some implementations.
* 2023-02-09 version 4.2.0
    - Add German as a display language. (Thanks Roy)
    - Adjust GUI layout.
    - Fix to display the CubePDF Page document page when pressing F1 key.
    - Update iText7 to 7.2.5.
* 2022-12-01 version 4.1.0
    - Update iText7 to 7.2.4.
* 2022-07-11 version 4.0.3
    - Update iText7 to 7.2.3.
* 2022-04-18 version 4.0.2
    - Update iText7 to 7.2.2.
* 2022-03-30 version 4.0.1
    - Fix an error when specifying a file containing full-width numbers.
* 2022-03-25 version 4.0.0
    - Change recommended environment to .NET Framework 4.7 or later.
    - Update iText7 to 7.2.1.
    - Add window for PDF metadata and encryption settings.
    - Add settings for deleting duplicate resources.
    - Add settings for bookmark information in the source PDF files.
    - Add settings for the temporary folder.
    - Add settings for display language on main window.
    - Change the order of files to be added when the file name contains numbers.
* 2022-01-07 version 3.6.1
    - Fix an issue that increases the file size unnecessarily when merging image files.
    - Update iText7 to 7.1.17.
* 2021-10-26 version 3.6.0
    - Fix to preserve nested outlines when merging PDFs.
    - Fix the update notification program.
* 2021-07-09 version 3.5.0
    - Migrate to iText7 and refactor the implementation.
* 2021-05-21 version 3.1.1
    - Fix an issue when multiple items were selected and the Up/Down buttons were pressed.
    - Update iTextSharp to 5.5.13.2.
* 2021-01-22 version 3.1.0
    - Fix the order when multiple files and folders are selected and added.
    - Fix to allow additional files to be specified as program arguments.
* 2020-11-13 version 3.0.0
    - Follow CubePDF Utility in implementation of merging and splitting process.
    - Fix not to display a message after processing is completed.
    - Change log4net to NLog.
* 2016-04-05 version 2.0.1
    - Fix an issue that caused the bookmarks to shift position.
    - Fix an issue that caused overwrite save to fail.
* 2015-12-28 version 2.0.0
    - Support for merging image files
    - Follow CubePDF Utility in implementation of merging and splitting process.
    - Fix GUI features.
* 2013-02-25 version 1.0.0
    - First release version.
