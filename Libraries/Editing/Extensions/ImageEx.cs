/* ------------------------------------------------------------------------- */
///
/// ImageEx.cs
///
/// Copyright (c) 2010 CubeSoft, Inc. All rights reserved.
///
/// This program is free software: you can redistribute it and/or modify
/// it under the terms of the GNU Affero General Public License as published
/// by the Free Software Foundation, either version 3 of the License, or
/// (at your option) any later version.
///
/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
/// GNU Affero General Public License for more details.
///
/// You should have received a copy of the GNU Affero General Public License
/// along with this program.  If not, see <http://www.gnu.org/licenses/>.
///
/* ------------------------------------------------------------------------- */
using System.Drawing;
using System.Drawing.Imaging;

namespace Cube.Pdf.Editing.Extensions
{
    /* --------------------------------------------------------------------- */
    ///
    /// ImageEx
    /// 
    /// <summary>
    /// System.Drawing.Image の拡張用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class ImageEx
    {
        /* ----------------------------------------------------------------- */
        ///
        /// GuessImageFormat
        /// 
        /// <summary>
        /// ImageFormat を推測します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public static ImageFormat GuessImageFormat(this Image image)
        {
            return image.RawFormat.Equals(ImageFormat.Bmp)       ? ImageFormat.Bmp       :
                   image.RawFormat.Equals(ImageFormat.Emf)       ? ImageFormat.Emf       :
                   image.RawFormat.Equals(ImageFormat.Exif)      ? ImageFormat.Exif      :
                   image.RawFormat.Equals(ImageFormat.Gif)       ? ImageFormat.Gif       :
                   image.RawFormat.Equals(ImageFormat.Icon)      ? ImageFormat.Icon      :
                   image.RawFormat.Equals(ImageFormat.Jpeg)      ? ImageFormat.Jpeg      :
                   image.RawFormat.Equals(ImageFormat.MemoryBmp) ? ImageFormat.MemoryBmp :
                   image.RawFormat.Equals(ImageFormat.Png)       ? ImageFormat.Png       :
                   image.RawFormat.Equals(ImageFormat.Tiff)      ? ImageFormat.Tiff      :
                   image.RawFormat.Equals(ImageFormat.Wmf)       ? ImageFormat.Wmf       :
                                                                   ImageFormat.Bmp       ;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Rotate
        /// 
        /// <summary>
        /// イメージを degree 度だけ回転させます。
        /// </summary>
        /// 
        /// <remarks>
        /// RotateFlip メソッドは 90 度単位でしか回転させる事ができないので、
        /// 引数に指定された回転度数を 90 度単位で丸めています。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static void Rotate(this Image image, int degree)
        {
            var value = (degree >=  90 && degree < 180) ? RotateFlipType.Rotate90FlipNone  :
                        (degree >= 180 && degree < 270) ? RotateFlipType.Rotate180FlipNone :
                        (degree >= 270 && degree < 360) ? RotateFlipType.Rotate270FlipNone :
                                                          RotateFlipType.RotateNoneFlipNone;

            if (value != RotateFlipType.RotateNoneFlipNone) image.RotateFlip(value);
        }
    }
}
