/* ------------------------------------------------------------------------- */
///
/// DocumentReader.cs
///
/// Copyright (c) 2010 CubeSoft, Inc. All rights reserved.
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
using System.Collections.Generic;
using System.Threading.Tasks;
using Cube.Pdf.Editing.Extensions;
using ReaderImpl = iTextSharp.text.pdf.PdfReader;
using ImageParser = iTextSharp.text.pdf.parser.PdfReaderContentParser;
using BadPasswordException = iTextSharp.text.exceptions.BadPasswordException;

namespace Cube.Pdf.Editing
{
    /* --------------------------------------------------------------------- */
    ///
    /// Cube.Pdf.Editing.DocumentReader
    /// 
    /// <summary>
    /// PDF ファイルを読み込んで各種情報を保持するためのクラスです。
    /// </summary>
    /// 
    /// <remarks>
    /// このクラスは iTextSharp を用いて PDF ファイルの解析を行います。
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public class DocumentReader : IDocumentReader
    {
        #region Constructors and destructors

        /* ----------------------------------------------------------------- */
        ///
        /// ~DocumentReader
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
        ~DocumentReader()
        {
            Dispose(false);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Path
        /// 
        /// <summary>
        /// PDF ファイルのパスを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Path { get; private set; } = string.Empty;

        /* ----------------------------------------------------------------- */
        ///
        /// Metadata
        /// 
        /// <summary>
        /// PDF ファイルに関するメタデータを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Metadata Metadata { get; private set; } = null;

        /* ----------------------------------------------------------------- */
        ///
        /// Metadata
        /// 
        /// <summary>
        /// PDF ファイルに関する暗号化情報を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Encryption Encryption { get; private set; } = null;

        /* ----------------------------------------------------------------- */
        ///
        /// EncryptionStatus
        /// 
        /// <summary>
        /// 暗号化されている PDF ファイルへのアクセス状態を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public EncryptionStatus EncryptionStatus { get; private set; } = EncryptionStatus.NotEncrypted;

        /* ----------------------------------------------------------------- */
        ///
        /// Pages
        /// 
        /// <summary>
        /// PDF ファイルのページ一覧を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IReadOnlyCollection<IPage> Pages { get; private set; } = null;

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// OpenAsync
        /// 
        /// <summary>
        /// PDF ファイルを非同期で開きます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Task OpenAsync(string filename, string password)
        {
            return Task.Run(() => { Open(filename, password); });
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Close
        /// 
        /// <summary>
        /// 現在、開いている PDF ファイルを閉じます。
        /// </summary>
        /// 
        /// <remarks>
        /// TODO: Pages オブジェクトの破棄方法を検討する。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public void Close()
        {
            if (_impl == null) return;

            _impl.Close();
            _impl = null;
            Metadata = null;
            Encryption = null;
            EncryptionStatus = EncryptionStatus.NotEncrypted;
            Pages = null;
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
        /// GetPage
        /// 
        /// <summary>
        /// 指定されたページ番号に対応するページ情報を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Page GetPage(int pagenum)
        {
            return _impl != null ? _impl.CreatePage(Path, GetInputPassword(), pagenum) : null;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetImagesAsync
        /// 
        /// <summary>
        /// 指定されたページ中に存在する画像を非同期で取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public Task<IList<Image>> GetImagesAsync(int pagenum)
        {
            return Task.Run(() => { return GetImages(pagenum); });
        }

        #endregion

        #region Override methods

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
            if (disposing) Close();
        }

        #endregion

        #region Other private methods

        /* ----------------------------------------------------------------- */
        ///
        /// Open
        /// 
        /// <summary>
        /// PDF ファイルを開きます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Open(string path, string password)
        {
            try
            {
                var bytes = !string.IsNullOrEmpty(password) ? System.Text.Encoding.UTF8.GetBytes(password) : null;
                _impl = new ReaderImpl(path, bytes, true);
                Path = path;
                Pages = new ReadOnlyPageCollection(_impl, Path, GetInputPassword());
            }
            catch (BadPasswordException err) { throw new EncryptionException(err.Message, err); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetImages
        /// 
        /// <summary>
        /// 指定されたページ中に存在する画像を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private IList<Image> GetImages(int pagenum)
        {
            if (pagenum < 0 || pagenum > Pages.Count) throw new IndexOutOfRangeException();

            var parser = new ImageParser(_impl);
            var listener = new ImageRenderListener();
            parser.ProcessContent(pagenum, listener);
            return listener.Images;
        }

        #endregion

        #region Other private methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetInputPassword
        /// 
        /// <summary>
        /// ユーザの入力したパスワードを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string GetInputPassword()
        {
            if (Encryption == null || !Encryption.IsEnabled) return string.Empty;
            else if (!string.IsNullOrEmpty(Encryption.OwnerPassword)) return Encryption.OwnerPassword;
            else if (Encryption.IsUserPasswordEnabled && !string.IsNullOrEmpty(Encryption.UserPassword))
            {
                return Encryption.UserPassword;
            }
            else return string.Empty;
        }

        #endregion

        #region Fields
        private bool _disposed = false;
        private ReaderImpl _impl = null;
        #endregion
    }
}
