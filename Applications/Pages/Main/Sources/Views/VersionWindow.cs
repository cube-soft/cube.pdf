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
using Cube.Forms;

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
        public VersionWindow()
        {
            InitializeComponent();
            ExecButton.Click += (s, e) => Close();
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
        protected override void OnBind(IPresentable src)
        {
            base.OnBind(src);
            if (!(src is VersionViewModel vm)) return;

            VersionBindingSource.DataSource = vm;
            ExecButton.Click += (s, e) => vm.Apply();
        }

        #endregion
    }
}
