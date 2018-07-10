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
using System.Windows.Media;

namespace Cube.Pdf.App.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// ImageEntry
    ///
    /// <summary>
    /// 画像情報を保持するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ImageEntry : ObservableProperty
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Image
        ///
        /// <summary>
        /// 画像オブジェクトを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ImageSource Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Text
        ///
        /// <summary>
        /// 表示テキストを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Width
        ///
        /// <summary>
        /// 幅を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Width
        {
            get => _width;
            set => SetProperty(ref _width, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Height
        ///
        /// <summary>
        /// 高さを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Height
        {
            get => _height;
            set => SetProperty(ref _height, value);
        }

        #endregion

        #region Fields
        private ImageSource _image;
        private string _text;
        private int _width;
        private int _height;
        #endregion
    }
}
