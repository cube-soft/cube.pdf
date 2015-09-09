/* ------------------------------------------------------------------------- */
///
/// Encryption.cs
/// 
/// Copyright (c) 2010 CubeSoft, Inc.
/// 
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
///
///  http://www.apache.org/licenses/LICENSE-2.0
///
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.
///
/* ------------------------------------------------------------------------- */

namespace Cube.Pdf
{
    /* --------------------------------------------------------------------- */
    ///
    /// Encryption
    /// 
    /// <summary>
    /// PDF の暗号化に関するデータを表すクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class Encryption
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// IsEnabled
        /// 
        /// <summary>
        /// この暗号化設定を適用するかどうかを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool IsEnabled { get; set; } = false;

        /* ----------------------------------------------------------------- */
        ///
        /// IsUserPasswordEnabled
        /// 
        /// <summary>
        /// ユーザパスワードを適用するかどうかを取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public bool IsUserPasswordEnabled { get; set; } = false;

        /* ----------------------------------------------------------------- */
        ///
        /// OwnerPassword
        /// 
        /// <summary>
        /// 所有者パスワードを取得または設定します。
        /// </summary>
        /// 
        /// <remarks>
        /// 所有者パスワードとは PDF ファイルに設定されているマスター
        /// パスワードを表し、このパスワードによって再暗号化や各種権限の
        /// 変更等すべての操作が可能となります。
        /// </remarks>
        /// 
        /* ----------------------------------------------------------------- */
        public string OwnerPassword { get; set; } = string.Empty;

        /* ----------------------------------------------------------------- */
        ///
        /// UserPassword
        /// 
        /// <summary>
        /// ユーザパスワードを取得または設定します。
        /// </summary>
        /// 
        /// <remarks>
        /// ユーザパスワードとは、PDF ファイルを開く際に必要となる
        /// パスワードを表します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public string UserPassword { get; set; } = string.Empty;

        /* ----------------------------------------------------------------- */
        ///
        /// Method
        /// 
        /// <summary>
        /// 適用する暗号化方式を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public EncryptionMethod Method { get; set; } = EncryptionMethod.Unknown;

        /* ----------------------------------------------------------------- */
        ///
        /// Permission
        /// 
        /// <summary>
        /// 暗号化された PDF に設定されている各種権限の状態を取得
        /// または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Permission Permission { get; set; } = new Permission();

        #endregion
    }
}
