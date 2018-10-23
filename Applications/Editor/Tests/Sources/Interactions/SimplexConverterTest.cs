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
using Cube.FileSystem.TestService;
using Cube.Generics;
using Cube.Pdf.App.Editor;
using Cube.Xui;
using Cube.Xui.Converters;
using NUnit.Framework;
using System;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace Cube.Pdf.Tests.Editor.Interactions
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
        /// TitleConverter_Throws
        ///
        /// <summary>
        /// Confirms the result when the unsupported method is used.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void TitleConverter_Throws() => Assert.That(
            () => new TitleConverter().ConvertBack(null, null, null, null),
            Throws.TypeOf<NotSupportedException>()
        );

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
        [TestCase(Language.Auto,     "en", ExpectedResult = "Auto")]
        [TestCase(Language.English,  "en", ExpectedResult = "English")]
        [TestCase(Language.Japanese, "ja", ExpectedResult = "日本語")]
        public string LanguageConverter(Language src, string culture) =>
            Convert<string>(new LanguageConverter(), src, culture);

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
        [TestCase(100,     "en", ExpectedResult = "1 KB (100 Bytes)")]
        [TestCase(101309,  "en", ExpectedResult = "98.9 KB (101,309 Bytes)")]
        [TestCase(200,     "ja", ExpectedResult = "1 KB (200 バイト)")]
        [TestCase(7654321, "ja", ExpectedResult = "7.3 MB (7,654,321 バイト)")]
        public string ByteConverter(long n, string culture) =>
            Convert<string>(new ByteConverter(), n, culture);

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
        [TestCase(100,     "en", ExpectedResult = "1 KB")]
        [TestCase(101309,  "en", ExpectedResult = "98.9 KB")]
        [TestCase(200,     "ja", ExpectedResult = "1 KB")]
        [TestCase(7654321, "ja", ExpectedResult = "7.3 MB")]
        public string ByteConverterLite(long n, string culture) =>
            Convert<string>(new ByteConverterLite(), n, culture);

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
        [TestCase(EncryptionMethod.Standard40,  "en", ExpectedResult = "40-bit RC4")]
        [TestCase(EncryptionMethod.Standard128, "en", ExpectedResult = "128-bit RC4")]
        [TestCase(EncryptionMethod.Aes128,      "en", ExpectedResult = "128-bit AES")]
        [TestCase(EncryptionMethod.Aes256,      "en", ExpectedResult = "256-bit AES")]
        [TestCase(EncryptionMethod.Aes256r6,    "en", ExpectedResult = "256-bit AES (Revision 6)")]
        [TestCase(EncryptionMethod.Unknown,     "en", ExpectedResult = "Unknown")]
        [TestCase(EncryptionMethod.Unknown,     "ja", ExpectedResult = "Unknown")]
        public string EncryptionMethodConverter(EncryptionMethod src, string culture) =>
            Convert<string>(new EncryptionMethodConverter(), src, culture);

        #endregion

        #region ViewerPreferencesConverter

        /* ----------------------------------------------------------------- */
        ///
        /// ViewerPreferencesConverter
        ///
        /// <summary>
        /// Executes the test of the ViewerPreferencesConverter class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(ViewerPreferences.SinglePage,     "en", ExpectedResult = "Single page")]
        [TestCase(ViewerPreferences.OneColumn,      "ja", ExpectedResult = "連続ページ")]
        [TestCase(ViewerPreferences.TwoPageLeft,    "en", ExpectedResult = "Two page (left)")]
        [TestCase(ViewerPreferences.TwoPageRight,   "ja", ExpectedResult = "見開きページ (右綴じ)")]
        [TestCase(ViewerPreferences.TwoColumnLeft,  "en", ExpectedResult = "Two column (left)")]
        [TestCase(ViewerPreferences.TwoColumnRight, "ja", ExpectedResult = "連続見開きページ (右綴じ)")]
        public string ViewerPreferencesConverter(ViewerPreferences src, string culture) =>
            Convert<string>(new ViewerPreferencesConverter(), src, culture);

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
        [TestCase(10, "en", ExpectedResult = "10 pages")]
        [TestCase(20, "ja", ExpectedResult = "全 20 ページ")]
        public string CountToText(int n, string culture) =>
            Convert<string>(new CountToText(), n, culture);

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
        [TestCase(0, "en", ExpectedResult = "1")]
        [TestCase(9, "ja", ExpectedResult = "10")]
        public string IndexToText(int index, string culture) =>
            Convert<string>(new IndexToText(), index, culture);

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
        [TestCase(5, "en", ExpectedResult = "5 pages selected")]
        [TestCase(8, "ja", ExpectedResult = "8 個の項目を選択")]
        public string SelectionToText(int n, string culture) =>
            Convert<string>(new SelectionToText(), n, culture);

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
            Convert<Visibility>(new HasValueToVisibility(), GetExamplesWith("Sample.pdf")),
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
            Convert<Visibility>(new HasValueToVisibilityInverse(), GetExamplesWith("Sample.pdf")),
            Is.EqualTo(Visibility.Collapsed)
        );

        #endregion

        #endregion

        #region Others

        /* ----------------------------------------------------------------- */
        ///
        /// Setup
        ///
        /// <summary>
        /// Invokes the setup operation at once.
        /// </summary>
        ///
        /// <remarks>
        /// Properties.Resources クラスの表示文字列を設定言語毎に
        /// 切り替えるための処理を SettingsFolder の静的コンストラクタで
        /// 実行しています。このコンストラクタが確実に実行されるように
        /// Setup で SettingsFolder オブジェクトを生成します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        [OneTimeSetUp]
        public void Setup()
        {
            var dummy = new SettingsFolder(Assembly.GetExecutingAssembly(), IO);
        }

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
        private T Convert<T>(SimplexConverter src, object value, string culture)
        {
            ResourceCulture.Set(culture);
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
