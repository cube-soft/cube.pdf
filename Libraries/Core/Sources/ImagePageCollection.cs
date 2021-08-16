/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
/* ------------------------------------------------------------------------- */
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using Cube.Collections;
using Cube.FileSystem;

namespace Cube.Pdf
{
    /* --------------------------------------------------------------------- */
    ///
    /// ImagePageCollection
    ///
    /// <summary>
    /// Represents a read only collection of PDF pages converted from an
    /// image file.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ImagePageCollection : EnumerableBase<Page>, IReadOnlyList<Page>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ImagePageCollection
        ///
        /// <summary>
        /// Initializes a new instance of the ImagePageCollection class with
        /// the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Path of the image file.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ImagePageCollection(string src)
        {
            using var ss = Io.Open(src);
            using var image = Image.FromStream(ss);
            Setup(src, image);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ImagePageCollection
        ///
        /// <summary>
        /// Initializes a new instance of the ImagePageCollection class with
        /// the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Path of the image file.</param>
        /// <param name="image">Image object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ImagePageCollection(string src, Image image) { Setup(src, image); }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// File
        ///
        /// <summary>
        /// Gets the PDF file information.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ImageFile File { get; private set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Count
        ///
        /// <summary>
        /// Gets the number of pages.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Count => _inner.Count;

        /* ----------------------------------------------------------------- */
        ///
        /// Item[int]
        ///
        /// <summary>
        /// Gets the Page object corresponding the specified index.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Page this[int index] => _inner[index];

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetEnumerator
        ///
        /// <summary>
        /// Returns an enumerator that iterates through this collection.
        /// </summary>
        ///
        /// <returns>
        /// An IEnumerator(Page) object for this collection.
        /// </returns>
        ///
        /* ----------------------------------------------------------------- */
        public override IEnumerator<Page> GetEnumerator()
        {
            for (var i = 0; i < Count; ++i) yield return this[i];
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases the unmanaged resources used by the object and
        /// optionally releases the managed resources.
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

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Setup
        ///
        /// <summary>
        /// Initializes properties of an ImagePageCollection object.
        /// </summary>
        ///
        /// <param name="src">Path of the image file.</param>
        /// <param name="image">Image object.</param>
        ///
        /* ----------------------------------------------------------------- */
        private void Setup(string src, Image image)
        {
            File = new(src, image);

            var dim = new FrameDimension(image.FrameDimensionsList[0]);

            for (var i = 0; i < image.GetFrameCount(dim); ++i)
            {
                _ = image.SelectActiveFrame(dim, i);

                var x = image.HorizontalResolution;
                var y = image.VerticalResolution;

                _inner.Add(new()
                {
                    File       = File,
                    Number     = i + 1,
                    Size       = image.Size,
                    Rotation   = new Angle(),
                    Resolution = new PointF(x, y),
                });
            }
        }

        #endregion

        #region Fields
        private readonly List<Page> _inner = new();
        #endregion
    }
}
