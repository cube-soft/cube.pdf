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
using System.ComponentModel;
using System.Windows;
using System.Windows.Interactivity;

namespace Cube.Pdf.App.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// Hack35Behavior
    ///
    /// <summary>
    /// .NET Framework 3.5 で動作させた時に、正常に機能しないものに対して
    /// 暫定的な回避策を実装するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class Hack35Behavior : Behavior<Window>
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

            AssociatedObject.Closing -= WhenClosing;
            AssociatedObject.Closing += WhenClosing;
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
            AssociatedObject.Closing -= WhenClosing;

            base.OnDetaching();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// WhenClosing
        ///
        /// <summary>
        /// Occurs when the Closing event is fired.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenClosing(object s, CancelEventArgs e)
        {
            if (!(AssociatedObject.DataContext is MainViewModel vm)) return;

            var cmd = vm.Ribbon.Close.Command;
            if (cmd?.CanExecute(e) ?? false) cmd.Execute(e);
        }

        #endregion
    }
}
