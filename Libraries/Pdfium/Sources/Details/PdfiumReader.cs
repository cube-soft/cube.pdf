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
    /// Provides functionality to read PDF documents via PDFium API.
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
        /// Initializes a new instance of the PdfiumReader class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="src">Path of the source PDF file.</param>
        /// <param name="io">I/O handler.</param>
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
        /// Gets the path of the source PDF file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Source { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// IO
        ///
        /// <summary>
        /// Gets the I/O handler.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IO IO { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// File
        ///
        /// <summary>
        /// Gets information of the PDF file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PdfFile File { get; private set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Metadata
        ///
        /// <summary>
        /// Gets the metadata of the PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Metadata Metadata { get; private set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Encryption
        ///
        /// <summary>
        /// Gets the encryption information of the PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Encryption Encryption { get; private set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Pages
        ///
        /// <summary>
        /// Gets the collection of pages.
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
            var dest = new PdfiumReader(src, io);
            var pass = (query as QueryValue<string>)?.Value ?? string.Empty;

            while (true)
            {
                try
                {
                    dest.Load(pass);
                    var denied = fullaccess && dest.File is PdfFile pf && !pf.FullAccess;
                    if (denied) throw new LoadException(LoadStatus.PasswordError);
                    return dest;
                }
                catch (LoadException e)
                {
                    if (e.Status != LoadStatus.PasswordError) throw;
                    if (query is QueryValue<string>) throw new EncryptionException(e.Message, e);
                    var args = query.RequestPassword(src);
                    if (!args.Cancel) pass = args.Result;
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
        /// Loads the PDF document.
        /// </summary>
        ///
        /// <param name="password">Password.</param>
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
        /// Reads data from the specified stream.
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
        /// Represents the delegate to read data from the specified stream.
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
