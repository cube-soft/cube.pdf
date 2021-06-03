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
using System.Collections.Generic;
using System.Windows.Forms;

namespace Cube.Pdf.Pages
{
    /* --------------------------------------------------------------------- */
    ///
    /// SelectionBehavior
    ///
    /// <summary>
    /// Represents the behavior about selected items.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class SelectionBehavior : DisposableBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// SelectionBehavior
        ///
        /// <summary>
        /// Initializes a new instance of the SelectionBehavior class with
        /// the specified arguments.
        /// </summary>
        ///
        /// <param name="view">View object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public SelectionBehavior(DataGridView view)
        {
            view.MouseUp += OnMouseUp;
            _disposables.Add(Disposable.Create(() => view.MouseUp -= OnMouseUp));
        }

        #endregion

        #region Implementations

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
        protected override void Dispose(bool disposing)
        {
            foreach (var e in _disposables) e.Dispose();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnMouseUp
        ///
        /// <summary>
        /// Occurs when the MouseUp event is raised.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void OnMouseUp(object s, MouseEventArgs e)
        {
            if (s is DataGridView view && view.HitTest(e.X, e.Y) == DataGridView.HitTestInfo.Nowhere)
            {
                view.ClearSelection();
                view.CurrentCell = null;
            }
        }

        #endregion

        #region Fields
        private readonly IList<IDisposable> _disposables = new List<IDisposable>();
        #endregion
    }
}
