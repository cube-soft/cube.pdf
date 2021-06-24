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
using System;
using System.Collections.Generic;
using Cube.Pdf.Mixin;
using NUnit.Framework;

namespace Cube.Pdf.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// FileTest
    ///
    /// <summary>
    /// Tests for File and its inherited classes.
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
        /// Executes the test for getting information of the specified file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public void Get(string klass, string filename, string password, bool fullaccess)
        {
            using (var src = Create(klass, GetSource(filename), password))
            {
                var dest = (PdfFile)src.File;
                Assert.That(dest.Name,         Is.EqualTo(filename));
                Assert.That(dest.FullName,     Is.EqualTo(GetSource(filename)));
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
        /// Get
        ///
        /// <summary>
        /// Executes the test for getting information of the image file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Get_Image()
        {
            var src  = GetSource("SampleImage02.png");
            var dest = IO.GetImageFile(src);

            Assert.That(dest.FullName,     Is.EqualTo(src));
            Assert.That(dest.Length,       Is.EqualTo(3765));
            Assert.That(dest.Resolution.X, Is.AtLeast(72.0f));
            Assert.That(dest.Resolution.Y, Is.AtLeast(96.0f));
            Assert.That(dest.Count,        Is.EqualTo(1));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Open_BadPassword_Throws
        ///
        /// <summary>
        /// Executes the test for confirming the result when the input
        /// password is wrong.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestClasses))]
        public void Open_BadPassword_Throws(string klass)
        {
            var src = GetSource("SampleRc40.pdf");
            var password = "bad-password-string";
            Assert.That(() =>
            {
                using (Create(klass, src, password)) { }
            }, Throws.TypeOf<EncryptionException>());
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Open_PasswordCancel_Throws
        ///
        /// <summary>
        /// Executes the test for confirming the result of canceling
        /// password.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestClasses))]
        public void Open_PasswordCancel_Throws(string klass)
        {
            var src = GetSource("SampleRc40.pdf");
            var query = new Query<string>(e => e.Cancel = true);
            Assert.That(() =>
            {
                using (Create(klass, src, query)) { /* Not reached */ }
            }, Throws.TypeOf<OperationCanceledException>());
        }

        #endregion

        #region TestCases

        /* ----------------------------------------------------------------- */
        ///
        /// TestClasses
        ///
        /// <summary>
        /// Gets classes to execute tests.
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
        /// Gets test cases.
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
                    yield return new TestCaseData(klass, "SampleRc40Open.pdf", "password", true );
                    yield return new TestCaseData(klass, "SampleRc40Open.pdf", "",         false);
                    yield return new TestCaseData(klass, "SampleAes128.pdf",   "password", true );
                    yield return new TestCaseData(klass, "SampleAes128.pdf",   "view",     false);
                    yield return new TestCaseData(klass, "SampleAes256.pdf",   "password", true );
                }
            }
        }

        #endregion
    }
}
