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
    /// 色の変換方法に関する列挙型です。
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
        /// <summary>入力ファイルと同じカラーモード</summary>
        SameAsSource,
        /// <summary>デバイスに依存しないカラーモード</summary>
        DeviceIndependent,
    }

    /* --------------------------------------------------------------------- */
    ///
    /// ColorModeExtension
    ///
    /// <summary>
    /// ColorMode の拡張用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class ColorModeExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetArgument
        ///
        /// <summary>
        /// ColorMode を表す Argument オブジェクトを取得します。
        /// </summary>
        ///
        /// <param name="src">ColorMode</param>
        ///
        /// <returns>Argument オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Argument GetArgument(this ColorMode src)
        {
            var result = GetColorModeMap().TryGetValue(src, out var value);
            Debug.Assert(result);
            return new Argument("ColorConversionStrategy", value);
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetColorModeMap
        ///
        /// <summary>
        /// ColorMode と関連情報の対応一覧を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static IDictionary<ColorMode, string> GetColorModeMap() => _map ?? (
            _map = new Dictionary<ColorMode, string>
            {
                { ColorMode.Rgb,               "RGB"                       },
                { ColorMode.Cmyk,              "CMYK"                      },
                { ColorMode.Grayscale,         "Gray"                      },
                { ColorMode.SameAsSource,      "LeaveColorUnchanged"       },
                { ColorMode.DeviceIndependent, "UseDeviceIndependentColor" },
            }
        );

        #endregion

        #region Fields
        private static IDictionary<ColorMode, string> _map;
        #endregion
    }
}
