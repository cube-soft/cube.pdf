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
using iTextSharp.text.pdf;
using iTextSharp.text.exceptions;
using Cube.Log;
using Cube.Pdf.Editing.ITextReader;
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
                Merge(tmp);
                using (var reader = new PdfReader(tmp))
                using (var stamper = new PdfStamper(reader, new IoEx.FileStream(path, IoEx.FileMode.Create)))
                {
                    SetMetadata(reader, stamper);
                    SetEncryption(stamper.Writer);
                    if (Metadata.Version.Minor >= 5) stamper.SetFullCompression();
                    SetBookmarks(stamper.Writer);
                }
            }
            catch (BadPasswordException err) { throw new EncryptionException(err.Message, err); }
            finally { TryDelete(tmp); }
        }

        #endregion

        #region Others

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
            if (IoEx.File.Exists(dest)) IoEx.File.Delete(dest);

            var cache  = new Dictionary<string, PdfReader>();

            try
            {
                var document = new iTextSharp.text.Document();
                var writer = GetRawWriter(document, dest);

                document.Open();
                ResetBookmarks();

                foreach (var page in Pages)
                {
                    if (page.File is PdfFile) AddPage(page, writer, cache);
                    else if (page.File is ImageFile) AddImagePage(page, writer);
                }

                SetAttachments(writer);

                document.Close();
                writer.Close();
            }
            finally
            {
                foreach (var reader in cache.Values) reader.Close();
                cache.Clear();
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
        private void AddPage(Page src, PdfCopy dest, Dictionary<string, PdfReader> cache)
        {
            if (!cache.ContainsKey(src.File.FullName))
            {
                var created = GetRawReader(src.File);
                if (created == null) return;
                cache.Add(src.File.FullName, created);
            }

            var reader = cache[src.File.FullName];
            reader.Rotate(src);

            var pagenum = dest.PageNumber; // see remarks
            StockBookmarks(reader, src.Number, pagenum);
            dest.AddPage(dest.GetImportedPage(reader, src.Number));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// AddImagePage
        /// 
        /// <summary>
        /// 画像ファイルを PDF ページとして追加します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void AddImagePage(Page src, PdfCopy dest)
        {
            using (var buffer = new IoEx.MemoryStream())
            using (var reader = GetRawReader(src, buffer))
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
        /// SetAttachments
        /// 
        /// <summary>
        /// 添付ファイルを追加します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SetAttachments(PdfCopy dest)
        {
            var done  = new List<string>();
            
            foreach (var item in Attachments)
            {
                var hash = item.Name.ToLower() + BitConverter.ToString(item.Checksum);
                if (done.Contains(hash)) continue;
                
                dest.AddFileAttachment(null, item.Data, null, item.Name);
                done.Add(hash);
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
            catch (Exception err)
            {
                this.LogError(err.Message, err);
                return false;
            }
        }

        #endregion
    }
}
