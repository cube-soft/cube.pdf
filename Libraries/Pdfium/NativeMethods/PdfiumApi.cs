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
using System.Runtime.InteropServices;

namespace Cube.Pdf.Pdfium.PdfiumApi
{
    /* --------------------------------------------------------------------- */
    ///
    /// PdfiumApi.NativeMethods
    ///
    /// <summary>
    /// PDFium の API を定義したクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal class NativeMethods
    {
        #region Methods

        #region Common

        /* ----------------------------------------------------------------- */
        ///
        /// FPDF_AddRef
        ///
        /// <summary>
        /// Increment the reference counter.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DllImport(LibName)]
        public static extern void FPDF_AddRef();

        /* ----------------------------------------------------------------- */
        ///
        /// FPDF_Release
        ///
        /// <summary>
        /// Decrement the reference counter.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DllImport(LibName)]
        public static extern void FPDF_Release();

        /* ----------------------------------------------------------------- */
        ///
        /// FPDF_GetLastError
        ///
        /// <summary>
        /// Get the latest error code.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DllImport(LibName)]
        public static extern uint FPDF_GetLastError();

        #endregion

        #region Document

        /* ----------------------------------------------------------------- */
        ///
        /// FPDF_LoadCustomDocument
        ///
        /// <summary>
        /// Load PDF document from a custom access descriptor.
        /// </summary>
        ///
        /// <see hcref="https://pdfium.googlesource.com/pdfium/+/master/public/fpdfview.h" />
        ///
        /* ----------------------------------------------------------------- */
        [DllImport(LibName, CharSet = CharSet.Ansi)]
        public static extern IntPtr FPDF_LoadCustomDocument(
            [MarshalAs(UnmanagedType.LPStruct)] FileAccess access,
            string password
        );

        /* ----------------------------------------------------------------- */
        ///
        /// FPDF_CloseDocument
        ///
        /// <summary>
        /// Close a loaded PDF document.
        /// </summary>
        ///
        /// <see hcref="https://pdfium.googlesource.com/pdfium/+/master/public/fpdfview.h" />
        ///
        /* ----------------------------------------------------------------- */
        [DllImport(LibName)]
        public static extern void FPDF_CloseDocument(IntPtr document);

        #endregion

        #region Page

        /* ----------------------------------------------------------------- */
        ///
        /// FPDF_GetPageCount
        ///
        /// <summary>
        /// Get total number of pages in the document.
        /// </summary>
        ///
        /// <see hcref="https://pdfium.googlesource.com/pdfium/+/master/public/fpdfview.h" />
        ///
        /* ----------------------------------------------------------------- */
        [DllImport(LibName)]
        public static extern int FPDF_GetPageCount(IntPtr document);

        #endregion

        #endregion

        #region Fields
        private const string LibName = "pdfium.dll";
        #endregion
    }
}
