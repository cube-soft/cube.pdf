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
using System;
using System.Collections.Generic;
using System.Linq;
using Cube.FileSystem;
using Cube.Pdf.Ghostscript;
using NUnit.Framework;

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
        public void SupportedFormats()
        {
            Assert.That(DocumentConverter.SupportedFormats.Count(), Is.EqualTo(3));
        }

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
        public void Create_Throws()
        {
            Assert.That(() => new DocumentConverter(Format.Bmp), Throws.TypeOf<NotSupportedException>());
        }

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
        public void Invoke(int id, Converter cv, string srcname, string destname)
        {
            var dest = Run(cv, srcname, destname);
            Assert.That(Io.Exists(dest), Is.True, $"No.{id}");
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke_Throws
        ///
        /// <summary>
        /// Confirms the error of invalid compression settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(Encoding.Flate,  Encoding.Jpeg)]
        [TestCase(Encoding.Flate,  Encoding.Base85)]
        [TestCase(Encoding.Fax,    Encoding.Fax)]
        [TestCase(Encoding.Base85, Encoding.Fax)]
        public void Invoke_Throws(Encoding color, Encoding mono)
        {
            if (Converter.Revision < 927) Assert.Ignore("Only for Ghostscript 9.27 or later.");

            var src = new PdfConverter
            {
                Compression     = color,
                MonoCompression = mono,
            };

            Assert.That(() => Run(src, "Sample.ps", $"{color}_{mono}"), Throws.TypeOf<GsApiException>());
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
        public static IEnumerable<TestCaseData> TestCases { get
        {
            var n = 0;

            /* --------------------------------------------------------- */
            // Format
            /* --------------------------------------------------------- */
            yield return TestCase(n++, new DocumentConverter(Format.Ps ), "Sample.ps", Format.Ps);
            yield return TestCase(n++, new DocumentConverter(Format.Eps), "Sample.ps", Format.Eps);

            /* --------------------------------------------------------- */
            // Metadata
            /* --------------------------------------------------------- */
            yield return TestCase(n++, new PdfConverter(), "SampleMetadata.ps", "Metadata");

            /* --------------------------------------------------------- */
            // Version
            /* --------------------------------------------------------- */
            yield return TestCase(n++, new PdfConverter
            {
                Version = new PdfVersion(1, 2),
            }, "SampleCjk.ps", new PdfVersion(1, 2));

            /* --------------------------------------------------------- */
            // Linearization
            /* --------------------------------------------------------- */
            yield return TestCase(n++, new PdfConverter
            {
                Linearization = true,
            }, "SampleWeb.ps", "Linearization");

            /* --------------------------------------------------------- */
            // Orientation
            /* --------------------------------------------------------- */
            yield return TestCase(n++, new PdfConverter
            {
                Orientation = Orientation.Auto,
            }, "SampleMix.ps", Orientation.Auto);

            yield return TestCase(n++, new PdfConverter
            {
                Orientation = Orientation.Portrait,
            }, "SampleMix.ps", Orientation.Portrait);

            yield return TestCase(n++, new PdfConverter
            {
                Orientation = Orientation.UpsideDown,
            }, "SampleMix.ps", Orientation.UpsideDown);

            yield return TestCase(n++, new PdfConverter
            {
                Orientation = Orientation.Landscape,
            }, "SampleMix.ps", Orientation.Landscape);

            yield return TestCase(n++, new PdfConverter
            {
                Orientation = Orientation.Seascape,
            }, "SampleMix.ps", Orientation.Seascape);

            /* --------------------------------------------------------- */
            // ColorMode
            /* --------------------------------------------------------- */
            yield return TestCase(n++, new PdfConverter
            {
                ColorMode   = ColorMode.Rgb,
            }, "SampleMix.ps", ColorMode.Rgb);

            yield return TestCase(n++, new PdfConverter
            {
                ColorMode = ColorMode.Rgb,
            }, "SamplePdf.ps", "ColorMode_Rgb_RePdf");

            yield return TestCase(n++, new PdfConverter
            {
                ColorMode   = ColorMode.Cmyk,
            }, "SampleMix.ps", ColorMode.Cmyk);

            yield return TestCase(n++, new PdfConverter
            {
                ColorMode = ColorMode.Cmyk,
            }, "SamplePdf.ps", "ColorMode_Cmyk_RePdf");

            yield return TestCase(n++, new PdfConverter
            {
                ColorMode   = ColorMode.Grayscale,
            }, "SampleMix.ps", ColorMode.Grayscale);

            yield return TestCase(n++, new PdfConverter
            {
                ColorMode = ColorMode.Grayscale,
            }, "SamplePdf.ps", "ColorMode_Grayscale_RePdf");

            yield return TestCase(n++, new PdfConverter
            {
                ColorMode   = ColorMode.SameAsSource,
            }, "SampleMix.ps", ColorMode.SameAsSource);

            yield return TestCase(n++, new PdfConverter
            {
                ColorMode = ColorMode.SameAsSource,
            }, "SamplePdf.ps", "ColorMode_SameAsSource_RePdf");

            yield return TestCase(n++, new PdfConverter
            {
                ColorMode = ColorMode.DeviceIndependent,
            }, "SampleMix.ps", ColorMode.DeviceIndependent);

            yield return TestCase(n++, new PdfConverter
            {
                ColorMode = ColorMode.DeviceIndependent,
            }, "SamplePdf.ps", "ColorMode_DeviceIndependent_RePdf");

            /* --------------------------------------------------------- */
            // Compression
            /* --------------------------------------------------------- */
            yield return TestCase(n++, new PdfConverter
            {
                Compression     = Encoding.None,
                MonoCompression = Encoding.None,
            }, "SampleMix.ps", Encoding.None);

            yield return TestCase(n++, new PdfConverter
            {
                Compression     = Encoding.Flate,
                MonoCompression = Encoding.Flate,
            }, "SampleMix.ps", Encoding.Flate);

            yield return TestCase(n++, new PdfConverter
            {
                Compression     = Encoding.Jpeg,
                MonoCompression = Encoding.Fax,
            }, "SampleMix.ps", Encoding.Jpeg);

            yield return TestCase(n++, new PdfConverter
            {
                Compression     = Encoding.Lzw,
                MonoCompression = Encoding.Lzw,
            }, "SampleMix.ps", Encoding.Lzw);

            /* --------------------------------------------------------- */
            // Down-sampling
            /* --------------------------------------------------------- */
            yield return TestCase(n++, new PdfConverter
            {
                Downsampling = Downsampling.None,
            }, "SampleMix.ps", Downsampling.None);

            yield return TestCase(n++, new PdfConverter
            {
                Downsampling = Downsampling.Average,
            }, "SampleMix.ps", Downsampling.Average);

            yield return TestCase(n++, new PdfConverter
            {
                Downsampling = Downsampling.Bicubic,
            }, "SampleMix.ps", Downsampling.Bicubic);

            yield return TestCase(n++, new PdfConverter
            {
                Downsampling = Downsampling.Subsample,
            }, "SampleMix.ps", Downsampling.Subsample);

            /* --------------------------------------------------------- */
            // Compression and down-sampling
            /* --------------------------------------------------------- */
            yield return TestCase(n++, new PdfConverter
            {
                Compression  = Encoding.Jpeg,
                Downsampling = Downsampling.None,
                Resolution   = 900,
            }, "Sample600dpi.ps", "Jpeg_None_600_900");

            yield return TestCase(n++, new PdfConverter
            {
                Compression  = Encoding.Jpeg,
                Downsampling = Downsampling.None,
                Resolution   = 600,
            }, "Sample600dpi.ps", "Jpeg_None_600_600");

            yield return TestCase(n++, new PdfConverter
            {
                Compression  = Encoding.Jpeg,
                Downsampling = Downsampling.None,
                Resolution   = 300,
            }, "Sample600dpi.ps", "Jpeg_None_600_300");

            yield return TestCase(n++, new PdfConverter
            {
                Compression  = Encoding.Jpeg,
                Downsampling = Downsampling.None,
                Resolution   = 150,
            }, "Sample600dpi.ps", "Jpeg_None_600_150");

            yield return TestCase(n++, new PdfConverter
            {
                Compression  = Encoding.Jpeg,
                Downsampling = Downsampling.Bicubic,
                Resolution   = 900,
            }, "Sample600dpi.ps", "Jpeg_Bicubic_600_900");

            yield return TestCase(n++, new PdfConverter
            {
                Compression  = Encoding.Jpeg,
                Downsampling = Downsampling.Bicubic,
                Resolution   = 600,
            }, "Sample600dpi.ps", "Jpeg_Bicubic_600_600");

            yield return TestCase(n++, new PdfConverter
            {
                Compression  = Encoding.Jpeg,
                Downsampling = Downsampling.Bicubic,
                Resolution   = 300,
            }, "Sample600dpi.ps", "Jpeg_Bicubic_600_300");

            yield return TestCase(n++, new PdfConverter
            {
                Compression  = Encoding.Jpeg,
                Downsampling = Downsampling.Bicubic,
                Resolution   = 150,
            }, "Sample600dpi.ps", "Jpeg_Bicubic_600_150");

            yield return TestCase(n++, new PdfConverter
            {
                Compression  = Encoding.Flate,
                Downsampling = Downsampling.None,
                Resolution   = 900,
            }, "Sample600dpi.ps", "Flate_None_600_900");

            yield return TestCase(n++, new PdfConverter
            {
                Compression  = Encoding.Flate,
                Downsampling = Downsampling.None,
                Resolution   = 600,
            }, "Sample600dpi.ps", "Flate_None_600_600");

            yield return TestCase(n++, new PdfConverter
            {
                Compression  = Encoding.Flate,
                Downsampling = Downsampling.None,
                Resolution   = 300,
            }, "Sample600dpi.ps", "Flate_None_600_300");

            yield return TestCase(n++, new PdfConverter
            {
                Compression  = Encoding.Flate,
                Downsampling = Downsampling.None,
                Resolution   = 150,
            }, "Sample600dpi.ps", "Flate_None_600_150");

            yield return TestCase(n++, new PdfConverter
            {
                Compression  = Encoding.Flate,
                Downsampling = Downsampling.Bicubic,
                Resolution   = 900,
            }, "Sample600dpi.ps", "Flate_Bicubic_600_900");

            yield return TestCase(n++, new PdfConverter
            {
                Compression  = Encoding.Flate,
                Downsampling = Downsampling.Bicubic,
                Resolution   = 600,
            }, "Sample600dpi.ps", "Flate_Bicubic_600_600");

            yield return TestCase(n++, new PdfConverter
            {
                Compression  = Encoding.Flate,
                Downsampling = Downsampling.Bicubic,
                Resolution   = 300,
            }, "Sample600dpi.ps", "Flate_Bicubic_600_300");

            yield return TestCase(n++, new PdfConverter
            {
                Compression  = Encoding.Flate,
                Downsampling = Downsampling.Bicubic,
                Resolution   = 150,
            }, "Sample600dpi.ps", "Flate_Bicubic_600_150");

            /* --------------------------------------------------------- */
            //
            // EmbedFonts
            //
            // TODO: Fix the text garbling when setting EmbedFonts to
            // false.
            //
            /* --------------------------------------------------------- */
            yield return TestCase(n++, new PdfConverter
            {
                EmbedFonts = true,
            }, "Sample.ps", "EmbedFonts_True");

            yield return TestCase(n++, new PdfConverter
            {
                EmbedFonts  = true,
            }, "SampleCjk.ps", "EmbedFonts_True_Cjk");

            yield return TestCase(n++, new PdfConverter
            {
                EmbedFonts = false,
            }, "Sample.ps", "EmbedFonts_False");

            yield return TestCase(n++, new PdfConverter
            {
                EmbedFonts = false,
            }, "SampleCjk.ps", "EmbedFonts_False_Cjk");
        }}

        #endregion
    }
}
