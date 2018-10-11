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
using Cube.Xui.Behaviors;
using Cube.Xui.Mixin;
using System.Windows.Controls;
using System.Windows.Input;

namespace Cube.Pdf.App.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// MouseClearBehavior
    ///
    /// <summary>
    /// Represents the action to clear selection through the mouse event.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class MouseClearBehavior : CommandBehavior<ListView>
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
            AssociatedObject.PreviewMouseLeftButtonDown += WhenMouseDown;
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
            AssociatedObject.PreviewMouseLeftButtonDown -= WhenMouseDown;
            base.OnDetaching();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// WhenMouseDown
        ///
        /// <summary>
        /// Occurs when the MouseDown event is fired.
        /// </summary>
        ///
        /// <remarks>
        /// TODO: 右端のスクロールバー領域を適当な値で判定しているので
        /// 修正方法を要検討。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenMouseDown(object s, MouseButtonEventArgs e)
        {
            if (IsKeyPresses()) return;

            var pt = e.GetPosition(AssociatedObject);
            if (pt.X >= AssociatedObject.ActualWidth - 16) return;

            var obj = AssociatedObject.GetObject<ListViewItem>(pt);
            if (obj?.IsSealed ?? false) return;

            if (Command?.CanExecute() ?? false) Command?.Execute();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// IsKeyPressed
        ///
        /// <summary>
        /// Gets a value indicating whether the Ctrl or Shift key is
        /// pressed.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private bool IsKeyPresses() =>
            (Keyboard.GetKeyStates(Key.LeftShift)  & KeyStates.Down) == KeyStates.Down ||
            (Keyboard.GetKeyStates(Key.RightShift) & KeyStates.Down) == KeyStates.Down ||
            (Keyboard.GetKeyStates(Key.LeftCtrl)   & KeyStates.Down) == KeyStates.Down ||
            (Keyboard.GetKeyStates(Key.RightCtrl)  & KeyStates.Down) == KeyStates.Down;

        #endregion
    }
}
