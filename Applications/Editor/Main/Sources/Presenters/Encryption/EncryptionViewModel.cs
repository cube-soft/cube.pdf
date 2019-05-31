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
            _model = new EncryptionFacade(src);

            OpenPassword.PropertyChanged  += (s, e) => Operation.Refresh("Value");
            SharePassword.PropertyChanged += (s, e) => Operation.Refresh("Value");

            OK.Command = new BindableCommand(
                () =>
                {
                    Send<CloseMessage>();
                    _model.Normalize();
                    callback(_model.Value);
                },
                () => _model.IsAcceptable())
            .Observe(Enabled)
            .Observe(OwnerPassword)
            .Observe(OwnerConfirm)
            .Observe(OpenPassword)
            .Observe(SharePassword)
            .Observe(UserPassword)
            .Observe(UserConfirm);
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
        public IEnumerable<EncryptionMethod> Methods => EncryptionFacade.Methods;

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
            () => _model.Value.Enabled,
            e  => _model.Value.Enabled = e,
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
            () => _model.Value.Method,
            e  => _model.Value.Method = e,
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
            () => _model.Value.OwnerPassword,
            e  => _model.Value.OwnerPassword = e,
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
            () => _model.OwnerConfirm,
            e  => _model.OwnerConfirm = e,
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
            () => _model.Value.UserPassword,
            e  => _model.Value.UserPassword = e,
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
            () => _model.UserConfirm,
            e  => _model.UserConfirm = e,
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
            () => _model.Value.OpenWithPassword,
            e  => _model.Value.OpenWithPassword = e,
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
            () => _model.SharePassword,
            e  => _model.SharePassword = e,
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
            () => _model.Value.Permission.Print.IsAllowed(),
            e  => _model.Value.Permission.Print = _model.GetPermission(e),
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
            () => _model.Value.Permission.CopyContents.IsAllowed(),
            e  => _model.Value.Permission.CopyContents = _model.GetPermission(e),
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
            () => _model.Value.Permission.ModifyContents.IsAllowed(),
            e  => _model.Value.Permission.ModifyContents = _model.GetPermission(e),
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
            () => _model.Value.Permission.Accessibility.IsAllowed(),
            e  => _model.Value.Permission.Accessibility = _model.GetPermission(e),
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
            () => _model.Value.Permission.InputForm.IsAllowed(),
            e  => _model.Value.Permission.InputForm = _model.GetPermission(e),
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
            () => _model.Value.Permission.ModifyAnnotations.IsAllowed(),
            e  => _model.Value.Permission.ModifyAnnotations = _model.GetPermission(e),
            GetDispatcher(false)
        ));

        #endregion

        #endregion

        #region Fields
        private readonly EncryptionFacade _model;
        #endregion
    }
}
