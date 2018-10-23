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
using Cube.Xui;
using System.Reflection;

namespace Cube.Pdf.App.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// Factory
    ///
    /// <summary>
    /// Provides functionality to create objects.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class Factory
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// OpenMessage
        ///
        /// <summary>
        /// Creates a message to show the OpenFileDialog.
        /// </summary>
        ///
        /// <param name="callback">
        /// Callback action when terminating the user operation.
        /// </param>
        ///
        /// <returns>OpenFileMessage object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static OpenFileMessage OpenMessage(OpenFileCallback callback) =>
            new OpenFileMessage(callback)
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
        /// InsertMessage
        ///
        /// <summary>
        /// Creates a message to show the OpenFileDialog.
        /// </summary>
        ///
        /// <param name="callback">
        /// Callback action when terminating the user operation.
        /// </param>
        ///
        /// <returns>OpenFileMessage object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static OpenFileMessage InsertMessage(OpenFileCallback callback) =>
            new OpenFileMessage(callback)
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
        /// SaveMessage
        ///
        /// <summary>
        /// Creates a message to show the SaveFileDialog.
        /// </summary>
        ///
        /// <param name="callback">
        /// Callback action when terminating the user operation.
        /// </param>
        ///
        /// <returns>SaveFileMessage object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static SaveFileMessage SaveMessage(SaveFileCallback callback) =>
            new SaveFileMessage(callback)
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

        /* ----------------------------------------------------------------- */
        ///
        /// CloseMessage
        ///
        /// <summary>
        /// Creates a message to show the MessageBox of overwriting
        /// confirmation.
        /// </summary>
        ///
        /// <param name="callback">
        /// Callback action when terminating the user operation.
        /// </param>
        ///
        /// <returns>DialogMessage object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static DialogMessage CloseMessage(DialogCallback callback) =>
            new DialogMessage(Properties.Resources.MessageOverwrite,
            Assembly.GetExecutingAssembly(), callback)
        {
            Button = System.Windows.MessageBoxButton.YesNoCancel,
            Image  = System.Windows.MessageBoxImage.Information,
        };

        #endregion
    }
}
