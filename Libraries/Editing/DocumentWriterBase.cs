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
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using iTextSharp.text.pdf;
using Cube.Log;
using Cube.Pdf.Editing.Images;
using Cube.Pdf.Editing.IText;

namespace Cube.Pdf.Editing
{
    /* --------------------------------------------------------------------- */
    ///
    /// DocumentWriterBase
    /// 
    /// <summary>
    /// DocumentWriter の基底クラスです。
    /// </summary>
    /// 
    /// <remarks>
    /// このクラスのオブジェクトを直接生成する事はできません。
    /// このクラスを継承して OnSave メソッドをオーバーライドし、必要な
    /// 処理を実装して下さい。
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public class DocumentWriterBase : IDocumentWriter
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// DocumentWriterBase
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected DocumentWriterBase() { }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Metadata
        /// 
        /// <summary>
        /// PDF ファイルのメタデータを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Metadata Metadata { get; set; } = new Metadata();

        /* ----------------------------------------------------------------- */
        ///
        /// Encryption
        /// 
        /// <summary>
        /// 暗号化に関する情報をを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Encryption Encryption { get; set; } = new Encryption();

        /* ----------------------------------------------------------------- */
        ///
        /// UseSmartCopy
        /// 
        /// <summary>
        /// ファイルサイズを抑えるための結合方法を使用するかどうかを取得、
        /// または設定します。
        /// </summary>
        /// 
        /// <remarks>
        /// DocumentWriter は通常 iTextSharp の PdfCopy クラスを用いて
        /// 結合を行っていますが、このクラスは複数の PDF ファイルが同じ
        /// フォントを使用していたとしても別々のものとして扱うため、
        /// フォント情報が重複してファイルサイズが増大する場合があります。
        /// 
        /// この問題を解決したものとして PdfSmartCopy クラスが存在すします。
        /// ただし、複雑な注釈が保存されている PDF を結合する際に使用した
        /// 場合、（別々として扱わなければならないはずの）情報が共有されて
        /// しまい、注釈の構造が壊れてしまう問題が確認されているので、
        /// 注意が必要です。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public bool UseSmartCopy { get; set; } = true;

        /* ----------------------------------------------------------------- */
        ///
        /// Dpi
        /// 
        /// <summary>
        /// PDF の DPI 値を取得します。
        /// </summary>
        /// 
        /// <remarks>
        /// iTextSharp では、72dpi を基準に用紙サイズ等の大きさを算出して
        /// いるものと推測されます。これが PDF の規格で決まっているのか、
        /// iTextSharp 独自の処理なのかは要調査。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        protected int Dpi { get { return 72; } }

        /* ----------------------------------------------------------------- */
        ///
        /// Pages
        /// 
        /// <summary>
        /// PDF ファイルの各ページ情報を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected IEnumerable<Page> Pages => _pages;

        /* ----------------------------------------------------------------- */
        ///
        /// Attachments
        /// 
        /// <summary>
        /// 添付ファイル一覧の情報を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected IEnumerable<Attachment> Attachments => _attach;

        /* ----------------------------------------------------------------- */
        ///
        /// Bookmarks
        /// 
        /// <summary>
        /// ブックマーク情報を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected IEnumerable<Dictionary<string, object>> Bookmarks => _bookmarks;

        /* ----------------------------------------------------------------- */
        ///
        /// SupportedImageFormats
        /// 
        /// <summary>
        /// 対応している画像フォーマットの一覧を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static IEnumerable<ImageFormat> SupportedImageFormats
            => new List<ImageFormat>()
        {
            ImageFormat.Bmp,
            ImageFormat.Gif,
            ImageFormat.Jpeg,
            ImageFormat.Png,
            ImageFormat.Tiff
        };

        #endregion

        #region Methods

        #region IDisposable

        /* ----------------------------------------------------------------- */
        ///
        /// ~DocumentWriterBase
        /// 
        /// <summary>
        /// オブジェクトを破棄します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        ~DocumentWriterBase()
        {
            Dispose(false);
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
        public void Dispose()
        {
            Dispose(true);
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
            if (_disposed) return;
            _disposed = true;
            if (disposing)
            {
                _pages.Clear();
                _attach.Clear();
                Release();
            }
        }

        #endregion

        #region IDocumentWriter

        /* ----------------------------------------------------------------- */
        ///
        /// Reset
        /// 
        /// <summary>
        /// 初期状態にリセットします。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Reset() => OnReset();

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        /// 
        /// <summary>
        /// プロパティが現在保持している、メタ情報、暗号化に関する情報、
        /// ページ情報に基づいた PDF ファイルを生成し、指定されたパスに
        /// 保存します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Save(string path) => OnSave(path);

        /* ----------------------------------------------------------------- */
        ///
        /// Add
        /// 
        /// <summary>
        /// ページを追加します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Add(Page page) => _pages.Add(page);

        /* ----------------------------------------------------------------- */
        ///
        /// Add
        /// 
        /// <summary>
        /// ページを追加します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Add(IEnumerable<Page> pages)
        {
            foreach (var page in pages) Add(page);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Attach
        /// 
        /// <summary>
        /// ファイルを添付します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Attach(Attachment data) => _attach.Add(data);

        /* ----------------------------------------------------------------- */
        ///
        /// Attach
        /// 
        /// <summary>
        /// ファイルを添付します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Attach(IEnumerable<Attachment> data)
        {
            foreach (var item in data) Attach(item);
        }


        #endregion

        /* ----------------------------------------------------------------- */
        ///
        /// Bind
        /// 
        /// <summary>
        /// DocumentReader オブジェクトを束縛します。
        /// </summary>
        /// 
        /// <param name="reader">
        /// 束縛する DocumentReader オブジェクト
        /// </param>
        /// 
        /// <remarks>
        /// 束縛された DocumentReader オブジェクトは、DocumentWriter に
        /// よって Dispose されます。
        /// </remarks>
        /// 
        /* ----------------------------------------------------------------- */
        public void Bind(DocumentReader reader)
        {
            var key = reader.File.FullName;
            if (_bounds.ContainsKey(key))
            {
                // same PDF file but other instance has already bound.
                if (_bounds[key] != reader) reader.Dispose();
            }
            else _bounds.Add(key, reader);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Release
        /// 
        /// <summary>
        /// 束縛されている DocumentReader オブジェクトを開放します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        protected void Release()
        {
            foreach (var kv in _bounds) kv.Value?.Dispose();
            _bounds.Clear();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// IsBound
        /// 
        /// <summary>
        /// 指定されたパスを指す DocumentReader オブジェクトが束縛されて
        /// いるかどうかを判別します。
        /// </summary>
        /// 
        /// <param name="path">PDF ファイルのパス</param>
        /// 
        /// <returns>束縛されているかどうかを示す値</returns>
        /// 
        /* ----------------------------------------------------------------- */
        public bool IsBound(string path)
            => _bounds.ContainsKey(path);

        /* ----------------------------------------------------------------- */
        ///
        /// IsBound
        /// 
        /// <summary>
        /// 指定された DocumentReader オブジェクトが束縛されているか
        /// どうかを判別します。
        /// </summary>
        /// 
        /// <param name="reader">DocumentReader オブジェクト</param>
        /// 
        /// <returns>束縛されているかどうかを示す値</returns>
        /// 
        /// <remarks>
        /// このメソッドは、引数に指定されたオブジェクトと完全に等しい
        /// オブジェクトが束縛されているかどうかを判別します。そのため、
        /// このメソッドが false を返した場合でも、同じパスの別の
        /// DocumentReader オブジェクトが束縛されている可能性があります。
        /// </remarks>
        /// 
        /* ----------------------------------------------------------------- */
        public bool IsBound(DocumentReader reader)
            => _bounds.ContainsKey(reader.File.FullName) &&
               _bounds[reader.File.FullName] == reader;

        #endregion

        #region Virtual methods

        /* ----------------------------------------------------------------- */
        ///
        /// OnReset
        /// 
        /// <summary>
        /// 初期状態にリセットします。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnReset()
        {
            Metadata   = new Metadata();
            Encryption = new Encryption();

            _pages.Clear();
            _attach.Clear();
            _bookmarks.Clear();

            Release();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnSave
        /// 
        /// <summary>
        /// 保存処理を実行します。
        /// </summary>
        /// 
        /// <remarks>
        /// DocumentWriterBase を継承したクラスで実装する必要があります。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnSave(string path) { }

        #endregion

        #region Helper methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetRawReader
        /// 
        /// <summary>
        /// PdfReader オブジェクトを取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        protected PdfReader GetRawReader(MediaFile src)
        {
            try
            {
                var file = src as PdfFile;
                if (file == null) return null;

                return file.Password.Length > 0 ?
                       new PdfReader(file.FullName, System.Text.Encoding.UTF8.GetBytes(file.Password)) :
                       new PdfReader(file.FullName);
            }
            catch (Exception err)
            {
                this.LogError(err.Message, err);
                return null;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetRawReader
        /// 
        /// <summary>
        /// 画像ファイルから PdfReader オブジェクトを生成します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        protected PdfReader GetRawReader(Page src)
        {
            if (src == null) return null;
            if (src.File is PdfFile) return GetRawReader(src.File);

            using (var ms = new System.IO.MemoryStream())
            using (var image = Image.FromFile(src.File.FullName))
            {
                var document = new iTextSharp.text.Document();
                var writer = PdfWriter.GetInstance(document, ms);
                document.Open();

                var guid = image.FrameDimensionsList[0];
                var dimension = new FrameDimension(guid);
                for (var i = 0; i < image.GetFrameCount(dimension); ++i)
                {
                    var size = src.ViewSize(Dpi);

                    image.SelectActiveFrame(dimension, i);
                    image.Rotate(src.Rotation);

                    document.SetPageSize(new iTextSharp.text.Rectangle(size.Width, size.Height));
                    document.NewPage();
                    document.Add(GetRawImage(image, src));
                }

                document.Close();
                writer.Close();

                return new PdfReader(ms.ToArray());
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetRawWriter
        /// 
        /// <summary>
        /// PdfCopy オブジェクトを取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        protected PdfCopy GetRawWriter(iTextSharp.text.Document src, string path)
        {
            try
            {
                var dest = UseSmartCopy ?
                           new PdfSmartCopy(src, System.IO.File.Create(path)) :
                           new PdfCopy(src, System.IO.File.Create(path));

                dest.PdfVersion = Metadata.Version.Minor.ToString()[0];
                dest.ViewerPreferences = Metadata.ViewPreferences;

                return dest;
            }
            catch (Exception err)
            {
                this.LogError(err.Message, err);
                return null;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetRawImage
        /// 
        /// <summary>
        /// イメージオブジェクトを取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        protected iTextSharp.text.Image GetRawImage(Image src, Page page)
        {
            var size  = page.ViewSize(Dpi);
            var scale = src.GetScale(size);
            var pos   = src.GetCenterPosition(size, scale);

            var dest = iTextSharp.text.Image.GetInstance(src, GetFormat(src));
            dest.SetAbsolutePosition(pos.X, pos.Y);
            dest.ScalePercent((float)(scale * 100.0));

            return dest;
        }

        #region Bookmarks

        /* ----------------------------------------------------------------- */
        ///
        /// SetBookmarks
        /// 
        /// <summary>
        /// しおり情報を PdfWriter オブジェクトに設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected void SetBookmarks(PdfWriter dest)
            => dest.Outlines = _bookmarks;

        /* ----------------------------------------------------------------- */
        ///
        /// ResetBookmarks
        /// 
        /// <summary>
        /// しおり情報をリセットします。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected void ResetBookmarks()
            => _bookmarks.Clear();

        /* ----------------------------------------------------------------- */
        ///
        /// StockBookmarks
        /// 
        /// <summary>
        /// PDF ファイルに存在するしおり情報を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected void StockBookmarks(PdfReader src, int srcPageNumber, int destPageNumber)
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

        #endregion

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetFormat
        /// 
        /// <summary>
        /// イメージフォーマットを取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private ImageFormat GetFormat(Image image)
        {
            var format = image.GuessImageFormat();
            return SupportedImageFormats.Contains(format) ? format : ImageFormat.Png;
        }

        #region Fields
        private bool _disposed = false;
        private List<Page> _pages = new List<Page>();
        private List<Attachment> _attach = new List<Attachment>();
        private List<Dictionary<string, object>> _bookmarks = new List<Dictionary<string, object>>();
        private IDictionary<string, DocumentReader> _bounds = new Dictionary<string, DocumentReader>();
        #endregion

        #endregion
    }
}
