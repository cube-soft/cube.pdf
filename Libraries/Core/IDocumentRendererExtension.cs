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
    /// IDocumentRenderer の拡張用クラスです。
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
        /// PDF の内容を描画します。
        /// </summary>
        ///
        /// <param name="src">Renderer オブジェクト</param>
        /// <param name="dest">描画先オブジェクト</param>
        /// <param name="page">ページ情報</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Render(this IDocumentRenderer src, Graphics dest, Page page) =>
            src.Render(dest, page, page.GetViewSize());

        /* ----------------------------------------------------------------- */
        ///
        /// Render
        ///
        /// <summary>
        /// PDF の内容を描画します。
        /// </summary>
        ///
        /// <param name="src">Renderer オブジェクト</param>
        /// <param name="dest">描画先オブジェクト</param>
        /// <param name="page">ページ情報</param>
        /// <param name="ratio">描画倍率</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Render(this IDocumentRenderer src, Graphics dest,
            Page page, double ratio) =>
            src.Render(dest, page, page.GetViewSize(ratio));

        /* ----------------------------------------------------------------- */
        ///
        /// Render
        ///
        /// <summary>
        /// PDF の内容を描画します。
        /// </summary>
        ///
        /// <param name="src">Renderer オブジェクト</param>
        /// <param name="dest">描画先オブジェクト</param>
        /// <param name="page">ページ情報</param>
        /// <param name="ratio">描画倍率</param>
        /// <param name="rotation">回転角度</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Render(this IDocumentRenderer src, Graphics dest,
            Page page, double ratio, Angle rotation) =>
            src.Render(dest, page, page.GetViewSize(ratio, rotation), rotation);

        /* ----------------------------------------------------------------- */
        ///
        /// Render
        ///
        /// <summary>
        /// PDF の内容を描画します。
        /// </summary>
        ///
        /// <param name="src">Renderer オブジェクト</param>
        /// <param name="dest">描画先オブジェクト</param>
        /// <param name="page">ページ情報</param>
        /// <param name="size">描画サイズ</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Render(this IDocumentRenderer src, Graphics dest,
            Page page, SizeF size) =>
            src.Render(dest, page, size, new Angle());

        /* ----------------------------------------------------------------- */
        ///
        /// Render
        ///
        /// <summary>
        /// PDF の内容を描画します。
        /// </summary>
        ///
        /// <param name="src">Renderer オブジェクト</param>
        /// <param name="dest">描画先オブジェクト</param>
        /// <param name="page">ページ情報</param>
        /// <param name="size">描画サイズ</param>
        /// <param name="rotation">回転角度</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Render(this IDocumentRenderer src, Graphics dest,
            Page page, SizeF size, Angle rotation) =>
            src.Render(dest, page, new PointF(0, 0), size, rotation);

        #endregion

        #region GetImage

        /* ----------------------------------------------------------------- */
        ///
        /// GetImage
        ///
        /// <summary>
        /// PDF の内容を描画した Image オブジェクトを取得します。
        /// </summary>
        ///
        /// <param name="src">Renderer オブジェクト</param>
        /// <param name="page">ページ情報</param>
        ///
        /// <returns>Image オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Image GetImage(this IDocumentRenderer src, Page page) =>
            src.GetImage(page, page.GetViewSize());

        /* ----------------------------------------------------------------- */
        ///
        /// GetImage
        ///
        /// <summary>
        /// PDF の内容を描画した Image オブジェクトを取得します。
        /// </summary>
        ///
        /// <param name="src">Renderer オブジェクト</param>
        /// <param name="page">ページ情報</param>
        /// <param name="ratio">描画倍率</param>
        ///
        /// <returns>Image オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Image GetImage(this IDocumentRenderer src,
            Page page, double ratio) =>
            src.GetImage(page, page.GetViewSize(ratio));

        /* ----------------------------------------------------------------- */
        ///
        /// GetImage
        ///
        /// <summary>
        /// PDF の内容を描画した Image オブジェクトを取得します。
        /// </summary>
        ///
        /// <param name="src">Renderer オブジェクト</param>
        /// <param name="page">ページ情報</param>
        /// <param name="ratio">描画倍率</param>
        /// <param name="rotation">回転角度</param>
        ///
        /// <returns>Image オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Image GetImage(this IDocumentRenderer src,
            Page page, double ratio, Angle rotation) =>
            src.GetImage(page, page.GetViewSize(ratio, rotation), rotation);

        /* ----------------------------------------------------------------- */
        ///
        /// GetImage
        ///
        /// <summary>
        /// PDF の内容を描画した Image オブジェクトを取得します。
        /// </summary>
        ///
        /// <param name="src">Renderer オブジェクト</param>
        /// <param name="page">ページ情報</param>
        /// <param name="size">描画サイズ</param>
        ///
        /// <returns>Image オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Image GetImage(this IDocumentRenderer src, Page page, SizeF size) =>
            src.GetImage(page, size, new Angle());

        /* ----------------------------------------------------------------- */
        ///
        /// GetImage
        ///
        /// <summary>
        /// PDF の内容を描画した Image オブジェクトを取得します。
        /// </summary>
        ///
        /// <param name="src">Renderer オブジェクト</param>
        /// <param name="page">ページ情報</param>
        /// <param name="size">描画サイズ</param>
        /// <param name="rotation">回転角度</param>
        ///
        /// <returns>Image オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Image GetImage(this IDocumentRenderer src,
            Page page, SizeF size, Angle rotation)
        {
            var dest = new Bitmap((int)size.Width, (int)size.Height);
            using (var gs = Graphics.FromImage(dest)) src.Render(gs, page, size, rotation);
            return dest;
        }

        #endregion

        #endregion
    }
}
