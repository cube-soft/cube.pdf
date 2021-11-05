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
using System.Threading;

namespace Cube.Pdf.Pages
{
    /* --------------------------------------------------------------------- */
    ///
    /// VersionViewModel
    ///
    /// <summary>
    /// Provides binding properties and commands for the PasswordWindow
    /// class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class VersionViewModel : PresentableBase<SettingFolder>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// VersionViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the VersionViewModel class.
        /// </summary>
        ///
        /// <param name="src">User settings.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public VersionViewModel(SettingFolder src, SynchronizationContext context) :
            base(src, new(), context) {}

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Version
        ///
        /// <summary>
        /// Gets the version string.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Version => $"Version {Facade.Version.ToString(3, true)}";

        /* ----------------------------------------------------------------- */
        ///
        /// CheckUpdate
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to check the update
        /// of the application.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool CheckUpdate
        {
            get => Facade.Startup.Enabled;
            set => Facade.Startup.Enabled = value;
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Apply
        ///
        /// <summary>
        /// Apply the user settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Apply() => Close(Facade.Save, true);

        #endregion
    }
}
