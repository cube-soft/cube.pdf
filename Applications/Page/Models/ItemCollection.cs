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
using System.IO;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Cube.Pdf.App.Page
{
    /* --------------------------------------------------------------------- */
    ///
    /// Cube.Pdf.App.Page.ItemCollection
    ///
    /// <summary>
    /// Item 一覧を管理するクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ItemCollection : ObservableCollection<Item>
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
        public ItemCollection() : base() { }

        #endregion

        #region Events

        /* ----------------------------------------------------------------- */
        ///
        /// PasswordRequired
        /// 
        /// <summary>
        /// パスワードが要求された時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public EventHandler<PasswordEventArgs> PasswordRequired;

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
        public async Task AddAsync(string path, string password = "")
        {
            var ext = Path.GetExtension(path).ToLower();
            if (ext == ".pdf") await AddPdfAsync(path, password);
            else AddImage(path);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Contains
        /// 
        /// <summary>
        /// 指定されたパスを表す項目が既に存在しているかどうか判別します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Contains(string path)
        {
            var check = new Item(PageType.Unknown, path);
            return Contains(check);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Move
        /// 
        /// <summary>
        /// 項目を移動します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Move(IList<int> indices, int offset)
        {
            if (offset == 0) return;
            MoveItems(offset < 0 ? indices : indices.Reverse(), offset);
        }

        #endregion

        #region Virtual methods

        /* ----------------------------------------------------------------- */
        ///
        /// OnPasswordRequired
        /// 
        /// <summary>
        /// パスワードが要求された時に実行されるハンドラです。
        /// </summary>
        /// 
        /// <remarks>
        /// イベントハンドラが設定されていない場合は、無限ループを防ぐ
        /// ために操作をキャンセルします。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnPasswordRequired(PasswordEventArgs e)
        {
            if (PasswordRequired != null) PasswordRequired(this, e);
            else e.Cancel = true;
        }

        #endregion

        #region Override methods

        /* ----------------------------------------------------------------- */
        ///
        /// ClearItems
        /// 
        /// <summary>
        /// 全ての項目を削除します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void ClearItems()
        {
            lock (_lock)
            {
                foreach (var item in Items)
                {
                    var dispose = item.Value as IDisposable;
                    if (dispose != null) dispose.Dispose();
                    item.Value = null;
                }
                base.ClearItems();
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// RemoveItem
        /// 
        /// <summary>
        /// 指定されたインデックスに対応する項目を削除します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void RemoveItem(int index)
        {
            lock (_lock)
            {
                var item = this[index];
                base.RemoveItem(index);

                var dispose = item.Value as IDisposable;
                if (dispose != null) dispose.Dispose();
            }
        }

        #endregion

        #region Other private methods

        /* ----------------------------------------------------------------- */
        ///
        /// AddPdfAsync
        /// 
        /// <summary>
        /// PDF ファイルを非同期で追加します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private async Task AddPdfAsync(string path, string password)
        {
            var reader = new Cube.Pdf.Editing.DocumentReader();

            try
            {
                await reader.OpenAsync(path, password);
                var encrypt = (reader.EncryptionStatus == EncryptionStatus.RestrictedAccess);
                if (encrypt) throw new EncryptionException();

                var item = new Item(PageType.Pdf, path);
                item.Value = reader;
                item.PageCount = reader.Pages.Count;
                lock (_lock) Add(item);
            }
            catch (EncryptionException /* err */)
            {
                reader.Dispose();
                var e = new PasswordEventArgs(path);
                OnPasswordRequired(e);
                if (!e.Cancel) await AddPdfAsync(path, e.Password);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// AddImage
        /// 
        /// <summary>
        /// 画像ファイルを追加します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void AddImage(string path)
        {
            var image = new Bitmap(path);
            var guid = image.FrameDimensionsList[0];
            var dimension = new System.Drawing.Imaging.FrameDimension(guid);

            var item = new Item(PageType.Image, path);
            item.Value = image;
            item.PageCount = image.GetFrameCount(dimension);
            lock (_lock) Add(item);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// MoveItems
        /// 
        /// <summary>
        /// 項目を移動します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void MoveItems(IEnumerable<int> indices, int offset)
        {
            lock (_lock)
            {
                foreach (var index in indices)
                {
                    var newindex = index + offset;
                    if (newindex < 0 || newindex >= Count) break;
                    Move(index, newindex);
                }
            }
        }

        #endregion

        #region Fields
        private object _lock = new object();
        #endregion
    }
}
