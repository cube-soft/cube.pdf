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
using System.Windows;
using System.Windows.Controls;

namespace Cube.Pdf.Editor.Tests.Interactions
{
    /* --------------------------------------------------------------------- */
    ///
    /// MouseMoveBehaviorTest
    ///
    /// <summary>
    /// Tests for the MouseMoveBehavior class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    class MouseMoveBehaviorTest : ViewModelFixture
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
            var view = new ListView { DataContext = vm };
            var src  = new MouseMoveBehavior();

            src.Attach(view);
            Assert.That(src.Command,   Is.Null);
            Assert.That(src.Selection, Is.Null);
            Assert.That(src.DrawingCanvas.Visibility,       Is.EqualTo(Visibility.Collapsed));
            Assert.That(src.Drawing.BorderBrush.ToString(), Is.EqualTo(SystemColors.HotTrackBrush.ToString()));
            Assert.That(src.Drawing.BorderThickness,        Is.EqualTo(new Thickness(1)));
            Assert.That(src.Drawing.Background.Opacity,     Is.EqualTo(0.1));
            Assert.That(src.Drawing.CornerRadius,           Is.EqualTo(new CornerRadius(1)));
            src.Detach();
        });

        /* ----------------------------------------------------------------- */
        ///
        /// IsPressed
        ///
        /// <summary>
        /// Executes the test of the IsPressed extended method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void IsPressed() => Assert.That(Keys.ModifierKeys.IsPressed(), Is.False);

        #endregion
    }
}
