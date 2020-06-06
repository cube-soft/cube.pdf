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
using Cube.Pdf.Ghostscript;
using Cube.Tests;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cube.Pdf.Tests.Ghostscript
{
    /* --------------------------------------------------------------------- */
    ///
    /// FormatTest
    ///
    /// <summary>
    /// Tests for the Format and Converter classes.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class FormatTest : FileFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Convert
        ///
        /// <summary>
        /// Executes tests of the Invoke method with the specified format.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public void Convert(Format fmt)
        {
            var name = $"{nameof(Convert)}_{fmt.ToString()}";
            var dest = Get($"{name}{fmt.GetExtension()}");
            var src  = GetSource("Sample.eps");
            var conv = new Converter(fmt) { Resolution = 72 };

            conv.Invoke(src, dest);
            Assert.That(IO.Get(dest).Length, Is.AtLeast(1));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ConvertToText
        ///
        /// <summary>
        /// Executes tests to convert from PostScript to Text format.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void ConvertToText()
        {
            var fmt  = Format.Text;
            var dest = Get($"{nameof(ConvertToText)}{fmt.GetExtension()}");
            var src  = GetSource("Sample.ps");
            var conv = new Converter(fmt);

            conv.Invoke(src, dest);
            Assert.That(IO.Get(dest).Length, Is.AtLeast(1));
        }

        #endregion

        #region TestCases

        /* ----------------------------------------------------------------- */
        ///
        /// TestCases
        ///
        /// <summary>
        /// Gets test cases.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                var v = Enum.GetValues(typeof(Format)).Cast<Format>().Where(e => e != Format.Text);
                foreach (var src in v) yield return new TestCaseData(src);
            }
        }

        #endregion
    }
}
