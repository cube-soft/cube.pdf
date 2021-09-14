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
using System.Windows.Controls;
using NUnit.Framework;

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
    class VisibleRangeTest : VmFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Tests the create, attach, detach, and some other methods.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Invoke()
        {
            var view = new ScrollViewer();
            var src  = new VisibleRange();

            Assert.That(view.ActualWidth,  Is.EqualTo(0), nameof(view.ActualWidth));
            Assert.That(view.ActualHeight, Is.EqualTo(0), nameof(view.ActualHeight));
            src.Attach(view);

            Assert.That(src.First, Is.EqualTo(0), nameof(src.First));
            Assert.That(src.Last,  Is.EqualTo(0), nameof(src.Last));
            Assert.That(src.Unit,  Is.EqualTo(0), nameof(src.Unit));
            src.Unit = 100;
            Assert.That(src.First, Is.EqualTo(0), nameof(src.First));
            Assert.That(src.Last,  Is.EqualTo(3), nameof(src.Last));
            Assert.That(src.Unit,  Is.EqualTo(100), nameof(src.Unit));

            src.Detach();
        }

        #endregion
    }
}
