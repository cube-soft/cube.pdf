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
    /// Stores an image and related information.
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
        /// Initializes a new instance with the specified arguments.
        /// </summary>
        ///
        /// <param name="image">Delegation to get an image.</param>
        /// <param name="preferences">Image preferences.</param>
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
        /// Gets the image of this entry.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ImageSource Image => _image(this);

        /* ----------------------------------------------------------------- */
        ///
        /// Width
        ///
        /// <summary>
        /// Gets the width of the image.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Width
        {
            get
            {
                var src = RawObject.GetDisplaySize();
                return (int)(src.Width * GetScale(src));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Height
        ///
        /// <summary>
        /// Gets the height of the image.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Height
        {
            get
            {
                var src = RawObject.GetDisplaySize();
                return (int)(src.Height * GetScale(src));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Index
        ///
        /// <summary>
        /// Gets the index of the this object in the collection.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Index
        {
            get => _index;
            set
            {
                if (SetProperty(ref _index, value)) RaisePropertyChanged(nameof(Text));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Text
        ///
        /// <summary>
        /// Gets the display text.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Text => (Index + 1).ToString();

        /* ----------------------------------------------------------------- */
        ///
        /// Preferences
        ///
        /// <summary>
        /// Gets the preferences about the image.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ImagePreferences Preferences { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// RawObject
        ///
        /// <summary>
        /// Gets the raw object to create the image.
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
        /// Updates the content of the Image property.
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
        /// GetScale
        ///
        /// <summary>
        /// Gets the scale factor to show the image.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private double GetScale(SizeF src)
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
        private int _index;
        #endregion
    }
}
