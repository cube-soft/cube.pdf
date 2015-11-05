/* ------------------------------------------------------------------------- */
///
/// MainForm.cs
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
using System.Drawing;
using System.Windows.Forms;

namespace Cube.Pdf.ImageEx
{
    /* --------------------------------------------------------------------- */
    ///
    /// Cube.Pdf.ImageEx.MainForm
    ///
    /// <summary>
    /// メイン画面を表示するクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public partial class MainForm : Cube.Forms.WidgetForm
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// MainForm
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public MainForm()
        {
            InitializeComponent();
            InitializeToolTip();

            ExitButton.Click += (s, e) => Close();
            ExitButton.MouseEnter += (s, e) => ShowCloseButton(Cursors.Hand);
            ExitButton.MouseLeave += (s, e) => HideCloseButton();
        }

        #endregion

        #region Override methods

        /* ----------------------------------------------------------------- */
        ///
        /// OnShown
        /// 
        /// <summary>
        /// フォームが表示された時に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnShown(EventArgs e)
        {
            InitializeLayout();
            base.OnShown(e);
        }

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
