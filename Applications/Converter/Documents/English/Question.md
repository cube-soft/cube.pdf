CubePDF FAQ
====

Copyright © 2010 CubeSoft, Inc.  
GNU Affero General Public License version 3 (AGPLv3)  
support@cube-soft.jp  
https://www.cube-soft.jp/cubepdf/

This article contains Frequently Asked Questions (FAQ) about usage of [CubePDF](https://www.cube-soft.jp/cubepdf/?lang=en). For more information on using CubePDF, please refer to [CubePDF Documents](https://en.cube-soft.jp/entry/cubepdf).

### I don't know how to use CubePDF.

![How to use CubePDF](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdf/overview.en.png)

CubePDF is a software called **Virtual Printer**. First, open the content you want to convert to PDF in a suitable application such as Microsoft Edge, Google Chrome, Microsoft Word, Excel, PowerPoint, etc. Then, select the **Print** menu of those applications and select CubePDF from the list of printers to run it. Finally, make the necessary settings on the CubePDF main window and click the **Convert** button to finish.

### I cannot find the CubePDF icon on my desktop

CubePDF is a software called **Virtual Printer**, so you cannot run CubePDF directly from the desktop or the start menu, or associate it with a PDF file, etc., as you can with general applications. Therefore, no shortcut (icon) of CubePDF will be created on the desktop.

### Work differently than before

CubeSoft, Inc. provides the following software under the name of **CubePDF Series**.

* [CubePDF](https://www.cube-soft.jp/cubepdf/?lang=en) ... Virtual Printer
* [CubePDF Utility](https://www.cube-soft.jp/cubepdfutility/?lang=en) ... PDF page based editor
* [CubePDF Page](https://www.cube-soft.jp/cubepdfpage/) ... PDF file based editor (Japanese)
* [CubePDF Clip](https://clown.cube-soft.jp/entry/2017/03/24/cubepdf-clip-1.0.0) ... Software for attaching to PDF files (Japanese)
* [CubePDF Viewer](https://www.cube-soft.jp/cubepdfviewer/) ... PDF viewer (Japanese)
* [CubePDF ImagePicker](https://www.cube-soft.jp/cubepdfimagepicker/) ... PDF embedded image picker (Japanese)

**These are all different software**. First, please make sure that the software installed in your environment is appropriate for your use. In addition, the functions provided and the layout of the GUI may be changed as a result of version updates.

### Unable to open the PDF file

CubePDF is a software that provides only the function of conversion to PDF files. Therefore, you will need an application to view the converted file. For example, you can use [Adobe Acrobat Reader DC](https://get.adobe.com/jp/reader/) as an application to view PDF files.

![PostProcess to None](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdf/doc/v1/en/faq-postprocess.png)

Also, if no application to open the converted file is installed on your PC, you may encounter an error when running CubePDF. In this case, please change the **Post process** item on the main window to **None** to avoid this problem.

### Filesize of PDF files suddenly increased

![Settings for embedded image compression](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdf/doc/v1/en/faq-filesize.png)

For conversion to PDF files, even if the content looks the same, the file size may vary greatly depending on the settings of the application executing the printing and minor differences in the content itself.

However, if there is a large difference in the file size of the converted PDF file despite printing the same content from the same application, it is expected that the difference is most likely due to whether or not the images were compressed to JPEG format in CubePDF. In this case, please check if the **Compress PDF images as JPEG** item in the **Others** tab of the main window is enabled.

### Fail to merge PDF files

![In case the PDF file is encrypted by a password](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdf/doc/v1/en/faq-security-01.png)

If the merging of PDF files fails, one of the possible causes is that the destination PDF file is encrypted by a password. For example, if you see the word **Protected** when you open a PDF file with Adobe Acrobat Reader DC, the PDF file is encrypted. In this case, you need to enter the **Owner Password** of the PDF file to be merged in the **Encryption** tab of the CubePDF main window.

![In case the PDF file is opened in another application](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdf/doc/v1/en/faq-security-02.png)

Also, if you receive the error message "The process cannot access the file because the file is being used by another process" as described above, it is expected that the PDF file to be merged is most likely being opened by some application. In this case, please check that you have not opened the PDF file.

### Can I edit a PDF file?

CubePDF is basically a software for converting PDF files, although you can merge them with other PDF files and set document properties, passwords, etc. during conversion. If you want to edit an existing PDF file in page units such as merging, splitting, rotating, etc., you can use [CubePDF Utility](https://www.cube-soft.jp/cubepdfutility/) or [CubePDF Page (Japanese)](https://www.cube-soft.jp/cubepdfpage/).

### Does network communication occur during execution?

CubePDF itself does not intentionally perform network communication at runtime; the only network communication related to CubePDF is the update check performed at PC startup. In the update check, the version numbers of CubePDF, Windows and .NET are sent. If you want to disable the update check, please disable the **Check for updates on startup** item in the **Others** tab of the main window and click the **Save settings** button at the bottom left.

![How to disable update checker](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdf/doc/v1/en/faq-network.png)

Also, please refer to [About communication that occurs when running CubePDF (Japanese)](https://clown.cube-soft.jp/entry/2011/10/26/upnp) for the problems we have received in the past. We recognize that this is done by Windows itself and cannot be controlled by CubePDF.

### What version of Windows does it work on?

CubePDF works only on Windows versions within the valid period.  There is no guarantee that CubePDF will work on Windows with expired support, and no fix will be made just for it if it stops working.

As for Windows 10 and Windows 11, considering the framework and libraries CubePDF uses, we believe it is unlikely that there will be a situation where CubePDF will not work on a specific version. For this reason, we will only announce when there is a specific problem, and in other cases, we do not plan to specify whether or not each version will work. In the end, please install the software yourself to determine whether or not it works and whether or not you can use it.

As for Windows Server, for the same reason, we expect it to work basically without problems. In addition, we have received many reports that it actually works. However, due to the fact that we are not able to conduct sufficient testing in the development environment, we may not be able to verify or solve problems specific to some server editions when they occur. Please be aware of this.


### Are any runtimes, frameworks, etc. required?

.NET Framework 3.5 or later (4.5.2 or later is strongly recommended) is required to use CubePDF as it is developed using it. .NET Framework should already be installed in most cases, but if you need to install it, you can download it from [Download .NET Framework](https://dotnet.microsoft.com/download/dotnet-framework).

Also, CubePDF uses PScript5, which is a standard Windows printer driver, to build a virtual printer. Therefore, if the corresponding modules (pscript5.dll and ps5ui.dll) do not exist on your PC, the installation will fail. If the installation fails, please contact us at support@cube-soft.jp with ``C:\ProgramData\CubeSoft\CubePDF\Log\CubeVpc.log`` as an attachment.

### How do I update my version?

To update CubePDF, download the latest version of the installer from the [CubePDF download page](https://www.cube-soft.jp/cubepdf/?lang=en) and run it again, as you did when you first installed it.

### What are the install options?

The CubePDF installer is built using a framework called [Inno Setup](http://www.jrsoftware.org/isinfo.php). For a list of the installation options provided by Inno Setup, see [Setup For a list of installation options provided by Inno Setup, see [Setup Command Line Parameters](http://www.jrsoftware.org/ishelp/index.php?topic=setupcmdline).

### CubePDF printer does not appear in the list

CubePDF outputs installation and execution logs in ```C:\ProgramData\CubeSoft\CubePdf\Log`` folder. If you encounter any problems, please contact us at support@cube-soft.jp with these logs attached. Also, if you encounter any errors while using CubePDF, please attach the logs as well.

### Can I hide the GUI and automate the conversion?

No, there is no setting to hide the CubePDF main window. However, [CubeVP (Japanese)](https://www.cube-soft.jp/cubevp/) allows users to create more flexible custom virtual printers by programming, including hiding the main window. CubeVP can be used free for personal use, so please consider it as well. In addition, [Tutorial (Japanese)](https://clown.cube-soft.jp/entry/cubevp/tutorial) describes how to register a virtual printer to run CubePDF without the main window.