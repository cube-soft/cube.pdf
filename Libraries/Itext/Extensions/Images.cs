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
using Cube.Images.BuiltIn;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace Cube.Pdf.Itext.Images
{
    /* --------------------------------------------------------------------- */
    ///
    /// ImageExtension
    ///
    /// <summary>
    /// System.Drawing.Image の拡張用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class ImageExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Convert
        ///
        /// <summary>
        /// System.Drawing.Image オブジェクトを iTextSharp.text.Image
        /// オブジェクトに変換します。
        /// </summary>
        ///
        /// <param name="image">変換前のオブジェクト</param>
        ///
        /// <returns>変換後のオブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static iTextSharp.text.Image Convert(this Image image)
        {
            var scale  = 72.0 / image.HorizontalResolution;
            var format = image.GetImageFormat();
            if (!GetSupportFormats().Contains(format)) format = ImageFormat.Png;

            var dest = iTextSharp.text.Image.GetInstance(image, format);
            dest.SetAbsolutePosition(0, 0);
            dest.ScalePercent((float)(scale * 100.0));

            return dest;
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetSupportFormats
        ///
        /// <summary>
        /// サポートしている画像フォーマット一覧を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static HashSet<ImageFormat> GetSupportFormats() => _supports ?? (
            _supports = new HashSet<ImageFormat>
            {
                ImageFormat.Bmp,
                ImageFormat.Gif,
                ImageFormat.Jpeg,
                ImageFormat.Png,
                ImageFormat.Tiff,
            }
        );

        #endregion

        #region Fields
        private static HashSet<ImageFormat> _supports;
        #endregion
    }
}
