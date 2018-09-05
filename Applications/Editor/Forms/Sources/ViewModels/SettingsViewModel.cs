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
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public SettingsViewModel(SynchronizationContext context) :
            base(() => Properties.Resources.TitleSettings, new Messenger(), context) { }

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
        public BindableElement Version { get; } = new BindableElement(
            () => Properties.Resources.MenuVersion
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Language
        ///
        /// <summary>
        /// Gets the language menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement Language { get; } = new BindableElement(
            () => Properties.Resources.MenuLanguage
        );

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
        public BindableElement Update { get; } = new BindableElement(
            () => Properties.Resources.MenuUpdate
        );

        #endregion
    }
}
