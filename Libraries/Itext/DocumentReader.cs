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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

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
    public class DocumentReader : IDocumentReader
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
            this(src, password, false, io) { }

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
        public DocumentReader(string src, IQuery<string> query, IO io) :
            this(src, query, false, io) { }

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
        /// <param name="denyUserPassword">
        /// ユーザパスワードの入力を拒否するかどうか
        /// </param>
        /// <param name="io">I/O オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public DocumentReader(string src, string password, bool denyUserPassword, IO io) :
            this(src, new OnceQuery(password), denyUserPassword, io) { }

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
        /// <param name="denyUserPassword">
        /// ユーザパスワードの入力を拒否するかどうか
        /// </param>
        /// <param name="io">I/O オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public DocumentReader(string src, IQuery<string> query, bool denyUserPassword, IO io)
        {
            _dispose = new OnceAction<bool>(Dispose);
            _core    = ReaderFactory.Create(src, query, denyUserPassword, out string password);

            Debug.Assert(_core != null);

            var f = new PdfFile(src, password, io.GetRefreshable())
            {
                FullAccess = _core.IsOpenedWithFullPermissions,
                Count      = _core.NumberOfPages
            };

            IO          = io;
            File        = f;
            Metadata    = _core.GetMetadata();
            Encryption  = _core.GetEncryption(f);
            Pages       = new ReadOnlyPageCollection(_core, f);
            Attachments = new ReadOnlyAttachmentCollection(_core, f, IO);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// IO
        ///
        /// <summary>
        /// I/O オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IO IO { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// File
        ///
        /// <summary>
        /// ファイル情報を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public File File { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Metadata
        ///
        /// <summary>
        /// PDF ファイルに関するメタ情報を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Metadata Metadata { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Encryption
        ///
        /// <summary>
        /// PDF ファイルに関する暗号化情報を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Encryption Encryption { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Pages
        ///
        /// <summary>
        /// PDF ファイルのページ一覧を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<Page> Pages { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Attachments
        ///
        /// <summary>
        /// 添付ファイルの一覧を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<Attachment> Attachments { get; }

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
        /// GetPage
        ///
        /// <summary>
        /// 指定されたページ番号に対応するページ情報を取得します。
        /// </summary>
        ///
        /// <param name="pagenum">ページ番号</param>
        ///
        /// <returns>Page オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public Page GetPage(int pagenum) => _core.GetPage(GetPdfFile(), pagenum);

        /* ----------------------------------------------------------------- */
        ///
        /// GetImages
        ///
        /// <summary>
        /// 指定されたページ中に存在する画像を取得します。
        /// </summary>
        ///
        /// <param name="pagenum">ページ番号</param>
        ///
        /// <returns>抽出された Image オブジェクトのリスト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<Image> GetImages(int pagenum)
        {
            if (pagenum < 0 || pagenum > Pages.Count()) throw new IndexOutOfRangeException();
            var dest = new EmbeddedImageCollection();
            _core.GetContentParser().ProcessContent(pagenum, dest);
            return dest;
        }

        #region IDisposable

        /* ----------------------------------------------------------------- */
        ///
        /// ~DocumentReader
        ///
        /// <summary>
        /// オブジェクトを破棄します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        ~DocumentReader() { _dispose.Invoke(false); }

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// オブジェクトを破棄する際に必要な終了処理を実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Dispose()
        {
            _dispose.Invoke(true);
            GC.SuppressFinalize(this);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// オブジェクトを破棄する際に必要な終了処理を実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void Dispose(bool disposing)
        {
            if (disposing) _core.Dispose();
        }

        #endregion

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetPdfFile
        ///
        /// <summary>
        /// PdfFile オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private PdfFile GetPdfFile()
        {
            var dest = File as PdfFile;
            Debug.Assert(dest != null);
            return dest;
        }

        #endregion

        #region Fields
        private readonly OnceAction<bool> _dispose;
        private readonly PdfReader _core;
        #endregion
    }
}
