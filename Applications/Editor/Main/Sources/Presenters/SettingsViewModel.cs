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
using Cube.Mixin.Assembly;
using Cube.Xui;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// SettingsViewModel
    ///
    /// <summary>
    /// Represents the ViewModel for a SettingsWindow instance.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class SettingsViewModel : DialogViewModel
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// SettingsViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the SettingsViewModel
        /// with the specified argumetns.
        /// </summary>
        ///
        /// <param name="src">User settings.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public SettingsViewModel(SettingsFolder src, SynchronizationContext context) :
            base(() => Properties.Resources.TitleSettings, new Aggregator(), context)
        {
            _model = src;
            OK.Command = new DelegateCommand(() =>
            {
                Send<UpdateSourcesMessage>();
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
            () => _model.Value.Language,
            e  => _model.Value.Language = e,
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
            () => $"{_model.Title} {_model.Version.ToString(true)}",
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
            () => _model.Value.Uri,
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
            () => _model.Value.CheckUpdate,
            e  => _model.Value.CheckUpdate = e,
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

        #region Fields
        private readonly SettingsFolder _model;
        #endregion
    }
}
