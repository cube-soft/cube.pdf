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
namespace Cube.Pdf.Ghostscript;

/* ------------------------------------------------------------------------- */
///
/// Format
///
/// <summary>
/// Specifies formats that Ghostscript can convert.
/// </summary>
///
/// <remarks>
/// Psd, Jpeg, Png, Bmp, and Tiff represent the most common format
/// aliases in each category.
/// </remarks>
///
/* ------------------------------------------------------------------------- */
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
