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
using Cube.Xui;
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
        /// Tests to open a PDF file and create images as an asynchronous
        /// operation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("Sample.pdf")]
        public void Open(string filename)
        {
            var vm = Create();

            vm.Messenger.Register<OpenFileDialogMessage>(this, e =>
            {
                e.FileName = GetExamplesWith(filename);
                e.Result   = true;
                e.Callback.Invoke(e);
            });

            vm.Ribbon.Open.Command.Execute(null);
            Assert.That(Wait(() => vm.Images.Count > 0), "Timeout");

            var src  = vm.Images[0];
            var dest = default(ImageSource);
            vm.Images[0].PropertyChanged += (s, e) => dest = src.Image;

            var dummy = src.Image;
            Assert.That(dummy, Is.Not.Null);
            Assert.That(Wait(() => dest != null));
            Assert.That(dest, Is.Not.EqualTo(dummy));
        }

        #endregion
    }
}
