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
using Cube.FileSystem.TestService;
using Cube.Xui.Mixin;
using NUnit.Framework;
using System.Linq;
using System.Threading;

namespace Cube.Pdf.Tests.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// MainViewModelTest
    ///
    /// <summary>
    /// Tests for the MainViewModel class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class MainViewModelTest : ViewModelFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Open
        ///
        /// <summary>
        /// Tests to open a PDF document and create images as an
        /// asynchronous operation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("Sample.pdf", 2)]
        public void Open(string filename, int n) => Create(filename, n, vm =>
        {
            var pref = vm.Data.Preferences;
            Assert.That(pref.ItemSize,      Is.EqualTo(250));
            Assert.That(pref.ItemSizeIndex, Is.EqualTo(3));
            Assert.That(pref.ItemMargin,    Is.EqualTo(3));
            Assert.That(pref.TextHeight,    Is.EqualTo(25));

            var images = vm.Data.Images.ToList();
            foreach (var item in images) Assert.That(item.Image, Is.Not.Null);

            var dest = images[0];
            var cts  = new CancellationTokenSource();
            dest.PropertyChanged += (s, e) => cts.Cancel();
            Execute(vm, vm.Ribbon.Refresh);
            Assert.That(Wait.For(cts.Token), nameof(vm.Ribbon.Refresh));
            Assert.That(dest.Image, Is.Not.EqualTo(pref.Dummy));
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// Tests to save a PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Save() => Create("Sample.pdf", 2, vm =>
        {
            Destination = GetResultsWith($"Sample{nameof(Save)}.pdf");
            Assert.That(IO.Exists(Destination), Is.False);

            Execute(vm, vm.Ribbon.SaveAs);
            Assert.That(Wait.For(() => IO.Exists(Destination)));
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Close
        ///
        /// <summary>
        /// Tests to close a PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Close() => Create(vm =>
        {
            Source = GetResultsWith($"{nameof(Close)}Sample.pdf");
            IO.Copy(GetExamplesWith("Sample.pdf"), Source, true);
            Execute(vm, vm.Ribbon.Open);
            Assert.That(Wait.For(() => vm.Data.Images.Count == 2), nameof(vm.Ribbon.Open));
            Execute(vm, vm.Ribbon.Close);
            Assert.That(IO.TryDelete(Source), Is.True);
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Select
        ///
        /// <summary>
        /// Tests to select items.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Select() => Create("SampleRotation.pdf", 9, vm =>
        {
            var dest = vm.Data.Selection;
            Assert.That(dest.Count,   Is.EqualTo(0));
            Assert.That(dest.Items,   Is.Not.Null);
            Assert.That(dest.Indices, Is.Not.Null);
            Assert.That(dest.Index,   Is.EqualTo(-1));

            vm.Data.Images.First().IsSelected = true;
            Assert.That(Wait.For(() => !vm.Data.IsBusy.Value));
            Assert.That(dest.Count,   Is.EqualTo(1), nameof(dest.Count));
            Assert.That(dest.Index,   Is.EqualTo(0), nameof(dest.Index));

            Execute(vm, vm.Ribbon.SelectFlip);
            Assert.That(dest.Count,   Is.EqualTo(8), nameof(dest.Count));
            Assert.That(dest.Index,   Is.EqualTo(8), nameof(dest.Index));

            Execute(vm, vm.Ribbon.Select); // SelectAll
            Assert.That(dest.Count,   Is.EqualTo(9), nameof(dest.Count));
            Assert.That(dest.Index,   Is.EqualTo(8), nameof(dest.Index));

            Execute(vm, vm.Ribbon.Select); // SelectClear
            Assert.That(dest.Count,   Is.EqualTo(0));
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Insert
        ///
        /// <summary>
        /// Tests to insert a new PDF behind the selected index.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Insert() => Create("SampleRotation.pdf", 9, vm =>
        {
            vm.Data.Images.Skip(2).First().IsSelected = true;
            Source = GetExamplesWith("Sample.pdf");
            Execute(vm, vm.Ribbon.Insert);

            var dest = vm.Data.Images.ToList();
            Assert.That(dest.Count, Is.EqualTo(9));
            for (var i = 0; i < dest.Count; ++i) Assert.That(dest[i].Index, Is.EqualTo(i));
        });

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
        public void Remove() => Create("SampleRotation.pdf", 9, vm =>
        {
            var src = vm.Data.Images.ToList();
            src[3].IsSelected = true;
            src[5].IsSelected = true;
            Execute(vm, vm.Ribbon.Remove);

            var dest = vm.Data.Images.ToList();
            Assert.That(dest.Count, Is.EqualTo(7));
            for (var i = 0; i < dest.Count; ++i) Assert.That(dest[i].Index, Is.EqualTo(i));
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Rotate
        ///
        /// <summary>
        /// Tests to rotate the selected item.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Rotate() => Create("Sample.pdf", 2, vm =>
        {
            var images = vm.Data.Images.ToList();
            var dest   = images[0];
            var dummy  = vm.Data.Preferences.Dummy;
            Assert.That(Wait.For(() => dest.Image != dummy), "Timeout");

            Assert.That(vm.Ribbon.RotateLeft.Command.CanExecute(),  Is.False);
            Assert.That(vm.Ribbon.RotateRight.Command.CanExecute(), Is.False);

            var image  = dest.Image;
            var width  = dest.Width;
            var height = dest.Height;
            var count  = 0;
            dest.IsSelected = true;
            dest.PropertyChanged += (s, e) => ++count;

            Execute(vm, vm.Ribbon.RotateLeft);
            Assert.That(Wait.For(() => count >= 4), "Timeout (Left)");
            Assert.That(dest.Image,  Is.Not.EqualTo(image).And.Not.EqualTo(dummy), "Left");
            Assert.That(dest.Width,  Is.Not.EqualTo(width), nameof(width));
            Assert.That(dest.Height, Is.Not.EqualTo(height), nameof(height));

            Execute(vm, vm.Ribbon.RotateRight);
            Assert.That(Wait.For(() => count >= 8), "Timeout (Right)");
            Assert.That(dest.Image,  Is.Not.EqualTo(image).And.Not.EqualTo(dummy), "Right");
            Assert.That(dest.Width,  Is.EqualTo(width), nameof(width));
            Assert.That(dest.Height, Is.EqualTo(height), nameof(height));
        });

        #endregion
    }
}
