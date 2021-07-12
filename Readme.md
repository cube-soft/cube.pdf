Cube.Pdf
====

[![NuGet](https://img.shields.io/nuget/v/Cube.Pdf.svg?label=core)](https://www.nuget.org/packages/Cube.Pdf/)
[![NuGet](https://img.shields.io/nuget/v/Cube.Pdf.Ghostscript.svg?label=ghostscript)](https://www.nuget.org/packages/Cube.Pdf.Ghostscript/)
[![NuGet](https://img.shields.io/nuget/v/Cube.Pdf.Itext.svg?label=itext)](https://www.nuget.org/packages/Cube.Pdf.Itext/)
[![NuGet](https://img.shields.io/nuget/v/Cube.Pdf.Pdfium.svg?label=pdfium)](https://www.nuget.org/packages/Cube.Pdf.Pdfium/)
[![NuGet](https://img.shields.io/nuget/v/Cube.Pdf.Converter.svg?label=cubepdf)](https://www.nuget.org/packages/Cube.Pdf.Converter/)  
[![NuGet](https://img.shields.io/nuget/v/Cube.Native.Ghostscript.svg?label=ghostscript.native)](https://www.nuget.org/packages/Cube.Native.Ghostscript)
[![NuGet](https://img.shields.io/nuget/v/Cube.Native.Pdfium.svg?label=pdfium.native)](https://www.nuget.org/packages/Cube.Native.Pdfium)
[![NuGet](https://img.shields.io/nuget/v/Cube.Native.Pdfium.Lite.svg?label=pdfium.lite.native)](https://www.nuget.org/packages/Cube.Native.Pdfium.Lite)  
[![AppVeyor](https://ci.appveyor.com/api/projects/status/es768q3if3t40cbg?svg=true)](https://ci.appveyor.com/project/clown/cube-pdf)
[![Codecov](https://codecov.io/gh/cube-soft/Cube.Pdf/branch/master/graph/badge.svg)](https://codecov.io/gh/cube-soft/Cube.Pdf)

Cube.Pdf libraries wrap [PDFium](https://pdfium.googlesource.com/pdfium/), [Ghostscript](https://www.ghostscript.com/), [iText](https://itextpdf.com/), and other third-party PDF libraries. The repository also has some PDF applications, such as [CubePDF](https://www.cube-soft.jp/cubepdf/), [CubePDF Utility](https://www.cube-soft.jp/cubepdfutility/), [CubePDF Page](https://www.cube-soft.jp/cubepdfpage/), and more.
Libraries and applications are available for .NET Framework 3.5, 4.5, .NET Standard 2.0 (.NET Core 3.1 for applications), or later.
Note that some projects are licensed under the GNU AGPLv3 and the others under the Apache 2.0. See [License.md](https://github.com/cube-soft/Cube.Pdf/blob/master/License.md) for more information.

## Libraries

### Cube.Pdf and related libraries

You can install Cube.Pdf libraries through the NuGet package commands or UI on Visual Studio.
The Libraries provide the functionality to treat third-party libraries as the same interface (except for the Cube.Pdf.Ghostscript).
Basic interfaces of the Cube.Pdf are as follows:

* [IDocumentReader](https://github.com/cube-soft/Cube.Pdf/blob/master/Libraries/Core/Sources/IDocumentReader.cs)
* [IDocumentRenderer](https://github.com/cube-soft/Cube.Pdf/blob/master/Libraries/Core/Sources/IDocumentRenderer.cs)
* [IDocumentWriter](https://github.com/cube-soft/Cube.Pdf/blob/master/Libraries/Core/Sources/IDocumentWriter.cs)

For example, the following sample accesses a PDF document by using the iTextSharp library.
And when you want to use the PDFium library, you only modify the description of "using Cube.Pdf.Itext" to "using Cube.Pdf.Pdfium".

```cs
// using Cube.Pdf.Itext;

// Set password directly or using Query<string>
var password = new Cube.Query<string>(e =>
{
    e.Value  = "password", // Source path is set in the e.Source property.
    e.Cancel = false,
});

using (var reader = new DocumentReader(@"path/to/sample.pdf", password))
{
    // Do something with Pages, Metadata, Encryption, and more properties.
}
```

When you merge, extract, or remove existing PDF pages, the simplest code is as follow.
Note that if you specify an IDocumentReader object to the Add method of an IDocumentWriter implementation class, the IDocumentWriter object automatically disposes the specified object before saving.

```cs
// using Cube.Pdf.Itext;

using (var writer = new DocumentWriter())
{
    var src0 = new DocumentReader("first.pdf", "password");
    var src1 = new DocumentReader("second.pdf", "password");

    writer.Add(src0.Pages[0], src0);
    writer.Add(src1.Pages[0], src1);

    // Disposes src0 and src1 before saving.
    writer.Save(@"path/to/dest.pdf");
}
```

### Cube.Pdf.Ghostscript

When you convert from PostScript to any other formats, you can use the Cube.Pdf.Ghostscript library.
The following code converts to the PDF file.

```cs
// using Cube.Pdf;
// using Cube.Pdf.Ghostscript;

var converter = new PdfConverter
{
    Paper        = Paper.Auto,
    Orientation  = Orientation.Auto,
    ColorMode    = ColorMode.Rgb,
    Resolution   = 600,
    Compression  = Encoding.Jpeg,
    Downsampling = Downsampling.None,
    Version      = new PdfVersion(1, 7),
};
converter.Invoke(@"path\to\src.ps", @"path\to\dest.pdf");
```

See the [Readme](https://github.com/cube-soft/Cube.Pdf/blob/master/Libraries/Ghostscript/Readme.md) in the Ghostscript directory for details.

## Applications

### CubePDF

![Screenshot](https://github.com/cube-soft/Cube.Pdf/blob/master/Applications/Converter/Assets/Overview.png?raw=true)

[CubePDF](https://www.cube-soft.jp/cubepdf/) is a PDF converter which allows you to convert files from any applications (for example, Google Chrome, Firefox, Microsoft Edge, Microsoft Word, Excel, PowerPoint, and more), whenever you need it. The converter allows you to convert the files as easy as you can print the files; as a matter of fact, you can do it in the same manner as you print files. The application uses Ghostscript and iTextSharp.

You can get the executable installer from the [download page](https://www.cube-soft.jp/cubepdf/) or [GitHub Releases](https://github.com/cube-soft/Cube.Pdf/releases).
Source codes of the CubePDF are in the [Applications/Converter](https://github.com/cube-soft/Cube.Pdf/tree/master/Applications/Converter) (except for the virtual printer).

### CubePDF Utility

![Screenshot](https://github.com/cube-soft/Cube.Pdf/blob/master/Applications/Editor/Assets/Overview.png?raw=true)

[CubePDF Utility](https://www.cube-soft.jp/cubepdfutility/) is a PDF editor which can insert, remove, move, rotate pages, add or modify some metadata (PDF version, title, author, subject, keywords, creator, page layout), and encryption settings GUI. The application uses PDFium and iTextSharp.

You can get the executable installer from the [download page](https://www.cube-soft.jp/cubepdfutility/) (Japanese) or [GitHub Releases](https://github.com/cube-soft/Cube.Pdf/releases). Source codes of the CubePDF Utility are in the [Applications/Editor](https://github.com/cube-soft/Cube.Pdf/tree/master/Applications/Editor).

## Dependencies

Dependencies of [Libraries](https://github.com/cube-soft/Cube.Pdf/tree/master/Libraries) are as follows. [Applications](https://github.com/cube-soft/Cube.Pdf/tree/master/Applications) may use some other third-party libraries.

* [Cube.Core](https://github.com/cube-soft/Cube.Core)
* [PDFium](https://pdfium.googlesource.com/pdfium/) ... [Cube.Native.Pdfium.Lite](https://www.nuget.org/packages/Cube.Native.Pdfium.Lite) is a NuGet package of the PDFium.
* [Ghostscript](https://www.ghostscript.com/) ... [Cube.Native.Ghostscript](https://www.nuget.org/packages/Cube.Native.Ghostscript) is a NuGet package of the Ghostscript.
* [iText7](https://www.nuget.org/packages/itext7/) (net45) or [iTextSharp](https://www.nuget.org/packages/iTextSharp/) (net35)

## Contributing

1. Fork [Cube.Pdf](https://github.com/cube-soft/Cube.Pdf/fork) repository.
2. Create a feature branch from the master or stable branch (e.g. git checkout -b my-new-feature origin/master). Note that the master branch may refer to some pre-release NuGet packages. Try the [rake clobber](https://github.com/cube-soft/Cube.Pdf/blob/master/Rakefile) and copy commands when build errors occur.
3. Commit your changes.
4. Rebase your local changes against the master or stable branch.
5. Run test suite with the [NUnit](http://nunit.org/) console or the Visual Studio (NUnit 3 test adapter) and confirm that it passes.
6. Create a new Pull Request.

## License
 
Copyright © 2010 [CubeSoft, Inc.](https://www.cube-soft.jp/)
See [License.md](https://github.com/cube-soft/Cube.Pdf/blob/master/License.md) for more information.
