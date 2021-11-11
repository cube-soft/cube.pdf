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

namespace Cube.Pdf.Clip
{
    /* --------------------------------------------------------------------- */
    ///
    /// UpdateListMessage
    ///
    /// <summary>
    /// Represents the message that the collection is changed.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class UpdateListMessage { }

    /* --------------------------------------------------------------------- */
    ///
    /// Message
    ///
    /// <summary>
    /// Provides functionality to create message objects.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class Message
    {
        #region OpenOrSaveMessage

        /* ----------------------------------------------------------------- */
        ///
        /// ForOpen
        ///
        /// <summary>
        /// Creates a message to show an OpenFileDialog dialog.
        /// </summary>
        ///
        /// <returns>OpenFileMessage object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static OpenFileMessage ForOpen() => new()
        {
            Text            = Properties.Resources.TitleOpen,
            CheckPathExists = true,
            Multiselect     = false,
            Filters         = new FileDialogFilter[] {
                new(Properties.Resources.FilterPdf, true, "*.pdf"),
                new(Properties.Resources.FilterAll, true, ".*"),
            },
        };

        /* ----------------------------------------------------------------- */
        ///
        /// ForAttach
        ///
        /// <summary>
        /// Creates a message to show an OpenFileDialog dialog.
        /// </summary>
        ///
        /// <returns>OpenFileMessage object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static OpenFileMessage ForAttach() => new()
        {
            Text            = Properties.Resources.TitleAttach,
            CheckPathExists = true,
            Multiselect     = true,
            Filters         = new FileDialogFilter[] {
                new(Properties.Resources.FilterAll, true, ".*"),
            },
        };

        #endregion
    }
}
