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
using NUnit.Framework;
using System.Linq;
using System.Threading;

namespace Cube.Pdf.Pages.Tests
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
            using (var view = new MainWindow())
            {
                view.Bind(new MainViewModel(new SynchronizationContext()));
                Assert.That(view.SelectedIndices.Count(), Is.EqualTo(0));
            }
        }

        #endregion
    }
}
