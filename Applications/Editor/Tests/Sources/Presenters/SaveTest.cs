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
using NUnit.Framework;
using System.Linq;

namespace Cube.Pdf.Editor.Tests.Presenters
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
        [TestCase("Sample.pdf",         ""        )]
        [TestCase("SampleAes128.pdf",   "password")]
        [TestCase("SampleRc40.pdf",     "password")]
        [TestCase("SampleRc40Open.pdf", "password")]
        public void SaveAs(string filename, string password) => Open(filename, password, vm =>
        {
            Destination = Get(Args(IO.Get(Source).BaseName));
            Password    = string.Empty;

            Assert.That(IO.Exists(Destination), Is.False);
            vm.Test(vm.Ribbon.SaveAs);
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
        [TestCase("Sample.pdf", "")]
        public void Overwrite(string filename, string password) => Create(vm =>
        {
            Source   = Get(Args(IO.Get(GetSource(filename)).BaseName));
            Password = password;

            IO.Copy(GetSource(filename), Source, true);
            vm.Test(vm.Ribbon.Open);
            vm.Value.Images.First().IsSelected = true;
            vm.Test(vm.Ribbon.RotateLeft);
            vm.Test(vm.Ribbon.Save);
        });

        #endregion
    }
}
