/* ------------------------------------------------------------------------- */
///
/// ImageFile.cs
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
    /// ImageFile
    /// 
    /// <summary>
    /// 画像ファイルの情報を保持するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ImageFile : FileBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ImageFile
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ImageFile(string path) : this(path, IconSize.Small) { }

        /* ----------------------------------------------------------------- */
        ///
        /// ImageFile
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ImageFile(string path, IconSize size)
            : base(path, size)
        {
            using (var image = Bitmap.FromFile(path))
            {
                InitializeValues(image);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ImageFile
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ImageFile(string path, Image image, IconSize size)
            : base(path, size)
        {
            InitializeValues(image);
        }

        #endregion

        #region Other private methods

        /* ----------------------------------------------------------------- */
        ///
        /// InitializeValues
        /// 
        /// <summary>
        /// 各種プロパティを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void InitializeValues(Image image)
        {
            var guid = image.FrameDimensionsList[0];
            var dim = new System.Drawing.Imaging.FrameDimension(guid);
            PageCount = image.GetFrameCount(dim);
            Resolution = new Point((int)image.HorizontalResolution, (int)image.VerticalResolution);
        }

        #endregion
    }
}
