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
using Cube.FileSystem;
using Cube.FileSystem.TestService;
using Cube.Xui.Mixin;
using NUnit.Framework;
using System.Linq;

namespace Cube.Pdf.Tests.Editor.ViewModels
{
    /* --------------------------------------------------------------------- */
    ///
    /// SaveTest
    ///
    /// <summary>
    /// Tests for Save commands.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class SaveTest : ViewModelFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// SaveAs
        ///
        /// <summary>
        /// Executes the test for saving the PDF document as a new file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("Sample.pdf",         "",         2)]
        [TestCase("SampleAes128.pdf",   "password", 2)]
        [TestCase("SampleRc40.pdf",     "password", 2)]
        [TestCase("SampleRc40Open.pdf", "password", 2)]
        public void SaveAs(string filename, string password, int n) =>
            Create(filename, password, n, vm =>
        {
            var fi = IO.Get(Source);
            Destination = Path(Args(fi.NameWithoutExtension));
            Password    = string.Empty;
            Assert.That(IO.Exists(Destination), Is.False);

            var src = vm.Data.Source;
            vm.Ribbon.SaveAs.Command.Execute();
            Assert.That(Wait.For(() => src.Value.FullName == Destination), "Timeout (Save)");
            Assert.That(IO.Exists(Destination), Is.True);
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Overwrite
        ///
        /// <summary>
        /// Executes the test for overwriting the PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("Sample.pdf", "", 2)]
        public void Overwrite(string filename, string password, int n) => Create(vm =>
        {
            var fi = IO.Get(GetExamplesWith(filename));
            Source = Path(Args(fi.NameWithoutExtension));
            IO.Copy(fi.FullName, Source, true);
            vm.Ribbon.Open.Command.Execute();
            Assert.That(Wait.For(() => vm.Data.Count.Value == n), "Timeout (Open)");

            vm.Data.Images.First().IsSelected = true;
            vm.Ribbon.RotateLeft.Command.Execute();
            Assert.That(Wait.For(() => vm.Data.Modified.Value), "Timeout (Rotate)");

            vm.Ribbon.Save.Command.Execute();
            Assert.That(Wait.For(() => !vm.Data.Modified.Value), "Timeout (Save)");
        });

        #endregion
    }
}
