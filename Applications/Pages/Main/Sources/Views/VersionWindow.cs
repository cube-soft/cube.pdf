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
using Cube.Mixin.Forms;
using Cube.Mixin.Forms.Controls;

namespace Cube.Pdf.Pages
{
    /* --------------------------------------------------------------------- */
    ///
    /// VersionWindow
    ///
    /// <summary>
    /// Represents the version window.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public partial class VersionWindow : Window
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// VersionWindow
        ///
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public VersionWindow() => InitializeComponent();

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
            if (src is not VersionViewModel vm) return;

            var bs = Behaviors.Hook(new BindingSource(vm, ""));
            bs.Bind(nameof(vm.Version), VersionPanel, nameof(VersionPanel.Version), true);
            bs.Bind(nameof(vm.CheckUpdate), UpdateCheckBox, nameof(CheckBox.Checked));

            Behaviors.Add(new CloseBehavior(this, vm));
            Behaviors.Add(new ClickEventBehavior(ExecButton, vm.Apply));
        }

        #endregion
    }
}
