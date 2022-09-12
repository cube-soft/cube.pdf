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
namespace Cube.Pdf.Converter;

using System.Threading;
using Cube.Observable.Extensions;
using Cube.Text.Extensions;

/* ------------------------------------------------------------------------- */
///
/// EncryptionViewModel
///
/// <summary>
/// Represents the ViewModel for the security tab in the main window.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public sealed class EncryptionViewModel : PresentableBase<Encryption>
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// EncryptionViewModel
    ///
    /// <summary>
    /// Initializes a new instance of the EncryptionViewModel class
    /// with the specified arguments.
    /// </summary>
    ///
    /// <param name="src">PDF encryption information.</param>
    /// <param name="proxy">Message aggregator.</param>
    /// <param name="ctx">Synchronization context.</param>
    ///
    /* --------------------------------------------------------------------- */
    public EncryptionViewModel(Encryption src, Aggregator proxy, SynchronizationContext ctx) :
        base(src, proxy, ctx) => Assets.Add(src.Forward(this));

    #endregion

    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// Enabled
    ///
    /// <summary>
    /// Gets or sets a value indicating to enable the security options.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool Enabled
    {
        get => Facade.Enabled;
        set => Facade.Enabled = value;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// OwnerPassword
    ///
    /// <summary>
    /// Gets or sets the owner password.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string OwnerPassword
    {
        get => Facade.OwnerPassword;
        set => Facade.OwnerPassword = value;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// OwnerConfirm
    ///
    /// <summary>
    /// Gets or sets the confirmed value of owner password.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string OwnerConfirm
    {
        get => Get(() => string.Empty);
        set => Set(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// OwnerCorrect
    ///
    /// <summary>
    /// Gets a value indicating whether the entered owner password is
    /// correct.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool OwnerCorrect => OwnerPassword.HasValue() && OwnerPassword == OwnerConfirm;

    /* --------------------------------------------------------------------- */
    ///
    /// OpenWithPassword
    ///
    /// <summary>
    /// Gets or sets a value indicating whether to require password a
    /// when opening the PDF file.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool OpenWithPassword
    {
        get => Facade.OpenWithPassword;
        set => Facade.OpenWithPassword = value;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// SharePassword
    ///
    /// <summary>
    /// Gets or sets a value indicating whether to share the owner
    /// password with the user password.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool SharePassword
    {
        get => Get(() => false);
        set => Set(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// UserPassword
    ///
    /// <summary>
    /// Gets or sets the user password.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string UserPassword
    {
        get => Facade.UserPassword;
        set => Facade.UserPassword = value;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// UserConfirm
    ///
    /// <summary>
    /// Gets or sets the confirmed value of user password.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string UserConfirm
    {
        get => Get(() => string.Empty);
        set => Set(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// UserCorrect
    ///
    /// <summary>
    /// Gets a value indicating whether the entered user password is
    /// correct. The property will also be true when the OpenWithPassword
    /// is set to false.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool UserCorrect => !OpenWithPassword || SharePassword || (
        UserPassword.HasValue() &&
        UserPassword != OwnerPassword &&
        UserPassword == UserConfirm
    );

    /* --------------------------------------------------------------------- */
    ///
    /// UserRequired
    ///
    /// <summary>
    /// Gets or sets a value indicating whether the user password is
    /// required to input.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool UserRequired => OpenWithPassword && !SharePassword;

    /* --------------------------------------------------------------------- */
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
    /* --------------------------------------------------------------------- */
    public bool Permissible => !(OpenWithPassword && SharePassword);

    /* --------------------------------------------------------------------- */
    ///
    /// AllowPrint
    ///
    /// <summary>
    /// Gets or sets a value indicating whether to allow printing.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool AllowPrint
    {
        get => Facade.Permission.Print.IsAllowed();
        set => Facade.Permission.Print = GetPermission(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// AllowCopy
    ///
    /// <summary>
    /// Gets or sets a value indicating whether to allow copying the
    /// PDF contents.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool AllowCopy
    {
        get => Facade.Permission.CopyContents.IsAllowed();
        set => Facade.Permission.CopyContents = GetPermission(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// AllowModify
    ///
    /// <summary>
    /// Gets or sets a value indicating whether to allow modifying
    /// the PDF contents.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool AllowModify
    {
        get => Facade.Permission.ModifyContents.IsAllowed();
        set => Facade.Permission.ModifyContents = GetPermission(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// AllowAccessibility
    ///
    /// <summary>
    /// Gets or sets a value indicating whether to allow content
    /// extraction for accessibility.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool AllowAccessibility
    {
        get => Facade.Permission.Accessibility.IsAllowed();
        set => Facade.Permission.Accessibility = GetPermission(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// AllowForm
    ///
    /// <summary>
    /// Gets or sets a value indicating whether to allow inputting to
    /// the form fields.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool AllowForm
    {
        get => Facade.Permission.InputForm.IsAllowed();
        set => Facade.Permission.InputForm = GetPermission(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// AllowAnnotation
    ///
    /// <summary>
    /// Gets or sets a value indicating whether to allow creation or
    /// editing of annotations.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool AllowAnnotation
    {
        get => Facade.Permission.ModifyAnnotations.IsAllowed();
        set => Facade.Permission.ModifyAnnotations = GetPermission(value);
    }

    #endregion

    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// Confirm
    ///
    /// <summary>
    /// Confirms if the current settings are acceptable.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool Confirm()
    {
        bool fail(string s) { Send(Message.Error(s)); return false; }
        if (Enabled && !OwnerCorrect) return fail(Properties.Resources.ErrorOwnerPassword);
        if (Enabled && !UserCorrect ) return fail(Properties.Resources.ErrorUserPassword);
        return true;
    }

    #endregion

    #region Implementations

    /* --------------------------------------------------------------------- */
    ///
    /// GetPermission
    ///
    /// <summary>
    /// Gets the PermissionValue object corresponding to the specified
    /// value.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private PermissionValue GetPermission(bool src) =>
        src ? PermissionValue.Allow : PermissionValue.Deny;

    #endregion
}
