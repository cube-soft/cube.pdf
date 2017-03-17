/* ------------------------------------------------------------------------- */
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
    class DocumentWriterTest : FileResource
    {
        /* ----------------------------------------------------------------- */
        ///
        /// SaveAs
        ///
        /// <summary>
        /// PDF を別のファイルに保存するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("bookmark.pdf", "",         90)]
        [TestCase("password.pdf", "password",  0)]
        public void SaveAs(string filename, string password, int rotation)
        {
            var output = string.Format("SaveAs_{0}", filename);
            var dest = System.IO.Path.Combine(Results, output);
            var src  = System.IO.Path.Combine(Examples, filename);

            using (var writer = new Cube.Pdf.Editing.DocumentWriter())
            using (var reader = new Cube.Pdf.Editing.DocumentReader())
            {
                reader.Open(src, password);
                
                writer.Metadata     = reader.Metadata;
                writer.Encryption   = reader.Encryption;
                writer.UseSmartCopy = true;
                foreach (var page in reader.Pages)
                {
                    page.Rotation = rotation;
                    writer.Add(page);
                }
                writer.Save(dest);
            }

            Assert.That(System.IO.File.Exists(dest), Is.True);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Merge
        ///
        /// <summary>
        /// PDF ファイルと画像ファイルのそれぞれ 1 ページ目を結合して
        /// 保存するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("bookmark.pdf", "image.png", 90)]
        public void Merge(string pdf, string image, int rotation)
        {
            var output = string.Format("Merge_{0}_{1}.pdf",
                System.IO.Path.GetFileNameWithoutExtension(pdf),
                System.IO.Path.GetFileNameWithoutExtension(image));
            var dest = System.IO.Path.Combine(Results, output);
            var src  = System.IO.Path.Combine(Examples, pdf);

            using (var writer = new Cube.Pdf.Editing.DocumentWriter())
            using (var reader = new Cube.Pdf.Editing.DocumentReader())
            {
                reader.Open(src);

                var p1 = reader.GetPage(1);
                var p2 = ImagePage.Create(System.IO.Path.Combine(Examples, image), 0);

                p1.Rotation = rotation;
                p2.Rotation = rotation;

                writer.Metadata     = reader.Metadata;
                writer.Encryption   = reader.Encryption;
                writer.UseSmartCopy = true;
                writer.Add(p1);
                writer.Add(p2);
                writer.Save(dest);
            }

            Assert.That(System.IO.File.Exists(dest), Is.True);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Split
        ///
        /// <summary>
        /// PDF ファイルを分割保存するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("bookmark.pdf", "",         ExpectedResult = 9)]
        [TestCase("password.pdf", "password", ExpectedResult = 2)]
        public int Split(string filename, string password)
        {
            var src = System.IO.Path.Combine(Examples, filename);

            using (var writer = new Cube.Pdf.Editing.DocumentSplitter())
            using (var reader = new Cube.Pdf.Editing.DocumentReader())
            {
                reader.Open(src, password);

                writer.Metadata     = reader.Metadata;
                writer.Encryption   = reader.Encryption;
                writer.UseSmartCopy = true;
                writer.Add(reader.Pages);
                writer.Save(Results);

                return writer.Results.Count;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Attach
        ///
        /// <summary>
        /// ファイルを添付するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("rotation.pdf",   "", "image.png",   ExpectedResult = 1)]
        [TestCase("attachment.pdf", "", "image.png",   ExpectedResult = 3)]
        [TestCase("attachment.pdf", "", "cubepdf.png", ExpectedResult = 2)]
        [TestCase("heavy.pdf", "", "image.pdf", ExpectedResult = 13)]
        public int Attach(string pdf, string password, string file)
        {
            var output = string.Format("Attach_{0}_{1}.pdf",
                System.IO.Path.GetFileNameWithoutExtension(pdf),
                System.IO.Path.GetFileNameWithoutExtension(file));
            var dest = System.IO.Path.Combine(Results, output);
            var src  = System.IO.Path.Combine(Examples, pdf);

            using (var writer = new Cube.Pdf.Editing.DocumentWriter())
            using (var reader = new Cube.Pdf.Editing.DocumentReader())
            {
                reader.Open(src, password);

                writer.Metadata = reader.Metadata;
                writer.Encryption = reader.Encryption;
                writer.UseSmartCopy = true;
                writer.Add(reader.Pages);
                writer.Attach(reader.Attachments);
                writer.Attach(new Attachment
                {
                    Name = file,
                    File = new File(System.IO.Path.Combine(Examples, file))
                });
                writer.Save(dest);
            }

            using (var reader = new Cube.Pdf.Editing.DocumentReader())
            {
                reader.Open(dest, password);
                return reader.Attachments.Count();
            }
        }
    }
}
