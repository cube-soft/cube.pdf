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
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using Cube.Collections;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace Cube.Pdf.Picker
{
    /* --------------------------------------------------------------------- */
    ///
    /// RenderListener
    ///
    /// <summary>
    /// provides functionality to extract embedded images in a PDF document.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal class RenderListener : EnumerableBase<Image>, IRenderListener
    {
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
        /// An IEnumerator(Image) object for this collection.
        /// </returns>
        ///
        /* ----------------------------------------------------------------- */
        public override IEnumerator<Image> GetEnumerator() => _inner.GetEnumerator();

        /* ----------------------------------------------------------------- */
        ///
        /// RenderImage
        ///
        /// <summary>
        /// Occurs when the specified image is rendered.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void RenderImage(ImageRenderInfo info)
        {
            var obj = info.GetImage();
            if (!(obj.Get(PdfName.FILTER) is PdfName)) return;

            var raw = obj.GetDrawingImage();
            if (raw == null) return;

            var sm = obj.GetDictionary().GetDirectObject(PdfName.SMASK);
            if (sm == null)
            {
                _inner.Add(raw);
                return;
            }

            var tmp  = new PdfImageObject(sm as PRStream);
            var mask = tmp.GetDrawingImage();
            var dest = Restore(raw as Bitmap, mask as Bitmap);
            _inner.Add(dest ?? raw);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// RenderText
        ///
        /// <summary>
        /// Occurs when the specified text is rendered.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void RenderText(TextRenderInfo info) { /* Ignore */ }

        /* ----------------------------------------------------------------- */
        ///
        /// BeginTextBlock
        ///
        /// <summary>
        /// Occurs when the text block begins to render.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void BeginTextBlock() { /* Ignore */ }

        /* ----------------------------------------------------------------- */
        ///
        /// EndTextBlock
        ///
        /// <summary>
        /// Occurs when the text block ends to render.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void EndTextBlock() { /* Ignore */ }

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
        /// Restore
        ///
        /// <summary>
        /// Resotres the bitmap data structure from the specified
        /// arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private Bitmap Restore(Bitmap src, Bitmap mask)
        {
            if (src == null || mask == null) return null;

            var dest = new Bitmap(src.Width, src.Height, PixelFormat.Format32bppArgb);
            for (int x = 0; x < src.Width; x++)
            for (int y = 0; y < src.Height; y++)
            {
                var color = src.GetPixel(x, y).ToArgb();
                if (x < mask.Width && y < mask.Height)
                {
                    var tmp   = mask.GetPixel(x, y);
                    var alpha = Color.FromArgb((tmp.R + tmp.G + tmp.B) / 3, 0, 0, 0);
                    color &= ~alpha.ToArgb();
                }
                dest.SetPixel(x, y, Color.FromArgb(color));
            }
            return dest;
        }

        #endregion

        #region Fields
        private readonly IList<Image> _inner = new List<Image>();
        #endregion
    }
}
