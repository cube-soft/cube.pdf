/* ------------------------------------------------------------------------- */
///
/// Copyright (c) 2010 CubeSoft, Inc.
///
/// This program is free software: you can redistribute it and/or modify
/// it under the terms of the GNU Affero General Public License as published
/// by the Free Software Foundation, either version 3 of the License, or
/// (at your option) any later version.
///
/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
/// GNU Affero General Public License for more details.
///
/// You should have received a copy of the GNU Affero General Public License
/// along with this program.  If not, see <http://www.gnu.org/licenses/>.
///
/* ------------------------------------------------------------------------- */
using System.Drawing;
using System.Windows.Forms;
using Cube.Conversions;

namespace Cube.Pdf.App.Clip
{
    /* --------------------------------------------------------------------- */
    ///
    /// ClipDataView
    /// 
    /// <summary>
    /// 添付ファイル一覧を表示するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ClipDataView : DataGridView
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ClipDataView
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ClipDataView() : base() { }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// OnCreateControl
        /// 
        /// <summary>
        /// コントロール生成時に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            // General settings
            AllowUserToAddRows = false;
            AllowUserToDeleteRows = false;
            AllowUserToResizeRows = false;
            AutoGenerateColumns = false;
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            BackgroundColor = SystemColors.Window;
            BorderStyle = BorderStyle.None;
            CellBorderStyle = DataGridViewCellBorderStyle.None;
            ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            Dock = DockStyle.Fill;
            GridColor = SystemColors.Control;
            ReadOnly = true;
            RowHeadersVisible = false;
            SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            if (DesignMode) return;

            // Column settings
            Columns.Clear();
            Columns.Add("Name", Properties.Resources.ColumnName);
            Columns.Add("Condition", Properties.Resources.ColumnCondition);
            Columns.Add("Length", Properties.Resources.ColumnLength);

            Columns["Name"].SortMode = DataGridViewColumnSortMode.NotSortable;
            Columns["Name"].DataPropertyName = "Name";
            Columns["Name"].FillWeight = 3.0f;

            Columns["Condition"].SortMode = DataGridViewColumnSortMode.NotSortable;
            Columns["Condition"].DataPropertyName = "Condition";
            Columns["Condition"].FillWeight = 1.0f;

            Columns["Length"].SortMode = DataGridViewColumnSortMode.NotSortable;
            Columns["Length"].DataPropertyName = "Length";
            Columns["Length"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            Columns["Length"].FillWeight = 1.2f;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnCellFormatting
        /// 
        /// <summary>
        /// セルの書式整形時に実行されます。
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

        #endregion
    }
}
