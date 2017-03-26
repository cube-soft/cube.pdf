/* ------------------------------------------------------------------------- */
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
using System.Linq;
using System.Text.RegularExpressions;
using iTextSharp.text.pdf;
using iTextSharp.text.exceptions;
using Cube.Log;
using Cube.Pdf.Editing.IText;

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
            var tmp = System.IO.Path.GetTempFileName();
            TryDelete(tmp);

            try
            {
                Merge(tmp);
                Release();

                using (var reader = new PdfReader(tmp))
                using (var stamper = new PdfStamper(reader, System.IO.File.Create(path)))
                {
                    stamper.Writer.Outlines = _bookmarks;
                    stamper.MoreInfo = reader.Merge(Metadata);
                    stamper.Writer.Set(Encryption);
                    if (Metadata.Version.Minor >= 5) stamper.SetFullCompression();
                }
            }
            catch (BadPasswordException err) { throw new EncryptionException(err.Message, err); }
            finally
            {
                TryDelete(tmp);
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
            _bookmarks.Clear();
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
            var document = new iTextSharp.text.Document();
            var writer = GetRawWriter(document, dest);

            document.Open();
            _bookmarks.Clear();

            foreach (var page in Pages)
            {
                if (page.File is PdfFile) AddPage(page, writer);
                else if (page.File is ImageFile) AddImagePage(page, writer);
            }

            SetAttachments(writer);

            document.Close();
            writer.Close();
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
        /// <remarks>
        /// TODO: Page オブジェクトが引数のはずなのに、全ページ挿入する
        /// 形となっている。要修正。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private void AddImagePage(Page src, PdfCopy dest)
        {
            var reader = GetRawReader(src);
            for (var i = 0; i < reader.NumberOfPages; ++i)
            {
                var page = dest.GetImportedPage(reader, i + 1);
                dest.AddPage(page);
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
            var done = new List<Attachment>();
            
            foreach (var item in Attachments)
            {
                if (done.Any(
                    x => x.Name.ToLower() == item.Name.ToLower() &&
                         x.Length == item.Length &&
                         x.Checksum.SequenceEqual(item.Checksum)
                )) continue;

                var fs = item is EmbeddedAttachment ?
                         PdfFileSpecification.FileEmbedded(dest, null, item.Name, item.Data) :
                         PdfFileSpecification.FileEmbedded(dest, item.File.FullName, item.Name, null);

                fs.SetUnicodeFileName(item.Name, true);
                dest.AddFileAttachment(fs);
                done.Add(item);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// StockBookmarks
        /// 
        /// <summary>
        /// PDF ファイルに存在するしおり情報を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void StockBookmarks(PdfReader src, int srcPageNumber, int destPageNumber)
        {
            var bookmarks = SimpleBookmark.GetBookmark(src);
            if (bookmarks == null) return;

            var pattern = string.Format("^{0} (XYZ|Fit|FitH|FitBH)", destPageNumber);
            SimpleBookmark.ShiftPageNumbers(bookmarks, destPageNumber - srcPageNumber, null);
            foreach (var bm in bookmarks)
            {
                if (bm.ContainsKey("Page") && Regex.IsMatch(bm["Page"].ToString(), pattern))
                {
                    _bookmarks.Add(bm);
                }
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
                System.IO.File.Delete(path);
                return true;
            }
            catch (Exception err)
            {
                this.LogError(err.Message, err);
                return false;
            }
        }

        #region Fields
        private List<Dictionary<string, object>> _bookmarks = new List<Dictionary<string, object>>();
        #endregion

        #endregion
    }
}
