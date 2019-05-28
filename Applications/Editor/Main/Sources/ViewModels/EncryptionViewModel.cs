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
    public sealed class EncryptionViewModel : DialogViewModel
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
        /// <param name="callback">Callback when applied.</param>
        /// <param name="src">Encryption object.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public EncryptionViewModel(Action<Encryption> callback,
            Encryption src,
            SynchronizationContext context
        ) : base(() => Properties.Resources.TitleEncryption, new Aggregator(), context)
        {
            // TODO: EncryptionFacade を追加して処理を移譲
            if (src.Method == EncryptionMethod.Unknown) src.Method = EncryptionMethod.Aes256;
            _share = new Accessor<bool>(src.OwnerPassword.HasValue() && src.OwnerPassword.FuzzyEquals(src.UserPassword));
            if (_share.Get()) src.UserPassword = string.Empty;

            _model = src;

            OpenPassword.PropertyChanged  += (s, e) => Operation.Refresh("Value");
            SharePassword.PropertyChanged += (s, e) => Operation.Refresh("Value");

            OK.Command = new BindableCommand(
                () => Execute(callback, src),
                () => CanExecute(),
                Enabled,
                OwnerPassword,
                OwnerConfirm,
                OpenPassword,
                SharePassword,
                UserPassword,
                UserConfirm
            );
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Methods
        ///
        /// <summary>
        /// Gets a collection of encryption methods.
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

        #region Elements

        /* ----------------------------------------------------------------- */
        ///
        /// Enabled
        ///
        /// <summary>
        /// Gets a menu that encryption is enabled.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<bool> Enabled => Get(() => new BindableElement<bool>(
            () => Properties.Resources.MenuEncryptionEnabled,
            () => _model.Enabled,
            e  => _model.Enabled = e,
            GetDispatcher(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// Operation
        ///
        /// <summary>
        /// Gets a menu to determine whether that permissions are settable.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<bool> Operation => Get(() => new BindableElement<bool>(
            () => Properties.Resources.MenuOperations,
            () => !OpenPassword.Value || !SharePassword.Value,
            GetDispatcher(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// Method
        ///
        /// <summary>
        /// Gets a menu of encryption method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<EncryptionMethod> Method => Get(() => new BindableElement<EncryptionMethod>(
            () => Properties.Resources.MenuEncryptionMethod,
            () => _model.Method,
            e  => _model.Method = e,
            GetDispatcher(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// OwnerPassword
        ///
        /// <summary>
        /// Gets a menu of owner password.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<string> OwnerPassword => Get(() => new BindableElement<string>(
            () => Properties.Resources.MenuOwnerPassword,
            () => _model.OwnerPassword,
            e  => _model.OwnerPassword = e,
            GetDispatcher(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// OwnerConfirm
        ///
        /// <summary>
        /// Gets a menu of owner password confirmation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<string> OwnerConfirm => Get(() => new BindableElement<string>(
            () => Properties.Resources.MenuConfirmPassword,
            new Accessor<string>(),
            GetDispatcher(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// UserPassword
        ///
        /// <summary>
        /// Gets a menu of user password.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<string> UserPassword => Get(() => new BindableElement<string>(
            () => Properties.Resources.MenuUserPassword,
            () => _model.UserPassword,
            e  => _model.UserPassword = e,
            GetDispatcher(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// UserConfirm
        ///
        /// <summary>
        /// Gets a menu of user password confirmation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<string> UserConfirm => Get(() => new BindableElement<string>(
            () => Properties.Resources.MenuConfirmPassword,
            new Accessor<string>(),
            GetDispatcher(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// OpenPassword
        ///
        /// <summary>
        /// Gets a menu to determine whether to enable user password.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<bool> OpenPassword => Get(() => new BindableElement<bool>(
            () => Properties.Resources.MenuOpenWithPassword,
            () => _model.OpenWithPassword,
            e  => _model.OpenWithPassword = e,
            GetDispatcher(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// SharePassword
        ///
        /// <summary>
        /// Gets a menu to determine whether to share the user password
        /// with the onwer password.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<bool> SharePassword => Get(() => new BindableElement<bool>(
            () => Properties.Resources.MenuSharePassword,
            _share,
            GetDispatcher(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// AllowPrint
        ///
        /// <summary>
        /// Gets a menu indicating whether to allow print.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<bool> AllowPrint => Get(() => new BindableElement<bool>(
            () => Properties.Resources.MenuAllowPrint,
            () => _model.Permission.Print.IsAllowed(),
            e  => _model.Permission.Print = GetValue(e),
            GetDispatcher(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// AllowCopy
        ///
        /// <summary>
        /// Gets a menu indicating whether to allow copying of text and
        /// images.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<bool> AllowCopy => Get(() => new BindableElement<bool>(
            () => Properties.Resources.MenuAllowCopy,
            () => _model.Permission.CopyContents.IsAllowed(),
            e  => _model.Permission.CopyContents = GetValue(e),
            GetDispatcher(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// AllowModify
        ///
        /// <summary>
        /// Gets a menu indicating whether to allow page insertion,
        /// rotation, or deletion.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<bool> AllowModify => Get(() => new BindableElement<bool>(
            () => Properties.Resources.MenuAllowAssemble,
            () => _model.Permission.ModifyContents.IsAllowed(),
            e  => _model.Permission.ModifyContents = GetValue(e),
            GetDispatcher(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// AllowAccessibility
        ///
        /// <summary>
        /// Gets a menu indicating whether to allow content extraction
        /// for accessibility.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<bool> AllowAccessibility => Get(() => new BindableElement<bool>(
            () => Properties.Resources.MenuAllowAccessibility,
            () => _model.Permission.Accessibility.IsAllowed(),
            e  => _model.Permission.Accessibility = GetValue(e),
            GetDispatcher(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// AllowForm
        ///
        /// <summary>
        /// Gets a menu indicating whether to allow input to form fields.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<bool> AllowForm => Get(() => new BindableElement<bool>(
            () => Properties.Resources.MenuAllowForm,
            () => _model.Permission.InputForm.IsAllowed(),
            e  => _model.Permission.InputForm = GetValue(e),
            GetDispatcher(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// AllowAnnotation
        ///
        /// <summary>
        /// Gets a menu indicating whether to allow creation or editing of
        /// annotations.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<bool> AllowAnnotation => Get(() => new BindableElement<bool>(
            () => Properties.Resources.MenuAllowAnnotation,
            () => _model.Permission.ModifyAnnotations.IsAllowed(),
            e  => _model.Permission.ModifyAnnotations = GetValue(e),
            GetDispatcher(false)
        ));

        #endregion

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
            if (SharePassword.Value) src.UserPassword = src.OwnerPassword;
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

            if (!OpenPassword.Value) return true;

            var share = SharePassword.Value;
            var user  = UserPassword.Value;
            var uc    = UserConfirm.Value;
            if (!share && (!user.HasValue() || !user.FuzzyEquals(uc))) return false;

            return true;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetValue
        ///
        /// <summary>
        /// Gets a value of the PermissionValue enum from the specified
        /// boolean value.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private PermissionValue GetValue(bool src) => src ? PermissionValue.Allow : PermissionValue.Deny;

        #endregion

        #region Fields
        private readonly Encryption _model;
        private readonly Accessor<bool> _share;
        #endregion
    }
}
