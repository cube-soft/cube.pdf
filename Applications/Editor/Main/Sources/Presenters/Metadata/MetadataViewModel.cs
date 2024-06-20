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
using System;
using System.Collections.Generic;
using System.Threading;
using Cube.FileSystem;
using Cube.Xui;

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
    public sealed class MetadataViewModel : DialogViewModel<MetadataFacade>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// MetadataViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the MetadataViewModel
        /// with the specified arguments.
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
            Entity file,
            SynchronizationContext context
        ) : base(new(src, file), new(), context)
        {
            OK.Command = new DelegateCommand(() => Quit(() => callback(src), true));
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
        /// Options
        ///
        /// <summary>
        /// Gets the collection of viewer options.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<ViewerOption> Options => MetadataFacade.ViewerOptions;

        /* ----------------------------------------------------------------- */
        ///
        /// Filename
        ///
        /// <summary>
        /// Gets the menu that represents the filename.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IElement<string> Filename => Get(() => new BindableElement<string>(
            () => Surface.Texts.Metadata_Filename,
            () => Facade.File.Name,
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
        public IElement<string> Document => Get(() => new BindableElement<string>(
            () => Surface.Texts.Metadata_Title,
            () => Facade.Value.Title,
            e  => Facade.Value.Title = e,
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
        public IElement<string> Author => Get(() => new BindableElement<string>(
            () => Surface.Texts.Metadata_Author,
            () => Facade.Value.Author,
            e  => Facade.Value.Author = e,
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
        public IElement<string> Subject => Get(() => new BindableElement<string>(
            () => Surface.Texts.Metadata_Subject,
            () => Facade.Value.Subject,
            e  => Facade.Value.Subject = e,
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
        public IElement<string> Keywords => Get(() => new BindableElement<string>(
            () => Surface.Texts.Metadata_Keyword,
            () => Facade.Value.Keywords,
            e  => Facade.Value.Keywords = e,
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
        public IElement<string> Creator => Get(() => new BindableElement<string>(
            () => Surface.Texts.Metadata_Creator,
            () => Facade.Value.Creator,
            e  => Facade.Value.Creator = e,
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
        public IElement<string> Producer => Get(() => new BindableElement<string>(
            () => Surface.Texts.Metadata_Producer,
            () => Facade.Value.Producer,
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
        public IElement<PdfVersion> Version => Get(() => new BindableElement<PdfVersion>(
            () => Surface.Texts.Metadata_Version,
            () => Facade.Value.Version,
            e  => Facade.Value.Version = e,
            GetDispatcher(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// Option
        ///
        /// <summary>
        /// Gets the menu that represents the viewer options of the
        /// PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IElement<ViewerOption> Option => Get(() => new BindableElement<ViewerOption>(
            () => Surface.Texts.Metadata_Layout,
            () => Facade.Value.Options,
            e  => Facade.Value.Options = e,
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
        public IElement<long> Length => Get(() => new BindableElement<long>(
            () => Surface.Texts.Metadata_Filesize,
            () => Facade.File.Length,
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
        public IElement<DateTime> CreationTime => Get(() => new BindableElement<DateTime>(
            () => Surface.Texts.Metadata_CreationTime,
            () => Facade.File.CreationTime,
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
        public IElement<DateTime> LastWriteTime => Get(() => new BindableElement<DateTime>(
            () => Surface.Texts.Metadata_LastWriteTime,
            () => Facade.File.LastWriteTime,
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
        public IElement Summary => Get(() => new BindableElement(
            () => Surface.Texts.Metadata_Summary,
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
        public IElement Details => Get(() => new BindableElement(
            () => Surface.Texts.Metadata_Detail,
            GetDispatcher(false)
        ));

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetTitle
        ///
        /// <summary>
        /// Gets the title of the dialog.
        /// </summary>
        ///
        /// <returns>String value.</returns>
        ///
        /* ----------------------------------------------------------------- */
        protected override string GetTitle() => Surface.Texts.Metadata_Window;

        #endregion
    }
}
