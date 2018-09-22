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
using Cube.FileSystem;
using iTextSharp.text.pdf;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Cube.Pdf.Itext
{
    /* --------------------------------------------------------------------- */
    ///
    /// DocumentReader
    ///
    /// <summary>
    /// PDF ファイルを読み込んで各種情報を保持するためのクラスです。
    /// </summary>
    ///
    /// <remarks>
    /// iTextSharp を用いて PDF ファイルの解析を行います。
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public class DocumentReader : DocumentReaderBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// DocumentReader
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="src">PDF ファイルのパス</param>
        ///
        /* ----------------------------------------------------------------- */
        public DocumentReader(string src) : this(src, string.Empty) { }

        /* ----------------------------------------------------------------- */
        ///
        /// DocumentReader
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="src">PDF ファイルのパス</param>
        /// <param name="password">パスワード</param>
        ///
        /* ----------------------------------------------------------------- */
        public DocumentReader(string src, string password) :
            this(src, password, new IO()) { }

        /* ----------------------------------------------------------------- */
        ///
        /// DocumentReader
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="src">PDF ファイルのパス</param>
        /// <param name="query">パスワード用オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public DocumentReader(string src, IQuery<string> query) :
            this(src, query, new IO()) { }

        /* ----------------------------------------------------------------- */
        ///
        /// DocumentReader
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="src">PDF ファイルのパス</param>
        /// <param name="password">パスワード</param>
        /// <param name="io">I/O オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public DocumentReader(string src, string password, IO io) :
            this(src, new OnceQuery<string>(password), io) { }

        /* ----------------------------------------------------------------- */
        ///
        /// DocumentReader
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="src">PDF ファイルのパス</param>
        /// <param name="query">パスワード用オブジェクト</param>
        /// <param name="io">I/O オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public DocumentReader(string src, IQuery<string> query, IO io) : base(io)
        {
            Debug.Assert(io != null);
            _core = ReaderFactory.Create(src, query, out string password);
            Debug.Assert(_core != null);

            var f = new PdfFile(src, password, io.GetRefreshable())
            {
                FullAccess = _core.IsOpenedWithFullPermissions,
                Count      = _core.NumberOfPages
            };

            File        = f;
            Metadata    = _core.GetMetadata();
            Encryption  = _core.GetEncryption(f);
            Pages       = new ReadOnlyPageList(_core, f);
            Attachments = new ReadOnlyAttachmentList(_core, f, IO);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// RawObject
        ///
        /// <summary>
        /// 内部実装オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public object RawObject => _core;

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// ExtractImages
        ///
        /// <summary>
        /// 指定されたページ中に存在する画像を抽出します。
        /// </summary>
        ///
        /// <param name="pagenum">ページ番号</param>
        ///
        /// <returns>抽出された Image オブジェクトのリスト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<Image> ExtractImages(int pagenum)
        {
            var dest = new EmbeddedImageCollection();
            _core.GetContentParser().ProcessContent(pagenum, dest);
            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// リソースを開放します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void Dispose(bool disposing)
        {
            if (disposing) _core.Dispose();
        }

        #endregion

        #region Fields
        private readonly PdfReader _core;
        #endregion
    }
}
