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
using Cube.FileSystem;
using Cube.Mixin.Assembly;

namespace Cube.Pdf.Converter
{
    /* --------------------------------------------------------------------- */
    ///
    /// SettingFacade
    ///
    /// <summary>
    /// Provides functionality to communicate with the SettingViewModel
    /// class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class SettingFacade : DisposableBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// SettingFacade
        ///
        /// <summary>
        /// Initializes a new instance of the SettingFacade class with the
        /// specified settings.
        /// </summary>
        ///
        /// <param name="src">User settings.</param>
        ///
        /* ----------------------------------------------------------------- */
        public SettingFacade(SettingFolder src)
        {
            var exe = Io.Combine(GetType().Assembly.GetDirectoryName(), "CubeChecker.exe");

            Settings = src;
            Startup  = new("cubepdf-checker") { Source = exe };
            Startup.Arguments.Add("cubepdf");
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Settings
        ///
        /// <summary>
        /// Gets the user settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public SettingFolder Settings { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Startup
        ///
        /// <summary>
        /// Gets the object to register or remove startup settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Startup Startup { get; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// Saves the settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Save()
        {
            Settings.Save();
            Startup.Save(true);
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases the unmanaged resources used by the DocumentReader
        /// and optionally releases the managed resources.
        /// </summary>
        ///
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        protected override void Dispose(bool disposing) { }

        #endregion
    }
}
