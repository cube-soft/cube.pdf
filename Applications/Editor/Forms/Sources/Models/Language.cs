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
using Cube.Generics;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Cube.Pdf.App.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// Language
    ///
    /// <summary>
    /// Represents the kind of Language.
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
    /// Provides extended methods for the <c>Language</c> class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class LanguageExtension
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// LanguageExtension
        ///
        /// <summary>
        /// Initializes static fields
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        static LanguageExtension()
        {
            _system = Thread.CurrentThread.CurrentUICulture.Name;
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetName
        ///
        /// <summary>
        /// Gets the name of the specified <c>Language</c>.
        /// </summary>
        ///
        /// <param name="src">Language</param>
        ///
        /// <returns>Name</returns>
        ///
        /// <remarks>
        /// Language.Auto または未対応の値が指定された場合、プログラム
        /// 起動時の名前を返します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static string GetName(this Language src)
        {
            var status = GetLanguageMap().TryGetValue(src, out var dest);
            Debug.Assert(status);
            return dest.HasValue() ? dest : _system;
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetLanguageMap
        ///
        /// <summary>
        /// Gets the collection that each item has the Language and
        /// its name.
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
        private static readonly string _system;
        private static IDictionary<Language, string> _map;
        #endregion
    }
}
