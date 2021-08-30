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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Cube.Collections;

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
    /// <remarks>
    /// IReadOnlyList(Page) implementations is for the GetPage extended
    /// method.
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    internal sealed class PageCollection : EnumerableBase<Page>, IReadOnlyList<Page>
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
            File  = file;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// File
        ///
        /// <summary>
        /// Gets the file information of the PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PdfFile File { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Count
        ///
        /// <summary>
        /// Gets the number of PDF pages.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Count => File.Count;

        /* ----------------------------------------------------------------- */
        ///
        /// Item[int]
        ///
        /// <summary>
        /// Gets the Page object corresponding to the specified index.
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
            for (var i = 0; i < Count; ++i) yield return this[i];
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

                return new Page
                {
                    File       = File,
                    Number     = index + 1,
                    Size       = size,
                    Rotation   = new(degree),
                    Resolution = new(72.0f, 72.0f),
                };
            }
            finally { NativeMethods.FPDF_ClosePage(page); }
        });

        /* ----------------------------------------------------------------- */
        ///
        /// GetPageSize
        ///
        /// <summary>
        /// Gets the page size.
        /// </summary>
        ///
        /// <remarks>
        /// PDFium returns the rotated size, while the Page object stores it
        /// as pre-rotated information. Therefore, in some cases, the width
        /// and height information is inverted.
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
        #endregion
    }
}
