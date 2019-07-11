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

namespace Cube.Pdf.Pdfium
{
    /* --------------------------------------------------------------------- */
    ///
    /// RenderExtension
    ///
    /// <summary>
    /// Provides extended methods of the RenderOption class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class RenderExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetFlags
        ///
        /// <summary>
        /// Gets the flags from the specified option.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static int GetFlags(this RenderOption src)
        {
            var dest = RenderFlags.Empty;
            if (src.Annotation) dest |= RenderFlags.Annotation;
            if (src.Grayscale)  dest |= RenderFlags.Grayscale;
            if (src.Print)      dest |= RenderFlags.Printng;
            if (!src.AntiAlias) dest |= RenderFlags.NoSmoothText | RenderFlags.NoSmoothImage | RenderFlags.NoSmoothPath;
            return (int)dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetBitmap
        ///
        /// <summary>
        /// Creates a new instance of the Bitmap class with the specified
        /// arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static Bitmap GetBitmap(this RenderOption src, int width, int height)
        {
            var dest = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            src.DrawBackground(e => { using (var gs = Graphics.FromImage(dest)) gs.Clear(e); });
            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// DrawBackground
        ///
        /// <summary>
        /// Draws the background.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static void DrawBackground(this RenderOption src, Action<Color> action)
        {
            if (src.Background != Color.Transparent) action(src.Background);
        }

        #endregion
    }
}
