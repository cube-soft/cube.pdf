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
using Cube.Pdf.Mixin;
using System;
using System.Drawing;
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
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ImageEntry
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="image">画像生成用オブジェクト</param>
        /// <param name="preferences">表示設定</param>
        ///
        /* ----------------------------------------------------------------- */
        public ImageEntry(Func<ImageEntry, ImageSource> image, ImagePreferences preferences)
        {
            _image = image;
            Preferences = preferences;
        }

        #endregion

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
        public ImageSource Image => _image(this);

        /* ----------------------------------------------------------------- */
        ///
        /// Width
        ///
        /// <summary>
        /// 画像を表示する幅を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Width
        {
            get
            {
                var src = RawObject.GetDisplaySize();
                return (int)(src.Width * GetRatio(src));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Height
        ///
        /// <summary>
        /// 画像を表示する高さを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Height
        {
            get
            {
                var src = RawObject.GetDisplaySize();
                return (int)(src.Height * GetRatio(src));
            }
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
        /// Preferences
        ///
        /// <summary>
        /// 表示設定に関する情報を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ImagePreferences Preferences { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// RawObject
        ///
        /// <summary>
        /// 画像生成元の情報を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Page RawObject
        {
            get => _rawObject;
            set => SetProperty(ref _rawObject, value);
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Update
        ///
        /// <summary>
        /// 表示内容を更新します。
        /// </summary>
        ///
        /// <remarks>
        /// 表示内容の生成方法はコンストラクタで指定されたオブジェクトに
        /// 移譲されているため、このメソッドは Image を対象とした
        /// PropertyChanged イベントを発生させます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public void Update() => RaisePropertyChanged(nameof(Image));

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetRatio
        ///
        /// <summary>
        /// 表示倍率を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private double GetRatio(SizeF src)
        {
            var m = 10; // TODO: how to calc?
            var h = (Preferences.Width -　Preferences.Margin * 2 - m) / src.Width;
            var v = (Preferences.Height - Preferences.Margin * 2 - Preferences.TextHeight) / src.Height;
            return Math.Min(h, v);
        }

        #endregion

        #region Fields
        private readonly Func<ImageEntry, ImageSource> _image;
        private Page _rawObject;
        private string _text;
        #endregion
    }
}
