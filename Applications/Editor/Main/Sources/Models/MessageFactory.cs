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
using Cube.Mixin.Assembly;
using System;
using System.Reflection;

namespace Cube.Pdf.Editor
{
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
        #region DialogMessage

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Create a message to show a DialogBox with an error icon
        /// and OK button.
        /// </summary>
        ///
        /// <param name="src">Occurred exception.</param>
        ///
        /// <returns>DialogMessage object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static DialogMessage Create(Exception src) =>
            CreateError($"{src.Message} ({src.GetType().Name})");

        /* ----------------------------------------------------------------- */
        ///
        /// CreateOverwriteWarn
        ///
        /// <summary>
        /// Creates a message to show a MessageBox of overwriting
        /// confirmation.
        /// </summary>
        ///
        /// <returns>DialogMessage object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static DialogMessage CreateOverwriteWarn() => new DialogMessage
        {
            Value   = Properties.Resources.MessageOverwrite,
            Title   = Assembly.GetExecutingAssembly().GetTitle(),
            Buttons = DialogButtons.YesNoCancel,
            Icon    = DialogIcon.Warning,
        };

        /* ----------------------------------------------------------------- */
        ///
        /// CreateError
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
        public static DialogMessage CreateError(string src) => new DialogMessage
        {
            Value   = src,
            Title   = Assembly.GetExecutingAssembly().GetTitle(),
            Icon    = DialogIcon.Error,
            Buttons = DialogButtons.Ok,
        };

        #endregion

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
            Filter          = new []
            {
                new ExtensionFilter(Properties.Resources.FilterPdf, true, ".pdf"),
                new ExtensionFilter(Properties.Resources.FilterAll, true, ".*"),
            }.GetFilter(),
        };

        /* ----------------------------------------------------------------- */
        ///
        /// CreateForInsert
        ///
        /// <summary>
        /// Creates a message to show an OpenFileDialog dialog.
        /// </summary>
        ///
        /// <returns>OpenFileMessage object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static OpenFileMessage CreateForInsert() => new OpenFileMessage
        {
            Title           = Properties.Resources.TitleOpen,
            CheckPathExists = true,
            Multiselect     = true,
            Filter          = new []
            {
                new ExtensionFilter(Properties.Resources.FilterInsertable, true, ".pdf", ".png", ".jpg", ".jpeg", ".bmp"),
                new ExtensionFilter(Properties.Resources.FilterAll, true, ".*"),
            }.GetFilter(),
        };

        /* ----------------------------------------------------------------- */
        ///
        /// CreateForSave
        ///
        /// <summary>
        /// Creates a message to show a SaveFileDialog dialog.
        /// </summary>
        ///
        /// <returns>SaveFileMessage object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static SaveFileMessage CreateForSave() => new SaveFileMessage
        {
            Title           = Properties.Resources.TitleSaveAs,
            OverwritePrompt = true,
            CheckPathExists = false,
            Filter          = new[]
            {
                new ExtensionFilter(Properties.Resources.FilterPdf, true, ".pdf"),
                new ExtensionFilter(Properties.Resources.FilterAll, true, ".*"),
            }.GetFilter(),
        };

        #endregion
    }
}
