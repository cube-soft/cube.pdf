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

        /* ----------------------------------------------------------------- */
        ///
        /// IsAllowed
        ///
        /// <summary>
        /// Determines whether the specified operation is allowed.
        /// </summary>
        ///
        /// <param name="src">PermissionMethod object.</param>
        ///
        /// <returns>true for allowed.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static bool IsAllowed(this PermissionValue src) => src == PermissionValue.Allow;

        /* ----------------------------------------------------------------- */
        ///
        /// IsDenid
        ///
        /// <summary>
        /// Determines whether the specified operation is denied.
        /// </summary>
        ///
        /// <param name="src">PermissionMethod object.</param>
        ///
        /// <returns>true for denied.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static bool IsDenid(this PermissionValue src) => src == PermissionValue.Deny;

        /* ----------------------------------------------------------------- */
        ///
        /// RequestPassword
        ///
        /// <summary>
        /// Requests the password for the specified PDF file.
        /// </summary>
        ///
        /// <param name="query">Query object.</param>
        /// <param name="src">Path of the PDF file.</param>
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
                else throw new ArgumentException("Password is empty.");
            }
            catch (Exception err) { throw Convert(err); }
        }

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

        /* ----------------------------------------------------------------- */
        ///
        /// Convert
        ///
        /// <summary>
        /// Creates a new instance of the EncryptionException class from
        /// the specified exception object.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static EncryptionException Convert(Exception src) =>
            new EncryptionException("Input password may be incorrect.", src);

        #endregion
    }
}
