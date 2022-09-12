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
    /// Change
    ///
    /// <summary>
    /// Tests the ChangeExtension method with customized file extensions.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Test]
    public void Change()
    {
        var ss = new SettingFolder(DataContract.Format.Registry, GetKeyName());
        ss.Load();
        ss.Normalize();
        ss.Value.Temp = Get(".tmp");
        ss.Value.Appendix.Extensions.Pdf  = ".test_pdf";
        ss.Value.Appendix.Extensions.Ps   = ".test_ps";
        ss.Value.Appendix.Extensions.Eps  = ".test_eps";
        ss.Value.Appendix.Extensions.Png  = ".test_png";
        ss.Value.Appendix.Extensions.Jpeg = ".test_jpeg";
        ss.Value.Appendix.Extensions.Bmp  = ".test_bmp";
        ss.Value.Appendix.Extensions.Tiff = ".test_tiff";
        ss.Set(new MockArguments(nameof(Change), GetSource("Sample.ps"), ss.Value.Temp));

        using var vm = new MainViewModel(ss);
        Assert.That(vm.Settings.Format, Is.EqualTo(Format.Pdf));
        Assert.That(vm.Settings.Destination, Does.EndWith(".test_pdf"));

        vm.Settings.Format = Format.Ps;
        Assert.That(vm.Settings.Destination, Does.EndWith(".test_ps"));

        vm.Settings.Format = Format.Eps;
        Assert.That(vm.Settings.Destination, Does.EndWith(".test_eps"));

        vm.Settings.Format = Format.Png;
        Assert.That(vm.Settings.Destination, Does.EndWith(".test_png"));

        vm.Settings.Format = Format.Jpeg;
        Assert.That(vm.Settings.Destination, Does.EndWith(".test_jpeg"));

        vm.Settings.Format = Format.Bmp;
        Assert.That(vm.Settings.Destination, Does.EndWith(".test_bmp"));

        vm.Settings.Format = Format.Tiff;
        Assert.That(vm.Settings.Destination, Does.EndWith(".test_tiff"));

        vm.Settings.Format = Format.Psd;
        Assert.That(vm.Settings.Destination, Does.EndWith(".psd"));

        vm.Settings.Format = Format.Pdf;
        Assert.That(vm.Settings.Destination, Does.EndWith(".test_pdf"));
    }
}
