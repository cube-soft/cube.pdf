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

namespace Cube.Pdf.App.Clip
{
    /* --------------------------------------------------------------------- */
    ///
    /// ClipListView
    /// 
    /// <summary>
    /// 添付ファイル一覧を表示するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ClipListView : DataGridView
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ClipListView
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ClipListView() : base()
        {
            InitializeLayout();
            InitializeColumns();
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// InitializeLayout
        /// 
        /// <summary>
        /// 各種レイアウトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void InitializeLayout()
        {
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
            GridColor = SystemColors.Control;
            ReadOnly = true;
            RowHeadersVisible = false;
            SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// InitializeColumns
        /// 
        /// <summary>
        /// カラムの内容を初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void InitializeColumns()
        {
            if (DesignMode) return;

            Columns.Clear();
            Columns.Add("Name", Properties.Resources.ColumnName);
            Columns.Add("Condition", Properties.Resources.ColumnCondition);
            Columns.Add("Dummy", "");

            Columns["Name"].SortMode = DataGridViewColumnSortMode.NotSortable;
            Columns["Name"].DataPropertyName = "Name";
            Columns["Name"].Width = 200;

            Columns["Condition"].SortMode = DataGridViewColumnSortMode.NotSortable;
            Columns["Condition"].DataPropertyName = "Condition";
            Columns["Condition"].Width = 80;

            Columns["Dummy"].SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        #endregion
    }
}
