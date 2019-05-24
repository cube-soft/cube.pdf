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
using Cube.Mixin.String;
using System.Linq;
using System.Threading;

namespace Cube.Pdf.Converter
{
    /* --------------------------------------------------------------------- */
    ///
    /// MetadataViewModel
    ///
    /// <summary>
    /// Represents the viewmodel for the metadata tab in the main window.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class MetadataViewModel : CommonViewModel
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// MetadataViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the MetadataViewModel class with
        /// the specified arguments.
        /// </summary>
        ///
        /// <param name="model">PDF metadata.</param>
        /// <param name="aggregator">Event aggregator.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public MetadataViewModel(Metadata model, Aggregator aggregator,
            SynchronizationContext context) : base(aggregator, context)
        {
            _model = model;
            _model.PropertyChanged += (s, e) => OnPropertyChanged(e);
        }

        #endregion

        #region Properties

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
            get => _model.Title;
            set => _model.Title = value;
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
            get => _model.Author;
            set => _model.Author = value;
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
            get => _model.Subject;
            set => _model.Subject = value;
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
            get => _model.Keywords;
            set => _model.Keywords = value;
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
            get => _model.Creator;
            set => _model.Creator = value;
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
            get => _model.Version.Minor;
            set => _model.Version = new PdfVersion(1, value);
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
        public ViewerOptions Options
        {
            get => _model.Options;
            set => _model.Options = value;
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// ConfirmForSave
        ///
        /// <summary>
        /// Confirms if the current settings are acceptable.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool ConfirmForSave()
        {
            var src = new[] { Title, Author, Subject, Keywords };
            if (src.All(e => !e.HasValue())) return true;
            else return Confirm(MessageFactory.CreateWarn(Properties.Resources.MessageSave));
        }

        #endregion

        #region Fields
        private readonly Metadata _model;
        #endregion
    }
}
