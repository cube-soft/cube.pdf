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
using System.Drawing;
using System.Linq;
using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text.exceptions;
using Cube.Pdf.Editing.ITextReader;

namespace Cube.Pdf.Editing
{
    /* --------------------------------------------------------------------- */
    ///
    /// DocumentReader
    /// 
    /// <summary>
    /// PDF ファイルを読み込んで各種情報を保持するためのクラスです。
    /// </summary>
    /// 
    /// <remarks>
    /// iTextSharp を用いて PDF ファイルの解析を行います。
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public class DocumentReader : IDocumentReader
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// DocumentReader
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public DocumentReader() { }

        /* ----------------------------------------------------------------- */
        ///
        /// DocumentReader
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public DocumentReader(string path)
        {
            Open(path);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// DocumentReader
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public DocumentReader(string path, string password)
        {
            Open(path, password);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// File
        /// 
        /// <summary>
        /// ファイル情報を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public MediaFile File { get; private set; } = null;

        /* ----------------------------------------------------------------- */
        ///
        /// Metadata
        /// 
        /// <summary>
        /// PDF ファイルに関するメタ情報を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Metadata Metadata { get; private set; } = null;

        /* ----------------------------------------------------------------- */
        ///
        /// Encryption
        /// 
        /// <summary>
        /// PDF ファイルに関する暗号化情報を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Encryption Encryption { get; private set; } = null;

        /* ----------------------------------------------------------------- */
        ///
        /// Pages
        /// 
        /// <summary>
        /// PDF ファイルのページ一覧を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<Page> Pages { get; private set; } = null;

        /* ----------------------------------------------------------------- */
        ///
        /// Attachments
        /// 
        /// <summary>
        /// 添付ファイルの一覧を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<Attachment> Attachments { get; private set; } = null;

        /* ----------------------------------------------------------------- */
        ///
        /// RawObject
        /// 
        /// <summary>
        /// 内部実装のオブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PdfReader RawObject { get; private set; } = null;

        /* ----------------------------------------------------------------- */
        ///
        /// IsOpen
        /// 
        /// <summary>
        /// ファイルが正常に開いているかどうかを示す値を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool IsOpen
            => RawObject   != null &&
               File        != null &&
               Metadata    != null &&
               Encryption  != null &&
               Pages       != null &&
               Attachments != null;

        #endregion

        #region Events

        /* ----------------------------------------------------------------- */
        ///
        /// PasswordRequired
        /// 
        /// <summary>
        /// パスワードが要求された時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public event QueryEventHandler<string, string> PasswordRequired;

        /* ----------------------------------------------------------------- */
        ///
        /// OnPasswordRequired
        /// 
        /// <summary>
        /// パスワードが要求された時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnPasswordRequired(QueryEventArgs<string, string> e)
        {
            if (PasswordRequired != null) PasswordRequired(this, e);
            else e.Cancel = true;
        }

        #endregion

        #region Methods

        #region IDisposable

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

            if (RawObject == null) return;

            if (disposing)
            {
                RawObject.Dispose();
                RawObject = null;

                File       = null;
                Metadata   = null;
                Encryption = null;
                Pages      = null;
            }
        }

        #endregion

        #region IDocumentReader

        /* ----------------------------------------------------------------- */
        ///
        /// Open
        /// 
        /// <summary>
        /// PDF ファイルを開きます。
        /// </summary>
        /// 
        /// <param name="path">PDF ファイルのパス</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Open(string path) => Open(path, string.Empty);

        /* ----------------------------------------------------------------- */
        ///
        /// Open
        /// 
        /// <summary>
        /// PDF ファイルを開きます。
        /// </summary>
        /// 
        /// <param name="path">PDF ファイルのパス</param>
        /// <param name="password">
        /// オーナパスワードまたはユーザパスワード
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        public void Open(string path, string password)
            => Open(path, password, false);

        /* ----------------------------------------------------------------- */
        ///
        /// Open
        /// 
        /// <summary>
        /// PDF ファイルを開きます。
        /// </summary>
        /// 
        /// <param name="path">PDF ファイルのパス</param>
        /// 
        /// <param name="password">
        /// オーナパスワードまたはユーザパスワード
        /// </param>
        /// 
        /// <param name="onlyFullAccess">
        /// フルアクセスのみを許可するかどうかを示す値
        /// </param>
        /// 
        /// <remarks>
        /// onlyFullAccess が true の場合、ユーザパスワードで
        /// PDF ファイルを開こうとすると PasswordRequired イベントが
        /// 発生します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public void Open(string path, string password, bool onlyFullAccess)
        {
            try
            {
                var pass = !string.IsNullOrEmpty(password) ? Encoding.UTF8.GetBytes(password) : null;
                RawObject = new PdfReader(path, pass, true);
                if (onlyFullAccess && !RawObject.IsOpenedWithFullPermissions)
                {
                    throw new BadPasswordException("allow only owner password");
                }

                var file = new PdfFile(path, password)
                {
                    FullAccess = RawObject.IsOpenedWithFullPermissions,
                    PageCount  = RawObject.NumberOfPages
                };

                File        = file;
                Metadata    = GetMetadata(RawObject);
                Encryption  = GetEncryption(RawObject, password, file.FullAccess);
                Pages       = new ReadOnlyPageCollection(RawObject, file);
                Attachments = new ReadOnlyAttachmentCollection(RawObject, file);
            }
            catch (BadPasswordException /* err */)
            {
                if (RawObject != null)
                {
                    RawObject.Dispose();
                    RawObject = null;
                }

                var e = new QueryEventArgs<string, string>(path);
                OnPasswordRequired(e);
                if (!e.Cancel) Open(path, e.Result);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetPage
        /// 
        /// <summary>
        /// 指定されたページ番号に対応するページ情報を取得します。
        /// </summary>
        /// 
        /// <param name="pagenum">ページ番号</param>
        /// 
        /// <returns>ページ情報を保持するオブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public Page GetPage(int pagenum)
            => RawObject != null && File != null ?
               RawObject.CreatePage(File, pagenum) :
               null;

        #endregion

        /* ----------------------------------------------------------------- */
        ///
        /// GetImages
        /// 
        /// <summary>
        /// 指定されたページ中に存在する画像を取得します。
        /// </summary>
        /// 
        /// <param name="pagenum">ページ番号</param>
        /// 
        /// <returns>抽出された Image オブジェクトのリスト</returns>
        /// 
        /* ----------------------------------------------------------------- */
        public IEnumerable<Image> GetImages(int pagenum)
        {
            if (pagenum < 0 || pagenum > Pages.Count()) throw new IndexOutOfRangeException();

            var parser = new iTextSharp.text.pdf.parser.PdfReaderContentParser(RawObject);
            var listener = new ImageRenderListener();
            parser.ProcessContent(pagenum, listener);
            return listener.Images;
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetMetadata
        /// 
        /// <summary>
        /// PDF ファイルのメタデータを抽出して返します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private Metadata GetMetadata(PdfReader src) => new Metadata()
        {
            Version         = new Version(1, Int32.Parse(src.PdfVersion.ToString()), 0, 0),
            Author          = src.Info.ContainsKey("Author")   ? src.Info["Author"]   : "",
            Title           = src.Info.ContainsKey("Title")    ? src.Info["Title"]    : "",
            Subtitle        = src.Info.ContainsKey("Subject")  ? src.Info["Subject"]  : "",
            Keywords        = src.Info.ContainsKey("Keywords") ? src.Info["Keywords"] : "",
            Creator         = src.Info.ContainsKey("Creator")  ? src.Info["Creator"]  : "",
            Producer        = src.Info.ContainsKey("Producer") ? src.Info["Producer"] : "",
            ViewPreferences = src.SimpleViewerPreferences
        };

        /* ----------------------------------------------------------------- */
        ///
        /// GetEncryption
        /// 
        /// <summary>
        /// Encryption オブジェクトを取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private Encryption GetEncryption(PdfReader src, string password, bool access)
        {
            var dest = new Encryption();
            if (access && string.IsNullOrEmpty(password)) return dest;

            dest.IsEnabled     = true;
            dest.Method        = Transform.ToEncryptionMethod(src.GetCryptoMode());
            dest.Permission    = Transform.ToPermission(src.Permissions);
            dest.OwnerPassword = access ? password : string.Empty;
            dest.UserPassword  = GetUserPassword(src, password, access, dest.Method);
            dest.IsUserPasswordEnabled = !string.IsNullOrEmpty(dest.UserPassword);

            return dest;
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
        /// TODO: 暗号化方式が AES256 の場合、ユーザパスワードの解析に
        /// 失敗するので除外しています。AES256 の場合の解析方法を要検討。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private string GetUserPassword(PdfReader src, string password, bool access, EncryptionMethod method)
        {
            if (access)
            {
                if (method == EncryptionMethod.Aes256) return string.Empty; // see remarks
                var bytes = src.ComputeUserPassword();
                if (bytes != null && bytes.Length > 0) return Encoding.UTF8.GetString(bytes);
            }
            return password;
        }

        #region Fields
        private bool _disposed = false;
        #endregion

        #endregion
    }
}
