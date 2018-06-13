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
using System.Collections.Generic;
using System.Linq;

namespace Cube.Pdf.Ghostscript
{
    /* --------------------------------------------------------------------- */
    ///
    /// DocumentConverter
    ///
    /// <summary>
    /// PDF などのドキュメント形式に変換するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class DocumentConverter : Converter
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// DocumentConverter
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="format">変換後のフォーマット</param>
        ///
        /* ----------------------------------------------------------------- */
        public DocumentConverter(Format format) : base(format) { }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Version
        ///
        /// <summary>
        /// バージョンを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Version Version { get; set; } = new Version(1, 7);

        /* ----------------------------------------------------------------- */
        ///
        /// ColorMode
        ///
        /// <summary>
        /// カラーモードを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ColorMode ColorMode { get; set; } = ColorMode.SameAsSource;

        /* ----------------------------------------------------------------- */
        ///
        /// EmbedFonts
        ///
        /// <summary>
        /// フォントを埋め込むかどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool EmbedFonts { get; set; } = true;

        #endregion

        #region Implementations

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
        protected override IEnumerable<Argument> OnCreateArguments() =>
            base.OnCreateArguments()
            .Concat(new[]
            {
                CreateVersion(),
                ColorMode.GetArgument(),
            })
            .Concat(CreateEmbedFonts());

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
        protected override IEnumerable<Code> OnCreateCodes() =>
            base.OnCreateCodes()
            .Concat(Trim(new[] { CreateEmbedFontsCode() }));

        /* ----------------------------------------------------------------- */
        ///
        /// CreateVersion
        ///
        /// <summary>
        /// バージョン番号を表す Argument を生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private Argument CreateVersion() =>
            new Argument('d', "CompatibilityLevel", $"{Version.Major}.{Version.Minor}");

        /* ----------------------------------------------------------------- */
        ///
        /// CreateEmbedFonts
        ///
        /// <summary>
        /// フォントの埋め込み設定に関する Argument を生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private IEnumerable<Argument> CreateEmbedFonts()
        {
            var dest = new List<Argument> { new Argument("EmbedAllFonts", EmbedFonts) };
            if (EmbedFonts) dest.Add(new Argument("SubsetFonts", true));
            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CreateEmbedFontsCode
        ///
        /// <summary>
        /// フォントの埋め込み設定に関する PostScript コードを生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private Code CreateEmbedFontsCode() =>
            EmbedFonts ?
            new Code(".setpdfwrite <</NeverEmbed [ ]>> setdistillerparams") :
            null;

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

        #endregion
    }
}
