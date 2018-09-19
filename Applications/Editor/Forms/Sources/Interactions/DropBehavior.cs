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
using Cube.Generics;
using Cube.Xui.Behaviors;
using System;
using System.Linq;
using System.Windows;

namespace Cube.Pdf.App.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// DropBehavior
    ///
    /// <summary>
    /// Represents the behavior when files are dropped.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class DropBehavior : CommandBehavior<Window>
    {
        #region Implementations

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
            AssociatedObject.PreviewDragOver -= WhenDragOver;
            AssociatedObject.PreviewDragOver += WhenDragOver;
            AssociatedObject.PreviewDrop     -= WhenDrop;
            AssociatedObject.PreviewDrop     += WhenDrop;
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
            AssociatedObject.PreviewDrop     -= WhenDrop;
            base.OnDetaching();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// WhenDrop
        ///
        /// <summary>
        /// Occurs when the <c>PreviewDrop</c> event is fired.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenDrop(object s, DragEventArgs e)
        {
            var dest = GetFirst(e.Data);
            e.Handled = dest.HasValue() && (Command?.CanExecute(dest) ?? false);
            if (e.Handled) Command.Execute(dest);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// WhenDragOver
        ///
        /// <summary>
        /// Occurs when the <c>PreviewDragOver</c> event is fired.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenDragOver(object s, DragEventArgs e)
        {
            var dest = GetFirst(e.Data);
            var ok   = dest.HasValue() && (Command?.CanExecute(dest) ?? false);

            e.Effects = ok ? DragDropEffects.Copy : DragDropEffects.None;
            e.Handled = true;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetFirst
        ///
        /// <summary>
        /// Gets the first item that represents the path of PDF file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string GetFirst(IDataObject src) =>
            src.GetData(DataFormats.FileDrop)
               .TryCast<string[]>()?
               .First(e => e.EndsWith(".pdf", StringComparison.InvariantCultureIgnoreCase));

        #endregion
    }
}
