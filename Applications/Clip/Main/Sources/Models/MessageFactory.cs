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
    /// CollectionMessage
    ///
    /// <summary>
    /// Represents the message that the collection is changed.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class CollectionMessage { }

    /* --------------------------------------------------------------------- */
    ///
    /// MessageFactory
    ///
    /// <summary>
    /// Provides functionality to create message objects.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class MessageFactory
    {
        #region OpenOrSaveMessage

        /* ----------------------------------------------------------------- */
        ///
        /// CreateForOpen
        ///
        /// <summary>
        /// Creates a message to show an OpenFileDialog dialog.
        /// </summary>
        ///
        /// <returns>OpenFileMessage object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static OpenFileMessage CreateForOpen() => new OpenFileMessage
        {
            Title           = Properties.Resources.TitleOpen,
            CheckPathExists = true,
            Multiselect     = false,
            Filter          = new[]
            {
                new ExtensionFilter(Properties.Resources.FilterPdf, true, "*.pdf"),
                new ExtensionFilter(Properties.Resources.FilterAll, true, ".*"),
            }.GetFilter(),
        };

        /* ----------------------------------------------------------------- */
        ///
        /// CreateForAttach
        ///
        /// <summary>
        /// Creates a message to show an OpenFileDialog dialog.
        /// </summary>
        ///
        /// <returns>OpenFileMessage object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static OpenFileMessage CreateForAttach() => new OpenFileMessage
        {
            Title           = Properties.Resources.TitleAttach,
            CheckPathExists = true,
            Multiselect     = true,
            Filter          = new[]
            {
                new ExtensionFilter(Properties.Resources.FilterAll, true, ".*"),
            }.GetFilter(),
        };

        #endregion
    }
}
