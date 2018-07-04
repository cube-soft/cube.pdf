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
using Cube.Collections;
using Cube.FileSystem;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections.Generic;

namespace Cube.Pdf.Itext
{
    /* --------------------------------------------------------------------- */
    ///
    /// WriterFactory
    ///
    /// <summary>
    /// PdfWriter および関連クラスを生成するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class WriterFactory
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// PDF 作成用オブジェクトを生成します。
        /// </summary>
        ///
        /// <param name="path">PDF ファイルのパス</param>
        /// <param name="data">メタ情報</param>
        /// <param name="smart">スマートコピーを使用するかどうか</param>
        /// <param name="io">I/O オブジェクト</param>
        ///
        /// <returns>Document および PdfCopy オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static KeyValuePair<Document, PdfCopy> Create(string path, Metadata data, bool smart, IO io)
        {
            var doc  = new Document();
            var dest = smart ?
                       new PdfSmartCopy(doc, io.Create(path)) :
                       new PdfCopy(doc, io.Create(path));

            dest.PdfVersion        = data.Version.Minor.ToString()[0];
            dest.ViewerPreferences = (int)data.ViewOption;

            return KeyValuePair.Create(doc, dest);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// PdfStamper オブジェクトを生成します。
        /// </summary>
        ///
        /// <param name="path">PDF ファイルのパス</param>
        /// <param name="reader">PdfReader オブジェクト</param>
        /// <param name="io">I/O オブジェクト</param>
        ///
        /// <returns>PdfStamper オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static PdfStamper Create(string path, PdfReader reader, IO io) =>
            new PdfStamper(reader, io.Create(path));

        #endregion
    }
}
