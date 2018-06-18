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

namespace Cube.Pdf.App.Converter
{
    /* --------------------------------------------------------------------- */
    ///
    /// Language
    ///
    /// <summary>
    /// 表示言語を表す列挙型です。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public enum Language
    {
        /// <summary>自動</summary>
        Auto = 0,
        /// <summary>英語</summary>
        English = 1,
        /// <summary>日本語</summary>
        Japanese = 2,
    }

    /* --------------------------------------------------------------------- */
    ///
    /// LanguageExtension
    ///
    /// <summary>
    /// Lanugage の拡張用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class LanguageExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetName
        ///
        /// <summary>
        /// Language に対応する名前を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static string GetName(this Language src) =>
            GetLanguageMap().TryGetValue(src, out var value) ? value : string.Empty;

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetLanguageMap
        ///
        /// <summary>
        /// Language と表示言語を表す文字列の対応関係を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static IDictionary<Language, string> GetLanguageMap() => _map ?? (
            _map = new Dictionary<Language, string>
            {
                { Language.Auto,     ""   },
                { Language.English,  "en" },
                { Language.Japanese, "ja" },
            }
        );

        #endregion

        #region Fields
        private static IDictionary<Language, string> _map;
        #endregion
    }
}
