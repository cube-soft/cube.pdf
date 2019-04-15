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
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Cube.Pdf.Picker
{
    /* --------------------------------------------------------------------- */
    ///
    /// DropForm
    ///
    /// <summary>
    /// メイン画面を表示するクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public partial class DropForm : Cube.Forms.BorderlessForm
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// DropForm
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public DropForm()
        {
            InitializeComponent();
            InitializeEvents();
            InitializePresenters();

            AllowExtensions.Add(".pdf");
            //Caption = DropPanel;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// DropForm
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public DropForm(string[] src)
            : this()
        {
            OnOpen(ValueEventArgs.Create(src));
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// AllowExtensions
        ///
        /// <summary>
        /// ドラッグ&amp;ドロップを受け付けるファイルの拡張子一覧を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IList<string> AllowExtensions { get; } = new List<string>();

        #endregion

        #region Events

        /* ----------------------------------------------------------------- */
        ///
        /// Open
        ///
        /// <summary>
        /// ファイルを開くときに発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public event EventHandler<ValueEventArgs<string[]>> Open;

        #endregion

        #region Virtual methods

        /* ----------------------------------------------------------------- */
        ///
        /// OnOpen
        ///
        /// <summary>
        /// Open イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnOpen(ValueEventArgs<string[]> e)
            => Open?.Invoke(this, e);

        #endregion

        #region Override methods

        /* ----------------------------------------------------------------- */
        ///
        /// OnLoad
        ///
        /// <summary>
        /// フォームが表示される直前に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            InitializeLayout();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnReceived
        ///
        /// <summary>
        /// 他プロセスからデータ受信時に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnReceived(EnumerableEventArgs<string> e)
        {
            base.OnReceived(e);
            OnOpen(ValueEventArgs.Create(e.Value.ToArray()));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnNcHitTest
        ///
        /// <summary>
        /// ヒットテスト時に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnNcHitTest(QueryEventArgs<Point, Cube.Forms.Position> e)
        {
            base.OnNcHitTest(e);
            if (e.Result == Cube.Forms.Position.Caption) ShowCloseButton();
            else HideCloseButton();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnDragEnter
        ///
        /// <summary>
        /// ファイルがドラッグされた時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnDragEnter(DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ?
                       DragDropEffects.Copy :
                       DragDropEffects.None;
            base.OnDragEnter(e);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnDragDrop
        ///
        /// <summary>
        /// ファイルがドロップされた時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnDragDrop(DragEventArgs e)
        {
            base.OnDragDrop(e);
            var files = e.Data.GetData(DataFormats.FileDrop, false) as string[];
            OnOpen(ValueEventArgs.Create(files));
        }

        #endregion

        #region Initialize methods

        /* ----------------------------------------------------------------- */
        ///
        /// InitializeLayout
        ///
        /// <summary>
        /// メイン画面のレイアウトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void InitializeLayout()
        {
            Width  = DropPanel.BackgroundImage.Width;
            Height = DropPanel.BackgroundImage.Height;

            StartPosition = FormStartPosition.Manual;
            var area = Screen.GetWorkingArea(this);
            var x = area.Width - Width - 10;
            var y = 10;
            Location = new Point(x, y);

            var cx = Width - ExitButton.Width - 1;
            var cy = 1;
            ExitButton.Location = new Point(cx, cy);
            ExitButton.Image = null;
            ExitButton.Cursor = Cursors.Hand;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// InitializeEvents
        ///
        /// <summary>
        /// 各種イベントを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void InitializeEvents()
        {
            ExitButton.Click      += (s, e) => Close();
            ExitButton.MouseEnter += (s, e) => ShowCloseButton();
            ExitButton.MouseLeave += (s, e) => HideCloseButton();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// InitializePresenters
        ///
        /// <summary>
        /// 各種 Presenter を初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void InitializePresenters()
        {
            new DropPresenter(this);
        }

        #endregion

        #region Others

        /* ----------------------------------------------------------------- */
        ///
        /// ShowCloseButton
        ///
        /// <summary>
        /// 閉じるボタンを表示します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void ShowCloseButton() => ExitButton.Image = Properties.Resources.CloseButton;

        /* ----------------------------------------------------------------- */
        ///
        /// HideCloseButton
        ///
        /// <summary>
        /// 閉じるボタンを非表示にします。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void HideCloseButton() => ExitButton.Image = null;

        #endregion
    }
}
