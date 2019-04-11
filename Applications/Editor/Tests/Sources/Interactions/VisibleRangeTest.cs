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
using System.Threading;
using System.Windows.Controls;

namespace Cube.Pdf.Editor.Tests.Interactions
{
    /* --------------------------------------------------------------------- */
    ///
    /// VisibleRangeTest
    ///
    /// <summary>
    /// Tests for the VisibleRange class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    class VisibleRangeTest : ViewModelFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Properties
        ///
        /// <summary>
        /// Confirms default values of properties.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Properties() => Create(vm =>
        {
            var view = new ScrollViewer { DataContext = vm };
            var src  = new VisibleRange();

            src.Attach(view);
            Assert.That(src.First, Is.EqualTo(0));
            Assert.That(src.Last,  Is.EqualTo(0));
            Assert.That(src.Unit,  Is.EqualTo(0));
            src.Detach();
        });

        #endregion
    }
}
