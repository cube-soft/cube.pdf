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
    /// Represents an encryption information of the PDF document.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Serializable]
    public class Encryption : SerializableBase
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Enabled
        ///
        /// <summary>
        /// Gets or sets a value indicating whether the PDF document is
        /// encrypted with password.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Enabled
        {
            get => _enabled;
            set => SetProperty(ref _enabled, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OpenWithPassword
        ///
        /// <summary>
        /// Gets or sets a value indicating whether the password is
        /// requested when opening the PDF document.
        /// </summary>
        ///
        /// <remarks>
        /// OpenWithPassword が true の場合、PDF ファイルを開く際に
        /// OwnerPassword または UserPassword を入力する必要があります。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public bool OpenWithPassword
        {
            get => _openWithPassword;
            set => SetProperty(ref _openWithPassword, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OwnerPassword
        ///
        /// <summary>
        /// Gets or sets an owner password.
        /// </summary>
        ///
        /// <remarks>
        /// 所有者パスワードとは PDF ファイルに設定されているマスター
        /// パスワードを表し、このパスワードによって再暗号化や各種権限の
        /// 変更等すべての操作が可能となります。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public string OwnerPassword
        {
            get => _ownerPassword;
            set => SetProperty(ref _ownerPassword, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// UserPassword
        ///
        /// <summary>
        /// Gets or sets a user password.
        /// </summary>
        ///
        /// <remarks>
        /// ユーザパスワードとは、PDF ファイルを開く際に必要となる
        /// パスワードを表します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public string UserPassword
        {
            get => _userPassword;
            set => SetProperty(ref _userPassword, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Method
        ///
        /// <summary>
        /// Gets or sets an encryption method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public EncryptionMethod Method
        {
            get => _method;
            set => SetProperty(ref _method, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Permission
        ///
        /// <summary>
        /// Gets or sets permissions of various operations with the
        /// encrypted PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Permission Permission
        {
            get => _permission;
            set => SetProperty(ref _permission, value);
        }

        #endregion

        #region Fields
        private bool _enabled = false;
        private bool _openWithPassword = false;
        private string _ownerPassword = string.Empty;
        private string _userPassword = string.Empty;
        private EncryptionMethod _method = EncryptionMethod.Unknown;
        private Permission _permission = new Permission();
        #endregion
    }
}
