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
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Cube.Mixin.String;

namespace Cube.Pdf.Converter
{
    /* --------------------------------------------------------------------- */
    ///
    /// EncryptionViewModel
    ///
    /// <summary>
    /// Represents the ViewModel for the security tab in the main window.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class EncryptionViewModel : Presentable<Encryption>
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
        /// <param name="src">PDF encryption information.</param>
        /// <param name="aggregator">Event aggregator.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public EncryptionViewModel(Encryption src, Aggregator aggregator,
            SynchronizationContext context) : base(src, aggregator, context)
        {
            Assets.Add(new ObservableProxy(Facade, this));
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
            get => Facade.Enabled;
            set => Facade.Enabled = value;
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
            get => Facade.OwnerPassword;
            set => Facade.OwnerPassword = value;
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
            get => Facade.UserPassword;
            set => Facade.UserPassword = value;
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
            get => Facade.OpenWithPassword;
            set
            {
                Facade.OpenWithPassword = value;
                Refresh(nameof(EnableUserPassword), nameof(EnablePermission));
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
            get => Get<bool>();
            set { if (Set(value)) Refresh(nameof(EnableUserPassword), nameof(EnablePermission)); }
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
        /// If the user password is shared with the owner password,
        /// the permission settings are not permitted.
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
            get => Facade.Permission.Print.IsAllowed();
            set => Update(() => Facade.Permission.Print = GetPermission(value));
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
            get => Facade.Permission.CopyContents.IsAllowed();
            set => Update(() => Facade.Permission.CopyContents = GetPermission(value));
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
            get => Facade.Permission.InputForm.IsAllowed();
            set => Update(() => Facade.Permission.InputForm = GetPermission(value));
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
            get => Facade.Permission.ModifyContents.IsAllowed();
            set => Update(() =>
            {
                Facade.Permission.ModifyContents    = GetPermission(value);
                Facade.Permission.ModifyAnnotations = GetPermission(value);
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
            var user  = !OpenWithPassword ||
                        UseOwnerPassword ||
                        UserPassword.FuzzyEquals(UserConfirm);
            if (owner && user) return true;

            Send(Message.ForError(Properties.Resources.MessagePassword));
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
            Refresh(name);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetPermission
        ///
        /// <summary>
        /// Gets the permission value.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private PermissionValue GetPermission(bool allow) =>
            allow ? PermissionValue.Allow : PermissionValue.Deny;

        #endregion
    }
}
