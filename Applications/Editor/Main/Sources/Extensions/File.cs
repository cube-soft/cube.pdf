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
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Cube.FileSystem;
using Cube.Icons;
using Cube.Mixin.Drawing;
using Cube.Mixin.Generic;
using Cube.Mixin.String;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// File
    ///
    /// <summary>
    /// Represents the extended methods of files.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class File
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetFiles
        ///
        /// <summary>
        /// Gets the files of the specified event args.
        /// </summary>
        ///
        /// <param name="src">Event arguments.</param>
        ///
        /// <returns>collection of files.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static string[] GetFiles(this DragEventArgs src) =>
            src.Data.GetDataPresent(DataFormats.FileDrop) ?
            src.Data.GetData(DataFormats.FileDrop).TryCast<string[]>() :
            null;

        /* ----------------------------------------------------------------- */
        ///
        /// FirstPdf
        ///
        /// <summary>
        /// Gets the first PDF file of the specified collection
        /// </summary>
        ///
        /// <param name="src">Collection of paths.</param>
        ///
        /// <returns>Path of the PDF file.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static string FirstPdf(this IEnumerable<string> src) =>
            src?.FirstOrDefault(e => e.EndsWith(".pdf", StringComparison.InvariantCultureIgnoreCase));

        /* ----------------------------------------------------------------- */
        ///
        /// IsImageFile
        ///
        /// <summary>
        /// Determines whether the specified file is bitmap image format.
        /// </summary>
        ///
        /// <param name="src">Path of file.</param>
        ///
        /// <returns>true for bitmap image file; otherwise false.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static bool IsImageFile(this string src) =>
            ImageFormats.Any(e => src.EndsWith(e, StringComparison.InvariantCultureIgnoreCase));

        /* ----------------------------------------------------------------- */
        ///
        /// GetItext
        ///
        /// <summary>
        /// Creates a new instance of the DocumentReader class with the
        /// specified file.
        /// </summary>
        ///
        /// <param name="src">Source file.</param>
        /// <param name="query">Password query.</param>
        /// <param name="partial">
        /// Value indicating whether to apply the partial mode.
        /// Note that you must set to false if you use the created
        /// reader in a DocumentWriter object.
        /// </param>
        ///
        /// <returns>DocumentReader object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static IDocumentReader GetItext(this Entity src, IQuery<string> query, bool partial)
        {
            var pass    = (src as PdfFile)?.Password;
            var options = new Itext.OpenOption
            {
                FullAccess = !pass.HasValue(),
                SaveMemory = partial,
            };

            return pass.HasValue() ?
                   new Itext.DocumentReader(src.FullName, pass, options) :
                   new Itext.DocumentReader(src.FullName, query, options);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetPdfium
        ///
        /// <summary>
        /// Casts the DocumentRenderer object.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static Pdfium.DocumentRenderer GetPdfium(this IDocumentRenderer src)
        {
            if (src is Pdfium.DocumentRenderer dest) return dest;
            typeof(OpenExtension).LogWarn("IDocumentRenderer to PDFium failed");
            return default;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetIconSource
        ///
        /// <summary>
        /// Creates a icon from the specified arguments.
        /// </summary>
        ///
        /// <param name="src">File information.</param>
        /// <param name="size">Icon size.</param>
        ///
        /// <returns>ImageSource of the requested icon.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static ImageSource GetIconSource(this Entity src, IconSize size) =>
            FileIcon.GetImage(src?.FullName, size).ToBitmapImage(true);

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// ImageFormats
        ///
        /// <summary>
        /// Gets the extensions of bitmap image files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static IEnumerable<string> ImageFormats { get; } = new[]
        {
            ".png", ".jpg", ".jpeg", ".bmp", ".tiff", ".tif",
        };

        #endregion
    }
}
