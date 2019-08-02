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
using GongSolutions.Wpf.DragDrop;
using System;
using System.Windows;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// InsertDropTarget
    ///
    /// <summary>
    /// Represents Drag&amp;Drop behavior in the InsertWindow.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class InsertDropTarget : IDropTarget
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// InsertDropTarget
        ///
        /// <summary>
        /// Initializes a new instance of the InsertDropTarget class
        /// with the specified callback.
        /// </summary>
        ///
        /// <param name="callback">Callback action when dropped.</param>
        ///
        /* ----------------------------------------------------------------- */
        public InsertDropTarget(Action<int, int> callback)
        {
            _callback = callback;
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// DragOver
        ///
        /// <summary>
        /// Invokes the dragover action.
        /// </summary>
        ///
        /// <param name="e">Dropped information.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void DragOver(IDropInfo e)
        {
            e.NotHandled = !(e.Data != e.TargetItem && e.TargetItem is FileItem);
            if (e.NotHandled) return;

            e.Effects           = DragDropEffects.Move;
            e.DropTargetAdorner = DropTargetAdorners.Insert;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Drop
        ///
        /// <summary>
        /// Invokes the drop action.
        /// </summary>
        ///
        /// <param name="e">Dropped information.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Drop(IDropInfo e) => _callback(e.DragInfo.SourceIndex, e.InsertIndex);

        #endregion

        #region Fields
        private readonly Action<int, int> _callback;
        #endregion
    }
}
