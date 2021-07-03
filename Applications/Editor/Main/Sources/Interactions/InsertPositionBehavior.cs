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
using System.Windows.Controls.Primitives;
using Cube.Xui.Behaviors;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// InsertPositionBehavior
    ///
    /// <summary>
    /// Represents the behavior when a RadioButton is checked.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class InsertPositionBehavior : CommandBehavior<ToggleButton, int>
    {
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
            AssociatedObject.Checked += WhenChecked;
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
            AssociatedObject.Checked -= WhenChecked;
            base.OnDetaching();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// WhenChecked
        ///
        /// <summary>
        /// Occurs when the Checked event is fired.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenChecked(object s, RoutedEventArgs e)
        {
            if (Command?.CanExecute(CommandParameter) ?? false) Command.Execute(CommandParameter);
        }
    }
}
