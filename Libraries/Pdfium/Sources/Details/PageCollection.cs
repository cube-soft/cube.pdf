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
using Cube.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Cube.Pdf.Pdfium
{
    /* --------------------------------------------------------------------- */
    ///
    /// PageCollection
    ///
    /// <summary>
    /// Represents the collection of PDF pages.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal sealed class PageCollection : EnumerableBase<Page>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// PageCollection
        ///
        /// <summary>
        /// Initializes a new instance of the PageCollection class with
        /// the specified arguments.
        /// </summary>
        ///
        /// <param name="core">PDFium object.</param>
        /// <param name="file">File information of the PDF document.</param>
        ///
        /* ----------------------------------------------------------------- */
        public PageCollection(PdfiumReader core, PdfFile file)
        {
            Debug.Assert(core != null && file != null);

            _core = core;
            _file = file;
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetEnumerator
        ///
        /// <summary>
        /// Returns an enumerator that iterates through this collection.
        /// </summary>
        ///
        /// <returns>
        /// An IEnumerator(Page) object for this collection.
        /// </returns>
        ///
        /* ----------------------------------------------------------------- */
        public override IEnumerator<Page> GetEnumerator()
        {
            for (var i = 0; i < _file.Count; ++i) yield return GetPage(i);
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetPage
        ///
        /// <summary>
        /// Gets the Page object.
        /// </summary>
        ///
        /// <param name="index">Zero for the first page.</param>
        ///
        /* ----------------------------------------------------------------- */
        private Page GetPage(int index) => _core.Invoke(e =>
        {
            var page = NativeMethods.FPDF_LoadPage(e, index);
            if (page == IntPtr.Zero) throw _core.GetLastError();

            try
            {
                var degree = GetPageRotation(page);
                var size   = GetPageSize(page, degree);

                return new Page(
                    _file,                   // File
                    index + 1,               // Number
                    size,                    // Size
                    new Angle(degree),       // Rotation
                    new PointF(72.0f, 72.0f) // Resolution
                );
            }
            finally { NativeMethods.FPDF_ClosePage(page); }
        });

        /* ----------------------------------------------------------------- */
        ///
        /// GetSize
        ///
        /// <summary>
        /// Gets the page size.
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
            var w = (float)NativeMethods.FPDF_GetPageWidth(handle);
            var h = (float)NativeMethods.FPDF_GetPageHeight(handle);
            return degree % 180 == 0 ? new SizeF(w, h) : new SizeF(h, w);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetPageRotation
        ///
        /// <summary>
        /// Gets the degree of PDF page rotation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private int GetPageRotation(IntPtr handle)
        {
            var dest = NativeMethods.FPDFPage_GetRotation(handle);
            return dest == 1 ?  90 :
                   dest == 2 ? 180 :
                   dest == 3 ? 270 : 0;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases the unmanaged resources used by the object and
        /// optionally releases the managed resources.
        /// </summary>
        ///
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        protected override void Dispose(bool disposing) { }

        #endregion

        #region Fields
        private readonly PdfiumReader _core;
        private readonly PdfFile _file;
        #endregion
    }
}
