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
