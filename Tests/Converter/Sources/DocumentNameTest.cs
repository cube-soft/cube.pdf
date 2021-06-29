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
using NUnit.Framework;

namespace Cube.Pdf.Converter.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// DocumentNameTest
    ///
    /// <summary>
    /// DocumentName のテスト用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class DocumentNameTest
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Name
        ///
        /// <summary>
        /// ドキュメント名をファイル名に変換するテストを実行します。
        /// </summary>
        ///
        /// <remarks>
        /// "File - .txt" -> ".txt" の変換に関して、もう少し良い方法が
        /// ないか要検討。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("Sample",                            ExpectedResult = "Sample")]
        [TestCase("Sample.png",                        ExpectedResult = "Sample.png")]
        [TestCase("Symbol*:?\"<>|",                    ExpectedResult = "Symbol_______")]
        [TestCase("Head.txt - Application",            ExpectedResult = "Head.txt")]
        [TestCase("Application - Tail.txt",            ExpectedResult = "Tail.txt")]
        [TestCase("File - Application",                ExpectedResult = "File - Application")]
        [TestCase("- Application",                     ExpectedResult = "- Application")]
        [TestCase("File - .txt",                       ExpectedResult = ".txt")] // see remarks
        [TestCase("http://www.example.com/index.html", ExpectedResult = "index.html")]
        [TestCase("",                                  ExpectedResult = "Default")]
        [TestCase(null,                                ExpectedResult = "Default")]
        public string Name(string src) => new DocumentName(src, "Default").Value;

        #endregion
    }
}
