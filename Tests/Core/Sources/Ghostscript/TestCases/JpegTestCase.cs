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
using Cube.Pdf.Ghostscript;
using NUnit.Framework;

/* ------------------------------------------------------------------------- */
///
/// JpegTestCase
///
/// <summary>
/// Represents test cases for PostScript to JPEG conversion.
/// These test cases are invoked via ConverterTest class.
/// </summary>
///
/* ------------------------------------------------------------------------- */
sealed class JpegTestCase : TestCaseBase<JpegConverter>
{
    #region TestCases

    /* --------------------------------------------------------------------- */
    ///
    /// GetBasicTestCases
    ///
    /// <summary>
    /// Gets test cases for basic PostScript to JPEG conversion.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private IEnumerable<TestCaseData> GetBasicTestCases()
    {
        yield return Make("_Default",    new());
        yield return Make("24bppRgb",    new(Format.Jpeg24bppRgb));
        yield return Make("32bppCmyk",   new(Format.Jpeg32bppCmyk));
        yield return Make("8bppGray",    new(Format.Jpeg8bppGrayscale));
        yield return Make("NoAntiAlias", new() { AntiAlias = false });
    }

    /* --------------------------------------------------------------------- */
    ///
    /// GetQualityTestCases
    ///
    /// <summary>
    /// Gets test cases for JPEG quality settings.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private IEnumerable<TestCaseData> GetQualityTestCases()
    {
        foreach (var n in new[] {
            1, 25, 50, 60, 70, 80, 90, 100,
        }) yield return Make($"Quality_{n}", new() { Quality = n });
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Get
    ///
    /// <summary>
    /// Gets the collection of test cases.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    protected override IEnumerable<TestCaseData> Get() => GetBasicTestCases()
        .Concat(GetQualityTestCases());

    #endregion

    #region Others

    /* --------------------------------------------------------------------- */
    ///
    /// OnMake
    ///
    /// <summary>
    /// Sets up additional settings for the specified converter before
    /// creating a new TestCaseData object.
    /// </summary>
    ///
    /// <param name="converter">Ghostscript converter.</param>
    ///
    /* --------------------------------------------------------------------- */
    protected override void OnMake(JpegConverter converter)
    {
        converter.Resolution = 96; // Reduce test time.
    }

    #endregion
}
