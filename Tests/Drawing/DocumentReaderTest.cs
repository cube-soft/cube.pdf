/* ------------------------------------------------------------------------- */
///
/// DocumentReaderTest.cs
/// 
/// Copyright (c) 2010 CubeSoft, Inc.
/// 
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
///
///  http://www.apache.org/licenses/LICENSE-2.0
///
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.
///
/* ------------------------------------------------------------------------- */
using System;
using System.Drawing;
using System.Linq;
using NUnit.Framework;
using IoEx = System.IO;

namespace Cube.Pdf.Tests.Drawing
{
    /* --------------------------------------------------------------------- */
    ///
    /// DocumentReaderTest
    /// 
    /// <summary>
    /// DocumentReader のテストを行うクラスです。
    /// </summary>
    /// 
    /* --------------------------------------------------------------------- */
    [Parallelizable]
    [TestFixture]
    class DocumentReaderTest : DocumentResource<Cube.Pdf.Drawing.DocumentReader>
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Open
        ///
        /// <summary>
        /// PDF ファイルを開くテストを行います。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        #region Open

        [TestCase("rotation.pdf", "")]
        [TestCase("password.pdf", "password")]
        [TestCase("password-aes256.pdf", "password")]
        public void Open(string filename, string password)
        {
            Assert.That(Create(filename, password).IsOpen, Is.True);
        }

        [Test]
        public void Open_UserPassword()
        {
            using (var reader = new Cube.Pdf.Drawing.DocumentReader())
            {
                var src = IoEx.Path.Combine(Examples, "password.pdf");
                reader.Open(src, "view");

                Assert.That(reader.IsOpen, Is.True);
                //Assert.That(((File)reader.File).FullAccess, Is.False);
            }
        }

        //[Test]
        public void Open_BadPassword_RaisesEvent()
        {
            using (var reader = new Cube.Pdf.Drawing.DocumentReader())
            {
                var src = IoEx.Path.Combine(Examples, "password.pdf");
                var raiseEvent = false;
                reader.PasswordRequired += (s, e) =>
                {
                    raiseEvent = true;
                    e.Cancel = true;
                };
                reader.Open(src, "bad-password-string");

                Assert.That(raiseEvent, Is.True);
            }
        }

        #endregion

        /* ----------------------------------------------------------------- */
        ///
        /// File
        ///
        /// <summary>
        /// ファイルの情報を取得するテストを行います。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        #region File

        [TestCase("rotation.pdf", "", 9)]
        [TestCase("password.pdf", "password", 2)]
        [TestCase("password-aes256.pdf", "password", 9)]
        public void File_PageCount(string filename, string password, int expected)
        {
            Assert.That(
                Create(filename, password).File.PageCount,
                Is.EqualTo(expected)
            );
        }

        [TestCase("rotation.pdf", "")]
        [TestCase("password.pdf", "password")]
        [TestCase("password-aes256.pdf", "password")]
        public void File_Password(string filename, string password)
        {
            var file = Create(filename, password).File as PdfFile;
            Assert.That(
                file.Password,
                Is.EqualTo(password)
            );
        }

        [TestCase("rotation.pdf", "", 72)]
        [TestCase("password.pdf", "password", 72)]
        [TestCase("password-aes256.pdf", "password", 72)]
        public void File_Resolution(string filename, string password, int expected)
        {
            Assert.That(
                Create(filename, password).File.Resolution,
                Is.EqualTo(new Point(expected, expected))
            );
        }

        #endregion

        /* ----------------------------------------------------------------- */
        ///
        /// Pages
        ///
        /// <summary>
        /// ページ数の情報を取得するテストを行います。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        #region Pages

        [TestCase("rotation.pdf", "", 9)]
        [TestCase("password.pdf", "password", 2)]
        [TestCase("password-aes256.pdf", "password", 9)]
        public void Pages_Count(string filename, string password, int expected)
        {
            Assert.That(
                Create(filename, password).Pages.Count(),
                Is.EqualTo(expected)
            );
        }

        #endregion

        /* ----------------------------------------------------------------- */
        ///
        /// Metadata
        ///
        /// <summary>
        /// メタ情報を取得するテストを行います。
        /// </summary>
        /// 
        /// <remarks>
        /// TODO: 要実装 & 修正
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        #region Metadata

        //[TestCase("rotation.pdf", 7)]
        public void Metadata_Version(string filename, int expected)
        {
            Assert.That(
                Create(filename).Metadata.Version,
                Is.EqualTo(new Version(1, expected, 0, 0))
            );
        }

        //[TestCase("rotation.pdf", ViewLayout.TwoPageLeft)]
        public void Metadata_ViewLayout(string filename, ViewLayout expected)
        {
            Assert.That(
                Create(filename).Metadata.ViewLayout,
                Is.EqualTo(expected)
            );
        }

        //[TestCase("rotation.pdf", ViewMode.None)]
        public void Metadata_ViewMode(string filename, ViewMode expected)
        {
            Assert.That(
                Create(filename).Metadata.ViewMode,
                Is.EqualTo(expected)
            );
        }

        //[TestCase("rotation.pdf", "テスト用文書")]
        public void Metadata_Title(string filename, string expected)
        {
            Assert.That(
                Create(filename).Metadata.Title,
                Is.EqualTo(expected)
            );
        }

        //[TestCase("rotation.pdf", "株式会社キューブ・ソフト")]
        public void Metadata_Author(string filename, string expected)
        {
            Assert.That(
                Create(filename).Metadata.Author,
                Is.EqualTo(expected)
            );
        }

        //[TestCase("rotation.pdf", "Cube.Pdf.Tests")]
        public void Metadata_Subtitle(string filename, string expected)
        {
            Assert.That(
                Create(filename).Metadata.Subtitle,
                Is.EqualTo(expected)
            );
        }

        //[TestCase("rotation.pdf", "CubeSoft,PDF,Test")]
        public void Metadata_Keywords(string filename, string expected)
        {
            Assert.That(
                Create(filename).Metadata.Keywords,
                Is.EqualTo(expected)
            );
        }

        #endregion

        /* ----------------------------------------------------------------- */
        ///
        /// Encryption
        ///
        /// <summary>
        /// 暗号化に関する情報を取得するテストを行います。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        #region Encryption

        //[TestCase("password.pdf", "password", EncryptionMethod.Standard128)]
        //[TestCase("password-aes256.pdf", "password", EncryptionMethod.Aes256)]
        public void Encryption_Method(string filename, string password, EncryptionMethod expected)
        {
            Assert.That(
                Create(filename, password).Encryption.Method,
                Is.EqualTo(expected)
            );
        }

        //[TestCase("password.pdf", "password", "view")]
        //[TestCase("password-aes256.pdf", "password", "" /* "view" */)]
        public void Encryption_UserPassword(string filename, string password, string expected)
        {
            Assert.That(
                Create(filename, password).Encryption.UserPassword,
                Is.EqualTo(expected)
            );
        }

        //[TestCase("password.pdf", "password", PermissionMethod.Deny)]
        public void Encryption_Accessibility(string filename, string password, PermissionMethod expected)
        {
            Assert.That(
                Create(filename, password).Encryption.Permission.Accessibility,
                Is.EqualTo(expected)
            );
        }

        //[TestCase("password.pdf", "password", PermissionMethod.Allow)]
        public void Encryption_Assembly(string filename, string password, PermissionMethod expected)
        {
            Assert.That(
                Create(filename, password).Encryption.Permission.Assembly,
                Is.EqualTo(expected)
            );
        }

        //[TestCase("password.pdf", "password", PermissionMethod.Deny)]
        public void Encryption_CopyContents(string filename, string password, PermissionMethod expected)
        {
            Assert.That(
                Create(filename, password).Encryption.Permission.CopyContents,
                Is.EqualTo(expected)
            );
        }

        //[TestCase("password.pdf", "password", PermissionMethod.Deny)]
        public void Encryption_InputFormFields(string filename, string password, PermissionMethod expected)
        {
            Assert.That(
                Create(filename, password).Encryption.Permission.InputFormFields,
                Is.EqualTo(expected)
            );
        }

        //[TestCase("password.pdf", "password", PermissionMethod.Deny)]
        public void Encryption_ModifyAnnotations(string filename, string password, PermissionMethod expected)
        {
            Assert.That(
                Create(filename, password).Encryption.Permission.ModifyAnnotations,
                Is.EqualTo(expected)
            );
        }

        //[TestCase("password.pdf", "password", PermissionMethod.Deny)]
        public void Encryption_ModifyContents(string filename, string password, PermissionMethod expected)
        {
            Assert.That(
                Create(filename, password).Encryption.Permission.ModifyContents,
                Is.EqualTo(expected)
            );
        }

        //[TestCase("password.pdf", "password", PermissionMethod.Allow)]
        public void Encryption_Printing(string filename, string password, PermissionMethod expected)
        {
            Assert.That(
                Create(filename, password).Encryption.Permission.Printing,
                Is.EqualTo(expected)
            );
        }

        //[TestCase("password.pdf", "password", PermissionMethod.Allow)]
        public void Encryption_Signature(string filename, string password, PermissionMethod expected)
        {
            Assert.That(
                Create(filename, password).Encryption.Permission.Signature,
                Is.EqualTo(expected)
            );
        }

        //[TestCase("password.pdf", "password", PermissionMethod.Allow)]
        public void Encryption_TemplatePage(string filename, string password, PermissionMethod expected)
        {
            Assert.That(
                Create(filename, password).Encryption.Permission.TemplatePage,
                Is.EqualTo(expected)
            );
        }

        #endregion

        /* ----------------------------------------------------------------- */
        ///
        /// GetPage
        ///
        /// <summary>
        /// 各ページの情報を取得するテストを行います。
        /// </summary>
        /// 
        /// <remarks>
        /// TODO: GetPage_Size の結果がおかしいので要修正
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        #region GetPage

        //[TestCase("rotation.pdf", 1, 595, 842)]
        public void GetPage_Size(string filename, int number, int width, int height)
        {
            Assert.That(
                Create(filename).GetPage(number).Size,
                Is.EqualTo(new Size(width, height))
            );
        }

        [TestCase("rotation.pdf", 1, 72)]
        public void GetPage_Resolution(string filename, int number, int expected)
        {
            Assert.That(
                Create(filename).GetPage(number).Resolution,
                Is.EqualTo(new Point(expected, expected))
            );
        }

        [TestCase("rotation.pdf", 1, 0)]
        [TestCase("rotation.pdf", 2, 90)]
        [TestCase("rotation.pdf", 3, 180)]
        [TestCase("rotation.pdf", 4, 270)]
        public void GetPage_Rotation(string filename, int number, int expected)
        {
            Assert.That(
                Create(filename).GetPage(number).Rotation,
                Is.EqualTo(expected)
            );
        }

        #endregion

        /* ----------------------------------------------------------------- */
        ///
        /// CreateImage
        ///
        /// <summary>
        /// Image オブジェクトを生成するテストを行います。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        #region CreateImage

        [TestCase("rotation.pdf", "", 1)]
        [TestCase("rotation.pdf", "", 2)]
        [TestCase("rotation.pdf", "", 3)]
        [TestCase("rotation.pdf", "", 4)]
        public void CreateImage(string filename, string password, int pagenum)
        {
            var power  = 1.0;
            var reader = Create(filename, password);
            var page   = reader.GetPage(pagenum);

            using (var image = reader.CreateImage(pagenum, power))
            {
                var dest = IoEx.Path.Combine(Results, $"CreateImage-{pagenum}.png");
                image.Save(dest);
                Assert.That(IoEx.File.Exists(dest), Is.True);
            }
        }

        #endregion
    }
}
