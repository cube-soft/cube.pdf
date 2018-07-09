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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Cube.Pdf.App.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// ImageCacheList
    ///
    /// <summary>
    /// PDF ページのサムネイル一覧を管理するクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ImageCacheList : IReadOnlyList<ImageSource>, INotifyCollectionChanged
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ImageCacheList
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="context">同期用コンテキスト</param>
        ///
        /* ----------------------------------------------------------------- */
        public ImageCacheList(SynchronizationContext context)
        {
            _context = context;

            var pages = new ObservableCollection<Page>();
            pages.CollectionChanged += (s, e) => OnCollectionChanged(e);
            Pages   = pages;
            Loading = new BitmapImage(new Uri("pack://application:,,,/Assets/Medium/Loading.png"));
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Items[int]
        ///
        /// <summary>
        /// インデックスに対応するオブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ImageSource this[int index] => Loading;

        /* ----------------------------------------------------------------- */
        ///
        /// Renderer
        ///
        /// <summary>
        /// 描画用オブジェクトを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IDocumentRenderer Renderer { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Pages
        ///
        /// <summary>
        /// Page オブジェクトを一覧を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IList<Page> Pages { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Count
        ///
        /// <summary>
        /// 要素数を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Count => Pages.Count;

        /* ----------------------------------------------------------------- */
        ///
        /// Loading
        ///
        /// <summary>
        /// ロード中を表すオブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected ImageSource Loading { get; private set; }

        #endregion

        #region Events

        /* ----------------------------------------------------------------- */
        ///
        /// CollectionChanged
        ///
        /// <summary>
        /// コレクションの内容変更時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /* ----------------------------------------------------------------- */
        ///
        /// OnCollectionChanged
        ///
        /// <summary>
        /// CollectionChanged イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (_context != null) _context.Send(_ => CollectionChanged?.Invoke(this, e), null);
            else CollectionChanged?.Invoke(this, e);
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetEnumerator
        ///
        /// <summary>
        /// 反復用オブジェクトを取得します。
        /// </summary>
        ///
        /// <returns>反復用オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerator<ImageSource> GetEnumerator()
        {
            for (var i = 0; i < Count; ++i) yield return this[i];
        }

        /* ----------------------------------------------------------------- */
        ///
        /// IEnumerable.GetEnumerator
        ///
        /// <summary>
        /// 反復用オブジェクトを取得します。
        /// </summary>
        ///
        /// <returns>反復用オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        #region Fields
        private readonly SynchronizationContext _context;
        #endregion
    }
}
