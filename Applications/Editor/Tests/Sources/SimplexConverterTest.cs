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
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace Cube.Pdf.Tests.Editor
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
        /// Convert_Title
        ///
        /// <summary>
        /// Tests to convert to a title.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("Dir\\To\\File.pdf", ExpectedResult = "File.pdf - CubePDF Utility")]
        [TestCase("Test",              ExpectedResult = "Test - CubePDF Utility")]
        [TestCase("",                  ExpectedResult = "CubePDF Utility")]
        public string Convert_Title(string src) =>
            Convert<string>(new TitleConverter(), src.HasValue() ? IO.Get(src) : null);

        #endregion

        #region LanguageConverter

        /* ----------------------------------------------------------------- */
        ///
        /// Convert_Language
        ///
        /// <summary>
        /// Tests to convert to a language.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(Language.Auto,     "en", ExpectedResult = "Auto")]
        [TestCase(Language.English,  "en", ExpectedResult = "English")]
        [TestCase(Language.Japanese, "ja", ExpectedResult = "日本語")]
        public string Convert_Language(Language src, string culture)
        {
            ResourceCulture.Set(culture);
            return Convert<string>(new LanguageConverter(), src);
        }

        #endregion

        #region ByteConverter

        /* ----------------------------------------------------------------- */
        ///
        /// Convert_Byte
        ///
        /// <summary>
        /// Tests to convert a byte size.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(100,     "en", ExpectedResult = "1 KB (100 Bytes)")]
        [TestCase(101309,  "en", ExpectedResult = "98.9 KB (101,309 Bytes)")]
        [TestCase(200,     "ja", ExpectedResult = "1 KB (200 バイト)")]
        [TestCase(7654321, "ja", ExpectedResult = "7.3 MB (7,654,321 バイト)")]
        public string Convert_Byte(long n, string culture)
        {
            ResourceCulture.Set(culture);
            return Convert<string>(new ByteConverter(), n);
        }

        #endregion

        #region EncryptionMethodConverter

        /* ----------------------------------------------------------------- */
        ///
        /// Convert_EncryptionMethod
        ///
        /// <summary>
        /// Tests to convert an EncryptionMethod value.
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
        public string Convert_EncryptionMethod(EncryptionMethod src, string culture)
        {
            ResourceCulture.Set(culture);
            return Convert<string>(new EncryptionMethodConverter(), src);
        }

        #endregion

        #region ViewerPreferencesConverter

        /* ----------------------------------------------------------------- */
        ///
        /// Convert_ViewerPreferences
        ///
        /// <summary>
        /// Tests to convert a ViewerPreferences value.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(ViewerPreferences.SinglePage,     "en", ExpectedResult = "Single page")]
        [TestCase(ViewerPreferences.OneColumn,      "ja", ExpectedResult = "連続ページ")]
        [TestCase(ViewerPreferences.TwoPageLeft,    "en", ExpectedResult = "Two page (left)")]
        [TestCase(ViewerPreferences.TwoPageRight,   "ja", ExpectedResult = "見開きページ (右綴じ)")]
        [TestCase(ViewerPreferences.TwoColumnLeft,  "en", ExpectedResult = "Two column (left)")]
        [TestCase(ViewerPreferences.TwoColumnRight, "ja", ExpectedResult = "連続見開きページ (右綴じ)")]
        public string Convert_ViewerPreferences(ViewerPreferences src, string culture)
        {
            ResourceCulture.Set(culture);
            return Convert<string>(new ViewerPreferencesConverter(), src);
        }

        #endregion

        #region BooleanToCursor

        /* ----------------------------------------------------------------- */
        ///
        /// Convert_WaitCursor
        ///
        /// <summary>
        /// Tests to convert a boolean value to the wait cursor.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Convert_WaitCursor() => Assert.That(
            Convert<Cursor>(new BooleanToCursor(), true),
            Is.EqualTo(Cursors.Wait)
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Convert_ArrowCursor
        ///
        /// <summary>
        /// Tests to convert a boolean value to the arrow cursor.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Convert_ArrowCursor() => Assert.That(
            Convert<Cursor>(new BooleanToCursor(), false),
            Is.EqualTo(Cursors.Arrow)
        );

        #endregion

        #region CountToText

        /* ----------------------------------------------------------------- */
        ///
        /// Convert_CountToText
        ///
        /// <summary>
        /// Tests to convert a number of pages to text.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(10, "en", ExpectedResult = "10 pages")]
        [TestCase(20, "ja", ExpectedResult = "全 20 ページ")]
        public string Convert_CountToText(int n, string culture)
        {
            ResourceCulture.Set(culture);
            return Convert<string>(new CountToText(), n);
        }

        #endregion

        #region IndexToText

        /* ----------------------------------------------------------------- */
        ///
        /// Convert_IndexToText
        ///
        /// <summary>
        /// Tests to convert an index to text.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(0, "en", ExpectedResult = "1")]
        [TestCase(9, "ja", ExpectedResult = "10")]
        public string Convert_IndexToText(int index, string culture)
        {
            ResourceCulture.Set(culture);
            return Convert<string>(new IndexToText(), index);
        }

        #endregion

        #region SelectionToText

        /* ----------------------------------------------------------------- */
        ///
        /// Convert_SelectionToText
        ///
        /// <summary>
        /// Tests to convert selection to text.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(5, "en", ExpectedResult = "5 pages selected")]
        [TestCase(8, "ja", ExpectedResult = "8 個の項目を選択")]
        public string Convert_SelectionToText(int n, string culture)
        {
            ResourceCulture.Set(culture);
            return Convert<string>(new SelectionToText(), n);
        }

        #endregion

        #region SelectionToVisibility

        /* ----------------------------------------------------------------- */
        ///
        /// Convert_SelectionToVisibility
        ///
        /// <summary>
        /// Tests to convert selection to visibility.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Convert_SelectionToVisibility() => Assert.That(
            Convert<Visibility>(new SelectionToVisibility(), 10),
            Is.EqualTo(Visibility.Visible)
        );

        #endregion

        #region HasValueToVisibility

        /* ----------------------------------------------------------------- */
        ///
        /// Convert_HasValueToVisibility
        ///
        /// <summary>
        /// Tests to convert an object to visibility.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Convert_HasValueToVisibility() => Assert.That(
            Convert<Visibility>(new HasValueToVisibility(), GetExamplesWith("Sample.pdf")),
            Is.EqualTo(Visibility.Visible)
        );

        #endregion

        #region HasValueToVisibilityInverse

        /* ----------------------------------------------------------------- */
        ///
        /// Convert_HasValueToVisibilityInverse
        ///
        /// <summary>
        /// Tests to convert an object to visibility.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Convert_HasValueToVisibilityInverse() => Assert.That(
            Convert<Visibility>(new HasValueToVisibilityInverse(), GetExamplesWith("Sample.pdf")),
            Is.EqualTo(Visibility.Collapsed)
        );

        #endregion

        #endregion

        #region Helper methods

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
        /// Convert メソッドを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private T Convert<T>(SimplexConverter src, object value) =>
            (T)src.Convert(value, typeof(T), null, CultureInfo.CurrentCulture);

        #endregion
    }
}
