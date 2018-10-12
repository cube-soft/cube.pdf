/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
/* ------------------------------------------------------------------------- */
using System;
using System.Collections.Generic;

namespace Cube.Pdf.App.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// DragDropObject
    ///
    /// <summary>
    /// Represents information for the Drag&amp;Drop behavior.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Serializable]
    public class DragDropObject
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Pid
        ///
        /// <summary>
        /// Gets or sets the process ID of the dragged application.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Pid { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// DragIndex
        ///
        /// <summary>
        /// Gets or sets the index of the dragged item.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int DragIndex { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// DropIndex
        ///
        /// <summary>
        /// Gets or sets the index of the dropped item.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int DropIndex { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Pages
        ///
        /// <summary>
        /// Gets or sets the collection of selected pages when dragging.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<Page> Pages { get; set; }

        #endregion
    }
}
