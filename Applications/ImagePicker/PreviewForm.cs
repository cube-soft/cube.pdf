/* ------------------------------------------------------------------------- */
///
/// PreviewForm.cs
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
using Cube.Pdf.App.ImageEx.Extensions;

namespace Cube.Pdf.App.ImageEx
{
    /* --------------------------------------------------------------------- */
    ///
    /// PreviewForm
    ///
    /// <summary>
    /// 画像のプレビュー画面を表示するクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public partial class PreviewForm : Cube.Forms.Form
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// PreviewForm
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PreviewForm()
        {
            InitializeComponent();
            PictureBox.Click += (s, e) => Close();
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Image
        /// 
        /// <summary>
        /// プレビュー画面に表示するイメージを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Image Image
        {
            get { return PictureBox.Image; }
            set
            {
                if (PictureBox.Image != value)
                {
                    PictureBox.Image = value;
                    ResizeForm(value.Size);
                }
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// FileName
        /// 
        /// <summary>
        /// ファイル名を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string FileName
        {
            get { return _filename; }
            set
            {
                if (_filename != value)
                {
                    _filename = value;
                    this.UpdateTitle(value);
                }
            }
        }

        #endregion

        #region Override methods

        /* ----------------------------------------------------------------- */
        ///
        /// OnClientSizeChanged
        /// 
        /// <summary>
        /// プレビュー画面サイズが変更された時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnClientSizeChanged(EventArgs e)
        {
            ResizeImage();
            base.OnClientSizeChanged(e);
        }

        #endregion

        #region Other private methods

        /* ----------------------------------------------------------------- */
        ///
        /// ResizeForm
        /// 
        /// <summary>
        /// プレビュー画面をリサイズします。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void ResizeForm(Size size)
        {
            var area   = Screen.GetWorkingArea(this);
            var width  = Math.Min(Math.Max(size.Width, MinimumSize.Width), area.Width);
            var height = Math.Min(Math.Max(size.Height, MinimumSize.Height), area.Height);

            ClientSize = new Size(width, height);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ResizeImage
        /// 
        /// <summary>
        /// 画像を表示する PictureBox をリサイズします。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void ResizeImage()
        {
            var width  = LayoutPanel.ClientSize.Width;
            var height = (PictureBox.Image != null) ?
                         (int)(LayoutPanel.ClientSize.Width * (PictureBox.Image.Height / (double)PictureBox.Image.Width)) :
                         LayoutPanel.ClientSize.Height;

            var x = 0;
            var y = Math.Max(ClientSize.Height - height, 0) / 2;

            PictureBox.Location = new Point(x, y);
            PictureBox.Width    = width;
            PictureBox.Height   = height;
        }

        #endregion

        #region Fields
        private string _filename = string.Empty;
        #endregion
    }
}
