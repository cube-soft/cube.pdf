CubePDF Series
====

[![Core](https://badgen.net/nuget/v/cube.pdf?label=core)](https://www.nuget.org/packages/cube.pdf/)
[![Ghostscript](https://badgen.net/nuget/v/cube.pdf.generating?label=gs)](https://www.nuget.org/packages/cube.pdf.generating/)
[![iText](https://badgen.net/nuget/v/cube.pdf.itext?label=itext)](https://www.nuget.org/packages/cube.pdf.itext/)
[![PDFium](https://badgen.net/nuget/v/cube.pdf.pdfium?label=pdfium)](https://www.nuget.org/packages/cube.pdf.pdfium/)
[![CubePDF](https://badgen.net/nuget/v/cube.pdf.converter?label=cubepdf)](https://www.nuget.org/packages/cube.pdf.converter/)
[![GS.Native](https://badgen.net/nuget/v/cube.native.ghostscript?label=gs.native)](https://www.nuget.org/packages/cube.native.ghostscript/)
[![PDFium.Native](https://badgen.net/nuget/v/cube.native.pdfium?label=pdfium.native)](https://www.nuget.org/packages/cube.native.pdfium/)
[![PDFium.Native.Lite](https://badgen.net/nuget/v/cube.native.pdfium.lite?label=pdfium.native.lite)](https://www.nuget.org/packages/cube.native.pdfium.lite/)
[![AppVeyor](https://badgen.net/appveyor/ci/clown/cube-pdf)](https://ci.appveyor.com/project/clown/cube-pdf)
[![Codecov](https://badgen.net/codecov/c/github/cube-soft/cube.pdf)](https://codecov.io/gh/cube-soft/cube.pdf)

The project provides the following Windows desktop applications:

* [CubePDF](https://www.cube-soft.com/cubepdf/) ... Virtual printer-based PDF conversion software
* [CubePDF Utility](https://www.cube-soft.com/cubepdfutility/) ... Thumbnail-based PDF editing software
* [CubePDF Page](https://www.cube-soft.com/cubepdfpage/) ... PDF editing software specialized in file-based merging

Moreover, CubePDF SDK provides wrapper APIs for the [PDFium](https://pdfium.googlesource.com/pdfium/), [Ghostscript](https://www.ghostscript.com/), and [iText](https://itextpdf.com/) libraries.

Libraries and applications are available for .NET Framework 3.5, 4.7, .NET Standard 2.0, .NET Core 6, or later. Note that some projects are licensed under the GNU AGPLv3 and the others under the Apache 2.0. See [License.md](https://github.com/cube-soft/Cube.Pdf/blob/master/License.md) for more information.

## CubePDF

![CubePDF overview](https://raw.githubusercontent.com/cube-soft/cube.assets/master/cubepdf/doc/v2/en/overview.png)

[CubePDF](https://www.cube-soft.com/cubepdf/) is a PDF converter which allows you to convert files from any applications (for example, Google Chrome, Firefox, Microsoft Edge, Microsoft Word, Excel, PowerPoint, and more), whenever you need it. The converter allows you to convert the files as easy as you can print the files; as a matter of fact, you can do it in the same manner as you print files. The application depends on the Ghostscript and iText libraries. For more information, see [CubePDF Documents](https://docs.cube-soft.jp/entry/cubepdf).

You can get the executable installer from the [download page](https://www.cube-soft.com/cubepdf/) or [GitHub Releases](https://github.com/cube-soft/cube.pdf/releases). Source codes of the CubePDF are in the [Applications/Converter](https://github.com/cube-soft/cube.pdf/tree/master/Applications/Converter) (except for the virtual printer).

## CubePDF Utility

![CubePDF Utility overview](https://raw.githubusercontent.com/cube-soft/cube.assets/master/cubepdfutility/doc/v1/en/overview.png)

[CubePDF Utility](https://www.cube-soft.com/cubepdfutility/) is a PDF editor which can insert, remove, move, rotate pages, add or modify some metadata (PDF version, title, author, subject, keywords, creator, page layout), and encryption settings GUI. The application depends on the PDFium and iText libraries. For more information, see [CubePDF Utility Documents](https://docs.cube-soft.jp/entry/cubepdf-utility).

You can get the executable installer from the [download page](https://www.cube-soft.com/cubepdfutility/) or [GitHub Releases](https://github.com/cube-soft/cube.pdf/releases). Source codes of the CubePDF Utility are in the [Applications/Editor](https://github.com/cube-soft/cube.pdf/tree/master/Applications/Editor).

## CubePDF Page

![CubePDF Page overview](https://raw.githubusercontent.com/cube-soft/cube.assets/master/cubepdfpage/doc/v2/en/main.png)

[CubPDF Page](https://www.cube-soft.com/cubepdfpage/) is a software for merging or splitting existing PDF and image files. You can also add various PDF metadata and encryption settings to the merged or split PDF file. For more information, see [CubePDF Page Documents](https://docs.cube-soft.jp/entry/cubepdf-page).

You can get the executable installer from the [download page](https://www.cube-soft.com/cubepdfpage/) or [GitHub Releases](https://github.com/cube-soft/cube.pdf/releases). Source codes of the CubePDF Page are in the [Applications/Pages](https://github.com/cube-soft/cube.pdf/tree/master/Applications/Pages).

## CubePDF SDK

### iText or PDFium wrapper

You can install Cube.Pdf libraries through the NuGet package commands or UI on Visual Studio.
The Libraries provide the functionality to treat third-party libraries as the same interface.
Basic interfaces of the Cube.Pdf are as follows:

* [IDocumentReader](https://github.com/cube-soft/cube.pdf/blob/master/Libraries/Core/Sources/IDocumentReader.cs)
* [IDocumentWriter](https://github.com/cube-soft/cube.pdf/blob/master/Libraries/Core/Sources/IDocumentWriter.cs)

For example, the following sample accesses a PDF document by using the iText library.
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

### Ghostscript wrapper

When you convert from PostScript to any other formats, you can use the Cube.Pdf.Generating (Cube.Pdf.Ghostscript) library.
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

See the [Readme](https://github.com/cube-soft/cube.pdf/blob/master/Libraries/Ghostscript/Readme.md) in the Ghostscript directory for details.

## Dependencies

Dependencies of [Libraries](https://github.com/cube-soft/cube.pdf/tree/master/Libraries) are as follows. [Applications](https://github.com/cube-soft/cube.pdf/tree/master/Applications) may use some more third-party libraries.

* [PDFium](https://pdfium.googlesource.com/pdfium/) ... [Cube.Native.Pdfium.Lite](https://www.nuget.org/packages/Cube.Native.Pdfium.Lite) is a NuGet package of the PDFium.
* [Ghostscript](https://www.ghostscript.com/) ... [Cube.Native.Ghostscript](https://www.nuget.org/packages/Cube.Native.Ghostscript) is a NuGet package of the Ghostscript.
* [itext7](https://www.nuget.org/packages/itext7/) (other branches) or [iTextSharp](https://www.nuget.org/packages/iTextSharp/) (net35)

## Contributing

1. Fork [Cube.Pdf](https://github.com/cube-soft/cube.pdf/fork) repository.
2. Create a feature branch from the master branch (e.g. git checkout -b my-new-feature origin/master). Note that the master branch may refer to some pre-release NuGet packages. Try the [rake clobber](https://github.com/cube-soft/cube.pdf/blob/master/Rakefile) and copy commands when build errors occur.
3. Commit your changes.
4. Rebase your local changes to the master branch.
5. Run the dotnet test command or the Visual Studio (NUnit 3 test adapter) and confirm that it passes.
6. Create a new Pull Request.

## License
 
Copyright © 2010 [CubeSoft, Inc.](https://www.cube-soft.com/)
See [License.md](https://github.com/cube-soft/cube.pdf/blob/master/License.md) for more information.
