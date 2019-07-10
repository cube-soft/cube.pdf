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
using System.Text;

namespace Cube.Pdf.Pdfium
{
    /* --------------------------------------------------------------------- */
    ///
    /// MetadataFactory
    ///
    /// <summary>
    /// Provides factory methods of the Metadata class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class MetadataFactory
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Creates a new instance of the Metadata class with the core
        /// object.
        /// </summary>
        ///
        /// <param name="core">PDFium object.</param>
        ///
        /// <returns>Metadata object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Metadata Create(PdfiumReader core) => core.Invoke(e => new Metadata
        {
            Version  = GetVersion(e),
            Title    = GetText(e, nameof(Metadata.Title)),
            Author   = GetText(e, nameof(Metadata.Author)),
            Subject  = GetText(e, nameof(Metadata.Subject)),
            Keywords = GetText(e, nameof(Metadata.Keywords)),
            Creator  = GetText(e, nameof(Metadata.Creator)),
            Producer = GetText(e, nameof(Metadata.Producer)),
            Options  = GetPageMode(e),
        });

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetText
        ///
        /// <summary>
        /// Gets the metadata corresponding to the specified name.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static string GetText(IntPtr core, string name)
        {
            var size = NativeMethods.FPDF_GetMetaText(core, name, null, 0);
            if (size <= 2) return string.Empty;

            var buffer = new byte[size];
            NativeMethods.FPDF_GetMetaText(core, name, buffer, size);
            return Encoding.Unicode.GetString(buffer, 0, (int)(size - 2));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetVersion
        ///
        /// <summary>
        /// Gets the PDF version.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static PdfVersion GetVersion(IntPtr core) =>
            NativeMethods.FPDF_GetFileVersion(core, out var dest) ?
            new PdfVersion(dest / 10, dest % 10) :
            new PdfVersion(1, 7);

        /* ----------------------------------------------------------------- */
        ///
        /// GetPageMode
        ///
        /// <summary>
        /// Gets the page mode of the specified PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static ViewerOptions GetPageMode(IntPtr core)
        {
            var m = NativeMethods.FPDFDoc_GetPageMode(core);
            return new Dictionary<int, ViewerOptions> {
                { 1, ViewerOptions.Outline         },
                { 2, ViewerOptions.Thumbnail       },
                { 3, ViewerOptions.FullScreen      },
                { 4, ViewerOptions.OptionalContent },
                { 5, ViewerOptions.Attachment      },
            }.TryGetValue(m, out var dest) ? dest : ViewerOptions.None;
        }

        #endregion
    }
}
