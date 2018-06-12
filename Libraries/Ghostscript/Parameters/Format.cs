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
using System.Collections.Generic;
using System.Diagnostics;

namespace Cube.Pdf.Ghostscript
{
    /* --------------------------------------------------------------------- */
    ///
    /// Format
    ///
    /// <summary>
    /// Ghostscript で変換可能なフォーマットを定義した列挙型です。
    /// </summary>
    ///
    /// <remarks>
    /// Psd, Jpeg, Png, Bmp, Tiff は、各カテゴリにおける最も代表的な
    /// フォーマットのエイリアスとなります。
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public enum Format
    {
        /// <summary>Text</summary>
        Text,
        /// <summary>PostScript (PS)</summary>
        Ps,
        /// <summary>EPS</summary>
        Eps,
        /// <summary>PDF</summary>
        Pdf,
        /// <summary>PSD that is equal to PsdRgb</summary>
        Psd,
        /// <summary>PSD (RGB)</summary>
        PsdRgb,
        /// <summary>PSD (CMYK)</summary>
        PsdCmyk,
        /// <summary>PSD (CMYK, Orange, and Green)</summary>
        PsdCmykog,
        /// <summary>JPEG that is equal to Jpeg24bppRGB</summary>
        Jpeg,
        /// <summary>JPEG (24bpp RGB)</summary>
        Jpeg24bppRgb,
        /// <summary>JPEG (32bpp CMYK)</summary>
        Jpeg32bppCmyk,
        /// <summary>JPEG (8bpp Grayscale)</summary>
        Jpeg8bppGrayscale,
        /// <summary>PNG that is equal to Png24bppRGB</summary>
        Png,
        /// <summary>PNG (24bpp RGB)</summary>
        Png24bppRgb,
        /// <summary>PNG (32bpp ARGB)</summary>
        Png32bppArgb,
        /// <summary>PNG (16 colors)</summary>
        Png4bppIndexed,
        /// <summary>PNG (256 colors)</summary>
        Png8bppIndexed,
        /// <summary>PNG (8bpp Grayscale)</summary>
        Png8bppGrayscale,
        /// <summary>PNG (Monochrome)</summary>
        Png1bppMonochrome,
        /// <summary>BMP that is equal to Bmp24bppRgb</summary>
        Bmp,
        /// <summary>BMP (24bpp RGB)</summary>
        Bmp24bppRgb,
        /// <summary>BMP (32bpp RGB)</summary>
        Bmp32bppArgb,
        /// <summary>BMP (16 colors)</summary>
        Bmp4bppIndexed,
        /// <summary>BMP (256 colors)</summary>
        Bmp8bppIndexed,
        /// <summary>BMP (8bpp Grayscale)</summary>
        Bmp8bppGrayscale,
        /// <summary>BMP (Monochrome)</summary>
        Bmp1bppMonochrome,
        /// <summary>TIFF that is equal to Tiff24bppRgb</summary>
        Tiff,
        /// <summary>TIFF (12bpp RGB)</summary>
        Tiff12bppRgb,
        /// <summary>TIFF (24bpp RGB)</summary>
        Tiff24bppRgb,
        /// <summary>TIFF (48bpp RGB)</summary>
        Tiff48bppRgb,
        /// <summary>TIFF (32bpp CMYK)</summary>
        Tiff32bppCmyk,
        /// <summary>TIFF (64bpp CMYK)</summary>
        Tiff64bppCmyk,
        /// <summary>TIFF (8bpp Grayscale)</summary>
        Tiff8bppGrayscale,
        /// <summary>TIFF (Monochrome)</summary>
        Tiff1bppMonochrome,
    }

    /* --------------------------------------------------------------------- */
    ///
    /// FormatExtension
    ///
    /// <summary>
    /// Format の拡張用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class FormatExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetArgument
        ///
        /// <summary>
        /// Format を表す Argument オブジェクトを取得します。
        /// </summary>
        ///
        /// <param name="src">Format</param>
        ///
        /// <returns>Argument オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Argument GetArgument(this Format src)
        {
            var result = GetFormatMap().TryGetValue(src, out var value);
            Debug.Assert(result);
            return new Argument('s', "DEVICE", value.Device);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetExtension
        ///
        /// <summary>
        /// Format を表す拡張子を取得します。
        /// </summary>
        ///
        /// <param name="src">Format</param>
        ///
        /// <returns>拡張子</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static string GetExtension(this Format src)
        {
            var result = GetFormatMap().TryGetValue(src, out var value);
            Debug.Assert(result);
            return value.Extension;
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetFormatMap
        ///
        /// <summary>
        /// Format と関連情報の対応一覧を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static IDictionary<Format, FormatInfo> GetFormatMap() => _map ?? (
            _map = new Dictionary<Format, FormatInfo>
            {
                { Format.Text,               new FormatInfo("txtwrite",  ".txt")  },
                { Format.Ps,                 new FormatInfo("ps2write",  ".ps")   },
                { Format.Eps,                new FormatInfo("eps2write", ".eps")  },
                { Format.Pdf,                new FormatInfo("pdfwrite",  ".pdf")  },
                { Format.Psd,                new FormatInfo("psdrgb",    ".psd")  },
                { Format.PsdRgb,             new FormatInfo("psdrgb",    ".psd")  },
                { Format.PsdCmyk,            new FormatInfo("psdcmyk",   ".psd")  },
                { Format.PsdCmykog,          new FormatInfo("psdcmykog", ".psd")  },
                { Format.Jpeg,               new FormatInfo("jpeg",      ".jpg")  },
                { Format.Jpeg24bppRgb,       new FormatInfo("jpeg",      ".jpg")  },
                { Format.Jpeg32bppCmyk,      new FormatInfo("jpegcmyk",  ".jpg")  },
                { Format.Jpeg8bppGrayscale,  new FormatInfo("jpeggray",  ".jpg")  },
                { Format.Png,                new FormatInfo("png16m",    ".png")  },
                { Format.Png24bppRgb,        new FormatInfo("png16m",    ".png")  },
                { Format.Png32bppArgb,       new FormatInfo("pngalpha",  ".png")  },
                { Format.Png4bppIndexed,     new FormatInfo("png16",     ".png")  },
                { Format.Png8bppIndexed,     new FormatInfo("png256",    ".png")  },
                { Format.Png8bppGrayscale,   new FormatInfo("pnggray",   ".png")  },
                { Format.Png1bppMonochrome,  new FormatInfo("pngmonod",  ".png")  },
                { Format.Bmp,                new FormatInfo("bmp16m",    ".bmp")  },
                { Format.Bmp24bppRgb,        new FormatInfo("bmp16m",    ".bmp")  },
                { Format.Bmp32bppArgb,       new FormatInfo("bmp32b",    ".bmp")  },
                { Format.Bmp4bppIndexed,     new FormatInfo("bmp16",     ".bmp")  },
                { Format.Bmp8bppIndexed,     new FormatInfo("bmp256",    ".bmp")  },
                { Format.Bmp8bppGrayscale,   new FormatInfo("bmpgray",   ".bmp")  },
                { Format.Bmp1bppMonochrome,  new FormatInfo("bmpmono",   ".bmp")  },
                { Format.Tiff,               new FormatInfo("tiff24nc",  ".tiff") },
                { Format.Tiff12bppRgb,       new FormatInfo("tiff12nc",  ".tiff") },
                { Format.Tiff24bppRgb,       new FormatInfo("tiff24nc",  ".tiff") },
                { Format.Tiff48bppRgb,       new FormatInfo("tiff48nc",  ".tiff") },
                { Format.Tiff32bppCmyk,      new FormatInfo("tiff32nc",  ".tiff") },
                { Format.Tiff64bppCmyk,      new FormatInfo("tiff64nc",  ".tiff") },
                { Format.Tiff8bppGrayscale,  new FormatInfo("tiffgray",  ".tiff") },
                { Format.Tiff1bppMonochrome, new FormatInfo("tifflzw",   ".tiff") },
            }
        );

        /* ----------------------------------------------------------------- */
        ///
        /// FormatInfo
        ///
        /// <summary>
        /// Format の関連情報を保持するための内部クラスです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private class FormatInfo
        {
            public FormatInfo(string dev, string ext)
            {
                Device    = dev;
                Extension = ext;
            }

            public string Device { get; }
            public string Extension { get; }
        }

        #endregion

        #region Fields
        private static IDictionary<Format, FormatInfo> _map;
        #endregion
    }
}
