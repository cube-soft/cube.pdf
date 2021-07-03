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
    /// OpenOption
    ///
    /// <summary>
    /// Represents the options to open a PDF file with the DocumentReader
    /// class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class OpenOption
    {
        /* ----------------------------------------------------------------- */
        ///
        /// FullAccess
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to open with fully
        /// accessible.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool FullAccess { get; set; } = false;
    }
}
