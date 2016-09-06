/* ------------------------------------------------------------------------- */
///
/// ReadOnlyPageCollection.cs
///
/// Copyright (c) 2010 CubeSoft, Inc. All rights reserved.
///
/// This program is free software: you can redistribute it and/or modify
/// it under the terms of the GNU Affero General Public License as published
/// by the Free Software Foundation, either version 3 of the License, or
/// (at your option) any later version.
///
/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
/// GNU Affero General Public License for more details.
///
/// You should have received a copy of the GNU Affero General Public License
/// along with this program.  If not, see <http://www.gnu.org/licenses/>.
///
/* ------------------------------------------------------------------------- */
using System;
using System.Collections;
using System.Collections.Generic;
using Cube.Pdf.Drawing.MuPdf;

namespace Cube.Pdf.Drawing
{
    /* --------------------------------------------------------------------- */
    ///
    /// ReadOnlyPageCollection
    /// 
    /// <summary>
    /// 読み取り専用で PDF ページ一覧へアクセスするためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ReadOnlyPageCollection : IReadOnlyCollection<Page>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ReadOnlyPageCollection
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ReadOnlyPageCollection() : this(IntPtr.Zero, null, 0) { }

        /* ----------------------------------------------------------------- */
        ///
        /// ReadOnlyPageCollection
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ReadOnlyPageCollection(IntPtr core, FileBase file, int count)
        {
            File  = file;
            Count = count;
            _core = core;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// FileBase
        /// 
        /// <summary>
        /// ファイル情報を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public FileBase File { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Count
        /// 
        /// <summary>
        /// ページ数を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Count { get; }

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
        /* ----------------------------------------------------------------- */
        public IEnumerator<Page> GetEnumerator()
        {
            for (var i = 0; i < Count; ++i)
            {
                var pagenum = i + 1;
                yield return _core.CreatePage(File, pagenum);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetEnumerator
        /// 
        /// <summary>
        /// 各ページオブジェクトへアクセスするための反復子を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        #region Fields
        private IntPtr _core = IntPtr.Zero;
        #endregion
    }
}
