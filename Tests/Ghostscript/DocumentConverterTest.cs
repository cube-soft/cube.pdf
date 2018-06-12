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
    /// DocumentConverterTest
    ///
    /// <summary>
    /// DocumentConverter のテスト用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class DocumentConverterTest : ConverterFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// 変換処理テストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public void Invoke(string filename, Converter conv, string name)
        {
            var lib  = IO.Get(AssemblyReader.Default.Location).DirectoryName;
            var dest = GetResultsWith($"{name}{conv.Format.GetExtension()}");
            var src  = GetExamplesWith(filename);

            conv.Log   = GetResultsWith($"{name}.log");
            conv.Quiet = false;
            conv.Resources.Add(IO.Combine(lib, "lib"));
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
                yield return TestCase("Sample.eps", new DocumentConverter(Format.Pdf)
                {
                    ColorMode = ColorMode.Rgb,
                }, ColorMode.Rgb);

                yield return TestCase("Sample.eps", new DocumentConverter(Format.Pdf)
                {
                    ColorMode = ColorMode.Cmyk,
                }, ColorMode.Cmyk);

                yield return TestCase("Sample.eps", new DocumentConverter(Format.Pdf)
                {
                    ColorMode = ColorMode.Grayscale,
                }, ColorMode.Grayscale);
            }
        }

        #endregion
    }
}
