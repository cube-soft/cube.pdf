/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2013 CubeSoft, Inc.
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

namespace Cube.Pdf.Pages
{
    /* --------------------------------------------------------------------- */
    ///
    /// EncryptionViewModel
    ///
    /// <summary>
    /// Represents the ViewModel for the EncryptionTab.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class EncryptionViewModel : PresentableBase<Encryption>
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
        /// <param name="src">Source information.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public EncryptionViewModel(Encryption src, SynchronizationContext context) :
            base(src, new(), context)
        {
            Enabled            = src.Enabled;
            OwnerPassword      = src.OwnerPassword;
            OpenWithPassword   = src.OpenWithPassword;
            UserPassword       = src.UserPassword;
            AllowPrint         = src.Permission.Print.IsAllowed();
            AllowCopy          = src.Permission.CopyContents.IsAllowed();
            AllowModify        = src.Permission.ModifyContents.IsAllowed();
            AllowAccessibility = src.Permission.Accessibility.IsAllowed();
            AllowForm          = src.Permission.InputForm.IsAllowed();
            AllowAnnotation    = src.Permission.ModifyAnnotations.IsAllowed();
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
            get => Get(() => false);
            set => Set(value);
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
            get => Get(() => string.Empty);
            set => Set(value);
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
        public string OwnerConfirm
        {
            get => Get(() => string.Empty);
            set => Set(value);
        }

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
            get => Get(() => false);
            set => Set(value);
        }

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
            get => Get(() => false);
            set => Set(value);
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
            get => Get(() => string.Empty);
            set => Set(value);
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
        public string UserConfirm
        {
            get => Get(() => string.Empty);
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// UserRequired
        ///
        /// <summary>
        /// Gets or sets a value indicating whether the user password is
        /// required to input.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool UserRequired => OpenWithPassword && !SharePassword;

        /* ----------------------------------------------------------------- */
        ///
        /// Permissible
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
        public bool Permissible => !(OpenWithPassword && SharePassword);

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
            get => Get(() => false);
            set => Set(value);
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
            get => Get(() => false);
            set => Set(value);
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
            get => Get(() => false);
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// AllowAccessibility
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to allow content
        /// extraction for accessibility.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool AllowAccessibility
        {
            get => Get(() => false);
            set => Set(value);
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
            get => Get(() => false);
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// AllowAnnotation
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to allow creation or
        /// editing of annotations.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool AllowAnnotation
        {
            get => Get(() => false);
            set => Set(value);
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Apply
        ///
        /// <summary>
        /// Apply the user settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Apply()
        {
            Facade.Enabled          = Enabled;
            Facade.OwnerPassword    = OwnerPassword;
            Facade.OpenWithPassword = OpenWithPassword;
            Facade.UserPassword     = SharePassword ? OwnerPassword : UserPassword;

            static PermissionValue cvt(bool e) => e ? PermissionValue.Allow : PermissionValue.Deny;
            Facade.Permission.Print             = cvt(AllowPrint);
            Facade.Permission.CopyContents      = cvt(AllowCopy);
            Facade.Permission.ModifyContents    = cvt(AllowModify);
            Facade.Permission.Accessibility     = cvt(AllowAccessibility);
            Facade.Permission.InputForm         = cvt(AllowForm);
            Facade.Permission.ModifyAnnotations = cvt(AllowAnnotation);
        }

        #endregion
    }
}
