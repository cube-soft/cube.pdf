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
namespace Cube.Pdf.App.Converter
{
    /* --------------------------------------------------------------------- */
    ///
    /// EncryptionViewModel
    ///
    /// <summary>
    /// セキュリティタブを表す ViewModel です。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class EncryptionViewModel : Cube.Forms.ViewModelBase<Messenger>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// EncryptionViewModel
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="model">PDF 暗号化情報</param>
        /// <param name="messenger">Messenger オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public EncryptionViewModel(Encryption model, Messenger messenger) : base(messenger)
        {
            _model = model;
            _model.PropertyChanged += (s, e) => OnPropertyChanged(e);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Enabled
        ///
        /// <summary>
        /// 暗号化を有効にするかどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Enabled
        {
            get => _model.Enabled;
            set => _model.Enabled = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OwnerPassword
        ///
        /// <summary>
        /// 管理用パスワードを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string OwnerPassword
        {
            get => _model.OwnerPassword;
            set => _model.OwnerPassword = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OwnerConfirm
        ///
        /// <summary>
        /// 管理用パスワードの確認を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string OwnerConfirm { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// UserPassword
        ///
        /// <summary>
        /// 閲覧用パスワードを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string UserPassword
        {
            get => _model.UserPassword;
            set => _model.UserPassword = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// UserConfirm
        ///
        /// <summary>
        /// 閲覧用パスワードの確認を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string UserConfirm { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// OpenWithPassword
        ///
        /// <summary>
        /// ファイルを開く時にパスワードを要求するかどうかを示す値を取得
        /// または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool OpenWithPassword
        {
            get => _model.OpenWithPassword;
            set
            {
                _model.OpenWithPassword = value;
                RaisePropertyChanged(nameof(EnableUserPassword));
                RaisePropertyChanged(nameof(EnablePermission));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// UseOwnerPassword
        ///
        /// <summary>
        /// 閲覧用パスワードと管理用パスワードを共用するかどうかを示す値を
        /// 取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool UseOwnerPassword
        {
            get => _useOwnerPassword;
            set
            {
                if (SetProperty(ref _useOwnerPassword, value))
                {
                    RaisePropertyChanged(nameof(EnableUserPassword));
                    RaisePropertyChanged(nameof(EnablePermission));
                }
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// EnableUserPassword
        ///
        /// <summary>
        /// 閲覧用パスワードを入力可能な状態かどうかを示す値を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool EnableUserPassword => OpenWithPassword && !UseOwnerPassword;

        /* ----------------------------------------------------------------- */
        ///
        /// EnablePermission
        ///
        /// <summary>
        /// 各種操作の許可設定を変更できるかどうかを示す値を取得または
        /// 設定します。
        /// </summary>
        ///
        /// <remarks>
        /// 閲覧用パスワードと管理用パスワードを共用する場合、許可設定を
        /// 変更する事はできません。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public bool EnablePermission => !(OpenWithPassword && UseOwnerPassword);

        /* ----------------------------------------------------------------- */
        ///
        /// AllowPrint
        ///
        /// <summary>
        /// 印刷を許可するかどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool AllowPrint
        {
            get => _model.Permission.Print == PermissionMethod.Allow;
            set
            {
                _model.Permission.Print = GetMethod(value);
                RaisePropertyChanged(nameof(AllowPrint));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// AllowCopy
        ///
        /// <summary>
        /// ページのコピーを許可するかどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool AllowCopy
        {
            get => _model.Permission.CopyContents == PermissionMethod.Allow;
            set
            {
                _model.Permission.CopyContents = GetMethod(value);
                RaisePropertyChanged(nameof(AllowCopy));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// AllowFillInFormFields
        ///
        /// <summary>
        /// フォームへの入力を許可するかどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool AllowFillInFormFields
        {
            get => _model.Permission.FillInFormFields == PermissionMethod.Allow;
            set
            {
                _model.Permission.FillInFormFields  = GetMethod(value);
                RaisePropertyChanged(nameof(AllowFillInFormFields));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// AllowModify
        ///
        /// <summary>
        /// コンテンツの修正を許可するかどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool AllowModify
        {
            get => _model.Permission.ModifyContents == PermissionMethod.Allow;
            set
            {
                _model.Permission.Assemble          = GetMethod(value);
                _model.Permission.ModifyContents    = GetMethod(value);
                _model.Permission.ModifyAnnotations = GetMethod(value);
                RaisePropertyChanged(nameof(AllowModify));
            }
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetMethod
        ///
        /// <summary>
        /// 真偽値に対応する PermissionMethod オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private PermissionMethod GetMethod(bool allow) =>
            allow ? PermissionMethod.Allow : PermissionMethod.Deny;

        #endregion

        #region Fields
        private readonly Encryption _model;
        private bool _useOwnerPassword;
        #endregion
    }
}
