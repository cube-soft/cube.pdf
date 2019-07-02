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
using System.Windows.Forms;

namespace Cube.Pdf.Pages
{
    /* --------------------------------------------------------------------- */
    ///
    /// FileMenuControl
    ///
    /// <summary>
    /// Represents the context menu to be displayed on the file list.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class FileMenuControl : ContextMenuStrip
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// FileMenuControl
        ///
        /// <summary>
        /// Initializes a new instance of the FileMenuControl class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public FileMenuControl()
        {
            PreviewMenu = new ToolStripMenuItem(Properties.Resources.MenuPreview);
            UpMenu      = new ToolStripMenuItem(Properties.Resources.MenuUp);
            DownMenu    = new ToolStripMenuItem(Properties.Resources.MenuDown);
            RemoveMenu  = new ToolStripMenuItem(Properties.Resources.MenuRemove);

            PreviewMenu.ShortcutKeys = Keys.Control | Keys.R;
            UpMenu.ShortcutKeys      = Keys.Control | Keys.Up;
            DownMenu.ShortcutKeys    = Keys.Control | Keys.Down;
            RemoveMenu.ShortcutKeys  = Keys.Control | Keys.D;

            Items.AddRange(new ToolStripItem[]
            {
                PreviewMenu,
                new ToolStripSeparator(),
                UpMenu,
                DownMenu,
                new ToolStripSeparator(),
                RemoveMenu,
            });
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// PreviewMenu
        ///
        /// <summary>
        /// Gets the preview menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ToolStripMenuItem PreviewMenu { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// UpMenu
        ///
        /// <summary>
        /// Gets the up menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ToolStripMenuItem UpMenu { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// DownMenu
        ///
        /// <summary>
        /// Gets the down menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ToolStripMenuItem DownMenu { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// RemoveMenu
        ///
        /// <summary>
        /// Gets the remove menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ToolStripMenuItem RemoveMenu { get; }

        #endregion
    }
}
