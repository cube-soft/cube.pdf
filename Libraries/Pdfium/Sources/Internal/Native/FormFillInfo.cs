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
    /// FormFillInfo
    ///
    /// <summary>
    /// Represents structures to tread form fields.
    /// </summary>
    ///
    /// <seealso chref="https://pdfium.googlesource.com/pdfium/+/master/public/fpdf_formfill.h" />
    ///
    /* --------------------------------------------------------------------- */
    [StructLayout(LayoutKind.Sequential)]
    internal class FormFillInfo
    {
        /* ----------------------------------------------------------------- */
        ///
        /// FPDF_InitLibrary
        ///
        /// <summary>
        /// Version number of the interface.
        /// Version 1 contains stable interfaces.Version 2 has additional
        /// experimental interfaces.
        /// When PDFium is built without the XFA module, version can be 1 or 2.
        /// With version 1, only stable interfaces are called.With version 2,
        /// additional experimental interfaces are also called.
        /// When PDFium is built with the XFA module, version must be 2.
        /// All the XFA related interfaces are experimental.If PDFium is
        /// built with the XFA module and version 1 then none of the XFA
        /// related interfaces would be called. When PDFium is built with
        /// XFA module then the version must be 2.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int version;

        #region Common fields
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
        #endregion

        #region XFA support i.e. version 2
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
        #endregion
    }
}
