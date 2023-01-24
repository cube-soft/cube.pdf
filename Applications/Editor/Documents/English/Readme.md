CubePDF Utility Documnets
====

Copyright © 2013 CubeSoft, Inc.  
GNU Affero General Public License version 3 (AGPLv3)  
support@cube-soft.jp  
https://www.cube-soft.jp/cubepdfutility/

## Overview

![CubePDF Utility main window](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdfutility/overview.en.png)

CubPDF Utility is a software that allows you to insert, extract, or remove pages, edit document metadata, and security settings for existing PDF files. To use CubPDF Utility, first open the PDF file you want to edit in one of the following three ways.

1. Select from **Recent files** displayed in the center of the main window or in the **File** menu.
2. Click the **Open** button and select from the displayed dialog.
3. Drag&Drop the PDF file to the main window.


If the selected PDF file is protected by a password, a dialog box will appear asking you to enter the administrative password. Please enter the correct password.

![Password dialog](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdfutility/doc/v1/en/open-02.png)

After opening and editing the PDF file, click the **Save (Overwrite)** or **Save as** button at the end. This will complete the editing of the PDF.

![Save menu](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdfutility/doc/v1/en/save.png)

### About PDF password

There are two types of passwords for PDF files: **User password**, which allows only viewing and pre-authorized operations, and **Owner password**, which allows all operations.

CubePDF Utility always requires **Owner password** when opening PDF files due to the feature of editing PDF files. For this reason, you cannot open a PDF file with the **User password**. Note that even if a PDF file can be opened without any password, **Owner password** may be set. If you want to edit the PDF file, please ask the creator of the PDF file for the **Owner password**.

## List of editing operations

### Insert

![Insert menu](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdfutility/doc/v1/en/insert-01.png)

CubePDF Utility allows you to insert the following files into a PDF file.

* PDF files
* Bitmap image files (PNG, JPEG, BMP, TIFF)

To insert a file, specify the insertion position using the one of following four menus displayed in the upper menu or context menu.

* **Insert** or **Insert behind selected position**
* **Insert at the begining**
* **Insert at the end**
* **Insert at other position**

When **Insert at other position** is selected, the following dialog will be displayed to set the insertion position and the files to be inserted. Otherwise, a system dialog for selecting files will be displayed.

![Insert at other position](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdfutility/doc/v1/en/insert-02.png)

In addition, CubePDF Utility can also insert PDF files by pages. To insert a page by page, first open both the source and destination PDF files in CubePDF Utility. Then, Drag&Drop the thumbnail images of the pages you want to insert.

![Insert by Drag&Drop](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdfutility/doc/v1/en/insert-03.png)

### Remove

![Remove menu](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdfutility/doc/v1/en/remove-01.png)

The **Remove** menu allows you to remove some pages of an opened PDF file. When you select **Remove** or **Remove the selected  pages** menu, the currently selected pages on the main window will be removed. Also, if you select the **Remove other pages** menu, the following dedicated dialog will be displayed.

![Remove other pages](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdfutility/doc/v1/en/remove-02.png)

**Target pages** should contain the range of pages to be removed. The allowed characters are numbers, "," (comma), and "-" (hyphen) (e.g. 1,2,4,7-9).

### Extract

![Extract menu](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdfutility/doc/v1/en/extract-01.png)

CubePDF Utility allows you to extract any page in a PDF file and save it in the following file formats.

* PDF
* PNG (Bitmap image file)

Note that if you select the **Extract** or **Extract the selected pages** menu, the saved format will be PDF. If you want to extract in PNG format, please make the necessary settings in the following dialog after selecting the **Extract with other settings** menu.

![Extract with other settings](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdfutility/doc/v1/en/extract-02.png)

In the **Save path** section of the dialog, set the path where the extracted results will be saved. For **Format** section, you can select either **PDF** or **PNG**. If you select PNG, you can set the DPI. This value is used to determine the size (width and height) of the PDF content when it is converted to bitmap format. For example, if you want to convert an A4 size PDF file, the width and height will be as follows, depending on the DPI setting.

*  72dpi ...  595 ×  842
*  96dpi ...  793 x 1122
* 144dpi ... 1190 x 1684
* 300dpi ... 2479 x 3508

For **Target pages**, select from **Selected pages**, **All pages**, or **Specified range**. When **Specified range** is selected, the text box at the bottom will contain the range of pages to be extracted. The allowed characters are numbers, "," (comma), and "-" (hyphen) (e.g., 1,2,4,4). 1,2,4,7-9).

If the **Save as a separate file per page** option is enabled, for example, extracting a 10-page PDF file will generate 10 PDF files. In this case, the file name of each generated PDF file will be the one specified in **Save path** with a number added. For example, if you specify the file name "Sample.pdf", the generated PDF files will be Sample-01.pdf, Sample-02.pdf, and so on. If you specify PNG as the format, the same behavior will occur.

### Reorder (Move)

![Reorder menu](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdfutility/doc/v1/en/reorder.png)

The **Forth** and **Back** menus allow you to move the currently selected page in the main window forward or backward by one page, respectively. The page order can also be changed by Drag&Drop operation. Select the pages you want to change, and then Drag&Drop them to the target position.

### Rotate

![Rotate menu](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdfutility/doc/v1/en/rotate.png)

The **Left** and **Right** menus allow you to rotate the currently selected pages on the main window left or right by 90 degrees, respectively.

### Metadata

![Metadata dialog](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdfutility/doc/v1/en/metadata.png)

In the **Metadata** dialog, you can register information such as the title and creator. The information registered here can be viewed in the properties dialog of PDF viewer such as Adobe Acrobat Reader DC. In addition, the **Layout** item allows you to change the way the document will be displayed when opened in PDF viewer.

### Security

![Security dialog](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdfutility/doc/v1/en/encryption.png)

In the **Security** dialog, you can protect the PDF file with a password. To set a password, first enable the **Encrypt the PDF with password** option, and then enter the same password twice in the **Password** and **Confirm** fields. Currently, four encryption methods are supported: **40-bit RC4**, **128-bit RC4**, **128-bit AES**, and **256-bit AES**.

In the **Operations** item, specify the operations to be allowed or restricted to users. The following items can be set.

* Open with password
* Allow printing
* Allow copying text and images
* Allow inserting and removing pages
* Allow using contents for accessibility
* Allow filling in form fields
* Allow creating and editing annotations

Note that if you enable the item **Open with password** and also enable the item **Use owner password**, CubePDF Utility will set the same password as the owner password for user password.

However, **if you share both passwords, the restrictions on printing and copying operations may not work properly depending on the PDF viwer**. This is probably because the PDF viewer recognizes that the PDF file was opened with the owner password. For this reason, CubePDF Utility is designed not to accept permission settings when it is shared with the owner password.

Moreover, if a PDF file is recognized as having been opened with the owner password, all PDF editing, including removal of the user password, will be possible. Please make sure you fully understand these behaviors when you share the user password with the owner password.

## Other settings

### View settings for the main window

![View menu](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdfutility/doc/v1/en/view.png)

On the **Others** tab of the main window, you can change the view settings of the CubePDF Utility. You can change the display size of the thumbnail image by using the **ZoomIn**, **ZoomOut**, and Pixels selection boxes. Note that immediately after changing the size, the thumbnail image before the change will be used as it is, so the display may be blurry. In this case, click the **Refresh** button.

**Frame only** button hides the thumbnail image and instead displays a border for the number of pages. This feature is mainly used to reduce memory consumption.

### Application settings

![Settings dialog](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdfutility/doc/v1/en/settings.png)

Click the **Settings** button on the **Others** tab of the main window to display the version information and related settings. The setting items are as follows.

If the **Reduce duplicate resources** item is enabled, CubePDF Utility will attempt to reduce file size by removing duplicated portions of font information and other resources as much as possible when saving. Currently, there are reports that some annotations look different when this feature is enabled than they did before being merged. If you encounter such a phenomenon, please consider disabling this item.

If the **Keep bookmarks of source PDF files** item is enabled, the resulting saved PDF file will include the bookmark information of each of the merging sources. Currently, there are several reports of PDF file merging failures in CubePDF Utility and CubePDF Page, most of which seem to be caused by the bookmark information. If you encounter such a phenomenon, please consider disabling this item.

If the **Enable backup** item is enabled, CubePDF Utility will keep a backup of the original PDF file for a certain period of time before overwriting. The text box below that specifies the folder where the backup files will be stored.

In **Temporary folder** section, you can specify the folder where temporary files etc. are created when CubePDF Utility performs some operations. If the section is empty, CubePDF Utility will also use the destination folder as a temporary folder.

In **Language** section, you can select the display language for the menu of the CubePDF Utility. There are three supported languages, English, German and Japanese. If you select **Auto** for the display language, one of the languages will be automatically selected according to the language setting of Windows.

If the **Show recently used files** item is enabled, CubePDF Utility will display a list of the recently opened PDF files at startup.

If the **Check for updates on startup** item is enabled, you will be notified in the lower right corner of your computer when the new version of the CubePDF Utility has been released. Checking for updates will be performed when your computer starts up. The information sent to confirm the update is the version number of CubePDF, Windows, and .NET Framework.

## List of keyboard shortcuts

The keyboard shortcuts available in CubePDF Utility are as follows.

* Ctrl + O ... Open PDF file
* Ctrl + S ... Overwrite
* Ctrl + Shit + S ... Save as
* Ctrl + W ... Close the PDF file
* Ctrl + Q ... Exit the application
* Ctrl + I or Insert ... Insert PDF, PNG, JPEG, or BMP file at the selected position
* Ctrl + Shift +I ... Insert at other position
* Ctrl + D or Delete ... Remove the selected pages
* Ctrl + Shift + D ... Remove other pages
* Ctrl + E ... Extract the selected pages
* Ctrl + Shift + E ... Extract with other settings
* Ctrl + B ... Move the selected page forward one page
* Ctrl + F ... Move the selected page back one page
* Ctrl + L ... Rotate the selected page 90 degrees left
* Ctrl + R ... Rotate the selected page 90 degrees right
* Ctrl + M ... Show the Metadata dialog
* Ctrl + K ... Show the Security dialog
* Ctrl + A ... Select all
* Ctrl + Z ... Undo
* Ctrl + Y ... Redo
* Ctrl + + ... Zoom in the thumbnail size
* Ctrl + - ... Zoom out the thumbnail size

## How to uninstall CubePDF Utility

To uninstall CubePDF Utility, first select **Uninstall a program** in **Control Panel** or **Apps** in **Settings** (Windows 8 or later). Then select the CubePDF Utility icon on the window that appears and run the **Uninstall** item.

![Uninstall for Windows 8 and later](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdfutility/doc/v1/en/uninstall-01.png)
![Uninstall in the control pannel](https://raw.githubusercontent.com/cube-soft/Cube.Assets/master/cubepdfutility/doc/v1/en/uninstall-02.png)

## Having problems with CubePDF Utility?

CubePDF Utility outputs logs in the ```C:\Users\%UserName%\AppData\Local\CubeSoft\CubePdfUtility2\Log``` folder. If you encounter any problems, please contact us at support@cube-soft.jp with these logs attached (Replace %UserName% with the name of the user currently logged on).