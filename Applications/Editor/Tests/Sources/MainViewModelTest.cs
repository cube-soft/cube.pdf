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
            Assert.That(Wait.For(() => vm.Data.Images.Count > 0), "Timeout");
            Assert.That(vm.Data.Images[0].Image, Is.Not.Null);
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
            Assert.That(Wait.For(() => vm.Data.Images.Count > 0), "Timeout");
            Assert.That(IO.TryDelete(src), Is.False);

            foreach (var image in vm.Data.Images) Assert.That(image, Is.Not.Null);
            Assert.That(vm.Ribbon.Close.Command.CanExecute(null), Is.True);
            vm.Ribbon.Close.Command.Execute(null);
            Assert.That(IO.TryDelete(src), Is.True);
        }

        #endregion
    }
}
