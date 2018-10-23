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
using Cube.FileSystem;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

namespace Cube.Pdf.Mixin
{
    /* --------------------------------------------------------------------- */
    ///
    /// PageExtension
    ///
    /// <summary>
    /// Describes extended methods for the Page class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class PageExtension
    {
        #region Methods

        #region GetImagePage

        /* ----------------------------------------------------------------- */
        ///
        /// GetImagePage
        ///
        /// <summary>
        /// Gets a Page collection from the specified file.
        /// </summary>
        ///
        /// <param name="io">I/O object.</param>
        /// <param name="src">File path of the Image.</param>
        ///
        /// <returns>Page collection.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static IEnumerable<Page> GetImagePages(this IO io, string src)
        {
            using (var ss = io.OpenRead(src))
            using (var image = Image.FromStream(ss))
            {
                return io.GetImagePages(src, image);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetImagePage
        ///
        /// <summary>
        /// Gets a Page collection from the specified Image.
        /// </summary>
        ///
        /// <param name="io">I/O object.</param>
        /// <param name="src">File path of the Image.</param>
        /// <param name="image">Image object.</param>
        ///
        /// <returns>Page collection.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static IEnumerable<Page> GetImagePages(this IO io, string src, Image image)
        {
            Debug.Assert(image != null);
            Debug.Assert(image.FrameDimensionsList != null);
            Debug.Assert(image.FrameDimensionsList.Length > 0);

            var dest = new List<Page>();
            var dim  = new FrameDimension(image.FrameDimensionsList[0]);

            for (var i = 0; i < image.GetFrameCount(dim); ++i)
            {
                dest.Add(io.GetImagePage(src, image, i, dim));
            }

            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetImagePage
        ///
        /// <summary>
        /// Gets a Page object from the specified file.
        /// </summary>
        ///
        /// <param name="io">I/O object.</param>
        /// <param name="src">File path of the Image.</param>
        /// <param name="index">Index of the Image.</param>
        ///
        /// <returns>Page object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Page GetImagePage(this IO io, string src, int index)
        {
            using (var ss = io.OpenRead(src))
            using (var image = Image.FromStream(ss))
            {
                return io.GetImagePage(src, image, index);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetImagePage
        ///
        /// <summary>
        /// Gets a Page object from the specified image.
        /// </summary>
        ///
        /// <param name="io">I/O object.</param>
        /// <param name="src">File path of the Image.</param>
        /// <param name="image">Image object.</param>
        /// <param name="index">Index of the Image.</param>
        ///
        /// <returns>Page object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Page GetImagePage(this IO io, string src, Image image, int index)
        {
            Debug.Assert(image != null);
            Debug.Assert(image.FrameDimensionsList != null);
            Debug.Assert(image.FrameDimensionsList.Length > 0);

            return io.GetImagePage(src, image, index,
                new FrameDimension(image.FrameDimensionsList[0]));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetImagePage
        ///
        /// <summary>
        /// Gets a Page object from the specified values.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static Page GetImagePage(this IO io, string src, Image image, int index, FrameDimension dim)
        {
            image.SelectActiveFrame(dim, index);

            var x = image.HorizontalResolution;
            var y = image.VerticalResolution;

            return new Page(
                io.GetImageFile(src, image), // File
                index + 1,                   // Number
                image.Size,                  // Size
                new Angle(),                 // Rotation
                new PointF(x, y)             // Resolution
            );
        }

        #endregion

        #region GetViewSize

        /* ----------------------------------------------------------------- */
        ///
        /// GetViewSize
        ///
        /// <summary>
        /// Gets the display size of this Page.
        /// </summary>
        ///
        /// <param name="src">Page object.</param>
        ///
        /// <remarks>Display size.</remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static SizeF? GetViewSize(this Page src) => src.GetViewSize(1.0);

        /* ----------------------------------------------------------------- */
        ///
        /// GetViewSize
        ///
        /// <summary>
        /// Gets the display size of this Page from the specified values.
        /// </summary>
        ///
        /// <param name="src">Page object.</param>
        /// <param name="scale">Scale factor.</param>
        ///
        /// <remarks>Display size.</remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static SizeF? GetViewSize(this Page src, double scale)
        {
            if (src == null) return null;

            var angle  = src.Rotation + src.Delta;
            var sin    = Math.Abs(Math.Sin(angle.Radian));
            var cos    = Math.Abs(Math.Cos(angle.Radian));
            var width  = src.Size.Width * cos + src.Size.Height * sin;
            var height = src.Size.Width * sin + src.Size.Height * cos;

            return new SizeF((float)(width * scale), (float)(height * scale));
        }

        #endregion

        #endregion
    }
}
