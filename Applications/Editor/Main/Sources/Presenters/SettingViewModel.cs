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
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Cube.Globalization;
using Cube.Reflection.Extensions;
using Cube.Xui;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// SettingViewModel
    ///
    /// <summary>
    /// Represents the ViewModel for a SettingWindow instance.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class SettingViewModel : DialogViewModel<SettingFolder>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// SettingViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the SettingViewModel
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="src">User settings.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public SettingViewModel(SettingFolder src, SynchronizationContext context) :
            base(src, new(), context)
        {
            ShrinkResources.Value = Facade.Value.ShrinkResources;
            KeepOutlines.Value    = Facade.Value.KeepOutlines;
            BackupEnabled.Value   = Facade.Value.BackupEnabled;
            Backup.Value          = Facade.Value.Backup;
            Temp.Value            = Facade.Value.Temp;
            Language.Value        = Facade.Value.Language;
            RecentVisible.Value   = Facade.Value.RecentVisible;
            CheckUpdate.Value     = Facade.Startup.Enabled;

            OK.Command = new DelegateCommand(() => Quit(Apply, true));
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// ShrinkResources
        ///
        /// <summary>
        /// Gets the menu indicating whether to shrink deduplicated resources
        /// when saving PDF files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IElement<bool> ShrinkResources => Get(() => new BindableElement<bool>(
            () => Surface.Texts.Setting_Shrink,
            GetDispatcher(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// KeepOutlines
        ///
        /// <summary>
        /// Gets the menu indicating whether to keep outline information
        /// when saving PDF files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IElement<bool> KeepOutlines => Get(() => new BindableElement<bool>(
            () => Surface.Texts.Setting_KeepOutline,
            GetDispatcher(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// RecentVisible
        ///
        /// <summary>
        /// Get the menu indicating whether to display the recently used
        /// files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IElement<bool> RecentVisible => Get(() => new BindableElement<bool>(
            () => Surface.Texts.Setting_Recent,
            GetDispatcher(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// CheckUpdate
        ///
        /// <summary>
        /// Gets the menu indicating whether to check for software updates
        /// at startup.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IElement<bool> CheckUpdate => Get(() => new BindableElement<bool>(
            () => Surface.Texts.Setting_CheckUpdate,
            GetDispatcher(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// BackupEnabled
        ///
        /// <summary>
        /// Gets the menu indicating whether to enable the backup function.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IElement<bool> BackupEnabled => Get(() => new BindableElement<bool>(
            () => Surface.Texts.Setting_Backup_Enable,
            GetDispatcher(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// Backup
        ///
        /// <summary>
        /// Gets the backup directory menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IElement<string> Backup => Get(() => new BindableElement<string>(
            () => Surface.Texts.Setting_Backup,
            new DelegateCommand(
                () => Send(Message.ForBackup(Backup.Value),
                e => Backup.Value = e, true
            )),
            GetDispatcher(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// Temp
        ///
        /// <summary>
        /// Gets the temp directory menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IElement<string> Temp => Get(() => new BindableElement<string>(
            () => Surface.Texts.Setting_Temp,
            new DelegateCommand(
                () => Send(Message.ForTemp(Temp.Value),
                e => Temp.Value = e, true
            )),
            GetDispatcher(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// Language
        ///
        /// <summary>
        /// Gets the language menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IElement<Language> Language => Get(() => new BindableElement<Language>(
            () => Surface.Texts.Setting_Language,
            GetDispatcher(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// Languages
        ///
        /// <summary>
        /// Gets the collection of supported languages.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<Language> Languages { get; } = new[]
        {
            Globalization.Language.Auto,
            Globalization.Language.English,
            Globalization.Language.German,
            Globalization.Language.Japanese,
            Globalization.Language.SimplifiedChinese,
        };

        /* ----------------------------------------------------------------- */
        ///
        /// Version
        ///
        /// <summary>
        /// Gets the version tab/menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IElement<string> Version => Get(() => new BindableElement<string>(
            () => Surface.Texts.Setting_Version,
            () => $"{Facade.Title} {Facade.Version.ToString(3, true)}",
            GetDispatcher(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// Link
        ///
        /// <summary>
        /// Gets the link menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IElement<Uri> Link => Get(() => new BindableElement<Uri>(
            () => Assembly.GetExecutingAssembly().GetCopyright(),
            () => Facade.ProductUri,
            new DelegateCommand(() => Post(new ProcessMessage(Link.Value.ToString()))),
            GetDispatcher(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// Windows
        ///
        /// <summary>
        /// Gets the menu of Windows version.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IElement Windows => Get(() => new BindableElement(
            () => $"{Environment.OSVersion}",
            GetDispatcher(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// Framework
        ///
        /// <summary>
        /// Gets the menu of .NET Framework version.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IElement Framework => Get(() => new BindableElement(
            () => $"Microsoft .NET Framework {Environment.Version}",
            GetDispatcher(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// Summary
        ///
        /// <summary>
        /// Gets the summary tab.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IElement Summary => Get(() => new BindableElement(
            () => Surface.Texts.Setting_Tab,
            GetDispatcher(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// SaveOptions
        ///
        /// <summary>
        /// Gets the menu of save options.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IElement SaveOptions => Get(() => new BindableElement(
            () => Surface.Texts.Setting_Options,
            GetDispatcher(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// OtherOptions
        ///
        /// <summary>
        /// Gets the menu of other options.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IElement OtherOptions => Get(() => new BindableElement(
            () => Surface.Texts.Setting_Others,
            GetDispatcher(false)
        ));

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetTitle
        ///
        /// <summary>
        /// Gets the title of the dialog.
        /// </summary>
        ///
        /// <returns>String value.</returns>
        ///
        /* ----------------------------------------------------------------- */
        protected override string GetTitle() => Surface.Texts.Setting_Window;

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Invokes the apply command.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Apply()
        {
            Facade.Value.ShrinkResources = ShrinkResources.Value;
            Facade.Value.KeepOutlines    = KeepOutlines.Value;
            Facade.Value.BackupEnabled   = BackupEnabled.Value;
            Facade.Value.Backup          = Backup.Value;
            Facade.Value.Temp            = Temp.Value;
            Facade.Value.Language        = Language.Value;
            Facade.Value.RecentVisible   = RecentVisible.Value;
            Facade.Startup.Enabled       = CheckUpdate.Value;
        }

        #endregion
    }
}
