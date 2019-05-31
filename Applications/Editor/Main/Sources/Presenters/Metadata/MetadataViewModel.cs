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
using System;
using System.Collections.Generic;
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
        public MetadataViewModel(Action<Metadata> callback,
            Metadata src,
            Information file,
            SynchronizationContext context
        ) : base(() => Properties.Resources.TitleMetadata, new Aggregator(), context)
        {
            _model = new MetadataFacade(src, file);
            OK.Command = new BindableCommand(() => { Send<CloseMessage>(); callback(src); });
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Versions
        ///
        /// <summary>
        /// Gets the collection of PDF version numbers.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<PdfVersion> Versions => MetadataFacade.Versions;

        /* ----------------------------------------------------------------- */
        ///
        /// ViewerPreferences
        ///
        /// <summary>
        /// Gets the collection of viewer preferences.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<ViewerOptions> ViewerOptions => MetadataFacade.ViewerOptions;

        /* ----------------------------------------------------------------- */
        ///
        /// Filename
        ///
        /// <summary>
        /// Gets the menu that represents the filename.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<string> Filename => Get(() => new BindableElement<string>(
            () => Properties.Resources.MenuFilename,
            () => _model.File.Name,
            GetDispatcher(false)
        ));

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
        public BindableElement<string> Document => Get(() => new BindableElement<string>(
            () => Properties.Resources.MenuTitle,
            () => _model.Value.Title,
            e  => _model.Value.Title = e,
            GetDispatcher(false)
        ));

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
        public BindableElement<string> Author => Get(() => new BindableElement<string>(
            () => Properties.Resources.MenuAuthor,
            () => _model.Value.Author,
            e  => _model.Value.Author = e,
            GetDispatcher(false)
        ));

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
        public BindableElement<string> Subject => Get(() => new BindableElement<string>(
            () => Properties.Resources.MenuSubject,
            () => _model.Value.Subject,
            e  => _model.Value.Subject = e,
            GetDispatcher(false)
        ));

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
        public BindableElement<string> Keywords => Get(() => new BindableElement<string>(
            () => Properties.Resources.MenuKeywords,
            () => _model.Value.Keywords,
            e  => _model.Value.Keywords = e,
            GetDispatcher(false)
        ));

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
        public BindableElement<string> Creator => Get(() => new BindableElement<string>(
            () => Properties.Resources.MenuCreator,
            () => _model.Value.Creator,
            e  => _model.Value.Creator = e,
            GetDispatcher(false)
        ));

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
        public BindableElement<string> Producer => Get(() => new BindableElement<string>(
            () => Properties.Resources.MenuProducer,
            () => _model.Value.Producer,
            GetDispatcher(false)
        ));

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
        public BindableElement<PdfVersion> Version => Get(() => new BindableElement<PdfVersion>(
            () => Properties.Resources.MenuVersion,
            () => _model.Value.Version,
            e  => _model.Value.Version = e,
            GetDispatcher(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// Options
        ///
        /// <summary>
        /// Gets the menu that represents the viewer options of the
        /// PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<ViewerOptions> Options => Get(() => new BindableElement<ViewerOptions>(
            () => Properties.Resources.MenuLayout,
            () => _model.Value.Options,
            e  => _model.Value.Options = e,
            GetDispatcher(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// Length
        ///
        /// <summary>
        /// Gets the menu that represents the length of the specified file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<long> Length => Get(() => new BindableElement<long>(
            () => Properties.Resources.MenuFilesize,
            () => _model.File.Length,
            GetDispatcher(false)
        ));

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
        public BindableElement<DateTime> CreationTime => Get(() => new BindableElement<DateTime>(
            () => Properties.Resources.MenuCreationTime,
            () => _model.File.CreationTime,
            GetDispatcher(false)
        ));

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
        public BindableElement<DateTime> LastWriteTime => Get(() => new BindableElement<DateTime>(
            () => Properties.Resources.MenuLastWriteTime,
            () => _model.File.LastWriteTime,
            GetDispatcher(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// Summary
        ///
        /// <summary>
        /// Gets the menu that represents the summary group.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement Summary => Get(() => new BindableElement(
            () => Properties.Resources.MenuSummary,
            GetDispatcher(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// Details
        ///
        /// <summary>
        /// Gets the menu that represents the details group.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement Details => Get(() => new BindableElement(
            () => Properties.Resources.MenuDetails,
            GetDispatcher(false)
        ));

        #endregion

        #region Fields
        private readonly MetadataFacade _model;
        #endregion
    }
}
