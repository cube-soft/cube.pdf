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
using System.Linq;
using Cube.Mixin.Commands;
using Cube.Tests;
using NUnit.Framework;

namespace Cube.Pdf.Editor.Tests.Presenters
{
    /* --------------------------------------------------------------------- */
    ///
    /// ViewTest
    ///
    /// <summary>
    /// Tests the display operations of the MainViewModel class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class ViewTest : VmFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Preview
        ///
        /// <summary>
        /// Tests the Preview command.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Preview()
        {
            using var vm = NewVM();
            using var z0 = vm.Boot(new() { Source = GetSource("Sample.pdf") });
            using var z1 = vm.Subscribe<PreviewViewModel>(pvm => {
                Assert.That(Wait.For(() => pvm.Value.Image != null), "Timeout");
                Assert.That(pvm.Title,        Is.Not.Null.And.Not.Empty);
                Assert.That(pvm.Value.File,   Is.Not.Null);
                Assert.That(pvm.Value.Width,  Is.GreaterThan(0));
                Assert.That(pvm.Value.Height, Is.GreaterThan(0));
                Assert.That(pvm.Value.Busy,   Is.False);
                pvm.Cancel.Command.Execute();
            });

            vm.Test(vm.Ribbon.Select);
            Assert.That(vm.Ribbon.Preview.Command.CanExecute(), Is.True);
            vm.Ribbon.Preview.Command.Execute();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Select
        ///
        /// <summary>
        /// Tests the Select commands.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Select()
        {
            using var vm = NewVM();
            using var z0 = vm.Boot(new() { Source = GetSource("Sample.pdf") });

            var unit    = 3; // Number of PropertyChanged events per action.
            var changed = 0;
            var dest    = vm.Value.Images.Selection;
            dest.PropertyChanged += (s, e) => ++changed;

            Assert.That(dest.Count,   Is.EqualTo(0));
            Assert.That(dest.Indices, Is.Not.Null);
            Assert.That(dest.Last,    Is.EqualTo(-1));

            vm.Select(0);
            Assert.That(changed,    Is.EqualTo(1 * unit));
            Assert.That(dest.Count, Is.EqualTo(1), nameof(dest.Count));
            Assert.That(dest.Last,  Is.EqualTo(0), nameof(dest.Last));

            vm.Test(vm.Ribbon.SelectFlip);
            Assert.That(changed,    Is.EqualTo(10 * unit));
            Assert.That(dest.Count, Is.EqualTo(8), nameof(dest.Count));
            Assert.That(dest.Last,  Is.EqualTo(8), nameof(dest.Last));

            vm.Test(vm.Ribbon.Select); // SelectAll
            Assert.That(changed,    Is.EqualTo(11 * unit));
            Assert.That(dest.Count, Is.EqualTo(9), nameof(dest.Count));
            Assert.That(dest.Last,  Is.EqualTo(8), nameof(dest.Last));

            vm.Test(vm.Ribbon.Select); // SelectClear
            Assert.That(changed,    Is.EqualTo(20 * unit));
            Assert.That(dest.Count, Is.EqualTo(0));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Zoom
        ///
        /// <summary>
        /// Tests the Zoom command.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Zoom()
        {
            using var vm = NewVM();
            using var z0 = vm.Boot(new() { Source = GetSource("Sample.pdf") });

            var ip = vm.Value.Images.Preferences;
            Assert.That(ip.ItemSizeOptions.Count, Is.EqualTo(9));
            Assert.That(ip.ItemSizeIndex,         Is.EqualTo(3));
            Assert.That(ip.ItemSize,              Is.EqualTo(250));

            vm.Value.ItemSize = 325;
            Assert.That(Wait.For(() => !vm.Value.Busy), "Timeout");
            Assert.That(ip.ItemSizeIndex, Is.EqualTo(4));
            Assert.That(ip.ItemSize,      Is.EqualTo(300));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// FrameOnly
        ///
        /// <summary>
        /// Tests the FrameOnly property.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void FrameOnly()
        {
            using var vm = NewVM();
            using var z0 = vm.Boot(new() { Source = GetSource("Sample.pdf") });

            Assert.That(vm.Ribbon.FrameOnly.Value, Is.False);
            vm.Ribbon.FrameOnly.Value = true;
            foreach (var item in vm.Value.Images) Assert.That(item.Image, Is.Null);
        }

        #endregion
    }
}
