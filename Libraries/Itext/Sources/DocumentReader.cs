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
using System;
using System.Collections.Generic;

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
    /* --------------------------------------------------------------------- */
    public class DocumentReader : DisposableBase, IDocumentReader
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
        public DocumentReader(string src, string password) : this(src, password, new()) { }

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
        /// <param name="options">Open options.</param>
        ///
        /* ----------------------------------------------------------------- */
        public DocumentReader(string src, string password, OpenOption options) :
            this(src, new Password(null, password), options) { }

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
        public DocumentReader(string src, IQuery<string> query) : this(src, query, new()) { }

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
        /// <param name="options">Open options.</param>
        ///
        /* ----------------------------------------------------------------- */
        public DocumentReader(string src, IQuery<string> query, OpenOption options) :
            this(src, new Password(query, string.Empty), options) { }

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
        /// <param name="password">Password query or string.</param>
        /// <param name="options">Open options.</param>
        ///
        /* ----------------------------------------------------------------- */
        private DocumentReader(string src, Password password, OpenOption options)
        {
            var obj  = Reader.FromPdf(src, password, options);
            var file = obj.GetFile(src, password.Value);

            Options     = options;
            Core        = obj;
            File        = file;
            Metadata    = obj.GetMetadata();
            Encryption  = obj.GetEncryption(file);
            Pages       = new PageCollection(obj, file);
            Attachments = new AttachmentCollection(obj, file);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// File
        ///
        /// <summary>
        /// Gets the information of the target file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public File File { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Metadata
        ///
        /// <summary>
        /// Gets the PDF metadata.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Metadata Metadata { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Encryption
        ///
        /// <summary>
        /// Gets the encryption information of the PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Encryption Encryption { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Pages
        ///
        /// <summary>
        /// Gets the page collection.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<Page> Pages { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Attachments
        ///
        /// <summary>
        /// Gets the attachment collection.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<Attachment> Attachments { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Options
        ///
        /// <summary>
        /// Get the options when the provided PDF file is opened.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected OpenOption Options { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Core
        ///
        /// <summary>
        /// Gets the core object.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected internal IDisposable Core { get; }

        #endregion

        #region Implementations

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
            if (disposing) Core?.Dispose();
        }

        #endregion
    }
}
