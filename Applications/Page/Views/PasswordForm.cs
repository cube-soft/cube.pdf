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

namespace Cube.Pdf.Page.App
{
    /* --------------------------------------------------------------------- */
    ///
    /// PasswordForm
    ///
    /// <summary>
    /// パスワード入力を行うためのフォームです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public partial class PasswordForm : Cube.Forms.StandardForm
    {
        #region Constructors

        /* --------------------------------------------------------------------- */
        ///
        /// PasswordForm
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        public PasswordForm()
        {
            InitializeComponent();
            ShowPasswordCheckBox.CheckedChanged += (s, e) =>
            {
                var check = ShowPasswordCheckBox.Checked;
                PasswordTextBox.UseSystemPasswordChar = !check;
            };
        }

        #endregion

        #region Properties

        /* --------------------------------------------------------------------- */
        ///
        /// Path
        ///
        /// <summary>
        /// ファイルのパスを取得または設定します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        public string Path
        {
            get { return _path; }
            set
            {
                if (_path != value)
                {
                    _path = value;
                    var message = string.Format(Properties.Resources.PasswordRequired,
                        System.IO.Path.GetFileName(value));
                    PasswordLabel.Text = string.Format(message);
                }
            }
        }

        /* --------------------------------------------------------------------- */
        ///
        /// Password
        ///
        /// <summary>
        /// 入力されたパスワードを取得します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        public string Password
        {
            get { return PasswordTextBox.Text; }
        }

        #endregion

        #region Override methods

        /* --------------------------------------------------------------------- */
        ///
        /// OnLoad
        ///
        /// <summary>
        /// フォームがロードされた時に実行されるハンドラです。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                PasswordKeyLabel.Margin = new Padding(
                    PasswordKeyLabel.Margin.Left,
                    PasswordTextBox.Margin.Top + Math.Max(PasswordTextBox.Height - PasswordKeyLabel.Height, 0) / 2,
                    PasswordKeyLabel.Margin.Right,
                    PasswordKeyLabel.Margin.Bottom
                );

                ShowPasswordCheckBox.Margin = new Padding(
                    ShowPasswordCheckBox.Margin.Left,
                    ShowPasswordCheckBox.Margin.Top,
                    PasswordTextBox.Width - ShowPasswordCheckBox.Width + PasswordTextBox.Margin.Right,
                    ShowPasswordCheckBox.Margin.Bottom
                );
            }
            finally { base.OnLoad(e); }
        }

        /* --------------------------------------------------------------------- */
        ///
        /// OnShown
        ///
        /// <summary>
        /// フォームが表示された時に実行されるハンドラです。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        protected override void OnShown(EventArgs e)
        {
            ActiveControl = PasswordTextBox;
            PasswordTextBox.Focus();
            base.OnShown(e);
        }

        #endregion

        #region Fields
        private string _path = string.Empty;
        #endregion
    }
}
