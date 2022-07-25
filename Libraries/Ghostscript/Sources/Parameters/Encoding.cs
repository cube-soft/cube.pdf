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

using System.Collections.Generic;

/* ------------------------------------------------------------------------- */
///
/// Encoding
///
/// <summary>
/// Specifies encoding methods.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public enum Encoding
{
    /// <summary>None</summary>
    None,
    /// <summary>Flate encoding</summary>
    Flate,
    /// <summary>LZW encoding</summary>
    Lzw,
    /// <summary>DCT encoding that is used in JPEG compression</summary>
    Jpeg,
    /// <summary>CCITT Fax encoding</summary>
    Fax,
    /// <summary>JBIG2 encoding</summary>
    Jbig2,
    /// <summary>Run Length Encoding (RLE)</summary>
    Rle,
    /// <summary>PackBits RLE</summary>
    PackBits,
    /// <summary>Base64 encoding</summary>
    Base64,
    /// <summary>Base85 encoding</summary>
    Base85,
}

/* ------------------------------------------------------------------------- */
///
/// EncodingExtension
///
/// <summary>
/// Provides extended methods of the Encoding enum.
/// </summary>
///
/* ------------------------------------------------------------------------- */
internal static class EncodingExtension
{
    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// GetArgument
    ///
    /// <summary>
    /// Gets a new instance of the Argument class from the specified
    /// parameters.
    /// </summary>
    ///
    /// <param name="src">Encoding value.</param>
    /// <param name="name">Name of the argument.</param>
    ///
    /// <returns>Argument object.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public static Argument GetArgument(this Encoding src, string name) =>
        Map.TryGetValue(src, out var value) ?
        new Argument(name, value) :
        null;

    #endregion

    #region Implementations

    /* --------------------------------------------------------------------- */
    ///
    /// Map
    ///
    /// <summary>
    /// Gets the collection of Encoding values and related information.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private static Dictionary<Encoding, string> Map { get; } = new Dictionary<Encoding, string>
    {
        { Encoding.Flate,  "FlateEncode"    },
        { Encoding.Jpeg,   "DCTEncode"      },
        { Encoding.Fax,    "CCITTFaxEncode" },
        { Encoding.Lzw,    "LZWEncode"      },
        { Encoding.Base85, "ASCII85Encode"  },
    };

    #endregion

}
