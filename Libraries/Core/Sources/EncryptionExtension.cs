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
using Cube.Generics;
using System;

namespace Cube.Pdf.Mixin
{
    /* --------------------------------------------------------------------- */
    ///
    /// EncryptionExtension
    ///
    /// <summary>
    /// Describes extended methods for the <c>Encryption</c> class.
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
        /// Gets the copied <c>Encryption</c> object.
        /// </summary>
        ///
        /// <param name="src"><c>Encryption</c> object.</param>
        ///
        /// <returns>Copied <c>Encryption</c> object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Encryption Copy(this Encryption src) => new Encryption
        {
            Context          = src.Context,
            IsSynchronous    = src.IsSynchronous,
            Enabled          = src.Enabled,
            Method           = src.Method,
            OwnerPassword    = src.OwnerPassword,
            UserPassword     = src.UserPassword,
            OpenWithPassword = src.OpenWithPassword,
            Permission       = new Permission(src.Permission.Value),
        };

        /* ----------------------------------------------------------------- */
        ///
        /// DenyAll
        ///
        /// <summary>
        /// Denies all of the permissions.
        /// </summary>
        ///
        /// <param name="src">Encryption object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void DenyAll(this Encryption src) => src.SetAll(PermissionMethod.Deny);

        /* ----------------------------------------------------------------- */
        ///
        /// AllowAll
        ///
        /// <summary>
        /// Allows all of the permissions.
        /// </summary>
        ///
        /// <param name="src">Encryption object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void AllowAll(this Encryption src) => src.SetAll(PermissionMethod.Allow);

        /* ----------------------------------------------------------------- */
        ///
        /// IsAllowed
        ///
        /// <summary>
        /// Determines whether the specified object is allowed.
        /// </summary>
        ///
        /// <param name="src">PermissionMethod object.</param>
        ///
        /// <returns>true for allowed.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static bool IsAllowed(this PermissionMethod src) => src == PermissionMethod.Allow;

        /* ----------------------------------------------------------------- */
        ///
        /// IsDenid
        ///
        /// <summary>
        /// Determines whether the specified object is denied.
        /// </summary>
        ///
        /// <param name="src">PermissionMethod object.</param>
        ///
        /// <returns>true for denied.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static bool IsDenid(this PermissionMethod src) => src == PermissionMethod.Deny;

        /* ----------------------------------------------------------------- */
        ///
        /// RequestPassword
        ///
        /// <summary>
        /// Requests the password for the specified PDF file.
        /// </summary>
        ///
        /// <param name="query">Query object.</param>
        /// <param name="src">PDF file path.</param>
        ///
        /// <returns>Query result.</returns>
        ///
        /// <remarks>
        /// 問い合わせ失敗時の挙動を EncryptionException を送出する形に
        /// 統一します。また、実行後に Result が空文字だった場合も失敗と
        /// 見なします。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static QueryEventArgs<string> RequestPassword(this IQuery<string> query, string src)
        {
            var dest = QueryEventArgs.Create(src);

            try
            {
                query.Request(dest);
                if (dest.Cancel || dest.Result.HasValue()) return dest;
            }
            catch (Exception) { /* throw EncryptionException */ }

            throw new EncryptionException(Properties.Resources.ErrorPassword);
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// SetAll
        ///
        /// <summary>
        /// Sets all of the methods to the same permission.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static void SetAll(this Encryption src, PermissionMethod method)
        {
            src.Permission.Accessibility     = method;
            src.Permission.Assemble          = method;
            src.Permission.CopyContents      = method;
            src.Permission.InputForm         = method;
            src.Permission.ModifyAnnotations = method;
            src.Permission.ModifyContents    = method;
            src.Permission.Print             = method;
        }

        #endregion
    }
}
