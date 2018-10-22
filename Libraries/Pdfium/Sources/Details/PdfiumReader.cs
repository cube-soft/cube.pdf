/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
/* ------------------------------------------------------------------------- */
using Cube.FileSystem;
using Cube.Pdf.Mixin;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Cube.Pdf.Pdfium
{
    /* --------------------------------------------------------------------- */
    ///
    /// PdfiumReader
    ///
    /// <summary>
    /// PDFium の API をラップした Reader クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal sealed class PdfiumReader : PdfiumLibrary, IDisposable
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// PdfiumReader
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="src">入力ファイルのパス</param>
        /// <param name="io">I/O オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        private PdfiumReader(string src, IO io)
        {
            Source = src;
            IO     = io;

            _stream   = IO.OpenRead(src);
            _delegate = new ReadDelegate(Read);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Source
        ///
        /// <summary>
        /// 対象となる PDF ファイルのパスを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Source { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// IO
        ///
        /// <summary>
        /// I/O オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IO IO { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// File
        ///
        /// <summary>
        /// ファイル情報を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PdfFile File { get; private set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Metadata
        ///
        /// <summary>
        /// メタ情報を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Metadata Metadata { get; private set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Encryption
        ///
        /// <summary>
        /// 暗号化情報を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Encryption Encryption { get; private set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Pages
        ///
        /// <summary>
        /// ページ一覧を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<Page> Pages { get; private set; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Creates a new instance of the PdfiumReader class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="src">Path of the PDF file.</param>
        /// <param name="query">Password query.</param>
        /// <param name="fullaccess">Requires full access.</param>
        /// <param name="io">I/O handler.</param>
        ///
        /// <returns>PdfiumReader object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static PdfiumReader Create(string src, IQuery<string> query, bool fullaccess, IO io)
        {
            var dest     = new PdfiumReader(src, io);
            var password = string.Empty;

            while (true)
            {
                try
                {
                    dest.Load(password);
                    var denied = fullaccess && dest.File is PdfFile pf && !pf.FullAccess;
                    if (denied) throw new LoadException(LoadStatus.PasswordError);
                    return dest;
                }
                catch (LoadException err)
                {
                    if (err.Status != LoadStatus.PasswordError) throw;
                    var e = query.RequestPassword(src);
                    if (!e.Cancel) password = e.Result;
                    else throw new OperationCanceledException();
                }
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Invokes the specified action.
        /// </summary>
        ///
        /// <param name="action">Action object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Invoke(Action<IntPtr> action)
        {
            if (_disposed) throw new ObjectDisposedException(GetType().Name);
            lock (_lock) action(_core);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Invokes the specified function.
        /// </summary>
        ///
        /// <param name="func">Function object.</param>
        ///
        /// <returns>Return value for the specified object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public T Invoke<T>(Func<IntPtr, T> func)
        {
            if (_disposed) throw new ObjectDisposedException(GetType().Name);
            lock (_lock) return func(_core);
        }

        #region IDisposable

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases all resources used by the PdfiumReader.
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
        /// Releases the unmanaged resources used by the PdfiumReader
        /// and optionally releases the managed resources.
        /// </summary>
        ///
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        private void Dispose(bool disposing)
        {
            if (_disposed) return;
            _disposed = true;

            lock (_lock)
            {
                if (_core != IntPtr.Zero)
                {
                    PdfiumApi.FPDF_CloseDocument(_core);
                    _core = IntPtr.Zero;
                }
            }

            if (disposing) _stream.Dispose();
        }

        #endregion

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Load
        ///
        /// <summary>
        /// PDF を読み込みます。
        /// </summary>
        ///
        /// <param name="password">パスワード</param>
        ///
        /* ----------------------------------------------------------------- */
        private void Load(string password)
        {
            var core = PdfiumApi.FPDF_LoadCustomDocument(
                new FileAccess
                {
                    Length    = (uint)_stream.Length,
                    GetBlock  = Marshal.GetFunctionPointerForDelegate(_delegate),
                    Parameter = IntPtr.Zero,
                },
                password
            );

            if (core == IntPtr.Zero) throw GetLastError();

            _core      = core;
            Encryption = EncryptionFactory.Create(this, password);
            File       = Create(password, !Encryption.OpenWithPassword);
            Pages      = new ReadOnlyPageList(this, File);
            Metadata   = MetadataFactory.Create(this);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Creates a PdfFile object from the specified arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private PdfFile Create(string password, bool fullaccess)
        {
            var dest = IO.GetPdfFile(Source, password);
            dest.Count      = Invoke(e => PdfiumApi.FPDF_GetPageCount(e));
            dest.FullAccess = fullaccess;
            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Read
        ///
        /// <summary>
        /// ストリームからデータを読み込みます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private int Read(IntPtr param, uint pos, IntPtr buffer, uint size)
        {
            try
            {
                var managed = new byte[size];

                _stream.Position = pos;
                if (_stream.Read(managed, 0, (int)size) != size) return 0;

                Marshal.Copy(managed, 0, buffer, (int)size);
                return 1;
            }
            catch { return 0; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ReadDelegate
        ///
        /// <summary>
        /// ストリームからデータを読み込むメソッドを表します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int ReadDelegate(IntPtr param, uint pos, IntPtr buffer, uint size);

        #endregion

        #region Fields
        private readonly object _lock = new object();
        private readonly System.IO.Stream _stream;
        private readonly ReadDelegate _delegate;
        private IntPtr _core;
        private bool _disposed;
        #endregion
    }
}
