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
    /// RemoveTest
    ///
    /// <summary>
    /// Tests the Remove commands and the RemoveViewModel class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class RemoveTest : VmFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Remove
        ///
        /// <summary>
        /// Tests to remove the selected items.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Remove()
        {
            using var vm = NewVM();
            using var z0 = vm.Boot(new() { Source = GetSource("Sample.pdf") });

            vm.Select(3, 5);
            vm.Test(vm.Ribbon.Remove);

            var dest = vm.Value.Images.ToList();
            Assert.That(dest.Count,               Is.EqualTo(7));
            Assert.That(dest[0].RawObject.Number, Is.EqualTo(1));
            Assert.That(dest[1].RawObject.Number, Is.EqualTo(2));
            Assert.That(dest[2].RawObject.Number, Is.EqualTo(3));
            Assert.That(dest[3].RawObject.Number, Is.EqualTo(5));
            Assert.That(dest[4].RawObject.Number, Is.EqualTo(7));
            Assert.That(dest[5].RawObject.Number, Is.EqualTo(8));
            Assert.That(dest[6].RawObject.Number, Is.EqualTo(9));
            for (var i = 0; i < dest.Count; ++i) Assert.That(dest[i].Index, Is.EqualTo(i));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// RemoveOthers
        ///
        /// <summary>
        /// Tests the RemoveOthers command.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void RemoveOthers()
        {
            using var vm = NewVM();
            using var z0 = vm.Boot(new() { Source = GetSource("Sample.pdf") });
            using var z1 = vm.Subscribe<RemoveViewModel>(e => {
                Assert.That(e.Title,        Is.EqualTo("Removal details"));
                Assert.That(e.Count.Text,   Is.EqualTo("Page count"));
                Assert.That(e.Count.Value,  Is.AtLeast(1));
                Assert.That(e.Range.Text,   Is.EqualTo("Target pages"));
                Assert.That(e.Range.Value,  Is.Empty);
                Assert.That(e.Example.Text, Is.EqualTo("e.g. 1,2,4-7,9"));

                Assert.That(e.OK.Command.CanExecute(), Is.False);
                e.Range.Value = "1,3,5-7,9";
                Assert.That(e.OK.Command.CanExecute(), Is.True);
                e.OK.Command.Execute();
            });

            Assert.That(vm.Ribbon.RemoveOthers.Command.CanExecute());
            vm.Ribbon.RemoveOthers.Command.Execute();
            Assert.That(Wait.For(() => vm.Value.Count == 3), "Timeout");

            var dest = vm.Value.Images.ToList();
            Assert.That(dest.Count,               Is.EqualTo(3));
            Assert.That(dest[0].RawObject.Number, Is.EqualTo(2));
            Assert.That(dest[1].RawObject.Number, Is.EqualTo(4));
            Assert.That(dest[2].RawObject.Number, Is.EqualTo(8));
            for (var i = 0; i < dest.Count; ++i) Assert.That(dest[i].Index, Is.EqualTo(i));
        }

        #endregion
    }
}
