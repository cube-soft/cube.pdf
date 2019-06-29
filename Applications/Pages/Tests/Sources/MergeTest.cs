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
    /// MergeTest
    ///
    /// <summary>
    /// Tests the Merge method of the MainFacade class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class MergeTest : FileFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Merge
        ///
        /// <summary>
        /// Tests the Merge method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public void Merge(int id, string f0, string f1)
        {
            var dest = Get($"{nameof(Merge)}-{id}.pdf");
            using (var src = new MainFacade(IO))
            {
                src.Add(GetSource(f0));
                src.Add(GetSource(f1));
                src.Move(new[] { 0 },  1);
                src.Move(new[] { 1 }, -1);
                src.Move(new[] { 0 }, -1);
                src.Merge(dest);

                Assert.That(src.Contains(GetSource(f0)), Is.True);
                Assert.That(src.Contains(GetSource(f1)), Is.True);
                Assert.That(IO.Exists(dest));
            }
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
                yield return new TestCaseData(n++, "Sample.pdf", "SampleRotation.pdf");
                yield return new TestCaseData(n++, "Sample.pdf", "Sample.jpg");
            }
        }

        #endregion
    }
}
