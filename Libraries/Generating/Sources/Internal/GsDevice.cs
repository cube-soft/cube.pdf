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
/// GsDevice
///
/// <summary>
/// Provides functionality to get the Ghostscript device name.
/// </summary>
///
/* ------------------------------------------------------------------------- */
internal static class GsDevice
{
    /* --------------------------------------------------------------------- */
    ///
    /// From
    ///
    /// <summary>
    /// Gets the device name from the specified file format.
    /// </summary>
    /// 
    /// <param name="src">File format.</param>
    /// 
    /// <returns>Device name.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public static string From(Format src) => src switch
    {
        Format.Text               => "txtwrite",
        Format.Ps                 => "ps2write",
        Format.Eps                => "eps2write",
        Format.Pdf                => "pdfwrite",
        Format.Psd                => "psdrgb",
        Format.PsdRgb             => "psdrgb",
        Format.PsdCmyk            => "psdcmyk",
        Format.PsdCmykog          => "psdcmykog",
        Format.Jpeg               => "jpeg",
        Format.Jpeg24bppRgb       => "jpeg",
        Format.Jpeg32bppCmyk      => "jpegcmyk",
        Format.Jpeg8bppGrayscale  => "jpeggray",
        Format.Png                => "png16m",
        Format.Png24bppRgb        => "png16m",
        Format.Png32bppArgb       => "png16malpha",
        Format.Png4bppIndexed     => "png16",
        Format.Png8bppIndexed     => "png256",
        Format.Png8bppGrayscale   => "pnggray",
        Format.Png1bppMonochrome  => "pngmonod",
        Format.Bmp                => "bmp16m",
        Format.Bmp24bppRgb        => "bmp16m",
        Format.Bmp32bppArgb       => "bmp32b",
        Format.Bmp4bppIndexed     => "bmp16",
        Format.Bmp8bppIndexed     => "bmp256",
        Format.Bmp8bppGrayscale   => "bmpgray",
        Format.Bmp1bppMonochrome  => "bmpmono",
        Format.Tiff               => "tiff24nc",
        Format.Tiff12bppRgb       => "tiff12nc",
        Format.Tiff24bppRgb       => "tiff24nc",
        Format.Tiff48bppRgb       => "tiff48nc",
        Format.Tiff32bppCmyk      => "tiff32nc",
        Format.Tiff64bppCmyk      => "tiff64nc",
        Format.Tiff8bppGrayscale  => "tiffgray",
        Format.Tiff1bppMonochrome => "tiffg4",
        _ => throw new System.NotImplementedException($"{src}"),
    };
}
