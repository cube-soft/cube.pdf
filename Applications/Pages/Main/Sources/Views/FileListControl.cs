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
using Cube.FileSystem;
using Cube.Logging;
using Cube.Mixin.ByteFormat;

namespace Cube.Pdf.Pages
{
    /* --------------------------------------------------------------------- */
    ///
    /// FileListControl
    ///
    /// <summary>
    /// Represents the collection view of PDF or image files to be combined.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class FileListControl : DataGridView
    {
        #region Constructorss

        /* ----------------------------------------------------------------- */
        ///
        /// FileListControl
        ///
        /// <summary>
        /// Initializes a new instance of the FileListControl class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public FileListControl()
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
            try
            {
                switch (e.ColumnIndex)
                {
                    case 1: e.Value = IoEx.GetTypeName((string)e.Value); break;
                    case 4: e.Value = ((long)e.Value).ToRoughBytes(); break;
                    default: return;
                }
                e.FormattingApplied = true;
            }
            catch (Exception err) { GetType().LogWarn(err); }
            finally { base.OnCellFormatting(e); }
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

            _ = Columns.Add(CreateColumn("Name",          Properties.Resources.ColumnName,   3.00f, false));
            _ = Columns.Add(CreateColumn("FullName",      Properties.Resources.ColumnType,   1.50f, false));
            _ = Columns.Add(CreateColumn("Count",         Properties.Resources.ColumnPages,  1.00f,  true));
            _ = Columns.Add(CreateColumn("LastWriteTime", Properties.Resources.ColumnDate,   2.00f,  true));
            _ = Columns.Add(CreateColumn("Length",        Properties.Resources.ColumnLength, 1.25f,  true));
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
        private DataGridViewColumn CreateColumn(string name, string text, float weight, bool right)
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

        #endregion
    }
}
