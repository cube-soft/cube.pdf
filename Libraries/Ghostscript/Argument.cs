using System;
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
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Key
        ///
        /// <summary>
        /// キーを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Key { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Value
        ///
        /// <summary>
        /// 値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Value { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// KeyPrefix
        ///
        /// <summary>
        /// キーの接頭辞を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public char KeyPrefix { get; set; } = 'd';

        /* ----------------------------------------------------------------- */
        ///
        /// ValuePrefix
        ///
        /// <summary>
        /// 値の接頭辞を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public char ValuePrefix { get; set; } = '/';

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
            if (string.IsNullOrEmpty(Key)) throw new ArgumentException(nameof(Key));

            var sb = new StringBuilder();
            sb.Append($"-{KeyPrefix}{Key}");
            if (!string.IsNullOrEmpty(Value)) sb.Append($"={ValuePrefix}{Value}");

            return base.ToString();
        }

        #endregion
    }
}
