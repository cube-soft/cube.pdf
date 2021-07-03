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

namespace Cube.Pdf.Editor
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
    public sealed class History : ObservableBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// History
        ///
        /// <summary>
        /// Initializes a new instance of the History class wit the
        /// specified dispatcher.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public History(Dispatcher dispatcher) : base(dispatcher) { }

        #endregion

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
        /// Registers a history item.
        /// </summary>
        ///
        /// <param name="item">History item.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Register(HistoryItem item) => Invoke(() =>
        {
            _reverse.Clear();
            _forward.Push(item);
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Clear
        ///
        /// <summary>
        /// Removes all of undo and redo actions.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Clear() => Invoke(() =>
        {
            _forward.Clear();
            _reverse.Clear();
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Undo
        ///
        /// <summary>
        /// Executes the undo action.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Undo() => Invoke(() =>
        {
            if (!Undoable) return;
            var item = _forward.Pop();
            item.Undo();
            _reverse.Push(item);
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Redo
        ///
        /// <summary>
        /// Executes the redo action.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Redo() => Invoke(() =>
        {
            if (!Redoable) return;
            var item = _reverse.Pop();
            item.Redo();
            _forward.Push(item);
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases the unmanaged resources used by the object and
        /// optionally releases the managed resources.
        /// </summary>
        ///
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        protected override void Dispose(bool disposing) { }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Invokes the action and raises property changed events.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Invoke(Action action)
        {
            action();
            Refresh(nameof(Undoable), nameof(Redoable));
        }

        #endregion

        #region Fields
        private readonly Stack<HistoryItem> _forward = new Stack<HistoryItem>();
        private readonly Stack<HistoryItem> _reverse = new Stack<HistoryItem>();
        #endregion
    }
}
