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
using Cube.Pdf.Mixin;
using Cube.Xui;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// EncryptionViewModel
    ///
    /// <summary>
    /// Represents the ViewModel for a EncryptionWindow instance.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class EncryptionViewModel : DialogViewModel
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// EncryptionViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the EncryptionViewModel
        /// with the specified argumetns.
        /// </summary>
        ///
        /// <param name="callback">Callback method when applied.</param>
        /// <param name="src">Encryption object.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public EncryptionViewModel(Action<Encryption> callback, Encryption src, SynchronizationContext context) :
            base(() => Properties.Resources.TitleEncryption, new Aggregator(), context)
        {
            if (src.Method == EncryptionMethod.Unknown) src.Method = EncryptionMethod.Aes256;

            IsSharePassword = CreateShare(src);
            Enabled         = this.Create(() => src.Enabled,          e => src.Enabled          = e, () => Properties.Resources.MenuEncryptionEnabled, Dispatcher);
            Method          = this.Create(() => src.Method,           e => src.Method           = e, () => Properties.Resources.MenuEncryptionMethod,  Dispatcher);
            OwnerPassword   = this.Create(() => src.OwnerPassword,    e => src.OwnerPassword    = e, () => Properties.Resources.MenuOwnerPassword,     Dispatcher);
            OwnerConfirm    = new BindableElement<string>(() => Properties.Resources.MenuConfirmPassword, Dispatcher);
            UserPassword    = this.Create(() => src.UserPassword,     e => src.UserPassword     = e, () => Properties.Resources.MenuUserPassword,      Dispatcher);
            UserConfirm     = new BindableElement<string>(() => Properties.Resources.MenuConfirmPassword, Dispatcher);
            IsSharePassword = new BindableElement<bool>(() => Properties.Resources.MenuSharePassword, Dispatcher);
            IsOpenPassword  = this.Create(() => src.OpenWithPassword, e => src.OpenWithPassword = e, () => Properties.Resources.MenuOpenWithPassword,  Dispatcher);
            Operation       = this.Create(() => !IsOpenPassword.Value || !IsSharePassword.Value, () => Properties.Resources.MenuOperations, Dispatcher);

            IsOpenPassword.PropertyChanged  += (s, e) => Operation.Refresh("Value");
            IsSharePassword.PropertyChanged += (s, e) => Operation.Refresh("Value");

            var pm = src.Permission;

            AllowPrint = this.Create(() => pm.Print.IsAllowed(),
                e  => pm.Print = Convert(e),
                () => Properties.Resources.MenuAllowPrint,
                Dispatcher);
            AllowCopy = this.Create(() => pm.CopyContents.IsAllowed(),
                e  => pm.CopyContents = Convert(e),
                () => Properties.Resources.MenuAllowCopy,
                Dispatcher);
            AllowAccessibility = this.Create(() => pm.Accessibility.IsAllowed(),
                e  => pm.Accessibility = Convert(e),
                () => Properties.Resources.MenuAllowAccessibility,
                Dispatcher);
            AllowForm = this.Create(() => pm.InputForm.IsAllowed(),
                e  => pm.InputForm = Convert(e),
                () => Properties.Resources.MenuAllowForm,
                Dispatcher);
            AllowAnnotation = this.Create(() => pm.ModifyAnnotations.IsAllowed(),
                e  => pm.ModifyAnnotations = Convert(e),
                () => Properties.Resources.MenuAllowAnnotation,
                Dispatcher);
            AllowModify = CreateModify(pm);

            OK.Command = new BindableCommand(
                () => Execute(callback, src),
                () => CanExecute(),
                Enabled,
                OwnerPassword,
                OwnerConfirm,
                IsOpenPassword,
                IsSharePassword,
                UserPassword,
                UserConfirm
            );
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Enabled
        ///
        /// <summary>
        /// Gets the menu that encryption is enabled.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<bool> Enabled { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Method
        ///
        /// <summary>
        /// Gets the encryption method menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<EncryptionMethod> Method { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// OwnerPassword
        ///
        /// <summary>
        /// Gets the owner password menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<string> OwnerPassword { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// OwnerConfirm
        ///
        /// <summary>
        /// Gets the confirmed owner password menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<string> OwnerConfirm { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// UserPassword
        ///
        /// <summary>
        /// Gets the user password menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<string> UserPassword { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// UserConfirm
        ///
        /// <summary>
        /// Gets the confirmed user password menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<string> UserConfirm { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// IsOpenPassword
        ///
        /// <summary>
        /// Gets the menu that user password is enabled.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<bool> IsOpenPassword { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// IsSharePassword
        ///
        /// <summary>
        /// Gets the menu of sharing user password with owner password.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<bool> IsSharePassword { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Operation
        ///
        /// <summary>
        /// Gets the operation menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<bool> Operation { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// AllowPrint
        ///
        /// <summary>
        /// Gets the menu indicating whether printing is allowed.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<bool> AllowPrint { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// AllowCopy
        ///
        /// <summary>
        /// Gets the menu indicating whether copying texts and images is
        /// allowed.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<bool> AllowCopy { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// AllowModify
        ///
        /// <summary>
        /// Gets the menu indicating whether inserting, rotating, and
        /// removing pages is allowed.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<bool> AllowModify { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// AllowAccessibility
        ///
        /// <summary>
        /// Gets the menu indicating whether extracting contents for
        /// accessibility is allowed.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<bool> AllowAccessibility { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// AllowForm
        ///
        /// <summary>
        /// Gets the menu indicating whether filling in form fields is
        /// allowed.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<bool> AllowForm { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// AllowAnnotation
        ///
        /// <summary>
        /// Gets the menu indicating whether creating and editing
        /// annotation is allowed.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<bool> AllowAnnotation { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Methods
        ///
        /// <summary>
        /// Gets the collection of encryption methods.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<EncryptionMethod> Methods { get; } = new[]
        {
            EncryptionMethod.Standard40,
            EncryptionMethod.Standard128,
            EncryptionMethod.Aes128,
            EncryptionMethod.Aes256,
        };

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Execute
        ///
        /// <summary>
        /// Executes the specified callback method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Execute(Action<Encryption> callback, Encryption src)
        {
            Send<CloseMessage>();
            if (IsSharePassword.Value) src.UserPassword = src.OwnerPassword;
            callback(src);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CanExecute
        ///
        /// <summary>
        /// Gets the value indicating whether the OK button can be clicked.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private bool CanExecute()
        {
            if (!Enabled.Value) return true;

            var owner = OwnerPassword.Value;
            var oc    = OwnerConfirm.Value;
            if (!owner.HasValue() || !owner.FuzzyEquals(oc)) return false;

            if (!IsOpenPassword.Value) return true;

            var share = IsSharePassword.Value;
            var user  = UserPassword.Value;
            var uc    = UserConfirm.Value;
            if (!share && (!user.HasValue() || !user.FuzzyEquals(uc))) return false;

            return true;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Convert
        ///
        /// <summary>
        /// Converts the specified value to the PermissionMethod.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private PermissionValue Convert(bool src) =>
            src ? PermissionValue.Allow : PermissionValue.Deny;

        /* ----------------------------------------------------------------- */
        ///
        /// CreateModify
        ///
        /// <summary>
        /// Creates a new menu with the specified arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private BindableElement<bool> CreateModify(Permission src) => this.Create(
            () => src.ModifyContents.IsAllowed(),
            e  => src.ModifyContents = Convert(e),
            () => Properties.Resources.MenuAllowAssemble,
            Dispatcher
        );

        /* ----------------------------------------------------------------- */
        ///
        /// CreateShare
        ///
        /// <summary>
        /// Creates a new menu with the specified arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private BindableElement<bool> CreateShare(Encryption src)
        {
            var value = src.OwnerPassword.HasValue() && src.OwnerPassword.FuzzyEquals(src.UserPassword);
            if (value) src.UserPassword = string.Empty;
            return new BindableElement<bool>(value, () => Properties.Resources.MenuSharePassword, Dispatcher);
        }

        #endregion
    }
}
