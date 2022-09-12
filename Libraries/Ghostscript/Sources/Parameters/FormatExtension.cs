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

using System;
using System.Collections.Generic;

/* ------------------------------------------------------------------------- */
///
/// FormatMethods
///
/// <summary>
/// Provides extended methods of the Format enum.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public static class FormatMethods
{
    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// GetExtension
    ///
    /// <summary>
    /// Gets an extension string corresponding to the specified format.
    /// </summary>
    ///
    /// <param name="src">Format value.</param>
    ///
    /// <returns>Extension string.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public static string GetExtension(this Format src) => Get(src).Extension;

    /* --------------------------------------------------------------------- */
    ///
    /// GetArgument
    ///
    /// <summary>
    /// Gets an Argument object corresponding to the specified format.
    /// </summary>
    ///
    /// <param name="src">Format value.</param>
    ///
    /// <returns>Argument that represents a format.</returns>
    ///
    /* --------------------------------------------------------------------- */
    internal static Argument GetArgument(this Format src) => new('s', "DEVICE", Get(src).Device);

    #endregion

    #region Implementations

    /* --------------------------------------------------------------------- */
    ///
    /// Get
    ///
    /// <summary>
    /// Gets the FormatInfo object corresponding to the specified format.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private static FormatInfo Get(Format src) =>
        Map.TryGetValue(src, out var dest) ? dest : throw new NotSupportedException($"{src}");

    /* --------------------------------------------------------------------- */
    ///
    /// Map
    ///
    /// <summary>
    /// Gets the collection of formats and related information.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private static Dictionary<Format, FormatInfo> Map { get; } = new()
    {
        { Format.Text,               new("txtwrite",    ".txt")  },
        { Format.Ps,                 new("ps2write",    ".ps")   },
        { Format.Eps,                new("eps2write",   ".eps")  },
        { Format.Pdf,                new("pdfwrite",    ".pdf")  },
        { Format.Psd,                new("psdrgb",      ".psd")  },
        { Format.PsdRgb,             new("psdrgb",      ".psd")  },
        { Format.PsdCmyk,            new("psdcmyk",     ".psd")  },
        { Format.PsdCmykog,          new("psdcmykog",   ".psd")  },
        { Format.Jpeg,               new("jpeg",        ".jpg")  },
        { Format.Jpeg24bppRgb,       new("jpeg",        ".jpg")  },
        { Format.Jpeg32bppCmyk,      new("jpegcmyk",    ".jpg")  },
        { Format.Jpeg8bppGrayscale,  new("jpeggray",    ".jpg")  },
        { Format.Png,                new("png16m",      ".png")  },
        { Format.Png24bppRgb,        new("png16m",      ".png")  },
        { Format.Png32bppArgb,       new("png16malpha", ".png")  },
        { Format.Png4bppIndexed,     new("png16",       ".png")  },
        { Format.Png8bppIndexed,     new("png256",      ".png")  },
        { Format.Png8bppGrayscale,   new("pnggray",     ".png")  },
        { Format.Png1bppMonochrome,  new("pngmonod",    ".png")  },
        { Format.Bmp,                new("bmp16m",      ".bmp")  },
        { Format.Bmp24bppRgb,        new("bmp16m",      ".bmp")  },
        { Format.Bmp32bppArgb,       new("bmp32b",      ".bmp")  },
        { Format.Bmp4bppIndexed,     new("bmp16",       ".bmp")  },
        { Format.Bmp8bppIndexed,     new("bmp256",      ".bmp")  },
        { Format.Bmp8bppGrayscale,   new("bmpgray",     ".bmp")  },
        { Format.Bmp1bppMonochrome,  new("bmpmono",     ".bmp")  },
        { Format.Tiff,               new("tiff24nc",    ".tiff") },
        { Format.Tiff12bppRgb,       new("tiff12nc",    ".tiff") },
        { Format.Tiff24bppRgb,       new("tiff24nc",    ".tiff") },
        { Format.Tiff48bppRgb,       new("tiff48nc",    ".tiff") },
        { Format.Tiff32bppCmyk,      new("tiff32nc",    ".tiff") },
        { Format.Tiff64bppCmyk,      new("tiff64nc",    ".tiff") },
        { Format.Tiff8bppGrayscale,  new("tiffgray",    ".tiff") },
        { Format.Tiff1bppMonochrome, new("tiffg4",      ".tiff") },
    };

    /* --------------------------------------------------------------------- */
    ///
    /// FormatInfo
    ///
    /// <summary>
    /// Represents information related with a Format value.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
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
}
