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
namespace Cube.Pdf.Converter;

using System.Collections.Generic;
using Cube.Pdf.Ghostscript;

/* ------------------------------------------------------------------------- */
///
/// FormatGroup
///
/// <summary>
/// Provides functionality to normalize the format value.
/// </summary>
///
/* ------------------------------------------------------------------------- */
internal static class FormatGroup
{
    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// Represent
    ///
    /// <summary>
    /// Gets a representative value for the group to which the specified
    /// value belongs.
    /// </summary>
    ///
    /// <param name="src">File format.</param>
    ///
    /// <returns>Representative value.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public static Format Represent(Format src) => src switch
    {
        Format.Png1bppMonochrome    => Format.Png,
        Format.Png24bppRgb          => Format.Png,
        Format.Png32bppArgb         => Format.Png,
        Format.Png4bppIndexed       => Format.Png,
        Format.Png8bppGrayscale     => Format.Png,
        Format.Png8bppIndexed       => Format.Png,
        Format.Jpeg24bppRgb         => Format.Jpeg,
        Format.Jpeg32bppCmyk        => Format.Jpeg,
        Format.Jpeg8bppGrayscale    => Format.Jpeg,
        Format.Bmp1bppMonochrome    => Format.Bmp,
        Format.Bmp24bppRgb          => Format.Bmp,
        Format.Bmp32bppArgb         => Format.Bmp,
        Format.Bmp4bppIndexed       => Format.Bmp,
        Format.Bmp8bppGrayscale     => Format.Bmp,
        Format.Bmp8bppIndexed       => Format.Bmp,
        Format.Tiff12bppRgb         => Format.Tiff,
        Format.Tiff1bppMonochrome   => Format.Tiff,
        Format.Tiff24bppRgb         => Format.Tiff,
        Format.Tiff32bppCmyk        => Format.Tiff,
        Format.Tiff48bppRgb         => Format.Tiff,
        Format.Tiff64bppCmyk        => Format.Tiff,
        Format.Tiff8bppGrayscale    => Format.Tiff,
        _ => src,
    };

    /* --------------------------------------------------------------------- */
    ///
    /// Lookup
    ///
    /// <summary>
    /// Gets the format value corresponding to the specified format and
    /// color mode.
    /// </summary>
    ///
    /// <param name="src">File format.</param>
    /// <param name="color">Color mode.</param>
    ///
    /// <returns>File format.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public static Format Lookup(Format src, ColorMode color)
    {
        var key = new KeyValuePair<Format, ColorMode>(src, color);
        return _items.TryGetValue(key, out var dest) ? dest : src;
    }

    #endregion

    #region Fields
    private static readonly Dictionary<KeyValuePair<Format, ColorMode>, Format> _items = new()
    {
        { new(Format.Jpeg, ColorMode.Grayscale),  Format.Jpeg8bppGrayscale  },
        { new(Format.Jpeg, ColorMode.Monochrome), Format.Jpeg8bppGrayscale  },
        { new(Format.Png,  ColorMode.Grayscale),  Format.Png8bppGrayscale   },
        { new(Format.Png,  ColorMode.Monochrome), Format.Png1bppMonochrome  },
        { new(Format.Bmp,  ColorMode.Grayscale),  Format.Bmp8bppGrayscale   },
        { new(Format.Bmp,  ColorMode.Monochrome), Format.Bmp1bppMonochrome  },
        { new(Format.Tiff, ColorMode.Grayscale),  Format.Tiff8bppGrayscale  },
        { new(Format.Tiff, ColorMode.Monochrome), Format.Tiff1bppMonochrome },
    };
    #endregion
}
