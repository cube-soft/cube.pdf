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

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// HistoryItem
    ///
    /// <summary>
    /// Represents a pair of undo and redo actions.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class HistoryItem
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Undo
        ///
        /// <summary>
        /// Gets the action that represents the undo command.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Action Undo { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Redo
        ///
        /// <summary>
        /// Gets the action that represents the redo command.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Action Redo { get; set; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Invokes the specified action and creates a new instance of the
        /// HistoryItem class with the specified actions.
        /// </summary>
        ///
        /// <param name="action">Do or Redo action.</param>
        /// <param name="undo">Undo action.</param>
        ///
        /// <returns>New instance of the HistoryItem class.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static HistoryItem Invoke(Action action, Action undo)
        {
            action();
            return new HistoryItem
            {
                Undo = undo,
                Redo = action,
            };
        }

        #endregion
    }
}
