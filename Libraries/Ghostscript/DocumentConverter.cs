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
    /// <see href="https://www.ghostscript.com/doc/9.23/VectorDevices.htm" />
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
        /// EmbedFonts
        ///
        /// <summary>
        /// フォントを埋め込むかどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool EmbedFonts { get; set; } = true;

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
        /// Compression
        ///
        /// <summary>
        /// 埋め込まれた画像データの圧縮方法を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Encoding Compression { get; set; } = Encoding.Flate;

        /* ----------------------------------------------------------------- */
        ///
        /// Downsampling
        ///
        /// <summary>
        /// 埋め込まれた画像データのダウンサンプリング方法を取得または
        /// 設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Downsampling Downsampling { get; set; } = Downsampling.None;

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
            .Concat(CreateFontArguments())
            .Concat(CreateImageArguments());

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
        /// CreateFontArguments
        ///
        /// <summary>
        /// フォントに関する Argument を生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private IEnumerable<Argument> CreateFontArguments()
        {
            var dest = new List<Argument> { new Argument("EmbedAllFonts", EmbedFonts) };
            if (EmbedFonts) dest.Add(new Argument("SubsetFonts", true));
            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CreateImageArguments
        ///
        /// <summary>
        /// 埋め込まれた画像に関する Argument を生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private IEnumerable<Argument> CreateImageArguments() => Trim(new[]
        {
            new Argument("ColorImageResolution",  Resolution),
            new Argument("GrayImageResolution",   Resolution),
            new Argument("MonoImageResolution",   GetMonoResolution()),
            new Argument("DownsampleColorImages", Downsampling != Downsampling.None),
            new Argument("DownsampleGrayImages",  Downsampling != Downsampling.None),
            new Argument("DownsampleMonoImages",  Downsampling != Downsampling.None),
            new Argument("EncodeColorImages",     Compression != Encoding.None),
            new Argument("EncodeGrayImages",      Compression != Encoding.None),
            new Argument("EncodeMonoImages",      Compression != Encoding.None),
            new Argument("AutoFilterColorImages", false),
            new Argument("AutoFilterGrayImages",  false),
            new Argument("AutoFilterMonoImages",  false),
            Downsampling.GetArgument("ColorImageDownsampleType"),
            Downsampling.GetArgument("GrayImageDownsampleType"),
            Downsampling.GetArgument("MonoImageDownsampleType"),
            Compression.GetArgument("ColorImageFilter"),
            Compression.GetArgument("GrayImageFilter"),
            Compression.GetArgument("MonoImageFilter"),
        });

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
        /// GetMonoResolution
        ///
        /// <summary>
        /// モノクロ画像に適用する解像度を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private int GetMonoResolution() =>
            Resolution <  300 ?  300 :
            Resolution < 1200 ? 1200 :
            Resolution;

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
