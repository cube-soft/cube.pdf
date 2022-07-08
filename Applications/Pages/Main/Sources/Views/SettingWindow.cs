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
    /// SettingWindow
    ///
    /// <summary>
    /// Represents the setting window.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public partial class SettingWindow : Window
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// SettingWindow
        ///
        /// <summary>
        /// Initializes a new instance of the SettingWindow class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public SettingWindow() => InitializeComponent();

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
            if (src is not SettingViewModel vm) return;

            var bs = Behaviors.Hook(new BindingSource(vm, ""));
            bs.Bind(nameof(vm.Language),        LanguageComboBox,       nameof(ComboBox.SelectedValue));
            bs.Bind(nameof(vm.Temp),            TempTextBox,            nameof(TextBox.Text));
            bs.Bind(nameof(vm.ShrinkResources), ShrinkResourceCheckBox, nameof(CheckBox.Checked));
            bs.Bind(nameof(vm.KeepOutlines),    KeepOutlineCheckBox,    nameof(CheckBox.Checked));
            bs.Bind(nameof(vm.CheckUpdate),     UpdateCheckBox,         nameof(CheckBox.Checked));
            bs.Bind(nameof(vm.Version),         VersionControl,         nameof(VersionControl.Version), true);
            bs.Bind(nameof(vm.Uri),             VersionControl,         nameof(VersionControl.Uri), true);

            Behaviors.Add(new CloseBehavior(this, vm));
            Behaviors.Add(new ClickEventBehavior(ExecButton, vm.Apply));
            Behaviors.Add(new ClickEventBehavior(TempButton, vm.SelectTemp));
            Behaviors.Add(new OpenDirectoryBehavior(vm));

            LanguageComboBox.Bind(Resource.Languages);
        }

        #endregion
    }
}
