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
namespace Cube.Pdf.Ghostscript
{
    /* --------------------------------------------------------------------- */
    ///
    /// Device
    ///
    /// <summary>
    /// Ghostscript で変換可能なデバイスを定義した列挙型です。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public enum Device
    {
        /// <summary>PostScript (PS)</summary>
        PostScript,
        /// <summary>EPS</summary>
        Eps,
        /// <summary>PDF</summary>
        Pdf,
        /// <summary>JPEG</summary>
        Jpeg,
        /// <summary>JPEG（グレースケール）</summary>
        JpegGray,
        /// <summary>PNG</summary>
        Png,
        /// <summary>PNG（αビット）</summary>
        PngAlpha,
        /// <summary>PNG（256 色）</summary>
        Png256,
        /// <summary>PNG（16 色）</summary>
        Png16,
        /// <summary>PNG（グレースケール）</summary>
        PngGray,
        /// <summary>PNG（モノクロ）</summary>
        PngMono,
        /// <summary>ビットマップ</summary>
        Bitmap,
        /// <summary>ビットマップ（256 色）</summary>
        Bitmap256,
        /// <summary>ビットマップ（16 色）</summary>
        Bitmap16,
        /// <summary>ビットマップ（グレースケール）</summary>
        BitmapGray,
        /// <summary>ビットマップ（モノクロ）</summary>
        BitmapMono,
        /// <summary>TIFF</summary>
        Tiff,
        /// <summary>TIFF（グレースケール）</summary>
        TiffGray,
        /// <summary>TIFF（モノクロ）</summary>
        TiffMono,
    }
}
