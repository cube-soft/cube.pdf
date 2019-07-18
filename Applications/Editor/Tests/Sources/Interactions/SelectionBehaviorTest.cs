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
    /// SelectionBehaviorTest
    ///
    /// <summary>
    /// Tests for the SelectionBehavior class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    class SelectionBehaviorTest : ViewModelFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Tests the create, attach, and detach methods.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Create()
        {
            var view = new ListView();
            var src  = new SelectionBehavior();

            src.Attach(view);
            Assert.That(src.Popup, Is.False);
            src.Detach();
        }

        #endregion
    }
}
