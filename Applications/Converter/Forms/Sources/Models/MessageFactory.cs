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
using Cube.Forms;
using Cube.Generics;

namespace Cube.Pdf.App.Converter
{
    /* --------------------------------------------------------------------- */
    ///
    /// MessageFactory
    ///
    /// <summary>
    /// Provides functionality to create messsage objects.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class MessageFactory
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// CreateSource
        ///
        /// <summary>
        /// Creates a message to show the OpenFileDialog.
        /// </summary>
        ///
        /// <param name="src">Initial value for the FileName.</param>
        /// <param name="io">I/O handler.</param>
        ///
        /// <returns>OpenFileEventArgs object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static OpenFileEventArgs CreateSource(string src, IO io) => new OpenFileEventArgs
        {
            Title            = Properties.Resources.TitleBrowseSource,
            FileName         = GetFileName(src, io),
            Multiselect      = false,
            Filter           = ViewResource.SourceFilters.GetFilter(),
            FilterIndex      = ViewResource.SourceFilters.GetFilterIndex(src, io),
        };

        /* ----------------------------------------------------------------- */
        ///
        /// CreateDestination
        ///
        /// <summary>
        /// Creates a message to show the SaveFileDialog.
        /// </summary>
        ///
        /// <param name="src">Initial value for the FileName.</param>
        /// <param name="io">I/O handler.</param>
        ///
        /// <returns>SaveFileEventArgs object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static SaveFileEventArgs CreateDestination(string src, IO io) => new SaveFileEventArgs
        {
            Title            = Properties.Resources.TitleBroseDestination,
            FileName         = GetFileName(src, io),
            OverwritePrompt  = false,
            Filter           = ViewResource.DestinationFilters.GetFilter(),
            FilterIndex      = ViewResource.DestinationFilters.GetFilterIndex(src, io),
        };

        /* ----------------------------------------------------------------- */
        ///
        /// CreateUserProgram
        ///
        /// <summary>
        /// Creates a message to show the OpenFileDialog.
        /// </summary>
        ///
        /// <param name="src">Initial value for the FileName.</param>
        /// <param name="io">I/O handler.</param>
        ///
        /// <returns>OpenFileEventArgs object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static OpenFileEventArgs CreateUserProgram(string src, IO io) => new OpenFileEventArgs
        {
            Title            = Properties.Resources.TitleBroseUserProgram,
            FileName         = GetFileName(src, io),
            Multiselect      = false,
            Filter           = ViewResource.UserProgramFilters.GetFilter(),
        };

        /* ----------------------------------------------------------------- */
        ///
        /// CreateWarning
        ///
        /// <summary>
        /// Create a message to show the DialogBox with a warning icon
        /// and OK/Cancel buttons.
        /// </summary>
        ///
        /// <param name="src">Description to be shown.</param>
        ///
        /// <returns>MessageEventArgs object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static MessageEventArgs CreateWarning(string src) => new MessageEventArgs(
            src,
            Properties.Resources.TitleWarning,
            System.Windows.Forms.MessageBoxButtons.OKCancel,
            System.Windows.Forms.MessageBoxIcon.Warning
        );

        /* ----------------------------------------------------------------- */
        ///
        /// CreateError
        ///
        /// <summary>
        /// Create a message to show the DialogBox with an error icon
        /// and OK button.
        /// </summary>
        ///
        /// <param name="src">Description to be shown.</param>
        ///
        /// <returns>MessageEventArgs object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static MessageEventArgs CreateError(string src) => new MessageEventArgs(
            src,
            Properties.Resources.TitleError,
            System.Windows.Forms.MessageBoxButtons.OK,
            System.Windows.Forms.MessageBoxIcon.Error
        );

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetFileName
        ///
        /// <summary>
        /// Gets a filename without extension.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static string GetFileName(string src, IO io) =>
            src.HasValue() ? io.Get(src).NameWithoutExtension : string.Empty;

        /* ----------------------------------------------------------------- */
        ///
        /// GetDirectoryName
        ///
        /// <summary>
        /// Gets a directory name.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static string GetDirectoryName(string src, IO io) =>
            src.HasValue() ? io.Get(src).DirectoryName : string.Empty;

        #endregion
    }
}
