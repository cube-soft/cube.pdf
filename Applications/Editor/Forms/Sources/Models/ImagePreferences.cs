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
using System;
using System.Collections.Generic;

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
        /// ItemSizeOptions
        ///
        /// <summary>
        /// Gets the selectable size list.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IReadOnlyList<int> ItemSizeOptions { get; } = new List<int>
        {
            100, 150, 200, 250, 300, 400, 500, 600, 900,
        };

        /* ----------------------------------------------------------------- */
        ///
        /// ItemSizeIndex
        ///
        /// <summary>
        /// Gets or sets the selected index of the ItemSizeOptions.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int ItemSizeIndex
        {
            get => _itemSizeIndex;
            set
            {
                var index = Math.Min(Math.Max(value, 0), ItemSizeOptions.Count - 1);
                if (!SetProperty(ref _itemSizeIndex, index)) return;
                RaisePropertyChanged(nameof(ItemSize));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ItemSize
        ///
        /// <summary>
        /// Gets the size of each item.
        /// </summary>
        ///
        /// <remarks>
        /// 設定時には ItemsSizeOptions の中で指定値を超えない最大の値が
        /// 選択されます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public int ItemSize => ItemSizeOptions[ItemSizeIndex];

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
        private int _itemSizeIndex;
        private int _itemMargin;
        private int _textHeight;
        #endregion
    }
}
