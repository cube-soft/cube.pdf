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
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// Keys
    ///
    /// <summary>
    /// Represents some keyboard features.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class Keys
    {
        /* ----------------------------------------------------------------- */
        ///
        /// ModifierKeys
        ///
        /// <summary>
        /// Gets the collection of modifier keys.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static IEnumerable<Key> ModifierKeys { get; } = new[]
        {
            Key.LeftCtrl,
            Key.LeftAlt,
            Key.LeftShift,
            Key.LWin,
            Key.RightCtrl,
            Key.RightAlt,
            Key.RightShift,
            Key.RWin,
        };

        /* ----------------------------------------------------------------- */
        ///
        /// IsPressed
        ///
        /// <summary>
        /// Gets a value indicating whether any of the specified keys
        /// are pressed.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static bool IsPressed(this IEnumerable<Key> src) =>
            src.Any(e => Keyboard.IsKeyDown(e));
    }
}
