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
using iTextSharp.text.pdf;
using System.Collections;
using System.Collections.Generic;

namespace Cube.Pdf.Itext
{
    /* --------------------------------------------------------------------- */
    ///
    /// ReadOnlyPageList
    ///
    /// <summary>
    /// 読み取り専用で PDF ページ一覧へアクセスするためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal class ReadOnlyPageList : IReadOnlyList<Page>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ReadOnlyPageList
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="core">PdfReader オブジェクト</param>
        /// <param name="file">PDF ファイル情報</param>
        ///
        /* ----------------------------------------------------------------- */
        public ReadOnlyPageList(PdfReader core, PdfFile file)
        {
            File  = file;
            _core = core;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// File
        ///
        /// <summary>
        /// PDF ファイル情報を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PdfFile File { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Count
        ///
        /// <summary>
        /// ページ数を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Count => _core?.NumberOfPages ?? 0;

        /* ----------------------------------------------------------------- */
        ///
        /// Item[int]
        ///
        /// <summary>
        /// Page オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Page this[int index] => _core.GetPage(File, index + 1);

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetEnumerator
        ///
        /// <summary>
        /// 各ページオブジェクトへアクセスするための反復子を取得します。
        /// </summary>
        ///
        /// <returns>反復子</returns>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerator<Page> GetEnumerator()
        {
            for (var i = 0; i < Count; ++i) yield return this[i];
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetEnumerator
        ///
        /// <summary>
        /// 各ページオブジェクトへアクセスするための反復子を取得します。
        /// </summary>
        ///
        /// <returns>反復子</returns>
        ///
        /* ----------------------------------------------------------------- */
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        #region Fields
        private readonly PdfReader _core;
        #endregion
    }
}
