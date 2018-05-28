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
using Cube.Log;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using IoEx = System.IO;

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
    public class FileCollection : ObservableCollection<MediaFile>
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

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Metadata
        ///
        /// <summary>
        /// PDF のメタ情報を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Metadata Metadata { get; } = new Metadata();

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
        public EventHandler<QueryEventArgs<string, string>> PasswordRequired;

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
            var ext = IoEx.Path.GetExtension(path).ToLower();
            if (ext == ".pdf") AddDocument(path, password);
            else lock (_lock) Add(new ImageFile(path));
        }

        /* --------------------------------------------------------------------- */
        ///
        /// Add
        ///
        /// <summary>
        /// ファイルを追加します。
        /// </summary>
        ///
        /// <remarks>
        /// 追加不可能なファイルに関しては読み飛ばします。
        /// </remarks>
        ///
        /* --------------------------------------------------------------------- */
        public void Add(string[] files, int hierarchy)
        {
            foreach (var path in files)
            {
                if (Contains(path)) continue;

                if (IoEx.Directory.Exists(path))
                {
                    if (hierarchy <= 0) continue;
                    Add(IoEx.Directory.GetFiles(path), hierarchy - 1);
                }
                else if (IoEx.File.Exists(path))
                {
                    try { Add(path); }
                    catch (Exception err) { this.LogWarn($"Ignore:{path}", err); }
                }
            }
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

        /* ----------------------------------------------------------------- */
        ///
        /// Merge
        ///
        /// <summary>
        /// ファイルを結合します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Merge(string path)
        {
            var dir = IoEx.Path.GetDirectoryName(path);
            var tmp = IoEx.Path.Combine(dir, Guid.NewGuid().ToString("N"));

            try
            {
                var writer = new Cube.Pdf.Itext.DocumentWriter();
                foreach (var file in Items)
                {
                    if (file is PdfFile) AddDocument(file as PdfFile, writer);
                    else AddImage(file as ImageFile, writer);
                }
                writer.Metadata = Metadata;
                writer.Save(tmp);

                var op = new Cube.FileSystem.IO();
                op.Failed += (s, e) =>
                {
                    e.Cancel = true;
                    throw e.Exception;
                };
                op.Move(tmp, path, true);
            }
            finally { IoEx.File.Delete(tmp); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Split
        ///
        /// <summary>
        /// ファイルを分割します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Split(string directory, IList<string> results)
        {
            var writer = new Cube.Pdf.Itext.DocumentSplitter();
            foreach (var item in Items)
            {
                if (item is PdfFile) AddDocument(item as PdfFile, writer);
                else AddImage(item as ImageFile, writer);
            }
            writer.Metadata = Metadata;
            writer.Save(directory);

            foreach (var result in writer.Results) results.Add(result);
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
        protected virtual void OnPasswordRequired(QueryEventArgs<string, string> e)
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
            using (var reader = new Cube.Pdf.Itext.DocumentReader())
            {
                reader.PasswordRequired += (s, e) => OnPasswordRequired(e);
                reader.Open(path, password, true);
                if (!reader.IsOpen) return;

                lock (_lock) Add(reader.File);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// AddDocument
        ///
        /// <summary>
        /// PDF ファイルを追加します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void AddDocument(PdfFile src, IDocumentWriter dest)
        {
            if (src == null) return;

            using (var reader = new Cube.Pdf.Itext.DocumentReader())
            {
                reader.PasswordRequired += (s, e) => { e.Cancel = true; };
                reader.Open(src.FullName, src.Password, true);
                if (reader.IsOpen) dest.Add(reader.Pages);
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
        private void AddImage(ImageFile src, IDocumentWriter dest)
        {
            if (src == null) return;

            var pages = ImagePage.Create(src.FullName);
            if (pages != null) dest.Add(pages);
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
                var inserted = offset < 0 ? -1 : Count;
                foreach (var index in indices)
                {
                    var newindex = offset < 0 ?
                        Math.Max(index + offset, inserted + 1) :
                        Math.Min(index + offset, inserted - 1);
                    Move(index, newindex);
                    inserted = newindex;
                }
            }
        }

        #endregion

        #region Fields
        private object _lock = new object();
        #endregion
    }
}
