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
    /// PdfiumRenderer
    ///
    /// <summary>
    /// Provides functionality to render PDF pages via PDFium API.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class PdfiumRenderer
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Render
        ///
        /// <summary>
        /// Executes the rendering with the specified arguments.
        /// </summary>
        ///
        /// <param name="src">PDFium object.</param>
        /// <param name="dest">Graphics to be rendered.</param>
        /// <param name="page">Page object.</param>
        /// <param name="point">Starting point.</param>
        /// <param name="size">Rendering size.</param>
        /// <param name="options">Rendering options.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Render(this PdfiumReader src,
            Graphics dest,
            Page page,
            PointF point,
            SizeF size,
            RenderOption options
        ) => src.Invoke(e => Render(e, dest, page, point, size, options));

        /* ----------------------------------------------------------------- */
        ///
        /// Render
        ///
        /// <summary>
        /// Executes the rendering with the specified arguments.
        /// </summary>
        ///
        /// <param name="src">PDFium object.</param>
        /// <param name="page">Page object.</param>
        /// <param name="size">Rendering size.</param>
        /// <param name="options">Rendering options.</param>
        ///
        /// <returns>Image to be rendered.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Image Render(this PdfiumReader src,
            Page page,
            SizeF size,
            RenderOption options
        ) => src.Invoke(e => Render(e, page, size, options));

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Render
        ///
        /// <summary>
        /// Executes the rendering with the specified arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static Image Render(IntPtr core, Page page, SizeF size,
            RenderOption options) => Load(core, page.Number, hp =>
        {
            var bpp    = 4;
            var width  = (int)size.Width;
            var height = (int)size.Height;
            var dest   = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            using (var gs = Graphics.FromImage(dest)) Draw(gs, options.Background);
            var obj = dest.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, dest.PixelFormat);
            var hbm = NativeMethods.FPDFBitmap_CreateEx(width, height, bpp, obj.Scan0, width * bpp);
            NativeMethods.FPDF_RenderPageBitmap(hbm, hp, 0, 0, width, height, GetRotation(page.Delta), 0);
            NativeMethods.FPDFBitmap_Destroy(hbm);
            dest.UnlockBits(obj);

            return dest;
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Render
        ///
        /// <summary>
        /// Executes the rendering with the specified arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static void Render(IntPtr core, Graphics dest,
            Page page, PointF point, SizeF size,
            RenderOption options) => Load(core, page.Number, hp =>
        {
            var hdc = dest.GetHdc();
            Draw(dest, options.Background);
            NativeMethods.FPDF_RenderPage(
                hdc,
                hp,
                (int)point.X,
                (int)point.Y,
                (int)size.Width,
                (int)size.Height,
                GetRotation(page.Delta),
                0
            );
            dest.ReleaseHdc(hdc);
            return true;
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Load
        ///
        /// <summary>
        /// Loads the specified page and invokes the specified function.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static T Load<T>(IntPtr core, int pagenum, Func<IntPtr, T> func)
        {
            if (core == IntPtr.Zero) return default;
            var hp = NativeMethods.FPDF_LoadPage(core, pagenum - 1);
            if (hp == IntPtr.Zero) throw new LoadException(LoadStatus.PageError);

            try { return func(hp); }
            finally { NativeMethods.FPDF_ClosePage(hp); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Draw
        ///
        /// <summary>
        /// Draws with the specified color.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static void Draw(Graphics src, Color color)
        {
            if (color != Color.Transparent) src.Clear(color);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetRotation
        ///
        /// <summary>
        /// Gets the rotation angle in degree unit.
        /// </summary>
        ///
        /// <remarks>
        /// Normalizes the degree because the PDFium only supports in
        /// 90-degree units.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private static int GetRotation(Angle src) => (src + 45).Degree / 90;

        #endregion
    }
}
