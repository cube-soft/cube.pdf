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
using System.Drawing;
using System.Drawing.Imaging;
using Cube.FileSystem;

namespace Cube.Pdf.Mixin
{
    /* --------------------------------------------------------------------- */
    ///
    /// FileExtension
    ///
    /// <summary>
    /// Provides extended methods about File and its inherited classes.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class FileExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetPdfFile
        ///
        /// <summary>
        /// Gets the File object that represents the specified PDF document.
        /// </summary>
        ///
        /// <param name="io">I/O handler.</param>
        /// <param name="src">Path of the PDF file.</param>
        /// <param name="password">Password to open the PDF file.</param>
        ///
        /// <returns>PdfFile object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static PdfFile GetPdfFile(this IO io, string src, string password) => new(src, password, io);

        /* ----------------------------------------------------------------- */
        ///
        /// GetImageFile
        ///
        /// <summary>
        /// Gets the File object that represents the specified image.
        /// </summary>
        ///
        /// <param name="io">I/O handler.</param>
        /// <param name="src">Path of the image file.</param>
        ///
        /// <returns>ImageFile object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static ImageFile GetImageFile(this IO io, string src)
        {
            using var ss    = io.OpenRead(src);
            using var image = Image.FromStream(ss);
            return io.GetImageFile(src, image);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetImageFile
        ///
        /// <summary>
        /// Gets the File object that represents the specified image.
        /// </summary>
        ///
        /// <param name="io">I/O handler.</param>
        /// <param name="src">Path of the image file.</param>
        /// <param name="image">Image object.</param>
        ///
        /// <returns>ImageFile object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static ImageFile GetImageFile(this IO io, string src, Image image)
        {
            var guid = image.FrameDimensionsList[0];
            var dim  = new FrameDimension(guid);
            var x    = image.HorizontalResolution;
            var y    = image.VerticalResolution;

            return new(src, io)
            {
                Count      = image.GetFrameCount(dim),
                Resolution = new PointF(x, y),
            };
        }

        #endregion
    }
}
