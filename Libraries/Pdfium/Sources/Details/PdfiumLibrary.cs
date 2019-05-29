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
namespace Cube.Pdf.Pdfium
{
    /* --------------------------------------------------------------------- */
    ///
    /// PdfiumLibrary
    ///
    /// <summary>
    /// Represents the base class to access the PDFium API.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal abstract class PdfiumLibrary
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// PdfLibrary
        ///
        /// <summary>
        /// Initializes a new instance of the PdfiumLibrary class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected PdfiumLibrary() { if (!_core.Invoked) _core.Invoke(); }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetLastError
        ///
        /// <summary>
        /// Gets a LoadException object with the last error value.
        /// </summary>
        ///
        /// <returns>LoadException object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public LoadException GetLastError() => new LoadException(PdfiumApi.FPDF_GetLastError());

        #endregion

        #region Fields
        private static readonly DisposableOnceAction _core = new DisposableOnceAction(
            () => PdfiumApi.FPDF_InitLibrary(),
            e => PdfiumApi.FPDF_DestroyLibrary()
        );
        #endregion
    }
}
