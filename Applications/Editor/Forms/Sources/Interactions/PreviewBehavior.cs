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
using Cube.Xui;
using Cube.Xui.Behaviors;
using Cube.Xui.Mixin;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Cube.Pdf.App.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// PreviewBehavior
    ///
    /// <summary>
    /// Represents the behavior when PreviewMouseDown or MouseDoubleClick
    /// events are fired in the ScrollViewer/ListView control.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class PreviewBehavior : CommandBehavior<ScrollViewer>
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// PreCommand
        ///
        /// <summary>
        /// Gets or sets the command when the PreviewMouseDown event
        /// is fired.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ICommand PreCommand
        {
            get => GetValue(PreCommandProperty) as ICommand;
            set => SetValue(PreCommandProperty, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// PreCommandProperty
        ///
        /// <summary>
        /// Gets the DependencyProperty object for the PreCommand property.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static readonly DependencyProperty PreCommandProperty =
            DependencyFactory.Create<PreviewBehavior, ICommand>(
                nameof(PreCommand), (s, e) => s.PreCommand = e
            );

        #endregion

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

            AssociatedObject.PreviewMouseDown -= WhenMouseDown;
            AssociatedObject.PreviewMouseDown += WhenMouseDown;

            AssociatedObject.MouseDoubleClick -= WhenDoubleClick;
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
            AssociatedObject.PreviewMouseDown -= WhenMouseDown;
            AssociatedObject.MouseDoubleClick -= WhenDoubleClick;

            base.OnDetaching();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// WhenMouseDown
        ///
        /// <summary>
        /// Occurs when the PreviewMouseDown event is fired.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenMouseDown(object s, MouseButtonEventArgs e)
        {
            if (IsKeyPresses()) return;
            if (PreCommand.CanExecute()) PreCommand.Execute();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// WhenDoubleClick
        ///
        /// <summary>
        /// Occurs when the MouseDoubleClick event is fired.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && Command.CanExecute()) Command.Execute();
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
