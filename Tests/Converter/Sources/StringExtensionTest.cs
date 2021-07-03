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
using NUnit.Framework;

namespace Cube.Pdf.Converter.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// StringExtensionTest
    ///
    /// <summary>
    /// StringExtension のテスト用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class StringExtensionTest
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// WordWrap
        ///
        /// <summary>
        /// 特定の文字数で折り返すテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("a",       5, ExpectedResult = 1)]
        [TestCase("abcde",   5, ExpectedResult = 1)]
        [TestCase("abcdefg", 5, ExpectedResult = 2)]
        [TestCase("",        5, ExpectedResult = 1)]
        public int WordWrap(string src, int n) =>
            src.WordWrap(n)
               .Split(new[] { Environment.NewLine }, StringSplitOptions.None)
               .Length;

        #endregion
    }
}
