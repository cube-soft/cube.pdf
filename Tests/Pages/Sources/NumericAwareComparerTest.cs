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
using Cube.Collections;
using NUnit.Framework;

namespace Cube.Pdf.Pages.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// NumericAwareComparerTest
    ///
    /// <summary>
    /// Tests the NumericAwareComparer class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class NumericAwareComparerTest
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Compare
        ///
        /// <summary>
        /// Tests the Compare method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase( "2",  "2", ExpectedResult =  0)]
        [TestCase( "2",  "1", ExpectedResult =  1)]
        [TestCase( "2", "01", ExpectedResult =  1)]
        [TestCase( "2", "02", ExpectedResult =  2)] // >= 1
        [TestCase( "2", "10", ExpectedResult = -1)]
        [TestCase( "2", "20", ExpectedResult = -1)]
        [TestCase( "2", "-2", ExpectedResult =  5)] // >= 1
        [TestCase("02", "01", ExpectedResult =  1)]
        [TestCase("02", "10", ExpectedResult = -1)]
        [TestCase("1.2.3-alpha",  "1.2.3-beta", ExpectedResult = -1)]
        [TestCase("1.2.13-alpha", "1.2.3-beta", ExpectedResult =  1)]
        [TestCase("sample", "test", ExpectedResult = -1)]
        [TestCase("sample", "Sample", ExpectedResult = 32)] // >= 1
        public int Compare(string x, string y) => new NumericAwareComparer().Compare(x, y);

        /* ----------------------------------------------------------------- */
        ///
        /// Compare
        ///
        /// <summary>
        /// Tests the Compare method with the Japanese full-width numeric
        /// characters.
        /// </summary>
        ///
        /// <remarks>
        /// When the Japanese full-width numeric characters are specified,
        /// these are compared as string (not number).
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("２", "２", ExpectedResult = 0)]
        [TestCase("２", "１", ExpectedResult = 1)]
        [TestCase("２", "０１", ExpectedResult = 2)]
        [TestCase("２", "０２", ExpectedResult = 2)]
        [TestCase("２", "１０", ExpectedResult = 1)] // not < 0
        [TestCase("２", "－２", ExpectedResult = 5)]
        [TestCase("０２", "０１", ExpectedResult = 1)]
        [TestCase("０２", "１０", ExpectedResult = -1)]
        public int CompareEM(string x, string y) => new NumericAwareComparer().Compare(x, y);

        #endregion
    }
}
