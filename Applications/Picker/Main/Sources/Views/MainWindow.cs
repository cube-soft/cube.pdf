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
    /// MainWindow
    ///
    /// <summary>
    /// Represents the main window.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public partial class MainWindow : BorderlessWindow
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// MainWindow
        ///
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public MainWindow()
        {
            InitializeComponent();

            ExitButton.Click += (s, e) => Close();
            ExitButton.MouseEnter += (s, e) => ExitButton.Image = Properties.Resources.CloseButton;
            ExitButton.MouseLeave += (s, e) => ExitButton.Image = null;

            MouseEnter += (s, e) => ExitButton.Image = Properties.Resources.CloseButton;
            MouseLeave += (s, e) => ExitButton.Image = null;
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// OnLoad
        ///
        /// <summary>
        /// Occurs when the Load event is fired.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

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

        #endregion
    }
}
