/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
/* ------------------------------------------------------------------------- */
namespace Cube.Pdf.Mixin
{
    /* --------------------------------------------------------------------- */
    ///
    /// EncryptionExtension
    ///
    /// <summary>
    /// Describes extended methods for the Encryption class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class EncryptionExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Copy
        ///
        /// <summary>
        /// Gets the copied Encryption.
        /// </summary>
        ///
        /// <param name="src">Original object.</param>
        ///
        /// <returns>Copied object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Encryption Copy(this Encryption src) => new()
        {
            Dispatcher       = src.Dispatcher,
            Enabled          = src.Enabled,
            Method           = src.Method,
            OwnerPassword    = src.OwnerPassword,
            UserPassword     = src.UserPassword,
            OpenWithPassword = src.OpenWithPassword,
            Permission       = new Permission(src.Permission.Value),
        };

        /* ----------------------------------------------------------------- */
        ///
        /// Deny
        ///
        /// <summary>
        /// Denies all operations.
        /// </summary>
        ///
        /// <param name="src">Encryption object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Deny(this Encryption src) => src.Set(PermissionValue.Deny);

        /* ----------------------------------------------------------------- */
        ///
        /// Allow
        ///
        /// <summary>
        /// Allows all operations.
        /// </summary>
        ///
        /// <param name="src">Encryption object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Allow(this Encryption src) => src.Set(PermissionValue.Allow);

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Set
        ///
        /// <summary>
        /// Sets all of the methods to the same permission.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static void Set(this Encryption src, PermissionValue value)
        {
            src.Permission.Accessibility     = value;
            src.Permission.CopyContents      = value;
            src.Permission.InputForm         = value;
            src.Permission.ModifyAnnotations = value;
            src.Permission.ModifyContents    = value;
            src.Permission.Print             = value;
        }

        #endregion
    }
}
