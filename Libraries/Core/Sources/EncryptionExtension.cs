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
    /// Encryption の拡張用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class EncryptionExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// DenyAll
        ///
        /// <summary>
        /// 全て操作を禁止します。
        /// </summary>
        ///
        /// <param name="src">Encryption オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void DenyAll(this Encryption src) => src.SetAll(PermissionMethod.Deny);

        /* ----------------------------------------------------------------- */
        ///
        /// AllowAll
        ///
        /// <summary>
        /// 全て操作を許可します。
        /// </summary>
        ///
        /// <param name="src">Encryption オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void AllowAll(this Encryption src) => src.SetAll(PermissionMethod.Allow);

        /* ----------------------------------------------------------------- */
        ///
        /// IsAllowed
        ///
        /// <summary>
        /// 操作が許可されているかどうかを判別します。
        /// </summary>
        ///
        /// <param name="src">判別対象の操作</param>
        ///
        /// <returns>許可されているかどうか</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static bool IsAllowed(this PermissionMethod src) => src == PermissionMethod.Allow;

        /* ----------------------------------------------------------------- */
        ///
        /// IsDenid
        ///
        /// <summary>
        /// 操作が禁止されているかどうかを判別します。
        /// </summary>
        ///
        /// <param name="src">判別対象の操作</param>
        ///
        /// <returns>禁止されているかどうか</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static bool IsDenid(this PermissionMethod src) => src == PermissionMethod.Deny;

        /* ----------------------------------------------------------------- */
        ///
        /// RequestPassword
        ///
        /// <summary>
        /// パスワードの問い合わせを実行します。
        /// </summary>
        ///
        /// <param name="query">問い合わせ用オブジェクト</param>
        /// <param name="src">入力ファイル</param>
        ///
        /// <returns>問い合わせ結果</returns>
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
        /// 全て操作に同じ内容を設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static void SetAll(this Encryption src, PermissionMethod method)
        {
            src.Permission.Accessibility     = method;
            src.Permission.Assemble          = method;
            src.Permission.CopyContents      = method;
            src.Permission.InputForms        = method;
            src.Permission.ModifyAnnotations = method;
            src.Permission.ModifyContents    = method;
            src.Permission.Print             = method;
        }

        #endregion
    }
}
