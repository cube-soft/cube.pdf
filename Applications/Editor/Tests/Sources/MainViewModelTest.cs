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
        [TestCase("Sample.pdf")]
        public void Open(string filename)
        {
            var src = GetExamplesWith(filename);
            var vm  = CreateViewModel();
            Assert.That(ExecuteOpenCommand(vm, src), "Timeout");

            var pref = vm.Data.Preferences;
            Assert.That(pref.ItemSize,   Is.EqualTo(250));
            Assert.That(pref.ItemMargin, Is.EqualTo(3));
            Assert.That(pref.TextHeight, Is.EqualTo(25));

            var images = vm.Data.Images;
            foreach (var item in images) Assert.That(item.Image, Is.Not.Null);

            var dest = images.First();
            var cts  = new CancellationTokenSource();
            dest.PropertyChanged += (s, e) => cts.Cancel();
            images.Refresh();
            Assert.That(Wait.For(cts.Token), "Timeout");
            Assert.That(dest.Image, Is.Not.EqualTo(images.LoadingImage));
        }

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
        public void Close()
        {
            var src = GetResultsWith($"{nameof(Close)}Sample.pdf");
            IO.Copy(GetExamplesWith("Sample.pdf"), src, true);

            var vm = CreateViewModel();
            Assert.That(ExecuteOpenCommand(vm, src), "Timeout");
            Assert.That(IO.TryDelete(src), Is.False);

            Assert.That(vm.Ribbon.Close.Command.CanExecute(null), Is.True);
            vm.Ribbon.Close.Command.Execute(null);
            Assert.That(IO.TryDelete(src), Is.True);
        }

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
        public void Select()
        {
            var src = GetExamplesWith("SampleRotation.pdf");
            var vm  = CreateViewModel();
            Assert.That(ExecuteOpenCommand(vm, src), "Timeout");

            var dest = vm.Data.Selection;
            Assert.That(dest.Count,   Is.EqualTo(0));
            Assert.That(dest.Items,   Is.Not.Null);
            Assert.That(dest.Indices, Is.Not.Null);
            Assert.That(dest.Index,   Is.EqualTo(-1));

            vm.Data.Images.First().IsSelected = true;
            Assert.That(dest.Count,   Is.EqualTo(1), nameof(dest.Count));
            Assert.That(dest.Index,   Is.EqualTo(0), nameof(dest.Index));

            vm.Ribbon.SelectFlip.Command.Execute(null);
            Assert.That(dest.Count,   Is.EqualTo(8), nameof(dest.Count));
            Assert.That(dest.Index,   Is.EqualTo(8), nameof(dest.Index));

            vm.Ribbon.SelectAll.Command.Execute(null);
            Assert.That(dest.Count,   Is.EqualTo(9), nameof(dest.Count));
            Assert.That(dest.Index,   Is.EqualTo(8), nameof(dest.Index));

            vm.Ribbon.SelectCancel.Command.Execute(null);
            Assert.That(dest.Count,   Is.EqualTo(0));
        }

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
        public void Rotate()
        {
            var src = GetExamplesWith("Sample.pdf");
            var vm  = CreateViewModel();
            Assert.That(ExecuteOpenCommand(vm, src), "Timeout");

            var images = vm.Data.Images;
            var dest   = images.First();
            var dummy  = vm.Data.Images.LoadingImage;
            Assert.That(Wait.For(() => dest.Image != dummy), "Timeout");

            var image  = images.First().Image;
            var width  = images.First().Width;
            var height = images.First().Height;
            var count  = 0;
            dest.IsSelected = true;
            dest.PropertyChanged += (s, e) => ++count;

            images.Rotate(90);
            Assert.That(Wait.For(() => count >= 4), "Timeout");
            Assert.That(dest.Image,  Is.Not.EqualTo(image).And.Not.EqualTo(dummy));
            Assert.That(dest.Width,  Is.Not.EqualTo(width), nameof(width));
            Assert.That(dest.Height, Is.Not.EqualTo(height), nameof(height));
        }

        #endregion
    }
}
