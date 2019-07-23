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
using System.Windows;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// ShowPasswordWindow
    ///
    /// <summary>
    /// Represents the behavior to show a PasswordWindow dialog.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ShowPasswordWindow : ShowBehavior<PasswordWindow, PasswordViewModel> { }

    /* --------------------------------------------------------------------- */
    ///
    /// ShowPreviewWindow
    ///
    /// <summary>
    /// Represents the behavior to show a PreviewWindow dialog.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ShowPreviewWindow : ShowBehavior<PreviewWindow, PreviewViewModel> { }

    /* --------------------------------------------------------------------- */
    ///
    /// ShowInsertWindow
    ///
    /// <summary>
    /// Represents the behavior to show a InsertWindow dialog.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ShowInsertWindow : ShowBehavior<InsertWindow, InsertViewModel> { }

    /* --------------------------------------------------------------------- */
    ///
    /// ShowRemoveWindow
    ///
    /// <summary>
    /// Represents the behavior to show a RemoveWindow dialog.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ShowRemoveWindow : ShowBehavior<RemoveWindow, RemoveViewModel> { }

    /* --------------------------------------------------------------------- */
    ///
    /// ShowMetadataWindow
    ///
    /// <summary>
    /// Represents the behavior to show a MetadataWindow dialog.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ShowMetadataWindow : ShowBehavior<MetadataWindow, MetadataViewModel> { }

    /* --------------------------------------------------------------------- */
    ///
    /// ShowEncryptionWindow
    ///
    /// <summary>
    /// Represents the behavior to show a EncryptionWindow dialog.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ShowEncryptionWindow : ShowBehavior<EncryptionWindow, EncryptionViewModel> { }

    /* --------------------------------------------------------------------- */
    ///
    /// ShowSettingWindow
    ///
    /// <summary>
    /// Represents the behavior to show a SettingWindow dialog.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ShowSettingWindow : ShowBehavior<SettingWindow, SettingViewModel> { }

    /* --------------------------------------------------------------------- */
    ///
    /// MouseOpenBehavior
    ///
    /// <summary>
    /// Represents the behavior when files are dropped.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class MouseOpenBehavior : FileDropToCommand<Window> { }
}