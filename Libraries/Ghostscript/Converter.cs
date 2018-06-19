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
using Cube.FileSystem;
using Cube.Generics;
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
        public Converter(Format format) : this(format, new IO()) { }

        /* ----------------------------------------------------------------- */
        ///
        /// Converter
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="format">変換後のフォーマット</param>
        /// <param name="io">I/O オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public Converter(Format format, IO io)
        {
            IO = io;
            Format = format;
            Fonts.Add(Environment.GetFolderPath(Environment.SpecialFolder.Fonts));
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// IO
        ///
        /// <summary>
        /// I/O オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IO IO { get; }

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
        /// WorkDirectory
        ///
        /// <summary>
        /// 作業ディレクトリのパスを取得または設定します。
        /// </summary>
        ///
        /// <remarks>
        /// このプロパティに値を設定した場合、変換処理の際に一時的に
        /// TEMP 環境変数が変更されます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public string WorkDirectory { get; set; } = string.Empty;

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
            SetWorkDirectory(() => GsApi.NativeMethods.Invoke(Create()
                .Concat(new[] { new Argument('s', "OutputFile", dest) })
                .Concat(OnCreateArguments())
                .Concat(CreateCodes())
                .Concat(new[] { new Argument('f') })
                .Select(e => e.ToString())
                .Concat(sources)
                .Where(e => { this.LogDebug(e); return true; }) // for debug
                .ToArray()
            ));

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
                CreateQuiet(),
                CreateLog(),
                CreateResources(),
                CreateFonts(),
                CreateResolution(),
                Paper.GetArgument(),
                Orientation.GetArgument(),
            }
            .Concat(Options);

            return Trim(args);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnCreateCodes
        ///
        /// <summary>
        /// Ghostscript API で実行するための PostScript コードを表す
        /// 引数一覧を生成します。
        /// </summary>
        ///
        /// <returns>引数一覧</returns>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual IEnumerable<Code> OnCreateCodes() =>
            Trim(new[] { Orientation.GetCode() });

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
        /// Ghostscript API は最初の引数を無視するため、引数の先頭に
        /// ダミーオブジェクトを配置します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private IEnumerable<Argument> Create() => new[] { Argument.Dummy };

        /* ----------------------------------------------------------------- */
        ///
        /// CreateCodes
        ///
        /// <summary>
        /// Ghostscript API で実行するための PostScript コードを表す
        /// 引数一覧を生成します。
        /// </summary>
        ///
        /// <returns>引数一覧</returns>
        ///
        /* ----------------------------------------------------------------- */
        private IEnumerable<Argument> CreateCodes()
        {
            var dest = OnCreateCodes();
            return dest.Count() > 0 ?
                   new[] { new Argument('c') }.Concat(dest) :
                   dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CreateResources
        ///
        /// <summary>
        /// リソースディレクトリ一覧を表す Argument を生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private Argument CreateResources() =>
            Resources.Count > 0 ?
            new Argument('I', string.Empty, string.Join(";", Resources)) :
            null;

        /* ----------------------------------------------------------------- */
        ///
        /// CreateFonts
        ///
        /// <summary>
        /// フォントディレクトリ一覧を表す Argument を生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private Argument CreateFonts() =>
            Fonts.Count > 0 ?
            new Argument('s', "FONTPATH", string.Join(";", Fonts)) :
            null;

        /* ----------------------------------------------------------------- */
        ///
        /// CreateLog
        ///
        /// <summary>
        /// ログファイルを表す Argument を生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private Argument CreateLog() =>
            Log.HasValue() ? new Argument('s', "stdout", Log) : null;

        /* ----------------------------------------------------------------- */
        ///
        /// CreateQuiet
        ///
        /// <summary>
        /// Quiet を表す Argument を生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private Argument CreateQuiet() =>
            Quiet ? new Argument('d', "QUIET") : null;

        /* ----------------------------------------------------------------- */
        ///
        /// CreateResolution
        ///
        /// <summary>
        /// Resolution を表す Argument を生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private Argument CreateResolution() => new Argument('r', Resolution);

        /* ----------------------------------------------------------------- */
        ///
        /// Trim
        ///
        /// <summary>
        /// null オブジェクトを除去します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private IEnumerable<T> Trim<T>(IEnumerable<T> src) => src.OfType<T>();

        /* ----------------------------------------------------------------- */
        ///
        /// SetWorkDirectory
        ///
        /// <summary>
        /// 作業ディレクトリを設定した後 Action を実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SetWorkDirectory(Action action)
        {
            var name = "TEMP";
            var prev = Environment.GetEnvironmentVariable(name);

            try
            {
                if (WorkDirectory.HasValue())
                {
                    if (!IO.Exists(WorkDirectory)) IO.CreateDirectory(WorkDirectory);
                    SetVariable(name, WorkDirectory);
                }
                action();
            }
            finally { SetVariable(name, prev); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetVariable
        ///
        /// <summary>
        /// 環境変数を設定します。
        /// </summary>
        ///
        /// <remarks>
        /// 設定された環境変数は実行プロセス中でのみ有効です。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private void SetVariable(string key, string value) =>
            Environment.SetEnvironmentVariable(key, value, EnvironmentVariableTarget.Process);

        #endregion
    }
}
