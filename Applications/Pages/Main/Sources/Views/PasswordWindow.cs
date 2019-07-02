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
using System.Windows.Forms;

namespace Cube.Pdf.Pages
{
    /* --------------------------------------------------------------------- */
    ///
    /// PasswordWindow
    ///
    /// <summary>
    /// Represents the window to input password.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public partial class PasswordWindow : Cube.Forms.WindowBase
    {
        #region Constructors

        /* --------------------------------------------------------------------- */
        ///
        /// PasswordWindow
        ///
        /// <summary>
        /// Initializes a new instance of the PasswordWindow class.
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        public PasswordWindow()
        {
            InitializeComponent();
            ShowPasswordCheckBox.CheckedChanged += (s, e) =>
            {
                var check = ShowPasswordCheckBox.Checked;
                PasswordTextBox.UseSystemPasswordChar = !check;
            };
        }

        #endregion

        #region Implementations

        /* --------------------------------------------------------------------- */
        ///
        /// OnLoad
        ///
        /// <summary>
        /// Occurs when the Loaded event is fired.
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                var v0 = Math.Max(PasswordTextBox.Height - PasswordKeyLabel.Height, 0) / 2;
                var l0 = PasswordKeyLabel.Margin.Left;
                var t0 = PasswordTextBox.Margin.Top + v0;
                var r0 = PasswordKeyLabel.Margin.Right;
                var b0 = PasswordKeyLabel.Margin.Bottom;

                var v1 = ShowPasswordCheckBox.Width + PasswordTextBox.Margin.Right;
                var l1 = ShowPasswordCheckBox.Margin.Left;
                var t1 = ShowPasswordCheckBox.Margin.Top;
                var r1 = PasswordTextBox.Width - v1;
                var b1 = ShowPasswordCheckBox.Margin.Bottom;

                PasswordKeyLabel.Margin     = new Padding(l0, t0, r0, b0);
                ShowPasswordCheckBox.Margin = new Padding(l1, t1, r1, b1);
            }
            finally { base.OnLoad(e); }
        }

        /* --------------------------------------------------------------------- */
        ///
        /// OnShown
        ///
        /// <summary>
        /// Occurs when the Shown event is fired.
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            ActiveControl = PasswordTextBox;
            _ = PasswordTextBox.Focus();
        }

        #endregion
    }
}
