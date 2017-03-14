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
using System.Collections.Generic;
using System.Drawing;
using Cube.Pdf.Drawing.MuPdf;
using IoEx = System.IO;

namespace Cube.Pdf.Drawing
{
    /* --------------------------------------------------------------------- */
    ///
    /// DocumentReader
    /// 
    /// <summary>
    /// PDF ファイルを読み込み、各種情報の保持およびページのイメージを
    /// 生成するためのクラスです。
    /// </summary>
    /// 
    /// <remarks>
    /// MuPDF を用いて PDF ファイルの解析を行います。
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public class DocumentReader : IDocumentReader
    {
        #region Constructors and destructors

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
        /// File
        /// 
        /// <summary>
        /// ファイル情報を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public FileBase File { get; private set; } = null;

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
        public IReadOnlyCollection<Page> Pages { get; private set; } = null;

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
        {
            get
            {
                return _mupdf     != null &&
                       File       != null &&
                       Metadata   != null &&
                       Encryption != null &&
                       Pages      != null ;
            }
        }

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
        public event EventHandler<QueryEventArgs<string, string>> PasswordRequired;

        #endregion

        #region Methods

        #region IDocumentReader

        /* ----------------------------------------------------------------- */
        ///
        /// Open
        /// 
        /// <summary>
        /// PDF ファイルを開きます。
        /// </summary>
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
        /// <remarks>
        /// TODO: パスワードを要求された時の処理の実装
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public void Open(string path, string password)
        {
            lock (_lock)
            {
                if (_mupdf != IntPtr.Zero) NativeMethods.Dispose(_mupdf);
                _mupdf = NativeMethods.Create();
                if (_mupdf == IntPtr.Zero) throw new IoEx.FileLoadException();

                var count = NativeMethods.LoadFile(_mupdf, path, password);
                if (count < 0) throw new IoEx.FileLoadException();
                NativeMethods.SetAlphaBits(_mupdf, 8);

                var file = new PdfFile(path, password);
                file.FullAccess = true;
                file.PageCount = count;

                File       = file;
                Metadata   = _mupdf.CreateMetadata();
                Encryption = _mupdf.CreateEncryption(file.Password);
                Pages      = new ReadOnlyPageCollection(_mupdf, file);
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
        /* ----------------------------------------------------------------- */
        public Page GetPage(int pagenum)
            => _mupdf != null && File != null ?
               _mupdf.CreatePage(File, pagenum) :
               null;

        #endregion

        #region IDisposable

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
                File       = null;
                Metadata   = null;
                Encryption = null;
                Pages      = null;
            }

            NativeMethods.Dispose(_mupdf);
            _mupdf = IntPtr.Zero;
        }

        #endregion

        #region Extended

        /* ----------------------------------------------------------------- */
        ///
        /// CreateImage
        /// 
        /// <summary>
        /// 指定されたページ番号に対応するイメージを生成します。
        /// </summary>
        /// 
        /// <remarks>
        /// 引数 power には、取得したいイメージのサイズを元データのサイズ
        /// に対する倍率で指定します。省略時は、等倍率の画像データが
        /// 生成されます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public Image CreateImage(int pagenum, double power = 1.0)
        {
            lock (_lock)
            {
                var page = _mupdf.CreatePage(File, pagenum);
                return (page != null) ? _mupdf.CreateImage(page, power) : null;
            }
        }

        #endregion

        #endregion

        #region Virtual methods

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

        #region Fields
        private bool _disposed = false;
        private object _lock = new object();
        private IntPtr _mupdf = IntPtr.Zero;
        #endregion
    }
}
