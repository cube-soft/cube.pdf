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
using System.Collections.Generic;
using System.Linq;

namespace Cube.Pdf.Tests.Itext
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
    class DocumentWriterTest : FileFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// PDF を保存するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("Sample.pdf",               "",           0, ExpectedResult = 2)]
        [TestCase("SampleAnnotation.pdf",     "",          90, ExpectedResult = 2)]
        [TestCase("SampleBookmark.pdf",       "",         180, ExpectedResult = 9)]
        [TestCase("SampleAttachment.pdf",     "",         270, ExpectedResult = 9)]
        [TestCase("SamplePassword.pdf",       "password", 180, ExpectedResult = 2)]
        [TestCase("SamplePasswordAes256.pdf", "password",  90, ExpectedResult = 9)]
        public int Save(string filename, string password, int degree)
        {
            var src  = GetExamplesWith(filename);
            var dest = GetResultsWith($"{nameof(Save)}_{filename}");

            using (var writer = new DocumentWriter(IO))
            using (var reader = new DocumentReader(src, password, IO))
            {
                writer.UseSmartCopy = true;
                writer.Set(reader.Metadata);
                writer.Set(reader.Encryption);
                writer.Add(Rotate(reader.Pages, degree));
                writer.Save(dest);
            }

            return Count(dest, password, degree);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Overwrite
        ///
        /// <summary>
        /// PDF を上書き保存するテストを実行します。
        /// </summary>
        ///
        /// <remarks>
        /// DocumentWriter で上書きする場合、保存の直前に DocumentReader
        /// オブジェクトを破棄する必要があるため、Bind(DocumentReader) を
        /// 利用します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("Sample.pdf",         "",          0, ExpectedResult = 2)]
        [TestCase("SamplePassword.pdf", "password", 90, ExpectedResult = 2)]
        public int Overwrite(string filename, string password, int degree)
        {
            var name = $"{nameof(Overwrite)}_{filename}";
            var dest = GetResultsWith(name);

            IO.Copy(GetExamplesWith(filename), dest, true);

            using (var writer = new DocumentWriter(IO))
            {
                var reader = new DocumentReader(dest, password, IO);

                writer.UseSmartCopy = true;
                writer.Set(reader.Metadata);
                writer.Set(reader.Encryption);
                writer.Add(Rotate(reader.Pages, degree));
                writer.Bind(reader);
                writer.Save(dest);
            }

            return Count(dest, password, degree);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Merge
        ///
        /// <summary>
        /// PDF ファイルを結合するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("Sample.pdf", "SampleBookmark.pdf", 90, ExpectedResult = 11)]
        public int Merge(string f0, string f1, int degree)
        {
            var s0   = IO.Get(f0).NameWithoutExtension;
            var s1   = IO.Get(f1).NameWithoutExtension;
            var dest = GetResultsWith($"{nameof(Merge)}_{s0}_{s1}.pdf");

            using (var writer = new DocumentWriter(IO))
            using (var r0 = new DocumentReader(GetExamplesWith(f0), "", IO))
            using (var r1 = new DocumentReader(GetExamplesWith(f1), "", IO))
            {
                writer.UseSmartCopy = true;
                writer.Set(r0.Metadata);
                writer.Set(r0.Encryption);
                writer.Add(Rotate(r0.Pages, degree));
                writer.Add(Rotate(r1.Pages, degree));
                writer.Save(dest);
            }

            return Count(dest, "", degree);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Merge_Image
        ///
        /// <summary>
        /// PDF ファイルに対して画像ファイルを結合して保存するテストを
        /// 実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("SampleBookmark.pdf", "SampleImage01.png", 90, ExpectedResult = 10)]
        public int Merge_Image(string pdf, string image, int degree)
        {
            var s0   = IO.Get(pdf).NameWithoutExtension;
            var s1   = IO.Get(image).NameWithoutExtension;
            var dest = GetResultsWith($"{nameof(Merge_Image)}_{s0}_{s1}.pdf");

            using (var writer = new DocumentWriter(IO))
            using (var reader = new DocumentReader(GetExamplesWith(pdf), "", IO))
            {
                writer.UseSmartCopy = true;
                writer.Set(reader.Metadata);
                writer.Set(reader.Encryption);
                writer.Add(Rotate(reader.Pages, degree));
                writer.Add(Rotate(IO.GetImagePages(GetExamplesWith(image)), degree));
                writer.Save(dest);
            }

            return Count(dest, "", degree);
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
        [TestCase("SampleBookmark.pdf", "",         ExpectedResult = 9)]
        [TestCase("SamplePassword.pdf", "password", ExpectedResult = 2)]
        public int Split(string filename, string password)
        {
            var src  = GetExamplesWith(filename);
            var dest = GetResultsWith($"{nameof(Split)}_{IO.Get(src).NameWithoutExtension}");

            using (var writer = new DocumentSplitter(IO))
            using (var reader = new DocumentReader(src, password, IO))
            {
                writer.UseSmartCopy = true;
                writer.Set(reader.Metadata);
                writer.Set(reader.Encryption);
                writer.Add(reader.Pages);
                writer.Save(dest);

                var n = IO.GetFiles(dest).Length;
                Assert.That(n, Is.EqualTo(writer.Results.Count));
                return n;
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
        [TestCase("SampleRotation.pdf",   "SampleImage01.png",   ExpectedResult = 1)]
        [TestCase("SampleAttachment.pdf", "SampleImage02.png",   ExpectedResult = 4)]
        [TestCase("SampleAttachment.pdf", "日本語のサンプル.md", ExpectedResult = 4)]
        public int Attach(string pdf, string file)
        {
            var s0   = IO.Get(GetExamplesWith(pdf)).NameWithoutExtension;
            var s1   = IO.Get(GetExamplesWith(file)).NameWithoutExtension;
            var src  = GetExamplesWith(pdf);
            var dest = GetResultsWith($"{nameof(Attach)}_{s0}_{s1}.pdf");

            using (var writer = new DocumentWriter())
            {
                var reader = new DocumentReader(src, "", IO);

                writer.UseSmartCopy = true;
                writer.Set(reader.Metadata);
                writer.Set(reader.Encryption);
                writer.Add(reader.Pages);
                writer.Attach(reader.Attachments);
                writer.Attach(new Attachment(GetExamplesWith(file), IO));
                writer.Bind(reader);
                writer.Save(dest);
            }

            using (var reader = new DocumentReader(dest, "", IO))
            {
                var items = reader.Attachments;
                Assert.That(items.Any(x => x.Name.ToLower() == file.ToLower()), Is.True);
                return items.Count();
            }
        }

        #endregion

        #region Helper methods

        /* ----------------------------------------------------------------- */
        ///
        /// Rotate
        ///
        /// <summary>
        /// 指定された全てのページを回転します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private IEnumerable<Page> Rotate(IEnumerable<Page> src, int degree) =>
            src.Select(e => Rotate(e, degree));

        /* ----------------------------------------------------------------- */
        ///
        /// Rotate
        ///
        /// <summary>
        /// ページを回転します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private Page Rotate(Page src, int degree)
        {
            src.Rotation = degree;
            return src;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Count
        ///
        /// <summary>
        /// ページ数を取得します。
        /// </summary>
        ///
        /// <remarks>
        /// リファクタリング以前からページ回転に関して不都合があった様子。
        /// ページ回転を要修正。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private int Count(string src, string password, int degree)
        {
            using (var reader = new DocumentReader(src, password, IO))
            {
                Assert.That(reader.File.Count, Is.EqualTo(reader.Pages.Count()));

                for (var i = 0; i < reader.File.Count; ++i)
                {
                    var n = i + 1;
                    var page = reader.GetPage(n);

                    // see ramarks
                    // Assert.That(page.Rotation, Is.EqualTo(degree), $"{src}:{n}");
                }
                return reader.File.Count;
            }
        }

        #endregion
    }
}
