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
namespace Cube.Pdf.Converter.Tests.Texts;

using System.Linq;
using System.Threading;
using NUnit.Framework;

/* ------------------------------------------------------------------------- */
///
/// ResourceTest
///
/// <summary>
/// Checkes the texts of the Resource class.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[TestFixture]
[Apartment(ApartmentState.STA)]
class ResourceTest
{
    #region Tests

    /* ----------------------------------------------------------------- */
    ///
    /// English
    ///
    /// <summary>
    /// Checks the English texts.
    /// </summary>
    ///
    /* ----------------------------------------------------------------- */
    [Test]
    public void English()
    {
        using var view = new MainWindow();
        var ss = new SettingFolder();
        var vm = new MainViewModel(ss);
        view.Bind(vm);
        vm.Settings.Language = Language.English;

        var c0 = Resource.Languages.ToArray();
        Assert.That(c0.Length,  Is.EqualTo(4), nameof(Resource.Languages));
        Assert.That(c0[0].Key,  Is.EqualTo("Auto"));
        Assert.That(c0[1].Key,  Is.EqualTo("English"));
        Assert.That(c0[2].Key,  Is.EqualTo("Japanese"));
        Assert.That(c0[3].Key,  Is.EqualTo("German"));

        var c1 = Resource.Formats.ToArray();
        Assert.That(c1.Length,  Is.EqualTo(7), nameof(Resource.Formats));
        Assert.That(c1[0].Key,  Is.EqualTo("PDF"));
        Assert.That(c1[1].Key,  Is.EqualTo("PS"));
        Assert.That(c1[2].Key,  Is.EqualTo("EPS"));
        Assert.That(c1[3].Key,  Is.EqualTo("PNG"));
        Assert.That(c1[4].Key,  Is.EqualTo("JPEG"));
        Assert.That(c1[5].Key,  Is.EqualTo("BMP"));
        Assert.That(c1[6].Key,  Is.EqualTo("TIFF"));

        var c2 = Resource.PdfVersions.ToArray();
        Assert.That(c2.Length,  Is.EqualTo(6), nameof(Resource.PdfVersions));
        Assert.That(c2[0].Key,  Is.EqualTo("PDF 1.7"));
        Assert.That(c2[1].Key,  Is.EqualTo("PDF 1.6"));
        Assert.That(c2[2].Key,  Is.EqualTo("PDF 1.5"));
        Assert.That(c2[3].Key,  Is.EqualTo("PDF 1.4"));
        Assert.That(c2[4].Key,  Is.EqualTo("PDF 1.3"));
        Assert.That(c2[5].Key,  Is.EqualTo("PDF 1.2"));

        var c3 = Resource.SaveOptions.ToArray();
        Assert.That(c3.Length,  Is.EqualTo(4), nameof(Resource.SaveOptions));
        Assert.That(c3[0].Key,  Is.EqualTo("Overwrite"));
        Assert.That(c3[1].Key,  Is.EqualTo("Merge at the beginning"));
        Assert.That(c3[2].Key,  Is.EqualTo("Merge at the end"));
        Assert.That(c3[3].Key,  Is.EqualTo("Rename"));

        var c4 = Resource.ViewerOptions.ToArray();
        Assert.That(c4.Length,  Is.EqualTo(6), nameof(Resource.ViewerOptions));
        Assert.That(c4[0].Key,  Is.EqualTo("Single page"));
        Assert.That(c4[1].Key,  Is.EqualTo("One column"));
        Assert.That(c4[2].Key,  Is.EqualTo("Two page (left)"));
        Assert.That(c4[3].Key,  Is.EqualTo("Two page (right)"));
        Assert.That(c4[4].Key,  Is.EqualTo("Two column (left)"));
        Assert.That(c4[5].Key,  Is.EqualTo("Two column (right)"));

        var c5 = Resource.PostProcesses.ToArray();
        Assert.That(c5.Length,  Is.EqualTo(4), nameof(Resource.PostProcesses));
        Assert.That(c5[0].Key,  Is.EqualTo("Open"));
        Assert.That(c5[1].Key,  Is.EqualTo("Open directory"));
        Assert.That(c5[2].Key,  Is.EqualTo("None"));
        Assert.That(c5[3].Key,  Is.EqualTo("Others"));

        var c6 = Resource.Orientations.ToArray();
        Assert.That(c6.Length,  Is.EqualTo(3), nameof(Resource.Orientations));
        Assert.That(c6[0].Key,  Is.EqualTo("Portrait"));
        Assert.That(c6[1].Key,  Is.EqualTo("Landscape"));
        Assert.That(c6[2].Key,  Is.EqualTo("Auto"));

        var c7 = Resource.ColorModes.ToArray();
        Assert.That(c7.Length,  Is.EqualTo(4), nameof(Resource.ColorModes));
        Assert.That(c7[0].Key,  Is.EqualTo("Auto"));
        Assert.That(c7[1].Key,  Is.EqualTo("RGB"));
        Assert.That(c7[2].Key,  Is.EqualTo("Grayscale"));
        Assert.That(c7[3].Key,  Is.EqualTo("Monochrome"));

        var f0 = ss.Value.Extensions.GetSourceFilters().ToArray();
        Assert.That(f0.Length,  Is.EqualTo(4), nameof(Resource.GetSourceFilters));
        Assert.That(f0[0].Text, Is.EqualTo("PS files"));
        Assert.That(f0[1].Text, Is.EqualTo("EPS files"));
        Assert.That(f0[2].Text, Is.EqualTo("PDF files"));
        Assert.That(f0[3].Text, Is.EqualTo("All files"));

        var f1 = ss.Value.Extensions.GetDestinationFilters().ToArray();
        Assert.That(f1.Length,  Is.EqualTo(7), nameof(Resource.GetDestinationFilters));
        Assert.That(f1[0].Text, Is.EqualTo("PDF files"));
        Assert.That(f1[1].Text, Is.EqualTo("PS files"));
        Assert.That(f1[2].Text, Is.EqualTo("EPS files"));
        Assert.That(f1[3].Text, Is.EqualTo("PNG files"));
        Assert.That(f1[4].Text, Is.EqualTo("JPEG files"));
        Assert.That(f1[5].Text, Is.EqualTo("BMP files"));
        Assert.That(f1[6].Text, Is.EqualTo("TIFF files"));

        var f2 = ss.Value.Extensions.GetProgramFilters().ToArray();
        Assert.That(f2.Length,  Is.EqualTo(2), nameof(Resource.GetProgramFilters));
        Assert.That(f2[0].Text, Is.EqualTo("Executable files"));
        Assert.That(f2[1].Text, Is.EqualTo("All files"));
    }

    /* ----------------------------------------------------------------- */
    ///
    /// Japanese
    ///
    /// <summary>
    /// Checks the Japanese texts.
    /// </summary>
    ///
    /* ----------------------------------------------------------------- */
    [Test]
    public void Japanese()
    {
        using var view = new MainWindow();
        var ss = new SettingFolder();
        var vm = new MainViewModel(ss);
        view.Bind(vm);
        vm.Settings.Language = Language.Japanese;

        var c0 = Resource.Languages.ToArray();
        Assert.That(c0.Length,  Is.EqualTo(4), nameof(Resource.Languages));
        Assert.That(c0[0].Key,  Is.EqualTo("Auto"));
        Assert.That(c0[1].Key,  Is.EqualTo("English"));
        Assert.That(c0[2].Key,  Is.EqualTo("Japanese"));
        Assert.That(c0[3].Key,  Is.EqualTo("German"));

        var c1 = Resource.Formats.ToArray();
        Assert.That(c1.Length,  Is.EqualTo(7), nameof(Resource.Formats));
        Assert.That(c1[0].Key,  Is.EqualTo("PDF"));
        Assert.That(c1[1].Key,  Is.EqualTo("PS"));
        Assert.That(c1[2].Key,  Is.EqualTo("EPS"));
        Assert.That(c1[3].Key,  Is.EqualTo("PNG"));
        Assert.That(c1[4].Key,  Is.EqualTo("JPEG"));
        Assert.That(c1[5].Key,  Is.EqualTo("BMP"));
        Assert.That(c1[6].Key,  Is.EqualTo("TIFF"));

        var c2 = Resource.PdfVersions.ToArray();
        Assert.That(c2.Length,  Is.EqualTo(6), nameof(Resource.PdfVersions));
        Assert.That(c2[0].Key,  Is.EqualTo("PDF 1.7"));
        Assert.That(c2[1].Key,  Is.EqualTo("PDF 1.6"));
        Assert.That(c2[2].Key,  Is.EqualTo("PDF 1.5"));
        Assert.That(c2[3].Key,  Is.EqualTo("PDF 1.4"));
        Assert.That(c2[4].Key,  Is.EqualTo("PDF 1.3"));
        Assert.That(c2[5].Key,  Is.EqualTo("PDF 1.2"));

        var c3 = Resource.SaveOptions.ToArray();
        Assert.That(c3.Length,  Is.EqualTo(4), nameof(Resource.SaveOptions));
        Assert.That(c3[0].Key,  Is.EqualTo("上書き"));
        Assert.That(c3[1].Key,  Is.EqualTo("先頭に結合"));
        Assert.That(c3[2].Key,  Is.EqualTo("末尾に結合"));
        Assert.That(c3[3].Key,  Is.EqualTo("リネーム"));

        var c4 = Resource.ViewerOptions.ToArray();
        Assert.That(c4.Length,  Is.EqualTo(6), nameof(Resource.ViewerOptions));
        Assert.That(c4[0].Key,  Is.EqualTo("単一ページ"));
        Assert.That(c4[1].Key,  Is.EqualTo("連続ページ"));
        Assert.That(c4[2].Key,  Is.EqualTo("見開きページ（左綴じ）"));
        Assert.That(c4[3].Key,  Is.EqualTo("見開きページ（右綴じ）"));
        Assert.That(c4[4].Key,  Is.EqualTo("連続見開きページ（左綴じ）"));
        Assert.That(c4[5].Key,  Is.EqualTo("連続見開きページ（右綴じ）"));

        var c5 = Resource.PostProcesses.ToArray();
        Assert.That(c5.Length,  Is.EqualTo(4), nameof(Resource.PostProcesses));
        Assert.That(c5[0].Key,  Is.EqualTo("開く"));
        Assert.That(c5[1].Key,  Is.EqualTo("フォルダーを開く"));
        Assert.That(c5[2].Key,  Is.EqualTo("何もしない"));
        Assert.That(c5[3].Key,  Is.EqualTo("その他"));

        var c6 = Resource.Orientations.ToArray();
        Assert.That(c6.Length,  Is.EqualTo(3), nameof(Resource.Orientations));
        Assert.That(c6[0].Key,  Is.EqualTo("縦"));
        Assert.That(c6[1].Key,  Is.EqualTo("横"));
        Assert.That(c6[2].Key,  Is.EqualTo("自動"));

        var c7 = Resource.ColorModes.ToArray();
        Assert.That(c7.Length,  Is.EqualTo(4), nameof(Resource.ColorModes));
        Assert.That(c7[0].Key,  Is.EqualTo("自動"));
        Assert.That(c7[1].Key,  Is.EqualTo("RGB"));
        Assert.That(c7[2].Key,  Is.EqualTo("グレースケール"));
        Assert.That(c7[3].Key,  Is.EqualTo("白黒"));

        var f0 = ss.Value.Extensions.GetSourceFilters().ToArray();
        Assert.That(f0.Length,  Is.EqualTo(4), nameof(Resource.GetSourceFilters));
        Assert.That(f0[0].Text, Is.EqualTo("PS ファイル"));
        Assert.That(f0[1].Text, Is.EqualTo("EPS ファイル"));
        Assert.That(f0[2].Text, Is.EqualTo("PDF ファイル"));
        Assert.That(f0[3].Text, Is.EqualTo("すべてのファイル"));

        var f1 = ss.Value.Extensions.GetDestinationFilters().ToArray();
        Assert.That(f1.Length,  Is.EqualTo(7), nameof(Resource.GetDestinationFilters));
        Assert.That(f1[0].Text, Is.EqualTo("PDF ファイル"));
        Assert.That(f1[1].Text, Is.EqualTo("PS ファイル"));
        Assert.That(f1[2].Text, Is.EqualTo("EPS ファイル"));
        Assert.That(f1[3].Text, Is.EqualTo("PNG ファイル"));
        Assert.That(f1[4].Text, Is.EqualTo("JPEG ファイル"));
        Assert.That(f1[5].Text, Is.EqualTo("BMP ファイル"));
        Assert.That(f1[6].Text, Is.EqualTo("TIFF ファイル"));

        var f2 = ss.Value.Extensions.GetProgramFilters().ToArray();
        Assert.That(f2.Length,  Is.EqualTo(2), nameof(Resource.GetProgramFilters));
        Assert.That(f2[0].Text, Is.EqualTo("実行可能なファイル"));
        Assert.That(f2[1].Text, Is.EqualTo("すべてのファイル"));
    }

    #endregion

    #region Others

    /* ----------------------------------------------------------------- */
    ///
    /// Teardown
    ///
    /// <summary>
    /// Invokes after each test.
    /// </summary>
    ///
    /* ----------------------------------------------------------------- */
    [TearDown]
    public void Teardown() => Locale.Set(Language.Auto);

    #endregion
}
