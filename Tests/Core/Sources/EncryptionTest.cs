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
using System.Collections.Generic;
using NUnit.Framework;

namespace Cube.Pdf.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// EncryptionTest
    ///
    /// <summary>
    /// Tests for the Encryption class through various IDocumentReader
    /// implementations.
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
        /// Executes the test for getting the encryption settings.
        /// </summary>
        ///
        /// <remarks>
        /// TODO: PDFium は「現在」の許可設定が返される。したがって、
        /// OwnerPassword で PDF を開いた場合、元の許可設定に関わらず
        /// 全て Allow と言う結果となる。元の許可設定を取得する方法を
        /// 要検討。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public void Get(string klass, string filename, string password, Encryption cmp)
        {
            using (var r = Create(klass, GetSource(filename), password))
            {
                var dest = r.Encryption;

                Assert.That(dest.Enabled,       Is.EqualTo(cmp.Enabled),       nameof(dest.Enabled));
                Assert.That(dest.Method,        Is.EqualTo(cmp.Method),        nameof(dest.Method));
                Assert.That(dest.OwnerPassword, Is.EqualTo(cmp.OwnerPassword), nameof(dest.OwnerPassword));

                // TODO: Implementation of PDFium is incomplete.
                // Assert.That(dest.OpenWithPassword, Is.EqualTo(cmp.OpenWithPassword), nameof(dest.OpenWithPassword));
                // Assert.That(dest.UserPassword, Is.EqualTo(cmp.UserPassword), nameof(dest.UserPassword));

                var x = dest.Permission;
                var y = cmp.Permission;

                Assert.That(x.Accessibility,     Is.EqualTo(y.Accessibility),     nameof(x.Accessibility));
                Assert.That(x.CopyContents,      Is.EqualTo(y.CopyContents),      nameof(x.CopyContents));
                Assert.That(x.InputForm,         Is.EqualTo(y.InputForm),         nameof(x.InputForm));
                Assert.That(x.ModifyAnnotations, Is.EqualTo(y.ModifyAnnotations), nameof(x.ModifyAnnotations));
                Assert.That(x.ModifyContents,    Is.EqualTo(y.ModifyContents),    nameof(x.ModifyContents));
                Assert.That(x.Print,             Is.EqualTo(y.Print),             nameof(x.Print));
                Assert.That(x.Value,             Is.EqualTo(y.Value),             nameof(x.Value));
            }
        }

        #endregion

        #region TestCases

        /* ----------------------------------------------------------------- */
        ///
        /// TestCases
        ///
        /// <summary>
        /// Gets test cases.
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
                            Accessibility     = PermissionValue.Allow,
                            CopyContents      = PermissionValue.Allow,
                            InputForm         = PermissionValue.Allow,
                            ModifyAnnotations = PermissionValue.Allow,
                            ModifyContents    = PermissionValue.Allow,
                            Print             = PermissionValue.Allow,
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
                            Accessibility     = PermissionValue.Allow,
                            CopyContents      = PermissionValue.Allow,
                            InputForm         = PermissionValue.Allow,
                            ModifyAnnotations = PermissionValue.Allow,
                            ModifyContents    = PermissionValue.Allow,
                            Print             = PermissionValue.Allow,
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
                            Accessibility     = PermissionValue.Allow,
                            CopyContents      = PermissionValue.Allow,
                            InputForm         = PermissionValue.Allow,
                            ModifyAnnotations = PermissionValue.Allow,
                            ModifyContents    = PermissionValue.Allow,
                            Print             = PermissionValue.Allow,
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
                            Accessibility     = PermissionValue.Deny,
                            CopyContents      = PermissionValue.Deny,
                            InputForm         = PermissionValue.Deny,
                            ModifyAnnotations = PermissionValue.Deny,
                            ModifyContents    = PermissionValue.Deny,
                            Print             = PermissionValue.Deny,
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
                            Accessibility     = PermissionValue.Allow,
                            CopyContents      = PermissionValue.Allow,
                            InputForm         = PermissionValue.Allow,
                            ModifyAnnotations = PermissionValue.Allow,
                            ModifyContents    = PermissionValue.Allow,
                            Print             = PermissionValue.Allow,
                        }
                    });
                }

                yield return new TestCaseData(nameof(Cube.Pdf.Itext), "SampleAes128.pdf", "password", new Encryption
                {
                    Method           = EncryptionMethod.Aes128,
                    Enabled          = true,
                    OwnerPassword    = "password",
                    OpenWithPassword = true,
                    UserPassword     = "view",
                    Permission       = new Permission
                    {
                        Accessibility     = PermissionValue.Deny,
                        CopyContents      = PermissionValue.Deny,
                        InputForm         = PermissionValue.Deny,
                        ModifyAnnotations = PermissionValue.Deny,
                        ModifyContents    = PermissionValue.Deny,
                        Print             = PermissionValue.Deny,
                    }
                });

                yield return new TestCaseData(nameof(Cube.Pdf.Pdfium), "SampleAes256r6.pdf", "password", new Encryption
                {
                    Method           = EncryptionMethod.Aes256r6,
                    Enabled          = true,
                    OwnerPassword    = "password",
                    OpenWithPassword = true,
                    UserPassword     = "view",
                    Permission       = new Permission
                    {
                        Accessibility     = PermissionValue.Allow,
                        CopyContents      = PermissionValue.Allow,
                        InputForm         = PermissionValue.Allow,
                        ModifyAnnotations = PermissionValue.Allow,
                        ModifyContents    = PermissionValue.Allow,
                        Print             = PermissionValue.Allow,
                    }
                });
            }
        }

        #endregion
    }
}
