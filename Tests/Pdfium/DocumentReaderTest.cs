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
using Cube.FileSystem.Tests;
using Cube.Pdf.Pdfium;
using NUnit.Framework;

namespace Cube.Pdf.Tests.Pdfium
{
    /* --------------------------------------------------------------------- */
    ///
    /// DocumentReaderTest
    ///
    /// <summary>
    /// DocumentReader のテスト用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class DocumentReaderTest : FileFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// PageCount
        ///
        /// <summary>
        /// ページ数を取得するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("Sample.pdf", ExpectedResult = 2)]
        public int PageCount(string filename) =>
            new DocumentReader(GetExamplesWith(filename)).File.Count;

        #endregion
    }
}
