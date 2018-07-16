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
using System.Drawing;
using System.Linq;

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
        /// BaseSize
        ///
        /// <summary>
        /// Width および Height の基準となる値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int BaseSize
        {
            get => _baseSize;
            set { if (SetProperty(ref _baseSize, value)) Resize(); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Margin
        ///
        /// <summary>
        /// 上下左右の余白を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Margin
        {
            get => _margin;
            set => SetProperty(ref _margin, value);
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
            private set => SetProperty(ref _width, value);
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
            private set => SetProperty(ref _height, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// TextHeight
        ///
        /// <summary>
        /// テキスト領域の高さを取得または設定します。
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

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Update
        ///
        /// <summary>
        /// Updates the size information.
        /// </summary>
        ///
        /// <param name="previous">Previous size object.</param>
        /// <param name="current">New size object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Update(SizeF? previous, SizeF? current)
        {
            if (!previous.HasValue && !current.HasValue) return;
            Resize(() =>
            {
                if (previous.HasValue)
                {
                    RemoveOrDecrement(_ws, (int)previous.Value.Width);
                    RemoveOrDecrement(_hs, (int)previous.Value.Height);
                }

                if (current.HasValue)
                {
                    SetOrIncrement(_ws, (int)current.Value.Width);
                    SetOrIncrement(_hs, (int)current.Value.Height);
                }
            });
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// SetOrIncrement
        ///
        /// <summary>
        /// Sets or increments a value to the IDictionary(int, int).
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SetOrIncrement(IDictionary<int, int> src, int key)
        {
            if (src.ContainsKey(key)) ++src[key];
            else src.Add(key, 1);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// RemoveOrDecrement
        ///
        /// <summary>
        /// Removes or decrements a value from the IDictionary(int, int).
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void RemoveOrDecrement(IDictionary<int, int> src, int key)
        {
            if (src.ContainsKey(key))
            {
                --src[key];
                if (src[key] <= 0) src.Remove(key);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Resize
        ///
        /// <summary>
        /// 任意の処理を実行後、Width および Height を更新します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Resize(Action action)
        {
            action();
            Resize();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Resize
        ///
        /// <summary>
        /// Width および Height を更新します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Resize()
        {
            var w = _ws.Keys.LastOrDefault();
            var h = _hs.Keys.LastOrDefault();

            Width  = (w >= h || h == 0) ? BaseSize : (int)(BaseSize * (w / (double)h));
            Height = (h >= w || w == 0) ? BaseSize : (int)(BaseSize * (h / (double)w));
        }

        #endregion

        #region Fields
        private int _first;
        private int _last;
        private int _width;
        private int _height;
        private int _baseSize;
        private int _margin;
        private int _textHeight;
        private readonly SortedDictionary<int, int> _ws = new SortedDictionary<int, int>();
        private readonly SortedDictionary<int, int> _hs = new SortedDictionary<int, int>();
        #endregion
    }
}
