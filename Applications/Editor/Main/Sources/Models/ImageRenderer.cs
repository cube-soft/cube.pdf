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
using System.Drawing;
using Cube.FileSystem;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// ImageRenderer
    ///
    /// <summary>
    /// Provides functionality to render the contents of an image file.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ImageRenderer : IDocumentRenderer
    {
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
        /// <remarks>
        /// The method does not be implemented.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public void Render(Graphics dest, Page page, PointF point, SizeF size) =>
            throw new NotImplementedException();

        /* ----------------------------------------------------------------- */
        ///
        /// Render
        ///
        /// <summary>
        /// Gets an Image object in which the Page content is rendered.
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
            using var ss = Io.Open(page.File.FullName);

            var src = Image.FromStream(ss);
            var ratio = Math.Min(size.Width / (double)src.Width, size.Height / (double)src.Height);
            if (ratio > 1.0) return src;

            try
            {
                var w = (int)(src.Width * ratio);
                var h = (int)(src.Height * ratio);
                var dest = new Bitmap(w, h);

                using var gs = Graphics.FromImage(dest);
                gs.DrawImage(src, 0, 0, w, h);
                return dest;
            }
            finally { src.Dispose(); }
        }

        #endregion
    }
}
