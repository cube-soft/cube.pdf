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
        public static DialogMessage From(Exception src)
        {
            var cvt = src switch
            {
                BackupException   => new ArgumentException(Surface.Texts.Error_Backup),
                MetadataException => new ArgumentException(Surface.Texts.Error_Metadata),
                PdfiumException   => new ArgumentException(Surface.Texts.Error_Open),
                _ => src
            };

            return DialogMessage.From(cvt);
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
        public static DialogMessage ForOverwrite() => new(Surface.Texts.Warn_Overwrite)
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
        public static OpenFileMessage ForOpen() => new(Surface.Texts.Window_Open)
        {
            CheckPathExists = true,
            Multiselect = false,
            Filters =
            [
                new(Surface.Texts.Filter_Pdf, true, ".pdf"),
                new(Surface.Texts.Filter_All, true, ".*"),
            ],
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
        public static OpenFileMessage ForInsert() => new(Surface.Texts.Window_Open)
        {
            CheckPathExists = true,
            Multiselect = true,
            Filters =
            [
                new(Surface.Texts.Filter_Insertable, true, ".pdf", ".png", ".jpg", ".jpeg", ".bmp", ".tif", ".tiff"),
                new(Surface.Texts.Filter_All, true, ".*"),
            ],
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
        public static OpenDirectoryMessage ForBackup(string src) => new(Surface.Texts.Window_Backup)
        {
            NewButton = true,
            Value = src,
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
        public static OpenDirectoryMessage ForTemp(string src) => new(Surface.Texts.Window_Temp)
        {
            NewButton = true,
            Value = src,
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
        public static SaveFileMessage ForExtract() => new(Surface.Texts.Window_Save)
        {
            OverwritePrompt = true,
            CheckPathExists = false,
            Filters =
            [
                new(Surface.Texts.Filter_Extractable, true, ".pdf", ".png"),
                new(Surface.Texts.Filter_All, true, ".*"),
            ],
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
        public static SaveFileMessage ForSave() => new(Surface.Texts.Window_Save)
        {
            OverwritePrompt = true,
            CheckPathExists = false,
            Filters =
            [
                new(Surface.Texts.Filter_Pdf, true, ".pdf"),
                new(Surface.Texts.Filter_All, true, ".*"),
            ],
        };

        #endregion
    }
}
