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
using System.Windows.Media;
using Cube.FileSystem;
using Cube.Mixin.Pdf;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// PreviewBindable
    ///
    /// <summary>
    /// Provides values for binding to the PreviewWindow.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class PreviewBindable : ObservableBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// PreviewBindable
        ///
        /// <summary>
        /// Initializes a new instance of the PreviewBindable class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Source images.</param>
        /// <param name="file">Information of the PDF file.</param>
        /// <param name="dispatcher">Dispatcher object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public PreviewBindable(ImageCollection src, Entity file, Dispatcher dispatcher) : base(dispatcher)
        {
            var size  = src[src.Selection.First].RawObject.GetViewSize();
            var magic = 14.0; // Scrollbar width
            var ratio = size.Width / size.Height;
            var diff  = size.Width > size.Height ? magic * ratio : -(magic * ratio);

            Source = src;
            File   = file;
            Width  = (int)size.Width;
            Height = (int)(size.Height + diff);
            Busy   = false;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Source
        ///
        /// <summary>
        /// Gets the collection of images.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ImageCollection Source { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// File
        ///
        /// <summary>
        /// Gets the information of the PDF file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Entity File { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Image
        ///
        /// <summary>
        /// Gets the generated image object.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ImageSource Image
        {
            get => Get<ImageSource>();
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Width
        ///
        /// <summary>
        /// Gets the width of the window.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Width
        {
            get => Get<int>();
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Height
        ///
        /// <summary>
        /// Gets the height of the window.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Height
        {
            get => Get<int>();
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Busy
        ///
        /// <summary>
        /// Gets a value indicating whether models are busy.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Busy
        {
            get => Get<bool>();
            set => Set(value);
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases the unmanaged resources used by the object
        /// and optionally releases the managed resources.
        /// </summary>
        ///
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        protected override void Dispose(bool disposing) { }

        #endregion
    }
}
