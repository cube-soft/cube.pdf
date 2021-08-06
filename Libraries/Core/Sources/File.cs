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
using System.Drawing;
using Cube.FileSystem;

namespace Cube.Pdf
{
    /* --------------------------------------------------------------------- */
    ///
    /// File
    ///
    /// <summary>
    /// Represents information of PDF and image files.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Serializable]
    public abstract class File : Entity
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// File
        ///
        /// <summary>
        /// Initializes a new instance of the File class with the specified
        /// arguments.
        /// </summary>
        ///
        /// <param name="src">Information object of the source file.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected File(EntitySource src) : base(src) { }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Count
        ///
        /// <summary>
        /// Gets the number of pages.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Count { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Resolution
        ///
        /// <summary>
        /// Gets the resolution of the PDF or image object.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PointF Resolution { get; set; }

        #endregion
    }
}
