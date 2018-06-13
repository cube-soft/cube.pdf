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

namespace Cube.Pdf.Tests.Ghostscript
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArgumentTest
    ///
    /// <summary>
    /// Argument のテスト用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class ArgumentTest
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// ToString
        ///
        /// <summary>
        /// 引数を表す文字列を取得するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public string ToString(Argument src) => src.ToString();

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
                yield return new TestCaseData(
                    new Argument('s', "DEVICE", "pdfwrite")
                ).Returns("-sDEVICE=pdfwrite");

                yield return new TestCaseData(
                    new Argument("ColorConversionStrategy", "RGB")
                ).Returns("-dColorConversionStrategy=/RGB");

                yield return new TestCaseData(
                    new Argument('r', "600")
                ).Returns("-r600");

                yield return new TestCaseData(
                    new Argument('f')
                ).Returns("-f");

                yield return new TestCaseData(
                    new Argument(@"c:\path\to\input.ps")
                ).Returns(@"c:\path\to\input.ps");
            }
        }

        #endregion
    }
}
