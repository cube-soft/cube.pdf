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
using Cube.FileSystem;
using Cube.Mixin.Assembly;
using Cube.Pdf.Pdfium;

namespace Cube.Pdf.Editor
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
        /// From
        ///
        /// <summary>
        /// Creates a message to show a MessageBox of error confirmation.
        /// </summary>
        ///
        /// <param name="src">Source exception.</param>
        ///
        /// <returns>DialogMessage object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static DialogMessage From(Exception src)
        {
            var dest = DialogMessage.From(src);
            if (src is PdfiumException e) dest.Text = $"{Properties.Resources.MessageOpenError} ({(int)e.Status})";
            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ForOverwrite
        ///
        /// <summary>
        /// Creates a message to show a MessageBox of overwriting
        /// confirmation.
        /// </summary>
        ///
        /// <returns>DialogMessage object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static DialogMessage ForOverwrite() => new()
        {
            Text    = Properties.Resources.MessageOverwrite,
            Title   = typeof(App).Assembly.GetTitle(),
            Buttons = DialogButtons.YesNoCancel,
            Icon    = DialogIcon.Warning,
        };

        #endregion

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
            Filters         = new FileDialogFilter[]
            {
                new(Properties.Resources.FilterPdf, true, ".pdf"),
                new(Properties.Resources.FilterAll, true, ".*"),
            },
        };

        /* ----------------------------------------------------------------- */
        ///
        /// ForInsert
        ///
        /// <summary>
        /// Creates a message to show an OpenFileDialog dialog.
        /// </summary>
        ///
        /// <returns>OpenFileMessage object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static OpenFileMessage ForInsert() => new()
        {
            Text            = Properties.Resources.TitleOpen,
            CheckPathExists = true,
            Multiselect     = true,
            Filters         = new FileDialogFilter[]
            {
                new(Properties.Resources.FilterInsertable, true, ".pdf", ".png", ".jpg", ".jpeg", ".bmp"),
                new(Properties.Resources.FilterAll, true, ".*"),
            },
        };

        /* ----------------------------------------------------------------- */
        ///
        /// ForExtract
        ///
        /// <summary>
        /// Creates a message to show a SaveFileDialog dialog.
        /// </summary>
        ///
        /// <returns>SaveFileMessage object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static SaveFileMessage ForExtract() => new()
        {
            Text            = Properties.Resources.MenuSaveAs,
            OverwritePrompt = true,
            CheckPathExists = false,
            Filters         = new FileDialogFilter[]
            {
                new(Properties.Resources.FilterExtract, true, ".pdf", ".png"),
                new(Properties.Resources.FilterAll, true, ".*"),
            },
        };

        /* ----------------------------------------------------------------- */
        ///
        /// ForSave
        ///
        /// <summary>
        /// Creates a message to show a SaveFileDialog dialog.
        /// </summary>
        ///
        /// <returns>SaveFileMessage object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static SaveFileMessage ForSave() => new()
        {
            Text            = Properties.Resources.TitleSaveAs,
            OverwritePrompt = true,
            CheckPathExists = false,
            Filters         = new FileDialogFilter[]
            {
                new(Properties.Resources.FilterPdf, true, ".pdf"),
                new(Properties.Resources.FilterAll, true, ".*"),
            },
        };

        #endregion
    }
}
