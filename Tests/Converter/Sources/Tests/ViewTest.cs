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
using System.Threading;
using System.Windows.Forms;
using NUnit.Framework;

namespace Cube.Pdf.Converter.Tests;

/* ------------------------------------------------------------------------- */
///
/// ViewTest
///
/// <summary>
/// Tests the MainWindow class.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[TestFixture]
[Apartment(ApartmentState.STA)]
class ViewTest
{
    /* --------------------------------------------------------------------- */
    ///
    /// Test
    ///
    /// <summary>
    /// Tests the Show and Close methods.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Test]
    public void Test()
    {
        var src  = new SettingFolder();
        src.Set(new[] { "-DocumentName", "WindowTest" });

        using var view = new MainWindow();
        view.Bind(new MainViewModel(src));
        view.StartPosition = FormStartPosition.Manual;
        view.Location = new(0, -view.Height);
        view.Show();

        Assert.That(view.Busy, Is.False);
        Assert.That(Locale.Language, Is.EqualTo(Language.Auto));
        Assert.That(view.Text, Does.StartWith("WindowTest - CubePDF 3.1.0 ("));
        Locale.Set(Language.Japanese);
        Assert.That(view.Text, Does.StartWith("WindowTest - CubePDF 3.1.0 ("));

        view.Close();
    }
}
