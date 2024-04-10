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
using System.Windows.Forms;

namespace Cube.Pdf.Pages
{
    /* --------------------------------------------------------------------- */
    ///
    /// FileContextMenu
    ///
    /// <summary>
    /// Represents the context menu to be displayed on the file list.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class FileContextMenu : ContextMenuStrip
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
        public FileContextMenu(Func<bool> predicate)
        {
            PreviewMenu = new FileContextMenuItem(Surface.Texts.Menu_Preview, predicate);
            UpMenu      = new FileContextMenuItem(Surface.Texts.Menu_Up, predicate);
            DownMenu    = new FileContextMenuItem(Surface.Texts.Menu_Down, predicate);
            RemoveMenu  = new FileContextMenuItem(Surface.Texts.Menu_Remove, predicate);

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
