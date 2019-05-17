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
using Cube.Mixin.Assembly;
using Cube.Mixin.Registry;
using Microsoft.Win32;

namespace Cube.Pdf.Converter
{
    /* --------------------------------------------------------------------- */
    ///
    /// SettingsExtension
    ///
    /// <summary>
    /// Provides extended methods of the SettingsFolder class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class SettingsExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetValue
        ///
        /// <summary>
        /// Gets the string value from the specified arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static string GetValue(this SettingsFolder src, RegistryKey root, string name) =>
            root.GetValue<string>($@"Software\{src.Assembly.GetCompany()}\{src.Assembly.GetProduct()}", name);

        #endregion
    }
}
