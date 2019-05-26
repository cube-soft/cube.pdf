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
using Cube.Images.Icons;
using Cube.Mixin.Assembly;
using Cube.Xui.Converters;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace Cube.Pdf.Editor
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
        #region Messages

        /* ----------------------------------------------------------------- */
        ///
        /// OpenMessage
        ///
        /// <summary>
        /// Creates a message to show the OpenFileDialog.
        /// </summary>
        ///
        /// <returns>OpenFileMessage object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static OpenFileMessage OpenMessage() => new OpenFileMessage
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
        /// <returns>OpenFileMessage object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static OpenFileMessage InsertMessage() => new OpenFileMessage
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
        /// <returns>SaveFileMessage object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static SaveFileMessage SaveMessage() => new SaveFileMessage
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
        /// <returns>DialogMessage object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static DialogMessage CloseMessage() => new DialogMessage
        {
            Value   = Properties.Resources.MessageOverwrite,
            Title   = Assembly.GetExecutingAssembly().GetTitle(),
            Buttons = DialogButtons.YesNoCancel,
            Icon    = DialogIcon.Warning,
        };

        #endregion

        #region Images

        /* ----------------------------------------------------------------- */
        ///
        /// IconImage
        ///
        /// <summary>
        /// Creates a icon from the specified arguments.
        /// </summary>
        ///
        /// <param name="src">File information.</param>
        /// <param name="size">Icon size.</param>
        ///
        /// <returns>Bitmap of the requested icon.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static BitmapImage IconImage(this Information src, IconSize size) =>
            src.GetIcon(size)?.ToBitmap().ToBitmapImage(true);

        #endregion
    }
}
