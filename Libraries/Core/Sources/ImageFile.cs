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
using System;
using System.Drawing;
using System.Drawing.Imaging;
using Cube.FileSystem;

namespace Cube.Pdf
{
    /* --------------------------------------------------------------------- */
    ///
    /// ImageFile
    ///
    /// <summary>
    /// Represents information of an image file.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Serializable]
    public class ImageFile : File
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ImageFile
        ///
        /// <summary>
        /// Initializes a new instance of the ImageFile class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="src">Path of the image file.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ImageFile(string src) : base(IoEx.GetEntitySource(src))
        {
            using var ss = Io.Open(src);
            using var image = Image.FromStream(ss);
            Setup(image);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ImageFile
        ///
        /// <summary>
        /// Initializes a new instance of the ImageFile class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="src">Path of the image file.</param>
        /// <param name="image">Image object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ImageFile(string src, Image image) : base(IoEx.GetEntitySource(src))
        {
            Setup(image);
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Setup
        ///
        /// <summary>
        /// Initializes properties of an ImageFile object.
        /// </summary>
        ///
        /// <param name="src">Image object.</param>
        ///
        /* ----------------------------------------------------------------- */
        private void Setup(Image src)
        {
            var guid = src.FrameDimensionsList[0];
            var dim  = new FrameDimension(guid);
            var x    = src.HorizontalResolution;
            var y    = src.VerticalResolution;

            Count      = src.GetFrameCount(dim);
            Resolution = new(x, y);
        }

        #endregion
    }
}
