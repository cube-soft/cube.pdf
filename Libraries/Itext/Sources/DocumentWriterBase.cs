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
using Cube.FileSystem;

namespace Cube.Pdf.Itext
{
    /* --------------------------------------------------------------------- */
    ///
    /// DocumentWriterBase
    ///
    /// <summary>
    /// Provides an implementation of the IDocumentWriter interface by
    /// using the iTextSharp.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public abstract class DocumentWriterBase : DisposableBase, IDocumentWriter
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// DocumentWriterBase
        ///
        /// <summary>
        /// Initializes a new instance of the DocumentWriterBase class
        /// with the specified options.
        /// </summary>
        ///
        /// <param name="options">Saving options.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected DocumentWriterBase(SaveOption options) { Options = options; }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Pages
        ///
        /// <summary>
        /// Gets the collection of pages.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected IEnumerable<Page> Pages => _pages;

        /* ----------------------------------------------------------------- */
        ///
        /// Attachments
        ///
        /// <summary>
        /// Gets the collection of attached files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected IEnumerable<Attachment> Attachments => _attachments;

        /* ----------------------------------------------------------------- */
        ///
        /// Metadata
        ///
        /// <summary>
        /// Gets the PDF metadata.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected Metadata Metadata { get; private set; } = new();

        /* ----------------------------------------------------------------- */
        ///
        /// Encryption
        ///
        /// <summary>
        /// Gets the encryption settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected Encryption Encryption { get; private set; } = new();

        /* ----------------------------------------------------------------- */
        ///
        /// Options
        ///
        /// <summary>
        /// Gets the options when saving the PDF file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected SaveOption Options { get; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Reset
        ///
        /// <summary>
        /// Resets values.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Reset() => OnReset();

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// Saves the document to the specified path.
        /// </summary>
        ///
        /// <param name="path">Path to save.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Save(string path) => OnSave(path);

        /* ----------------------------------------------------------------- */
        ///
        /// Add
        ///
        /// <summary>
        /// Adds pages to the document.
        /// </summary>
        ///
        /// <param name="pages">Collection of pages.</param>
        ///
        /// <remarks>
        /// Use the Add(IEnumerable{Page}, IDocumentReader) method to specify
        /// the DocumentReader.Pages object.
        /// </remarks>
        ///
        /// <see cref="Add(IEnumerable{Page}, IDocumentReader)"/>
        ///
        /* ----------------------------------------------------------------- */
        public void Add(IEnumerable<Page> pages) => _pages.AddRange(pages);

        /* ----------------------------------------------------------------- */
        ///
        /// Add
        ///
        /// <summary>
        /// Adds pages to the document.
        /// </summary>
        ///
        /// <param name="pages">Collection of pages.</param>
        /// <param name="hint">
        /// Document reader object to get more detailed information about
        /// the specified pages.
        /// </param>
        ///
        /// <remarks>
        /// The ownership of the IDocumentReader object will be transferred
        /// to this class, and Dispose will be executed automatically.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public void Add(IEnumerable<Page> pages, IDocumentReader hint)
        {
            Add(pages);
            Bind(hint);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Add
        ///
        /// <summary>
        /// Adds attached objects to the document.
        /// </summary>
        ///
        /// <param name="files">Collection of attached files.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Add(IEnumerable<Attachment> files) => _attachments.AddRange(files);

        /* ----------------------------------------------------------------- */
        ///
        /// Set
        ///
        /// <summary>
        /// Sets the PDF metadata.
        /// </summary>
        ///
        /// <param name="src">PDF metadata.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Set(Metadata src) => Metadata = src;

        /* ----------------------------------------------------------------- */
        ///
        /// Set
        ///
        /// <summary>
        /// Sets the encryption settings.
        /// </summary>
        ///
        /// <param name="src">Encryption settings.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Set(Encryption src) => Encryption = src;

        /* ----------------------------------------------------------------- */
        ///
        /// OnSave
        ///
        /// <summary>
        /// Executes the save operation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected abstract void OnSave(string path);

        /* ----------------------------------------------------------------- */
        ///
        /// OnReset
        ///
        /// <summary>
        /// Executes the reset operation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnReset()
        {
            _pages.Clear();
            _attachments.Clear();
            Set(new Metadata());
            Set(new Encryption());
            Release();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Bind
        ///
        /// <summary>
        /// Binds the specified document reader to the class.
        /// </summary>
        ///
        /// <param name="src">Document reader.</param>
        ///
        /// <remarks>
        /// The specified DocumentReader object will transfer ownership to
        /// the DocumentWriter object, and processes such as Dispose will be
        /// executed automatically.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        protected void Bind(IDocumentReader src)
        {
            if (_resources.Contains(src)) return;
            _resources.Add(src);

            if (src is DocumentReader itext)
            {
                var k = itext.File.FullName;
                var v = itext.Core;

                if (v != null && !_hints.ContainsKey(k)) _hints.Add(k, v);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Release
        ///
        /// <summary>
        /// Releases all bound objects.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected void Release()
        {
            _hints.Clear();
            foreach (var obj in _resources) obj.Dispose();
            _resources.Clear();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetRawReader
        ///
        /// <summary>
        /// Gets the PdfReader corresponding to the specified Page.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected IDisposable GetRawReader(Page src) =>
            src.File is PdfFile   pdf ? GetRawReader(pdf) :
            src.File is ImageFile img ? GetRawReader(img) : null;

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases the unmanaged resources used by the object
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
            if (disposing) Release();
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetRawReader
        ///
        /// <summary>
        /// Gets the PdfReader corresponding to the specified PDF file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private IDisposable GetRawReader(PdfFile src)
        {
            var key = src.FullName;
            if (_hints.TryGetValue(key, out var hit)) return hit;

            var options = new OpenOption { IO = Options.IO, SaveMemory = false };
            var reader  = new DocumentReader(key, src.Password, options);
            _resources.Add(reader);
            _hints.Add(key, reader.Core);

            return reader.Core;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetRawReader
        ///
        /// <summary>
        /// Gets the PdfReader corresponding to the specified information.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private IDisposable GetRawReader(ImageFile src)
        {
            var key = src.FullName;
            if (_hints.TryGetValue(key, out var hit)) return hit;

            var dest = ReaderFactory.FromImage(key, Options.IO);
            _resources.Add(dest);
            _hints.Add(key, dest);

            return dest;
        }

        #endregion

        #region Fields
        private readonly List<Page> _pages = new();
        private readonly List<Attachment> _attachments = new();
        private readonly List<IDisposable> _resources = new();
        private readonly Dictionary<string, IDisposable> _hints = new();
        #endregion
    }
}
