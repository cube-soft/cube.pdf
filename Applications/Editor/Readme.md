CubePDF Utility
====

Copyright © 2010 CubeSoft, Inc.  
GNU Affero General Public License version 3 (AGPLv3)  
support@cube-soft.jp  
https://www.cube-soft.jp/cubepdfutility/

## Overview

CubePDF Utility is a PDF editor which can insert, remove, move, rotate pages, add or modify some metadata (PDF version, title, author, subject, keywords, creator, page layout), and encryption settings through the graphical user interface (GUI).

CubePDF Utility requires Microsoft .NET Framework 3.5 or more (4.5.2 or more recommended).
The download link is as follows:  

* Microsoft .NET Framework 4.5.2  
  https://www.microsoft.com/ja-JP/download/details.aspx?id=42643
* Microsoft .NET Framework 3.5  
  https://www.microsoft.com/ja-jp/download/details.aspx?id=22

## Support

CubePDF Utility outputs the log to the following directory.
When you have some troubles, please contact support@cube-soft.jp along with these log files.

C:\Users\\(UserName *1)\AppData\Local\CubeSoft\CubePdfUtility2\Log

The application saves backup of edited files to the following directory during the past 30 days.
When you lost files in some unexpected errors, please recover them from the backup.

C:\Users\\(UserName *1)\AppData\Local\CubeSoft\CubePdfUtility2\\(Date *2)

*1 <UserName> ... log-on username  
*2 <Date> ... date of editting the file

## Dependencies

Dependencies of the CubePDF Utility are as follows.

* iTextSharp
    - GNU Affero General Public License
    - https://itextpdf.com/
    - https://www.nuget.org/packages/iTextSharp/
* PDFium
    - 3-clause BSD License
    - https://pdfium.googlesource.com/pdfium/
    - https://www.nuget.org/packages/PdfiumViewer.Native.x86.no_v8-no_xfa/
    - https://www.nuget.org/packages/PdfiumViewer.Native.x86_64.no_v8-no_xfa/
* log4net
    - Apache License, Version 2.0
    - http://logging.apache.org/log4net/
    - https://www.nuget.org/packages/log4net/
* MVVM Light Toolkit
    - MIT License
    - http://www.galasoft.ch/mvvm
    - https://www.nuget.org/packages/MvvmLight/
* AsyncBridge (.NET Framework 3.5)
    - MIT License
    - http://omermor.github.io/AsyncBridge/
    - https://www.nuget.org/packages/AsyncBridge.Net35

## History

* 2018/10/25 version 0.5.1β
    - Add menu of "Insertion details".
    - Fix to insert PDF pages from another process by Drag&Drop operation.
    - Fix to insert image files (PNG, JPEG, BMP) as PDF pages.
    - Fix to confirm the owner password when opening All encrypted files.
    - Fix not to clear the selection when clicking the scrollbar.
* 2018/09/26 version 0.5.0β
    - Fix many features.
    - Fix to use PDFium as a rendering engine.
    - Add settings for selecting the display language (Auto, English, and Japanese).
* 2013/05/20 version 0.1.0β
    - First release version.
