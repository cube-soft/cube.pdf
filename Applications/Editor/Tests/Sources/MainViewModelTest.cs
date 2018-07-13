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
using System.Windows.Media;

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
            var vm = CreateViewModel();
            ExecuteOpenCommand(vm, GetExamplesWith(filename));
            Assert.That(Wait(() => vm.Images.Count > 0), "Timeout");

            var src  = vm.Images[0];
            var dest = default(ImageSource);
            vm.Images[0].PropertyChanged += (s, e) => dest = src.Image;

            var dummy = src.Image;
            Assert.That(dummy, Is.Not.Null);
            Assert.That(Wait(() => dest != null));
            Assert.That(dest, Is.Not.EqualTo(dummy));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Open
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
            ExecuteOpenCommand(vm, src);
            Assert.That(Wait(() => vm.Images.Count > 0), "Timeout");
            Assert.That(IO.TryDelete(src), Is.False);

            foreach (var image in vm.Images) Assert.That(image, Is.Not.Null);
            Assert.That(vm.Ribbon.Close.Command.CanExecute(null), Is.True);
            vm.Ribbon.Close.Command.Execute(null);
            Assert.That(IO.TryDelete(src), Is.True);
        }

        #endregion
    }
}
