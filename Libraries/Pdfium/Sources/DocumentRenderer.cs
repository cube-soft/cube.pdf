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
    public class DocumentRenderer : DocumentReader, IDocumentRenderer
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// DocumentRenderer
        ///
        /// <summary>
        /// Initializes a new instance of the DocumentRenderer class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Path of the PDF file.</param>
        ///
        /* ----------------------------------------------------------------- */
        public DocumentRenderer(string src) : base(src) { }

        /* ----------------------------------------------------------------- */
        ///
        /// DocumentRenderer
        ///
        /// <summary>
        /// Initializes a new instance of the DocumentRenderer class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Path of the PDF file.</param>
        /// <param name="password">Password string.</param>
        ///
        /* ----------------------------------------------------------------- */
        public DocumentRenderer(string src, string password) : base(src, password) { }

        /* ----------------------------------------------------------------- */
        ///
        /// DocumentRenderer
        ///
        /// <summary>
        /// Initializes a new instance of the DocumentRenderer class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Path of the PDF file.</param>
        /// <param name="password">Password string.</param>
        /// <param name="options">Other options.</param>
        ///
        /* ----------------------------------------------------------------- */
        public DocumentRenderer(string src, string password, OpenOption options) :
            base(src, password, options) { }

        /* ----------------------------------------------------------------- */
        ///
        /// DocumentRenderer
        ///
        /// <summary>
        /// Initializes a new instance of the DocumentRenderer class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Path of the PDF file.</param>
        /// <param name="query">Password query.</param>
        ///
        /* ----------------------------------------------------------------- */
        public DocumentRenderer(string src, IQuery<string> query) : base(src, query) { }

        /* ----------------------------------------------------------------- */
        ///
        /// DocumentRenderer
        ///
        /// <summary>
        /// Initializes a new instance of the DocumentRenderer class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Path of the PDF file.</param>
        /// <param name="query">Password query.</param>
        /// <param name="options">Other options.</param>
        ///
        /* ----------------------------------------------------------------- */
        public DocumentRenderer(string src, IQuery<string> query, OpenOption options) :
            base(src, query, options) { }

        #endregion

        #region Properties

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
        public void Render(Graphics dest, Page2 page, PointF point, SizeF size) =>
            Core.Invoke(e => PdfiumRenderer.Render(e, dest, page.Number, point, size, page.Delta, RenderOption));

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
        public Image Render(Page2 page, SizeF size) =>
            Core.Invoke(e => PdfiumRenderer.Render(e, page.Number, size, page.Delta, RenderOption));

        #endregion
    }
}
