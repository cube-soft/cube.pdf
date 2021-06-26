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
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Cube.Pdf.Itext
{
    /* --------------------------------------------------------------------- */
    ///
    /// WriterEngine
    ///
    /// <summary>
    /// Represents the components to save the PDF document.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal class WriterEngine
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// WriterEngine
        ///
        /// <summary>
        /// Initializes a new instance of the WriterEngine class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="doc">Document object.</param>
        /// <param name="writer">PdfWriter object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public WriterEngine(Document doc, PdfCopy writer)
        {
            Document = doc;
            Writer   = writer;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Document
        ///
        /// <summary>
        /// Gets the Document object.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Document Document { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Writer
        ///
        /// <summary>
        /// Gets the PdfCopy object.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PdfCopy Writer { get; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Close
        ///
        /// <summary>
        /// Gets the components.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Close()
        {
            Document.Close();
            Writer.Close();
        }

        #endregion
    }
}
