/* ------------------------------------------------------------------------- */
///
/// FileListView.cs
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
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Cube.Pdf.App.Page
{
    /* --------------------------------------------------------------------- */
    ///
    /// FileListView
    /// 
    /// <summary>
    /// ファイル一覧を表示するための ListView クラスです。
    /// </summary>
    /// 
    /* --------------------------------------------------------------------- */
    public class FileListView : Cube.Forms.ListView
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// FileListView
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public FileListView() : base() { }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Aggregator
        /// 
        /// <summary>
        /// イベントを集約したオブジェクトを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public EventAggregator Aggregator { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// MouseDownLocation
        /// 
        /// <summary>
        /// MouseDown イベントが発生した時の Location を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected Point MouseDownLocation { get; set; }

        #endregion

        #region Override methods

        /* ----------------------------------------------------------------- */
        ///
        /// OnCreateControl
        /// 
        /// <summary>
        /// コントロールが生成された時に実行されます。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            AllowDrop        = true;
            BorderStyle      = BorderStyle.None;
            DoubleBuffered   = true;
            FullRowSelect    = true;
            HeaderStyle      = ColumnHeaderStyle.Nonclickable;
            ShowItemToolTips = true;
            Theme            = Cube.Forms.WindowTheme.Explorer;
            View             = View.Details;

            InitializeColumns();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnMouseDown
        /// 
        /// <summary>
        /// マウスが押下された時に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            MouseDownLocation = e.Location;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnMouseUp
        /// 
        /// <summary>
        /// マウスが押下された時に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            MouseDownLocation = Point.Empty;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnMouseMove
        /// 
        /// <summary>
        /// マウスが移動した時に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.Button != MouseButtons.Left) return;
            DoDragMove(e.Location);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnDragEnter
        /// 
        /// <summary>
        /// 項目がドラッグ移動された時に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnDragEnter(DragEventArgs e)
        {
            var prev = e.Effect;
            base.OnDragEnter(e);
            if (e.Effect != prev) return;

            e.Effect = e.Data.GetDataPresent(typeof(ListViewItem)) ?
                       DragDropEffects.Move :
                       DragDropEffects.None;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnDragDrop
        /// 
        /// <summary>
        /// 項目がドロップされた時に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnDragDrop(DragEventArgs e)
        {
            try
            {
                base.OnDragDrop(e);

                var item = e.Data.GetData(typeof(ListViewItem)) as ListViewItem;
                if (item == null) return;

                var src = Items.IndexOf(item);
                if (src == -1) return;

                var point = PointToClient(new Point(e.X, e.Y));
                int dest = Items.IndexOf(GetItemAt(point.X, point.Y));
                if (dest == -1) dest = Items.Count - 1;

                Aggregator?.Move.Raise(ValueEventArgs.Create(dest - src));
            }
            finally { MouseDownLocation = Point.Empty; }
        }

        #endregion

        #region Others

        /* ----------------------------------------------------------------- */
        ///
        /// InitializeColumns
        /// 
        /// <summary>
        /// Columns オブジェクトを初期化します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private void InitializeColumns()
        {
            Columns?.Clear();

            var file = new ColumnHeader();
            var type = new ColumnHeader();
            var page = new ColumnHeader();
            var date = new ColumnHeader();
            var size = new ColumnHeader();

            Columns?.AddRange(new ColumnHeader[] {
                file, type, page, date, size
            });

            file.Text  = Properties.Resources.ColumnFile;
            file.Width = 180;

            type.Text  = Properties.Resources.ColumnType;
            type.Width = 90;

            page.Text  = Properties.Resources.ColumnPage;
            page.Width = 70;

            date.Text  = Properties.Resources.ColumnDate;
            date.Width = 120;

            size.Text  = Properties.Resources.ColumnSize;
            size.Width = 80;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// DoDragMove
        /// 
        /// <summary>
        /// ドラッグ移動を実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void DoDragMove(Point location)
        {
            if (MouseDownLocation == Point.Empty) return;

            var dx = Math.Abs(location.X - MouseDownLocation.X);
            var dy = Math.Abs(location.Y - MouseDownLocation.Y);
            if (dx < SystemInformation.DoubleClickSize.Width &&
                dy < SystemInformation.DoubleClickSize.Height) return;

            var item = GetItemAt(location.X, location.Y);
            if (item == null) return;

            DoDragDrop(item, DragDropEffects.Move);
        }

        #endregion
    }
}
