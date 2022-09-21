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

using System.Threading;
using Cube.Pdf.Ghostscript;
using NUnit.Framework;

/* ------------------------------------------------------------------------- */
///
/// MessageTest
///
/// <summary>
/// Checkes the texts of the Message class.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[TestFixture]
[Apartment(ApartmentState.STA)]
class MessageTest
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

        var e0 = Message.From(new GsApiException(GsApiStatus.Fatal));
        Assert.That(e0.Title, Is.EqualTo("CubePDF"));
        Assert.That(e0.Text,  Is.EqualTo("Ghostscript error (-100)"));

        var w0 = Message.From("dummy", SaveOption.Overwrite);
        Assert.That(w0.Title, Is.EqualTo("CubePDF"));
        Assert.That(w0.Text,  Does.StartWith("dummy already exists."));
        Assert.That(w0.Text,  Does.EndWith("Do you want to overwrite the file?"));

        var w1 = Message.From("dummy", SaveOption.MergeHead);
        Assert.That(w1.Text,  Does.EndWith("Do you want to merge at the beginning of the existing file?"));

        var w2 = Message.From("dummy", SaveOption.MergeTail);
        Assert.That(w2.Text,  Does.EndWith("Do you want to merge at the end of the existing file?"));

        var f0 = Message.ForSource(ss);
        Assert.That(f0.Text,  Is.EqualTo("Select source file"));

        var f1 = Message.ForDestination(ss);
        Assert.That(f1.Text,  Is.EqualTo("Save as"));

        var f2 = Message.ForUserProgram(ss);
        Assert.That(f2.Text,  Is.EqualTo("Select user program"));
    }

    /* ----------------------------------------------------------------- */
    ///
    /// English
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

        var e0 = Message.From(new GsApiException(GsApiStatus.Fatal));
        Assert.That(e0.Title, Is.EqualTo("CubePDF"));
        Assert.That(e0.Text,  Is.EqualTo("Ghostscript API による変換中にエラーが発生しました。(-100)"));

        var w0 = Message.From("dummy", SaveOption.Overwrite);
        Assert.That(w0.Title, Is.EqualTo("CubePDF"));
        Assert.That(w0.Text,  Does.StartWith("dummy は既に存在します。"));
        Assert.That(w0.Text,  Does.EndWith("上書きしますか？"));

        var w1 = Message.From("dummy", SaveOption.MergeHead);
        Assert.That(w1.Text,  Does.EndWith("先頭に結合しますか？"));

        var w2 = Message.From("dummy", SaveOption.MergeTail);
        Assert.That(w2.Text,  Does.EndWith("末尾に結合しますか？"));

        var f0 = Message.ForSource(ss);
        Assert.That(f0.Text,  Is.EqualTo("入力ファイルを選択"));

        var f1 = Message.ForDestination(ss);
        Assert.That(f1.Text,  Is.EqualTo("名前を付けて保存"));

        var f2 = Message.ForUserProgram(ss);
        Assert.That(f2.Text,  Is.EqualTo("変換完了時に実行するプログラムを選択"));
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
