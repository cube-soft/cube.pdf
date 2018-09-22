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
using System.Collections.Generic;

namespace Cube.Pdf.Itext
{
    /* --------------------------------------------------------------------- */
    ///
    /// DocumentWriter
    ///
    /// <summary>
    /// PDF ファイルを生成するためのクラスです。
    /// </summary>
    ///
    /// <remarks>
    /// DocumentWriter はページ回転情報 (Page.Rotation.Delta) を
    /// DocumentReader の内部オブジェクトを変更する事によって実現します。
    /// しかし、DocumentReader を Partial モードで生成している場合、この
    /// 変更が無効化されるためページ回転の変更結果を反映する事ができません。
    /// ページを回転させた場合は、Partial モードを無効化した DocumentReader
    /// オブジェクトを指定するようにして下さい。
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
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public DocumentWriter() : this(new IO()) { }

        /* ----------------------------------------------------------------- */
        ///
        /// DocumentWriter
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="io">I/O オブジェクト</param>
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
        /// しおり情報を取得します。
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
        /// メンバ変数が保持している、メタデータ、暗号化に関する情報、
        /// 各ページ情報に基づいた PDF ファイルを指定されたパスに保存
        /// します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnSave(string path)
        {
            var tmp = System.IO.Path.GetTempFileName();
            IO.TryDelete(tmp);

            try
            {
                Merge(tmp);
                Release();
                Finalize(tmp, path);
            }
            catch (BadPasswordException err) { throw new EncryptionException(err.Message, err); }
            finally
            {
                IO.TryDelete(tmp);
                Reset();
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnReset
        ///
        /// <summary>
        /// 初期状態にリセットします。
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
        /// ページを結合し、新たな PDF ファイルを生成します。
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
        /// 一時的に生成された PDF ファイルに対して、各種メタ情報を追加
        /// して保存します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Finalize(string src, string dest)
        {
            using (var reader = ReaderFactory.Create(src))
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
        /// PDF ページを追加します。
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
