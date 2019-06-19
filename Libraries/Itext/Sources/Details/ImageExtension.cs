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
using Cube.Mixin.Drawing;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace Cube.Pdf.Itext
{
    /* --------------------------------------------------------------------- */
    ///
    /// ImageExtension
    ///
    /// <summary>
    /// Provides extended methods of the image classes.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class ImageExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetItextImage
        ///
        /// <summary>
        /// Converts from System.Drawing.Image to iTextSharp.text.Image.
        /// </summary>
        ///
        /// <param name="image">Source image.</param>
        ///
        /// <returns>Converted image.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static iTextSharp.text.Image GetItextImage(this Image image)
        {
            var scale  = PdfFile.Point / image.HorizontalResolution;
            var format = image.GetImageFormat();
            if (!SupportFormats.Contains(format)) format = ImageFormat.Png;

            var dest = iTextSharp.text.Image.GetInstance(image, format);
            dest.SetAbsolutePosition(0, 0);
            dest.ScalePercent(scale * 100);

            return dest;
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// SupportFormats
        ///
        /// <summary>
        /// Gets the collection of supported image formats.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static HashSet<ImageFormat> SupportFormats { get; } = new HashSet<ImageFormat>
        {
            ImageFormat.Bmp,
            ImageFormat.Gif,
            ImageFormat.Jpeg,
            ImageFormat.Png,
            ImageFormat.Tiff,
        };

        #endregion
    }
}
