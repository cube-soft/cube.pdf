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
using System.Collections.Generic;
using System.Linq;

namespace Cube.Pdf.Tests.Ghostscript
{
    /* --------------------------------------------------------------------- */
    ///
    /// ImageConverterTest
    ///
    /// <summary>
    /// ImageConverter のテスト用クラスです。
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
        public void SupportedFormats() => Assert.That(
            new ImageConverter(Format.Png).SupportedFormats.Count(),
            Is.EqualTo(30)
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// 変換処理テストを実行します。
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
        /// テストケース一覧を取得します。
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
                }, "SampleResolution600.ps", "Quality_1");

                yield return TestCase(new JpegConverter(Format.Jpeg)
                {
                    Quality = 25,
                }, "SampleResolution600.ps", "Quality_25");

                yield return TestCase(new JpegConverter(Format.Jpeg)
                {
                    Quality = 50,
                }, "SampleResolution600.ps", "Quality_50");

                yield return TestCase(new JpegConverter(Format.Jpeg)
                {
                    Quality = 75,
                }, "SampleResolution600.ps", "Quality_75");

                yield return TestCase(new JpegConverter(Format.Jpeg)
                {
                    Quality = 100,
                }, "SampleResolution600.ps", "Quality_100");
            }
        }

        #endregion
    }
}
