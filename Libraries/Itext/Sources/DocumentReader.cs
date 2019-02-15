/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
/* ------------------------------------------------------------------------- */
using Cube.FileSystem;
using Cube.Pdf.Mixin;
using iTextSharp.text.pdf;

namespace Cube.Pdf.Itext
{
    /* --------------------------------------------------------------------- */
    ///
    /// DocumentReader
    ///
    /// <summary>
    /// Provides functionality to read a PDF document.
    /// </summary>
    ///
    /// <remarks>
    /// iTextSharp を用いて PDF ファイルの解析を行います。
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public class DocumentReader : DocumentReaderBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// DocumentReader
        ///
        /// <summary>
        /// Initializes a new instance of the DocumentReader class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Path of the PDF file.</param>
        ///
        /* ----------------------------------------------------------------- */
        public DocumentReader(string src) : this(src, string.Empty) { }

        /* ----------------------------------------------------------------- */
        ///
        /// DocumentReader
        ///
        /// <summary>
        /// Initializes a new instance of the DocumentReader class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Path of the PDF file.</param>
        /// <param name="password">Password string.</param>
        ///
        /* ----------------------------------------------------------------- */
        public DocumentReader(string src, string password) :
            this(src, password, true, new IO()) { }

        /* ----------------------------------------------------------------- */
        ///
        /// DocumentReader
        ///
        /// <summary>
        /// Initializes a new instance of the DocumentReader class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Path of the PDF file.</param>
        /// <param name="query">Password query.</param>
        ///
        /* ----------------------------------------------------------------- */
        public DocumentReader(string src, IQuery<string> query) :
            this(src, query, false, true, new IO()) { }

        /* ----------------------------------------------------------------- */
        ///
        /// DocumentReader
        ///
        /// <summary>
        /// Initializes a new instance of the DocumentReader class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Path of the PDF file.</param>
        /// <param name="password">Password string.</param>
        /// <param name="io">I/O handler.</param>
        ///
        /* ----------------------------------------------------------------- */
        public DocumentReader(string src, string password, IO io) :
            this(src, password, true, io) { }

        /* ----------------------------------------------------------------- */
        ///
        /// DocumentReader
        ///
        /// <summary>
        /// Initializes a new instance of the DocumentReader class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Path of the PDF file.</param>
        /// <param name="query">Password query.</param>
        /// <param name="io">I/O handler.</param>
        ///
        /* ----------------------------------------------------------------- */
        public DocumentReader(string src, IQuery<string> query, IO io) :
            this(src, query, false, true, io) { }

        /* ----------------------------------------------------------------- */
        ///
        /// DocumentReader
        ///
        /// <summary>
        /// Initializes a new instance of the DocumentReader class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Path of the PDF file.</param>
        /// <param name="password">Password string.</param>
        /// <param name="partial">Partial reading mode.</param>
        /// <param name="io">I/O handler.</param>
        ///
        /* ----------------------------------------------------------------- */
        public DocumentReader(string src, string password, bool partial, IO io) :
            this(src, new QueryValue<string>(password), false, partial, io) { }

        /* ----------------------------------------------------------------- */
        ///
        /// DocumentReader
        ///
        /// <summary>
        /// Initializes a new instance of the DocumentReader class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Path of the PDF file.</param>
        /// <param name="password">Password query.</param>
        /// <param name="fullaccess">Requires full access.</param>
        /// <param name="partial">Partial reading mode.</param>
        /// <param name="io">I/O handler.</param>
        ///
        /* ----------------------------------------------------------------- */
        public DocumentReader(string src, IQuery<string> password,
            bool fullaccess, bool partial, IO io) : base(io)
        {
            _core = ReaderFactory.Create(src, password, fullaccess, partial, out var result);

            var f = io.GetPdfFile(src, result);
            f.Count      = _core.NumberOfPages;
            f.FullAccess = _core.IsOpenedWithFullPermissions;

            File        = f;
            Metadata    = _core.GetMetadata();
            Encryption  = _core.GetEncryption(f);
            Pages       = new ReadOnlyPageList(_core, f);
            Attachments = new AttachmentCollection(_core, f, IO);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// RawObject
        ///
        /// <summary>
        /// Gets the raw object.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public object RawObject => _core;

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases the unmanaged resources used by the DocumentReader
        /// and optionally releases the managed resources.
        /// </summary>
        ///
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        protected override void Dispose(bool disposing)
        {
            if (disposing) _core?.Dispose();
        }

        #endregion

        #region Fields
        private readonly PdfReader _core;
        #endregion
    }
}
