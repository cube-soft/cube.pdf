/* ------------------------------------------------------------------------- */
///
/// DocumentWriter.cs
///
/// Copyright (c) 2010 CubeSoft, Inc.
///
/// This program is free software: you can redistribute it and/or modify
/// it under the terms of the GNU Affero General Public License as published
/// by the Free Software Foundation, either version 3 of the License, or
/// (at your option) any later version.
///
/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
/// GNU Affero General Public License for more details.
///
/// You should have received a copy of the GNU Affero General Public License
/// along with this program.  If not, see <http://www.gnu.org/licenses/>.
///
/* ------------------------------------------------------------------------- */
using System;
using System.Collections.Generic;
using System.Drawing;
using iTextSharp.text.pdf;
using iTextSharp.text.exceptions;
using Cube.Pdf.Editing.Extensions;
using IoEx = System.IO;

namespace Cube.Pdf.Editing
{
    /* --------------------------------------------------------------------- */
    ///
    /// DocumentWriter
    /// 
    /// <summary>
    /// PDF ファイルの生成するためのクラスです。
    /// </summary>
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
        public DocumentWriter() : base() { }

        #endregion

        #region Override methods

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
            var tmp = IoEx.Path.GetTempFileName();

            try
            {
                Bind(tmp);
                using (var reader = new PdfReader(tmp))
                using (var writer = new PdfStamper(reader, new IoEx.FileStream(path, IoEx.FileMode.Create)))
                {
                    AddMetadata(reader, writer);
                    AddEncryption(writer);
                    if (Metadata.Version.Minor >= 5) writer.SetFullCompression();
                    writer.Writer.Outlines = Bookmarks;
                }
            }
            catch (BadPasswordException err) { throw new EncryptionException(err.Message, err); }
            finally { TryDelete(tmp); }
        }

        #endregion

        #region Other private methods

        /* ----------------------------------------------------------------- */
        ///
        /// Bind
        /// 
        /// <summary>
        /// 指定された各ページを結合し、新たな PDF ファイルを生成します。
        /// </summary>
        /// 
        /// <remarks>
        /// 注釈等を含めて完全にページ内容をコピーするためにいったん
        /// PdfCopy クラスを用いて全ページを結合します。
        /// セキュリティ設定や文書プロパティ等の情報は生成された PDF に
        /// 対して付加します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private void Bind(string dest)
        {
            if (IoEx.File.Exists(dest)) IoEx.File.Delete(dest);

            var readers  = new Dictionary<string, PdfReader>();
            var document = new iTextSharp.text.Document();
            var writer   = CreatePdfCopy(document, dest);

            document.Open();
            Bookmarks.Clear();

            foreach (var page in Pages)
            {
                if (page.File is File) AddPage(page, writer, readers);
                else if (page.File is ImageFile) AddImagePage(page, writer);
            }

            document.Close();
            writer.Close();
            foreach (var reader in readers.Values) reader.Close();
            readers.Clear();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// AddPage
        /// 
        /// <summary>
        /// PDF ページを追加します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void AddPage(byte[] src, PdfCopy dest)
        {
            using (var reader = new PdfReader(src))
            {
                for (var i = 0; i < reader.NumberOfPages; ++i)
                {
                    var page = dest.GetImportedPage(reader, i + 1);
                    dest.AddPage(page);
                }
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
        /* ----------------------------------------------------------------- */
        private void AddPage(Page src, PdfCopy dest, Dictionary<string, PdfReader> cache)
        {
            if (src == null) return;

            if (!cache.ContainsKey(src.File.FullName))
            {
                var created = CreatePdfReader(src.File as File);
                if (created == null) return;
                cache.Add(src.File.FullName, created);
            }

            var reader = cache[src.File.FullName];
            var rot    = reader.GetPageRotation(src.Number);
            var dic    = reader.GetPageN(src.Number);
            if (rot != src.Rotation) dic.Put(PdfName.ROTATE, new PdfNumber(src.Rotation));
            
            dest.AddPage(dest.GetImportedPage(reader, src.Number));
            StockBookmarks(reader, src.Number, dest.PageNumber);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// AddImagePage
        /// 
        /// <summary>
        /// 画像ファイルを 1 ページの PDF として追加します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void AddImagePage(Page src, PdfCopy dest)
        {
            if (src == null) return;

            using (var image = new Bitmap(src.File.FullName))
            using (var stream = new IoEx.MemoryStream())
            {
                var document = new iTextSharp.text.Document();
                var writer = PdfWriter.GetInstance(document, stream);
                document.Open();

                var guid = image.FrameDimensionsList[0];
                var dimension = new System.Drawing.Imaging.FrameDimension(guid);
                for (var i = 0; i < image.GetFrameCount(dimension); ++i)
                {
                    var size = src.ViewSize(Dpi);

                    image.SelectActiveFrame(dimension, i);
                    image.Rotate(src.Rotation);

                    document.SetPageSize(new iTextSharp.text.Rectangle(size.Width, size.Height));
                    document.NewPage();
                    document.Add(CreateImage(image, src));
                }

                document.Close();
                writer.Close();

                AddPage(stream.ToArray(), dest);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// TryDelete
        /// 
        /// <summary>
        /// ファイルの削除を試行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private bool TryDelete(string path)
        {
            try
            {
                IoEx.File.Delete(path);
                return true;
            }
            catch (Exception /* err */) { return false; }
        }

        #endregion
    }
}
