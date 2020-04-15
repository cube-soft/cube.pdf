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
using Cube.FileSystem;

namespace Cube.Pdf.Converter
{
    /* --------------------------------------------------------------------- */
    ///
    /// IoExtension
    ///
    /// <summary>
    /// Provides extended methods of the IO class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class IoExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// MoveOrCopy
        ///
        /// <summary>
        /// Moves or copies the specified file.
        /// </summary>
        /// 
        /// <param name="io">I/O handler.</param>
        /// <param name="src">Source path.</param>
        /// <param name="dest">Path to copy.</param>
        /// <param name="overwrite">Overwrite or not.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void MoveOrCopy(this IO io, string src, string dest, bool overwrite)
        {
            try { io.Move(src, dest, overwrite); }
            catch { io.Copy(src, dest, overwrite); }
        }

        #endregion
    }
}
