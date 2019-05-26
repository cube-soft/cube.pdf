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
using Cube.Images;
using Cube.Mixin.String;
using Cube.Pdf.Mixin;
using Cube.Xui.Converters;
using System;
using System.Drawing;
using System.Windows.Media;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// DocumentExtension
    ///
    /// <summary>
    /// Represents the extended methods to handle the PDF document.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class DocumentExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// IsPdf
        ///
        /// <summary>
        /// Gets the value indicating whether the specified file is PDF.
        /// </summary>
        ///
        /// <param name="src">File path.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static bool IsPdf(this string src) =>
            src.EndsWith(".pdf", StringComparison.InvariantCultureIgnoreCase);

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Create a new instance of the ImageSource class with the
        /// specified parameters.
        /// </summary>
        ///
        /// <param name="src">Renderer object.</param>
        /// <param name="entry">Information of the creating image.</param>
        ///
        /// <returns>ImageSource object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static ImageSource Create(this IDocumentRenderer src, ImageItem entry) =>
            src.Create(entry.RawObject, new SizeF(entry.Width, entry.Height));

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Create a new instance of the ImageSource class with the
        /// specified parameters.
        /// </summary>
        ///
        /// <param name="src">Renderer object.</param>
        /// <param name="page">Page object.</param>
        /// <param name="ratio">Scaling ratio.</param>
        ///
        /// <returns>ImageSource object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static ImageSource Create(this IDocumentRenderer src, Page page, double ratio) =>
            src.Create(page, page.GetViewSize(ratio).Value);

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Create a new instance of the ImageSource class with the
        /// specified parameters.
        /// </summary>
        ///
        /// <param name="src">Renderer object.</param>
        /// <param name="page">Page object.</param>
        /// <param name="size">Image size.</param>
        ///
        /// <returns>ImageSource object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static ImageSource Create(this IDocumentRenderer src, Page page, SizeF size) =>
            page.File is ImageFile f ? Create(f, size) : src?.Render(page, size).ToBitmapImage();

        /* ----------------------------------------------------------------- */
        ///
        /// GetItextReader
        ///
        /// <summary>
        /// Gets the DocumentReader of the specified file.
        /// </summary>
        ///
        /// <remarks>
        /// Partial モードは必ず無効にする必要があります。有効にした場合、
        /// ページ回転情報が正常に適用されない可能性があります。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static Itext.DocumentReader GetItexReader(this Information src, IQuery<string, string> query, IO io)
        {
            var pass = (src as PdfFile)?.Password;
            return pass.HasValue() ?
                   new Itext.DocumentReader(src.FullName, pass, false, io) :
                   new Itext.DocumentReader(src.FullName, query, true, false, io);
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Create a new instance of the ImageSource class with the
        /// specified parameters.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static ImageSource Create(ImageFile src, SizeF size)
        {
            using (var obj = new ImageResizer(src.FullName)
            {
                ResizeMode          = ImageResizeMode.HighSpeed,
                PreserveAspectRatio = true,
                LongSide            = (int)Math.Max(size.Width, size.Height),
            }) return obj.Resized.ToBitmapImage(false);
        }

        #endregion
    }
}
