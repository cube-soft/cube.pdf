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
using System.IO;
using System.Linq;
using Cube.FileSystem;
using Cube.Pdf.Ghostscript;
using Cube.Tests;
using NUnit.Framework;

/* ------------------------------------------------------------------------- */
///
/// ConverterTestEx
///
/// <summary>
/// Invokes additonal testing to Converter or its inherited classes.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[TestFixture]
class ConverterTestEx : FileFixture
{
    #region Check basic information

    /* --------------------------------------------------------------------- */
    ///
    /// Revision
    ///
    /// <summary>
    /// Confirms the revision number of Ghostscript.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Test]
    public void Revision()
    {
        Assert.That(Converter.Revision, Is.EqualTo(9561));
        Assert.That(Converter.Revision, Is.EqualTo(9561));
    }

    /* --------------------------------------------------------------------- */
    ///
    /// SupportedFormats
    ///
    /// <summary>
    /// Tests the number of supported formats.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Test]
    public void SupportedFormats()
    {
        // All
        Assert.That(Converter.SupportedFormats.Count(), Is.EqualTo(34), "All");

        // Document
        Assert.That(DocumentConverter.SupportedFormats.Count(), Is.EqualTo(3), "Document");
        Assert.That(() => new DocumentConverter(Format.Bmp), Throws.TypeOf<NotSupportedException>(), "Document");

        // PDF
        Assert.That(PdfConverter.SupportedFormats.Count(), Is.EqualTo(1), "PDF");

        // Image
        Assert.That(ImageConverter.SupportedFormats.Count(), Is.EqualTo(30), "Image");
        Assert.That(() => new ImageConverter(Format.Pdf), Throws.TypeOf<NotSupportedException>(), "Image");

        // JPEG
        Assert.That(JpegConverter.SupportedFormats.Count(), Is.EqualTo(4), "JPEG");
        Assert.That(() => new JpegConverter(Format.Png), Throws.TypeOf<NotSupportedException>(), "JPEG");

        // TIFF
        Assert.That(TiffConverter.SupportedFormats.Count(), Is.EqualTo(8), "TIFF");
        Assert.That(() => new TiffConverter(Format.Png), Throws.TypeOf<NotSupportedException>(), "TIFF");
    }

    #endregion

    #region Tests for irregular cases

    /* --------------------------------------------------------------------- */
    ///
    /// Test_Cjk_Throws
    ///
    /// <summary>
    /// Confirm the behavior when CJK characters are set as output
    /// path.
    /// </summary>
    ///
    /// <remarks>
    /// The Ghostscript API currently fails to invoke the conversion
    /// process if a path containing CJK characters is specified.
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    [Test]
    public void Test_Cjk_Throws() => Assert.That(() =>
    {
        var src  = GetSource("Sample.ps");
        var dest = Get("Pdf", $"CJKファイル.pdf");

        new PdfConverter
        {
            Quiet = false,
            Log   = Get($"{nameof(Test_Cjk_Throws)}.log"),
            Temp  = Get("Tmp"),
        }.Invoke(src, dest);

        if (!Io.Exists(dest)) throw new FileNotFoundException();
    }, Throws.TypeOf<FileNotFoundException>().Or.TypeOf<GsApiException>());

    /* ----------------------------------------------------------------- */
    ///
    /// Test_PdfEncoding_Throws
    ///
    /// <summary>
    /// Tests the conversion for invalid PDF encoding settings.
    /// </summary>
    ///
    /// <remarks>
    /// The error occurs in Ghostscript 9.27 or later.
    /// </remarks>
    ///
    /* ----------------------------------------------------------------- */
    [TestCase(Encoding.Flate,  Encoding.Jpeg  )]
    [TestCase(Encoding.Flate,  Encoding.Base85)]
    [TestCase(Encoding.G4Fax,  Encoding.G4Fax )]
    [TestCase(Encoding.Base85, Encoding.G4Fax )]
    public void Test_PdfEncoding_Throws(Encoding color, Encoding mono) => Assert.That(
        () => new PdfConverter
        {
            Compression              = color,
            CompressionForMonochrome = mono,
            Quiet                    = false,
            Log                      = Get($"{color}_{mono}.log"),
            Temp                     = Get("Tmp"),
        }.Invoke(GetSource("Sample.ps"), Get("Pdf", $"{color}_{mono}.pdf"))
    ,Throws.TypeOf<GsApiException>());

    #endregion
}
