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
    internal sealed class PdfiumReader : PdfiumLibrary
    {
        #region Constructors

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
        /// <param name="password">Password string or query.</param>
        /// <param name="options">Other options.</param>
        ///
        /// <returns>PdfiumReader object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static PdfiumReader Create(string src,
            QueryMessage<IQuery<string>, string> password,
            OpenOption options
        ) {
            var dest = new PdfiumReader(src, options.IO);
            try
            {
                Load(src, dest, password, options);
                return dest;
            }
            catch { dest.Dispose(); throw; }
        }

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

            _stream  = IO.OpenRead(src);
            _handler = new GetBlockHandler(Read);
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
        /// Invoke
        ///
        /// <summary>
        /// Invokes the specified action with the global lock.
        /// </summary>
        ///
        /// <param name="action">User action.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Invoke(Action<IntPtr> action) => Invoke(() => action(_core));

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Invokes the specified function with the global lock.
        /// </summary>
        ///
        /// <param name="func">User function.</param>
        ///
        /// <returns>Returned value of the specified function.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public T Invoke<T>(Func<IntPtr, T> func) => Invoke(() => func(_core));

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
        protected override void Dispose(bool disposing) => Lock(() =>
        {
            if (_core != IntPtr.Zero)
            {
                NativeMethods.FPDF_CloseDocument(_core);
                _core = IntPtr.Zero;
            }
            if (disposing) _stream?.Dispose();
        });

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Load
        ///
        /// <summary>
        /// Loads the PDF document with the specified arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static void Load(string src,
            PdfiumReader dest,
            QueryMessage<IQuery<string>, string> password,
            OpenOption options
        ) {
            while (true)
            {
                try
                {
                    dest.Load(password.Value);
                    var denied = options.FullAccess && dest.File is PdfFile f && !f.FullAccess;
                    if (denied) throw new PdfiumException(PdfiumStatus.PasswordError);
                    else return;
                }
                catch (PdfiumException err)
                {
                    if (err.Status != PdfiumStatus.PasswordError) throw;
                    var msg = password.Source.Request(src);
                    if (!msg.Cancel) password.Value = msg.Value;
                    else throw new OperationCanceledException("Password");
                }
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Load
        ///
        /// <summary>
        /// Loads the PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Load(string password)
        {
            _core = Invoke(() => NativeMethods.FPDF_LoadCustomDocument(
                new FileAccess
                {
                    Length    = (uint)_stream.Length,
                    GetBlock  = Marshal.GetFunctionPointerForDelegate(_handler),
                    Parameter = IntPtr.Zero,
                },
                password
            ));

            if (_core == IntPtr.Zero) throw GetLastError();

            Metadata   = MetadataFactory.Create(this);
            Encryption = EncryptionFactory.Create(this, password);
            File       = FileFactory.Create(this, password, !Encryption.OpenWithPassword);
            Pages      = new PageCollection(this, File);
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
        private int Read(IntPtr param, uint pos, IntPtr dest, uint size)
        {
            try
            {
                var bytes = new byte[size];

                _stream.Position = pos;
                if (_stream.Read(bytes, 0, (int)size) != size) return 0;

                Marshal.Copy(bytes, 0, dest, (int)size);
                return 1;
            }
            catch { return 0; }
        }

        #endregion

        #region Fields
        private readonly System.IO.Stream _stream;
        private readonly GetBlockHandler _handler;
        private IntPtr _core;
        #endregion
    }
}
