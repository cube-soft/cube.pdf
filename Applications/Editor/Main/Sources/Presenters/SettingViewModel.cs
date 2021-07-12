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
using Cube.Mixin.Assembly;
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
            OK.Command = new DelegateCommand(() =>
            {
                Send<ApplyMessage>();
                Send<CloseMessage>();
            });
        }

        #endregion

        #region Properties

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
            Cube.Language.Auto,
            Cube.Language.English,
            Cube.Language.Japanese,
        };

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
            () => Properties.Resources.MenuLanguage,
            () => Facade.Value.Language,
            e  => Facade.Value.Language = e,
            GetDispatcher(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// Version
        ///
        /// <summary>
        /// Gets the version menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IElement<string> Version => Get(() => new BindableElement<string>(
            () => Properties.Resources.MenuVersion,
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
            () => Facade.Value.Uri,
            GetDispatcher(false)
        ) { Command = new DelegateCommand(() => Post(Link.Value)) });

        /* ----------------------------------------------------------------- */
        ///
        /// Update
        ///
        /// <summary>
        /// Gets the menu indicating whether checking software update
        /// at launching process.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IElement<bool> Update => Get(() => new BindableElement<bool>(
            () => Properties.Resources.MenuUpdate,
            () => Facade.Startup.Enabled,
            e  => Facade.Startup.Enabled = e,
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
        protected override string GetTitle() => Properties.Resources.TitleSetting;

        #endregion
    }
}
