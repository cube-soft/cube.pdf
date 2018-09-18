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
using Cube.FileSystem;
using Cube.Xui;

namespace Cube.Pdf.App.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// MainBindable
    ///
    /// <summary>
    /// Provides values for binding to the MainWindow.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class MainBindable
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// MainBindable
        ///
        /// <summary>
        /// Initializes a new instance of the <c>MainBindable</c> class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="images">Image collection.</param>
        /// <param name="settings">Settings object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public MainBindable(ImageCollection images, SettingsFolder settings)
        {
            _settings = settings;
            Images    = images;
            Count     = new Bindable<int>(() => Images.Count);
            ItemSize  = new Bindable<int>(
                () => Settings.ItemSize,
                e =>
                {
                    if (Settings.ItemSize == e) return false;
                    Settings.ItemSize = e;
                    return true;
                }
            );
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Images
        ///
        /// <summary>
        /// Gets an image collection of PDF documents.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ImageCollection Images { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Selection
        ///
        /// <summary>
        /// Gets the selection of thumbnails.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ImageSelection Selection => Images.Selection;

        /* ----------------------------------------------------------------- */
        ///
        /// Preferences
        ///
        /// <summary>
        /// Gets the preferences for thumbnails.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ImagePreferences Preferences => Images.Preferences;

        /* ----------------------------------------------------------------- */
        ///
        /// Settings
        ///
        /// <summary>
        /// Gets an application settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Settings Settings => _settings.Value;

        /* ----------------------------------------------------------------- */
        ///
        /// History
        ///
        /// <summary>
        /// Gets a history to execute the undo and redo actions.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public History History { get; } = new History();

        /* ----------------------------------------------------------------- */
        ///
        /// Source
        ///
        /// <summary>
        /// Gets a file path of the PDF file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Bindable<Information> Source { get; } = new Bindable<Information>();

        /* ----------------------------------------------------------------- */
        ///
        /// Metadata
        ///
        /// <summary>
        /// Gets the metadata of the PDF file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Bindable<Metadata> Metadata { get; } = new Bindable<Metadata>();

        /* ----------------------------------------------------------------- */
        ///
        /// Source
        ///
        /// <summary>
        /// Gets the encryption information of the PDF file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Bindable<Encryption> Encryption { get; } = new Bindable<Encryption>();

        /* ----------------------------------------------------------------- */
        ///
        /// Count
        ///
        /// <summary>
        /// Gets the number of pages in the PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Bindable<int> Count { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// ItemSize
        ///
        /// <summary>
        /// Gets the displayed item size.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Bindable<int> ItemSize { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Busy
        ///
        /// <summary>
        /// Gets a value indicating whether models are busy.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Bindable<bool> Busy { get; } = new Bindable<bool>(false);

        /* ----------------------------------------------------------------- */
        ///
        /// Message
        ///
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Bindable<string> Message { get; } = new Bindable<string>(string.Empty);

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// IsOpen
        ///
        /// <summary>
        /// Gets the value indicatint whether a PDF document is open.
        /// </summary>
        ///
        /// <returns>true for open.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public bool IsOpen() => Source.Value != null;

        /* ----------------------------------------------------------------- */
        ///
        /// Open
        ///
        /// <summary>
        /// Sets properties of the specified <c>IDocumentReader</c>.
        /// </summary>
        ///
        /// <param name="src">Document information.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Open(IDocumentReader src)
        {
            Source.Value     = src.File;
            Metadata.Value   = src.Metadata;
            Encryption.Value = src.Encryption;

            Images.Add(src.Pages);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Close
        ///
        /// <summary>
        /// Clears properties of the current PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Close()
        {
            Source.Value     = null;
            Metadata.Value   = null;
            Encryption.Value = null;

            History.Clear();
            Images.Clear();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetMessage
        ///
        /// <summary>
        /// Sets the specified message.
        /// </summary>
        ///
        /// <param name="format">Format for the message.</param>
        /// <param name="args">Additional arguments.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void SetMessage(string format, params object[] args) =>
            Message.Value = string.Format(format, args);

        #endregion

        #region Fields
        private SettingsFolder _settings;
        #endregion
    }
}
