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
using System.Windows;
using Cube.Xui.Behaviors;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// FileDropBehavior
    ///
    /// <summary>
    /// Represents the behavior when files are dropped.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class FileDropBehavior : CommandBehavior<Window>
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// OnAttached
        ///
        /// <summary>
        /// Called after the action is attached to an AssociatedObject.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewDragOver += WhenDragOver;
            AssociatedObject.PreviewDrop += WhenDrop;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnDetaching
        ///
        /// <summary>
        /// Called when the action is being detached from its
        /// AssociatedObject, but before it has actually occurred.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnDetaching()
        {
            AssociatedObject.PreviewDragOver -= WhenDragOver;
            AssociatedObject.PreviewDrop -= WhenDrop;
            base.OnDetaching();
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// WhenDrop
        ///
        /// <summary>
        /// Occurs when the PreviewDrop event is fired.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenDrop(object s, DragEventArgs e)
        {
            e.Handled = Command?.CanExecute(e) ?? false;
            if (e.Handled) Command.Execute(e);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// WhenDragOver
        ///
        /// <summary>
        /// Occurs when the PreviewDragOver event is fired.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenDragOver(object s, DragEventArgs e)
        {
            e.Handled = Command?.CanExecute(e) ?? false;
            e.Effects = e.Handled ? DragDropEffects.Copy : DragDropEffects.None;
        }

        #endregion
    }
}
