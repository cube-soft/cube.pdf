/* ------------------------------------------------------------------------- */
///
/// ItemCollection.cs
///
/// Copyright (c) 2010 CubeSoft, Inc.
///
/// This program is free software: you can redistribute it and/or modify
/// it under the terms of the GNU Affero General Public License as published
/// by the Free Software Foundation, either version 3 of the License, or
/// (at your option) any later version.
///
/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
/// GNU Affero General Public License for more details.
///
/// You should have received a copy of the GNU Affero General Public License
/// along with this program.  If not, see <http://www.gnu.org/licenses/>.
///
/* ------------------------------------------------------------------------- */
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cube.Pdf.Page
{
    /* --------------------------------------------------------------------- */
    ///
    /// Cube.Pdf.Page.ItemCollection
    ///
    /// <summary>
    /// Item 一覧を管理するクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ItemCollection
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ItemCollection
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ItemCollection(IList<Item> inner)
        {
            InnerCollection = inner;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// InnerCollection
        /// 
        /// <summary>
        /// 内部で使用しているコレクションオブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IList<Item> InnerCollection { get; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// AddAsync
        /// 
        /// <summary>
        /// 指定されたファイルを非同期で追加します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Task AddAsync(string path)
        {
            return Task.Run(() => Add(path));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Clear
        /// 
        /// <summary>
        /// 全ての項目を削除します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Clear()
        {
            lock (_lock) InnerCollection.Clear();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// RemoveAt
        /// 
        /// <summary>
        /// 指定されたインデックスに対応する項目を削除します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void RemoveAt(int index)
        {
            lock (_lock) InnerCollection.RemoveAt(index);
        }

        #endregion

        #region Other private methods

        /* ----------------------------------------------------------------- */
        ///
        /// Add
        /// 
        /// <summary>
        /// 指定されたファイルを追加します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Add(string path) { }

        #endregion

        #region Fields
        private object _lock = new object();
        #endregion
    }
}
