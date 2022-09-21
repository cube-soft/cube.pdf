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
using System.Drawing.Imaging;
using Cube.FileSystem;
using Cube.Pdf.Extensions;

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
            using var ss  = Io.Open(page.File.FullName);
            using var src = Image.FromStream(ss);

            var r = GetRatio(page, size);
            var w = (int)(src.Width  * r);
            var h = (int)(src.Height * r);
            var dest = new Bitmap(w, h);

            Select(src, page);
            using (var gs = Graphics.FromImage(dest)) gs.DrawImage(src, 0, 0, w, h);
            Rotate(dest, page.Rotation + page.Delta);
            return dest;
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetRatio
        ///
        /// <summary>
        /// Get the drawing size scale factor.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private double GetRatio(Page page, SizeF size)
        {
            var src = page.GetViewSize();
            var rw  = size.Width  / src.Width;
            var rh  = size.Height / src.Height;
            return Math.Min(rw, rh);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Select
        ///
        /// <summary>
        /// Selects the active frame.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Select(Image src, Page page)
        {
            if (page.Number <= 1) return;

            var dim   = new FrameDimension(src.FrameDimensionsList[0]);
            var max   = src.GetFrameCount(dim);
            var index = Math.Min(page.Number - 1, max);
            _ = src.SelectActiveFrame(dim, index);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Rotate
        ///
        /// <summary>
        /// Rotates the specified image.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Rotate(Image src, Angle angle)
        {
            var degree = angle.Degree;
            if (degree < 90) return;
            else if (degree is >=  90 and < 180) src.RotateFlip(RotateFlipType.Rotate90FlipNone);
            else if (degree is >= 180 and < 270) src.RotateFlip(RotateFlipType.Rotate180FlipNone);
            else if (degree is >= 270 and < 360) src.RotateFlip(RotateFlipType.Rotate270FlipNone);
        }

        #endregion
    }
}
