/* ------------------------------------------------------------------------- */
/* ------------------------------------------------------------------------- */
using System;
///
/// ProgressForm.cs
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
using System.Drawing;
using System.Windows.Forms;

namespace Cube.Pdf.ImageEx
{
    /* --------------------------------------------------------------------- */
    ///
    /// Cube.Pdf.ImageEx.PreviewForm
    ///
    /// <summary>
    /// 画像のプレビュー画面を表示するクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public partial class PreviewForm : Cube.Forms.NtsForm
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
            _frame.Click += (s, e) => Close();
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
            get { return _frame.Image; }
            set
            {
                if (LayoutPanel.Controls.Contains(_frame)) LayoutPanel.Controls.Remove(_frame);

                _frame.Image = value;
                _frame.Location = new Point(0, 0);

                ClientSize = Image.Size;
                _frame.SizeMode = PictureBoxSizeMode.Zoom;

                LayoutPanel.Controls.Add(_frame);
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
            var width  = ClientSize.Width;
            var height = (_frame.Image != null) ?
                         (int)(ClientSize.Width * (_frame.Image.Height / (double)_frame.Image.Width)) :
                         ClientSize.Height;

            var x = 0;
            var y = Math.Max(ClientSize.Height - height, 0) / 2;

            _frame.Location = new Point(x, y);
            _frame.Width  = width;
            _frame.Height = height;

            base.OnClientSizeChanged(e);
        }

        #endregion

        #region Fields
        private PictureBox _frame = new PictureBox();
        #endregion
    }
}
