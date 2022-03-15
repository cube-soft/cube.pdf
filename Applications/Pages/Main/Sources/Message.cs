/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2013 CubeSoft, Inc.
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
using System.Collections.Generic;
using System.Linq;
using Cube.Mixin.Assembly;

namespace Cube.Pdf.Pages
{
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
        #region DialogMessage

        /* ----------------------------------------------------------------- */
        ///
        /// ForError
        ///
        /// <summary>
        /// Create a message to show a DialogBox with an error icon
        /// and OK button.
        /// </summary>
        ///
        /// <param name="src">Error message.</param>
        ///
        /// <returns>DialogMessage object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static DialogMessage ForError(string src) => new()
        {
            Text    = src,
            Title   = typeof(Message).Assembly.GetTitle(),
            Icon    = DialogIcon.Error,
            Buttons = DialogButtons.Ok,
        };

        #endregion

        #region FileDialogMessage

        /* ----------------------------------------------------------------- */
        ///
        /// ForAdd
        ///
        /// <summary>
        /// Creates a message to show an OpenFileDialog dialog.
        /// </summary>
        ///
        /// <returns>OpenFileMessage object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static OpenFileMessage ForAdd() => new()
        {
            Text            = Properties.Resources.TitleAdd,
            CheckPathExists = true,
            Multiselect     = true,
            Filters         = new FileDialogFilter[]
            {
                new(Properties.Resources.FilterSupported, true,
                    ".pdf", ".bmp", ".gif", ".jpg", ".jpeg", ".png", ".tiff"),
                new(Properties.Resources.FilterAll, true, ".*"),
            },
        };

        /* ----------------------------------------------------------------- */
        ///
        /// ForMerge
        ///
        /// <summary>
        /// Creates a message to show a SaveFileDialog dialog.
        /// </summary>
        ///
        /// <returns>SaveFileMessage object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static SaveFileMessage ForMerge() => new()
        {
            Text            = Properties.Resources.TitleMerge,
            OverwritePrompt = true,
            CheckPathExists = false,
            Filters         = new FileDialogFilter[]
            {
                new(Properties.Resources.FilterPdf, true, ".pdf"),
                new(Properties.Resources.FilterAll, true, ".*"),
            },
        };

        /* ----------------------------------------------------------------- */
        ///
        /// ForSplit
        ///
        /// <summary>
        /// Creates a message to show a BrowserFolder dialog.
        /// </summary>
        ///
        /// <returns>OpenDirectoryMessage object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static OpenDirectoryMessage ForSplit() => new()
        {
            Text      = Properties.Resources.TitleSplit,
            NewButton = true,
        };

        /* ----------------------------------------------------------------- */
        ///
        /// ForTemp
        ///
        /// <summary>
        /// Creates a message to show a BrowserFolder dialog.
        /// </summary>
        ///
        /// <returns>OpenDirectoryMessage object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static OpenDirectoryMessage ForTemp() => new()
        {
            Text      = Properties.Resources.TitleTemp,
            NewButton = true,
        };

        /* ----------------------------------------------------------------- */
        ///
        /// ForSelect
        ///
        /// <summary>
        /// Creates a message to select items of the specified indices.
        /// </summary>
        ///
        /// <param name="indices">Source selected indices.</param>
        /// <param name="offset">Offset to move.</param>
        /// <param name="count">Number of files.</param>
        ///
        /// <returns>SelectMessage object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static SelectMessage ForSelect(IEnumerable<int> indices, int offset, int count) => new()
        {
            Text  = string.Empty,
            Value = indices.Select(e => Math.Max(Math.Min(e + offset, count - 1), 0)),
        };

        /* ----------------------------------------------------------------- */
        ///
        /// ForPreview
        ///
        /// <summary>
        /// Creates a message to preview the specified files.
        /// </summary>
        ///
        /// <param name="src">File list.</param>
        /// <param name="indices">Source selected indices.</param>
        ///
        /// <returns>PreviewMessage object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static PreviewMessage ForPreview(IList<File> src, IEnumerable<int> indices) => new()
        {
            Value = src[indices.First()].FullName,
        };

        #endregion
    }
}
