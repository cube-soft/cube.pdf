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
using System.Diagnostics;
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
    public class DocumentReader : DocumentReaderBase, IDocumentRenderer
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
            this(src, password, new IO()) { }

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
        public DocumentReader(string src, IQuery<string, string> query) :
            this(src, query, new IO()) { }

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
            this(src, MakeQuery(null, password), false, io) { }

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
        /// <param name="query">Password query.</param>
        /// <param name="io">I/O handler.</param>
        ///
        /* ----------------------------------------------------------------- */
        public DocumentReader(string src, IQuery<string, string> query, IO io) :
            this(src, query, false, io) { }

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
        /// <param name="query">Password query.</param>
        /// <param name="fullaccess">Requires full access.</param>
        /// <param name="io">I/O handler.</param>
        ///
        /* ----------------------------------------------------------------- */
        public DocumentReader(string src,
            IQuery<string, string> query,
            bool fullaccess,
            IO io
        ) : this(src, MakeQuery(query, string.Empty), fullaccess, io) { }

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
        /// <param name="qv">Password query or string.</param>
        /// <param name="fullaccess">Requires full access.</param>
        /// <param name="io">I/O handler.</param>
        ///
        /* ----------------------------------------------------------------- */
        private DocumentReader(string src,
            QueryMessage<IQuery<string, string>, string> qv,
            bool fullaccess,
            IO io
        ) : base(io)
        {
            Debug.Assert(io != null);
            _core = PdfiumReader.Create(src, qv, fullaccess, io);
            Debug.Assert(_core != null);

            File        = _core.File;
            Pages       = _core.Pages;
            Metadata    = _core.Metadata;
            Encryption  = _core.Encryption;
            Attachments = null; // not implemented
        }

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
            _core.Render(dest, page, point, size, 0);

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
        public Image Render(Page page, SizeF size)
        {
            var dest = new Bitmap((int)size.Width, (int)size.Height);
            using (var gs = Graphics.FromImage(dest))
            {
                gs.Clear(Color.White);
                Render(gs, page, new PointF(0, 0), size);
            }
            return dest;
        }

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
        private static QueryMessage<IQuery<string, string>, string> MakeQuery(
            IQuery<string, string> query, string password) =>
            Query.NewMessage(query, password);

        #endregion

        #region Fields
        private readonly PdfiumReader _core;
        #endregion
    }
}
