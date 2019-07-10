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
using System.Collections.Generic;
using System.Drawing;

namespace Cube.Pdf.Pdfium
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
    public class DocumentReader : DisposableBase, IDocumentReader, IDocumentRenderer
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
            this(src, password, new OpenOption()) { }

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
            this(src, query, new OpenOption()) { }

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
        /// <param name="options">Other options.</param>
        ///
        /* ----------------------------------------------------------------- */
        public DocumentReader(string src, string password, OpenOption options) :
            this(src, MakeQuery(null, password), options) { }

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
        /// <param name="options">Other options.</param>
        ///
        /* ----------------------------------------------------------------- */
        public DocumentReader(string src, IQuery<string> query, OpenOption options) :
            this(src, MakeQuery(query, string.Empty), options) { }

        /* ----------------------------------------------------------------- */
        ///
        /// DocumentReader
        ///
        /// <summary>
        /// Initializes a new instance of the DocumentReader class with
        /// the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Path of the PDF file.</param>
        /// <param name="query">Password query or string.</param>
        /// <param name="options">Other options.</param>
        ///
        /* ----------------------------------------------------------------- */
        private DocumentReader(string src,
            QueryMessage<IQuery<string>, string> query,
            OpenOption options
        ) { _core = PdfiumReader.Create(src, query, options); }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// File
        ///
        /// <summary>
        /// Gets information of the provided PDF file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public File File => _core.File;

        /* ----------------------------------------------------------------- */
        ///
        /// Metadata
        ///
        /// <summary>
        /// Gets the PDF metadata.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Metadata Metadata => _core.Metadata;

        /* ----------------------------------------------------------------- */
        ///
        /// Encryption
        ///
        /// <summary>
        /// Gets the encryption information of the PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Encryption Encryption => _core.Encryption;

        /* ----------------------------------------------------------------- */
        ///
        /// Pages
        ///
        /// <summary>
        /// Gets the page collection.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<Page> Pages => _core.Pages;

        /* ----------------------------------------------------------------- */
        ///
        /// Attachments
        ///
        /// <summary>
        /// Gets the attachment collection.
        /// </summary>
        ///
        /// <remarks>
        /// Currently not supported.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<Attachment> Attachments { get; } = new Attachment[0];

        /* ----------------------------------------------------------------- */
        ///
        /// RenderOption
        ///
        /// <summary>
        /// Gets or sets the rendering options.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RenderOption RenderOption { get; set; } = new RenderOption();

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Render
        ///
        /// <summary>
        /// Render the Page content to the Graphics object with the
        /// specified parameters
        /// </summary>
        ///
        /// <param name="dest">Graphics object.</param>
        /// <param name="page">Page object.</param>
        /// <param name="point">Start point to render.</param>
        /// <param name="size">Rendering size.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Render(Graphics dest, Page page, PointF point, SizeF size) =>
            _core.Render(dest, page, point, size, RenderOption);

        /* ----------------------------------------------------------------- */
        ///
        /// Render
        ///
        /// <summary>
        /// Get an Image object in which the Page content is rendered.
        /// </summary>
        ///
        /// <param name="page">Page object.</param>
        /// <param name="size">Rendering size.</param>
        ///
        /// <returns>Image object</returns>
        ///
        /* ----------------------------------------------------------------- */
        public Image Render(Page page, SizeF size) => _core.Render(page, size, RenderOption);

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
        protected override void Dispose(bool disposing) => _core?.Dispose();

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// MakeQuery
        ///
        /// <summary>
        /// Creates a password query and string.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static QueryMessage<IQuery<string>, string> MakeQuery(
            IQuery<string> query, string password) =>
            Query.NewMessage(query, password);

        #endregion

        #region Fields
        private readonly PdfiumReader _core;
        #endregion
    }
}
