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
using System.Threading;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// PreviewViewModel
    ///
    /// <summary>
    /// Represents the ViewModel for a PreviewWindow instance.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class PreviewViewModel : DialogViewModel<PreviewFacade>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// PreviewViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the PreviewViewModel
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Image collection.</param>
        /// <param name="file">File information.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public PreviewViewModel(ImageCollection src, Entity file, SynchronizationContext context) : base(
            new PreviewFacade(src, file, new Dispatcher(context, false)),
            new Aggregator(),
            context
        ) { }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Value
        ///
        /// <summary>
        /// Gets the bindable value.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PreviewBindable Value => Facade.Value;

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
        protected override string GetTitle() => string.Format(
            Properties.Resources.TitlePreview,
            Facade.Value.File.Name,
            Facade.Value.Source.Selection.First + 1,
            Facade.Value.Source.Count
        );

        #endregion
    }
}
