Cube.Pdf.Generating
====

[![Package](https://img.shields.io/nuget/v/cube.pdf.generating)](https://www.nuget.org/packages/cube.pdf.generating/)
[![Native](https://img.shields.io/nuget/v/cube.native.pdfgen?label=native)](https://www.nuget.org/packages/cube.native.pdfgen/)
[![AppVeyor](https://img.shields.io/appveyor/build/clown/cube-pdf)](https://ci.appveyor.com/project/clown/cube-pdf)
[![Codecov](https://img.shields.io/codecov/c/github/cube-soft/cube.pdf)](https://codecov.io/gh/cube-soft/cube.pdf)

The Cube.Pdf.Generating package provides the wrapper APIs for the [Ghostscript](https://www.ghostscript.com/) in the .NET Framework 3.5, 4.6, .NET Standard 2.0, or later. Note that the library reuqires the gsdll32.dll. You can download the DLL from [www.ghostscript.com](https://www.ghostscript.com/) or [Cube.Native.Pdfgen](https://www.nuget.org/packages/Cube.Native.Pdfgen) NuGet package.

## Usage

Cube.Pdf.Ghostscript.Converter is the base class of other converter classes and a thin wrapper of the Ghostscript API.
Basic interfaces of converters are as follows:

```cs
// using System.Collections.Generics;
// using Cube.Pdf.Ghostscript;

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
The library provides the following variations. All available formats and other options are defined in the [Parameters](https://github.com/cube-soft/Cube.Pdf/tree/master/Libraries/Generating/Sources/Parameters) directory.

* [DocumentConverter](https://github.com/cube-soft/cube.pdf/blob/master/Libraries/Generating/Sources/DocumentConverter.cs) ... PS/EPS/PDF
    * [PdfConverter](https://github.com/cube-soft/cube.pdf/blob/master/Libraries/Generating/Sources/PdfConverter.cs)
* [ImageConverter](https://github.com/cube-soft/cube.pdf/blob/master/Libraries/Generating/Sources/ImageConverter.cs) ... PNG/JPEG/BMP/TIFF
    * [JpegConverter](https://github.com/cube-soft/cube.pdf/blob/master/Libraries/Generating/Sources/JpegConverter.cs)

When you need to add some options manually, you create a new instance of the [Argument](https://github.com/cube-soft/cube.pdf/tree/master/Libraries/Generating/Sources/Argument.cs) class and add it to the Options property. Constructors of the Argument class are as follows:

```cs
// using Cube.Pdf.Ghostscript;

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
