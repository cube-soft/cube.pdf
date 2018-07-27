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
using Cube.Pdf.App.Editor;
using Cube.Xui.Converters;
using NUnit.Framework;
using System.Globalization;
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
    class SimplexConverterTest
    {
        #region Tests

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

        #endregion

        #region Helper methods

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
