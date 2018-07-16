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
using Cube.Xui;
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
    public class ImageEntry : ObservableProperty, IListItem
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
        /// <param name="selection">Shared object for selection.</param>
        /// <param name="preferences">Image preferences.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ImageEntry(Func<ImageEntry, ImageSource> image,
            ImageSelection selection, ImagePreferences preferences)
        {
            _image      = image;
            _selection  = selection;
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
            set => SetProperty(ref _index, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// IsSelected
        ///
        /// <summary>
        /// Gets a value indicating whether this entry is selected.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool IsSelected
        {
            get => _selected;
            set
            {
                if (SetProperty(ref _selected, value))
                {
                    if (value) _selection.Add(this);
                    else _selection.Remove(this);
                }
            }
        }

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

        /* ----------------------------------------------------------------- */
        ///
        /// Preferences
        ///
        /// <summary>
        /// Gets the preferences for images.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ImagePreferences Preferences { get; }

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
        private readonly ImageSelection _selection;
        private int _index;
        private bool _selected;
        private Page _rawObject;
        #endregion
    }
}
