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
    /// ImageConverter
    ///
    /// <summary>
    /// PNG などのビットマップ画像形式に変換するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ImageConverter : Converter
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ImageConverter
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="format">変換後のフォーマット</param>
        ///
        /* ----------------------------------------------------------------- */
        public ImageConverter(Format format) : base(format)
        {
            if (!SupportedFormats.Contains(format)) throw new NotSupportedException();
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// SupportedFormats
        ///
        /// <summary>
        /// サポートする形式一覧を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static IEnumerable<Format> SupportedFormats { get; } = new HashSet<Format>
        {
            Format.Psd,
            Format.PsdRgb,
            Format.PsdCmyk,
            Format.PsdCmykog,
            Format.Jpeg,
            Format.Jpeg24bppRgb,
            Format.Jpeg32bppCmyk,
            Format.Jpeg8bppGrayscale,
            Format.Png,
            Format.Png24bppRgb,
            Format.Png32bppArgb,
            Format.Png4bppIndexed,
            Format.Png8bppIndexed,
            Format.Png8bppGrayscale,
            Format.Png1bppMonochrome,
            Format.Bmp,
            Format.Bmp24bppRgb,
            Format.Bmp32bppArgb,
            Format.Bmp4bppIndexed,
            Format.Bmp8bppIndexed,
            Format.Bmp8bppGrayscale,
            Format.Bmp1bppMonochrome,
            Format.Tiff,
            Format.Tiff12bppRgb,
            Format.Tiff24bppRgb,
            Format.Tiff48bppRgb,
            Format.Tiff32bppCmyk,
            Format.Tiff64bppCmyk,
            Format.Tiff8bppGrayscale,
            Format.Tiff1bppMonochrome,
        };

        /* ----------------------------------------------------------------- */
        ///
        /// AntiAlias
        ///
        /// <summary>
        /// アンチエイリアスが有効かどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool AntiAlias { get; set; } = true;

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
            .Concat(CreateAntiAlias());

        /* ----------------------------------------------------------------- */
        ///
        /// CreateAntiAlias
        ///
        /// <summary>
        /// AntiAlias を表す Argument を生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private IEnumerable<Argument> CreateAntiAlias() =>
            AntiAlias ?
            new[]
            {
                new Argument("GraphicsAlphaBits", 4),
                new Argument("TextAlphaBits", 4),
            } :
            new Argument[0];

        #endregion
    }
}
