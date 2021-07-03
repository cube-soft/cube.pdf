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
using System.Linq;
using NUnit.Framework;

namespace Cube.Pdf.Editor.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// RangeTest
    ///
    /// <summary>
    /// Tests for the Range class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class RangeTest
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Parse
        ///
        /// <summary>
        /// Executes the test for parsing the string.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("1,2,3,4",  ExpectedResult = 4)]
        [TestCase("1,2-5,7",  ExpectedResult = 6)]
        [TestCase("1,3,1,3",  ExpectedResult = 2)]
        [TestCase("3, 1, 4",  ExpectedResult = 3)]
        [TestCase("2 - 8, 1", ExpectedResult = 8)]
        [TestCase("",         ExpectedResult = 0)]
        public int Parse(string src) => new Range(src, 100).Count();

        /* ----------------------------------------------------------------- */
        ///
        /// Parse_Throws
        ///
        /// <summary>
        /// Executes the test for confirming the result when the specified
        /// string is wrong.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("1,2,a")]
        [TestCase("1.3,5")]
        [TestCase("[123]")]
        [TestCase("1+2*3-4/5=6")]
        [TestCase("日本語のテスト")]
        public void Parse_Throws(string src) => Assert.That(
            () => new Range(src, 100),
            Throws.TypeOf<RangeException>()
        );

        #endregion
    }
}
