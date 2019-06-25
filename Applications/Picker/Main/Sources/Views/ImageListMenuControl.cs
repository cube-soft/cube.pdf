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

namespace Cube.Pdf.Picker
{
    /* --------------------------------------------------------------------- */
    ///
    /// ImageListMenuControl
    ///
    /// <summary>
    /// Represents the context menu to be displayed on the image list.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ImageListMenuControl : ContextMenuStrip
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ImageListMenuControl
        ///
        /// <summary>
        /// Initializes a new instance of the ImageListMenuControl class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ImageListMenuControl()
        {
            PreviewMenu   = new ToolStripMenuItem(Properties.Resources.MenuPreviewImage);
            SelectAllMenu = new ToolStripMenuItem(Properties.Resources.MenuSelectAll);
            SaveMenu      = new ToolStripMenuItem(Properties.Resources.MenuSave);
            RemoveMenu    = new ToolStripMenuItem(Properties.Resources.MenuRemove);

            PreviewMenu.ShortcutKeys   = Keys.Control | Keys.R;
            SelectAllMenu.ShortcutKeys = Keys.Control | Keys.A;
            SaveMenu.ShortcutKeys      = Keys.Control | Keys.S;
            RemoveMenu.ShortcutKeys    = Keys.Control | Keys.D;

            Items.AddRange(new ToolStripItem[]
            {
                PreviewMenu,
                new ToolStripSeparator(),
                SaveMenu,
                RemoveMenu,
                new ToolStripSeparator(),
                SelectAllMenu,
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
        /// SelectAll
        ///
        /// <summary>
        /// Gets the menu to select all items.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ToolStripMenuItem SelectAllMenu { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// Gets the save menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ToolStripMenuItem SaveMenu { get; }

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
