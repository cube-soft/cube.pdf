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
using NUnit.Framework;

namespace Cube.Pdf.Converter.Tests.Views
{
    /* --------------------------------------------------------------------- */
    ///
    /// MainWindowTest
    ///
    /// <summary>
    /// Tests the MainWindowTest class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    class MainWindowTest
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Bind
        ///
        /// <summary>
        /// Tests the Bind method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Bind()
        {
            var src  = new SettingFolder();
            src.Set(new[] { "-DocumentName", "WindowTest" });

            using var view = new MainWindow();
            view.Bind(new MainViewModel(src));
            Assert.That(view.Text, Does.Contain("WindowTest"));

            Assert.That(Locale.Language, Is.EqualTo(Language.Auto));
            Locale.Set(Language.Japanese);
            Assert.That(view.Text, Does.Contain("WindowTest"));

        }

        #endregion
    }
}
