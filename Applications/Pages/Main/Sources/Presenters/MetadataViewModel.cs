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
using System.Threading;

namespace Cube.Pdf.Pages
{
    /* --------------------------------------------------------------------- */
    ///
    /// MetadataViewModel
    ///
    /// <summary>
    /// Represents the ViewModel for the MetadataWindow.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class MetadataViewModel : PresentableBase<Metadata>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// MetadataViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the MetadataViewModel class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Source metadata information.</param>
        /// <param name="encryption">Source encryption information.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public MetadataViewModel(Metadata src, Encryption encryption, SynchronizationContext context) :
            base(src, new(), context)
        {
            Encryption = new(encryption, Aggregator, context);
            Title      = src.Title;
            Author     = src.Author;
            Subject    = src.Subject;
            Keywords   = src.Keywords;
            Creator    = src.Creator;
            Version    = src.Version.Minor;
            Options    = src.Options;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Encryption
        ///
        /// <summary>
        /// Gets the ViewModel object for encryption settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public EncryptionViewModel Encryption { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Title
        ///
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Title
        {
            get => Get(() => string.Empty);
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Author
        ///
        /// <summary>
        /// Gets or sets the author.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Author
        {
            get => Get(() => string.Empty);
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Subject
        ///
        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Subject
        {
            get => Get(() => string.Empty);
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Keywords
        ///
        /// <summary>
        /// Gets or sets the keywords.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Keywords
        {
            get => Get(() => string.Empty);
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Creator
        ///
        /// <summary>
        /// Gets or sets the name of creator program.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Creator
        {
            get => Get(() => string.Empty);
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Version
        ///
        /// <summary>
        /// Gets or sets the PDF version.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Version
        {
            get => Get(() => 7);
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Options
        ///
        /// <summary>
        /// Gets or sets the view options.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ViewerOption Options
        {
            get => Get(() => ViewerOption.None);
            set => Set(value);
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Apply
        ///
        /// <summary>
        /// Apply the user settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Apply()
        {
            if (Encryption.Apply()) Quit(() => {
                Facade.Title    = Title;
                Facade.Author   = Author;
                Facade.Subject  = Subject;
                Facade.Keywords = Keywords;
                Facade.Creator  = Creator;
                Facade.Version  = new(1, Version);
                Facade.Options  = Options;
            }, true);
        }

        #endregion
    }
}
