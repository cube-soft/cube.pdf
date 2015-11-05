/* ------------------------------------------------------------------------- */
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
/* ------------------------------------------------------------------------- */
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
                _frame.Location = new Point(0, 0);
                _frame.SizeMode = PictureBoxSizeMode.AutoSize;
                _frame.Image = value;
                LayoutPanel.Controls.Add(_frame);
            }
        }

        #endregion

        #region Fields
        private PictureBox _frame = new PictureBox();
        #endregion
    }
}
