Cube.Pdf.Pdfium
====

[![Package](https://img.shields.io/nuget/v/cube.pdf.pdfium)](https://www.nuget.org/packages/cube.pdf.pdfium/)
[![Native](https://img.shields.io/nuget/v/cube.native.pdfium?label=native)](https://www.nuget.org/packages/cube.native.pdfium/)
[![Native.Lite](https://img.shields.io/nuget/v/cube.native.pdfium.lite?label=native.lite)](https://www.nuget.org/packages/cube.native.pdfium.lite/)
[![AppVeyor](https://img.shields.io/appveyor/build/clown/cube-pdf)](https://ci.appveyor.com/project/clown/cube-pdf)
[![Codecov](https://img.shields.io/codecov/c/github/cube-soft/cube.pdf)](https://codecov.io/gh/cube-soft/cube.pdf)

The Cube.Pdf.Pdfium package is a library to use the [PDFium](https://pdfium.googlesource.com/pdfium/) in the .NET Framework 3.5, 4.6, .NET Standard 2.0, or later. Note that the Cube.Pdf.Pdfium library requires the pdfium.dll. You can download the DLL from [Cube.Native.Pdfium](https://www.nuget.org/packages/cube.native.pdfium) or [Cube.Native.Pdfium.Lite](https://www.nuget.org/packages/cube.native.pdfium.lite), which are our unofficial NuGet packages.

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
