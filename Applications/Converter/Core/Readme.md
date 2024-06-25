Cube.Pdf.Converter
====

[![Package](https://img.shields.io/nuget/v/cube.pdf.converter)](https://www.nuget.org/packages/cube.pdf.converter/)
[![AppVeyor](https://img.shields.io/appveyor/build/clown/cube-pdf)](https://ci.appveyor.com/project/clown/cube-pdf)
[![Codecov](https://img.shields.io/codecov/c/github/cube-soft/cube.pdf)](https://codecov.io/gh/cube-soft/cube.pdf)


Cube.Pdf.Converter is the core module of the [CubePDF](https://www.cube-soft.com/cubepdf/), which is available for .NET Framework 3.5, 4.6.2, .NET Standard 2.0, or later. Note that the Cube.Pdf.Converter reuqires the gsdll32.dll. You can download the DLL from [www.ghostscript.com](https://www.ghostscript.com/) or [Cube.Native.Pdfgen](https://www.nuget.org/packages/cube.native.pdfgen) NuGet package.

The Cube.Pdf.Converter is used for a limited purpose, such as emulating the CubePDF conversion. For more general purposes, consider using the following packages:

* [Cube.Pdf](https://www.nuget.org/packages/cube.pdf/)
* [Cube.Pdf.Pdfium](https://www.nuget.org/packages/cube.pdf.pdfium/)
* [Cube.Pdf.Itext](https://www.nuget.org/packages/cube.pdf.itext/)
* [Cube.Pdf.Generating](https://www.nuget.org/packages/cube.pdf.generating/)

## Dependencies

* [Ghostscript](https://www.ghostscript.com/) ... [Cube.Native.Pdfgen](https://www.nuget.org/packages/cube.native.pdfgen) is an unofficial package.
* [iText](https://www.nuget.org/packages/itext/) (except net35) or [iTextSharp](https://www.nuget.org/packages/iTextSharp/) (net35)

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
