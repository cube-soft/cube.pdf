CubePDF Page FAQ
====

Copyright © 2013 CubeSoft, Inc.  
GNU Affero General Public License version 3 (AGPLv3)  
support@cube-soft.jp  
https://www.cube-soft.com/cubepdfpage/

### Introduction

This article contains Frequently Asked Questions (FAQ) about usage of [CubePDF Page](https://www.cube-soft.com/cubepdfpage/). For more information on using CubePDF Page, please refer to [CubePDF Page Documents](https://en.cube-soft.jp/entry/cubepdf-page).

### Work differently than before

CubeSoft, Inc. provides the following software under the name of **CubePDF Series**.

* [CubePDF](https://www.cube-soft.com/cubepdf/) ... Virtual Printer
* [CubePDF Utility](https://www.cube-soft.com/cubepdfutility/) ... PDF page based editor
* [CubePDF Page](https://www.cube-soft.com/cubepdfpage/) ... PDF file based editor
* [CubePDF Clip](https://clown.cube-soft.jp/entry/2017/03/24/cubepdf-clip-1.0.0) ... Software for attaching to PDF files (Japanese)
* [CubePDF Viewer](https://www.cube-soft.com/cubepdfviewer/) ... PDF viewer (Japanese)
* [CubePDF ImagePicker](https://www.cube-soft.com/cubepdfimagepicker/) ... PDF embedded image picker (Japanese)

**These are all different software**. First, please make sure that the software installed in your environment is appropriate for your use. In addition, the functions provided and the layout of the GUI may be changed as a result of version updates.

### What version of Windows does it work on?

CubePDF Page works only on Windows versions within the valid period. There is no guarantee that CubePDF Page will work on Windows with expired support, and no fix will be made just for it if it stops working.

As for Windows 10 and Windows 11, considering the framework and libraries CubePDF Page uses, we believe it is unlikely that there will be a situation where CubePDF Page will not work on a specific version. For this reason, we will only announce when there is a specific problem, and in other cases, we do not plan to specify whether or not each version will work. In the end, please install the software yourself to determine whether or not it works and whether or not you can use it.

As for Windows Server, for the same reason, we expect it to work basically without problems. In addition, we have received many reports that it actually works. However, due to the fact that we are not able to conduct sufficient testing in the development environment, we may not be able to verify or solve problems specific to some server editions when they occur. Please be aware of this.

### Are any runtimes, frameworks, etc. required?

.NET Framework 3.5 or later (4.7 or later is strongly recommended) is required to use CubePDF Page as it is developed using it. .NET Framework should already be installed in most cases, but if you need to install it, you can download it from [Download .NET Framework](https://dotnet.microsoft.com/download/dotnet-framework).

Note that the executable file to be installed for CubePDF Page differs depending on the installed version of .NET Framework. Therefore, if you update the .NET Framework version, it is recommended to reinstall the CubePDF Page as well.

### How do I update my version?

To update CubePDF Page, download the latest version of the installer from the [CubePDF Page download page](https://www.cube-soft.com/cubepdfpage/) and run it again, as you did when you first installed it.

### What are the install options?

The CubePDF Page installer is built using a framework called [Inno Setup](http://www.jrsoftware.org/isinfo.php). For a list of the installation options provided by Inno Setup, see [Setup Command Line Parameters](http://www.jrsoftware.org/ishelp/index.php?topic=setupcmdline).

### Does network communication occur during execution?

CubePDF Page itself does not intentionally perform network communication at runtime; the only network communication related to CubePDF Page is the update check performed at PC startup. In the update check, the version numbers of CubePDF Page, Windows and .NET are sent. If you want to disable the update check, please disable the **Check for updates on startup** item in the setting dialog box.

### Problems occurred while using CubePDF Page.

CubePDF Page outputs logs in the ```C:\Users\%UserName%\AppData\Local\CubeSoft\CubePdfPage\Log``` folder. If you encounter any problems, please contact us at support@cube-soft.jp with these logs attached (Replace %UserName% with the name of the user currently logged on).
