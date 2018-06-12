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
using Cube.FileSystem.Tests;
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
    class ConverterTest : FileFixture
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
        public void Invoke(int index, string filename, Converter conv)
        {
            var lib  = IO.Get(AssemblyReader.Default.Location).DirectoryName;
            var name = $"{nameof(Invoke)}_{index}";
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
        /// <remarks>
        /// Paper の設定は入力ファイルによっては反映されない場合がある。
        /// 例えば、SampleCjk.ps を変換すると Paper の設定に関わらず常に
        /// A4 サイズで変換される。原因を要調査。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                var n = 0;

                yield return new TestCaseData(n++, "Sample.ps", new Converter(Format.Pdf)
                {
                    Paper = Paper.B4,
                });

                yield return new TestCaseData(n++, "Sample.ps", new Converter(Format.Pdf)
                {
                    Orientation = Orientation.Portrait,
                });

                yield return new TestCaseData(n++, "Sample.ps", new Converter(Format.Pdf)
                {
                    Orientation = Orientation.PortraitReverse,
                });

                yield return new TestCaseData(n++, "Sample.ps", new Converter(Format.Pdf)
                {
                    Orientation = Orientation.Landscape,
                });

                yield return new TestCaseData(n++, "Sample.ps", new Converter(Format.Pdf)
                {
                    Orientation = Orientation.LandscapeReverse,
                });
            }
        }

        #endregion
    }
}
