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
using Cube.FileSystem;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

namespace Cube.Pdf
{
    /* --------------------------------------------------------------------- */
    ///
    /// PageExtension
    ///
    /// <summary>
    /// Page の拡張用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class PageExtension
    {
        #region Methods

        #region GetImagePage

        /* ----------------------------------------------------------------- */
        ///
        /// GetImagePages
        ///
        /// <summary>
        /// 画像ファイルに含まれる Page オブジェクト一覧を取得します。
        /// </summary>
        ///
        /// <param name="io">入出力用オブジェクト</param>
        /// <param name="src">画像ファイルのパス</param>
        ///
        /// <returns>Page オブジェクト一覧</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static IEnumerable<Page> GetImagePages(this IO io, string src)
        {
            using (var ss = io.OpenRead(src))
            using (var image = Image.FromStream(ss))
            {
                return io.GetImagePages(src, image);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetImagePages
        ///
        /// <summary>
        /// ページオブジェクト一覧を生成します。
        /// </summary>
        ///
        /// <param name="io">入出力用オブジェクト</param>
        /// <param name="src">画像ファイルのパス</param>
        /// <param name="image">画像オブジェクト</param>
        ///
        /// <returns>Page オブジェクト一覧</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static IEnumerable<Page> GetImagePages(this IO io, string src, Image image)
        {
            Debug.Assert(image != null);
            Debug.Assert(image.FrameDimensionsList != null);
            Debug.Assert(image.FrameDimensionsList.Length > 0);

            var dest = new List<Page>();
            var dim  = new FrameDimension(image.FrameDimensionsList[0]);

            for (var i = 0; i < image.GetFrameCount(dim); ++i)
            {
                dest.Add(io.GetImagePage(src, image, i, dim));
            }

            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetImagePage
        ///
        /// <summary>
        /// 画像ファイルに含まれる Page オブジェクトを取得します。
        /// </summary>
        ///
        /// <param name="io">入出力用オブジェクト</param>
        /// <param name="src">画像ファイルのパス</param>
        /// <param name="index">ページを表すインデックス</param>
        ///
        /// <returns>Page オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Page GetImagePage(this IO io, string src, int index)
        {
            using (var ss = io.OpenRead(src))
            using (var image = Image.FromStream(ss))
            {
                return io.GetImagePage(src, image, index);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetImagePage
        ///
        /// <summary>
        /// 画像ファイルに含まれる Page オブジェクトを取得します。
        /// </summary>
        ///
        /// <param name="io">入出力用オブジェクト</param>
        /// <param name="src">画像ファイルのパス</param>
        /// <param name="image">画像オブジェクト</param>
        /// <param name="index">ページを表すインデックス</param>
        ///
        /// <returns>Page オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Page GetImagePage(this IO io, string src, Image image, int index)
        {
            Debug.Assert(image != null);
            Debug.Assert(image.FrameDimensionsList != null);
            Debug.Assert(image.FrameDimensionsList.Length > 0);

            return io.GetImagePage(src, image, index,
                new FrameDimension(image.FrameDimensionsList[0]));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetImagePage
        ///
        /// <summary>
        /// Page オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static Page GetImagePage(this IO io, string src, Image image, int index, FrameDimension dim)
        {
            image.SelectActiveFrame(dim, index);

            var x = image.HorizontalResolution;
            var y = image.VerticalResolution;

            return new Page
            {
                File       = io.GetImageFile(src, image),
                Number     = index + 1,
                Size       = image.Size,
                Resolution = new PointF(x, y),
                Rotation   = 0,
            };
        }

        #endregion

        #region GetViewSize

        /* ----------------------------------------------------------------- */
        ///
        /// GetViewSize
        ///
        /// <summary>
        /// ページオブジェクトの表示サイズを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static SizeF GetViewSize(this Page src) =>
            GetViewSize(src, src.Resolution);

        /* ----------------------------------------------------------------- */
        ///
        /// GetViewSize
        ///
        /// <summary>
        /// ページオブジェクトの表示サイズを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static SizeF GetViewSize(this Page src, int dpi) =>
            GetViewSize(src, new PointF(dpi, dpi));

        /* ----------------------------------------------------------------- */
        ///
        /// GetViewSize
        ///
        /// <summary>
        /// ページオブジェクトの表示サイズを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static SizeF GetViewSize(this Page src, PointF dpi)
        {
            var radian = Math.PI * src.Rotation / 180.0;
            var sin    = Math.Abs(Math.Sin(radian));
            var cos    = Math.Abs(Math.Cos(radian));
            var width  = src.Size.Width * cos + src.Size.Height * sin;
            var height = src.Size.Width * sin + src.Size.Height * cos;
            var h      = dpi.X / src.Resolution.X;
            var v      = dpi.Y / src.Resolution.Y;

            return new SizeF((float)(width * h), (float)(height * v));
        }

        #endregion

        #endregion
    }
}
