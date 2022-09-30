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
namespace Cube.Pdf.Converter.Tests;

using System.Collections.Generic;
using System.Linq;
using Cube.Pdf.Converter;
using Cube.Pdf.Ghostscript;
using NUnit.Framework;

/* ------------------------------------------------------------------------- */
///
/// SdkTestCase
///
/// <summary>
/// Represents test cases for conversion.
/// These test cases are invoked through the SdkTest class.
/// </summary>
///
/* ------------------------------------------------------------------------- */
sealed class SdkTestCase : TestCaseBase<SettingValue>
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// SdkTestCase
    ///
    /// <summary>
    /// Initializes a new instance of the SdkTestCase class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public SdkTestCase() : base("Sample1pPhoto.ps") { }

    #endregion

    #region TestCases

    /* --------------------------------------------------------------------- */
    ///
    /// GetJpegTestCases
    ///
    /// <summary>
    /// Gets test cases for PostScript to JPEG conversion.
    /// Note that the ColorMode property will be ignored in the test cases.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private IEnumerable<TestCaseData> GetJpegTestCases()
    {
        yield return Make(nameof(Format.Jpeg24bppRgb), new() {
            Format    = Format.Jpeg24bppRgb,
            ColorMode = ColorMode.Grayscale,
        });

        yield return Make(nameof(Format.Jpeg8bppGrayscale), new() {
            Format    = Format.Jpeg8bppGrayscale,
            ColorMode = ColorMode.Rgb,
        });
    }

    /* --------------------------------------------------------------------- */
    ///
    /// GetTiffTestCases
    ///
    /// <summary>
    /// Gets test cases for PostScript to TIFF conversion.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private IEnumerable<TestCaseData> GetTiffTestCases()
    {
        yield return Make($"Color_{nameof(Encoding.G4Fax)}", new() {
            Format   = Format.Tiff24bppRgb,
            Encoding = Encoding.G4Fax, // will be ignored
        });

        yield return Make($"Mono_{nameof(Encoding.Lzw)}", new() {
            Format   = Format.Tiff1bppMonochrome,
            Encoding = Encoding.Lzw,
        });

        yield return Make($"Mono_{nameof(Encoding.G4Fax)}", new() {
            Format   = Format.Tiff1bppMonochrome,
            Encoding = Encoding.G4Fax,
        });

        yield return Make($"Mono_{nameof(Encoding.G3Fax)}", new() {
            Format   = Format.Tiff1bppMonochrome,
            Encoding = Encoding.G3Fax,
        });
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
    protected override IEnumerable<TestCaseData> Get() => GetJpegTestCases()
        .Concat(GetTiffTestCases());

    #endregion
}
