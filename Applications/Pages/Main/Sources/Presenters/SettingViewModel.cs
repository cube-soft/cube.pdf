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
using System;
using System.Threading;
using Cube.Globalization;

namespace Cube.Pdf.Pages
{
    /* --------------------------------------------------------------------- */
    ///
    /// SettingViewModel
    ///
    /// <summary>
    /// Represents the ViewModel for the SettingWindow.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class SettingViewModel : PresentableBase<SettingFolder>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// SettingViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the SettingViewModel class with
        /// the specified arguments.
        /// </summary>
        ///
        /// <param name="src">User settings.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public SettingViewModel(SettingFolder src, SynchronizationContext context) :
            base(src, new(), context)
        {
            Language        = src.Value.Language;
            Temp            = src.Value.Temp;
            ShrinkResources = src.Value.ShrinkResources;
            KeepOutlines    = src.Value.KeepOutlines;
            CheckUpdate     = src.Startup.Enabled;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Language
        ///
        /// <summary>
        /// Gets or sets the displayed language.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Language Language
        {
            get => Get(() => Language.Auto);
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Temp
        ///
        /// <summary>
        /// Gets or sets the path of the working directory. If this value is
        /// empty, the program will use the same directory as the destination
        /// path as its working directory.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Temp
        {
            get => Get(() => string.Empty);
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ShrinkResources
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to shrink deduplicated
        /// resources when saving PDF files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool ShrinkResources
        {
            get => Get(() => true);
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// KeepOutlines
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to keep outline
        /// information when saving PDF files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool KeepOutlines
        {
            get => Get(() => true);
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CheckUpdate
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to check the update
        /// of the application.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool CheckUpdate
        {
            get => Get(() => false);
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Version
        ///
        /// <summary>
        /// Gets the version string.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Version => Facade.Version.ToString(3, true);

        /* ----------------------------------------------------------------- */
        ///
        /// Uri
        ///
        /// <summary>
        /// Gets the product URL.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Uri Uri => Surface.ProductUri;

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// SelectTemp
        ///
        /// <summary>
        /// Shows the open directory dialog to select the temp directory.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void SelectTemp() => Send(Message.ForTemp(), e => Temp = e, true);

        /* ----------------------------------------------------------------- */
        ///
        /// Apply
        ///
        /// <summary>
        /// Apply the user settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Apply() => Quit(() =>
        {
            Facade.Startup.Enabled       = CheckUpdate;
            Facade.Value.Language        = Language;
            Facade.Value.Temp            = Temp;
            Facade.Value.ShrinkResources = ShrinkResources;
            Facade.Value.KeepOutlines    = KeepOutlines;
            Facade.Save();
        }, true);

        #endregion
    }
}
