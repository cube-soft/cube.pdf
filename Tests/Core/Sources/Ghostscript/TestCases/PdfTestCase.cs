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

using System;
using System.Collections.Generic;
using System.Linq;
using Cube.Pdf.Ghostscript;
using NUnit.Framework;

/* ------------------------------------------------------------------------- */
///
/// PdfTestCase
///
/// <summary>
/// Represents test cases for PostScript to PDF conversion.
/// These test cases are invoked via ConverterTest class.
/// </summary>
///
/* ------------------------------------------------------------------------- */
internal class PdfTestCase
{
    #region TestCases

    /* --------------------------------------------------------------------- */
    ///
    /// GetBasicTestCases
    ///
    /// <summary>
    /// Gets the test cases for basic PostScript to TIFF conversion.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private static IEnumerable<TestCaseData> GetBasicTestCases()
    {
        yield return Make("_Default", new());
        yield return Make("Web", "SampleWeb.ps", new() { Linearization = true });
        yield return Make("Version_12", new() { Version = new PdfVersion(1, 2) });
        yield return Make("Metadata", "SampleMetadata.ps", new());
    }

    /* --------------------------------------------------------------------- */
    ///
    /// GetPaperTestCases
    ///
    /// <summary>
    /// Gets test cases for PDF paper settings.
    /// </summary>
    ///
    /// <remarks>
    /// TODO: Paper の設定は入力ファイルによっては反映されない場合がある。
    /// 例えば、SampleCjk.ps を変換すると Paper の設定に関わらず常に
    /// A4 サイズで変換される。原因を要調査。
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    private static IEnumerable<TestCaseData> GetPaperTestCases()
    {
        foreach (var e in new[]
        {
            Paper.IsoB4,
            Paper.JisB4,
            Paper.Letter,
        }) yield return Make($"Paper_{e}", new() { Paper = e });
    }

    /* --------------------------------------------------------------------- */
    ///
    /// GetOrientationTestCases
    ///
    /// <summary>
    /// Gets test cases for PDF orientation settings.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private static IEnumerable<TestCaseData> GetOrientationTestCases()
    {
        foreach (var e in new[] {
            Orientation.Portrait,
            Orientation.UpsideDown,
            Orientation.Landscape,
            Orientation.Seascape,
        }) yield return Make($"Orientation_{e}", new() { Orientation = e });
    }

    /* --------------------------------------------------------------------- */
    ///
    /// GetColorModeTestCases
    ///
    /// <summary>
    /// Gets test cases for PDF color mode settings.
    /// </summary>
    ///
    /// <remarks>
    /// Includes the relevant test cases because a problem was found in
    /// which colors become strange when PDF to PostScript to PDF
    /// conversion is executed.
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    private static IEnumerable<TestCaseData> GetColorModeTestCases()
    {
        foreach (ColorMode e in Enum.GetValues(typeof(ColorMode)))
        {
            yield return Make($"Color_{e}", new() { ColorMode = e });
            yield return Make($"Color_{e}_RePdf", "SamplePdf.ps", new() { ColorMode = e });
        }
    }

    /* --------------------------------------------------------------------- */
    ///
    /// GetColorModeTestCases
    ///
    /// <summary>
    /// Gets test cases for PDF embed fonts settings.
    /// </summary>
    ///
    /// <remarks>
    /// A bug has been confirmed that garbled characters occur when
    /// converting without font embedding.
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    private static IEnumerable<TestCaseData> GetEmbedFontsTestCases()
    {
        yield return Make("_NoEmbedFonts", "Sample.ps", new() { EmbedFonts = false });
        yield return Make("_NoEmbedFonts_Cjk", "SampleCjk.ps", new() { EmbedFonts = false });
    }

    /* --------------------------------------------------------------------- */
    ///
    /// GetEncodingTestCases
    ///
    /// <summary>
    /// Gets test cases for PDF Encoding, Downsampling, and Resolution
    /// settings.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private static IEnumerable<TestCaseData> GetEncodingTestCases()
    {
        var resolutions = new[] { 72, 96 };
        var encodings   = new KeyValuePair<Encoding, Encoding>[]
        {
            new(Encoding.None,  Encoding.None),
            new(Encoding.Lzw,   Encoding.Lzw),
            new(Encoding.Flate, Encoding.Flate),
            new(Encoding.Flate, Encoding.G3Fax),
            new(Encoding.Jpeg,  Encoding.G4Fax),
        };

        foreach (var kv in encodings)
        foreach (var n  in resolutions)
        foreach (var e  in new[] { // Downsamplings
            Downsampling.None,
            Downsampling.Bicubic,
            Downsampling.Subsample,
        }) yield return Make($"Encoding_{kv.Key}_{kv.Value}_{e}_{n}", "Sample600dpi.ps", new()
        {
            Downsampling    = e,
            Resolution      = n,
            Compression     = kv.Key,
            MonoCompression = kv.Value,
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
    public static IEnumerable<TestCaseData> Get() => GetBasicTestCases()
        .Concat(GetPaperTestCases())
        .Concat(GetOrientationTestCases())
        .Concat(GetColorModeTestCases())
        .Concat(GetEmbedFontsTestCases())
        .Concat(GetEncodingTestCases());

    #endregion

    #region Others

    /* --------------------------------------------------------------------- */
    ///
    /// Make
    ///
    /// <summary>
    /// Creates a new TestCaseData object.
    /// </summary>
    ///
    /// <param name="name">
    /// Test name, which is used for a part of the destination path.
    /// </param>
    ///
    /// <param name="converter">Converter object.</param>
    ///
    /* --------------------------------------------------------------------- */
    private static TestCaseData Make(string name, PdfConverter converter) =>
        Make(name, "SampleMix.ps", converter);

    /* --------------------------------------------------------------------- */
    ///
    /// Make
    ///
    /// <summary>
    /// Creates a new TestCaseData object.
    /// </summary>
    ///
    /// <param name="name">
    /// Test name, which is used for a part of the destination path.
    /// </param>
    ///
    /// <param name="src">Source filename.</param>
    /// <param name="converter">Converter object.</param>
    ///
    /* --------------------------------------------------------------------- */
    private static TestCaseData Make(string name, string src, PdfConverter converter) =>
        new("Pdf", name, src, converter);

    #endregion
}
