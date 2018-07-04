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

namespace Cube.Pdf.Ghostscript
{
    /* --------------------------------------------------------------------- */
    ///
    /// Encoding
    ///
    /// <summary>
    /// エンコード方法に関する列挙型です。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public enum Encoding
    {
        /// <summary>None</summary>
        None,
        /// <summary>Flate encoding</summary>
        Flate,
        /// <summary>DCT encoding that is used in JPEG compression</summary>
        Jpeg,
        /// <summary>CCITT Fax encoding</summary>
        Fax,
        /// <summary>LZW encoding</summary>
        Lzw,
        /// <summary>Run Length Encoding (RLE)</summary>
        Rle,
        /// <summary>PackBits RLE</summary>
        PackBits,
        /// <summary>Base64 encoding</summary>
        Base64,
        /// <summary>Base85 encoding</summary>
        Base85,
    }

    /* --------------------------------------------------------------------- */
    ///
    /// EncodingExtension
    ///
    /// <summary>
    /// Encoding の拡張用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class EncodingExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetArgument
        ///
        /// <summary>
        /// Encoding を表す Argument オブジェクトを取得します。
        /// </summary>
        ///
        /// <param name="src">Encoding</param>
        /// <param name="name">名前</param>
        ///
        /// <returns>Argument オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Argument GetArgument(this Encoding src, string name) =>
            GetEncodingMap().TryGetValue(src, out var value) ?
            new Argument(name, value) :
            null;

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetEncodingMap
        ///
        /// <summary>
        /// Encoding と関連情報の対応一覧を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static IDictionary<Encoding, string> GetEncodingMap() => _map ?? (
            _map = new Dictionary<Encoding, string>
            {
                { Encoding.Flate,  "FlateEncode"    },
                { Encoding.Jpeg,   "DCTEncode"      },
                { Encoding.Fax,    "CCITTFaxEncode" },
                { Encoding.Lzw,    "LZWEncode"      },
                { Encoding.Base85, "ASCII85Encode"  },
            }
        );

        #endregion

        #region Fields
        private static IDictionary<Encoding, string> _map;
        #endregion
    }
}
