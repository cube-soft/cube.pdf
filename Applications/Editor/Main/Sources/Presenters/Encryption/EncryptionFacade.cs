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
using Cube.Mixin.String;
using System.Collections.Generic;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// EncryptionFacade
    ///
    /// <summary>
    /// Provides functionality to access or update the PDF encryption.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class EncryptionFacade
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// EncryptionFacade
        ///
        /// <summary>
        /// Initializes a new instance of the EncryptionFacade class with
        /// the specified arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public EncryptionFacade(Encryption src)
        {
            if (src.Method == EncryptionMethod.Unknown) src.Method = EncryptionMethod.Aes256;
            SharePassword = src.OwnerPassword.HasValue() && src.OwnerPassword.FuzzyEquals(src.UserPassword);
            if (SharePassword) src.UserPassword = string.Empty;

            Value = src;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Value
        ///
        /// <summary>
        /// Gets the Encryption settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Encryption Value { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// OwnerConfirm
        ///
        /// <summary>
        /// Gets or sets a value of owner password confirmation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string OwnerConfirm { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// UserConfirm
        ///
        /// <summary>
        /// Gets or sets a value of user password confirmation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string UserConfirm { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// SharePassword
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to share the user
        /// password with the owner password.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool SharePassword { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Methods
        ///
        /// <summary>
        /// Gets a collection of encryption methods.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static IEnumerable<EncryptionMethod> Methods { get; } = new[]
        {
            EncryptionMethod.Standard40,
            EncryptionMethod.Standard128,
            EncryptionMethod.Aes128,
            EncryptionMethod.Aes256,
        };

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// IsAcceptable
        ///
        /// <summary>
        /// Gets a value indicating whether that the current settings are
        /// acceptable.
        /// </summary>
        ///
        /// <returns>true for acceptable.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public bool IsAcceptable()
        {
            if (!Value.Enabled) return true;

            // Check OwnerPassword
            var p0 = Value.OwnerPassword;
            if (!p0.HasValue() || !p0.FuzzyEquals(OwnerConfirm)) return false;

            // Check UserPassword
            if (!Value.OpenWithPassword || SharePassword) return true;
            var p1 = Value.UserPassword;
            if (!p1.HasValue() || !p1.FuzzyEquals(UserConfirm)) return false;

            return true;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Normalize
        ///
        /// <summary>
        /// Normalizes the current settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Normalize()
        {
            if (SharePassword) Value.UserPassword = Value.OwnerPassword;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetPermission
        ///
        /// <summary>
        /// Gets a value of the PermissionValue enum from the specified
        /// boolean value.
        /// </summary>
        ///
        /// <param name="value">
        /// Value indicating whether to allow the permission.
        /// </param>
        ///
        /// <returns>PermissionValue enum.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public PermissionValue GetPermission(bool value) =>
            value ? PermissionValue.Allow : PermissionValue.Deny;

        #endregion
    }
}
