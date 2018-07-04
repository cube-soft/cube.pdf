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
using System.Drawing;
using System.Windows.Forms;

namespace Cube.Pdf.App.Pages
{
    /* --------------------------------------------------------------------- */
    ///
    /// FileGridView
    ///
    /// <summary>
    /// 結合対象となる PDF または画像ファイル一覧を表示するクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class FileGridView : DataGridView
    {
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
        /// InitializeColumns
        ///
        /// <summary>
        /// カラムを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void InitializeColumns()
        {
            Columns.Clear();

            Columns.Add(new DataGridViewImageColumn
            {
                Name             = "Icon",
                DataPropertyName = "Icon",
                HeaderText       = Properties.Resources.ColumnIcon,
                FillWeight       = 1.0f,
                SortMode         = DataGridViewColumnSortMode.NotSortable,
            });

            Columns.Add(CreateColumn("Name",  Properties.Resources.ColumnName,   5.0f));
            Columns.Add(CreateColumn("Type",  Properties.Resources.ColumnType,   2.4f));
            Columns.Add(CreateColumn("Pages", Properties.Resources.ColumnPages,  2.0f));
            Columns.Add(CreateColumn("Date",  Properties.Resources.ColumnDate,   3.0f));
            Columns.Add(CreateColumn("Size",  Properties.Resources.ColumnLength, 2.4f));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CreateColumn
        ///
        /// <summary>
        /// 新しいカラムを生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private DataGridViewColumn CreateColumn(string name, string text, float weight) =>
            new DataGridViewColumn
            {
                Name             = name,
                DataPropertyName = name,
                HeaderText       = text,
                FillWeight       = weight,
                SortMode         = DataGridViewColumnSortMode.NotSortable,
            };

        #endregion
    }
}
