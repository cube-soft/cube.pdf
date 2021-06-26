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
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Cube.Pdf.Itext
{
    /* --------------------------------------------------------------------- */
    ///
    /// Writer
    ///
    /// <summary>
    /// Provides factory and other static methods about PdfWriter.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class Writer
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Creates new PDF writer objects.
        /// </summary>
        ///
        /// <param name="path">Path of the PDF document.</param>
        /// <param name="options">Save options.</param>
        /// <param name="metadata">PDF metadata.</param>
        ///
        /// <returns>Document and PdfCopy objects.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static WriterEngine Create(string path, SaveOption options, Metadata metadata)
        {
            var doc  = new Document();
            var dest = options.UseSmartCopy ?
                       new PdfSmartCopy(doc, options.IO.Create(path)) :
                       new PdfCopy(doc, options.IO.Create(path));

            dest.PdfVersion = metadata.Version.Minor.ToString()[0];
            dest.ViewerPreferences = (int)metadata.Options;

            return new(doc, dest);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Stamp
        ///
        /// <summary>
        /// Adds the specified information to the source PDF file and save
        /// to the specified path.
        /// </summary>
        ///
        /// <param name="path">Path to save the PDF file.</param>
        /// <param name="options">Save options.</param>
        /// <param name="src">Source PDF file.</param>
        /// <param name="metadata">PDF metadata.</param>
        /// <param name="encryption">PDF encryption settings.</param>
        /// <param name="bookmark">Bookmark information.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Stamp(string path, SaveOption options,
            string src, Metadata metadata, Encryption encryption, Bookmark bookmark)
        {
            using var r = Reader.FromPdf(src);
            using var e = new PdfStamper(r, options.IO.Create(path));

            e.Writer.Outlines = bookmark;
            e.Set(metadata, r.Info);
            e.Writer.Set(encryption);
            if (metadata.Version.Minor >= 5) e.SetFullCompression();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// Extracts the specified page from the specified source reader
        /// and saves to the specified path.
        /// </summary>
        ///
        /// <param name="path">Path to save the PDF file.</param>
        /// <param name="options">Save options.</param>
        /// <param name="src">PdfReader object.</param>
        /// <param name="pagenum">Page number to save.</param>
        /// <param name="metadata">PDF metadata.</param>
        /// <param name="encryption">PDF encryption settings.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Extract(string path, SaveOption options,
            IDisposable src, int pagenum, Metadata metadata, Encryption encryption)
        {
            var r = Reader.From(src);
            var e = Create(path, options, metadata);

            e.Writer.Set(encryption);
            e.Document.Open();
            e.Writer.AddPage(e.Writer.GetImportedPage(r, pagenum));
            e.Close();
        }

        #endregion
    }
}
