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
using Cube.Pdf.Itext;

namespace Cube.Pdf.Pages
{
    /* --------------------------------------------------------------------- */
    ///
    /// SettingExtension
    ///
    /// <summary>
    /// Provides extended methods of the SettingFolder class..
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class SettingExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// ToOpenOption
        ///
        /// <summary>
        /// Converts to a OpenOption object.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static OpenOption ToOpenOption(this SettingFolder _) => new()
        {
            FullAccess = true,
        };

        /* ----------------------------------------------------------------- */
        ///
        /// ToSaveOption
        ///
        /// <summary>
        /// Converts to a SaveOption object.
        /// </summary>
        ///
        /// <param name="src">User settings.</param>
        ///
        /// <returns>SaveOption object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static SaveOption ToSaveOption(this SettingFolder src) => new()
        {
            Temp            = src.Value.Temp,
            ShrinkResources = src.Value.ShrinkResources,
        };

        #endregion
    }
}
