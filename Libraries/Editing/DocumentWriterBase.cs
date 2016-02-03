/* ------------------------------------------------------------------------- */
///
/// DocumentWriterBase.cs
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
using System.Text.RegularExpressions;
using iTextSharp.text.pdf;
using Cube.Pdf.Editing.Extensions;
using IoEx = System.IO;

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
        #region Constructors and destructors

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
        /// Pages
        /// 
        /// <summary>
        /// PDF ファイルの各ページ情報を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ICollection<Page> Pages { get; } = new List<Page>();

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
        /// Bookmarks
        /// 
        /// <summary>
        /// ブックマーク情報を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected List<Dictionary<string, object>> Bookmarks { get; }
            = new List<Dictionary<string, object>>();

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

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Reset
        /// 
        /// <summary>
        /// 初期状態にリセットします。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Reset()
        {
            OnReset();
        }

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
        public void Save(string path)
        {
            OnSave(path);
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

        #endregion

        #region Virtual methods

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
            if (disposing) Pages.Clear();
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
        protected virtual void OnReset()
        {
            Metadata = new Metadata();
            Encryption = new Encryption();
            Pages.Clear();
            Bookmarks.Clear();
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
        /// CreatePdfReader
        /// 
        /// <summary>
        /// PdfReader オブジェクトを生成します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        protected PdfReader CreatePdfReader(Page src)
        {
            try
            {
                var file = src.File as File;
                if (file == null) return null;

                return file.Password.Length > 0 ?
                       new PdfReader(file.FullName, System.Text.Encoding.UTF8.GetBytes(file.Password)) :
                       new PdfReader(file.FullName);
            }
            catch (Exception /* err */) { return null; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CreatePdfReader
        /// 
        /// <summary>
        /// 画像ファイルから PdfReader オブジェクトを生成します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        protected PdfReader CreatePdfReader(Page src, IoEx.MemoryStream buffer)
        {
            if (src == null) return null;
            if (src.File is File) return CreatePdfReader(src);

            using (var image = Image.FromFile(src.File.FullName))
            {
                var document = new iTextSharp.text.Document();
                var writer = PdfWriter.GetInstance(document, buffer);
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
            }

            return new PdfReader(buffer.ToArray());
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CreatePdfCopy
        /// 
        /// <summary>
        /// PdfCopy オブジェクトを生成します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        protected PdfCopy CreatePdfCopy(iTextSharp.text.Document src, string path)
        {
            try
            {
                var dest = UseSmartCopy ?
                           new PdfSmartCopy(src, new IoEx.FileStream(path, IoEx.FileMode.Create)) :
                           new PdfCopy(src, new IoEx.FileStream(path, IoEx.FileMode.Create));

                dest.PdfVersion = Metadata.Version.Minor.ToString()[0];
                dest.ViewerPreferences = Metadata.ViewPreferences;

                return dest;
            }
            catch (Exception /* err */) { return null; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CreateImage
        /// 
        /// <summary>
        /// イメージオブジェクトを生成します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        protected iTextSharp.text.Image CreateImage(Image src, Page page)
        {
            var size  = page.ViewSize(Dpi);
            var scale = GetScale(src, size);
            var pos   = GetPosition(src, scale, size);

            var dest = iTextSharp.text.Image.GetInstance(src, GetFormat(src));
            dest.SetAbsolutePosition(pos.X, pos.Y);
            dest.ScalePercent((float)(scale * 100.0));

            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// AddMetadata
        /// 
        /// <summary>
        /// タイトル、著者名等の各種メタデータを追加します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected void AddMetadata(iTextSharp.text.Document document)
        {
            document.AddTitle(Metadata.Title);
            document.AddSubject(Metadata.Subtitle);
            document.AddKeywords(Metadata.Keywords);
            document.AddCreator(Metadata.Creator);
            document.AddAuthor(Metadata.Author);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// AddMetadata
        /// 
        /// <summary>
        /// タイトル、著者名等のメタ情報を追加します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected void AddMetadata(PdfReader reader, PdfStamper dest)
        {
            var info = reader.Info;

            info.Add("Title",    Metadata.Title);
            info.Add("Subject",  Metadata.Subtitle);
            info.Add("Keywords", Metadata.Keywords);
            info.Add("Creator",  Metadata.Creator);
            info.Add("Author",   Metadata.Author);

            dest.MoreInfo = info;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// AddEncryption
        /// 
        /// <summary>
        /// 暗号化に関する情報を付加します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected void AddEncryption(PdfWriter dest)
        {
            if (Encryption.IsEnabled && Encryption.OwnerPassword.Length > 0)
            {
                var method   = Transform.ToIText(Encryption.Method);
                var flags    = Transform.ToIText(Encryption.Permission);
                var password = string.IsNullOrEmpty(Encryption.UserPassword) ?
                               Encryption.OwnerPassword :
                               Encryption.UserPassword;
                if (!Encryption.IsUserPasswordEnabled) password = string.Empty;

                dest.SetEncryption(method, password, Encryption.OwnerPassword, flags);
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
                    Bookmarks.Add(bm);
                }
            }
        }

        #endregion

        #region Others

        /* ----------------------------------------------------------------- */
        ///
        /// GetScale
        /// 
        /// <summary>
        /// イメージの縮小倍率を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private double GetScale(Image image, Size size)
        {
            var x = size.Width / (double)image.Width;
            var y = size.Height / (double)image.Height;
            return Math.Min(Math.Min(x, y), 1.0);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetPosition
        /// 
        /// <summary>
        /// イメージの表示位置を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private Point GetPosition(Image image, double scale, Size size)
        {
            var x = (size.Width - image.Width * scale) / 2.0;
            var y = (size.Height - image.Height * scale) / 2.0;
            return new Point((int)x, (int)y);
        }

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
            var supports = new List<ImageFormat>()
            {
                ImageFormat.Bmp,
                ImageFormat.Gif,
                ImageFormat.Jpeg,
                ImageFormat.Png,
                ImageFormat.Tiff
            };

            var dest = image.GuessImageFormat();
            return supports.Contains(dest) ? dest : ImageFormat.Png;
        }

        #endregion

        #region Fields
        private bool _disposed = false;
        #endregion
    }
}
