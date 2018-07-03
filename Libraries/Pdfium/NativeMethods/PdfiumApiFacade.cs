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
using Cube.Log;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Cube.Pdf.Pdfium.PdfiumApi
{
    /* --------------------------------------------------------------------- */
    ///
    /// PdfiumApi.Facade
    ///
    /// <summary>
    /// PDFium API のラッパクラスです。
    /// </summary>
    ///
    /// <remarks>
    /// PDFium がスレッドセーフではないため、lock オブジェクトを利用して
    /// 実際の API を実行します。
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    internal static class Facade
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
        public static void FPDF_AddRef() =>
            Invoke(() => NativeMethods.FPDF_AddRef());

        /* ----------------------------------------------------------------- */
        ///
        /// FPDF_Release
        ///
        /// <summary>
        /// Decrement the reference counter.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static void FPDF_Release() =>
            Invoke(() => NativeMethods.FPDF_Release());

        /* ----------------------------------------------------------------- */
        ///
        /// FPDF_GetLastError
        ///
        /// <summary>
        /// Get the latest error code.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static uint FPDF_GetLastError() =>
            Invoke(() => NativeMethods.FPDF_GetLastError());

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
        public static IntPtr FPDF_LoadCustomDocument([MarshalAs(UnmanagedType.LPStruct)]
            FileAccess access, string password) =>
            Invoke(() => NativeMethods.FPDF_LoadCustomDocument(access, password));

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
        public static void FPDF_CloseDocument(IntPtr document) =>
            Invoke(() => NativeMethods.FPDF_CloseDocument(document));

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
        public static int FPDF_GetPageCount(IntPtr document) =>
            Invoke(() => NativeMethods.FPDF_GetPageCount(document));

        /* ----------------------------------------------------------------- */
        ///
        /// FPDF_LoadPage
        ///
        /// <summary>
        /// Load a page inside the document.
        /// </summary>
        ///
        /// <remarks>
        /// テスト試行の結果から、FPDF_LoadPage は原因不明の
        /// NullReferenceException が送出されるケースを多々観測しています。
        /// 現在は暫定的に、一定回数試行し、それらの試行全てが失敗した
        /// 場合に IntPtr.Zero を返す仕様としています。
        /// </remarks>
        ///
        /// <see hcref="https://pdfium.googlesource.com/pdfium/+/master/public/fpdfview.h" />
        ///
        /* ----------------------------------------------------------------- */
        public static IntPtr FPDF_LoadPage(IntPtr document, int page_index, int retry)
        {
            for (var i = 0; i < retry; ++i)
            {
                try { return Invoke(() => NativeMethods.FPDF_LoadPage(document, page_index)); }
                catch (Exception err)
                {
                    Logger.Warn(typeof(Facade), $"FPDF_LoadPage error ({i + 1}/{retry})");
                    Logger.Warn(typeof(Facade), err.ToString());
                    Task.Delay(50).Wait();
                }
            }
            return IntPtr.Zero;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// FPDF_ClosePage
        ///
        /// <summary>
        /// Close a loaded PDF page.
        /// </summary>
        ///
        /// <see hcref="https://pdfium.googlesource.com/pdfium/+/master/public/fpdfview.h" />
        ///
        /* ----------------------------------------------------------------- */
        public static void FPDF_ClosePage(IntPtr page) =>
            Invoke(() => NativeMethods.FPDF_ClosePage(page));

        /* ----------------------------------------------------------------- */
        ///
        /// FPDF_GetPageWidth
        ///
        /// <summary>
        /// Get page width.
        /// </summary>
        ///
        /// <see hcref="https://pdfium.googlesource.com/pdfium/+/master/public/fpdfview.h" />
        ///
        /* ----------------------------------------------------------------- */
        public static double FPDF_GetPageWidth(IntPtr page) =>
            Invoke(() => NativeMethods.FPDF_GetPageWidth(page));

        /* ----------------------------------------------------------------- */
        ///
        /// FPDF_GetPageHeight
        ///
        /// <summary>
        /// Get page height.
        /// </summary>
        ///
        /// <see hcref="https://pdfium.googlesource.com/pdfium/+/master/public/fpdfview.h" />
        ///
        /* ----------------------------------------------------------------- */
        public static double FPDF_GetPageHeight(IntPtr page) =>
            Invoke(() => NativeMethods.FPDF_GetPageHeight(page));

        /* ----------------------------------------------------------------- */
        ///
        /// FPDFPage_GetRotation
        ///
        /// <summary>
        /// Get the rotation of the page.
        /// </summary>
        ///
        /// <returns>
        /// Returns one of the following indicating the page rotation:
        /// 0 - No rotation.
        /// 1 - Rotated 90 degrees clockwise.
        /// 2 - Rotated 180 degrees clockwise.
        /// 3 - Rotated 270 degrees clockwise.
        /// </returns>
        ///
        /// <see hcref="https://pdfium.googlesource.com/pdfium/+/master/public/fpdf_edit.h" />
        ///
        /* ----------------------------------------------------------------- */
        public static int FPDFPage_GetRotation(IntPtr page) =>
            Invoke(() => NativeMethods.FPDFPage_GetRotation(page));

        #endregion

        #region Metadata

        /* ----------------------------------------------------------------- */
        ///
        /// FPDF_GetFileVersion
        ///
        /// <summary>
        /// Get the file version of the given PDF document.
        /// </summary>
        ///
        /// <remarks>
        /// File version: 14 for 1.4, 15 for 1.5, ...
        /// </remarks>
        ///
        /// <see hcref="https://pdfium.googlesource.com/pdfium/+/master/public/fpdfview.h" />
        ///
        /* ----------------------------------------------------------------- */
        public static bool FPDF_GetFileVersion(IntPtr document, out int version)
        {
            lock (_lock) return NativeMethods.FPDF_GetFileVersion(document, out version);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// FPDF_GetMetaText
        ///
        /// <summary>
        /// Get meta-data tag content from document.
        /// </summary>
        ///
        /// <remarks>
        /// The tag can be one of: Title, Author, Subject, Keywords,
        /// Creator, Producer, CreationDate, or ModDate.
        ///
        /// For linearized files, FPDFAvail_IsFormAvail must be called
        /// before this, and it must have returned PDF_FORM_AVAIL or
        /// PDF_FORM_NOTEXIST. Before that, there is no guarantee the
        /// metadata has been loaded.
        /// </remarks>
        ///
        /// <see hcref="https://pdfium.googlesource.com/pdfium/+/master/public/fpdf_doc.h" />
        ///
        /* ----------------------------------------------------------------- */
        public static uint FPDF_GetMetaText(IntPtr document, string tag, byte[] buffer, uint buflen) =>
            Invoke(() => NativeMethods.FPDF_GetMetaText(document, tag, buffer, buflen));

        /* ----------------------------------------------------------------- */
        ///
        /// FPDFDoc_GetPageMode
        ///
        /// <summary>
        /// Get the document's PageMode.
        /// </summary>
        ///
        /// <returns>
        /// Returns one of the following flags:
        /// -1 - Unknown page mode.
        ///  0 - Document outline, and thumbnails hidden.
        ///  1 - Document outline visible.
        ///  2 - Thumbnail images visible.
        ///  3 - Full-screen mode, no menu bar, window controls, or
        ///      other decorations visible.
        ///  4 - Optional content group panel visible.
        ///  5 - Attachments panel visible.
        /// </returns>
        ///
        /// <see hcref="https://pdfium.googlesource.com/pdfium/+/master/public/fpdf_ext.h" />
        ///
        /* ----------------------------------------------------------------- */
        public static int FPDFDoc_GetPageMode(IntPtr document) =>
            Invoke(() => NativeMethods.FPDFDoc_GetPageMode(document));

        #endregion

        #region Encryption

        /* ----------------------------------------------------------------- */
        ///
        /// FPDF_GetDocPermissions
        ///
        /// <summary>
        /// Get file permission flags of the document.
        /// </summary>
        ///
        /// <returns>
        /// If the document is not, protected, 0xffffffff will be returned.
        /// </returns>
        ///
        /// <see hcref="https://pdfium.googlesource.com/pdfium/+/master/public/fpdfview.h" />
        ///
        /* ----------------------------------------------------------------- */
        public static ulong FPDF_GetDocPermissions(IntPtr document) =>
            Invoke(() => NativeMethods.FPDF_GetDocPermissions(document));

        /* ----------------------------------------------------------------- */
        ///
        /// FPDF_GetSecurityHandlerRevision
        ///
        /// <summary>
        /// Get the revision for the security handler.
        /// </summary>
        ///
        /// <see hcref="https://pdfium.googlesource.com/pdfium/+/master/public/fpdfview.h" />
        ///
        /* ----------------------------------------------------------------- */
        public static int FPDF_GetSecurityHandlerRevision(IntPtr document) =>
            Invoke(() => NativeMethods.FPDF_GetSecurityHandlerRevision(document));

        #endregion

        #region Render

        /* ----------------------------------------------------------------- */
        ///
        /// FPDF_RenderPage
        ///
        /// <summary>
        /// Render contents of a page to a device (screen, bitmap, or
        /// printer).
        /// </summary>
        ///
        /// <see hcref="https://pdfium.googlesource.com/pdfium/+/master/public/fpdfview.h" />
        ///
        /* ----------------------------------------------------------------- */
        public static void FPDF_RenderPage(IntPtr dc, IntPtr page,
            int start_x, int start_y, int size_x, int size_y, int rotate, int flags) =>
            Invoke(() => NativeMethods.FPDF_RenderPage(dc, page, start_x, start_y, size_x, size_y, rotate, flags));

        #endregion

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Action オブジェクトを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static void Invoke(Action action)
        {
            lock (_lock) action();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Func(T) オブジェクトを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static T Invoke<T>(Func<T> func)
        {
            lock (_lock) return func();
        }

        #endregion

        #region Fields
        private static readonly string _lock = string.Intern("9864BE23-E546-491D-BADC-4E567C779742");
        #endregion
    }
}
