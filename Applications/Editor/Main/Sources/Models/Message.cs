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
using Cube.Pdf.Pdfium;
using Cube.Reflection.Extensions;

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
        public static DialogMessage From(Exception src) => DialogMessage.From(
            src is PdfiumException ?
            new ArgumentException(Properties.Resources.MessageOpenError) :
            src
        );

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
        public static DialogMessage ForOverwrite() => new(Properties.Resources.MessageOverwrite)
        {
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
        public static OpenFileMessage ForOpen() => new(Properties.Resources.TitleOpen)
        {
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
        public static OpenFileMessage ForInsert() => new(Properties.Resources.TitleOpen)
        {
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
        /// ForBackup
        ///
        /// <summary>
        /// Creates a message to show an open directory dialog.
        /// </summary>
        ///
        /// <returns>OpenDirectoryMessage object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static OpenDirectoryMessage ForBackup(string src) => new(Properties.Resources.TitleBackup)
        {
            NewButton = true,
            Value     = src,
        };

        /* ----------------------------------------------------------------- */
        ///
        /// ForTemp
        ///
        /// <summary>
        /// Creates a message to show an open directory dialog.
        /// </summary>
        ///
        /// <returns>OpenDirectoryMessage object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static OpenDirectoryMessage ForTemp(string src) => new(Properties.Resources.TitleTemp)
        {
            NewButton = true,
            Value     = src,
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
        public static SaveFileMessage ForExtract() => new(Properties.Resources.MenuSaveAs)
        {
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
        public static SaveFileMessage ForSave() => new(Properties.Resources.TitleSaveAs)
        {
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
