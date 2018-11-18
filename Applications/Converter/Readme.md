# CubePDF

Copyright © 2010 CubeSoft, Inc.  
GNU Affero General Public License version 3 (AGPLv3)  
support@cube-soft.jp  
https://www.cube-soft.jp/cubepdf/

## Overview

CubePDF is a PDF converter which allows you to convert files from any applications
(for example, Google Chrome, Firefox, Microsoft Edge, Microsoft Word, Excel,
PowerPoint, and more), whenever you need it.
The converter allows you to convert the files as easy as you can print the files;
as a matter of fact, you can do it in the same manner as you print files. 

CubePDF requires Microsoft .NET Framework 3.5 or more.
The download link of Microsoft .NET Framework 3.5 is as follows:  
https://www.microsoft.com/en-us/download/details.aspx?id=22

## Support

CubePDF puts logs to the following files.
When you have some troubles, please contact support@cube-soft.jp along with these log files.

* C:\ProgramData\CubeSoft\CubePdf\Log\CubePinstaller.log
* C:\ProgramData\CubeSoft\CubePdf\Log\CubePdf.log
* C:\ProgramData\CubeSoft\CubePdf\Log\CubeProxy.log

## Dependencies

Dependencies of the CubePDF is as follows.

* GPL Ghostscript
    - GNU Affero General Public License
    - http://www.ghostscript.com/
* iTextSharp
    - GNU Affero General Public License
    - http://sourceforge.net/projects/itextsharp/
    - https://www.nuget.org/packages/iTextSharp/
* log4net
    - Apache License, Version 2.0
    - http://logging.apache.org/log4net/
    - https://www.nuget.org/packages/log4net/
* AlphaFS
    - MIT License
    - http://alphafs.alphaleonis.com/
    - https://www.nuget.org/packages/AlphaFS/

## History

* 2018/11/22 version 1.0.0RC16
    - Fix to apply for the specified resolution to embedded PDF images.
    - Fix the initial directory of the save file dialog.
    - Fix to install and uninstall the CubePDF printer.
    - Fix to show messages of the installer in English or Japanese.
* 2018/10/01 version 1.0.0RC15
    - Update Ghostscript 9.25.
* 2018/09/12 version 1.0.0RC14
    - Update Ghostscript 9.24.
    - Fix to load user settings.
* 2018/08/27 version 1.0.0RC13
    - Fix to confirm checksum of the source file.
    - Fix to activate the main window at launching.
* 2018/06/26 version 1.0.0RC12
    - Add settings for selecting the display language (Auto, English, and Japanese).
    - Add the page layout menu (in PDF metadata tab).
    - Fix layout of the main window.
    - Fix the Creator menu (in PDF metadata tab) as editable.
* 2010/07/07 version 0.9.0β
    - First release version.
