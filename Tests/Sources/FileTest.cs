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
    /// FileTest
    ///
    /// <summary>
    /// File のテスト用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class FileTest : DocumentReaderFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Get
        ///
        /// <summary>
        /// ファイル情報を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public void Get(string klass, string filename, string password, bool fullaccess)
        {
            var src = GetExamplesWith(filename);

            using (var reader = Create(klass, src, password))
            {
                var dest = reader.File as PdfFile;

                Assert.That(dest.Name,         Is.EqualTo(filename));
                Assert.That(dest.FullName,     Is.EqualTo(GetExamplesWith(filename)));
                Assert.That(dest.Password,     Is.EqualTo(password));
                Assert.That(dest.FullAccess,   Is.EqualTo(fullaccess));
                Assert.That(dest.Length,       Is.AtLeast(1));
                Assert.That(dest.Resolution.X, Is.EqualTo(72.0f));
                Assert.That(dest.Resolution.Y, Is.EqualTo(72.0f));
                Assert.That(dest.Count,        Is.AtLeast(1));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Open_BadPassword_Throws
        ///
        /// <summary>
        /// 間違ったパスワードを入力して PDF ファイルを開こうとするテストを
        /// 実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestClasses))]
        public void Open_BadPassword_Throws(string klass) => Assert.That(() =>
            {
                var src      = GetExamplesWith("SampleRc40.pdf");
                var password = "bad-password-string";
                using (var _ = Create(klass, src, password)) { /* Not reached */ }
            },
            Throws.TypeOf<EncryptionException>()
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Open_PasswordCancel_Throws
        ///
        /// <summary>
        /// パスワードの入力をキャンセルした時の挙動を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestClasses))]
        public void Open_PasswordCancel_Throws(string klass) => Assert.That(() =>
            {
                var src   = GetExamplesWith("SampleRc40.pdf");
                var query = new Query<string>(e => e.Cancel = true);
                using (var _ = Create(klass, src, query)) { /* Not reached */ }
            },
            Throws.TypeOf<OperationCanceledException>()
        );

        #endregion

        #region TestCases

        /* ----------------------------------------------------------------- */
        ///
        /// TestClasses
        ///
        /// <summary>
        /// テストクラス一覧を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static IEnumerable<TestCaseData> TestClasses
        {
            get
            {
                foreach (var klass in GetClassIds()) yield return new TestCaseData(klass);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// TestCases
        ///
        /// <summary>
        /// テストケース一覧を取得します。
        /// </summary>
        ///
        /// <remarks>
        /// テストケースは以下の順で指定します。
        /// - IDocumentReader の実装を表す名前
        /// - ファイル名
        /// - パスワード
        /// - フルアクセスな状態で開かれたかどうか
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                foreach (var klass in GetClassIds())
                {
                    yield return new TestCaseData(klass, "SampleRotation.pdf", "",         true );
                    yield return new TestCaseData(klass, "SampleAes128.pdf",   "password", true );
                    yield return new TestCaseData(klass, "SampleAes128.pdf",   "view",     false);
                    yield return new TestCaseData(klass, "SampleAes256.pdf",   "password", true );
                }
            }
        }

        #endregion
    }
}
