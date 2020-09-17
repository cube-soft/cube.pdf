/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2013 CubeSoft, Inc.
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
using System.Windows.Forms;

namespace Cube.Pdf.Pages
{
    /* --------------------------------------------------------------------- */
    ///
    /// FileContextMenuItem
    ///
    /// <summary>
    /// Represents the context menu item to be displayed on the file list.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal class FileContextMenuItem : ToolStripMenuItem
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// FileContextMenuItem
        ///
        /// <summary>
        /// Initializes a new instance of the FileContextMenuItem class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="text">Displayed text.</param>
        /// <param name="predicate">
        /// Value indicating whether the item is selectable.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        public FileContextMenuItem(string text, Func<bool> predicate) : base(text)
        {
            _predicate = predicate;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// CanSelect
        ///
        /// <summary>
        /// Gets a value indicating whether the item is selectable.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public override bool CanSelect => _predicate();

        #endregion

        #region Fields
        private readonly Func<bool> _predicate;
        #endregion
    }
}
