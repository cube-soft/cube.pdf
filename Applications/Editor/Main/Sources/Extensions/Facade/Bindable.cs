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

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// BindableExtension
    ///
    /// <summary>
    /// Represents the extended methods of the MainBindable class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class BindableExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Clear
        ///
        /// <summary>
        /// Clears the current properties.
        /// </summary>
        ///
        /// <param name="src">Source object.</param>
        /// <param name="cache">Renderer cache.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Clear(this MainBindable src, RendererCache cache)
        {
            cache?.Clear();

            src.Source     = null;
            src.Metadata   = null;
            src.Encryption = null;

            src.History.Clear();
            src.Images.Clear();

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetMetadata
        ///
        /// <summary>
        /// Sets the Metadata object.
        /// </summary>
        ///
        /// <param name="src">Source object.</param>
        /// <param name="value">Metadata object.</param>
        ///
        /// <returns>
        /// History item to execute undo and redo actions.
        /// </returns>
        ///
        /* ----------------------------------------------------------------- */
        public static HistoryItem SetMetadata(this MainBindable src, Metadata value)
        {
            var prev = src.Metadata;
            return HistoryItem.CreateInvoke(
                () => src.Metadata = value,
                () => src.Metadata = prev
            );
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetEncryption
        ///
        /// <summary>
        /// Sets the Encryption object.
        /// </summary>
        ///
        /// <param name="src">Source object.</param>
        /// <param name="value">Encryption object.</param>
        ///
        /// <returns>
        /// History item to execute undo and redo actions.
        /// </returns>
        ///
        /* ----------------------------------------------------------------- */
        public static HistoryItem SetEncryption(this MainBindable src, Encryption value)
        {
            var prev = src.Encryption;
            return HistoryItem.CreateInvoke(
                () => src.Encryption = value,
                () => src.Encryption = prev
            );
        }

        #endregion
    }
}
