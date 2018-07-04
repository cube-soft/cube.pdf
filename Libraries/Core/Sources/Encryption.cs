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
    public class Encryption : ObservableProperty
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Enabled
        ///
        /// <summary>
        /// 暗号化を有効化するかどうかを示す値を取得または設定します。
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
        /// PDF ファイルを開く際にパスワードを要求するかどうかを示す値を
        /// 取得または設定します。
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
        /// ユーザパスワードを取得または設定します。
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
        /// 適用する暗号化方式を取得または設定します。
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
        /// 暗号化された PDF に設定されている各種権限の状態を取得
        /// または設定します。
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
