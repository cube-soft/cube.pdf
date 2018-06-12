/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
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
    /// Argument のテストを行うクラスです。
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
