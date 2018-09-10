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
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Cube.Pdf.App.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// MetadataViewModel
    ///
    /// <summary>
    /// Represents the ViewModel for a <c>MetadataWindow</c> instance.
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
        /// Initializes a new instance of the <c>MetadataViewModel</c>
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
            base(() => Properties.Resources.TitleMetadata, new Messenger(), context)
        {
            Filename      = Create(() => file.Name,          () => Properties.Resources.MenuFilename     );
            Producer      = Create(() => src.Producer,       () => Properties.Resources.MenuProducer     );
            Length        = Create(() => file.Length,        () => Properties.Resources.MenuFilesize     );
            CreationTime  = Create(() => file.CreationTime,  () => Properties.Resources.MenuCreationTime );
            LastWriteTime = Create(() => file.LastWriteTime, () => Properties.Resources.MenuLastWriteTime);

            Document = Create(() => src.Title,    e => src.Title    = e, () => Properties.Resources.MenuTitle   );
            Author   = Create(() => src.Author,   e => src.Author   = e, () => Properties.Resources.MenuAuthor  );
            Subject  = Create(() => src.Subject,  e => src.Subject  = e, () => Properties.Resources.MenuSubject );
            Keywords = Create(() => src.Keywords, e => src.Keywords = e, () => Properties.Resources.MenuKeywords);
            Creator  = Create(() => src.Creator,  e => src.Creator  = e, () => Properties.Resources.MenuCreator );
            Version  = Create(() => src.Version,  e => src.Version  = e, () => Properties.Resources.MenuVersion );
            Layout   = CreateLayout(src);

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
        public BindableElement<Version> Version { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Layout
        ///
        /// <summary>
        /// Gets the menu that represents the page layout (a.k.a viewer
        /// preferences) of the PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<DisplayOptions> Layout { get; }

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
        public BindableElement Summary { get; } = new BindableElement(
            () => Properties.Resources.MenuSummary
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Details
        ///
        /// <summary>
        /// Gets the menu that represents the details group.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement Details { get; } = new BindableElement(
            () => Properties.Resources.MenuDetails
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Versions
        ///
        /// <summary>
        /// Gets the collection of PDF version numbers.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<Version> Versions { get; } = new[]
        {
            new Version(1, 7),
            new Version(1, 6),
            new Version(1, 5),
            new Version(1, 4),
            new Version(1, 3),
            new Version(1, 2),
        };

        /* ----------------------------------------------------------------- */
        ///
        /// Layouts
        ///
        /// <summary>
        /// Gets the collection of display options.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<DisplayOptions> Layouts { get; } = new[]
        {
            DisplayOptions.SinglePage,
            DisplayOptions.OneColumn,
            DisplayOptions.TwoColumnLeft,
            DisplayOptions.TwoColumnRight,
            DisplayOptions.TwoPageLeft,
            DisplayOptions.TwoPageRight,
        };

        #endregion

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Creates a new menu with the specified arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static BindableElement<T> Create<T>(Getter<T> getter, Getter<string> gettext) =>
            new BindableElement<T>(getter, gettext);

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Creates a new menu with the specified arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static BindableElement<T> Create<T>(Getter<T> getter, Action<T> setter, Getter<string> gettext) =>
            new BindableElement<T>(getter, e => { setter(e); return true; }, gettext);

        /* ----------------------------------------------------------------- */
        ///
        /// CreateLayout
        ///
        /// <summary>
        /// Creates a new menu for the DisplayOptions property.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static BindableElement<DisplayOptions> CreateLayout(Metadata src)
        {
            if (src.DisplayOptions == DisplayOptions.None) src.DisplayOptions = DisplayOptions.OneColumn;
            return Create(() => src.DisplayOptions, e => src.DisplayOptions = e, () => Properties.Resources.MenuLayout);
        }

        #endregion
    }
}
