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
using Cube.Tests;
using NUnit.Framework;

namespace Cube.Pdf.Converter.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// FacadeTest
    ///
    /// <summary>
    /// Tests the Facade class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class FacadeTest : FileFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Convert
        ///
        /// <summary>
        /// Tests the Convert method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Convert()
        {
            var dest = Get($"{nameof(Convert)}.pdf");
            var src  = GetSource("Sample.pdf");
            var hash = "B5797B3DEA8CEE49A02D26864CBCB55411F71C2018109620DF5D7E704838BDBB";

            using (var e = new Facade(new SettingsFolder()))
            {
                e.Settings.Value.Source = src;
                e.Settings.Value.Destination = dest;
                e.Settings.Value.PostProcess = PostProcess.None;
                e.Settings.Digest = hash;
                e.Convert();

                Assert.That(e.Settings.Value.Busy, Is.False);
                Assert.That(IO.Exists(dest), Is.True);
            }
        }

        #endregion
    }
}
