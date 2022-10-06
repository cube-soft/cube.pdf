CubePDF Documents
====

Copyright © 2010 CubeSoft, Inc.  
GNU Affero General Public License version 3 (AGPLv3)  
support@cube-soft.jp  
https://www.cube-soft.jp/cubepdf/

## Overview

![How to convert to PDF](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdf/doc/v3/en/overview.png)

CubePDF is installed as a **Virtual Printer**. Therefore, any application that has a **Print** button, such as Microsoft Edge, Google Chrome, Microsoft Word, Excel, PowerPoint, etc., can be converted to PDF in the following 3 steps.

1. Display the file you want to convert to PDF in an appropriate application and select **Print**.
2. Select **CubePDF** from the list of available printers and click the **Print** button.
3. Confirm the location of the file on the main window of CubePDF, and click the **Convert** button.

If a PDF file with the specified name already exists, you can merge it into an existing PDF file. Existing PDF files can be processed in the following four ways.

* **Overwrite**  
  Overwrite an existing PDF file with a new PDF file.
* **Merge head**  
  Merge the converted content into the beginning of an existing PDF file.
* **Merge tail**  
  Merge the converted content at the end of an existing PDF file.
* **Rename**   
  Save the file automatically under a different name, such as sample(2).pdf.

Note that if the destination PDF file is protected by a password, it can only be merged if the same owner password is set in the security tab.

### Attention

CubePDF is a software called **Virtual Printer**, which runs via **Printing** in other applications. Therefore, you cannot run CubePDF directly from the desktop or start menu, or associate it with a PDF file, etc., as you can with regular applications.

In addition, CubePDF only provides the conversion function, and another application is required to view the converted files (for example, [Adobe Acrobat Reader DC](https://get.adobe.com/reader/), etc.).

## CubePDF application settings

### General

CubePDF can also convert to file formats other than PDF. If you want to convert to a file format other than PDF, please select the file type you want to convert from the **Format** selection.

Note that for file formats that cannot hold multiple pages, such as PNG, a file will be created for each page to be converted. For example, if you specify Sample.png as the output file name, the file will actually be created as Sample-01.png, Sample-02.png, and so on.

![General settings](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdf/doc/v2/en/general.png)

If the selected format is PDF, you can select the PDF version, which can be found to the right of the **Format** selection.

The **ColorMode** item allows you to specify the color of the converted fonts, shapes, and embedded images. The items that can be set are as follows:

* **Auto**  
  Preserves the colors of the print data as they are.
* **RGB**  
  Converts to RGB. This setting is similar to **Auto** in most cases.
* **Grayscale**  
  Converts to grayscale.
* **Monochrome**  
  Converts to monochrome (black and white).
  Note that the monochrome conversion is supported only for PNG, BMP, and TIFF bitmap image formats.
  For other file formats, you can select the monochrome item, but it will actually be converted as **Grayscale**.

The **Resolution** item is mainly used for file size compression (down-sampling). Therefore, even if you set a higher resolution than the original data, the quality and file size may not change. Also, if the selected format is PDF, PS, or EPS, only embedded images are affected by the settings.

The **Orientation** item allows you to set the orientation of the converted file. The items that can be set are as follows:

* **Portrait**, **Landscape**  
  Aligns the orientation of all pages vertically or horizontally.
* **Auto**  
  Preserves the orientation of each page in the original file. If the selected format is other than PDF, the behavior will be the same as when **Portrait** is selected.

Other options for conversion are as follows:

* **Compress PDF images as JPEG**  
  Compress the embedded images into JPEG format when the conversion process is executed.
* **Optimize PDF for fast Web view**  
  When viewing a PDF file on the Web, you usually have to wait until all the data has been downloaded. If you enable this option, it will be optimized in such a way that you can view the part that has been downloaded first. This is a specification called Linearized PDF. However, this option cannot be applied to an encrypted PDF file. CubePDF will ignore this option if any security settings are enabled.

The **Post process** item allows you to set the operation to be performed after the conversion by CubePDF is finished. The items that can be set are as follows:

* **Open**  
  Open the converted file in its associated application.
* **Open directory**  
  Open the folder where you saved the converted file.
* **None**  
  Exit without doing anything.
* **Others**  
  Execute the specified program.

Note that if you specify any other program, CubePDF will execute it with the path of the converted file as an argument.

### Metadata

![Metadata](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdf/doc/v2/en/metadata.png)

If the selected format is PDF, you can register information such as the title, creator, etc. The information registered here can be confirmed in the properties window of a PDF viewer such as Adobe Acrobat Reader DC. In addition, the **Layout** item allows you to change the way the PDF is displayed when it is opened with a PDF viewer.

If you omit these information, CubePDF will create PDF files with Title, Author, Subtitle, Keywords left blank and Creator set to "CubePDF".

### Encryption

If the selected format is PDF, you can set a password to protect the created PDF file. To set a password, first enable the **Encrypt the PDF with password** option, then enter the same password twice in the **Password** and **Confirm** fields.

![Encryption](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdf/doc/v2/en/encryption.png)

Next, in the **Operations** section, specify the operations to be allowed or restricted to users. Available settings are as follows:

* Open with password
* Allow printing
* Allow copying text and images
* Allow inserting and removing pages
* Allow using contents for accessibility
* Allow filling in forms
* Allow creating adn editing annotations

Note that if you enable the item **Open with password** and also enable the item **Use owner password**, CubePDF will set the user password to the same password as the owner password.

However, if you share both passwords, the restrictions on printing, copying, etc. may not work properly depending on the PDF viewer. This is probably because the PDF viewer recognizes that the PDF file was opened with the owner password. For this reason, CubePDF is designed not to accept permission settings when it is shared with the owner password.

Moreover, if a PDF file is recognized as having been opened with an owner password, all PDF editing, including removal of the user password, will be possible. Please make sure you fully understand these behaviors when you share the user password with the owner password.

### Other settings

The **About** section displays the version information of CubePDF. If you enable the **Check for updates on startup** item, you will receive a notification in the lower right corner of your computer when CubePDF is updated. Checking for updates will be performed when your computer starts up. The information sent for checking the update is the version number of CubePDF, Windows, and .NET.

![Language](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdf/doc/v2/en/version.png)

In the **Language** section, you can set the display language for the menu and other items on the CubePDF main window. There are two supported languages, **English** and **Japanese**. If you select **Auto** for the display language, one of the languages will be automatically selected according to the Windows language settings.

### Save settings

When you change any item in the CubePDF main window, the **Save settings** button will become clickable. If you click it at this time, the current settings will be saved, and will be used as the initial settings for the next time CubePDF is executed.

For example, CubePDF will save the converted files on the desktop by default, but if you click the **Save settings** button after specifying a different folder as the destination, CubePDF will use the specified folder as the default setting for the saving folder from next time on.

However, various security items and the file name part of the destination are not covered by the function to save settings.

## CubePDF printer settings

CubePDF has settings for the **CubePDF printer** in addition to the application settings described above. To change the printer settings, first right-click CubePDF on the window that appears in **Devices and Printers** in **Control Panel** and select **Print preferences**.

![Printer settings in the control pannel](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdf/doc/v2/en/printer-01.png)

You can also do the same from **Settings** in Windows 8 or later. From **Settings**, select **Devices**, **Printers & scanners**, **CubePDF**, **Manage**, and **Print preferences**.

![Printer settings for Windows 8 and later](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdf/doc/v2/en/printer-02.png)

In the print settings, the **Paper/Quality** tab allows you to change the settings for printing in black and white or color. For other settings, click the **Advanced...** button at the bottom right.

![Advanced settings for printer](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdf/doc/v2/en/printer-03.png)

**Paper Size** will be reflected in the size of each page after conversion. The values that can be set are mainly those corresponding to the actual paper size, such as A0-A6. Note that **Slide** is the size corresponding to the default value of Microsoft PowerPoint (4:3).

**Print Quality** affects the quality of the converted image data, the higher the value, the higher the quality and the larger the file size. The **Resolution** setting in the CubePDF application will be limited to the value set here.

**Scaling** is a setting to convert the source content to a larger or smaller size. The default value is 100 when converting at equal size. In some environments, this default value is set to a very large value. If the converted PDF is unusually large, check this setting.

**TrueType Font Download Option** under **PostScript Options** is a setting related to the character conversion method, and can be set as follows:

* **Native TrueType**  
  Converted as text and the font information is retained. However, some applications, including many Web browsers such as Microsoft Edge and Google Chrome, perform outlining to convert text into graphics during the printing process. Note that in this case, regardless of the printer settings, the text information will be lost.
* **Outline**  
  Converts text as graphics during printing. In this case, font information will be lost, but this has the effect of reducing problems such as misalignment after conversion.
* **Bitmap**  
  Converts text to a bitmap image for printing. This also solves some problems such as misalignment, but depending on the print quality and other settings, it may also cause problems such as jaggies.
* **Auto**  
  The printer will automatically choose between **Native TrueType**, **Outline**, and **Bitmap** settings.

The **Flip Left/Right Print** setting under **PostScript Options** is for converting the source content to be flipped left/right or up/down, and is normally set to **No**. In some environments, the default value is set to **Yes**. If the converted PDF is flipped upside down, please check this setting.

### Attention

The CubePDF printer settings may not be reflected depending on the application that executes printing, for example, if it has its own print settings. Please check the settings of the application carefully for the print settings.

## How to uninstall CubePDF

To uninstall CubePDF, first select **Uninstall a program** in **Control Panel** or **Apps** in **Settings** (Windows 8 or later). Then select the CubePDF icon on the window that appears and run the **Uninstall** item.

![Uninstall for Windows 8 and later](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdf/doc/v2/en/uninstall-01.png)
![Uninstall in the control pannel](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdf/doc/v2/en/uninstall-02.png)

## Having problems with CubePDF?

CubePDF outputs logs in the ```C:\ProgramData\CubeSoft\CubePdf\Log``` folder. If you encounter any problems, please contact us at support@cube-soft.jp with these logs attached.