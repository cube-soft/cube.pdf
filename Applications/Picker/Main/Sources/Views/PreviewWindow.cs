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
using Cube.Forms;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Cube.Pdf.Picker
{
    /* --------------------------------------------------------------------- */
    ///
    /// PreviewWindow
    ///
    /// <summary>
    /// Represents the window to preview the provided image.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public partial class PreviewWindow : Window
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// PreviewForm
        ///
        /// <summary>
        /// Initializes a new instance of the PreviewWindow class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PreviewWindow()
        {
            InitializeComponent();
            PictureBox.Click += (s, e) => Close();
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// OnClientSizeChanged
        ///
        /// <summary>
        /// Occurs when the ClientSizeChanged event is fired.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnClientSizeChanged(EventArgs e)
        {
            base.OnClientSizeChanged(e);
            ResizeImage();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ResizeImage
        ///
        /// <summary>
        /// Resize the provided image.
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
    }
}
