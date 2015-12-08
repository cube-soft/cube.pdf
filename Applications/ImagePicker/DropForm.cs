/* ------------------------------------------------------------------------- */
///
/// DropForm.cs
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
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Cube.Pdf.ImageEx
{
    /* --------------------------------------------------------------------- */
    ///
    /// Cube.Pdf.ImageEx.DropForm
    ///
    /// <summary>
    /// メイン画面を表示するクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public partial class DropForm : Cube.Forms.WidgetForm
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
            InitializeToolTip();

            AllowExtensions.Add(".pdf");

            ExitButton.Click += (s, e) => Close();
            ExitButton.MouseEnter += (s, e) => ShowCloseButton(Cursors.Hand);
            ExitButton.MouseLeave += (s, e) => HideCloseButton();
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
            Create(src);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// AllowExtensions
        /// 
        /// <summary>
        /// ドラッグ&ドロップを受け付けるファイルの拡張子一覧を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IList<string> AllowExtensions { get; } = new List<string>();

        #endregion

        #region Override methods

        /* ----------------------------------------------------------------- */
        ///
        /// OnMouseEnter
        /// 
        /// <summary>
        /// マウスポインタがフォーム内に移動した時に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnMouseEnter(EventArgs e)
        {
            ShowCloseButton(Cursors.Default);
            base.OnMouseEnter(e);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnMouseLeave
        /// 
        /// <summary>
        /// マウスポインタがフォーム外に移動した時に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnMouseLeave(EventArgs e)
        {
            HideCloseButton();
            base.OnMouseLeave(e);
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
            if (files == null) return;
            Create(files);
        }


        /* ----------------------------------------------------------------- */
        ///
        /// OnLoad
        /// 
        /// <summary>
        /// フォームが表示される直前に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnLoad(EventArgs arg)
        {
            InitializeLayout();
            base.OnLoad(arg);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnReceived
        /// 
        /// <summary>
        /// 他プロセスからデータ受信時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnReceived(DataEventArgs<object> e)
        {
            base.OnReceived(e);

            var args = e.Value as string[];
            if (args != null) Create(args);
        }

        #endregion

        #region Other private methods


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
            Width  = BackgroundImage.Width;
            Height = BackgroundImage.Height;

            StartPosition = FormStartPosition.Manual;
            var area = Screen.GetWorkingArea(this);
            var x = area.Width - Width - 10;
            var y = 10;
            Location = new System.Drawing.Point(x, y);

            var cx = Width - ExitButton.Width - 1;
            var cy = 1;
            ExitButton.Location = new Point(cx, cy);
            ExitButton.Image = null;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// InitializeToolTip
        /// 
        /// <summary>
        /// ツールチップの表示を初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void InitializeToolTip()
        {
            var tips = new ToolTip();

            tips.InitialDelay =  300;
            tips.ReshowDelay  = 1000;
            tips.AutoPopDelay = 3000;
            tips.ShowAlways   = true;

            tips.SetToolTip(this, Properties.Resources.DragDropMessage);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        /// 
        /// <summary>
        /// 画像抽出用の Model/View/Presenter を生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Create(string[] files)
        {
            foreach (var path in files)
            {
                try
                {
                    var ext = System.IO.Path.GetExtension(path).ToLower();
                    if (!AllowExtensions.Contains(ext) || !System.IO.File.Exists(path)) continue;

                    var model     = new PickTask(path);
                    var view      = new ProgressForm();
                    var presenter = new ProgressPresenter(view, model);
                    view.Show();
                }
                catch (Exception /* err */) { continue; }
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ShowCloseButton
        /// 
        /// <summary>
        /// 閉じるボタンを表示します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void ShowCloseButton(Cursor cursor)
        {
            ExitButton.Image = Properties.Resources.CloseButton;
            Cursor = cursor;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// HideCloseButton
        /// 
        /// <summary>
        /// 閉じるボタンを非表示にします。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void HideCloseButton()
        {
            ExitButton.Image = null;
            Cursor = Cursors.Default;
        }

        #endregion
    }
}
