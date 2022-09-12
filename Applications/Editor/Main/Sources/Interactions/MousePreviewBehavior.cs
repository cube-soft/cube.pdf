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
using System.Windows.Controls;
using System.Windows.Input;
using Cube.Xui.Behaviors;
using Cube.Xui.Commands.Extensions;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// MousePreviewBehavior
    ///
    /// <summary>
    /// Represents the action to show a preview dialog through the mouse
    /// event.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class MousePreviewBehavior : CommandBehavior<ListView>
    {
        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// OnAttached
        ///
        /// <summary>
        /// Called when the action is attached to an AssociatedObject.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseDoubleClick += WhenDoubleClick;
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
            AssociatedObject.MouseDoubleClick -= WhenDoubleClick;
            base.OnDetaching();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// WhenMouseDown
        ///
        /// <summary>
        /// Occurs when the MouseDoubleClick event is fired.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenDoubleClick(object s, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) return;
            var pt  = e.GetPosition(AssociatedObject);
            var obj = AssociatedObject.GetObject<ListViewItem>(pt);
            if (obj != null && Command != null && Command.CanExecute()) Command.Execute();
        }

        #endregion
    }
}
