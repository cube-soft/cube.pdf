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
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

namespace Cube.Pdf
{
    /* --------------------------------------------------------------------- */
    ///
    /// FileExtension
    ///
    /// <summary>
    /// File の拡張用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class FileExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetImageFile
        ///
        /// <summary>
        /// 画像ファイルを表す File オブジェクトを取得します。
        /// </summary>
        ///
        /// <param name="io">入出力用オブジェクト</param>
        /// <param name="src">画像ファイルのパス</param>
        ///
        /// <returns>File オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static File GetImageFile(this IO io, string src)
        {
            using (var ss = io.OpenRead(src))
            using (var image = Image.FromStream(ss))
            {
                return io.GetImageFile(src, image);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetImageFile
        ///
        /// <summary>
        /// 画像ファイルを表す File オブジェクトを取得します。
        /// </summary>
        ///
        /// <param name="io">入出力用オブジェクト</param>
        /// <param name="src">画像ファイルのパス</param>
        /// <param name="image">画像オブジェクト</param>
        ///
        /// <returns>File オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static File GetImageFile(this IO io, string src, Image image)
        {
            Debug.Assert(image != null);
            Debug.Assert(image.FrameDimensionsList != null);
            Debug.Assert(image.FrameDimensionsList.Length > 0);

            var guid = image.FrameDimensionsList[0];
            var dim  = new FrameDimension(guid);
            var x    = image.HorizontalResolution;
            var y    = image.VerticalResolution;

            return new File(src, io.GetRefreshable())
            {
                Count      = image.GetFrameCount(dim),
                Resolution = new PointF(x, y),
            };
        }

        #endregion
    }
}
