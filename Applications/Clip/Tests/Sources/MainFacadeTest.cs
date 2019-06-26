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
using System.Collections.Generic;

namespace Cube.Pdf.Clip.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// MainFacadeTest
    ///
    /// <summary>
    /// Tests the MainFacade class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class MainFacadeTest : FileFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Attach
        ///
        /// <summary>
        /// Tests the Attach and related methods.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public void Attach(int id, string filename, string clip)
        {
            var dest = Get($"{nameof(Attach)}-{id}.pdf");
            IO.Copy(GetSource(filename), dest);

            using (var facade = new MainFacade(IO))
            {
                facade.Source = dest;
                facade.Attach(GetSource(clip));
                facade.Save();
            }

            Assert.That(IO.Exists(dest), Is.True);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Detach
        ///
        /// <summary>
        /// Tests the Detach and related methods.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Detach()
        {
            var dest = Get($"{nameof(Detach)}.pdf");
            IO.Copy(GetSource("SampleAttachmentCjk.pdf"), dest);

            using (var facade = new MainFacade(IO))
            {
                facade.Source = dest;
                facade.Detach(0);
                facade.Save();
            }

            Assert.That(IO.Exists(dest), Is.True);
        }

        #endregion

        #region TestCases

        /* ----------------------------------------------------------------- */
        ///
        /// TestCases
        ///
        /// <summary>
        /// Gets the test cases.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                var n = 0;
                yield return new TestCaseData(n++, "Sample.pdf", "Sample.jpg");
            }
        }

        #endregion
    }
}
