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

namespace Cube.Pdf.Tests.Ghostscript
{
    /* --------------------------------------------------------------------- */
    ///
    /// DocumentConverterTest
    ///
    /// <summary>
    /// DocumentConverter のテスト用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class DocumentConverterTest : ConverterFixture
    {
        #region Tests

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
                // Version
                /* --------------------------------------------------------- */
                yield return TestCase(new DocumentConverter(Format.Pdf)
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
                yield return TestCase(new DocumentConverter(Format.Pdf)
                {
                    EmbedFonts = true,
                }, "Sample.ps", "EmbedFonts_True_1");

                yield return TestCase(new DocumentConverter(Format.Pdf)
                {
                    EmbedFonts  = true,
                    Orientation = Orientation.Portrait,
                }, "Sample.ps", "EmbedFonts_True_2");

                yield return TestCase(new DocumentConverter(Format.Pdf)
                {
                    EmbedFonts = false,
                }, "Sample.ps", "EmbedFonts_False");

                yield return TestCase(new DocumentConverter(Format.Pdf)
                {
                    EmbedFonts = false,
                }, "SampleCjk.ps", "EmbedFonts_False_Cjk");

                /* --------------------------------------------------------- */
                // ColorMode
                /* --------------------------------------------------------- */
                yield return TestCase(new DocumentConverter(Format.Pdf)
                {
                    ColorMode   = ColorMode.Rgb,
                    Orientation = Orientation.Portrait,
                }, "Sample.eps", ColorMode.Rgb);

                yield return TestCase(new DocumentConverter(Format.Pdf)
                {
                    ColorMode   = ColorMode.Cmyk,
                    Orientation = Orientation.Portrait,
                }, "Sample.eps", ColorMode.Cmyk);

                yield return TestCase(new DocumentConverter(Format.Pdf)
                {
                    ColorMode   = ColorMode.Grayscale,
                    Orientation = Orientation.Portrait,
                }, "Sample.eps", ColorMode.Grayscale);
            }
        }

        #endregion
    }
}
