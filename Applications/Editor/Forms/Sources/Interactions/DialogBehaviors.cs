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
using Cube.Xui.Behaviors;

namespace Cube.Pdf.App.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// PreviewWindowBehavior
    ///
    /// <summary>
    /// Represents the behavior to show a <c>PreviewWindow</c> dialog.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class PreviewWindowBehavior :
        ShowDialogBehavior<PreviewWindow, PreviewViewModel> { }

    /* --------------------------------------------------------------------- */
    ///
    /// InsertWindowBehavior
    ///
    /// <summary>
    /// Represents the behavior to show a <c>InsertWindow</c> dialog.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class InsertWindowBehavior :
        ShowDialogBehavior<InsertWindow, InsertViewModel> { }

    /* --------------------------------------------------------------------- */
    ///
    /// ExtractWindowBehavior
    ///
    /// <summary>
    /// Represents the behavior to show a <c>ExtractWindow</c> dialog.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ExtractWindowBehavior :
        ShowDialogBehavior<ExtractWindow, ExtractViewModel> { }

    /* --------------------------------------------------------------------- */
    ///
    /// RemoveWindowBehavior
    ///
    /// <summary>
    /// Represents the behavior to show a <c>RemoveWindow</c> dialog.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class RemoveWindowBehavior :
        ShowDialogBehavior<RemoveWindow, RemoveViewModel> { }

    /* --------------------------------------------------------------------- */
    ///
    /// MetadataWindowBehavior
    ///
    /// <summary>
    /// Represents the behavior to show a <c>MetadataWindow</c> dialog.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class MetadataWindowBehavior :
        ShowDialogBehavior<MetadataWindow, MetadataViewModel> { }

    /* --------------------------------------------------------------------- */
    ///
    /// EncryptionWindowBehavior
    ///
    /// <summary>
    /// Represents the behavior to show a <c>EncryptionWindow</c> dialog.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class EncryptionWindowBehavior :
        ShowDialogBehavior<EncryptionWindow, EncryptionViewModel> { }

    /* --------------------------------------------------------------------- */
    ///
    /// SettingsWindowBehavior
    ///
    /// <summary>
    /// Represents the behavior to show a <c>SettingsWindow</c> dialog.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class SettingsWindowBehavior :
        ShowDialogBehavior<SettingsWindow, SettingsViewModel> { }
}
