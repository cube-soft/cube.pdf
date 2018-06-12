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
using Cube.Log;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cube.Pdf.Ghostscript
{
    /* --------------------------------------------------------------------- */
    ///
    /// Converter
    ///
    /// <summary>
    /// Ghostscript 変換プログラムの基底クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class Converter
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// Converter
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="format">変換後のフォーマット</param>
        ///
        /* ----------------------------------------------------------------- */
        public Converter(Format format)
        {
            Format = format;
            Fonts.Add(Environment.GetFolderPath(Environment.SpecialFolder.Fonts));
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Format
        ///
        /// <summary>
        /// 変換後のフォーマットを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Format Format { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Paper
        ///
        /// <summary>
        /// 用紙サイズを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Paper Paper { get; set; } = Paper.Auto;

        /* ----------------------------------------------------------------- */
        ///
        /// Orientation
        ///
        /// <summary>
        /// ページの向きを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Orientation Orientation { get; set; } = Orientation.Auto;

        /* ----------------------------------------------------------------- */
        ///
        /// Resolution
        ///
        /// <summary>
        /// 画像データの変換時に適用する解像度を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Resolution { get; set; } = 600;

        /* ----------------------------------------------------------------- */
        ///
        /// Quiet
        ///
        /// <summary>
        /// いくつかのメッセージの出力を抑制するかどうかを示す値を取得
        /// または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Quiet { get; set; } = true;

        /* ----------------------------------------------------------------- */
        ///
        /// Log
        ///
        /// <summary>
        /// Ghostscript の出力ログを保存するパスを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Log { get; set; } = string.Empty;

        /* ----------------------------------------------------------------- */
        ///
        /// Resources
        ///
        /// <summary>
        /// リソースファイルが格納されているディレクトリ一覧を取得
        /// または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ICollection<string> Resources { get; } = new List<string>();

        /* ----------------------------------------------------------------- */
        ///
        /// Fonts
        ///
        /// <summary>
        /// フォントが格納されているディレクトリ一覧を取得または設定します。
        /// </summary>
        ///
        /// <remarks>
        /// 初期値として C:\Windows\Fonts に相当するパスが設定されます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public ICollection<string> Fonts { get; } = new List<string>();

        /* ----------------------------------------------------------------- */
        ///
        /// Options
        ///
        /// <summary>
        /// オプション引数一覧を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ICollection<Argument> Options { get; } = new List<Argument>();

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// 変換処理を実行します。
        /// </summary>
        ///
        /// <param name="src">変換元ファイル</param>
        /// <param name="dest">変換結果を保存するパス</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Invoke(string src, string dest) => Invoke(new[] { src }, dest);

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// 変換処理を実行します。
        /// </summary>
        ///
        /// <param name="sources">変換元ファイル一覧</param>
        /// <param name="dest">変換結果を保存するパス</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Invoke(IEnumerable<string> sources, string dest) =>
            GsApi.NativeMethods.Invoke(Create()
            .Concat(OnCreateArguments())
            .Concat(new[]
            {
                new Argument('s', "OutputFile", dest),
                new Argument('f'),
            })
            .Concat(sources.Select(e => new Argument(e)))
            .Select(e =>
            {
                var str = e.ToString();
                this.LogDebug(str);
                return str;
            })
            .ToArray()
        );

        /* ----------------------------------------------------------------- */
        ///
        /// OnCreateArguments
        ///
        /// <summary>
        /// Ghostscript API で実行するための引数一覧を生成します。
        /// </summary>
        ///
        /// <returns>引数一覧</returns>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual IEnumerable<Argument> OnCreateArguments()
        {
            var args = new List<Argument>
            {
                Format.GetArgument(),
                new Argument('d', "NOSAFER"),
                new Argument('d', "BATCH"),
                new Argument('d', "NOPAUSE"),
            };

            return args;
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// 引数一覧を表すコレクションを生成します。
        /// </summary>
        ///
        /// <remarks>
        /// Ghostscript API は最初の引数を無視するので、ダミー引数を
        /// 設定しています。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private IEnumerable<Argument> Create() => new[] { new Argument("gs") };

        #endregion
    }
}
