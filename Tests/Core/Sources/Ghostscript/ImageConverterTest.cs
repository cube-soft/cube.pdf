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
using Cube.FileSystem;
using Cube.Pdf.Ghostscript;
using NUnit.Framework;

/* ------------------------------------------------------------------------- */
///
/// ImageConverterTest
///
/// <summary>
/// Represents tests of the ImageConverter class.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[TestFixture]
class ImageConverterTest : ConverterFixture
{
    private static IEnumerable<TestCaseData> TestCases
    {
        get
        {
            var n = 0;

            /* ------------------------------------------------------------- */
            // AntiAlias
            /* ------------------------------------------------------------- */
            yield return TestCase(n++, new ImageConverter(Format.Png) {
                AntiAlias = true,
                Resolution = 72,
            }, "Sample.ps", "AntiAlias_True");

            yield return TestCase(n++, new ImageConverter(Format.Png) {
                AntiAlias = false,
                Resolution = 72,
            }, "Sample.ps", "AntiAlias_False");

            /* ------------------------------------------------------------- */
            // ColorMode
            /* ------------------------------------------------------------- */
            yield return TestCase(n++, new ImageConverter(Format.Jpeg24bppRgb) {
                Resolution = 300,
            }, "SampleMix.ps", Format.Jpeg24bppRgb);

            yield return TestCase(n++, new ImageConverter(Format.Jpeg32bppCmyk) {
                Resolution = 300,
            }, "SampleMix.ps", Format.Jpeg32bppCmyk);

            yield return TestCase(n++, new ImageConverter(Format.Jpeg8bppGrayscale) {
                Resolution = 300,
            }, "SampleMix.ps", Format.Jpeg8bppGrayscale);

            /* ------------------------------------------------------------- */
            // JPEG Quality
            /* ------------------------------------------------------------- */
            yield return TestCase(n++, new JpegConverter(Format.Jpeg) {
                Quality = 1,
            }, "Sample600dpi.ps", "Quality_1");

            yield return TestCase(n++, new JpegConverter(Format.Jpeg) {
                Quality = 25,
            }, "Sample600dpi.ps", "Quality_25");

            yield return TestCase(n++, new JpegConverter(Format.Jpeg) {
                Quality = 50,
            }, "Sample600dpi.ps", "Quality_50");

            yield return TestCase(n++, new JpegConverter(Format.Jpeg) {
                Quality = 75,
            }, "Sample600dpi.ps", "Quality_75");

            yield return TestCase(n++, new JpegConverter(Format.Jpeg) {
                Quality = 100,
            }, "Sample600dpi.ps", "Quality_100");

            /* ------------------------------------------------------------- */
            // TIFF encoding
            /* ------------------------------------------------------------- */
            yield return TestCase(n++, new TiffConverter(Format.Tiff1bppMonochrome) {
                Encoding = Encoding.G3Fax,
            }, "SampleMix.ps", Encoding.G3Fax);

            yield return TestCase(n++, new TiffConverter(Format.Tiff1bppMonochrome) {
                Encoding = Encoding.G4Fax,
            }, "SampleMix.ps", Encoding.G4Fax);

            yield return TestCase(n++, new TiffConverter(Format.Tiff1bppMonochrome) {
                Encoding = Encoding.Lzw,
            }, "SampleMix.ps", Encoding.Lzw);

            yield return TestCase(n++, new TiffConverter(Format.Tiff1bppMonochrome) {
                Encoding = Encoding.PackBits,
            }, "SampleMix.ps", Encoding.PackBits);

            yield return TestCase(n++, new TiffConverter(Format.Tiff1bppMonochrome) {
                Encoding = Encoding.None,
            }, "SampleMix.ps", Encoding.None);
        }
    }

    #region Tests

    /* --------------------------------------------------------------------- */
    ///
    /// Test
    ///
    /// <summary>
    /// Tests the ImageConverter and its inherited classes.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestCaseSource(nameof(TestCases))]
    public void Test(int id, Converter cv, string srcname, string destname)
    {
        var dest = Run(cv, srcname, destname);
        Assert.That(Io.Exists(dest), Is.True, $"No.{id}");
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Check
    ///
    /// <summary>
    /// Confirms the basic behaviors.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Test]
    public void Check()
    {
        Assert.That(ImageConverter.SupportedFormats.Count(), Is.EqualTo(30));
        Assert.That(JpegConverter.SupportedFormats.Count(),  Is.EqualTo(4));

        Assert.That(() => new ImageConverter(Format.Pdf), Throws.TypeOf<NotSupportedException>());
        Assert.That(() => new JpegConverter(Format.Png),  Throws.TypeOf<NotSupportedException>());
        Assert.That(() => new TiffConverter(Format.Png),  Throws.TypeOf<NotSupportedException>());
    }

    #endregion
}
