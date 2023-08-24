/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
/* ------------------------------------------------------------------------- */
namespace Cube.Pdf.Converter.Tests.Settings;

using Cube.Pdf.Converter.Extensions;
using Cube.Pdf.Ghostscript;
using NUnit.Framework;

/* ------------------------------------------------------------------------- */
///
/// ExtensionTest
///
/// <summary>
/// Tests for handling file extensions.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[TestFixture]
class ExtensionTest : MockFixture
{
    /* --------------------------------------------------------------------- */
    ///
    /// Test
    ///
    /// <summary>
    /// Tests to change the extension.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestCase("Default.txt", "Default.pdf")]
    [TestCase("Sample.7z", "Sample.pdf")]
    [TestCase("Number.123", "Number.123.pdf")]
    [TestCase("TooLong.Extension", "TooLong.Extension.pdf")]
    [TestCase("2023.08.24", "2023.08.24.pdf")]
    [TestCase("日本語.区切り", "日本語.区切り.pdf")]
    [TestCase("FullWidth.ｔｘｔ", "FullWidth.ｔｘｔ.pdf")]
    [TestCase("FullWidth.７z", "FullWidth.７z.pdf")]
    public void Test(string src, string expected)
    {
        var ss = new SettingFolder(DataContract.Format.Registry, GetKeyName());
        ss.Load();
        ss.Normalize();
        ss.Set(new MockArguments(src, GetSource("Sample.ps"), ss.Value.Temp));
        Assert.That(ss.Value.Destination, Does.EndWith(expected));
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Test_CustomExtension
    ///
    /// <summary>
    /// Tests the ChangeExtension method with customized file extensions.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestCase(Format.Pdf,            ".test_pdf")]
    [TestCase(Format.Ps,             ".test_ps" )]
    [TestCase(Format.Eps,            ".test_eps")]
    [TestCase(Format.Png,            ".test_png")]
    [TestCase(Format.Jpeg,           ".test_jpg")]
    [TestCase(Format.Bmp,            ".test_bmp")]
    [TestCase(Format.Tiff,           ".test_tif")]
    [TestCase(Format.Png8bppIndexed, ".test_png")]
    [TestCase(Format.Psd,            ".psd"     )]
    public void Test_CustomExtension(Format src, string expected)
    {
        var ss = new SettingFolder(DataContract.Format.Registry, GetKeyName());
        ss.Load();
        ss.Normalize();
        ss.Value.Temp = Get(".tmp");
        ss.Value.Extensions.Pdf  = ".test_pdf";
        ss.Value.Extensions.Ps   = ".test_ps";
        ss.Value.Extensions.Eps  = ".test_eps";
        ss.Value.Extensions.Png  = ".test_png";
        ss.Value.Extensions.Jpeg = ".test_jpg";
        ss.Value.Extensions.Bmp  = ".test_bmp";
        ss.Value.Extensions.Tiff = ".test_tif";
        ss.Set(new MockArguments(nameof(Test_CustomExtension), GetSource("Sample.ps"), ss.Value.Temp));

        using var vm = new MainViewModel(ss);
        Assert.That(vm.Settings.Format, Is.EqualTo(Format.Pdf));
        Assert.That(vm.Settings.Destination, Does.EndWith(".test_pdf"));

        vm.Settings.Format = src;
        Assert.That(vm.Settings.Destination, Does.EndWith(expected));
    }
}
