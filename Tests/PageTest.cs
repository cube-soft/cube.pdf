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
using Cube.Pdf.Mixin;
using NUnit.Framework;
using System.Collections.Generic;

namespace Cube.Pdf.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// PageTest
    ///
    /// <summary>
    /// Page のテスト用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class PageTest : DocumentReaderFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// GetPage
        ///
        /// <summary>
        /// 各ページの情報を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public void GetPage(string klass, string filename, int n, float w, float h, int degree)
        {
            var src = GetExamplesWith(filename);

            using (var reader = Create(klass, src, ""))
            {
                var dest = reader.GetPage(n);

                Assert.That(dest.Resolution.X, Is.EqualTo(72.0f));
                Assert.That(dest.Resolution.Y, Is.EqualTo(72.0f));
                Assert.That(dest.Size.Width,   Is.EqualTo(w));
                Assert.That(dest.Size.Height,  Is.EqualTo(h));
                Assert.That(dest.Rotation,     Is.EqualTo(degree));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Rotation
        ///
        /// <summary>
        /// 回転角度の正規化テストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(   0, ExpectedResult =   0)]
        [TestCase( 359, ExpectedResult = 359)]
        [TestCase( 360, ExpectedResult =   0)]
        [TestCase(1000, ExpectedResult = 280)]
        [TestCase(  -1, ExpectedResult = 359)]
        [TestCase(-900, ExpectedResult = 180)]
        public int Rotation(int degree) => new Page { Rotation = degree }.Rotation;

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
                foreach (var klass in GetClassIds())
                {
                    yield return new TestCaseData(klass, "SampleRotation.pdf", 1, 595.0f, 842.0f,   0);
                    yield return new TestCaseData(klass, "SampleRotation.pdf", 2, 595.0f, 842.0f,  90);
                    yield return new TestCaseData(klass, "SampleRotation.pdf", 3, 595.0f, 842.0f, 180);
                    yield return new TestCaseData(klass, "SampleRotation.pdf", 4, 595.0f, 842.0f, 270);
                    yield return new TestCaseData(klass, "SampleRotation.pdf", 5, 595.0f, 842.0f,   0);
                }
            }
        }

        #endregion
    }
}
