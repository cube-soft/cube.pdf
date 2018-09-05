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
    /// History
    ///
    /// <summary>
    /// Provides functionality to undo and redo actions.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class History
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Undoable
        ///
        /// <summary>
        /// Gets the value indicating whether any of undo actions exist.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Undoable => _forward.Count > 0;

        /* ----------------------------------------------------------------- */
        ///
        /// Redoable
        ///
        /// <summary>
        /// Gets the value indicating whether any of redo actions exist.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Redoable => _reverse.Count > 0;

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Register
        ///
        /// <summary>
        /// Registers a pair of forward (do or redo) and reverse (undo)
        /// actions, and then executes the forward action.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Register(Action forward, Action reverse)
        {
            _reverse.Clear();
            _forward.Push(new Item { Undo = reverse, Redo = forward });
            forward();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Clear
        ///
        /// <summary>
        /// Removes all of undo and redo actions.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Clear()
        {
            _forward.Clear();
            _reverse.Clear();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Undo
        ///
        /// <summary>
        /// Executes the undo action.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Undo()
        {
            if (!Undoable) return;
            var item = _forward.Pop();
            item.Undo();
            _reverse.Push(item);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Redo
        ///
        /// <summary>
        /// Executes the redo action.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Redo()
        {
            if (!Redoable) return;
            var item = _reverse.Pop();
            item.Redo();
            _forward.Push(item);
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Item
        ///
        /// <summary>
        /// Represents a pair of undo and redo actions.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private class Item
        {
            public Action Undo { get; set; }
            public Action Redo { get; set; }
        }

        #endregion

        #region Fields
        private readonly Stack<Item> _forward = new Stack<Item>();
        private readonly Stack<Item> _reverse = new Stack<Item>();
        #endregion
    }
}
