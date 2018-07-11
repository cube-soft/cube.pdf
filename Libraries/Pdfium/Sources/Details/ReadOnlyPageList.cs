/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
/* ------------------------------------------------------------------------- */
using Cube.Pdf.Pdfium.PdfiumApi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Cube.Pdf.Pdfium
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
        /// <param name="core">PDFium を実行するためのオブジェクト</param>
        /// <param name="file">PDF ファイル情報</param>
        ///
        /* ----------------------------------------------------------------- */
        public ReadOnlyPageList(IntPtr core, PdfFile file)
        {
            Debug.Assert(core != IntPtr.Zero);
            Debug.Assert(file != null);

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
        public int Count => File.Count;

        /* ----------------------------------------------------------------- */
        ///
        /// Item[int]
        ///
        /// <summary>
        /// Page オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Page this[int index] => GetPage(index);

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

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetPage
        ///
        /// <summary>
        /// Page オブジェクトを取得します。
        /// </summary>
        ///
        /// <param name="index">
        /// 最初のページを "ゼロ" とするインデックス
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        private Page GetPage(int index)
        {
            var page = Facade.FPDF_LoadPage(_core, index, 5);
            if (page == IntPtr.Zero) throw PdfiumLibrary.GetLoadException();

            try
            {
                var degree = GetPageRotation(page);
                var size   = GetPageSize(page, degree);

                return new Page(
                    File,                    // File
                    index + 1,               // Number
                    size,                    // Size
                    new Angle(degree),       // Rotation
                    new PointF(72.0f, 72.0f) // Resolution
                );
            }
            finally { Facade.FPDF_ClosePage(page); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetSize
        ///
        /// <summary>
        /// ページサイズを取得します。
        /// </summary>
        ///
        /// <remarks>
        /// PDFium は回転後のサイズを返しますが、Page オブジェクトでは
        /// 回転前の情報として格納します。そのため、場合によって幅と
        /// 高さの情報を反転しています。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private SizeF GetPageSize(IntPtr handle, int degree)
        {
            var w = (float)Facade.FPDF_GetPageWidth(handle);
            var h = (float)Facade.FPDF_GetPageHeight(handle);

            return (degree != 90 && degree != 270) ? new SizeF(w, h) : new SizeF(h, w);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetPageRotation
        ///
        /// <summary>
        /// ページの回転角度を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private int GetPageRotation(IntPtr handle)
        {
            var dest = Facade.FPDFPage_GetRotation(handle);
            return dest == 1 ?  90 :
                   dest == 2 ? 180 :
                   dest == 3 ? 270 : 0;
        }

        #endregion

        #region Fields
        private readonly IntPtr _core;
        #endregion
    }
}
