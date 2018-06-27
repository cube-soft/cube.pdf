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
    /// PdfLibrary
    ///
    /// <summary>
    /// PDFium API を利用するための基底クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal abstract class PdfLibrary : IDisposable
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// PdfLibrary
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected PdfLibrary()
        {
            _dispose = new OnceAction<bool>(Dispose);
            NativeMethods.FPDF_AddRef();
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// PdfLibrary
        ///
        /// <summary>
        /// オブジェクトを破棄します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        ~PdfLibrary() { _dispose.Invoke(false); }

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// リソースを開放します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Dispose()
        {
            _dispose.Invoke(true);
            GC.SuppressFinalize(this);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// リソースを開放します。
        /// </summary>
        ///
        /// <param name="disposing">
        /// マネージオブジェクトを開放するかどうか
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void Dispose(bool disposing)
        {
            NativeMethods.FPDF_Release();
        }

        #endregion

        #region Fields
        private OnceAction<bool> _dispose;
        #endregion
    }
}
