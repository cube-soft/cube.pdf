/* ------------------------------------------------------------------------- */
///
/// ImagePage.cs
/// 
/// Copyright (c) 2010 CubeSoft, Inc.
/// 
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
///
///  http://www.apache.org/licenses/LICENSE-2.0
///
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.
///
/* ------------------------------------------------------------------------- */
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;

namespace Cube.Pdf
{
    /* --------------------------------------------------------------------- */
    ///
    /// ImagePage
    /// 
    /// <summary>
    /// 画像ページを生成するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class ImagePage
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        /// 
        /// <summary>
        /// ページオブジェクトを生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static Page Create(string path, int index)
        {
            using (var image = Image.FromFile(path))
            {
                return Create(path, image, index);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        /// 
        /// <summary>
        /// ページオブジェクトを生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static Page Create(string path, Image image, int index)
        {
            var guid = image.FrameDimensionsList[0];
            return CreatePage(path, image, index, new FrameDimension(guid));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        /// 
        /// <summary>
        /// ページオブジェクト一覧を生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static Page[] Create(string path)
        {
            using (var image = Image.FromFile(path))
            {
                return Create(path, image);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        /// 
        /// <summary>
        /// ページオブジェクト一覧を生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static Page[] Create(string path, Image image)
        {
            var dest = new List<Page>();
            var dimension = new FrameDimension(image.FrameDimensionsList[0]);

            for (var i = 0; i < image.GetFrameCount(dimension); ++i)
            {
                dest.Add(CreatePage(path, image, i, dimension));
            }

            return dest.ToArray();
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// CreatePage
        /// 
        /// <summary>
        /// ページオブジェクトを生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static Page CreatePage(string path, Image image, int index, FrameDimension dimension)
        {
            image.SelectActiveFrame(dimension, index);

            var x = (int)image.HorizontalResolution;
            var y = (int)image.VerticalResolution;

            return new Page
            {
                File       = new ImageFile(path, image, IconSize.Zero),
                Number     = index + 1,
                Size       = image.Size,
                Resolution = new Point(x, y),
                Rotation   = 0
            };
        }

        #endregion
    }
}
