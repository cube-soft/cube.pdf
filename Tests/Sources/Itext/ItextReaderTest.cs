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
using Cube.FileSystem.TestService;
using Cube.Pdf.Itext;
using NUnit.Framework;
using System.Linq;

namespace Cube.Pdf.Tests.Itext
{
    /* --------------------------------------------------------------------- */
    ///
    /// ItextReaderTest
    ///
    /// <summary>
    /// DocumentReader のテスト用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class ItextReaderTest : FileFixture
    {
        #region Tests

        #region Open

        /* ----------------------------------------------------------------- */
        ///
        /// Open_Attachments_Count
        ///
        /// <summary>
        /// 添付ファイルの個数を取得するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("SampleRotation.pdf",        ExpectedResult = 0)]
        [TestCase("SampleAttachment.pdf",      ExpectedResult = 2)]
        [TestCase("SampleAttachmentEmpty.pdf", ExpectedResult = 3)]
        public int Open_Attachments_Count(string filename)
        {
            using (var reader = Create(filename))
            {
                foreach (var obj in reader.Attachments)
                {
                    Assert.That(obj.Data,            Is.Not.Null);
                    Assert.That(obj.Checksum.Length, Is.EqualTo(32));
                }
                return reader.Attachments.Count();
            }
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
        [TestCase("SampleAttachment.pdf",      "CubePDF.png",         ExpectedResult =   3765L)]
        [TestCase("SampleAttachment.pdf",      "CubeICE.png",         ExpectedResult = 165524L)]
        [TestCase("SampleAttachmentEmpty.pdf", "Empty",               ExpectedResult =      0L)]
        [TestCase("SampleAttachmentCjk.pdf",   "日本語のサンプル.md", ExpectedResult =  12843L)]
        public long Open_Attachments_Length(string filename, string key)
        {
            using (var reader = Create(filename))
            {
                var dest = reader.Attachments.First(x => x.Name == key);
                Assert.That(dest.Data,            Is.Not.Null);
                Assert.That(dest.Checksum.Length, Is.EqualTo(32));
                return dest.Length;
            }
        }

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
            using (var reader = Create(filename)) return reader.GetEmbeddedImages(n).Count();
        }

        #endregion

        #region Others

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
            new DocumentReader(GetExamplesWith(filename), password, true, IO);

        #endregion
    }
}
