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
using Cube.Pdf.Pdfium.PdfiumApi;
using System;
using System.Drawing;

namespace Cube.Pdf.Pdfium
{
    /* --------------------------------------------------------------------- */
    ///
    /// PdfiumRenderer
    ///
    /// <summary>
    /// PDFium の API をラップした描画クラスです。
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
        /// 指定された条件で描画します。
        /// </summary>
        ///
        /// <param name="src">PDFium オブジェクト</param>
        /// <param name="dest">出力オブジェクト</param>
        /// <param name="page">ページ情報</param>
        /// <param name="point">描画開始座標</param>
        /// <param name="size">描画サイズ</param>
        /// <param name="angle">回転角度</param>
        /// <param name="flags">描画フラグ</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Render(this PdfiumReader src, Graphics dest, Page page,
            PointF point, SizeF size, Angle angle, int flags)
        {
            var retry = 5;
            var hp = Facade.FPDF_LoadPage(src.RawObject, page.Number - 1, retry);
            if (hp == IntPtr.Zero) throw PdfiumLibrary.GetLoadException();
            var hdc = dest.GetHdc();

            try
            {
                Facade.FPDF_RenderPage(
                    hdc,
                    hp,
                    (int)point.X,
                    (int)point.Y,
                    (int)size.Width,
                    (int)size.Height,
                    GetRotation(angle),
                    flags,
                    retry
                );
            }
            finally
            {
                dest.ReleaseHdc(hdc);
                Facade.FPDF_ClosePage(hp);
            }
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetRotation
        ///
        /// <summary>
        /// 回転角度を表す値を取得します。
        /// </summary>
        ///
        /// <remarks>
        /// PDFium は 90 度単位でしか対応していないため、45 度単位で
        /// 補正しています。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private static int GetRotation(Angle src) => (src + 45).Degree / 90;

        #endregion
    }
}
