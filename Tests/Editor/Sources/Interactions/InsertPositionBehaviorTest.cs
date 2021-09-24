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
using System.Windows.Controls.Primitives;
using NUnit.Framework;

namespace Cube.Pdf.Editor.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// InsertPositionBehaviorTest
    ///
    /// <summary>
    /// Tests the InsertPositionBehavior class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    class InsertPositionBehaviorTest
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Test
        ///
        /// <summary>
        /// Tests the create, attach, detach, and some other methods.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Test()
        {
            var view = new ToggleButton();
            var src  = new InsertPositionBehavior();

            Assert.That(src.Command, Is.Null);
            Assert.That(src.CommandParameter, Is.EqualTo(0));

            src.Attach(view);
            view.IsChecked = true;
            view.IsChecked = false;
            src.Detach();
        }

        #endregion
    }
}
