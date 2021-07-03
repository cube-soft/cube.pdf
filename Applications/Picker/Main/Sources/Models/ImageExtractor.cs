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
using System.Collections.Generic;
using System.Drawing;
using Cube.Pdf.Itext;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace Cube.Pdf.Picker
{
    /* --------------------------------------------------------------------- */
    ///
    /// ImageExtractor
    ///
    /// <summary>
    /// Provides extended methods to extract embedded images.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ImageExtractor : DocumentReader
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ImageExtractor
        ///
        /// <summary>
        /// Creates a new instance of the ImageExtractor class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="src">Path of the PDF file.</param>
        /// <param name="query">Password query.</param>
        /// <param name="options">Open options.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ImageExtractor(string src, IQuery<string> query, OpenOption options) :
            base(src, query, options) { }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetEmbeddedImages
        ///
        /// <summary>
        /// Gets the collection of embedded images in the specified page.
        /// </summary>
        ///
        /// <param name="pagenum">Page number.</param>
        ///
        /// <returns>Collection of embedded images.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<Image> GetEmbeddedImages(int pagenum) =>
            new PdfReaderContentParser(Core as PdfReader).ProcessContent(pagenum, new RenderListener());

        #endregion
    }
}
