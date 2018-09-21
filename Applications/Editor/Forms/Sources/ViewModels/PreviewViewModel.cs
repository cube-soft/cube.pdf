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
using GalaSoft.MvvmLight.Messaging;
using System.Threading;

namespace Cube.Pdf.App.Editor
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
    public class PreviewViewModel : DialogViewModel
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// PreviewViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the PreviewViewModel
        /// with the specified argumetns.
        /// </summary>
        ///
        /// <param name="src">Image collection.</param>
        /// <param name="file">File information.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public PreviewViewModel(ImageCollection src, Information file, SynchronizationContext context) :
            base(() => GetTitle(src, file), new Messenger(), context)
        {
            Model = new PreviewFacade(src, file);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Data
        ///
        /// <summary>
        /// Gets the bindable data.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PreviewBindable Data => Model.Bindable;

        /* ----------------------------------------------------------------- */
        ///
        /// Model
        ///
        /// <summary>
        /// Gets the model for the window.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected PreviewFacade Model { get; }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetTitle
        ///
        /// <summary>
        /// Gets the title of the preview window.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static string GetTitle(ImageCollection src, Information file) =>
            string.Format(
                Properties.Resources.TitlePreview,
                file.Name,
                src.Selection.First + 1,
                src.Count
            );

        #endregion
    }
}
