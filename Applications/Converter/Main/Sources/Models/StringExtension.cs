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
using System;
using System.Text.RegularExpressions;

namespace Cube.Pdf.Converter
{
    /* --------------------------------------------------------------------- */
    ///
    /// StringExtension
    ///
    /// <summary>
    /// 文字列の拡張用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class StringExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// WordWrap
        ///
        /// <summary>
        /// 指定した文字数で折り返します。
        /// </summary>
        ///
        /// <param name="src">元の文字列</param>
        /// <param name="n">折り返す文字数</param>
        ///
        /// <returns>変換後の文字列</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static string WordWrap(this string src, int n)
            => Regex.Replace(src, $@"(?<=\G.{{{n}}})(?!$)", Environment.NewLine);

        #endregion
    }
}
