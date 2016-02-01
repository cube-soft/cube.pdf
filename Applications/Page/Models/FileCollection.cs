/* ------------------------------------------------------------------------- */
///
/// FileCollection.cs
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
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Cube.Pdf.App.Page
{
    /* --------------------------------------------------------------------- */
    ///
    /// FileCollection
    ///
    /// <summary>
    /// ファイル一覧を管理するクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class FileCollection : ObservableCollection<FileBase>
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
        public FileCollection() : base() { }

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
        /// Add
        /// 
        /// <summary>
        /// 指定されたファイルを追加します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Add(string path, string password = "")
        {
            var ext = Path.GetExtension(path).ToLower();
            if (ext == ".pdf") AddDocument(path, password);
            else lock (_lock) Add(new ImageFile(path));
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
            return Items.Any(f => f.FullName == path);
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

        #region Other private methods

        /* ----------------------------------------------------------------- */
        ///
        /// AddDocument
        /// 
        /// <summary>
        /// PDF ファイルを追加します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void AddDocument(string path, string password)
        {
            using (var reader = new Cube.Pdf.Editing.DocumentReader())
            {
                reader.PasswordRequired += (s, e) => OnPasswordRequired(new PasswordEventArgs(path));
                reader.Open(path, password, true);
                lock (_lock) Add(reader.File);
            }
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
