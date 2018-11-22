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
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cube.Pdf.Tests.Ghostscript
{
    /* --------------------------------------------------------------------- */
    ///
    /// DocumentConverterTest
    ///
    /// <summary>
    /// Represents tests of the DocumentConverter class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class DocumentConverterTest : ConverterFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// SupportedFormats
        ///
        /// <summary>
        /// Confirms the number of supported formats.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void SupportedFormats() => Assert.That(
            DocumentConverter.SupportedFormats.Count(),
            Is.EqualTo(3)
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Create_Throws
        ///
        /// <summary>
        /// Confirms the behavior when an unsupported format is set.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Create_Throws() => Assert.That(
            () => new DocumentConverter(Format.Bmp),
            Throws.TypeOf<NotSupportedException>()
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Executes the test to convert.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public void Invoke(Converter cv, string srcname, string destname)
        {
            var dest = Run(cv, srcname, destname);
            Assert.That(IO.Exists(dest), Is.True);
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
                /* --------------------------------------------------------- */
                // Format
                /* --------------------------------------------------------- */
                yield return TestCase(new DocumentConverter(Format.Ps  ), "Sample.ps", Format.Ps);
                yield return TestCase(new DocumentConverter(Format.Eps ), "Sample.ps", Format.Eps);

                /* --------------------------------------------------------- */
                // Version
                /* --------------------------------------------------------- */
                yield return TestCase(new PdfConverter
                {
                    Version = new Version(1, 2),
                }, "SampleCjk.ps", new Version(1, 2));

                /* --------------------------------------------------------- */
                //
                // EmbedFonts
                //
                // TODO: EmbedFonts が false の場合、変換後の PDF ファイル
                // に文字化けが発生します。回避方法を要調査。
                //
                /* --------------------------------------------------------- */
                yield return TestCase(new PdfConverter
                {
                    EmbedFonts = true,
                }, "Sample.ps", "EmbedFonts_True_1");

                yield return TestCase(new PdfConverter
                {
                    EmbedFonts  = true,
                    Orientation = Orientation.Portrait,
                }, "Sample.ps", "EmbedFonts_True_2");

                yield return TestCase(new PdfConverter
                {
                    EmbedFonts = false,
                }, "Sample.ps", "EmbedFonts_False");

                yield return TestCase(new PdfConverter
                {
                    EmbedFonts = false,
                }, "SampleCjk.ps", "EmbedFonts_False_Cjk");

                /* --------------------------------------------------------- */
                // ColorMode
                /* --------------------------------------------------------- */
                yield return TestCase(new PdfConverter
                {
                    ColorMode   = ColorMode.Rgb,
                    Orientation = Orientation.Portrait,
                }, "SampleMix.ps", ColorMode.Rgb);

                yield return TestCase(new PdfConverter
                {
                    ColorMode   = ColorMode.Cmyk,
                    Orientation = Orientation.Portrait,
                }, "SampleMix.ps", ColorMode.Cmyk);

                yield return TestCase(new PdfConverter
                {
                    ColorMode   = ColorMode.Grayscale,
                    Orientation = Orientation.Portrait,
                }, "SampleMix.ps", ColorMode.Grayscale);

                /* --------------------------------------------------------- */
                // Compression
                /* --------------------------------------------------------- */
                yield return TestCase(new PdfConverter
                {
                    Compression = Encoding.None,
                }, "SampleMix.ps", Encoding.None);

                yield return TestCase(new PdfConverter
                {
                    Compression = Encoding.Flate,
                }, "SampleMix.ps", Encoding.Flate);

                yield return TestCase(new PdfConverter
                {
                    Compression = Encoding.Jpeg,
                }, "SampleMix.ps", Encoding.Jpeg);

                yield return TestCase(new PdfConverter
                {
                    Compression = Encoding.Lzw,
                }, "SampleMix.ps", Encoding.Lzw);

                yield return TestCase(new PdfConverter
                {
                    Compression = Encoding.Fax,
                }, "SampleMix.ps", Encoding.Fax);

                /* --------------------------------------------------------- */
                // Downsampling
                /* --------------------------------------------------------- */
                yield return TestCase(new PdfConverter
                {
                    Downsampling = Downsampling.None,
                }, "SampleMix.ps", Downsampling.None);

                yield return TestCase(new PdfConverter
                {
                    Downsampling = Downsampling.Average,
                }, "SampleMix.ps", Downsampling.Average);

                yield return TestCase(new PdfConverter
                {
                    Downsampling = Downsampling.Bicubic,
                }, "SampleMix.ps", Downsampling.Bicubic);

                yield return TestCase(new PdfConverter
                {
                    Downsampling = Downsampling.Subsample,
                }, "SampleMix.ps", Downsampling.Subsample);

                /* --------------------------------------------------------- */
                // Mixed
                /* --------------------------------------------------------- */
                yield return TestCase(new PdfConverter
                {
                    Compression  = Encoding.Jpeg,
                    Downsampling = Downsampling.None,
                    Resolution   = 900,
                }, "Sample600dpi.ps", "Jpeg_None_600_900");

                yield return TestCase(new PdfConverter
                {
                    Compression  = Encoding.Jpeg,
                    Downsampling = Downsampling.None,
                    Resolution   = 600,
                }, "Sample600dpi.ps", "Jpeg_None_600_600");

                yield return TestCase(new PdfConverter
                {
                    Compression  = Encoding.Jpeg,
                    Downsampling = Downsampling.None,
                    Resolution   = 300,
                }, "Sample600dpi.ps", "Jpeg_None_600_300");

                yield return TestCase(new PdfConverter
                {
                    Compression  = Encoding.Jpeg,
                    Downsampling = Downsampling.None,
                    Resolution   = 150,
                }, "Sample600dpi.ps", "Jpeg_None_600_150");

                yield return TestCase(new PdfConverter
                {
                    Compression  = Encoding.Jpeg,
                    Downsampling = Downsampling.Bicubic,
                    Resolution   = 900,
                }, "Sample600dpi.ps", "Jpeg_Bicubic_600_900");

                yield return TestCase(new PdfConverter
                {
                    Compression  = Encoding.Jpeg,
                    Downsampling = Downsampling.Bicubic,
                    Resolution   = 600,
                }, "Sample600dpi.ps", "Jpeg_Bicubic_600_600");

                yield return TestCase(new PdfConverter
                {
                    Compression  = Encoding.Jpeg,
                    Downsampling = Downsampling.Bicubic,
                    Resolution   = 300,
                }, "Sample600dpi.ps", "Jpeg_Bicubic_600_300");

                yield return TestCase(new PdfConverter
                {
                    Compression  = Encoding.Jpeg,
                    Downsampling = Downsampling.Bicubic,
                    Resolution   = 150,
                }, "Sample600dpi.ps", "Jpeg_Bicubic_600_150");

                yield return TestCase(new PdfConverter
                {
                    Compression  = Encoding.Flate,
                    Downsampling = Downsampling.None,
                    Resolution   = 900,
                }, "Sample600dpi.ps", "Flate_None_600_900");

                yield return TestCase(new PdfConverter
                {
                    Compression  = Encoding.Flate,
                    Downsampling = Downsampling.None,
                    Resolution   = 600,
                }, "Sample600dpi.ps", "Flate_None_600_600");

                yield return TestCase(new PdfConverter
                {
                    Compression  = Encoding.Flate,
                    Downsampling = Downsampling.None,
                    Resolution   = 300,
                }, "Sample600dpi.ps", "Flate_None_600_300");

                yield return TestCase(new PdfConverter
                {
                    Compression  = Encoding.Flate,
                    Downsampling = Downsampling.None,
                    Resolution   = 150,
                }, "Sample600dpi.ps", "Flate_None_600_150");

                yield return TestCase(new PdfConverter
                {
                    Compression  = Encoding.Flate,
                    Downsampling = Downsampling.Bicubic,
                    Resolution   = 900,
                }, "Sample600dpi.ps", "Flate_Bicubic_600_900");

                yield return TestCase(new PdfConverter
                {
                    Compression  = Encoding.Flate,
                    Downsampling = Downsampling.Bicubic,
                    Resolution   = 600,
                }, "Sample600dpi.ps", "Flate_Bicubic_600_600");

                yield return TestCase(new PdfConverter
                {
                    Compression  = Encoding.Flate,
                    Downsampling = Downsampling.Bicubic,
                    Resolution   = 300,
                }, "Sample600dpi.ps", "Flate_Bicubic_600_300");

                yield return TestCase(new PdfConverter
                {
                    Compression  = Encoding.Flate,
                    Downsampling = Downsampling.Bicubic,
                    Resolution   = 150,
                }, "Sample600dpi.ps", "Flate_Bicubic_600_150");
            }
        }

        #endregion
    }
}
