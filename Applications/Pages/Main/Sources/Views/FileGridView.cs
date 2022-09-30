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
using System.Drawing;
using System.Windows.Forms;
using Cube.ByteFormat;
using Cube.FileSystem;

namespace Cube.Pdf.Pages
{
    /* --------------------------------------------------------------------- */
    ///
    /// FileGridView
    ///
    /// <summary>
    /// Represents the collection view of PDF or image files to be combined.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class FileGridView : DataGridView
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// FileGridView
        ///
        /// <summary>
        /// Initializes a new instance of the FileGridView class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public FileGridView()
        {
            AllowUserToAddRows          = false;
            AllowUserToDeleteRows       = false;
            AllowUserToResizeRows       = false;
            AutoGenerateColumns         = false;
            AutoSizeColumnsMode         = DataGridViewAutoSizeColumnsMode.Fill;
            BackgroundColor             = SystemColors.Window;
            BorderStyle                 = BorderStyle.None;
            CellBorderStyle             = DataGridViewCellBorderStyle.None;
            ColumnHeadersBorderStyle    = DataGridViewHeaderBorderStyle.None;
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            Dock                        = DockStyle.Fill;
            GridColor                   = SystemColors.Control;
            ReadOnly                    = true;
            RowHeadersVisible           = false;
            SelectionMode               = DataGridViewSelectionMode.FullRowSelect;
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Refresh
        ///
        /// <summary>
        /// Forces the control to invalidate its client area and immediately
        /// redraw itself and any child controls.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public override void Refresh()
        {
            base.Refresh();
            for (var i = 0; i < Columns.Count; ++i) Columns[i].HeaderText = GetColumnText(i);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnCreateControl
        ///
        /// <summary>
        /// Occurs when creating the control.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (DesignMode) return;

            Columns.Clear();

            _ = Columns.Add(MakeColumn("Name",          GetColumnText(0), 3.00f, false));
            _ = Columns.Add(MakeColumn("FullName",      GetColumnText(1), 1.50f, false));
            _ = Columns.Add(MakeColumn("Count",         GetColumnText(2), 1.00f,  true));
            _ = Columns.Add(MakeColumn("LastWriteTime", GetColumnText(3), 2.00f,  true));
            _ = Columns.Add(MakeColumn("Length",        GetColumnText(4), 1.25f,  true));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnCellFormatting
        ///
        /// <summary>
        /// Occurs when the cell is formatting.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnCellFormatting(DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                switch (e.ColumnIndex)
                {
                    case 1: e.Value = Shell.GetTypeName((string)e.Value); break;
                    case 4: e.Value = ((long)e.Value).ToRoughBytes(); break;
                    default: return;
                }
                e.FormattingApplied = true;
            }
            catch (Exception err) { Logger.Warn(err); }
            finally { base.OnCellFormatting(e); }
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// MakeColumn
        ///
        /// <summary>
        /// Creates a new instance of the DataGridViewColumn with the
        /// specified arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private DataGridViewColumn MakeColumn(string name, string text, float weight, bool right)
        {
            var dest = new DataGridViewColumn
            {
                Name             = name,
                DataPropertyName = name,
                HeaderText       = text,
                FillWeight       = weight,
                SortMode         = DataGridViewColumnSortMode.NotSortable,
                CellTemplate     = new DataGridViewTextBoxCell(),
            };

            dest.DefaultCellStyle.Alignment =
                right ?
                DataGridViewContentAlignment.MiddleRight :
                DataGridViewContentAlignment.MiddleLeft;

            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetColumnText
        ///
        /// <summary>
        /// Gets a header text of the specified column.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string GetColumnText(int column) => column switch
        {
            0 => Properties.Resources.ColumnName,
            1 => Properties.Resources.ColumnType,
            2 => Properties.Resources.ColumnPages,
            3 => Properties.Resources.ColumnDate,
            4 => Properties.Resources.ColumnLength,
            _ => "Unknown"
        };

        #endregion
    }
}
