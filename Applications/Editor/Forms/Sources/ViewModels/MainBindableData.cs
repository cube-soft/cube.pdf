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

namespace Cube.Pdf.App.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// MainBindableData
    ///
    /// <summary>
    /// Provides values for binding to the MainWindow.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class MainBindableData
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// MainBindableData
        ///
        /// <summary>
        /// Initializes a new instance with the specified parameters.
        /// </summary>
        ///
        /// <param name="images">Image collection.</param>
        /// <param name="settings">Settings object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public MainBindableData(ImageList images, SettingsFolder settings)
        {
            Images    = images;
            _settings = settings;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Images
        ///
        /// <summary>
        /// Gets the images of the PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ImageList Images { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Settings
        ///
        /// <summary>
        /// Gets the settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Settings Settings => _settings.Value;

        /* ----------------------------------------------------------------- */
        ///
        /// IsOpen
        ///
        /// <summary>
        /// Gets a value that determines whether a PDF document is open.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Bindable<bool> IsOpen { get; } = new Bindable<bool>(false);

        /* ----------------------------------------------------------------- */
        ///
        /// IsBusy
        ///
        /// <summary>
        /// Gets a value that determines whether models are busy.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Bindable<bool> IsBusy { get; } = new Bindable<bool>(false);

        /* ----------------------------------------------------------------- */
        ///
        /// Message
        ///
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Bindable<string> Message { get; } = new Bindable<string>("Ready");

        #endregion

        #region Fields
        private SettingsFolder _settings;
        #endregion
    }
}
