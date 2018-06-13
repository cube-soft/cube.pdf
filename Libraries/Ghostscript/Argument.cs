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
    /// Ghostscript の引数を表すクラスです。
    /// </summary>
    ///
    /// <see href="https://www.ghostscript.com/doc/current/Use.htm" />
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
        /// <param name="name">名前</param>
        /// <param name="value">値</param>
        ///
        /// <remarks>
        /// このコンストラクタのみ、値をリテラルオブジェクトと見なします。
        /// これ以外でリテラルオブジェクトと見なす必要がある場合、
        /// Argument(string, string, string, bool) コンストラクタを実行
        /// して下さい。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public Argument(string name, string value) : this('d', name, value, true) { } // see remarks.

        /* ----------------------------------------------------------------- */
        ///
        /// Argument
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="type">引数の種類</param>
        ///
        /* ----------------------------------------------------------------- */
        public Argument(char type) : this(type, string.Empty, string.Empty) { }

        /* ----------------------------------------------------------------- */
        ///
        /// Argument
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="type">引数の種類</param>
        /// <param name="value">値</param>
        ///
        /// <remarks>
        /// 主に d オプション以外で利用されます (e.g, -r72)。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public Argument(char type, int value) : this(type, string.Empty, value) { }

        /* ----------------------------------------------------------------- */
        ///
        /// Argument
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="type">引数の種類</param>
        /// <param name="name">名前</param>
        ///
        /// <remarks>
        /// 主に d オプションで利用されます (e.g, -dBATCH)。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public Argument(char type, string name) : this(type, name, string.Empty) { }

        /* ----------------------------------------------------------------- */
        ///
        /// Argument
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="name">名前</param>
        /// <param name="value">値</param>
        ///
        /* ----------------------------------------------------------------- */
        public Argument(string name, bool value) : this('d', name, value) { }

        /* ----------------------------------------------------------------- */
        ///
        /// Argument
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="name">名前</param>
        /// <param name="value">値</param>
        ///
        /* ----------------------------------------------------------------- */
        public Argument(string name, int value) : this('d', name, value) { }

        /* ----------------------------------------------------------------- */
        ///
        /// Argument
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="type">引数の種類</param>
        /// <param name="name">名前</param>
        /// <param name="value">値</param>
        ///
        /* ----------------------------------------------------------------- */
        public Argument(char type, string name, bool value) :
            this(type, name, value.ToString().ToLowerInvariant()) { }

        /* ----------------------------------------------------------------- */
        ///
        /// Argument
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="type">引数の種類</param>
        /// <param name="name">名前</param>
        /// <param name="value">値</param>
        ///
        /* ----------------------------------------------------------------- */
        public Argument(char type, string name, int value) :
            this(type, name, value.ToString()) { }

        /* ----------------------------------------------------------------- */
        ///
        /// Argument
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="type">引数の種類</param>
        /// <param name="name">名前</param>
        /// <param name="value">値</param>
        ///
        /* ----------------------------------------------------------------- */
        public Argument(char type, string name, string value) :
            this(type, name, value, false) { }

        /* ----------------------------------------------------------------- */
        ///
        /// Argument
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="type">引数の種類</param>
        /// <param name="name">名前</param>
        /// <param name="value">値</param>
        /// <param name="literal">値がリテラルオブジェクトかどうか</param>
        ///
        /* ----------------------------------------------------------------- */
        public Argument(char type, string name, string value, bool literal)
        {
            Type      = type;
            Name      = name;
            Value     = value;
            IsLiteral = literal;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Argument
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="description">コード内容</param>
        ///
        /// <remarks>
        /// 主に PostScript コードを保持する際に利用されます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        protected Argument(string description) :
            this(default(char), string.Empty, description, false) { }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Dummy
        ///
        /// <summary>
        /// ダミー用 Argument オブジェクトを取得します。
        /// </summary>
        ///
        /// <remarks>
        /// Ghostscript は最初の引数を無視するため、主に最初の引数として
        /// 利用されます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static Argument Dummy { get; } = new Argument("gs");

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
        /// IsLiteral
        ///
        /// <summary>
        /// 値がリテラルオブジェクトかどうかを示す値を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool IsLiteral { get; }

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

            if (Type != default(char)) sb.Append($"-{Type}");
            if (Name.HasValue()) sb.Append(Name);
            if (Value.HasValue())
            {
                if (Name.HasValue()) sb.Append('=');
                if (IsLiteral) sb.Append('/');
                sb.Append(Value);
            }

            return sb.ToString();
        }

        #endregion
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Code
    ///
    /// <summary>
    /// Ghostscript で実行される PostScript コードを表すクラスです。
    /// </summary>
    ///
    /// <remarks>
    /// Argument.Value プロパティに保持する形で実装されます。
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public class Code : Argument
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// Code
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="description">コード内容</param>
        ///
        /* ----------------------------------------------------------------- */
        public Code(string description) : base(description) { }

        #endregion
    }
}
