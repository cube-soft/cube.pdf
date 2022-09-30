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
/// TiffTestCase
///
/// <summary>
/// Represents test cases for PostScript to TIFF conversion.
/// These test cases are invoked via ConverterTest class.
/// </summary>
///
/* ------------------------------------------------------------------------- */
sealed class TiffTestCase : TestCaseBase<TiffConverter>
{
    #region TestCases

    /* --------------------------------------------------------------------- */
    ///
    /// GetBasicTestCases
    ///
    /// <summary>
    /// Gets test cases for basic PostScript to TIFF conversion.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private IEnumerable<TestCaseData> GetBasicTestCases()
    {
        yield return Make("_Default",    new());
        yield return Make("48bppRgb",    new(Format.Tiff48bppRgb));
        yield return Make("24bppRgb",    new(Format.Tiff24bppRgb));
        yield return Make("12bppRgb",    new(Format.Tiff12bppRgb));
        yield return Make("64bppCmyk",   new(Format.Tiff64bppCmyk));
        yield return Make("32bppCmyk",   new(Format.Tiff32bppCmyk));
        yield return Make("8bppGray",    new(Format.Tiff8bppGrayscale));
        yield return Make("NoAntiAlias", new() { AntiAlias = false });
    }

    /* --------------------------------------------------------------------- */
    ///
    /// GetMonoTestCases
    ///
    /// <summary>
    /// Gets test cases for PostScript to monochrome TIFF conversion.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private IEnumerable<TestCaseData> GetMonoTestCases()
    {
        foreach (var e in new[]
        {
            Encoding.None,
            Encoding.G3Fax,
            Encoding.G4Fax,
            Encoding.Lzw,
            Encoding.PackBits,
        }) yield return Make($"Mono_{e}", new(Format.Tiff1bppMonochrome) { Compression = e });
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
        .Concat(GetMonoTestCases());

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
    protected override void OnMake(TiffConverter converter)
    {
        converter.Resolution = 96; // Reduce test time.
    }

    #endregion
}
