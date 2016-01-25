/* ------------------------------------------------------------------------- */
///
/// DocumentWriterTest.cs
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
using NUnit.Framework;
using IoEx = System.IO;

namespace Cube.Pdf.Tests.Editing
{
    /* --------------------------------------------------------------------- */
    ///
    /// DocumentWriterTest
    /// 
    /// <summary>
    /// DocumentWriter のテストを行うクラスです。
    /// </summary>
    /// 
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class DocumentWriterTest : DocumentResource
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Save_SameContents
        ///
        /// <summary>
        /// PDF の内容を変更せずに別のファイルに保存するテストです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("bookmark.pdf", "")]
        [TestCase("password.pdf", "password")]
        public void Save_SameContents(string filename, string password)
        {
            var output = string.Format("Save_SameContents_{0}", filename);
            var dest = IoEx.Path.Combine(Results, output);

            using (var writer = new Cube.Pdf.Editing.DocumentWriter())
            {
                var reader = Create(filename, password);
                writer.Metadata = reader.Metadata;
                writer.Encryption = reader.Encryption;
                writer.UseSmartCopy = true;
                foreach (var page in reader.Pages) writer.Pages.Add(page);
                writer.Save(dest);
            }

            Assert.That(IoEx.File.Exists(dest), Is.True);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Save_FileAndImage
        ///
        /// <summary>
        /// PDF ファイルの 1 ページ目と画像をページ結合して保存するテストを
        /// 行います。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("bookmark.pdf", "cubepdf.png")]
        public void Save_FileAndImage(string file, string image)
        {
            var output = string.Format("Save_FileAndImage_{0}_{1}.pdf",
                IoEx.Path.GetFileNameWithoutExtension(file),
                IoEx.Path.GetFileNameWithoutExtension(image));
            var dest = IoEx.Path.Combine(Results, output);

            using (var writer = new Cube.Pdf.Editing.DocumentWriter())
            {
                var reader = Create(file);
                writer.Metadata = reader.Metadata;
                writer.Encryption = reader.Encryption;
                writer.UseSmartCopy = true;
                writer.Pages.Add(reader.GetPage(1));
                writer.Pages.Add(ImagePage.Create(IoEx.Path.Combine(Examples, image), 0));
                writer.Save(dest);
            }

            Assert.That(IoEx.File.Exists(dest), Is.True);
        }
    }
}
