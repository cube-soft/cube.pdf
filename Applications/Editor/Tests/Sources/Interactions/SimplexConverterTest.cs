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
using Cube.FileSystem;
using Cube.Mixin.String;
using Cube.Tests;
using Cube.Xui.Converters;
using NUnit.Framework;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Cube.Pdf.Editor.Tests.Interactions
{
    /* --------------------------------------------------------------------- */
    ///
    /// SimplexConverterTest
    ///
    /// <summary>
    /// Tests for various converter classes.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class SimplexConverterTest : FileFixture
    {
        #region Tests

        #region TitleConverter

        /* ----------------------------------------------------------------- */
        ///
        /// TitleConverter
        ///
        /// <summary>
        /// Executes the test of the TitleConverter class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("Dir\\To\\File.pdf", false, ExpectedResult = "File.pdf - CubePDF Utility")]
        [TestCase("Dir\\To\\Mode.pdf", true,  ExpectedResult = "Mode.pdf* - CubePDF Utility")]
        [TestCase("Test",              false, ExpectedResult = "Test - CubePDF Utility")]
        [TestCase("Modified",          true,  ExpectedResult = "Modified* - CubePDF Utility")]
        [TestCase("",                  false, ExpectedResult = "CubePDF Utility")]
        [TestCase("",                  true,  ExpectedResult = "CubePDF Utility")]
        public string TitleConverter(string src, bool modified)
        {
            var fi   = src.HasValue() ? IO.Get(src) : null;
            var args = new object[] { fi, modified };
            var type = typeof(string);
            var ci   = CultureInfo.CurrentCulture;
            var dest = new TitleConverter();

            Assert.That(dest.ProvideValue(null), Is.EqualTo(dest));
            return dest.Convert(args, type, null, ci) as string;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// TitleConverter_Empty
        ///
        /// <summary>
        /// Tests the TitleConverter class with the empty parameters.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void TitleConverter_Empty()
        {
            var src  = new TitleConverter();
            var type = typeof(string);
            var ci   = CultureInfo.CurrentCulture;
            var dest = src.Convert(new object[0], type, null, ci) as string;

            Assert.That(dest, Is.EqualTo("CubePDF Utility"));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// TitleConverter_Throws
        ///
        /// <summary>
        /// Confirms the result when the unsupported method is used.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void TitleConverter_Throws()
        {
            var src = new TitleConverter();
            Assert.That(() => src.ConvertBack(null, null, null, null), Throws.TypeOf<NotSupportedException>());
        }

        #endregion

        #region IconConverter

        /* ----------------------------------------------------------------- */
        ///
        /// IconConverter
        ///
        /// <summary>
        /// Executes the test of the IconConverter class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("Sample.pdf", ExpectedResult = true)]
        [TestCase("NotFound",   ExpectedResult = true)]
        [TestCase("",           ExpectedResult = false)]
        public bool IconConverter(string src) => Convert<ImageSource>(
            new IconConverter(),
            src.HasValue() ? IO.Get(GetSource(src)) : null
        ) != null;

        #endregion

        #region LanguageConverter

        /* ----------------------------------------------------------------- */
        ///
        /// LanguageConverter
        ///
        /// <summary>
        /// Executes the test of the LanguageConverter class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(Language.Auto,     Language.English,  ExpectedResult = "Auto")]
        [TestCase(Language.English,  Language.English,  ExpectedResult = "English")]
        [TestCase(Language.Japanese, Language.Japanese, ExpectedResult = "Japanese")]
        public string LanguageConverter(Language src, Language lang) =>
            Convert<string>(new LanguageConverter(), src, lang);

        #endregion

        #region ByteConverter

        /* ----------------------------------------------------------------- */
        ///
        /// ByteConverter
        ///
        /// <summary>
        /// Executes the test of the ByteConverter class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(100,     Language.English,  ExpectedResult = "1 KB (100 Bytes)")]
        [TestCase(101309,  Language.English,  ExpectedResult = "98.9 KB (101,309 Bytes)")]
        [TestCase(200,     Language.Japanese, ExpectedResult = "1 KB (200 バイト)")]
        [TestCase(7654321, Language.Japanese, ExpectedResult = "7.3 MB (7,654,321 バイト)")]
        public string ByteConverter(long n, Language lang) =>
            Convert<string>(new ByteConverter(), n, lang);

        #endregion

        #region ByteConverterLite

        /* ----------------------------------------------------------------- */
        ///
        /// ByteConverter
        ///
        /// <summary>
        /// Executes the test of the ByteConverter class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(100,     Language.English,  ExpectedResult = "1 KB")]
        [TestCase(101309,  Language.English,  ExpectedResult = "98.9 KB")]
        [TestCase(200,     Language.Japanese, ExpectedResult = "1 KB")]
        [TestCase(7654321, Language.Japanese, ExpectedResult = "7.3 MB")]
        public string ByteConverterLite(long n, Language lang) =>
            Convert<string>(new ByteConverterLite(), n, lang);

        #endregion

        #region EncryptionMethodConverter

        /* ----------------------------------------------------------------- */
        ///
        /// EncryptionMethodConverter
        ///
        /// <summary>
        /// Executes the test of the EncryptionMethodConverter class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(EncryptionMethod.Standard40,  Language.English,  ExpectedResult = "40-bit RC4")]
        [TestCase(EncryptionMethod.Standard128, Language.English,  ExpectedResult = "128-bit RC4")]
        [TestCase(EncryptionMethod.Aes128,      Language.English,  ExpectedResult = "128-bit AES")]
        [TestCase(EncryptionMethod.Aes256,      Language.English,  ExpectedResult = "256-bit AES")]
        [TestCase(EncryptionMethod.Aes256r6,    Language.English,  ExpectedResult = "256-bit AES (Revision 6)")]
        [TestCase(EncryptionMethod.Unknown,     Language.English,  ExpectedResult = "Unknown")]
        [TestCase(EncryptionMethod.Unknown,     Language.Japanese, ExpectedResult = "Unknown")]
        public string EncryptionMethodConverter(EncryptionMethod src, Language lang) =>
            Convert<string>(new EncryptionMethodConverter(), src, lang);

        #endregion

        #region ViewerOptionsConverter

        /* ----------------------------------------------------------------- */
        ///
        /// ViewerPreferencesConverter
        ///
        /// <summary>
        /// Executes the test of the ViewerPreferencesConverter class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(ViewerOption.SinglePage,     Language.English,  ExpectedResult = "Single page")]
        [TestCase(ViewerOption.OneColumn,      Language.Japanese, ExpectedResult = "連続ページ")]
        [TestCase(ViewerOption.TwoPageLeft,    Language.English,  ExpectedResult = "Two page (left)")]
        [TestCase(ViewerOption.TwoPageRight,   Language.Japanese, ExpectedResult = "見開きページ (右綴じ)")]
        [TestCase(ViewerOption.TwoColumnLeft,  Language.English,  ExpectedResult = "Two column (left)")]
        [TestCase(ViewerOption.TwoColumnRight, Language.Japanese, ExpectedResult = "連続見開きページ (右綴じ)")]
        public string ViewerOptionsConverter(ViewerOption src, Language lang) =>
            Convert<string>(new ViewerOptionsConverter(), src, lang);

        #endregion

        #region BooleanToCursor

        /* ----------------------------------------------------------------- */
        ///
        /// BooleanToWaitCursor
        ///
        /// <summary>
        /// Executes the test of the BooleanToCursor class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void BooleanToWaitCursor() => Assert.That(
            Convert<Cursor>(new BooleanToCursor(), true),
            Is.EqualTo(Cursors.Wait)
        );

        /* ----------------------------------------------------------------- */
        ///
        /// BooleanToArrowCursor
        ///
        /// <summary>
        /// Executes the test of the BooleanToCursor class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void BooleanToArrowCursor() => Assert.That(
            Convert<Cursor>(new BooleanToCursor(), false),
            Is.EqualTo(Cursors.Arrow)
        );

        #endregion

        #region CountToText

        /* ----------------------------------------------------------------- */
        ///
        /// CountToText
        ///
        /// <summary>
        /// Executes the test of the CountToText class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(10, Language.English,  ExpectedResult = "10 pages")]
        [TestCase(20, Language.Japanese, ExpectedResult = "全 20 ページ")]
        public string CountToText(int n, Language lang) =>
            Convert<string>(new CountToText(), n, lang);

        #endregion

        #region IndexToText

        /* ----------------------------------------------------------------- */
        ///
        /// IndexToText
        ///
        /// <summary>
        /// Executes the test of the IndexToText class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(0, Language.English,  ExpectedResult = "1")]
        [TestCase(9, Language.Japanese, ExpectedResult = "10")]
        public string IndexToText(int index, Language lang) =>
            Convert<string>(new IndexToText(), index, lang);

        #endregion

        #region SelectionToText

        /* ----------------------------------------------------------------- */
        ///
        /// SelectionToText
        ///
        /// <summary>
        /// Executes the test of the SelectionToText class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(5, Language.English,  ExpectedResult = "5 pages selected")]
        [TestCase(8, Language.Japanese, ExpectedResult = "8 個の項目を選択")]
        public string SelectionToText(int n, Language lang) =>
            Convert<string>(new SelectionToText(), n, lang);

        #endregion

        #region SelectionToVisibility

        /* ----------------------------------------------------------------- */
        ///
        /// SelectionToVisibility
        ///
        /// <summary>
        /// Executes the test of the SelectionToVisibility class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void SelectionToVisibility() => Assert.That(
            Convert<Visibility>(new SelectionToVisibility(), 10),
            Is.EqualTo(Visibility.Visible)
        );

        #endregion

        #region HasValueToVisibility

        /* ----------------------------------------------------------------- */
        ///
        /// HasValueToVisibility
        ///
        /// <summary>
        /// Executes the test of the HasValueToVisibility class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void HasValueToVisibility() => Assert.That(
            Convert<Visibility>(new HasValueToVisibility(), GetSource("Sample.pdf")),
            Is.EqualTo(Visibility.Visible)
        );

        #endregion

        #region HasValueToVisibilityInverse

        /* ----------------------------------------------------------------- */
        ///
        /// HasValueToVisibilityInverse
        ///
        /// <summary>
        /// Executes the test of the HasValueToVisibilityInverse class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void HasValueToVisibilityInverse() => Assert.That(
            Convert<Visibility>(new HasValueToVisibilityInverse(), GetSource("Sample.pdf")),
            Is.EqualTo(Visibility.Collapsed)
        );

        #endregion

        #endregion

        #region Others

        /* ----------------------------------------------------------------- */
        ///
        /// Convert
        ///
        /// <summary>
        /// Sets the culture and executes the Convert method of the
        /// specified SimplexConverter.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private T Convert<T>(SimplexConverter src, object value, Language lang)
        {
            Locale.Set(lang);
            return Convert<T>(src, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Convert
        ///
        /// <summary>
        /// Executes the Convert method of the specified SimplexConverter.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private T Convert<T>(SimplexConverter src, object value) =>
            (T)src.Convert(value, typeof(T), null, CultureInfo.CurrentCulture);

        #endregion
    }
}
