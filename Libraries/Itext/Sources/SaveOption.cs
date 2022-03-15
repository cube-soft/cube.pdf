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
        /// ShrinkResources
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to shrink deduplicated
        /// resources.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool ShrinkResources { get; set; } = true;

        /* ----------------------------------------------------------------- */
        ///
        /// Temp
        ///
        /// <summary>
        /// Gets or sets the path of the working directory. If the property
        /// is empty, the same directory as the source PDF file will be used.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Temp { get; set; }
    }
}
