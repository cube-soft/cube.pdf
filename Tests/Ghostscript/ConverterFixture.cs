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

namespace Cube.Pdf.Tests.Ghostscript
{
    /* --------------------------------------------------------------------- */
    ///
    /// ConverterFixture
    ///
    /// <summary>
    /// Converter およびそのサブクラスのテストを補助するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    abstract class ConverterFixture : FileFixture
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Run
        ///
        /// <summary>
        /// Converter オブジェクトを実行します。
        /// </summary>
        ///
        /// <param name="cv">Converter オブジェクト</param>
        /// <param name="src">入力ファイル名</param>
        /// <param name="dest">拡張子を含まない出力ファイル名</param>
        ///
        /// <returns>出力パス</returns>
        ///
        /* ----------------------------------------------------------------- */
        protected string Run(Converter cv, string src, string dest) =>
            Run(cv, src, dest, dest);

        /* ----------------------------------------------------------------- */
        ///
        /// Run
        ///
        /// <summary>
        /// Converter オブジェクトを実行します。
        /// </summary>
        ///
        /// <param name="cv">Converter オブジェクト</param>
        /// <param name="src">入力ファイル名</param>
        /// <param name="dest">拡張子を含まない出力ファイル名</param>
        /// <param name="log">拡張子を含まないログファイル名</param>
        ///
        /// <returns>出力パス</returns>
        ///
        /* ----------------------------------------------------------------- */
        protected string Run(Converter cv, string src, string dest, string log)
        {
            var sp  = GetExamplesWith(src);
            var dp  = GetResultsWith($"{dest}{cv.Format.GetExtension()}");
            var dir = IO.Get(AssemblyReader.Default.Location).DirectoryName;

            cv.Log           = GetResultsWith($"{log}.log");
            cv.Quiet         = false;
            cv.WorkDirectory = GetResultsWith("Tmp");
            cv.Resources.Add(IO.Combine(dir, "lib"));
            cv.Invoke(sp, dp);

            return dp;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// TestCase
        ///
        /// <summary>
        /// テストケースを生成します。
        /// </summary>
        ///
        /// <param name="cv">Converter オブジェクト</param>
        /// <param name="src">入力ファイル名</param>
        /// <param name="obj">出力ファイル名を決定するオブジェクト</param>
        ///
        /// <returns>テストケースオブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        protected static TestCaseData TestCase<T>(Converter cv, string src, T obj)
        {
            var cvt = $"{obj.GetType().Name}_{obj.ToString()}";
            return TestCase(cv, src, cvt);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// TestCase
        ///
        /// <summary>
        /// テストケースを生成します。
        /// </summary>
        ///
        /// <param name="cv">Converter オブジェクト</param>
        /// <param name="src">入力ファイル名</param>
        /// <param name="dest">拡張子を含まない出力ファイル名</param>
        ///
        /// <returns>テストケースオブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        protected static TestCaseData TestCase(Converter cv, string src, string dest) =>
            new TestCaseData(cv, src, dest);

        #endregion
    }
}
