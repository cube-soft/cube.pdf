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
            Assert.That(() => new ImageConverter(Format.Pdf), Throws.TypeOf<NotSupportedException>());
            Assert.That(() => new JpegConverter(Format.Png),  Throws.TypeOf<NotSupportedException>());
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Exexutes the test to convert.
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
                // AntiAlias
                /* --------------------------------------------------------- */
                yield return TestCase(new ImageConverter(Format.Png)
                {
                    AntiAlias  = true,
                    Resolution = 72,
                }, "Sample.ps", "AntiAlias_True");

                yield return TestCase(new ImageConverter(Format.Png)
                {
                    AntiAlias  = false,
                    Resolution = 72,
                }, "Sample.ps", "AntiAlias_False");

                /* --------------------------------------------------------- */
                // Quality
                /* --------------------------------------------------------- */
                yield return TestCase(new JpegConverter(Format.Jpeg)
                {
                    Quality = 1,
                }, "Sample600dpi.ps", "Quality_1");

                yield return TestCase(new JpegConverter(Format.Jpeg)
                {
                    Quality = 25,
                }, "Sample600dpi.ps", "Quality_25");

                yield return TestCase(new JpegConverter(Format.Jpeg)
                {
                    Quality = 50,
                }, "Sample600dpi.ps", "Quality_50");

                yield return TestCase(new JpegConverter(Format.Jpeg)
                {
                    Quality = 75,
                }, "Sample600dpi.ps", "Quality_75");

                yield return TestCase(new JpegConverter(Format.Jpeg)
                {
                    Quality = 100,
                }, "Sample600dpi.ps", "Quality_100");
            }
        }

        #endregion
    }
}
