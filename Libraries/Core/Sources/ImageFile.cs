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
        public ImageFile(string src) : this(new InitSource(src)) { }

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
        public ImageFile(string src, Image image) : this(new InitSource(src, image)) { }

        /* ----------------------------------------------------------------- */
        ///
        /// ImageFile
        ///
        /// <summary>
        /// Initializes a new instance of the ImageFile class with the
        /// specified source object.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ImageFile(InitSource src) : base(IoEx.GetEntitySource(src.Path), true)
        {
            try
            {
                var guid = src.Image.FrameDimensionsList[0];
                var dim  = new FrameDimension(guid);
                var x    = src.Image.HorizontalResolution;
                var y    = src.Image.VerticalResolution;

                Count      = src.Image.GetFrameCount(dim);
                Resolution = new(x, y);
            }
            finally { src.Disposable?.Dispose(); }
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// InitSource
        ///
        /// <summary>
        /// Represents the resources when initialized.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private class InitSource
        {
            public InitSource(string src)
            {
                var ss = Io.Open(src);
                Path       = src;
                Image      = Image.FromStream(ss);
                Disposable = new(Image, ss);
            }
            public InitSource(string src, Image image)
            {
                Path  = src;
                Image = image;
            }
            public string Path { get; }
            public Image Image { get; }
            public DisposableContainer Disposable { get; }
        }

        #endregion
    }
}
