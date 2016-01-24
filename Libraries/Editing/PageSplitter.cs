/* ------------------------------------------------------------------------- */
///
/// PageSplitter.cs
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
using Cube.Pdf.Editing.Extensions;
using IoEx = System.IO;

namespace Cube.Pdf.Editing
{
    /* --------------------------------------------------------------------- */
    ///
    /// PageSplitter
    /// 
    /// <summary>
    /// PDF ファイルを全て 1 ページの PDF ファイルに分割するクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class PageSplitter : IDocumentWriter
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// PageSplitter
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PageSplitter() { }

        /* ----------------------------------------------------------------- */
        ///
        /// ~PageSplitter
        /// 
        /// <summary>
        /// オブジェクトを破棄します。
        /// </summary>
        /// 
        /// <remarks>
        /// クラスで必要な終了処理は、デストラクタではなく Dispose メソッド
        /// に記述して下さい。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        ~PageSplitter()
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
        /// 通常時には iTextSharp の PdfCopy クラスを用いて結合を行って
        /// いるが、このクラスは複数の PDF ファイルが同じフォントを使用
        /// していたとしても別々のものとして扱うため、フォント情報が重複して
        /// ファイルサイズが増大する場合がある。
        /// 
        /// この問題を解決したものとして PdfSmartCopy クラスが存在する。
        /// ただし、複雑な注釈が保存されている PDF を結合する際に使用した
        /// 場合、（別々として扱わなければならないはずの）情報が共有されて
        /// しまい、注釈の構造が壊れてしまう問題が確認されている。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public bool UseSmartCopy { get; set; } = true;

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
            Metadata = new Metadata();
            Encryption = new Encryption();
            Pages.Clear();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        /// 
        /// <summary>
        /// PDF ファイルを指定されたパスに保存します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Save(string folder)
        {
            var results = new List<string>();
            Save(folder, results);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        /// 
        /// <summary>
        /// PDF ファイルを指定されたパスに保存します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Save(string folder, IList<string> results)
        {
            if (!IoEx.Directory.Exists(folder)) IoEx.Directory.CreateDirectory(folder);

            var readers = new Dictionary<string, PdfReader>();

            foreach (var page in Pages)
            {
                if (page.File is File) SavePage(page, folder, results, readers);
                else if (page.File is ImageFile) SaveImagePage(page, folder, results);
            }

            foreach (var reader in readers.Values) reader.Close();
            readers.Clear();
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
            if (disposing) Reset();
        }

        #endregion

        #region Other private methods

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        /// 
        /// <summary>
        /// PDF ファイルを分割して保存します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SavePage(Page src, string folder, IList<string> results, Dictionary<string, PdfReader> readers)
        {
            if (src == null) return;

            if (!readers.ContainsKey(src.File.FullName)) AddReader(src.File as File, readers);
            if (!readers.ContainsKey(src.File.FullName)) return;

            var reader = readers[src.File.FullName];
            var rot = reader.GetPageRotation(src.Number);
            var dic = reader.GetPageN(src.Number);
            if (rot != src.Rotation) dic.Put(PdfName.ROTATE, new PdfNumber(src.Rotation));

            var basename = IoEx.Path.GetFileNameWithoutExtension(src.File.FullName);
            var pagenum = src.Number;
            var dest = Unique(folder, basename, pagenum, reader.NumberOfPages);
            SaveOne(reader, pagenum, dest);
            results.Add(dest);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        /// 
        /// <summary>
        /// 画像ファイルを 1 ページの PDF ファイルに変換して保存します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SaveImagePage(Page src, string folder, IList<string> results)
        {
            if (src == null) return;

            using (var image = new System.Drawing.Bitmap(src.File.FullName))
            using (var stream = new IoEx.MemoryStream())
            {
                var document = new iTextSharp.text.Document();
                var writer = PdfWriter.GetInstance(document, stream);
                document.Open();

                var guid = image.FrameDimensionsList[0];
                var dimension = new System.Drawing.Imaging.FrameDimension(guid);
                for (var i = 0; i < image.GetFrameCount(dimension); ++i)
                {
                    image.SelectActiveFrame(dimension, i);

                    var dpi = 72f / image.HorizontalResolution;
                    var obj = iTextSharp.text.Image.GetInstance(image, image.GuessImageFormat());
                    obj.SetAbsolutePosition(0, 0);
                    obj.ScalePercent(dpi * 100f);

                    document.SetPageSize(new iTextSharp.text.Rectangle(image.Width * dpi, image.Height * dpi));
                    document.NewPage();
                    document.Add(obj);
                }

                document.Close();
                writer.Close();

                using (var reader = new PdfReader(stream.ToArray())) SaveImagePage(src, folder, results, reader);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        /// 
        /// <summary>
        /// 画像ファイルを 1 ページの PDF ファイルに変換して保存します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SaveImagePage(Page src, string folder, IList<string> results, PdfReader reader)
        {
            for (var i = 0; i < reader.NumberOfPages; ++i)
            {
                var basename = IoEx.Path.GetFileNameWithoutExtension(src.File.FullName);
                var pagenum = i + 1;
                var dest = Unique(folder, basename, pagenum, reader.NumberOfPages);
                SaveOne(reader, pagenum, dest);
                results.Add(dest);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SaveOne
        /// 
        /// <summary>
        /// 1 ページの PDF ファイルを保存します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private void SaveOne(PdfReader reader, int pagenum, string dest)
        {
            var document = new iTextSharp.text.Document();
            var writer   = UseSmartCopy ?
                           new PdfSmartCopy(document, new IoEx.FileStream(dest, IoEx.FileMode.Create)) :
                           new PdfCopy(document, new IoEx.FileStream(dest, IoEx.FileMode.Create));

            writer.PdfVersion = Metadata.Version.Minor.ToString()[0];
            writer.ViewerPreferences = Metadata.ViewPreferences;

            document.Open();

            writer.AddPage(writer.GetImportedPage(reader, pagenum));
            AddMetadata(document);
            AddEncryption(writer);

            document.Close();
            writer.Close();
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
        private void AddMetadata(iTextSharp.text.Document document)
        {
            document.AddTitle(Metadata.Title);
            document.AddSubject(Metadata.Subtitle);
            document.AddKeywords(Metadata.Keywords);
            document.AddCreator(Metadata.Creator);
            document.AddAuthor(Metadata.Author);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// AddEncryption
        /// 
        /// <summary>
        /// 各種セキュリティ情報を付加します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void AddEncryption(PdfCopy writer)
        {
            if (Encryption.IsEnabled && Encryption.OwnerPassword.Length > 0)
            {
                var method     = Transform.ToIText(Encryption.Method);
                var permission = Transform.ToIText(Encryption.Permission);
                var userpass   = Encryption.IsUserPasswordEnabled ?
                                 GetUserPassword(Encryption.UserPassword, Encryption.OwnerPassword) :
                                 string.Empty;
                writer.SetEncryption(method, userpass, Encryption.OwnerPassword, permission);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// AddReader
        /// 
        /// <summary>
        /// ファイル情報を基に PdfReader オブジェクトを生成し、一覧に
        /// 追加します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void AddReader(File src, Dictionary<string, PdfReader> dest)
        {
            if (src == null) return;

            var item = src.Password.Length > 0 ?
                       new PdfReader(src.FullName, System.Text.Encoding.UTF8.GetBytes(src.Password)) :
                       new PdfReader(src.FullName);
            dest.Add(src.FullName, item);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetUserPassword
        /// 
        /// <summary>
        /// ユーザパスワードを取得します。
        /// </summary>
        /// 
        /// <remarks>
        /// ユーザから明示的にユーザパスワードが指定されていない場合、
        /// オーナパスワードと同じ文字列を使用します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private string GetUserPassword(string userPassword, string ownerPassword)
        {
            return !string.IsNullOrEmpty(userPassword) ? userPassword : ownerPassword;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Unique
        /// 
        /// <summary>
        /// 一意のパス名を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private string Unique(string folder, string basename, int pagenum, int all)
        {
            var digit = string.Format("D{0}", all.ToString("D").Length);
            for (var i = 1; i < 1000; ++i)
            {
                var filename = (i == 1) ?
                               string.Format("{0}-{1}.pdf", basename, pagenum.ToString(digit)) :
                               string.Format("{0}-{1} ({2}).pdf", basename, pagenum.ToString(digit), i);
                var dest = IoEx.Path.Combine(folder, filename);
                if (!IoEx.File.Exists(dest)) return dest;
            }

            return IoEx.Path.Combine(folder, IoEx.Path.GetRandomFileName());
        }

        #endregion

        #region Fields
        private bool _disposed = false;
        #endregion
    }
}
