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
namespace Cube.Pdf.App.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// ImagePreferences
    ///
    /// <summary>
    /// 画像表示に関する情報を保持するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ImagePreferences : ObservableProperty
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// ItemSize
        ///
        /// <summary>
        /// Gets or sets the size of each item.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int ItemSize
        {
            get => _itemSize;
            set => SetProperty(ref _itemSize, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ItemMargin
        ///
        /// <summary>
        /// Gets or sets the margin of each item.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int ItemMargin
        {
            get => _itemMargin;
            set => SetProperty(ref _itemMargin, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// TextHeight
        ///
        /// <summary>
        /// Gets or sets the height of each text field.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int TextHeight
        {
            get => _textHeight;
            set => SetProperty(ref _textHeight, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// VisibleFirst
        ///
        /// <summary>
        /// Gets or sets the first index of currently visible items.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int VisibleFirst
        {
            get => _first;
            set => SetProperty(ref _first, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// VisibleLast
        ///
        /// <summary>
        /// Gets or sets the last index of currently visible items.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int VisibleLast
        {
            get => _last;
            set => SetProperty(ref _last, value);
        }

        #endregion

        #region Fields
        private int _first;
        private int _last;
        private int _itemSize;
        private int _itemMargin;
        private int _textHeight;
        #endregion
    }
}
