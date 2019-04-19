CubePDF
====

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

CubePDF requires .NET Framework 3.5 or later (4.5.2 or later recommended).  
The download links are as follows:

* Download .NET Framework  
  https://dotnet.microsoft.com/download/dotnet-framework

## Support

CubePDF outputs the log to the following directory.  
```C:\ProgramData\CubeSoft\CubePdf\Log```  
When you have some troubles, please contact support@cube-soft.jp along with these log files.

## Dependencies

Dependencies of the CubePDF are as follows.

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

* 2019/03/14 version 1.0.0RC19
    - Fix to install and uninstall the CubePDF printer.
* 2019/02/15 version 1.0.0RC18
    - Fix to apply for the Metadata as the target of the saving settings function.
    - Add an option to set the initial directory when showing the saving file dialog.
    - Fix to install and uninstall the CubePDF printer.
* 2018/12/04 version 1.0.0RC17
    - Fix to use the CreateProcessAsUserW function.
    - Fix to install and uninstall the CubePDF printer.
    - Update Ghostscript 9.26.
* 2018/11/22 version 1.0.0RC16
    - Fix to apply for the specified resolution to embedded PDF images.
    - Fix the initial directory of the saving file dialog.
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
* 2017/05/15 version 1.0.0RC11
    - Fix an error when executing the CubePDF via remote desktop.
    - Fix language settings of the main window.
* 2017/02/20 version 1.0.0RC10
    - Fix an error where the main window is not displayed in some cases.
* 2017/01/27 version 1.0.0RC9
    - Fix an error that CubePDF cannot convert files via Windows store applications.
    - Fix to show an error message when you restrict some operations and omit the user password.
    - Fix to display menus and messages in English or Japanese.
    - Fix layout of the main window.
    - Fix the method of outputing log.
* 2015/09/24 version 1.0.0RC8
    - Fix an error temporarily that CubePDF cannot convert files via Windows store applications.
    - Fix an error that CubePDF cannot convert files when CJK characters are mixed in the username.
    - Fix an error that CubePDF forcibly sets the paper size to A4 when the fast Web view option is enabled.
    - Fix an error that CubePDF forcibly sets the PDF version to 1.5 or more.
* 2014/05/12 version 1.0.0RC7
    - Remove the SVG from available formats.
    - Fix settings about the downsampling.
    - Add an option of the page orientation.
    - Fix to choose the encryption method corresponding to the PDF version.
    - Fix to use smart copy when merging PDF files.
    - Fix to convert files in temporary directory.
    - Fix an option of the post process.
    - Fix the initial value of output filename.
    - Fix to show system information (Windows and .NET Framework).
    - Fix layout of the main window.
    - Fix the list of paper sizes.
* 2013/08/13 version 1.0.0RC6
    - Fix to refresh automatically when saving a converted file to the desktop.
* 2013/07/09 version 1.0.0RC5
    - Change license from GPLv3 to AGPLv3 according to the Ghostscript.
    - Fix text conversion method so that text display does not become unnatural.
    - Fix to show dialog when the saving file is used by another process.
    - Fix to skip checking the signature when launching the CubePDF.
    - Fix to prevent users from entering invalid characters.
    - Fix the method of checking version update.
* 2012/09/21 version 1.0.0RC4
    - Add the "Save settings" button.
    - Fix an error when installing CubePDF in the Windows XP (x64).
    - Fix to remove the update checker when uninstalling the CubePDF.
    - Add an option of the waiting for exit.
    - Fix the method of saving output path settings.
* 2012/05/25 version 1.0.0RC3
    - Fix not to wait for the termination of CubePDF.
    - Fix garbled characters when multi-byte characters (except for Japanese) is setting.
    - Fix not to be able to press the button during conversion.
    - Fix to be able to input the path of user program.
    - Fix layout of the main window in multiple DPIs.
    - Fix to check whether the file is associated when opening the converted file.
* 2012/03/28 version 1.0.0RC2
    - Merge the function of cubepdf-redirect.exe to cubepdf.exe.
    - Fix layout of the "Security" tab.
    - Fix to add the extension automatically when inputting the output path.
    - Fix to merge the converted file to the encrypted PDF file.
    - Fix to prevent the PDF file from being damaged when failing to merge PDF files.
* 2012/01/31 version 1.0.0RC1
    - Fix the port monitor.
    - Fix an option to compress embedded images as JPEG format.
    - Fix to prevent users not to input invalid characters.
* 2010/07/07 version 0.9.0β
    - First release version.
