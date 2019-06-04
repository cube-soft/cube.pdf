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
using Cube.Mixin.Pdf;
using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Cube.Pdf.Converter
{
    /* --------------------------------------------------------------------- */
    ///
    /// EncryptionViewModel
    ///
    /// <summary>
    /// Represents the viewmodel for the security tab in the main window.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class EncryptionViewModel : CommonViewModel
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// EncryptionViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the EncryptionViewModel class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="model">PDF encryption information.</param>
        /// <param name="aggregator">Event aggregator.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public EncryptionViewModel(Encryption model, Aggregator aggregator,
            SynchronizationContext context) : base(aggregator, context)
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
        /// Gets or sets a value indicating to enable the security options.
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
        /// Gets or sets the owner password.
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
        /// Gets or sets the confirmed value of owner password.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string OwnerConfirm { get; set; } = string.Empty;

        /* ----------------------------------------------------------------- */
        ///
        /// UserPassword
        ///
        /// <summary>
        /// Gets or sets the user password.
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
        /// Gets or sets the confirmed value of user password.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string UserConfirm { get; set; } = string.Empty;

        /* ----------------------------------------------------------------- */
        ///
        /// OpenWithPassword
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to require password a
        /// when opening the PDF file.
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
        /// Gets or sets a value indicating whether to share the owner
        /// password with the user password.
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
        /// Gets or sets a value indicating whether the user password is
        /// enabled to input.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool EnableUserPassword => OpenWithPassword && !UseOwnerPassword;

        /* ----------------------------------------------------------------- */
        ///
        /// EnablePermission
        ///
        /// <summary>
        /// Gets or sets a value indicating whether the permission values
        /// are enabled to input.
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
        /// Gets or sets a value indicating whether to allow printing.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool AllowPrint
        {
            get => _model.Permission.Print.IsAllowed();
            set => Update(() => _model.Permission.Print = GetMethod(value));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// AllowCopy
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to allow copying the
        /// PDF contents.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool AllowCopy
        {
            get => _model.Permission.CopyContents.IsAllowed();
            set => Update(() => _model.Permission.CopyContents = GetMethod(value));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// AllowInputForm
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to allow inputting to
        /// the form fields.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool AllowInputForm
        {
            get => _model.Permission.InputForm.IsAllowed();
            set => Update(() => _model.Permission.InputForm = GetMethod(value));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// AllowModify
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to allow modifying
        /// the PDF contents.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool AllowModify
        {
            get => _model.Permission.ModifyContents.IsAllowed();
            set => Update(() =>
            {
                _model.Permission.ModifyContents    = GetMethod(value);
                _model.Permission.ModifyAnnotations = GetMethod(value);
            });
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Confirm
        ///
        /// <summary>
        /// Confirms if the current settings are acceptable.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Confirm()
        {
            if (!Enabled) return true;

            var owner = OwnerPassword.FuzzyEquals(OwnerConfirm);
            var user = !OpenWithPassword ||
                         UseOwnerPassword ||
                         UserPassword.FuzzyEquals(UserConfirm);
            if (owner && user) return true;

            Send(MessageFactory.CreateError(Properties.Resources.MessagePassword));
            return false;
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Update
        ///
        /// <summary>
        /// Invokes the specified action and RaisePropertyChanged.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Update(Action action, [CallerMemberName] string name = null)
        {
            action();
            RaisePropertyChanged(name);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetMethod
        ///
        /// <summary>
        /// Gets the permission value.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private PermissionValue GetMethod(bool allow) =>
            allow ? PermissionValue.Allow : PermissionValue.Deny;

        #endregion

        #region Fields
        private readonly Encryption _model;
        private bool _useOwnerPassword;
        #endregion
    }
}
