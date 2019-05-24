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
            using (var e = new Facade(new SettingsFolder()))
            {
                var dest = Get($"{nameof(Convert)}.pdf");

                e.Settings.Value.Source = GetSource("Sample.ps");
                e.Settings.Value.Destination = dest;
                e.Settings.Value.PostProcess = PostProcess.None;
                e.Convert();

                Assert.That(e.Settings.Value.Busy, Is.False);
                Assert.That(IO.Exists(dest), Is.True);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Convert_SaveOption
        ///
        /// <summary>
        /// Tests the Convert method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(SaveOption.Overwrite)]
        [TestCase(SaveOption.MergeHead)]
        [TestCase(SaveOption.MergeTail)]
        [TestCase(SaveOption.Rename)]
        public void Convert_SaveOption(SaveOption so)
        {
            var dest = Get($"{nameof(Convert)}_{so}.pdf");
            IO.Copy(GetSource("Sample.pdf"), dest, true);

            using (var e = new Facade(new SettingsFolder()))
            {
                e.Settings.Value.Source = GetSource("Sample.ps");
                e.Settings.Value.Destination = dest;
                e.Settings.Value.SaveOption  = so;
                e.Settings.Value.PostProcess = PostProcess.None;
                e.Convert();

                Assert.That(e.Settings.Value.Busy, Is.False);
                Assert.That(IO.Exists(dest), Is.True);
            }
        }

        #endregion
    }
}
