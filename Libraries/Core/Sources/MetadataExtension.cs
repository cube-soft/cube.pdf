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
namespace Cube.Pdf.Mixin
{
    /* --------------------------------------------------------------------- */
    ///
    /// MetadataExtension
    ///
    /// <summary>
    /// Describes extended methods for the <c>Metadata</c> class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class MetadataExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Copy
        ///
        /// <summary>
        /// Gets the copied <c>Metadata</c> object.
        /// </summary>
        ///
        /// <param name="src">Original object.</param>
        ///
        /// <returns>Copied object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Metadata Copy(this Metadata src) => new Metadata
        {
            Context        = src.Context,
            IsSynchronous  = src.IsSynchronous,
            Title          = src.Title,
            Author         = src.Author,
            Subject        = src.Subject,
            Keywords       = src.Keywords,
            Version        = src.Version,
            Creator        = src.Creator,
            Producer       = src.Producer,
            Viewer    = src.Viewer,
        };

        #endregion
    }
}
