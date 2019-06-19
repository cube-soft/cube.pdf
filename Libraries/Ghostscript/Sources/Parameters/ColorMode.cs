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
    /// ColorMode
    ///
    /// <summary>
    /// Specifies color mode.
    /// </summary>
    ///
    /// <remarks>
    /// 主にドキュメントに埋め込まれた画像に対して適用されます。
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public enum ColorMode
    {
        /// <summary>RGB</summary>
        Rgb,
        /// <summary>CMYK</summary>
        Cmyk,
        /// <summary>Grayscale</summary>
        Grayscale,
        /// <summary>Same as the source file</summary>
        SameAsSource,
        /// <summary>Device independent color mode</summary>
        DeviceIndependent,
    }

    /* --------------------------------------------------------------------- */
    ///
    /// ColorModeExtension
    ///
    /// <summary>
    /// Provides extended methods of the ColorMode enum.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class ColorModeExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetArgument
        ///
        /// <summary>
        /// Gets the argument representing the color mode.
        /// </summary>
        ///
        /// <param name="src">Color mode value.</param>
        ///
        /// <returns>Argument object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Argument GetArgument(this ColorMode src)
        {
            var status = Map.TryGetValue(src, out var value);
            Debug.Assert(status);
            return new Argument("ColorConversionStrategy", value);
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Map
        ///
        /// <summary>
        /// Gets the collection of the color mode and related information.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static Dictionary<ColorMode, string> Map { get; } = new Dictionary<ColorMode, string>
        {
            { ColorMode.Rgb,               "RGB"                       },
            { ColorMode.Cmyk,              "CMYK"                      },
            { ColorMode.Grayscale,         "Gray"                      },
            { ColorMode.SameAsSource,      "LeaveColorUnchanged"       },
            { ColorMode.DeviceIndependent, "UseDeviceIndependentColor" },
        };

        #endregion
    }
}
