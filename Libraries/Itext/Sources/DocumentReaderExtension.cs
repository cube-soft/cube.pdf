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
using iTextSharp.text.pdf;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Cube.Pdf.Itext
{
    /* --------------------------------------------------------------------- */
    ///
    /// DocumentReaderExtension
    ///
    /// <summary>
    /// Provides extended methods of the DocumentReader class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class DocumentReaderExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetEmbeddedImages
        ///
        /// <summary>
        /// Gets the collection of embedded images in the specified page.
        /// </summary>
        ///
        /// <param name="src">Document reader.</param>
        /// <param name="pagenum">Page number.</param>
        ///
        /// <returns>Collection of embedded images.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static IEnumerable<Image> GetEmbeddedImages(this DocumentReader src, int pagenum)
        {
            var core = src.RawObject as PdfReader;
            Debug.Assert(core != null);
            var dest = new EmbeddedImageCollection();
            core.GetContentParser().ProcessContent(pagenum, dest);
            return dest;
        }

        #endregion
    }
}
