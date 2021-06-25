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
using Cube.FileSystem;

namespace Cube.Pdf.Itext
{
    /* --------------------------------------------------------------------- */
    ///
    /// SaveOption
    ///
    /// <summary>
    /// Represents the options to save a PDF file with a DocumentWriter
    /// object.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class SaveOption
    {
        /* ----------------------------------------------------------------- */
        ///
        /// IO
        ///
        /// <summary>
        /// Gets or sets the I/O handler.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IO IO { get; set; } = new();

        /* ----------------------------------------------------------------- */
        ///
        /// UseSmartCopy
        ///
        /// <summary>
        /// Gets or sets the value indicating whether to use the smart
        /// copy algorithm.
        /// </summary>
        ///
        /// <remarks>
        /// DocumentWriter usually uses iTextSharp PdfCopy class for
        /// merging, but this class treats multiple PDF files as separate
        /// even if they use the same font, so font information may be
        /// duplicated, increasing the file size.
        ///
        /// The PdfSmartCopy class is a solution to this problem.
        /// However, be careful when using it to merge PDFs with complex
        /// annotations, as it has been observed that the information is
        /// shared and the annotation structure is broken.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public bool UseSmartCopy { get; set; } = true;
    }
}
