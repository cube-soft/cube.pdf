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
/// SettingWindow
///
/// <summary>
/// Represents the setting window.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public partial class SettingWindow : Window
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// SettingWindow
    ///
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public SettingWindow() => InitializeComponent();

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
        if (src is not SettingViewModel vm) return;

        BindCore(vm);
        BindTexts(vm);
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
    private void BindCore(SettingViewModel vm)
    {
        var bs = Behaviors.Hook(new BindingSource(vm, ""));
        bs.Bind(nameof(vm.Language), LanguageComboBox, nameof(LanguageComboBox.SelectedValue));
        bs.Bind(nameof(vm.Temp), TempTextBox, nameof(TempTextBox.Text));
        bs.Bind(nameof(vm.ShrinkResources), ShrinkResourceCheckBox, nameof(ShrinkResourceCheckBox.Checked));
        bs.Bind(nameof(vm.KeepOutlines), KeepOutlineCheckBox, nameof(KeepOutlineCheckBox.Checked));
        bs.Bind(nameof(vm.AutoSort), AutoSortCheckBox, nameof(AutoSortCheckBox.Checked));
        bs.Bind(nameof(vm.CheckUpdate), UpdateCheckBox, nameof(UpdateCheckBox.Checked));
        bs.Bind(nameof(vm.Version), VersionControl, nameof(VersionControl.Version), true);
        bs.Bind(nameof(vm.Uri), VersionControl, nameof(VersionControl.Uri), true);

        Behaviors.Add(new CloseBehavior(this, vm));
        Behaviors.Add(new ClickEventBehavior(ExecButton, vm.Apply));
        Behaviors.Add(new ClickEventBehavior(TempButton, vm.SelectTemp));
        Behaviors.Add(new OpenDirectoryBehavior(vm));

        LanguageComboBox.Bind(Surface.Languages);
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
    private void BindTexts(SettingViewModel _)
    {
        Text = Surface.Texts.Setting_Window;
        ExecButton.Text = Surface.Texts.Menu_Ok;
        ExitButton.Text = Surface.Texts.Menu_Cancel;
        SettingTabPage.Text = Surface.Texts.Setting_Tab;
        VersionTabPage.Text = Surface.Texts.Setting_Version;
        OptionLabel.Text = Surface.Texts.Setting_Options;
        TempLabel.Text = Surface.Texts.Setting_Temp;
        LanguageLabel.Text = Surface.Texts.Setting_Language;
        OtherLabel.Text = Surface.Texts.Setting_Others;
        ShrinkResourceCheckBox.Text = Surface.Texts.Setting_Shrink;
        KeepOutlineCheckBox.Text = Surface.Texts.Setting_KeepOutline;
        AutoSortCheckBox.Text = Surface.Texts.Setting_AutoSort;
        UpdateCheckBox.Text = Surface.Texts.Setting_CheckUpdate;
    }

    #endregion
}