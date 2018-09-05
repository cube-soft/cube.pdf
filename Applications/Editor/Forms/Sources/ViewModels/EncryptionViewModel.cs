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
using GalaSoft.MvvmLight.Messaging;
using System.Threading;

namespace Cube.Pdf.App.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// EncryptionViewModel
    ///
    /// <summary>
    /// Represents the ViewModel for a <c>EncryptionWindow</c> instance.
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
        /// Initializes a new instance of the <c>EncryptionViewModel</c>
        /// with the specified argumetns.
        /// </summary>
        ///
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public EncryptionViewModel(SynchronizationContext context) :
            base(() => Properties.Resources.TitleEncryption, new Messenger(), context) { }

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
        public BindableElement Enabled { get; } = new BindableElement(
            () => Properties.Resources.MenuEncryptionEnabled
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Method
        ///
        /// <summary>
        /// Gets the encryption method menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement Method { get; } = new BindableElement(
            () => Properties.Resources.MenuEncryptionMethod
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Operation
        ///
        /// <summary>
        /// Gets the operation menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement Operation { get; } = new BindableElement(
            () => Properties.Resources.MenuOperations
        );

        /* ----------------------------------------------------------------- */
        ///
        /// OwnerPassword
        ///
        /// <summary>
        /// Gets the owner password menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement OwnerPassword { get; } = new BindableElement(
            () => Properties.Resources.MenuOwnerPassword
        );

        /* ----------------------------------------------------------------- */
        ///
        /// OwnerConfirm
        ///
        /// <summary>
        /// Gets the confirmed owner password menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement OwnerConfirm { get; } = new BindableElement(
            () => Properties.Resources.MenuConfirmPassword
        );

        /* ----------------------------------------------------------------- */
        ///
        /// UserPassword
        ///
        /// <summary>
        /// Gets the user password menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement UserPassword { get; } = new BindableElement(
            () => Properties.Resources.MenuUserPassword
        );

        /* ----------------------------------------------------------------- */
        ///
        /// UserConfirm
        ///
        /// <summary>
        /// Gets the confirmed user password menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement UserConfirm { get; } = new BindableElement(
            () => Properties.Resources.MenuConfirmPassword
        );

        /* ----------------------------------------------------------------- */
        ///
        /// OpenWithPassword
        ///
        /// <summary>
        /// Gets the menu that user password is enabled.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement OpenWithPassword { get; } = new BindableElement(
            () => Properties.Resources.MenuOpenWithPassword
        );

        /* ----------------------------------------------------------------- */
        ///
        /// SharePassword
        ///
        /// <summary>
        /// Gets the menu of sharing user password with owner password.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement SharePassword { get; } = new BindableElement(
            () => Properties.Resources.MenuSharePassword
        );

        /* ----------------------------------------------------------------- */
        ///
        /// AllowPrint
        ///
        /// <summary>
        /// Gets the menu indicating whether printing is allowed.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement AllowPrint { get; } = new BindableElement(
            () => Properties.Resources.MenuAllowPrint
        );

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
        public BindableElement AllowCopy { get; } = new BindableElement(
            () => Properties.Resources.MenuAllowCopy
        );

        /* ----------------------------------------------------------------- */
        ///
        /// AllowAssemble
        ///
        /// <summary>
        /// Gets the menu indicating whether inserting, rotating, and
        /// removing pages is allowed.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement AllowAssemble { get; } = new BindableElement(
            () => Properties.Resources.MenuAllowAssemble
        );

        /* ----------------------------------------------------------------- */
        ///
        /// AllowExtract
        ///
        /// <summary>
        /// Gets the menu indicating whether extracting pages is allowed.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement AllowExtract { get; } = new BindableElement(
            () => Properties.Resources.MenuAllowExtract
        );

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
        public BindableElement AllowAccessibility { get; } = new BindableElement(
            () => Properties.Resources.MenuAllowAccessibility
        );

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
        public BindableElement AllowForm { get; } = new BindableElement(
            () => Properties.Resources.MenuAllowForm
        );

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
        public BindableElement AllowAnnotation { get; } = new BindableElement(
            () => Properties.Resources.MenuAllowAnnotation
        );

        #endregion
    }
}
