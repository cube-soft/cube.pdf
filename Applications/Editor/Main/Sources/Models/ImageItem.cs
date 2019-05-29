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
using Cube.Mixin.Pdf;
using Cube.Xui;
using System;
using System.ComponentModel;
using System.Windows.Media;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// ImageItem
    ///
    /// <summary>
    /// Represents information of an image.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ImageItem : DisposableObservable, IListItem
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ImageEntry
        ///
        /// <summary>
        /// Initializes a new instance of the ImageItem class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="image">Delegation to get an image.</param>
        /// <param name="selection">Shared object for selection.</param>
        /// <param name="preferences">Image preferences.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ImageItem(Func<ImageItem, ImageSource> image,
            ImageSelection selection, ImagePreferences preferences)
        {
            _image       = image;
            _selection   = selection;
            _preferences = preferences;
            _preferences.PropertyChanged += WhenPreferencesChanged;
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
        public ImageSource Image => _image?.Invoke(this);

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
            get => _width;
            private set => SetProperty(ref _width, value);
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
            get => _height;
            private set => SetProperty(ref _height, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Index
        ///
        /// <summary>
        /// Gets the index of the image in the collection.
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
                if (!SetProperty(ref _selected, value)) return;
                if (value) _selection.Add(this);
                else _selection.Remove(this);
            }
        }


        /* ----------------------------------------------------------------- */
        ///
        /// RawObject
        ///
        /// <summary>
        /// Gets or sets the raw object to create the image.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Page RawObject
        {
            get => _rawObject;
            set { if (SetProperty(ref _rawObject, value)) UpdateSize(); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Stretch
        ///
        /// <summary>
        /// Gets a value indicating how to resize the image to fill the
        /// allocated space.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Stretch Stretch
        {
            get
            {
                var w = Image?.Width  ?? 0.0;
                var h = Image?.Height ?? 1.0;
                var x = w / h;
                var y = Width / (double)Math.Max(Height, 1);

                return Math.Abs(x - y) < 0.05 ? Stretch.UniformToFill : Stretch.None;
            }
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Refresh
        ///
        /// <summary>
        /// Refresh the display image.
        /// </summary>
        ///
        /// <remarks>
        /// 表示内容の生成方法はコンストラクタで指定されたオブジェクトに
        /// 移譲されているため、このメソッドは Image を対象とした
        /// PropertyChanged イベントを発生させます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public void Refresh()
        {
            Refresh(nameof(Stretch));
            Refresh(nameof(Image));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Rotate
        ///
        /// <summary>
        /// Rotate the image with the specified degree.
        /// </summary>
        ///
        /// <param name="degree">Angle in degree unit.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Rotate(int degree)
        {
            RawObject.Delta += degree;
            UpdateSize();
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases the unmanaged resources used by the ImageItem
        /// and optionally releases the managed resources.
        /// </summary>
        ///
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                IsSelected = false;

                _preferences.PropertyChanged -= WhenPreferencesChanged;
                _preferences = null;
                _image       = null;
                _selection   = null;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// UpdateSize
        ///
        /// <summary>
        /// Updates the Width and Height properties.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void UpdateSize()
        {
            var h_magic = 22; // how to calc?
            var v_magic = 12;

            var src   = RawObject.GetViewSize().Value;
            var size  = _preferences.ItemSize;

            var h = (size - h_magic) / src.Width;
            var v = (size - v_magic - _preferences.TextHeight) / src.Height;

            var scale = Math.Min(h, v);

            Width  = (int)(src.Width * scale);
            Height = (int)(src.Height * scale);

            Refresh();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// WhenPreferencesChanged
        ///
        /// <summary>
        /// Called when some properties in the ImagePreferences class
        /// are changed.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenPreferencesChanged(object s, PropertyChangedEventArgs e)
        {
            var fire = e.PropertyName == nameof(_preferences.ItemSizeIndex) ||
                       e.PropertyName == nameof(_preferences.TextHeight);
            if (fire) UpdateSize();
        }

        #endregion

        #region Fields
        private Func<ImageItem, ImageSource> _image;
        private ImagePreferences _preferences;
        private ImageSelection _selection;
        private int _index;
        private int _width;
        private int _height;
        private bool _selected;
        private Page _rawObject;
        #endregion
    }
}
