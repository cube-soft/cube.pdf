Cube.Pdf.Ghostscript
====

[![NuGet](https://img.shields.io/nuget/v/Cube.Pdf.Ghostscript.svg)](https://www.nuget.org/packages/Cube.Pdf.Ghostscript/)
[![AppVeyor](https://ci.appveyor.com/api/projects/status/es768q3if3t40cbg?svg=true)](https://ci.appveyor.com/project/clown/cube-pdf)
[![Azure Pipelines](https://dev.azure.com/cube-soft-jp/Cube.Pdf/_apis/build/status/cube-soft.Cube.Pdf?branchName=master)](https://dev.azure.com/cube-soft-jp/Cube.Pdf/_build)
[![Codecov](https://codecov.io/gh/cube-soft/Cube.Pdf/branch/master/graph/badge.svg)](https://codecov.io/gh/cube-soft/Cube.Pdf)

Cube.Pdf.Ghostscript is a wrapper library of the [Ghostscript](https://www.ghostscript.com/), which is available for .NET Framework 3.5, 4.5 or later. Note that the Cube.Pdf.Ghostscript library is available for NuGet, but you need to copy the gsdll32.dll to the executing directory manually. You can download the DLL from [www.ghostscript.com](https://www.ghostscript.com/) or our [GitHub releases](https://github.com/cube-soft/Cube.Pdf/releases).

## Usage

Cube.Pdf.Ghostscript.Converter is the base class of other converter classes and a thin wrapper of the Ghostscript API.
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

When you convert a PostScript file to any other formats, you specify the target format at the constructor of the Converter class, add some options, and finally execute the Invoke method.
The Ghostscript API has two kinds of parameters, one is normal arguments and the other is PostScript codes.
Options and Codes properties of the Converter class correspond respectively.

Instead of using the Converter class directly, you can use some inherited classes according to your purpose.
For example, the following code converts to the PDF format.

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

When you set values to the properties, Converter inherited classes automatically add the corresponding arguments or PostScript codes to the Ghostscript API.
The library provides the following variations. All available formats and other options are defined in the [Parameters](https://github.com/cube-soft/Cube.Pdf/tree/master/Libraries/Ghostscript/Sources/Parameters) directory.

* [DocumentConverter](https://github.com/cube-soft/Cube.Pdf/blob/master/Libraries/Ghostscript/Sources/DocumentConverter.cs) ... PS/EPS/PDF
    * [PdfConverter](https://github.com/cube-soft/Cube.Pdf/blob/master/Libraries/Ghostscript/Sources/PdfConverter.cs)
* [ImageConverter](https://github.com/cube-soft/Cube.Pdf/blob/master/Libraries/Ghostscript/Sources/ImageConverter.cs) ... PNG/JPEG/BMP/TIFF
    * [JpegConverter](https://github.com/cube-soft/Cube.Pdf/blob/master/Libraries/Ghostscript/Sources/JpegConverter.cs)

When you need to add some options manually, you create a new instance of the [Argument](https://github.com/cube-soft/Cube.Pdf/tree/master/Libraries/Ghostscript/Sources/Argument.cs) class and add it to the Options property. Constructors of the Argument class are as follows:

```cs
public class Argument
{
    public Argument(string name, string value);
    public Argument(string name, bool value);
    public Argument(string name, int value);
    public Argument(char type);
    public Argument(char type, int value);
    public Argument(char type, string name);
    public Argument(char type, string name, bool value);
    public Argument(char type, string name, int value);
    public Argument(char type, string name, string value);
    public Argument(char type, string name, string value, bool literal);
}
```

## Dependencies

* [Cube.Core](https://github.com/cube-soft/Cube.Core)
* [Cube.FileSystem](https://github.com/cube-soft/Cube.FileSystem)
* [Cube.Pdf](https://github.com/cube-soft/Cube.Pdf)
* [Ghostscript](https://www.ghostscript.com/)

## Contributing

1. Fork [Cube.Pdf](https://github.com/cube-soft/Cube.Pdf/fork) repository.
2. Create a feature branch from the master or stable branch (e.g. git checkout -b my-new-feature origin/master). Note that the master branch may refer some pre-released NuGet packages. Try the [rake clean](https://github.com/cube-soft/Cube.Pdf/blob/master/Rakefile) command when build errors occur.
3. Commit your changes.
4. Rebase your local changes against the master or stable branch.
5. Run test suite with the [NUnit](http://nunit.org/) console or the Visual Studio (NUnit 3 test adapter) and confirm that it passes.
6. Create new Pull Request.

## License
 
Copyright © 2010 [CubeSoft, Inc.](https://www.cube-soft.jp/)
The Cube.Pdf.Ghostscript library is licensed under the [GNU AGPLv3](https://github.com/cube-soft/Cube.Pdf/blob/master/Libraries/Ghostscript/License.txt).