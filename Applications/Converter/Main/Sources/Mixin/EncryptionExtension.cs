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
namespace Cube.Pdf.Converter.Mixin
{
    /* --------------------------------------------------------------------- */
    ///
    /// EncryptionExtension
    ///
    /// <summary>
    /// Provides extended methods of the Encryption and related classes.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    static class EncryptionExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// ToPermission
        ///
        /// <summary>
        /// Gets the PermissionValue object corresponding to the specified
        /// value.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static PermissionValue ToPermission(this bool src) =>
            src ? PermissionValue.Allow : PermissionValue.Deny;

        #endregion
    }
}
