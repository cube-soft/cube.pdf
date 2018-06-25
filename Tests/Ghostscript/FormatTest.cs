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
using Cube.Pdf.Ghostscript;
using NUnit.Framework;
using System;
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
    class FormatTest : FileFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Convert
        ///
        /// <summary>
        /// 指定されたフォーマットに変換するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public void Convert(Format fmt)
        {
            var name = $"{nameof(Convert)}_{fmt.ToString()}";
            var dest = GetResultsWith($"{name}{fmt.GetExtension()}");
            var src  = GetExamplesWith("Sample.eps");
            var conv = new Converter(fmt) { Resolution = 72 };

            conv.Invoke(src, dest);
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
                foreach (Format src in Enum.GetValues(typeof(Format)))
                {
                    yield return new TestCaseData(src);
                }
            }
        }

        #endregion
    }
}
