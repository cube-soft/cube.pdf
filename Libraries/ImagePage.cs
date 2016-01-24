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
        /* ----------------------------------------------------------------- */
        ///
        /// Create
        /// 
        /// <summary>
        /// ページオブジェクトを生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static Page Create(string path, int index = 0)
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
        public static Page Create(string path, Image image, int index = 0)
        {
            var guid = image.FrameDimensionsList[0];
            var dim  = new System.Drawing.Imaging.FrameDimension(guid);
            image.SelectActiveFrame(dim, index);

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
    }
}
