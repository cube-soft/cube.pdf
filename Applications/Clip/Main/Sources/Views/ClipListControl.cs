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
using Cube.Mixin.ByteFormat;
using System.Drawing;
using System.Windows.Forms;

namespace Cube.Pdf.Clip
{
    /* --------------------------------------------------------------------- */
    ///
    /// ClipListControl
    ///
    /// <summary>
    /// Represents the collection of attachments.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class ClipListControl : DataGridView
    {
        #region Implementations

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

            if (!DesignMode) InitializeColumns();
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
            if (e.ColumnIndex == 2)
            {
                try
                {
                    e.Value = ((long)e.Value).ToRoughBytes();
                    e.FormattingApplied = true;
                }
                catch { /* use default format */ }
            }
            base.OnCellFormatting(e);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// InitializeColumns
        ///
        /// <summary>
        /// Initializes the layout of columns.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void InitializeColumns()
        {
            Columns.Clear();

            _ = Columns.Add(CreateColumn("Name",   Properties.Resources.ColumnName,   3.0f, true ));
            _ = Columns.Add(CreateColumn("Status", Properties.Resources.ColumnStatus, 1.0f, true ));
            _ = Columns.Add(CreateColumn("Length", Properties.Resources.ColumnLength, 1.2f, false));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CreateColumn
        ///
        /// <summary>
        /// Creates a new instance of the DataGridViewColumn with the
        /// specified arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private DataGridViewColumn CreateColumn(string name, string text, float weight, bool left)
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
                left ?
                DataGridViewContentAlignment.MiddleLeft :
                DataGridViewContentAlignment.MiddleRight;

            return dest;
        }

        #endregion
    }
}
