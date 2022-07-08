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
using System.Windows.Forms;
using Cube.Forms;
using Cube.Forms.Behaviors;
using Cube.Forms.Binding;

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
    public partial class PasswordWindow : Window
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
        public PasswordWindow() => InitializeComponent();

        #endregion

        #region Methods

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

            var bs = Behaviors.Hook(new BindingSource(vm, ""));
            bs.Bind(nameof(vm.Password), PasswordTextBox, nameof(TextBox.Text));
            bs.Bind(nameof(vm.Message), PasswordLabel, nameof(Label.Text), true);
            bs.Bind(nameof(vm.Ready), ExecButton, nameof(Enabled), true);

            Behaviors.Add(new CloseBehavior(this, vm));
            Behaviors.Add(new ShownEventBehavior(this, Setup));
            Behaviors.Add(new ClickEventBehavior(ExecButton, vm.Apply));
            Behaviors.Add(new EventBehavior(ShowPasswordCheckBox, nameof(CheckBox.CheckedChanged), UpdatePasswordMask));
        }

        #endregion

        #region Implementations

        /* --------------------------------------------------------------------- */
        ///
        /// Setup
        ///
        /// <summary>
        /// Initializes the settings when the window is shown.
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private void Setup()
        {
            ActiveControl = PasswordTextBox;
            _ = PasswordTextBox.Focus();
        }

        /* --------------------------------------------------------------------- */
        ///
        /// UpdatePasswordMask
        ///
        /// <summary>
        /// Sets or resets the password mask.
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private void UpdatePasswordMask() =>
            PasswordTextBox.UseSystemPasswordChar = !ShowPasswordCheckBox.Checked;

        #endregion
    }
}
