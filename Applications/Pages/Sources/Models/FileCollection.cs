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
using Cube.FileSystem;
using Cube.Log;
using Cube.Pdf.Mixin;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Cube.Pdf.App.Pages
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
    public class FileCollection : ObservableCollection<File>
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// IO
        ///
        /// <summary>
        /// I/O オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IO IO { get; } = new IO();

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
        public event EventHandler<QueryEventArgs<string>> PasswordRequired;

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
        public void Add(string path)
        {
            var ext = IO.Get(path).Extension.ToLower();
            if (ext == ".pdf") AddDocument(path);
            else lock (_lock) Add(IO.GetImageFile(path));
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

                var info = IO.Get(path);

                if (info.IsDirectory)
                {
                    if (hierarchy <= 0) continue;
                    Add(IO.GetFiles(path), hierarchy - 1);
                }
                else if (info.Exists)
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
            var dir = IO.Get(path).DirectoryName;
            var tmp = IO.Combine(dir, Guid.NewGuid().ToString("N"));

            try
            {
                var writer = new Cube.Pdf.Itext.DocumentWriter();
                foreach (var file in Items)
                {
                    if (file is PdfFile) AddDocument(file as PdfFile, writer);
                    else AddImage(file as ImageFile, writer);
                }
                writer.Set(Metadata);
                writer.Save(tmp);
                IO.Move(tmp, path, true);
            }
            finally { IO.TryDelete(tmp); }
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
            writer.Set(Metadata);
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
        protected virtual void OnPasswordRequired(QueryEventArgs<string> e)
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
        private void AddDocument(string path)
        {
            var query = new Query<string>(e => OnPasswordRequired(e));
            using (var reader = new Cube.Pdf.Itext.DocumentReader(path, query, true, IO))
            {
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
            var query = new Query<string>(e => e.Cancel = true);
            using (var reader = new Cube.Pdf.Itext.DocumentReader(src.FullName, query, true, IO))
            {
                dest.Add(reader.Pages);
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
            var pages = IO.GetImagePages(src.FullName);
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
        private readonly object _lock = new object();
        #endregion
    }
}
