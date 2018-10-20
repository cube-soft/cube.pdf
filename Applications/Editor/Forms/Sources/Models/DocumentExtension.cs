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
using Cube.Pdf.Mixin;
using Cube.Xui.Converters;
using System.Drawing;
using System.Windows.Media;

namespace Cube.Pdf.App.Editor
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
        /// Create
        ///
        /// <summary>
        /// Create a new instance of the ImageSource class with the
        /// specified parameters.
        /// </summary>
        ///
        /// <param name="src">Renderer object.</param>
        /// <param name="page">Page object.</param>
        /// <param name="ratio">Scale ratio.</param>
        ///
        /// <returns>ImageSource object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static ImageSource Create(this IDocumentRenderer src, Page page, double ratio)
        {
            if (src == null || page == null) return null;
            var size = page.GetViewSize(ratio).Value;
            return src.Render(page, ratio).ToBitmapImage();
        }

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
            src?.Render(entry.RawObject, new SizeF(entry.Width, entry.Height)).ToBitmapImage();

        #endregion
    }
}
