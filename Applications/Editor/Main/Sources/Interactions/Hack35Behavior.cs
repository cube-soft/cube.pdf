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
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Cube.Pdf.Editor
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
            SetKeys();
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
            base.OnDetaching();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetKeys
        ///
        /// <summary>
        /// Sets key bindings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SetKeys()
        {
            if (!(AssociatedObject.DataContext is MainViewModel vm)) return;

            var v = AssociatedObject;
            var r = vm.Ribbon;

            if (v == null || r == null) return;

            v.InputBindings.Clear();
            v.InputBindings.Add(new KeyBinding(r.Refresh.Command,      Key.F5,       ModifierKeys.None));
            v.InputBindings.Add(new KeyBinding(r.Insert.Command,       Key.Insert,   ModifierKeys.None));
            v.InputBindings.Add(new KeyBinding(r.Remove.Command,       Key.Delete,   ModifierKeys.None));
            v.InputBindings.Add(new KeyBinding(r.Extract.Command,      Key.E,        ModifierKeys.Control));
            v.InputBindings.Add(new KeyBinding(r.Open.Command,         Key.O,        ModifierKeys.Control));
            v.InputBindings.Add(new KeyBinding(r.Save.Command,         Key.S,        ModifierKeys.Control));
            v.InputBindings.Add(new KeyBinding(r.SaveAs.Command,       Key.S,        ModifierKeys.Control | ModifierKeys.Shift));
            v.InputBindings.Add(new KeyBinding(r.Close.Command,        Key.W,        ModifierKeys.Control));
            v.InputBindings.Add(new KeyBinding(r.SelectAll.Command,    Key.A,        ModifierKeys.Control));
            v.InputBindings.Add(new KeyBinding(r.SelectClear.Command,  Key.D,        ModifierKeys.Control));
            v.InputBindings.Add(new KeyBinding(r.MoveNext.Command,     Key.F,        ModifierKeys.Control));
            v.InputBindings.Add(new KeyBinding(r.MovePrevious.Command, Key.B,        ModifierKeys.Control));
            v.InputBindings.Add(new KeyBinding(r.RotateLeft.Command,   Key.L,        ModifierKeys.Control));
            v.InputBindings.Add(new KeyBinding(r.RotateRight.Command,  Key.R,        ModifierKeys.Control));
            v.InputBindings.Add(new KeyBinding(r.Metadata.Command,     Key.I,        ModifierKeys.Control));
            v.InputBindings.Add(new KeyBinding(r.Encryption.Command,   Key.K,        ModifierKeys.Control));
            v.InputBindings.Add(new KeyBinding(r.Undo.Command,         Key.Z,        ModifierKeys.Control));
            v.InputBindings.Add(new KeyBinding(r.Redo.Command,         Key.Y,        ModifierKeys.Control));
            v.InputBindings.Add(new KeyBinding(r.ZoomIn.Command,       Key.OemPlus,  ModifierKeys.Control));
            v.InputBindings.Add(new KeyBinding(r.ZoomIn.Command,       Key.Add,      ModifierKeys.Control));
            v.InputBindings.Add(new KeyBinding(r.ZoomOut.Command,      Key.OemMinus, ModifierKeys.Control));
            v.InputBindings.Add(new KeyBinding(r.ZoomOut.Command,      Key.Subtract, ModifierKeys.Control));
        }

        #endregion
    }
}
