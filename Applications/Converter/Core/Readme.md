Cube.Pdf.Converter
====

[![NuGet](https://img.shields.io/nuget/v/Cube.Pdf.Converter.svg)](https://www.nuget.org/packages/Cube.Pdf.Converter/)
[![AppVeyor](https://ci.appveyor.com/api/projects/status/es768q3if3t40cbg?svg=true)](https://ci.appveyor.com/project/clown/cube-pdf)
[![Azure Pipelines](https://dev.azure.com/cube-soft-jp/Cube.Pdf/_apis/build/status/cube-soft.Cube.Pdf?branchName=master)](https://dev.azure.com/cube-soft-jp/Cube.Pdf/_build)
[![Codecov](https://codecov.io/gh/cube-soft/Cube.Pdf/branch/master/graph/badge.svg)](https://codecov.io/gh/cube-soft/Cube.Pdf)

Cube.Pdf.Converter is the core module of the [CubePDF](https://www.cube-soft.jp/cubepdf/), which is available for .NET Framework 3.5, 4.5, or later. Note that the Cube.Pdf.Converter is available for NuGet, but you need to copy the gsdll32.dll to the executing directory manually. You can download the DLL from [www.ghostscript.com](https://www.ghostscript.com/) or [Cube.Native.Ghostscript](https://www.nuget.org/packages/Cube.Native.Ghostscript) NuGet package.

The Cube.Pdf.Converter is used for a limited purpose, such as emulating the CubePDF conversion. For more general purposes, consider using the following packages:

* [Cube.Pdf](https://www.nuget.org/packages/Cube.Pdf/)
* [Cube.Pdf.Ghostscript](https://www.nuget.org/packages/Cube.Pdf.Ghostscript/)
* [Cube.Pdf.Pdfium](https://www.nuget.org/packages/Cube.Pdf.Pdfium/)
* [Cube.Pdf.Itext](https://www.nuget.org/packages/Cube.Pdf.Itext/)

## Dependencies

* [Cube.Core](https://github.com/cube-soft/Cube.Core)
* [Cube.FileSystem](https://github.com/cube-soft/Cube.FileSystem)
* [Cube.Pdf](https://github.com/cube-soft/Cube.Pdf)
* [iTextSharp](https://www.nuget.org/packages/iTextSharp/)
* [Ghostscript](https://www.ghostscript.com/) ... [Cube.Native.Ghostscript](https://www.nuget.org/packages/Cube.Native.Ghostscript) is an unofficial package.

## Contributing

1. Fork [Cube.Pdf](https://github.com/cube-soft/Cube.Pdf/fork) repository.
2. Create a feature branch from the master or stable branch (e.g. git checkout -b my-new-feature origin/master). Note that the master branch may refer to some pre-release NuGet packages. Try the [rake clean](https://github.com/cube-soft/Cube.Pdf/blob/master/Rakefile) and copy commands when build errors occur.
3. Commit your changes.
4. Rebase your local changes against the master or stable branch.
5. Run test suite with the [NUnit](http://nunit.org/) console or the Visual Studio (NUnit 3 test adapter) and confirm that it passes.
6. Create a new Pull Request.

## License
 
Copyright © 2010 [CubeSoft, Inc.](https://www.cube-soft.jp/)
The Cube.Pdf.Ghostscript library is licensed under the [GNU AGPLv3](https://github.com/cube-soft/Cube.Pdf/blob/master/Libraries/Ghostscript/License.txt).