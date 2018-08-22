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
using GalaSoft.MvvmLight.Messaging;
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
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public MetadataViewModel(SynchronizationContext context) :
            base(() => Properties.Resources.TitleMetadata, new Messenger(), context) { }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Summary
        ///
        /// <summary>
        /// Gets the menu that represents the summary group.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public MenuEntry Summary { get; } = new MenuEntry(
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
        public MenuEntry Details { get; } = new MenuEntry(
            () => Properties.Resources.MenuDetails
        );

        /* ----------------------------------------------------------------- */
        ///
        /// PageCount
        ///
        /// <summary>
        /// Gets the menu that represents the number of pages.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public MenuEntry Filename { get; } = new MenuEntry(
            () => Properties.Resources.MenuFilename
        );

        /* ----------------------------------------------------------------- */
        ///
        /// DocumentTitle
        ///
        /// <summary>
        /// Gets the menu that represents the title of the specified
        /// PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public MenuEntry DocumentTitle { get; } = new MenuEntry(
            () => Properties.Resources.MenuTitle
        );

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
        public MenuEntry Author { get; } = new MenuEntry(
            () => Properties.Resources.MenuAuthor
        );

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
        public MenuEntry Subject { get; } = new MenuEntry(
            () => Properties.Resources.MenuSubject
        );

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
        public MenuEntry Keywords { get; } = new MenuEntry(
            () => Properties.Resources.MenuKeywords
        );

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
        public MenuEntry Version { get; } = new MenuEntry(
            () => Properties.Resources.MenuPdfVersion
        );

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
        public MenuEntry Layout { get; } = new MenuEntry(
            () => Properties.Resources.MenuLayout
        );

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
        public MenuEntry Creator { get; } = new MenuEntry(
            () => Properties.Resources.MenuCreator
        );

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
        public MenuEntry Producer { get; } = new MenuEntry(
            () => Properties.Resources.MenuProducer
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Length
        ///
        /// <summary>
        /// Gets the menu that represents the length of the specified file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public MenuEntry Length { get; } = new MenuEntry(
            () => Properties.Resources.MenuFilesize
        );

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
        public MenuEntry CreationTime { get; } = new MenuEntry(
            () => Properties.Resources.MenuCreationTime
        );

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
        public MenuEntry LastWriteTime { get; } = new MenuEntry(
            () => Properties.Resources.MenuLastWriteTime
        );

        #endregion
    }
}
