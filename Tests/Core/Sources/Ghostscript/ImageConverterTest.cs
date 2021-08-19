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
    /// ImageConverterTest
    ///
    /// <summary>
    /// Represents tests of the ImageConverter class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class ImageConverterTest : ConverterFixture
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
            Assert.That(ImageConverter.SupportedFormats.Count(), Is.EqualTo(30));
            Assert.That(JpegConverter.SupportedFormats.Count(),  Is.EqualTo(4));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Create_NotSupportedException
        ///
        /// <summary>
        /// Confirms the behavior when an unsupported format is set.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Create_NotSupportedException()
        {
            Assert.That(() => new ImageConverter(Format.Pdf), Throws.TypeOf<NotSupportedException>());
            Assert.That(() => new JpegConverter(Format.Png),  Throws.TypeOf<NotSupportedException>());
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Tests the converter object.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public void Invoke(int id, Converter cv, string srcname, string destname)
        {
            var dest = Run(cv, srcname, destname);
            Assert.That(Io.Exists(dest), Is.True, $"No.{id}");
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
            // AntiAlias
            /* --------------------------------------------------------- */
            yield return TestCase(n++, new ImageConverter(Format.Png)
            {
                AntiAlias  = true,
                Resolution = 72,
            }, "Sample.ps", "AntiAlias_True");

            yield return TestCase(n++, new ImageConverter(Format.Png)
            {
                AntiAlias  = false,
                Resolution = 72,
            }, "Sample.ps", "AntiAlias_False");

            /* --------------------------------------------------------- */
            // ColorMode
            /* --------------------------------------------------------- */
            yield return TestCase(n++, new ImageConverter(Format.Jpeg24bppRgb)
            {
                Resolution = 300,
            }, "SampleMix.ps", Format.Jpeg24bppRgb);

            yield return TestCase(n++, new ImageConverter(Format.Jpeg32bppCmyk)
            {
                Resolution = 300,
            }, "SampleMix.ps", Format.Jpeg32bppCmyk);

            yield return TestCase(n++, new ImageConverter(Format.Jpeg8bppGrayscale)
            {
                Resolution = 300,
            }, "SampleMix.ps", Format.Jpeg8bppGrayscale);

            /* --------------------------------------------------------- */
            // Quality
            /* --------------------------------------------------------- */
            yield return TestCase(n++, new JpegConverter(Format.Jpeg)
            {
                Quality = 1,
            }, "Sample600dpi.ps", "Quality_1");

            yield return TestCase(n++, new JpegConverter(Format.Jpeg)
            {
                Quality = 25,
            }, "Sample600dpi.ps", "Quality_25");

            yield return TestCase(n++, new JpegConverter(Format.Jpeg)
            {
                Quality = 50,
            }, "Sample600dpi.ps", "Quality_50");

            yield return TestCase(n++, new JpegConverter(Format.Jpeg)
            {
                Quality = 75,
            }, "Sample600dpi.ps", "Quality_75");

            yield return TestCase(n++, new JpegConverter(Format.Jpeg)
            {
                Quality = 100,
            }, "Sample600dpi.ps", "Quality_100");
        }}

        #endregion
    }
}
