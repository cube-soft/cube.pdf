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
using System.Collections.Generic;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace Cube.Pdf.Editing
{
    /* --------------------------------------------------------------------- */
    ///
    /// Cube.Pdf.Editing.ImageRenderListener
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
            var image = info.GetImage();
            var filter = image.Get(PdfName.FILTER) as PdfName;
            if (filter == null) return;
            Images.Add(image.GetDrawingImage());
        }

        #region Other implementations for IRenderListener
        public void BeginTextBlock() { }
        public void EndTextBlock() { }
        public void RenderText(TextRenderInfo info) { }
        #endregion

        #endregion
    }
}
