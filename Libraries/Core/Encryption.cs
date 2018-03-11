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
using System;

namespace Cube.Pdf
{
    /* --------------------------------------------------------------------- */
    ///
    /// Encryption
    ///
    /// <summary>
    /// PDF の暗号化に関する情報を保持するためのクラスです。
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

    /* --------------------------------------------------------------------- */
    ///
    /// EncryptionMethod
    ///
    /// <summary>
    /// PDF の暗号化の際に使用可能な暗号化方式を定義した列挙型です。
    /// </summary>
    ///
    /// <remarks>
    /// 現在のところ、以下の暗号化方式を使用する事ができます（括弧内の値は、
    /// 最初にサポートされた PDF バージョンを表します）。
    /// -  40bit RC4 (PDF 1.1)
    /// - 128bit RC4 (PDF 1.4)
    /// - 128bit AES (PDF 1.5)
    /// - 256bit AES (PDF 1.7 ExtensionLevel 3)
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public enum EncryptionMethod
    {
        Standard40,     //  40bit RC4
        Standard128,    // 128bit RC4
        Aes128,         // 128bit AES
        Aes256,         // 256bit AES
        Unknown = -1,
    }

    /* --------------------------------------------------------------------- */
    ///
    /// EncryptionException
    ///
    /// <summary>
    /// 暗号化に関する例外を送出するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Serializable]
    public class EncryptionException : Exception
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// EncryptionException
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public EncryptionException() : base() { }

        /* ----------------------------------------------------------------- */
        ///
        /// EncryptionException
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public EncryptionException(string message) : base(message) { }

        /* ----------------------------------------------------------------- */
        ///
        /// EncryptionException
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public EncryptionException(string message, Exception innerException)
            : base(message, innerException)
        { }

        #endregion
    }
}
