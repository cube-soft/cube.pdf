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
using Cube.Pdf.Itext;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cube.Pdf.Tests.Itext
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

        #region Open

        /* ----------------------------------------------------------------- */
        ///
        /// Open
        ///
        /// <summary>
        /// PDF ファイルを開くテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("SampleRotation.pdf",       "",         true,  ExpectedResult = 9)]
        [TestCase("SamplePassword.pdf",       "password", true,  ExpectedResult = 2)]
        [TestCase("SamplePassword.pdf",       "view",     false, ExpectedResult = 2)]
        [TestCase("SamplePasswordAes256.pdf", "password", true,  ExpectedResult = 9)]
        public int Open(string filename, string password, bool fullaccess)
        {
            using (var reader = Create(filename, password))
            {
                var dest = reader.File as PdfFile;

                Assert.That(dest.Name,         Is.EqualTo(filename));
                Assert.That(dest.FullName,     Is.EqualTo(GetExamplesWith(filename)));
                Assert.That(dest.Password,     Is.EqualTo(password));
                Assert.That(dest.FullAccess,   Is.EqualTo(fullaccess));
                Assert.That(dest.Length,       Is.AtLeast(1));
                Assert.That(dest.Resolution.X, Is.EqualTo(72.0f));
                Assert.That(dest.Resolution.Y, Is.EqualTo(72.0f));
                Assert.That(dest.Count,        Is.EqualTo(reader.Pages.Count()));

                return dest.Count;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Encryption
        ///
        /// <summary>
        /// Encryption オブジェクトの内容を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(Open_Encryption_TestCases))]
        public void Open_Encryption(string filename, string password, Encryption expected)
        {
            using (var reader = Create(filename, password))
            {
                var dest = reader.Encryption;
                Assert.That(dest.Enabled,          Is.EqualTo(expected.Enabled),          nameof(dest.Enabled));
                Assert.That(dest.OpenWithPassword, Is.EqualTo(expected.OpenWithPassword), nameof(dest.OpenWithPassword));
                Assert.That(dest.Method,           Is.EqualTo(expected.Method),           nameof(dest.Method));
                Assert.That(dest.UserPassword,     Is.EqualTo(expected.UserPassword),     nameof(dest.UserPassword));

                var dpm = dest.Permission;
                var epm = expected.Permission;
                Assert.That(dpm.Accessibility,     Is.EqualTo(epm.Accessibility),     nameof(dpm.Accessibility));
                Assert.That(dpm.Assemble,          Is.EqualTo(epm.Assemble),          nameof(dpm.Assemble));
                Assert.That(dpm.CopyContents,      Is.EqualTo(epm.CopyContents),      nameof(dpm.CopyContents));
                Assert.That(dpm.InputForms,  Is.EqualTo(epm.InputForms),  nameof(dpm.InputForms));
                Assert.That(dpm.ModifyAnnotations, Is.EqualTo(epm.ModifyAnnotations), nameof(dpm.ModifyAnnotations));
                Assert.That(dpm.ModifyContents,    Is.EqualTo(epm.ModifyContents),    nameof(dpm.ModifyContents));
                Assert.That(dpm.Print,             Is.EqualTo(epm.Print),             nameof(dpm.Print));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Open_Attachments_Count
        ///
        /// <summary>
        /// 添付ファイルの個数を取得するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("SampleRotation.pdf",   ExpectedResult = 0)]
        [TestCase("SampleAttachment.pdf", ExpectedResult = 3)]
        public int Open_Attachments_Count(string filename)
        {
            using (var reader = Create(filename)) return reader.Attachments.Count();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Open_Attachments_Length
        ///
        /// <summary>
        /// 添付ファイルのファイルサイズを取得するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("SampleAttachment.pdf",    "CubePDF.png",         ExpectedResult =   3765L)]
        [TestCase("SampleAttachment.pdf",    "CubeICE.png",         ExpectedResult = 165524L)]
        [TestCase("SampleAttachment.pdf",    "Empty",               ExpectedResult =      0L)]
        [TestCase("SampleAttachmentCjk.pdf", "日本語のサンプル.md", ExpectedResult =  12843L)]
        public long Open_Attachments_Length(string filename, string key)
        {
            using (var reader = Create(filename))
            {
                return reader.Attachments.First(x => x.Name == key).Length;
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
        [Test]
        public void Open_BadPassword_Throws() => Assert.That(() =>
        {
            var filename = "SamplePassword.pdf";
            var password = "bad-password-string";
            using (var _ = Create(filename, password)) { }
        }, Throws.TypeOf<EncryptionException>());

        /* ----------------------------------------------------------------- */
        ///
        /// Open_PasswordCancel_Throws
        ///
        /// <summary>
        /// パスワードの入力をキャンセルした時の挙動を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Open_PasswordCancel_Throws() => Assert.That(() =>
        {
            var src   = GetExamplesWith("SamplePassword.pdf");
            var query = new Query<string>(e => e.Cancel = true);
            using (var reader = new DocumentReader(src, query, IO)) { }
        }, Throws.TypeOf<OperationCanceledException>());

        #endregion

        /* ----------------------------------------------------------------- */
        ///
        /// ExtractImages
        ///
        /// <summary>
        /// ページ内に存在する画像の抽出テストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("SampleImage.pdf", 1, ExpectedResult = 2)]
        [TestCase("SampleImage.pdf", 2, ExpectedResult = 0)]
        public int ExtractImages(string filename, int n)
        {
            using (var reader = Create(filename)) return reader.ExtractImages(n).Count();
        }

        #endregion

        #region TestCases

        /* ----------------------------------------------------------------- */
        ///
        /// Open_Encryption_TestCases
        ///
        /// <summary>
        /// Open_Encryption のテストに必要なテストケースを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static IEnumerable<TestCaseData> Open_Encryption_TestCases
        {
            get
            {
                yield return new TestCaseData("SamplePassword.pdf", "password", new Encryption
                {
                    Method           = EncryptionMethod.Standard128,
                    Enabled          = true,
                    OpenWithPassword = true,
                    UserPassword     = "view",
                    Permission       = new Permission
                    {
                        Accessibility     = PermissionMethod.Deny,
                        Assemble          = PermissionMethod.Allow,
                        CopyContents      = PermissionMethod.Deny,
                        InputForms        = PermissionMethod.Deny,
                        ModifyAnnotations = PermissionMethod.Deny,
                        ModifyContents    = PermissionMethod.Deny,
                        Print             = PermissionMethod.Allow,
                    }
                });

                yield return new TestCaseData("SamplePasswordAes256.pdf", "password", new Encryption
                {
                    Method           = EncryptionMethod.Aes256,
                    Enabled          = true,
                    OpenWithPassword = false, // true
                    UserPassword     = "", // "view"
                    Permission       = new Permission
                    {
                        Accessibility     = PermissionMethod.Allow,
                        Assemble          = PermissionMethod.Allow,
                        CopyContents      = PermissionMethod.Allow,
                        InputForms        = PermissionMethod.Allow,
                        ModifyAnnotations = PermissionMethod.Allow,
                        ModifyContents    = PermissionMethod.Allow,
                        Print             = PermissionMethod.Allow,
                    }
                });
            }
        }

        #endregion

        #region Helper methods

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// DocumentReader オブジェクトを生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private DocumentReader Create(string filename) => Create(filename, string.Empty);

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// DocumentReader オブジェクトを生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private DocumentReader Create(string filename, string password) =>
            new DocumentReader(GetExamplesWith(filename), password, IO);

        #endregion
    }
}
