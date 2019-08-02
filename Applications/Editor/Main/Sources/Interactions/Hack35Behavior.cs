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
            _ = v.InputBindings.Add(new KeyBinding(r.Redraw.Command,        Key.F5,       ModifierKeys.None));
            _ = v.InputBindings.Add(new KeyBinding(r.Insert.Command,        Key.Insert,   ModifierKeys.None));
            _ = v.InputBindings.Add(new KeyBinding(r.Insert.Command,        Key.I,        ModifierKeys.Control));
            _ = v.InputBindings.Add(new KeyBinding(r.InsertOthers.Command,  Key.I,        ModifierKeys.Control | ModifierKeys.Shift));
            _ = v.InputBindings.Add(new KeyBinding(r.Remove.Command,        Key.Delete,   ModifierKeys.None));
            _ = v.InputBindings.Add(new KeyBinding(r.Remove.Command,        Key.D,        ModifierKeys.Control));
            _ = v.InputBindings.Add(new KeyBinding(r.RemoveOthers.Command,  Key.D,        ModifierKeys.Control | ModifierKeys.Shift));
            _ = v.InputBindings.Add(new KeyBinding(r.Extract.Command,       Key.E,        ModifierKeys.Control));
            _ = v.InputBindings.Add(new KeyBinding(r.ExtractOthers.Command, Key.E,        ModifierKeys.Control | ModifierKeys.Shift));
            _ = v.InputBindings.Add(new KeyBinding(r.Open.Command,          Key.O,        ModifierKeys.Control));
            _ = v.InputBindings.Add(new KeyBinding(r.Save.Command,          Key.S,        ModifierKeys.Control));
            _ = v.InputBindings.Add(new KeyBinding(r.SaveAs.Command,        Key.S,        ModifierKeys.Control | ModifierKeys.Shift));
            _ = v.InputBindings.Add(new KeyBinding(r.Close.Command,         Key.W,        ModifierKeys.Control));
            _ = v.InputBindings.Add(new KeyBinding(r.SelectAll.Command,     Key.A,        ModifierKeys.Control));
            _ = v.InputBindings.Add(new KeyBinding(r.MoveNext.Command,      Key.F,        ModifierKeys.Control));
            _ = v.InputBindings.Add(new KeyBinding(r.MovePrevious.Command,  Key.B,        ModifierKeys.Control));
            _ = v.InputBindings.Add(new KeyBinding(r.RotateLeft.Command,    Key.L,        ModifierKeys.Control));
            _ = v.InputBindings.Add(new KeyBinding(r.RotateRight.Command,   Key.R,        ModifierKeys.Control));
            _ = v.InputBindings.Add(new KeyBinding(r.Metadata.Command,      Key.M,        ModifierKeys.Control));
            _ = v.InputBindings.Add(new KeyBinding(r.Encryption.Command,    Key.K,        ModifierKeys.Control));
            _ = v.InputBindings.Add(new KeyBinding(r.Undo.Command,          Key.Z,        ModifierKeys.Control));
            _ = v.InputBindings.Add(new KeyBinding(r.Redo.Command,          Key.Y,        ModifierKeys.Control));
            _ = v.InputBindings.Add(new KeyBinding(r.ZoomIn.Command,        Key.OemPlus,  ModifierKeys.Control));
            _ = v.InputBindings.Add(new KeyBinding(r.ZoomIn.Command,        Key.Add,      ModifierKeys.Control));
            _ = v.InputBindings.Add(new KeyBinding(r.ZoomOut.Command,       Key.OemMinus, ModifierKeys.Control));
            _ = v.InputBindings.Add(new KeyBinding(r.ZoomOut.Command,       Key.Subtract, ModifierKeys.Control));
        }

        #endregion
    }
}
