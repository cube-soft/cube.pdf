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
using System;
using System.Collections.Generic;
using Cube.FileSystem;

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
    /// DocumentWriter realizes the page rotation information
    /// (Page.Rotation.Delta) by modifying the internal object of
    /// DocumentReader. However, if DocumentReader is generated with
    /// OpenOption.ReduceMemory enabled, this change will be disabled and
    /// the result of the page rotation change cannot be reflected.
    /// If you have rotated the page, please set the corresponding option
    /// to disabled.
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
            catch (Exception err) { throw err.Convert(); }
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

            foreach (var page in Pages)
            {
                var reader = ReaderFactory.From(GetRawReader(page));
                kv.Value.Set(reader, page, Bookmarks);
            }

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
            using var reader = ReaderFactory.FromPdf(src);
            using var writer = WriterFactory.Create(dest, reader, IO);

            writer.Writer.Outlines = Bookmarks;
            writer.Set(Metadata, reader.Info);
            writer.Writer.Set(Encryption);
            if (Metadata.Version.Minor >= 5) writer.SetFullCompression();
        }

        #endregion
    }
}
