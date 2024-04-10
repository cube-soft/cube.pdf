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
namespace Cube.Pdf.Pages;

using System.Windows.Forms;
using Cube.Forms;
using Cube.Forms.Behaviors;
using Cube.Forms.Binding;

/* ------------------------------------------------------------------------- */
///
/// PasswordWindow
///
/// <summary>
/// Represents the window to input password.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public partial class PasswordWindow : Window
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// PasswordWindow
    ///
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public PasswordWindow() => InitializeComponent();

    #endregion

    #region Bindings

    /* --------------------------------------------------------------------- */
    ///
    /// OnBind
    ///
    /// <summary>
    /// Invokes the binding to the specified object.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    protected override void OnBind(IBindable src)
    {
        base.OnBind(src);
        if (src is not PasswordViewModel vm) return;

        BindCore(vm);
        BindTexts(vm);
        BindBehaviors(vm);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// BindCore
    ///
    /// <summary>
    /// Invokes the binding settings.
    /// </summary>
    ///
    /// <param name="vm">VM object to bind.</param>
    ///
    /* --------------------------------------------------------------------- */
    private void BindCore(PasswordViewModel vm)
    {
        var bs = Behaviors.Hook(new BindingSource(vm, ""));
        bs.Bind(nameof(vm.Password), PasswordTextBox, nameof(TextBox.Text));
        bs.Bind(nameof(vm.Message), PasswordLabel, nameof(Label.Text), true);
        bs.Bind(nameof(vm.Ready), ExecButton, nameof(Enabled), true);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// BindTexts
    ///
    /// <summary>
    /// Sets the displayed text with the specified language.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private void BindTexts(PasswordViewModel _)
    {
        Text = Surface.Texts.Password_Window;
        ExecButton.Text = Surface.Texts.Menu_Ok;
        ExitButton.Text = Surface.Texts.Menu_Cancel;
        ShowPasswordCheckBox.Text = Surface.Texts.Password_Show;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// BindBehaviors
    ///
    /// <summary>
    /// Invokes the binding settings about behaviors.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private void BindBehaviors(PasswordViewModel vm)
    {
        void setup()  { ActiveControl = PasswordTextBox; PasswordTextBox.Focus(); }
        void invert() { PasswordTextBox.UseSystemPasswordChar = !ShowPasswordCheckBox.Checked; }

        Behaviors.Add(new CloseBehavior(this, vm));
        Behaviors.Add(new ShownEventBehavior(this, setup));
        Behaviors.Add(new ClickEventBehavior(ExecButton, vm.Apply));
        Behaviors.Add(new EventBehavior(ShowPasswordCheckBox, nameof(CheckBox.CheckedChanged), invert));
    }

    #endregion
}