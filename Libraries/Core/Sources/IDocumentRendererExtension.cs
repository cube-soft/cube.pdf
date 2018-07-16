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

namespace Cube.Pdf.Mixin
{
    /* --------------------------------------------------------------------- */
    ///
    /// IDocumentRendererExtension
    ///
    /// <summary>
    /// Describes extended methods for the IDocumentRenderer interface.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class IDocumentRendererExtension
    {
        #region Methods

        #region Render

        /* ----------------------------------------------------------------- */
        ///
        /// Render
        ///
        /// <summary>
        /// Render the Page content to the Graphics object.
        /// </summary>
        ///
        /// <param name="src">Renderer object.</param>
        /// <param name="dest">Graphics object.</param>
        /// <param name="page">Page object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Render(this IDocumentRenderer src, Graphics dest, Page page) =>
            src.Render(dest, page, new PointF(0, 0), dest.VisibleClipBounds.Size);

        #endregion

        #region GetImage

        /* ----------------------------------------------------------------- */
        ///
        /// GetImage
        ///
        /// <summary>
        /// Get an Image object in which the Page content is rendered.
        /// </summary>
        ///
        /// <param name="src">Renderer object.</param>
        /// <param name="page">Page object.</param>
        ///
        /// <returns>Image object</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Image GetImage(this IDocumentRenderer src, Page page) =>
            src.GetImage(page, 1.0);

        /* ----------------------------------------------------------------- */
        ///
        /// GetImage
        ///
        /// <summary>
        /// Get an Image object in which the Page content is rendered.
        /// </summary>
        ///
        /// <param name="src">Renderer object.</param>
        /// <param name="page">Page object.</param>
        /// <param name="scale">Scale factor.</param>
        ///
        /// <returns>Image object</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Image GetImage(this IDocumentRenderer src, Page page, double scale) =>
            src.GetImage(page, page.GetDisplaySize(scale).Value);

        /* ----------------------------------------------------------------- */
        ///
        /// GetImage
        ///
        /// <summary>
        /// Get an Image object in which the Page content is rendered.
        /// </summary>
        ///
        /// <param name="src">Renderer object.</param>
        /// <param name="page">Page object.</param>
        /// <param name="size">Rendering size.</param>
        ///
        /// <returns>Image object</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Image GetImage(this IDocumentRenderer src, Page page, SizeF size)
        {
            var dest = new Bitmap((int)size.Width, (int)size.Height);
            using (var gs = Graphics.FromImage(dest)) src.Render(gs, page);
            return dest;
        }

        #endregion

        #endregion
    }
}
