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
using System.Text;

namespace Cube.Pdf.Ghostscript
{
    /* --------------------------------------------------------------------- */
    ///
    /// Argument
    ///
    /// <summary>
    /// Interpreter に指定可能な引数を表すクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class Argument
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// Argument
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Argument(char type) :
            this(type, string.Empty, ' ', string.Empty) { }

        /* ----------------------------------------------------------------- */
        ///
        /// Argument
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Argument(string value) :
            this(' ', string.Empty, ' ', value) { }

        /* ----------------------------------------------------------------- */
        ///
        /// Argument
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Argument(char type, string value) :
            this(type, string.Empty, ' ', value) { }

        /* ----------------------------------------------------------------- */
        ///
        /// Argument
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Argument(string name, string value) :
            this('d', name, '/', value) { }

        /* ----------------------------------------------------------------- */
        ///
        /// Argument
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Argument(char type, string name, string value) :
            this(type, name, ' ', value) { }

        /* ----------------------------------------------------------------- */
        ///
        /// Argument
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Argument(char type, string name, char prefix, string value)
        {
            Type  = type;
            Name  = name;
            Prefix = prefix;
            Value  = value;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Type
        ///
        /// <summary>
        /// 引数の種類を表す文字を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public char Type { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Name
        ///
        /// <summary>
        /// 名前を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Name { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Value
        ///
        /// <summary>
        /// 値を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Value { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Prefix
        ///
        /// <summary>
        /// 値の接頭辞を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public char Prefix { get; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// ToString
        ///
        /// <summary>
        /// 引数を表す文字列を取得します。
        /// </summary>
        ///
        /// <returns>文字列</returns>
        ///
        /* ----------------------------------------------------------------- */
        public override string ToString()
        {
            var sb = new StringBuilder();

            if (IsValid(Type)) sb.Append($"-{Type}");
            if (Name.HasValue()) sb.Append(Name);
            if (Value.HasValue())
            {
                if (Name.HasValue()) sb.Append('=');
                if (IsValid(Prefix)) sb.Append(Prefix);
                sb.Append(Value);
            }

            return sb.ToString();
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// IsValid
        ///
        /// <summary>
        /// 有効な文字かどうかを判別します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private bool IsValid(char c) => c != default(char) && c != ' ';

        #endregion
    }
}
