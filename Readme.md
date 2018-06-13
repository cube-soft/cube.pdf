Cube.Pdf
====

Cube.Pdf projects wrap [Ghostscript](https://www.ghostscript.com/), [iText](https://itextpdf.com/), and other third-party's PDF libraries.
The repository also has some implemented PDF applications, such as [CubePDF](https://www.cube-soft.jp/cubepdf/), [CubePDF Page](https://www.cube-soft.jp/cubepdfpage/), and more.
Note that some projects are licensed under the GNU AGPLv3. See the License section for details.

## Dependencies

* [Cube.Core](https://github.com/cube-soft/Cube.Core)
* [Cube.FileSystem](https://github.com/cube-soft/Cube.FileSystem)
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
 
Copyright (c) 2010 [CubeSoft, Inc.](http://www.cube-soft.jp/)

Projects are respectively licensed as follows:

* Libraries
    * [Cube.Pdf](https://github.com/cube-soft/Cube.Pdf/tree/master/Libraries/Core) ... [Apache 2.0](https://github.com/cube-soft/Cube.Pdf/blob/master/License.txt)
    * [Cube.Pdf.Ghostscript](https://github.com/cube-soft/Cube.Pdf/tree/master/Libraries/Ghostscript) ... [GNU AGPLv3](https://github.com/cube-soft/Cube.Pdf/blob/master/Libraries/Ghostscript/License.txt)
    * [Cube.Pdf.Itext](https://github.com/cube-soft/Cube.Pdf/tree/master/Libraries/Itext) ... [GNU AGPLv3](https://github.com/cube-soft/Cube.Pdf/blob/master/Libraries/Itext/License.txt)
* Applications
    * [CubePDF Clip](https://github.com/cube-soft/Cube.Pdf/tree/master/Applications/Clip) ... [GNU AGPLv3](https://github.com/cube-soft/Cube.Pdf/blob/master/Applications/Clip/License.txt)
    * [CubePDF Page](https://github.com/cube-soft/Cube.Pdf/tree/master/Applications/Pages) ... [GNU AGPLv3](https://github.com/cube-soft/Cube.Pdf/blob/master/Applications/Pages/License.txt)
    * [CubePDF ImagePicker](https://github.com/cube-soft/Cube.Pdf/tree/master/Applications/Picker) ... [GNU AGPLv3](https://github.com/cube-soft/Cube.Pdf/blob/master/Applications/Picker/License.txt)

Note that trade names, trademarks, service marks, or logo images distributed in CubeSoft applications are not allowed to reuse or modify all or parts of them.