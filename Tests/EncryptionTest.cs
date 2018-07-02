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
using System.Collections.Generic;

namespace Cube.Pdf.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// EncryptionTest
    ///
    /// <summary>
    /// Encryption のテスト用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class EncryptionTest : DocumentReaderFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Get
        ///
        /// <summary>
        /// 暗号化情報を確認します。
        /// </summary>
        ///
        /// <remarks>
        /// UserPassword に関する情報の取得が未実装（主に PDFium）。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public void Get(string klass, string filename, string password, Encryption expected)
        {
            var src = GetExamplesWith(filename);

            using (var reader = Create(klass, src, password))
            {
                var dest = reader.Encryption;
                Assert.That(dest.Enabled,          Is.EqualTo(expected.Enabled), nameof(dest.Enabled));
                Assert.That(dest.Method,           Is.EqualTo(expected.Method), nameof(dest.Method));
                Assert.That(dest.OwnerPassword,    Is.EqualTo(expected.OwnerPassword), nameof(dest.OwnerPassword));
                // Assert.That(dest.OpenWithPassword, Is.EqualTo(expected.OpenWithPassword), nameof(dest.OpenWithPassword));
                // Assert.That(dest.UserPassword,     Is.EqualTo(expected.UserPassword), nameof(dest.UserPassword));

                var pm  = dest.Permission;
                var epm = expected.Permission;
                Assert.That(pm.Accessibility,      Is.EqualTo(epm.Accessibility), nameof(pm.Accessibility));
                Assert.That(pm.Assemble,           Is.EqualTo(epm.Assemble), nameof(pm.Assemble));
                Assert.That(pm.CopyContents,       Is.EqualTo(epm.CopyContents), nameof(pm.CopyContents));
                Assert.That(pm.InputForms,         Is.EqualTo(epm.InputForms), nameof(pm.InputForms));
                Assert.That(pm.ModifyAnnotations,  Is.EqualTo(epm.ModifyAnnotations), nameof(pm.ModifyAnnotations));
                Assert.That(pm.ModifyContents,     Is.EqualTo(epm.ModifyContents), nameof(pm.ModifyContents));
                Assert.That(pm.Print,              Is.EqualTo(epm.Print), nameof(pm.Print));
                Assert.That(pm.Value,              Is.EqualTo(epm.Value), nameof(pm.Value));
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
                    yield return new TestCaseData(klass, "Sample.pdf", "", new Encryption
                    {
                        Method           = EncryptionMethod.Unknown,
                        Enabled          = false,
                        OwnerPassword    = string.Empty,
                        OpenWithPassword = false,
                        UserPassword     = string.Empty,
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

                    yield return new TestCaseData(klass, "SampleRc40.pdf", "password", new Encryption
                    {
                        Method           = EncryptionMethod.Standard40,
                        Enabled          = true,
                        OwnerPassword    = "password",
                        OpenWithPassword = true,
                        UserPassword     = "view",
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

                    yield return new TestCaseData(klass, "SampleRc128.pdf", "password", new Encryption
                    {
                        Method           = EncryptionMethod.Standard128,
                        Enabled          = true,
                        OwnerPassword    = "password",
                        OpenWithPassword = true,
                        UserPassword     = "view",
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

                    yield return new TestCaseData(klass, "SampleAes128.pdf", "view", new Encryption
                    {
                        Method           = EncryptionMethod.Aes128,
                        Enabled          = true,
                        OwnerPassword    = "", // "password"
                        OpenWithPassword = true,
                        UserPassword     = "view",
                        Permission       = new Permission
                        {
                            Accessibility     = PermissionMethod.Deny,
                            Assemble          = PermissionMethod.Deny,
                            CopyContents      = PermissionMethod.Deny,
                            InputForms        = PermissionMethod.Deny,
                            ModifyAnnotations = PermissionMethod.Deny,
                            ModifyContents    = PermissionMethod.Deny,
                            Print             = PermissionMethod.Deny,
                        }
                    });

                    yield return new TestCaseData(klass, "SampleAes256.pdf", "password", new Encryption
                    {
                        Method           = EncryptionMethod.Aes256,
                        Enabled          = true,
                        OwnerPassword    = "password",
                        OpenWithPassword = true,
                        UserPassword     = "view",
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

                    // Tests only for PDFium
                    if (klass == nameof(Cube.Pdf.Itext)) continue;

                    yield return new TestCaseData(klass, "SampleAes256r6.pdf", "password", new Encryption
                    {
                        Method           = EncryptionMethod.Aes256r6,
                        Enabled          = true,
                        OwnerPassword    = "password",
                        OpenWithPassword = true,
                        UserPassword     = "view",
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
        }

        #endregion
    }
}
