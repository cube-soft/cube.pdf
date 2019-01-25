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
        public static Metadata Create(PdfiumReader core) => new Metadata
        {
            Version  = GetVersion(core),
            Title    = GetText(core, nameof(Metadata.Title)),
            Author   = GetText(core, nameof(Metadata.Author)),
            Subject  = GetText(core, nameof(Metadata.Subject)),
            Keywords = GetText(core, nameof(Metadata.Keywords)),
            Creator  = GetText(core, nameof(Metadata.Creator)),
            Producer = GetText(core, nameof(Metadata.Producer)),
            Options  = GetPageMode(core),
        };

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
        private static string GetText(PdfiumReader core, string name)
        {
            var size = core.Invoke(e => PdfiumApi.FPDF_GetMetaText(e, name, null, 0));
            if (size <= 2) return string.Empty;

            var buffer = new byte[size];
            core.Invoke(e => PdfiumApi.FPDF_GetMetaText(e, name, buffer, size));
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
        private static Version GetVersion(PdfiumReader core) => core.Invoke(
            e => PdfiumApi.FPDF_GetFileVersion(e, out var version) ?
            new Version(version / 10, version % 10) :
            new Version(1, 7)
        );

        /* ----------------------------------------------------------------- */
        ///
        /// GetPageMode
        ///
        /// <summary>
        /// Gets the page mode of the specified PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static ViewerOptions GetPageMode(PdfiumReader core)
        {
            var m = core.Invoke(e => PdfiumApi.FPDFDoc_GetPageMode(e));
            return new Dictionary<int, ViewerOptions>
            {
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
