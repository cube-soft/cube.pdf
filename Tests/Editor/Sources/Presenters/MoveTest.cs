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
using NUnit.Framework;

namespace Cube.Pdf.Editor.Tests.Presenters
{
    /* --------------------------------------------------------------------- */
    ///
    /// MoveTest
    ///
    /// <summary>
    /// Tests the Move commands.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class MoveTest : VmFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// MoveNext
        ///
        /// <summary>
        /// Executes the test for moving selected items.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void MoveNext()
        {
            using var vm = NewVM();
            using var z0 = vm.Boot(new() { Source = GetSource("Sample.pdf") });

            vm.Select(1, 3, 8);
            vm.Test(vm.Ribbon.MoveNext);

            var dest = vm.Value.Images.ToList();
            Assert.That(dest.Count,               Is.EqualTo(9));
            Assert.That(dest[0].RawObject.Number, Is.EqualTo(1));
            Assert.That(dest[1].RawObject.Number, Is.EqualTo(3));
            Assert.That(dest[2].RawObject.Number, Is.EqualTo(2));
            Assert.That(dest[3].RawObject.Number, Is.EqualTo(5));
            Assert.That(dest[4].RawObject.Number, Is.EqualTo(4));
            Assert.That(dest[5].RawObject.Number, Is.EqualTo(6));
            Assert.That(dest[6].RawObject.Number, Is.EqualTo(7));
            Assert.That(dest[7].RawObject.Number, Is.EqualTo(8));
            Assert.That(dest[8].RawObject.Number, Is.EqualTo(9));
            for (var i = 0; i < dest.Count; ++i) Assert.That(dest[i].Index, Is.EqualTo(i));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// MovePrevious
        ///
        /// <summary>
        /// Executes the test for moving selected items.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void MovePrevious()
        {
            using var vm = NewVM();
            using var z0 = vm.Boot(new() { Source = GetSource("Sample.pdf") });

            vm.Select(0, 3, 6);
            vm.Test(vm.Ribbon.MovePrevious);

            var dest = vm.Value.Images.ToList();
            Assert.That(dest.Count,               Is.EqualTo(9));
            Assert.That(dest[0].RawObject.Number, Is.EqualTo(1));
            Assert.That(dest[1].RawObject.Number, Is.EqualTo(2));
            Assert.That(dest[2].RawObject.Number, Is.EqualTo(4));
            Assert.That(dest[3].RawObject.Number, Is.EqualTo(3));
            Assert.That(dest[4].RawObject.Number, Is.EqualTo(5));
            Assert.That(dest[5].RawObject.Number, Is.EqualTo(7));
            Assert.That(dest[6].RawObject.Number, Is.EqualTo(6));
            Assert.That(dest[7].RawObject.Number, Is.EqualTo(8));
            Assert.That(dest[8].RawObject.Number, Is.EqualTo(9));
            for (var i = 0; i < dest.Count; ++i) Assert.That(dest[i].Index, Is.EqualTo(i));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// MoveNext_DragDrop
        ///
        /// <summary>
        /// Executes the test for moving selected items by Drag&amp;Drop.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void MoveNext_DragDrop()
        {
            using var vm = NewVM();
            using var z0 = vm.Boot(new() { Source = GetSource("Sample.pdf") });

            var obj = new DragDropObject(1) { DropIndex = 4 };
            vm.Select(1, 3, 6);
            vm.Test(() => vm.InsertOrMove.Execute(obj));

            var dest = vm.Value.Images.ToList();
            Assert.That(dest.Count,               Is.EqualTo(9));
            Assert.That(dest[0].RawObject.Number, Is.EqualTo(1));
            Assert.That(dest[1].RawObject.Number, Is.EqualTo(3));
            Assert.That(dest[2].RawObject.Number, Is.EqualTo(5));
            Assert.That(dest[3].RawObject.Number, Is.EqualTo(2));
            Assert.That(dest[4].RawObject.Number, Is.EqualTo(6));
            Assert.That(dest[5].RawObject.Number, Is.EqualTo(4));
            Assert.That(dest[6].RawObject.Number, Is.EqualTo(8));
            Assert.That(dest[7].RawObject.Number, Is.EqualTo(9));
            Assert.That(dest[8].RawObject.Number, Is.EqualTo(7));
            for (var i = 0; i < dest.Count; ++i) Assert.That(dest[i].Index, Is.EqualTo(i));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// MovePrevious_DragDrop
        ///
        /// <summary>
        /// Executes the test for moving selected items by Drag&amp;Drop.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void MovePrevious_DragDrop()
        {
            using var vm = NewVM();
            using var z0 = vm.Boot(new() { Source = GetSource("Sample.pdf") });

            var obj = new DragDropObject(6) { DropIndex = 3 };
            vm.Select(1, 3, 6);
            vm.Test(() => vm.InsertOrMove.Execute(obj));

            var dest = vm.Value.Images.ToList();
            Assert.That(dest.Count,               Is.EqualTo(9));
            Assert.That(dest[0].RawObject.Number, Is.EqualTo(2));
            Assert.That(dest[1].RawObject.Number, Is.EqualTo(4));
            Assert.That(dest[2].RawObject.Number, Is.EqualTo(1));
            Assert.That(dest[3].RawObject.Number, Is.EqualTo(3));
            Assert.That(dest[4].RawObject.Number, Is.EqualTo(7));
            Assert.That(dest[5].RawObject.Number, Is.EqualTo(5));
            Assert.That(dest[6].RawObject.Number, Is.EqualTo(6));
            Assert.That(dest[7].RawObject.Number, Is.EqualTo(8));
            Assert.That(dest[8].RawObject.Number, Is.EqualTo(9));
            for (var i = 0; i < dest.Count; ++i) Assert.That(dest[i].Index, Is.EqualTo(i));
        }

        #endregion
    }
}
