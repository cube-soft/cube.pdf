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

namespace Cube.Pdf
{
    /* --------------------------------------------------------------------- */
    ///
    /// ViewOption
    ///
    /// <summary>
    /// Specifies the display options of the PDF document.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Flags]
    public enum ViewOption
    {
        /// <summary>No options.</summary>
        None = 0x0000,
        /// <summary>Single page.</summary>
        SinglePage = 0x0001,
        /// <summary>One column.</summary>
        OneColumn = 0x0002,
        /// <summary>Two column (left)</summary>
        TwoColumnLeft = 0x0004,
        /// <summary>Two column (right)</summary>
        TwoColumnRight = 0x0008,
        /// <summary>Two page (left)</summary>
        TwoPageLeft = 0x0010,
        /// <summary>Two page (right)</summary>
        TwoPageRight = 0x0020,
        /// <summary>Shows only pages.</summary>
        PageOnly = 0x0040,
        /// <summary>Shows outline.</summary>
        Outline = 0x0080,
        /// <summary>Shows thumbnail of the pages.</summary>
        Thumbnail = 0x0100,
        /// <summary>Full screen mode.</summary>
        FullScreen = 0x0200,
        /// <summary>Shows optional contents.</summary>
        OptionalContent = 0x0400,
        /// <summary>Shows attached objects.</summary>
        Attachment = 0x0800,
    }

    /* --------------------------------------------------------------------- */
    ///
    /// ViewOptionFactory
    ///
    /// <summary>
    /// Provides extended methods for the ViewOption.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class ViewOptionFactory
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Creates a ViewOption object from the specified value.
        /// </summary>
        ///
        /// <param name="src">Value for options.</param>
        ///
        /// <returns>ViewOption objects.</returns>
        ///
        /// <remarks>
        /// Ignores flags that do not define in the ViewOption.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static ViewOption Create(int src) => (ViewOption)(src & 0x0fff);

        /* ----------------------------------------------------------------- */
        ///
        /// GetLayout
        ///
        /// <summary>
        /// Gets the settings for layout with specified value.
        /// </summary>
        ///
        /// <param name="src">Value for options.</param>
        ///
        /// <returns>Converted ViewOption object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static ViewOption GetLayout(this ViewOption src) => (ViewOption)((int)src & 0x003f);

        #endregion
    }
}
