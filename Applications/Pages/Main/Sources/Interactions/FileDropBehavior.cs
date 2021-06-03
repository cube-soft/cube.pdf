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
    /// FileDropBehavior
    ///
    /// <summary>
    /// Represents the behavior when dragging and dropping files.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class FileDropBehavior : DisposableBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// FileDropBehavior
        ///
        /// <summary>
        /// Initializes a new instance of the FileDropBehavior class with
        /// the specified arguments..
        /// </summary>
        ///
        /// <param name="vm">ViewModel object.</param>
        /// <param name="view">View object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public FileDropBehavior(MainWindow view, MainViewModel vm)
        {
            void invoke(object s, DragEventArgs e)
            {
                if (e.Data.GetData(DataFormats.FileDrop, false) is string[] src)
                {
                    view.Activate();
                    vm.Add(src);
                }
            }

            view.AllowDrop = true;
            view.DragOver += OnDragOver;
            view.DragDrop += invoke;

            _subscriber.Add(Disposable.Create(() => view.DragOver -= OnDragOver));
            _subscriber.Add(Disposable.Create(() => view.DragDrop -= invoke));
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
            foreach (var e in _subscriber) e.Dispose();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnDragOver
        ///
        /// <summary>
        /// Occurs when dragging files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void OnDragOver(object s, DragEventArgs e) =>
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ?
                       DragDropEffects.Copy :
                       DragDropEffects.None;

        #endregion

        #region Fields
        private readonly List<IDisposable> _subscriber = new();
        #endregion
    }
}
