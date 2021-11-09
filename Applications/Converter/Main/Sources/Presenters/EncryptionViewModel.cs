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
using System.Threading;
using Cube.Pdf.Converter.Mixin;

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
    public sealed class EncryptionViewModel : PresentableBase<Encryption>
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
        public EncryptionViewModel(Encryption src,
            Aggregator aggregator,
            SynchronizationContext context
        ) : base(src, aggregator, context)
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
        /// OwnerCorrect
        ///
        /// <summary>
        /// Gets a value indicating whether the entered owner password is
        /// valid.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool OwnerCorrect => OwnerPassword == OwnerConfirm;

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
                Refresh(nameof(DividePassword), nameof(PermissionEditable));
            }
        }

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
        /// UserCorrect
        ///
        /// <summary>
        /// Gets a value indicating whether the entered user password is
        /// valid. The property will also be true when the OpenWithPassword
        /// is set to false.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool UserCorrect =>
            !OpenWithPassword ||
             SharePassword ||
            (OwnerPassword != UserPassword && UserPassword == UserConfirm);

        /* ----------------------------------------------------------------- */
        ///
        /// SharePassword
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to share the owner
        /// password with the user password.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool SharePassword
        {
            get => Get<bool>();
            set { if (Set(value)) Refresh(nameof(DividePassword), nameof(PermissionEditable)); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// DividePassword
        ///
        /// <summary>
        /// Gets or sets a value indicating whether the user password is
        /// enabled to input.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool DividePassword => OpenWithPassword && !SharePassword;

        /* ----------------------------------------------------------------- */
        ///
        /// PermissionEditable
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
        public bool PermissionEditable => !(OpenWithPassword && SharePassword);

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
            set => this.Refresh(() => Facade.Permission.Print = value.ToPermission());
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
            set => this.Refresh(() => Facade.Permission.CopyContents = value.ToPermission());
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
        public bool AllowForm
        {
            get => Facade.Permission.InputForm.IsAllowed();
            set => this.Refresh(() => Facade.Permission.InputForm = value.ToPermission());
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
            set => this.Refresh(() =>
            {
                Facade.Permission.ModifyContents    = value.ToPermission();
                Facade.Permission.ModifyAnnotations = value.ToPermission();
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
            if (!Enabled || OwnerCorrect && UserCorrect) return true;
            Send(Message.ForError(Properties.Resources.MessagePassword));
            return false;
        }

        #endregion
    }
}
