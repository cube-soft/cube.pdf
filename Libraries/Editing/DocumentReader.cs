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
using iTextSharp.text.pdf;
using iTextSharp.text.exceptions;
using Cube.Pdf.Editing.Extensions;
using TaskEx = Cube.TaskEx;

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
            return TaskEx.Run(() => { Open(filename, password); });
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
            return TaskEx.Run(() => { return GetImages(pagenum); });
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
                _impl = new PdfReader(path, bytes, true);
                Path = path;
                Pages = new ReadOnlyPageCollection(_impl, Path, password);
                Metadata = GetMetadata(_impl);
                EncryptionStatus = GetEncryptionStatus(_impl, password);
                Encryption = GetEncryption(_impl, password, EncryptionStatus);
            }
            catch (BadPasswordException err) { throw new EncryptionException(err.Message, err); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetMetadata
        /// 
        /// <summary>
        /// PDF ファイルのメタデータを抽出して返します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private Metadata GetMetadata(PdfReader src)
        {
            var dest = new Metadata();

            dest.Version         = new Version(1, Int32.Parse(src.PdfVersion.ToString()), 0, 0);
            dest.Author          = src.Info.ContainsKey("Author") ? src.Info["Author"] : "";
            dest.Title           = src.Info.ContainsKey("Title") ? src.Info["Title"] : "";
            dest.Subtitle        = src.Info.ContainsKey("Subject") ? src.Info["Subject"] : "";
            dest.Keywords        = src.Info.ContainsKey("Keywords") ? src.Info["Keywords"] : "";
            dest.Creator         = src.Info.ContainsKey("Creator") ? src.Info["Creator"] : "";
            dest.Producer        = src.Info.ContainsKey("Producer") ? src.Info["Producer"] : "";
            dest.ViewPreferences = src.SimpleViewerPreferences;

            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetEncryption
        /// 
        /// <summary>
        /// PDF ファイルの暗号化に関わる情報を抽出して返します。
        /// </summary>
        /// 
        /// <remarks>
        /// TODO: 暗号化方式が AES256 の場合、ユーザパスワードの解析に
        /// 失敗するので除外しています。AES256 の場合の解析方法を要検討。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private Encryption GetEncryption(PdfReader src, string password, EncryptionStatus status)
        {
            var dest = new Encryption();
            if (status == EncryptionStatus.NotEncrypted) return dest;

            dest.IsEnabled     = true;
            dest.OwnerPassword = (status == EncryptionStatus.FullAccess) ? password : string.Empty;
            dest.Method        = Translator.ToEncryptionMethod(src.GetCryptoMode());
            dest.Permission    = Translator.ToPermission(src.Permissions);

            switch (status)
            {
                case EncryptionStatus.FullAccess:
                    if (dest.Method == EncryptionMethod.Aes256) break; // see remarks

                    var bytes = src.ComputeUserPassword();
                    if (bytes != null && bytes.Length > 0)
                    {
                        dest.IsUserPasswordEnabled = true;
                        dest.UserPassword = System.Text.Encoding.UTF8.GetString(bytes);
                    }
                    break;
                case EncryptionStatus.RestrictedAccess:
                    if (string.IsNullOrEmpty(password))
                    {
                        dest.IsUserPasswordEnabled = true;
                        dest.UserPassword = password;
                    }
                    break;
                default:
                    break;
            }

            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetEncryptionStatus
        /// 
        /// <summary>
        /// PDF ファイルの複合状態に関わる情報を抽出して返します。
        /// </summary>
        /// 
        /// <remarks>
        /// パスワードは、オーナパスワードとユーザパスワードのどちらかが
        /// 指定されます。どちらが指定されたのかについては、
        /// PdfReader.IsOpenedWithFullPermissions プロパティから判断します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private EncryptionStatus GetEncryptionStatus(PdfReader src, string password)
        {
            if (src.IsOpenedWithFullPermissions)
            {
                if (string.IsNullOrEmpty(password)) return EncryptionStatus.NotEncrypted;
                else return EncryptionStatus.FullAccess;
            }
            else return EncryptionStatus.RestrictedAccess;
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

            var parser = new iTextSharp.text.pdf.parser.PdfReaderContentParser(_impl);
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
        private PdfReader _impl = null;
        #endregion
    }
}
