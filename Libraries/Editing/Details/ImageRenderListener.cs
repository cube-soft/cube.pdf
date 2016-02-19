/* ------------------------------------------------------------------------- */
///
/// ImageRenderListener.cs
///
/// Copyright (c) 2010 CubeSoft, Inc. All rights reserved.
///
/// This program is free software: you can redistribute it and/or modify
/// it under the terms of the GNU Affero General Public License as published
/// by the Free Software Foundation, either version 3 of the License, or
/// (at your option) any later version.
///
/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
/// GNU Affero General Public License for more details.
///
/// You should have received a copy of the GNU Affero General Public License
/// along with this program.  If not, see <http://www.gnu.org/licenses/>.
///
/* ------------------------------------------------------------------------- */
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace Cube.Pdf.Editing
{
    /* --------------------------------------------------------------------- */
    ///
    /// ImageRenderListener
    /// 
    /// <summary>
    /// PDF ファイル中の画像を抽出するた IRenderListerner 実装クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal class ImageRenderListener : IRenderListener
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Images
        /// 
        /// <summary>
        /// 抽出した画像一覧を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IList<Image> Images { get; } = new List<Image>();

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// RenderImage
        ///
        /// <summary>
        /// 画像を抽出します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void RenderImage(ImageRenderInfo info)
        {
            var obj = info.GetImage();
            var filter = obj?.Get(PdfName.FILTER) as PdfName;
            if (filter == null) return;

            var image = obj?.GetDrawingImage();
            if (image == null) return;

            var smask = obj?.GetDictionary()?.GetDirectObject(PdfName.SMASK);
            if (smask == null)
            {
                Images.Add(image);
                return;
            }

            var tmp = new PdfImageObject(smask as PRStream);
            var mask = tmp?.GetDrawingImage();
            var restore = Restore(image as Bitmap, mask as Bitmap);
            Images.Add(restore ?? image);
        }

        #region Other implementations for IRenderListener
        public void BeginTextBlock() { }
        public void EndTextBlock() { }
        public void RenderText(TextRenderInfo info) { }
        #endregion

        #endregion

        #region Others

        /* ----------------------------------------------------------------- */
        ///
        /// Restore
        ///
        /// <summary>
        /// 描画部分のビットマップとマスク処理用のビットマップから元々の
        /// 画像を生成します。
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
                    var tmp = mask.GetPixel(x, y);
                    var alpha = Color.FromArgb((tmp.R + tmp.G + tmp.B) / 3, 0, 0, 0);
                    color &= ~alpha.ToArgb();
                }
                dest.SetPixel(x, y, Color.FromArgb(color));
            }
            return dest;
        }

        #endregion
    }
}
