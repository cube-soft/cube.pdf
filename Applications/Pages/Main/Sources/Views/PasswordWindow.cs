/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2013 CubeSoft, Inc.
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
    public partial class PasswordWindow : Cube.Forms.Window
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
                PasswordTextBox.UseSystemPasswordChar = !ShowPasswordCheckBox.Checked;
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// OnBind
        ///
        /// <summary>
        /// Invokes the binding to the specified object.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnBind(IBindable src)
        {
            base.OnBind(src);
            if (src is not PasswordViewModel vm) return;

            PasswordBindingSource.DataSource = vm;
            ExecButton.Click += (s, e) => vm.Apply();
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
