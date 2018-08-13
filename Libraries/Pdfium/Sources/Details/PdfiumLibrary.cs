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

namespace Cube.Pdf.Pdfium
{
    /* --------------------------------------------------------------------- */
    ///
    /// PdfiumLibrary
    ///
    /// <summary>
    /// PDFium API を利用するための基底クラスです。
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
        protected PdfiumLibrary() { _once.Invoke(); }

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
        public LoadException GetLastError() =>
            new LoadException(Facade.FPDF_GetLastError());

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Initialize
        ///
        /// <summary>
        /// Initializes the PDFium library.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static void Initialize() => _core = new PdfiumCore();

        /* ----------------------------------------------------------------- */
        ///
        /// PdfiumCore
        ///
        /// <summary>
        /// Initializes and destroys the PDFium library.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private sealed class PdfiumCore : IDisposable
        {
            public PdfiumCore() { System.Diagnostics.Trace.WriteLine("FPDF_InitLibrary"); Facade.FPDF_InitLibrary(); }
            ~PdfiumCore() { Dispose(false); }
            public void Dispose() { Dispose(true); GC.SuppressFinalize(this); }
            private void Dispose(bool _)
            {
                if (_disposed) return;
                _disposed = true;
                Facade.FPDF_DestroyLibrary();
            }

            private bool _disposed = false;
        }

        #endregion

        #region Fields
        private static readonly OnceAction _once = new OnceAction(Initialize);
        private static PdfiumCore _core;
        #endregion
    }
}
