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
using iTextSharp.text.exceptions;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;

namespace Cube.Pdf.Itext
{
    /* --------------------------------------------------------------------- */
    ///
    /// DocumentWriter
    ///
    /// <summary>
    /// Provides functionality to create or modify a PDF document.
    /// </summary>
    ///
    /// <remarks>
    /// DocumentWriter はページ回転情報 (Page.Rotation.Delta) を
    /// DocumentReader の内部オブジェクトを変更する事によって実現します。
    /// しかし、OpenOption.ReduceMemory が有効な状態で DocumentReader を
    /// 生成している場合、この変更が無効化されるためページ回転の変更結果を反映する
    /// 事ができません。ページを回転させた場合は、該当オプションを無効に設定して
    /// 下さい。
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public class DocumentWriter : DocumentWriterBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// DocumentWriter
        ///
        /// <summary>
        /// Initializes a new instance of the DocumentWriter class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public DocumentWriter() : this(new IO()) { }

        /* ----------------------------------------------------------------- */
        ///
        /// DocumentWriter
        ///
        /// <summary>
        /// Initializes a new instance of the DocumentWriter class with
        /// the specified arguments..
        /// </summary>
        ///
        /// <param name="io">I/O handler.</param>
        ///
        /* ----------------------------------------------------------------- */
        public DocumentWriter(IO io) : base(io) { }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Bookmarks
        ///
        /// <summary>
        /// Gets the collection of bookmarks.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected IList<Dictionary<string, object>> Bookmarks { get; } =
            new List<Dictionary<string, object>>();

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// OnSave
        ///
        /// <summary>
        /// Executes the save operation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnSave(string path)
        {
            var dir = IO.Get(path).DirectoryName;
            var tmp = IO.Combine(dir, Guid.NewGuid().ToString("D"));

            try
            {
                Merge(tmp);
                Release();
                Finalize(tmp, path);
            }
            catch (BadPasswordException err) { throw new EncryptionException(err.Message, err); }
            finally
            {
                _ = IO.TryDelete(tmp);
                Reset();
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnReset
        ///
        /// <summary>
        /// Executes the reset operation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnReset()
        {
            base.OnReset();
            Bookmarks.Clear();
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Merge
        ///
        /// <summary>
        /// Merges pages and save the document to the specified path.
        /// </summary>
        ///
        /// <remarks>
        /// 注釈等を含めて完全にページ内容をコピーするため、いったん
        /// PdfCopy クラスを用いて全ページを結合します。セキュリティ設定や
        /// 文書プロパティ等の情報は生成された PDF に対して付加します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private void Merge(string dest)
        {
            var kv = WriterFactory.Create(dest, Metadata, UseSmartCopy, IO);

            kv.Key.Open();
            Bookmarks.Clear();

            foreach (var page in Pages) AddPage(page, kv.Value);

            kv.Value.Set(Attachments);
            kv.Key.Close();
            kv.Value.Close();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Finalize
        ///
        /// <summary>
        /// Adds some additional metadata to the merged document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Finalize(string src, string dest)
        {
            using (var reader = ReaderFactory.FromPdf(src))
            using (var writer = WriterFactory.Create(dest, reader, IO))
            {
                writer.Writer.Outlines = Bookmarks;
                writer.Set(Metadata, reader.Info);
                writer.Writer.Set(Encryption);
                if (Metadata.Version.Minor >= 5) writer.SetFullCompression();
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// AddPage
        ///
        /// <summary>
        /// Adds the specified page to the specified writer.
        /// </summary>
        ///
        /// <remarks>
        /// PdfCopy.PageNumber (dest) は、AddPage を実行した段階で値が
        /// 自動的に増加するので注意。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private void AddPage(Page src, PdfCopy dest)
        {
            var reader = GetRawReader(src);
            reader.Rotate(src);
            if (src.File is PdfFile)
            {
                var n = dest.PageNumber; // see remarks
                reader.GetBookmarks(n, n - src.Number, Bookmarks);
            }
            dest.AddPage(dest.GetImportedPage(reader, src.Number));
        }

        #endregion
    }
}
