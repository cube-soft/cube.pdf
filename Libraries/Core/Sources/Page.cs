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
using Cube.DataContract;

namespace Cube.Pdf
{
    /* --------------------------------------------------------------------- */
    ///
    /// Page
    ///
    /// <summary>
    /// Stores a page information of the document.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Serializable]
    public class Page : SerializableBase
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// File
        ///
        /// <summary>
        /// Get the file information that owns this Page.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public File File { get; init; }

        /* ----------------------------------------------------------------- */
        ///
        /// Number
        ///
        /// <summary>
        /// Get the page number.
        /// </summary>
        ///
        /// <remarks>
        /// 1 for first page.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public int Number { get; init; }

        /* ----------------------------------------------------------------- */
        ///
        /// Rotation
        ///
        /// <summary>
        /// Get the rotation of this Page.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Angle Rotation { get; init; }

        /* ----------------------------------------------------------------- */
        ///
        /// Resolution
        ///
        /// <summary>
        /// Get the horizontal and vertical resolution (dpi) of this Page.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PointF Resolution { get; init; }

        /* ----------------------------------------------------------------- */
        ///
        /// Size
        ///
        /// <summary>
        /// Get the page size.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public SizeF Size { get; init; }

        /* ----------------------------------------------------------------- */
        ///
        /// Delta
        ///
        /// <summary>
        /// Get or set the angle you rotate this Page.
        /// </summary>
        ///
        /// <remarks>
        /// The rotation result of this Page is calculated by
        /// Rotation + Delta.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public Angle Delta
        {
            get => Get(() => new Angle());
            set => Set(value);
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Reset
        ///
        /// <summary>
        /// Reset the values of editable properties.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Reset() => OnReset();

        /* ----------------------------------------------------------------- */
        ///
        /// OnReset
        ///
        /// <summary>
        /// Reset the values of editable properties.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnReset() => Delta = new();

        #endregion
    }
}
