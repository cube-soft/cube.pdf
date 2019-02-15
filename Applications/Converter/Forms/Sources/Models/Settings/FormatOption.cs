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
    /// FormatOption
    ///
    /// <summary>
    /// Specifies format options.
    /// </summary>
    ///
    /// <remarks>
    /// 旧 CubePDF で PDFVersion と呼んでいたものを汎用化した形で定義
    /// しています。ただし、現在定義されているものは PDF のバージョン
    /// のみです。
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public enum FormatOption
    {
        /// <summary>PDF 1.2</summary>
        Pdf12 = 5,
        /// <summary>PDF 1.3</summary>
        Pdf13 = 4,
        /// <summary>PDF 1.4</summary>
        Pdf14 = 3,
        /// <summary>PDF 1.5</summary>
        Pdf15 = 2,
        /// <summary>PDF 1.6</summary>
        Pdf16 = 1,
        /// <summary>PDF 1.7</summary>
        Pdf17 = 0,
        /// <summary>PDF/A</summary>
        PdfA = 6,
        /// <summary>PDF/X-1a</summary>
        PdfX1a = 7,
        /// <summary>PDF/X-3</summary>
        PdfX3 = 8,
    }

    /* --------------------------------------------------------------------- */
    ///
    /// FormatOptionExtension
    ///
    /// <summary>
    /// Provides extended methods of the FormatOption class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class FormatOptionExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetVersion
        ///
        /// <summary>
        /// FormatOption に対応する Version オブジェクトを取得します。
        /// </summary>
        ///
        /// <param name="src">FormatOption</param>
        ///
        /// <returns>Version</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static PdfVersion GetVersion(this FormatOption src) =>
            GetFormatOptionMap().TryGetValue(src, out var dest) ?
            dest : new PdfVersion(1, 0);

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetFormatOptionMap
        ///
        /// <summary>
        /// FormatOption の対応関係一覧を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static IDictionary<FormatOption, PdfVersion> GetFormatOptionMap() => _options ?? (
            _options = new Dictionary<FormatOption, PdfVersion>
            {
                { FormatOption.Pdf12,  new PdfVersion(1, 2) },
                { FormatOption.Pdf13,  new PdfVersion(1, 3) },
                { FormatOption.Pdf14,  new PdfVersion(1, 4) },
                { FormatOption.Pdf15,  new PdfVersion(1, 5) },
                { FormatOption.Pdf16,  new PdfVersion(1, 6) },
                { FormatOption.Pdf17,  new PdfVersion(1, 7) },
                { FormatOption.PdfA,   new PdfVersion(1, 3) },
                { FormatOption.PdfX1a, new PdfVersion(1, 3) },
                { FormatOption.PdfX3,  new PdfVersion(1, 3) },
            }
        );

        #endregion

        #region Fields
        private static IDictionary<FormatOption, PdfVersion> _options;
        #endregion
    }
}
