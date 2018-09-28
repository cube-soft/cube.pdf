Cube.Pdf
====

[![NuGet](https://img.shields.io/nuget/v/Cube.Pdf.svg?label=core)](https://www.nuget.org/packages/Cube.Pdf/)
[![NuGet](https://img.shields.io/nuget/v/Cube.Pdf.Ghostscript.svg?label=gs)](https://www.nuget.org/packages/Cube.Pdf.Ghostscript/)
[![NuGet](https://img.shields.io/nuget/v/Cube.Pdf.Itext.svg?label=itext)](https://www.nuget.org/packages/Cube.Pdf.Itext/)
[![NuGet](https://img.shields.io/nuget/v/Cube.Pdf.Pdfium.svg?label=pdfium)](https://www.nuget.org/packages/Cube.Pdf.Pdfium/)  
[![AppVeyor](https://ci.appveyor.com/api/projects/status/es768q3if3t40cbg?svg=true)](https://ci.appveyor.com/project/clown/cube-pdf)
[![Codecov](https://codecov.io/gh/cube-soft/Cube.Pdf/branch/master/graph/badge.svg)](https://codecov.io/gh/cube-soft/Cube.Pdf)

Cube.Pdf projects wrap [PDFium](https://pdfium.googlesource.com/pdfium/), [Ghostscript](https://www.ghostscript.com/), [iText](https://itextpdf.com/), and other third-party's PDF libraries. The repository also has some implemented PDF applications, such as [CubePDF](https://www.cube-soft.jp/cubepdf/), [CubePDF Utility](https://www.cube-soft.jp/cubepdfutility/), [CubePDF Page](https://www.cube-soft.jp/cubepdfpage/), and more. We will move [CubePdfViewer](https://github.com/cube-soft/CubePdfViewer) to the repository.
Note that some projects are licensed under the GNU AGPLv3. See the License section for details.

## Summary

### Libraries

Cube.Pdf Libraries provide functionality to treat third-party libraries as the same interface as possible (except for Cube.Pdf.Ghostscript). Interfaces of the Cube.Pdf are as follows:

* [IDocumentReader](https://github.com/cube-soft/Cube.Pdf/blob/master/Libraries/Core/Sources/IDocumentReader.cs)
* [IDocumentRenderer](https://github.com/cube-soft/Cube.Pdf/blob/master/Libraries/Core/Sources/IDocumentRenderer.cs)
* [IDocumentWriter](https://github.com/cube-soft/Cube.Pdf/blob/master/Libraries/Core/Sources/IDocumentWriter.cs)

For example, the avobe program accesses a PDF document by using the iTextSharp library.
And when you want to use the PDFium library, you only modify the description of "Cube.Pdf.Itext.DocumentReader" to "Cube.Pdf.Pdfium.DocumentReader".

```cs
// Set password directly or using Query<string>
var password = new Cube.Query<string>(e => e.Result = "password");
var path = @"path/to/sample.pdf";
using (var reader = new Cube.Pdf.Itext.DocumentReader(path, password))
{
    // Do something.
}
```

### CubePDF

![Screenshot](https://github.com/cube-soft/Cube.Pdf/blob/master/Applications/Converter/Overview.png?raw=true)

[CubePDF](https://www.cube-soft.jp/cubepdf/) is a PDF converter which allows you to convert files from any applications (for example, Google Chrome, Firefox, Microsoft Edge, Microsoft Word, Excel, PowerPoint, and more), whenever you need it. The converter allows you to convert the files as easy as you can print the files; as a matter of fact, you can do it in the same manner as you print files. The application uses Ghostscript and iTextSharp.

You can get the executable installer from the [download page](https://www.cube-soft.jp/cubepdf/) (Japanese), or [directly download link](https://www.cube-soft.jp/cubepdf/dl.php).
Note that the installer always shows menus and other messages only in Japanese. Source codes of the CubePDF are in the [Applications/Converter](https://github.com/cube-soft/Cube.Pdf/tree/master/Applications/Converter) (except for the virtual printer).

### CubePDF Utility

![Screenshot](https://github.com/cube-soft/Cube.Pdf/blob/master/Applications/Editor/Overview.png?raw=true)

[CubePDF Utility](https://www.cube-soft.jp/cubepdfutility/) is a PDF editor which can insert, remove, move, rotate pages, add or modify some metadata (PDF version, title, author, subject, keywords, creator, page layout), and encryption settings through the graphical user interface (GUI). The application uses PDFium and iTextSharp.

You can get the executable installer from the [download page](https://www.cube-soft.jp/cubepdfutility/) (Japanese), or [directly download link](https://www.cube-soft.jp/cubepdfutility/dl.php). Source codes of the CubePDF Utility are in the [Applications/Editor](https://github.com/cube-soft/Cube.Pdf/tree/master/Applications/Editor).

## Dependencies

Dependencies of [Libraries](https://github.com/cube-soft/Cube.Pdf/tree/master/Libraries) are as follows. [Applications](https://github.com/cube-soft/Cube.Pdf/tree/master/Applications) may use some other third-party's libraries.

* [Cube.Core](https://github.com/cube-soft/Cube.Core)
* [Cube.FileSystem](https://github.com/cube-soft/Cube.FileSystem)
* [PDFium](https://pdfium.googlesource.com/pdfium/)
* [Ghostscript](https://www.ghostscript.com/)
* [iTextSharp](https://www.nuget.org/packages/iTextSharp/)

## Contributing

1. Fork [Cube.Pdf](https://github.com/cube-soft/Cube.Pdf/fork) repository.
2. Create a feature branch from the [stable](https://github.com/cube-soft/Cube.Pdf/tree/stable) branch (git checkout -b my-new-feature origin/stable). The [master](https://github.com/cube-soft/Cube.Pdf/tree/master) branch may refer some pre-released NuGet packages. See [AppVeyor.yml](https://github.com/cube-soft/Cube.Pdf/blob/master/AppVeyor.yml) if you want to build and commit in the master branch.
3. Commit your changes.
4. Rebase your local changes against the stable (or master) branch.
5. Run test suite with the [NUnit](http://nunit.org/) console or the Visual Studio (NUnit 3 test adapter) and confirm that it passes.
6. Create new Pull Request.

## License
 
Copyright &copy; 2010 [CubeSoft, Inc.](http://www.cube-soft.jp/)
Projects are respectively licensed as follows:

### Libraries

| Name | License |
| ---- | ------- |
| [Cube.Pdf](https://github.com/cube-soft/Cube.Pdf/tree/master/Libraries/Core)                    | Apache 2.0 |
| [Cube.Pdf.Pdfium](https://github.com/cube-soft/Cube.Pdf/tree/master/Libraries/Pdfium)           | Apache 2.0 |
| [Cube.Pdf.Ghostscript](https://github.com/cube-soft/Cube.Pdf/tree/master/Libraries/Ghostscript) | GNU AGPLv3 |
| [Cube.Pdf.Itext](https://github.com/cube-soft/Cube.Pdf/tree/master/Libraries/Itext)             | GNU AGPLv3 |

### Applications

| Name | License |
| ---- | ------- |
| [CubePDF](https://github.com/cube-soft/Cube.Pdf/tree/master/Applications/Converter)          | GNU AGPLv3 |
| [CubePDF Utility](https://github.com/cube-soft/Cube.Pdf/tree/master/Applications/Editor)     | GNU AGPLv3 |
| [CubePDF Clip](https://github.com/cube-soft/Cube.Pdf/tree/master/Applications/Clip)          | GNU AGPLv3 |
| [CubePDF Page](https://github.com/cube-soft/Cube.Pdf/tree/master/Applications/Pages)         | GNU AGPLv3 |
| [CubePDF ImagePicker](https://github.com/cube-soft/Cube.Pdf/tree/master/Applications/Picker) | GNU AGPLv3 |

Note that trade names, trademarks, service marks, or logo images distributed in CubeSoft applications are not allowed to reuse or modify all or parts of them.