/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2013 CubeSoft, Inc.
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
using System.Collections.Generic;
using System.Linq;
using Cube.Tests;
using NUnit.Framework;

namespace Cube.Pdf.Pages.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// FileSelectorTest
    ///
    /// <summary>
    /// Tests the FileSelector class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class FileSelectorTest : FileFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Get
        ///
        /// <summary>
        /// Tests the Get method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public void Get(int _, IEnumerable<string> src, IEnumerable<string> expected)
        {
            var cmp  = expected.Select(e => GetSource(e)).ToArray();
            var dest = new FileSelector().Get(src.Select(e => GetSource(e))).ToArray();

            Assert.That(dest.Length, Is.EqualTo(cmp.Length));
            for (var i = 0; i < dest.Length; ++i) Assert.That(dest[i], Is.EqualTo(cmp[i]));
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
        public static IEnumerable<TestCaseData> TestCases { get
        {
            var n = 0;

            yield return new(n++,
                new List<string> { "Sample.pdf", "Sample.jpg", "Sample.txt" },
                new List<string> { "Sample.jpg", "Sample.pdf" }
            );

            yield return new(n++,
                new List<string> { "Dir1" },
                new List<string> { @"Dir1\Dir1Sample.jpg", @"Dir1\Dir1Sample.pdf" }
            );

            yield return new(n++,
                new List<string> { "Sample.pdf", "Sample.jpg", "Sample.txt", "Dir1", "Dir2" },
                new List<string>
                {
                    @"Dir1\Dir1Sample.jpg",
                    @"Dir1\Dir1Sample.pdf",
                    @"Dir2\Dir2Sample.jpg",
                    @"Dir2\Dir2Sample.pdf",
                    "Sample.jpg",
                    "Sample.pdf",
                }
            );

            yield return new(n++, new List<string>(), new List<string>());
        }}

        #endregion
    }
}
