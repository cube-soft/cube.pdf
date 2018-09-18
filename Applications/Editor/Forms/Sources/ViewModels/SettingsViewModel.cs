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
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Cube.Pdf.App.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// SettingsViewModel
    ///
    /// <summary>
    /// Represents the ViewModel for a <c>SettingsWindow</c> instance.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class SettingsViewModel : DialogViewModel
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// SettingsViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the <c>SettingsViewModel</c>
        /// with the specified argumetns.
        /// </summary>
        ///
        /// <param name="src">User settings.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public SettingsViewModel(SettingsFolder src, SynchronizationContext context) :
            base(() => Properties.Resources.TitleSettings, new Messenger(), context)
        {
            var asm = Assembly.GetExecutingAssembly().GetReader();

            Language = this.Create(() => src.Value.Language,    e  => src.Value.Language    = e, () => Properties.Resources.MenuLanguage);
            Update   = this.Create(() => src.Value.CheckUpdate, e  => src.Value.CheckUpdate = e, () => Properties.Resources.MenuUpdate  );
            Version  = this.Create(() => $"{src.Title} {src.Version.ToString(true)}", () => Properties.Resources.MenuVersion);
            Link     = this.Create(() => asm.Copyright, () => src.Value.Uri.ToString());

            OK.Command = new RelayCommand(() =>
            {
                Send<UpdateSourcesMessage>();
                Send<CloseMessage>();
            });
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Version
        ///
        /// <summary>
        /// Gets the version menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<string> Version { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Link
        ///
        /// <summary>
        /// Gets the link menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<string> Link { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Language
        ///
        /// <summary>
        /// Gets the language menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<Language> Language { get; }

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
        public BindableElement<bool> Update { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Windows
        ///
        /// <summary>
        /// Gets the menu of Windows version.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement Windows { get; } =
            new BindableElement(() => $"{Environment.OSVersion}");

        /* ----------------------------------------------------------------- */
        ///
        /// Framework
        ///
        /// <summary>
        /// Gets the menu of .NET Framework version.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement Framework { get; } =
            new BindableElement(() => $"Microsoft .NET Framework {Environment.Version}");

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
            Editor.Language.Auto,
            Editor.Language.English,
            Editor.Language.Japanese,
        };

        #endregion
    }
}
