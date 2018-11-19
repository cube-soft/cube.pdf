Cube.Pdf.Ghostscript
====

[![NuGet](https://img.shields.io/nuget/v/Cube.Pdf.Ghostscript.svg)](https://www.nuget.org/packages/Cube.Pdf.Ghostscript/)
[![AppVeyor](https://ci.appveyor.com/api/projects/status/es768q3if3t40cbg?svg=true)](https://ci.appveyor.com/project/clown/cube-pdf)
[![Codecov](https://codecov.io/gh/cube-soft/Cube.Pdf/branch/master/graph/badge.svg)](https://codecov.io/gh/cube-soft/Cube.Pdf)

Cube.Pdf.Ghostscript wraps the [Ghostscript](https://www.ghostscript.com/) library.
The library is available for .NET Framework 3.5, 4.5 or more.

## Usage

The Cube.Pdf.Ghostscript library is available for NuGet, but you need to copy the gsdll32.dll to the executing directory manually.
You can download the library from [www.ghostscript.com](https://www.ghostscript.com/) or our [GitHub releases](https://github.com/cube-soft/Cube.Pdf/releases).

The Cube.Pdf.Ghostscript.Converter is the base class of other converter classes and a thin wrapper of the Ghostscript API.
Basic interfaces of converters are as follows:

```cs
public class Converter
{
    public Converter(Format format);
    public ICollection<Argument> Options;
    public ICollection<Code> Codes;
    public void Invoke(string src, string dest);
}
```

When you convert a PostScript file to any other formats, you specified the target format at the constructor of the Converter class, then add some options, and finally execute the Invoke method.
The Ghostscript API has two kinds of parameters, one is normal arguments and the other is PostScript codes.
Options and code properties of the Converter class correspond respectively.

Instead of using the Converter class directly, you usually use some inherited classes according to your purpose.
For example, the following code converts to the PDF format.

```cs
// using Cube.Pdf.Ghostscript;
var converter = new DocumentConverter(Format.Pdf)
{
    Paper        = Paper.Auto,
    Orientation  = Orientation.Auto,
    ColorMode    = ColorMode.Rgb,
    Resolution   = 600,
    Compression  = Encoding.Jpeg,
    Downsampling = Downsampling.None,
}
converter.Invoke(@"path\to\src.ps", @"path\to\dest.pdf");
```

When you set values to the properties, Converter inherited classes automatically create and add the corresponding arguments or PostScript codes to the Ghostscript API.
The library prepares the following variations.

* [DocumentConverter](https://github.com/cube-soft/Cube.Pdf/blob/master/Libraries/Ghostscript/Sources/DocumentConverter.cs) ... PS/EPS/PDF
    * [PdfConverter](https://github.com/cube-soft/Cube.Pdf/blob/master/Libraries/Ghostscript/Sources/PdfConverter.cs)
* [ImageConverter](https://github.com/cube-soft/Cube.Pdf/blob/master/Libraries/Ghostscript/Sources/ImageConverter.cs) ... PNG/JPEG/BMP/TIFF
    * [JpegConverter](https://github.com/cube-soft/Cube.Pdf/blob/master/Libraries/Ghostscript/Sources/JpegConverter.cs)

All available formats and other options are defined in the [Parameters](https://github.com/cube-soft/Cube.Pdf/tree/master/Libraries/Ghostscript/Sources/Parameters) directory.

## Dependencies

* [Cube.Core](https://github.com/cube-soft/Cube.Core)
* [Cube.FileSystem](https://github.com/cube-soft/Cube.FileSystem)
* [Cube.Pdf](https://github.com/cube-soft/Cube.Pdf)
* [Ghostscript](https://www.ghostscript.com/)

## License
 
Copyright © 2010 [CubeSoft, Inc.](https://www.cube-soft.jp/)
The Cube.Pdf.Ghostscript project is licensed under the [GNU AGPLv3](https://github.com/cube-soft/Cube.Pdf/blob/master/Libraries/Ghostscript/License.txt).