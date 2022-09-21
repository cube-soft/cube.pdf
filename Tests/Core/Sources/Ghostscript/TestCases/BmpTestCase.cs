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
using Cube.Pdf.Ghostscript;
using NUnit.Framework;

/* ------------------------------------------------------------------------- */
///
/// BmpTestCase
///
/// <summary>
/// Represents test cases for PostScript to BMP conversion.
/// These test cases are invoked via ConverterTest class.
/// </summary>
///
/* ------------------------------------------------------------------------- */
sealed class BmpTestCase : TestCaseBase<ImageConverter>
{
    #region TestCases

    /* --------------------------------------------------------------------- */
    ///
    /// Get
    ///
    /// <summary>
    /// Gets the collection of test cases.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    protected override IEnumerable<TestCaseData> Get()
    {
        yield return Make("_Default",    new(Format.Bmp));
        yield return Make("32bppArgb",   new(Format.Bmp32bppArgb));
        yield return Make("24bppRgb",    new(Format.Bmp24bppRgb));
        yield return Make("8bppIndex",   new(Format.Bmp8bppIndexed));
        yield return Make("4bppIndex",   new(Format.Bmp4bppIndexed));
        yield return Make("8bppGray",    new(Format.Bmp8bppGrayscale));
        yield return Make("1bppMono",    new(Format.Bmp1bppMonochrome));
        yield return Make("NoAntiAlias", new(Format.Bmp) { AntiAlias = false });
    }

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
    protected override void OnMake(ImageConverter converter)
    {
        converter.Resolution = 96; // Reduce test time.
    }

    #endregion
}
