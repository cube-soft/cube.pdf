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

namespace Cube.Pdf.Pdfium
{
    /* --------------------------------------------------------------------- */
    ///
    /// FormFields
    ///
    /// <summary>
    /// Provies functionality to treat form fields by usin PDFium APIs.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal class FormFields : DisposableBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// FormFields
        ///
        /// <summary>
        /// Initializes a new instnce of the FormFields class with the
        /// specified object.
        /// </summary>
        ///
        /// <param name="src">
        /// Handle to document from FPDF_LoadDocument().
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        public FormFields(IntPtr src) { _core = Create(src); }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Render
        ///
        /// <summary>
        /// Draws the form fiels with the specified arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Render(IntPtr bitmap, IntPtr page, int x, int y, int w, int h, int rotate, int flags)
        {
            if ((flags & (int)RenderFlags.Annotation) == 0) return;
            NativeMethods.FPDF_FFLDraw(_core, bitmap, page, x, y, w, h, rotate, flags);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases the unmanaged resources used by the FormFields
        /// and optionally releases the managed resources.
        /// </summary>
        ///
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        protected override void Dispose(bool disposing) =>
            NativeMethods.FPDFDOC_ExitFormFillEnvironment(_core);

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Creates a core object for form fields.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private IntPtr Create(IntPtr src)
        {
            for (int i = 1; i <= 2; i++)
            {
                var dest = NativeMethods.FPDFDOC_InitFormFillEnvironment(src, new FormFillInfo { version = i });
                if (dest != IntPtr.Zero) return dest;
            }
            return IntPtr.Zero;
        }

        #endregion

        #region Fields
        private readonly IntPtr _core;
        #endregion
    }
}
