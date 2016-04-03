/* ------------------------------------------------------------------------- */
///
/// ITextReader.cs
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
using iTextSharp.text.pdf;

namespace Cube.Pdf.Editing.ITextReader
{
    /* --------------------------------------------------------------------- */
    ///
    /// ITextReader.Operations
    /// 
    /// <summary>
    /// iTextSharp.text.pdf.PdfReader の拡張用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class Operations
    {
        /* ----------------------------------------------------------------- */
        ///
        /// CreatePage
        /// 
        /// <summary>
        /// Page オブジェクトを生成します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public static Page CreatePage(this PdfReader reader, FileBase file, int pagenum)
        {
            var size = reader.GetPageSize(pagenum);
            var dest = new Page();
            dest.File = file;
            dest.Number = pagenum;
            dest.Size = new Size((int)size.Width, (int)size.Height);
            dest.Rotation = reader.GetPageRotation(pagenum);
            dest.Resolution = new Point(72, 72);
            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Rotate
        /// 
        /// <summary>
        /// Page オブジェクトの情報にしたがって回転します。
        /// </summary>
        /// 
        /// <remarks>
        /// PDF ページを回転させる場合、いったん PdfReader オブジェクトの
        /// 内容を改変した後に PdfCopy オブジェクト等でコピーする方法が
        /// もっとも容易に実現できます。
        /// </remarks>
        /// 
        /* ----------------------------------------------------------------- */
        public static void Rotate(this PdfReader reader, Page page)
        {
            var rot = reader.GetPageRotation(page.Number);
            var dic = reader.GetPageN(page.Number);
            if (rot != page.Rotation) dic.Put(PdfName.ROTATE, new PdfNumber(page.Rotation));
        }
    }
}
