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
using System.Linq;
using NUnit.Framework;
using IoEx = System.IO;

namespace Cube.Pdf.Tests.Editing
{
    /* --------------------------------------------------------------------- */
    ///
    /// DocumentWriterTest
    /// 
    /// <summary>
    /// DocumentWriter および DocumentSplitter のテスト用クラスです。
    /// </summary>
    /// 
    /* --------------------------------------------------------------------- */
    [Parallelizable]
    [TestFixture]
    class DocumentWriterTest : DocumentResource<Cube.Pdf.Editing.DocumentReader>
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Merge_File
        ///
        /// <summary>
        /// PDF を別のファイルに保存するテストです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("bookmark.pdf", "", 90)]
        [TestCase("password.pdf", "password", 0)]
        public void Merge_File(string filename, string password, int rotation)
        {
            var output = string.Format("Merge_File_{0}", filename);
            var dest = IoEx.Path.Combine(Results, output);

            using (var writer = new Cube.Pdf.Editing.DocumentWriter())
            {
                var reader = Create(filename, password);
                writer.Metadata = reader.Metadata;
                writer.Encryption = reader.Encryption;
                writer.UseSmartCopy = true;
                foreach (var page in reader.Pages)
                {
                    page.Rotation = rotation;
                    writer.Pages.Add(page);
                }
                writer.Save(dest);
            }

            Assert.That(IoEx.File.Exists(dest), Is.True);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Merge_FileAndImage
        ///
        /// <summary>
        /// PDF ファイルと画像ファイルのそれぞれ 1 ページ目を結合して
        /// 保存するテストを行います。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("bookmark.pdf", "image.png", 90)]
        public void Merge_FileAndImage(string file, string image, int rotation)
        {
            var output = string.Format("Merge_FileAndImage_{0}_{1}.pdf",
                IoEx.Path.GetFileNameWithoutExtension(file),
                IoEx.Path.GetFileNameWithoutExtension(image));
            var dest = IoEx.Path.Combine(Results, output);

            using (var writer = new Cube.Pdf.Editing.DocumentWriter())
            {
                var reader = Create(file);
                var p1 = reader.GetPage(1);
                var p2 = ImagePage.Create(IoEx.Path.Combine(Examples, image), 0);

                p1.Rotation = rotation;
                p2.Rotation = rotation;

                writer.Metadata = reader.Metadata;
                writer.Encryption = reader.Encryption;
                writer.UseSmartCopy = true;
                writer.Pages.Add(p1);
                writer.Pages.Add(p2);
                writer.Save(dest);
            }

            Assert.That(IoEx.File.Exists(dest), Is.True);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Split_File
        ///
        /// <summary>
        /// PDF ファイルを分割保存するテストを行います。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("bookmark.pdf", "", 9)]
        [TestCase("password.pdf", "password", 2)]
        public void Split_File(string filename, string password, int expected)
        {
            using (var writer = new Cube.Pdf.Editing.DocumentSplitter())
            {
                var reader = Create(filename, password);
                writer.Metadata = reader.Metadata;
                writer.Encryption = reader.Encryption;
                writer.UseSmartCopy = true;
                foreach (var page in reader.Pages) writer.Pages.Add(page);
                writer.Save(Results);

                Assert.That(writer.Results.Count, Is.EqualTo(expected));
            }
        }

        [TestCase("rotation.pdf", "", "image.png", ExpectedResult = 1)]
        [TestCase("attachment.pdf", "", "image.png", ExpectedResult = 3)]
        public int Add_Attachment(string src, string password, string file)
        {
            var output = string.Format("Attach_{0}_{1}.pdf",
                IoEx.Path.GetFileNameWithoutExtension(src),
                IoEx.Path.GetFileNameWithoutExtension(file));
            var dest = IoEx.Path.Combine(Results, output);

            using (var writer = new Cube.Pdf.Editing.DocumentWriter())
            {
                var reader = Create(src, password);
                writer.Metadata = reader.Metadata;
                writer.Encryption = reader.Encryption;
                writer.UseSmartCopy = true;
                foreach (var page in reader.Pages) writer.Pages.Add(page);
                foreach (var item in reader.Attachments) writer.Attachments.Add(item);
                writer.Attachments.Add(new Attachment
                {
                    Name = file,
                    File = new File(IoEx.Path.Combine(Examples, file))
                });
                writer.Save(dest);
            }

            using (var result = Create(dest, password)) return result.Attachments.Count();
        }

    }
}
