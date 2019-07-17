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
using Cube.FileSystem;
using Cube.Images.Icons;
using Cube.Mixin.Drawing;
using Cube.Mixin.String;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

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
            src.FirstOrDefault(e => e.IsPdf());

        /* ----------------------------------------------------------------- */
        ///
        /// IsPdf
        ///
        /// <summary>
        /// Determines whether the specified file is PDF.
        /// </summary>
        ///
        /// <param name="src">Path of file.</param>
        ///
        /// <returns>true for PDF; otherwise false.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static bool IsPdf(this string src) =>
            src.EndsWith(".pdf", StringComparison.InvariantCultureIgnoreCase);

        /* ----------------------------------------------------------------- */
        ///
        /// GetItexReader
        ///
        /// <summary>
        /// Creates a new instance of the DocumentReader class with the
        /// specified file.
        /// </summary>
        ///
        /// <param name="src">Source file.</param>
        /// <param name="query">Password query.</param>
        /// <param name="io">I/O handler.</param>
        ///
        /// <returns>DocumentReader object.</returns>
        ///
        /// <remarks>
        /// Partial モードは必ず無効にする必要があります。有効にした場合、
        /// ページ回転情報が正常に適用されない可能性があります。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static Itext.DocumentReader GetItexReader(this Entity src, IQuery<string> query, IO io)
        {
            var pass    = (src as PdfFile)?.Password;
            var options = new Itext.OpenOption
            {
                IO           = io,
                FullAccess   = !pass.HasValue(),
                ReduceMemory = false,
            };

            return pass.HasValue() ?
                   new Itext.DocumentReader(src.FullName, pass, options) :
                   new Itext.DocumentReader(src.FullName, query, options);
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
            src.GetIcon(size)?.ToBitmap().ToBitmapImage(true);

        #endregion
    }
}
