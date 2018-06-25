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
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace Cube.Pdf.Itext
{
    /* --------------------------------------------------------------------- */
    ///
    /// EmbeddedImageCollection
    ///
    /// <summary>
    /// PDF ファイルに埋め込まれた画像一覧を表すクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal class EmbeddedImageCollection : IRenderListener, IEnumerable<Image>
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetEnumerator
        ///
        /// <summary>
        /// 反復用オブジェクトを取得します。
        /// </summary>
        ///
        /// <returns>反復用オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerator<Image> GetEnumerator() => _inner.GetEnumerator();

        /* ----------------------------------------------------------------- */
        ///
        /// IEnumerable.GetEnumerator
        ///
        /// <summary>
        /// 反復用オブジェクトを取得します。
        /// </summary>
        ///
        /// <returns>反復用オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /* ----------------------------------------------------------------- */
        ///
        /// RenderImage
        ///
        /// <summary>
        /// 画像描画時に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void RenderImage(ImageRenderInfo info)
        {
            var obj = info.GetImage();
            if (!(obj?.Get(PdfName.FILTER) is PdfName)) return;

            var raw = obj?.GetDrawingImage();
            if (raw == null) return;

            var sm = obj?.GetDictionary()?.GetDirectObject(PdfName.SMASK);
            if (sm == null)
            {
                _inner.Add(raw);
                return;
            }

            var tmp  = new PdfImageObject(sm as PRStream);
            var mask = tmp?.GetDrawingImage();
            var dest = Restore(raw as Bitmap, mask as Bitmap);
            _inner.Add(dest ?? raw);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// RenderText
        ///
        /// <summary>
        /// テキスト描画時に実行されます。。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void RenderText(TextRenderInfo info) { /* Ignore */ }

        /* ----------------------------------------------------------------- */
        ///
        /// BeginTextBlock
        ///
        /// <summary>
        /// テキスト描画開始時に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void BeginTextBlock() { /* Ignore */ }

        /* ----------------------------------------------------------------- */
        ///
        /// EndTextBlock
        ///
        /// <summary>
        /// テキスト描画終了時に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void EndTextBlock() { /* Ignore */ }

        #endregion

        #region Implementations

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
