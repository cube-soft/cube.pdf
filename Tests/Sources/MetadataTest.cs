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
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Cube.Pdf.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// MetadataTest
    ///
    /// <summary>
    /// Metadata のテスト用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class MetadataTest : DocumentReaderFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Get
        ///
        /// <summary>
        /// PDF ファイルのメタ情報を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public void Get(string klass, string filename, Metadata expected)
        {
            var src = GetExamplesWith(filename);

            using (var reader = Create(klass, src, ""))
            {
                var dest = reader.Metadata;

                Assert.That(dest.Version.Major, Is.EqualTo(expected.Version.Major));
                Assert.That(dest.Version.Minor, Is.EqualTo(expected.Version.Minor));
                Assert.That(dest.Title,         Is.EqualTo(expected.Title));
                Assert.That(dest.Author,        Is.EqualTo(expected.Author));
                Assert.That(dest.Subject,       Is.EqualTo(expected.Subject));
                Assert.That(dest.Keywords,      Is.EqualTo(expected.Keywords));
                Assert.That(dest.Creator,       Is.EqualTo(expected.Creator));
                Assert.That(dest.Producer,      Does.StartWith(expected.Producer));
                // Assert.That(dest.ViewOption,    Is.EqualTo(expected.ViewOption));
            }
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
                foreach (var klass in GetClassIds())
                {
                    yield return new TestCaseData(klass, "SampleRotation.pdf", new Metadata
                    {
                        Version    = new Version(1, 7, 0, 0),
                        Title      = "テスト用文書",
                        Author     = "株式会社キューブ・ソフト",
                        Subject    = "Cube.Pdf.Tests",
                        Keywords   = "CubeSoft,PDF,Test",
                        Creator    = "CubePDF",
                        Producer   = "iTextSharp",
                        ViewOption = ViewOption.TwoPageLeft,
                    });
                }
            }
        }

        #endregion
    }
}
