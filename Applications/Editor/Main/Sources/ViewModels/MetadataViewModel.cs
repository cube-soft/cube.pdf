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
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// MetadataViewModel
    ///
    /// <summary>
    /// Represents the ViewModel for a MetadataWindow instance.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class MetadataViewModel : DialogViewModel
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// MetadataViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the MetadataViewModel
        /// with the specified argumetns.
        /// </summary>
        ///
        /// <param name="callback">Callback method when applied.</param>
        /// <param name="src">Metadata object.</param>
        /// <param name="file">File information.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public MetadataViewModel(Action<Metadata> callback, Metadata src, Information file, SynchronizationContext context) :
            base(() => Properties.Resources.TitleMetadata, new Aggregator(), context)
        {
            Filename      = this.Create(() => file.Name, () => Properties.Resources.MenuFilename, GetDispatcher(false));
            Producer      = this.Create(() => src.Producer, () => Properties.Resources.MenuProducer, GetDispatcher(false));
            Length        = this.Create(() => file.Length, () => Properties.Resources.MenuFilesize, GetDispatcher(false));
            CreationTime  = this.Create(() => file.CreationTime, () => Properties.Resources.MenuCreationTime, GetDispatcher(false));
            LastWriteTime = this.Create(() => file.LastWriteTime, () => Properties.Resources.MenuLastWriteTime, GetDispatcher(false));

            Document = this.Create(() => src.Title, e => src.Title = e, () => Properties.Resources.MenuTitle, GetDispatcher(false));
            Author   = this.Create(() => src.Author, e => src.Author = e, () => Properties.Resources.MenuAuthor, GetDispatcher(false));
            Subject  = this.Create(() => src.Subject, e => src.Subject = e, () => Properties.Resources.MenuSubject, GetDispatcher(false));
            Keywords = this.Create(() => src.Keywords, e => src.Keywords = e, () => Properties.Resources.MenuKeywords, GetDispatcher(false));
            Creator  = this.Create(() => src.Creator, e => src.Creator = e, () => Properties.Resources.MenuCreator, GetDispatcher(false));
            Version  = CreateVersion(src);
            Options  = CreateViewerOptions(src);

            Summary = new BindableElement(() => Properties.Resources.MenuSummary, GetDispatcher(false));
            Details = new BindableElement(() => Properties.Resources.MenuDetails, GetDispatcher(false));

            OK.Command = new RelayCommand(() => { Send<CloseMessage>(); callback(src); });
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Filename
        ///
        /// <summary>
        /// Gets the menu that represents the filename.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<string> Filename { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Document
        ///
        /// <summary>
        /// Gets the menu that represents the title of the specified
        /// PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<string> Document { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Author
        ///
        /// <summary>
        /// Gets the menu that represents the author of the specified
        /// PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<string> Author { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Subject
        ///
        /// <summary>
        /// Gets the menu that represents the subject of the specified
        /// PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<string> Subject { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Keywords
        ///
        /// <summary>
        /// Gets the menu that represents keywords of the specified
        /// PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<string> Keywords { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Version
        ///
        /// <summary>
        /// Gets the menu that represents the PDF version of the specified
        /// document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<PdfVersion> Version { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// ViewerOptions
        ///
        /// <summary>
        /// Gets the menu that represents the viewer options of the
        /// PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<ViewerOptions> Options { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Creator
        ///
        /// <summary>
        /// Gets the menu that represents the creation program of the
        /// specified PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<string> Creator { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Producer
        ///
        /// <summary>
        /// Gets the menu that represents the generating program of the
        /// specified PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<string> Producer { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Length
        ///
        /// <summary>
        /// Gets the menu that represents the length of the specified file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<long> Length { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// CreationTime
        ///
        /// <summary>
        /// Gets the menu that represents the creation time of the
        /// specified file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<DateTime> CreationTime { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// LastWriteTime
        ///
        /// <summary>
        /// Gets the menu that represents the last updated time of the
        /// specified file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<DateTime> LastWriteTime { get; }

        #region Texts

        /* ----------------------------------------------------------------- */
        ///
        /// Summary
        ///
        /// <summary>
        /// Gets the menu that represents the summary group.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement Summary { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Details
        ///
        /// <summary>
        /// Gets the menu that represents the details group.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement Details { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Versions
        ///
        /// <summary>
        /// Gets the collection of PDF version numbers.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<PdfVersion> Versions { get; } = new[]
        {
            new PdfVersion(1, 7),
            new PdfVersion(1, 6),
            new PdfVersion(1, 5),
            new PdfVersion(1, 4),
            new PdfVersion(1, 3),
            new PdfVersion(1, 2),
        };

        /* ----------------------------------------------------------------- */
        ///
        /// ViewerPreferences
        ///
        /// <summary>
        /// Gets the collection of viewer preferences.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<ViewerOptions> ViewerOptions { get; } = new[]
        {
            Pdf.ViewerOptions.SinglePage,
            Pdf.ViewerOptions.OneColumn,
            Pdf.ViewerOptions.TwoColumnLeft,
            Pdf.ViewerOptions.TwoColumnRight,
            Pdf.ViewerOptions.TwoPageLeft,
            Pdf.ViewerOptions.TwoPageRight,
        };

        #endregion

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// CreateVersion
        ///
        /// <summary>
        /// Creates a new menu for the Version property.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private BindableElement<PdfVersion> CreateVersion(Metadata src)
        {
            src.Version = Versions.FirstOrDefault(e => e.Minor == src.Version.Minor) ??
                          Versions.First();
            return this.Create(() => src.Version, e => src.Version = e, () => Properties.Resources.MenuVersion, GetDispatcher(false));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CreateViewerPreferences
        ///
        /// <summary>
        /// Creates a new menu for the ViewerPreferences property.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private BindableElement<ViewerOptions> CreateViewerOptions(Metadata src)
        {
            if (src.Options == Pdf.ViewerOptions.None) src.Options = Pdf.ViewerOptions.OneColumn;
            return this.Create(() => src.Options, e => src.Options = e, () => Properties.Resources.MenuLayout, GetDispatcher(false));
        }

        #endregion
    }
}
