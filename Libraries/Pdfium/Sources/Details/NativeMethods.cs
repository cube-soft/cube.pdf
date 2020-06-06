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

namespace Cube.Pdf.Pdfium
{
    /* --------------------------------------------------------------------- */
    ///
    /// NativeMethods
    ///
    /// <summary>
    /// Represents APIs of the PDFium library.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class NativeMethods
    {
        #region Common

        /* ----------------------------------------------------------------- */
        ///
        /// FPDF_InitLibrary
        ///
        /// <summary>
        /// Initialize the FPDFSDK library.
        /// </summary>
        ///
        /// <seealso hcref="https://pdfium.googlesource.com/pdfium/+/master/public/fpdfview.h"/>
        ///
        /* ----------------------------------------------------------------- */
        [DllImport(LibName)]
        public static extern void FPDF_InitLibrary();

        /* ----------------------------------------------------------------- */
        ///
        /// FPDF_DestroyLibrar
        ///
        /// <summary>
        /// Release all resources allocated by the FPDFSDK library.
        /// </summary>
        ///
        /// <seealso hcref="https://pdfium.googlesource.com/pdfium/+/master/public/fpdfview.h"/>
        ///
        /* ----------------------------------------------------------------- */
        [DllImport(LibName)]
        public static extern void FPDF_DestroyLibrary();

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
        /// <seealso hcref="https://pdfium.googlesource.com/pdfium/+/master/public/fpdfview.h"/>
        ///
        /* ----------------------------------------------------------------- */
        [DllImport(LibName, CharSet = CharSet.Ansi)]
        public static extern IntPtr FPDF_LoadCustomDocument(
            [MarshalAs(UnmanagedType.LPStruct)] FileAccess access, string password);

        /* ----------------------------------------------------------------- */
        ///
        /// FPDF_CloseDocument
        ///
        /// <summary>
        /// Close a loaded PDF document.
        /// </summary>
        ///
        /// <seealso hcref="https://pdfium.googlesource.com/pdfium/+/master/public/fpdfview.h"/>
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
        /// <seealso hcref="https://pdfium.googlesource.com/pdfium/+/master/public/fpdfview.h"/>
        ///
        /* ----------------------------------------------------------------- */
        [DllImport(LibName)]
        public static extern int FPDF_GetPageCount(IntPtr document);

        /* ----------------------------------------------------------------- */
        ///
        /// FPDF_LoadPage
        ///
        /// <summary>
        /// Load a page inside the document.
        /// </summary>
        ///
        /// <remarks>
        /// page_index parameter indicates the first page as ZERO.
        /// </remarks>
        ///
        /// <seealso hcref="https://pdfium.googlesource.com/pdfium/+/master/public/fpdfview.h"/>
        ///
        /* ----------------------------------------------------------------- */
        [DllImport(LibName)]
        public static extern IntPtr FPDF_LoadPage(IntPtr document, int page_index);

        /* ----------------------------------------------------------------- */
        ///
        /// FPDF_ClosePage
        ///
        /// <summary>
        /// Close a loaded PDF page.
        /// </summary>
        ///
        /// <seealso hcref="https://pdfium.googlesource.com/pdfium/+/master/public/fpdfview.h"/>
        ///
        /* ----------------------------------------------------------------- */
        [DllImport(LibName)]
        public static extern void FPDF_ClosePage(IntPtr page);

        /* ----------------------------------------------------------------- */
        ///
        /// FPDF_GetPageWidth
        ///
        /// <summary>
        /// Get page width.
        /// </summary>
        ///
        /// <seealso hcref="https://pdfium.googlesource.com/pdfium/+/master/public/fpdfview.h"/>
        ///
        /* ----------------------------------------------------------------- */
        [DllImport(LibName)]
        public static extern double FPDF_GetPageWidth(IntPtr page);

        /* ----------------------------------------------------------------- */
        ///
        /// FPDF_GetPageHeight
        ///
        /// <summary>
        /// Get page height.
        /// </summary>
        ///
        /// <seealso hcref="https://pdfium.googlesource.com/pdfium/+/master/public/fpdfview.h"/>
        ///
        /* ----------------------------------------------------------------- */
        [DllImport(LibName)]
        public static extern double FPDF_GetPageHeight(IntPtr page);

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
        /// <seealso hcref="https://pdfium.googlesource.com/pdfium/+/master/public/fpdf_edit.h" />
        ///
        /* ----------------------------------------------------------------- */
        [DllImport(LibName)]
        public static extern int FPDFPage_GetRotation(IntPtr page);

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
        [DllImport(LibName)]
        public static extern bool FPDF_GetFileVersion(IntPtr document, out int version);

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
        [DllImport(LibName)]
        public static extern uint FPDF_GetMetaText(IntPtr document, string tag, byte[] buffer, uint buflen);

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
        [DllImport(LibName)]
        public static extern int FPDFDoc_GetPageMode(IntPtr document);

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
        [DllImport(LibName)]
        public static extern ulong FPDF_GetDocPermissions(IntPtr document);

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
        [DllImport(LibName)]
        public static extern int FPDF_GetSecurityHandlerRevision(IntPtr document);

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
        [DllImport(LibName)]
        public static extern void FPDF_RenderPage(IntPtr dc, IntPtr page,
            int start_x, int start_y, int size_x, int size_y, int rotate, int flags);

        /* ----------------------------------------------------------------- */
        ///
        /// FPDF_RenderPageBitmap
        ///
        /// <summary>
        /// Render contents of a page to a device independent bitmap.
        /// </summary>
        ///
        /// <see hcref="https://pdfium.googlesource.com/pdfium/+/master/public/fpdfview.h" />
        ///
        /* ----------------------------------------------------------------- */
        [DllImport(LibName)]
        public static extern void FPDF_RenderPageBitmap(IntPtr bitmap, IntPtr page,
            int start_x, int start_y, int size_x, int size_y, int rotate, int flags);

        [DllImport(LibName)]
        public static extern IntPtr FPDFDOC_InitFormFillEnvironment(IntPtr document, FPDF_FORMFILLINFO formInfo);

        [DllImport(LibName)]
        public static extern void FPDF_FFLDraw(IntPtr form, IntPtr bitmap, IntPtr page,
            int start_x, int start_y, int size_x, int size_y, int rotate, int flags);

        /* ----------------------------------------------------------------- */
        ///
        /// FPDFBitmap_CreateEx
        ///
        /// <summary>
        /// Create a device independent bitmap.
        /// </summary>
        ///
        /// <see hcref="https://pdfium.googlesource.com/pdfium/+/master/public/fpdfview.h" />
        ///
        /* ----------------------------------------------------------------- */
        [DllImport(LibName)]
        public static extern IntPtr FPDFBitmap_CreateEx(int width, int height,
            int format, IntPtr first, int stride);

        /* ----------------------------------------------------------------- */
        ///
        /// FPDFBitmap_Destroy
        ///
        /// <summary>
        /// Destroy a bitmap and release all related buffers.
        /// </summary>
        ///
        /// <see hcref="https://pdfium.googlesource.com/pdfium/+/master/public/fpdfview.h" />
        ///
        /* ----------------------------------------------------------------- */
        [DllImport(LibName)]
        public static extern void FPDFBitmap_Destroy(IntPtr bitmap);

        #endregion

        #region Fields
        private const string LibName = "pdfium.dll";
        #endregion

        [StructLayout(LayoutKind.Sequential)]
        public class FPDF_FORMFILLINFO
        {
            public int version;

            private IntPtr Release;

            private IntPtr FFI_Invalidate;

            private IntPtr FFI_OutputSelectedRect;

            private IntPtr FFI_SetCursor;

            private IntPtr FFI_SetTimer;

            private IntPtr FFI_KillTimer;

            private IntPtr FFI_GetLocalTime;

            private IntPtr FFI_OnChange;

            private IntPtr FFI_GetPage;

            private IntPtr FFI_GetCurrentPage;

            private IntPtr FFI_GetRotation;

            private IntPtr FFI_ExecuteNamedAction;

            private IntPtr FFI_SetTextFieldFocus;

            private IntPtr FFI_DoURIAction;

            private IntPtr FFI_DoGoToAction;

            private IntPtr m_pJsPlatform;

            // XFA support i.e. version 2

            private IntPtr FFI_DisplayCaret;

            private IntPtr FFI_GetCurrentPageIndex;

            private IntPtr FFI_SetCurrentPage;

            private IntPtr FFI_GotoURL;

            private IntPtr FFI_GetPageViewRect;

            private IntPtr FFI_PageEvent;

            private IntPtr FFI_PopupMenu;

            private IntPtr FFI_OpenFile;

            private IntPtr FFI_EmailTo;

            private IntPtr FFI_UploadTo;

            private IntPtr FFI_GetPlatform;

            private IntPtr FFI_GetLanguage;

            private IntPtr FFI_DownloadFromURL;

            private IntPtr FFI_PostRequestURL;

            private IntPtr FFI_PutRequestURL;
        }
    }
}
