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
using System.Windows;
using Cube.Mixin.Commands;
using Cube.Xui.Behaviors;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// SetupBehavior
    ///
    /// <summary>
    /// Represents the behavior when an Windows is shown.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class SetupBehavior : CommandBehavior<Window>
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
            AssociatedObject.ContentRendered += WhenContentRendered;
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
            AssociatedObject.ContentRendered -= WhenContentRendered;
            base.OnDetaching();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// WhenContentRendered
        ///
        /// <summary>
        /// Called when the window is shown.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenContentRendered(object s, EventArgs e)
        {
            var ok = Command?.CanExecute() ?? false;
            if (ok) Command.Execute();
        }

        #endregion
    }
}
