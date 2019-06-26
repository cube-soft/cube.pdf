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

namespace Cube.Pdf.Pages.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// SplitTest
    ///
    /// <summary>
    /// Tests the Split method of the MainFacade class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class SplitTest : FileFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Split
        ///
        /// <summary>
        /// Tests the Split method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public int Split(int id, string filename)
        {
            var files = new List<string>();
            var dest  = Get($"{nameof(Split)}-{id}");

            using (var src = new MainFacade(IO))
            {
                src.Add(GetSource(filename));
                src.Split(dest, files);
                Assert.That(src.Contains(GetSource(filename)), Is.True);
            }
            return files.Count;
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
                yield return new TestCaseData(n++, "SampleRotation.pdf").Returns(9);
                yield return new TestCaseData(n++, "Sample.jpg").Returns(1);
            }
        }

        #endregion
    }
}
