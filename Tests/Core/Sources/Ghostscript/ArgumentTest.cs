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
using System.Collections.Generic;
using Cube.Pdf.Ghostscript;
using NUnit.Framework;

namespace Cube.Pdf.Tests.Ghostscript
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArgumentTest
    ///
    /// <summary>
    /// Tests the Argument class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class ArgumentTest
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// ToString
        ///
        /// <summary>
        /// Tests the ToString method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public string ToString(Argument src) => src.ToString();

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
            yield return new TestCaseData(
                new Argument('s', "DEVICE", "pdfwrite")
            ).Returns("-sDEVICE=pdfwrite");

            yield return new TestCaseData(
                new Argument('I', string.Empty, @"Path\To\Resources")
            ).Returns(@"-IPath\To\Resources");

            yield return new TestCaseData(
                new Argument("ColorConversionStrategy", "RGB")
            ).Returns("-dColorConversionStrategy=/RGB");

            yield return new TestCaseData(
                new Argument("DownsampleColorImages", true)
            ).Returns("-dDownsampleColorImages=true");

            yield return new TestCaseData(
                new Argument("ColorImageResolution", 300)
            ).Returns("-dColorImageResolution=300");

            yield return new TestCaseData(
                new Argument('r', 600)
            ).Returns("-r600");

            yield return new TestCaseData(
                new Argument('d', "BATCH")
            ).Returns("-dBATCH");

            yield return new TestCaseData(
                new Argument('f')
            ).Returns("-f");

            yield return new TestCaseData(
                new Code("<</Orientation 1}>> setpagedevice")
            ).Returns("<</Orientation 1}>> setpagedevice");
        }}

        #endregion
    }
}
