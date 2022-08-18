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
namespace Cube.Pdf.Tests.Ghostscript;

using System.Collections.Generic;
using System.Linq;
using Cube.FileSystem;
using Cube.Pdf.Ghostscript;
using Cube.Tests;
using NUnit.Framework;

/* ------------------------------------------------------------------------- */
///
/// ConverterTest
///
/// <summary>
/// Tests the Converter and its inherited classes.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[TestFixture]
class ConverterTest : FileFixture
{
    /* --------------------------------------------------------------------- */
    ///
    /// Test
    ///
    /// <summary>
    /// Tests the conversion using Ghostscript.
    /// </summary>
    ///
    /// <param name="category">
    /// Category name. Test results are saved to a directory with the
    /// specified name.
    /// </param>
    ///
    /// <param name="name">
    /// Test name, which is used for a part of the destination path.
    /// </param>
    ///
    /// <param name="src">
    /// Name of the source file, which must be stored in the Examples
    /// directory.
    /// </param>
    ///
    /// <param name="converter">Converter object.</param>
    ///
    /* --------------------------------------------------------------------- */
    [TestCaseSource(nameof(TestCases))]
    public void Test(string category, string name, string src, Converter converter)
    {
        var sp = GetSource(src);
        var dp = Get(category, $"{name}{converter.Format.GetExtension()}");

        converter.Quiet = false;
        converter.Log   = Get(category, $"{name}.log");
        converter.Temp  = Get(".tmp");
        converter.Invoke(sp, dp);

        Assert.That(Io.Get(dp).Length, Is.AtLeast(1));
    }

    /* --------------------------------------------------------------------- */
    ///
    /// TestCases
    ///
    /// <summary>
    /// Gets test cases of the Test method.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    static IEnumerable<TestCaseData> TestCases => Enumerable.Empty<TestCaseData>()
        .Concat(new TextTestCase())
        .Concat(new PsTestCase())
        .Concat(new EpsTestCase())
        .Concat(new PdfTestCase())
        .Concat(new BmpTestCase())
        .Concat(new PngTestCase())
        .Concat(new PsdTestCase())
        .Concat(new JpegTestCase())
        .Concat(new TiffTestCase());
}
