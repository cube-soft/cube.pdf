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
using Cube.Collections;
using Cube.FileSystem;
using Cube.Pdf.Itext;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cube.Pdf.Picker
{
    /* --------------------------------------------------------------------- */
    ///
    /// ImageCollection
    ///
    /// <summary>
    /// PDF ファイルから抽出したイメージを管理するクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ImageCollection : ObservableCollection<Image>, IDisposable
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ImageCollection
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ImageCollection(string path)
        {
            Path = path;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ~ImageCollection
        ///
        /// <summary>
        /// オブジェクトを解放します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        ~ImageCollection()
        {
            Dispose(false);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// IO
        ///
        /// <summary>
        /// I/O オブジェクトを取得します
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IO IO { get; } = new IO();

        /* ----------------------------------------------------------------- */
        ///
        /// Path
        ///
        /// <summary>
        /// 画像を抽出するファイルを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Path { get; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// ExtractAsync
        ///
        /// <summary>
        /// 抽出処理を非同期で実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public async Task ExtractAsync(IProgress<ProgressEventArgs<string>> progress)
        {
            try
            {
                using (_source = new CancellationTokenSource())
                {
                    await TaskEx.Run(() => Extract(progress)).ConfigureAwait(false);
                }
            }
            finally { _source = null; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Cancel
        ///
        /// <summary>
        /// 非同期で実行中の処理をキャンセルします。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Cancel() => _source?.Cancel();

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// 全てのイメージを保存します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Save(string directory)
        {
            var basename = IO.Get(Path).BaseName;
            for (var index = 0; index < Items.Count; ++index)
            {
                Save(Items[index], directory, basename, index);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// 選択したイメージを保存します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Save(string directory, IEnumerable<int> indices)
        {
            var basename = IO.Get(Path).BaseName;
            foreach (var index in indices)
            {
                if (index < 0 || index >= Items.Count) continue;
                Save(Items[index], directory, basename, index);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Restore
        ///
        /// <summary>
        /// Images に対して行った処理を破棄し、RunAsync 完了直後の状態に
        /// 復元します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Restore()
        {
            lock (_lock)
            {
                if (Items.Count == _allImages.Count) return;
                Items.Clear();
                foreach (var image in _allImages) Items.Add(image);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// オブジェクトを解放します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Virtual methods

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// オブジェクトを解放します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void Dispose(bool disposing)
        {
            lock (_lock)
            {
                if (_disposed) return;
                _disposed = true;

                if (disposing)
                {
                    Cancel();
                    Items.Clear();
                    foreach (var image in _allImages) image.Dispose();
                    _allImages.Clear();
                }
            }
        }

        #endregion

        #region Event handlers

        /* ----------------------------------------------------------------- */
        ///
        /// WhenPasswordRequired
        ///
        /// <summary>
        /// パスワードの要求が発生した時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenPasswordRequired(QueryEventArgs<string> e)
        {
            e.Cancel = true;
            throw new ArgumentException(string.Format(
                Properties.Resources.MessagePassword,
                IO.Get(e.Query).Name
            ));
        }

        #endregion

        #region Extract methods

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// PDF ファイルからイメージを抽出します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Extract(IProgress<ProgressEventArgs<string>> progress)
        {
            try
            {
                var name = IO.Get(Path).BaseName;
                progress.Report(Create(
                    -1,
                    string.Format(Properties.Resources.MessageBegin, name)
                ));

                var result = ExtractImages(progress);

                progress.Report(Create(
                    100,
                    result.Value > 0 ?
                    string.Format(Properties.Resources.MessageEnd, name, result.Key, result.Value) :
                    string.Format(Properties.Resources.MessageNotFound, name, result.Key)
                ));
            }
            catch (OperationCanceledException /* err */)
            {
                progress.Report(Create(0, Properties.Resources.MessageCancel));
            }
            catch (Exception err)
            {
                progress.Report(Create(0, err.Message));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ExtractImages
        ///
        /// <summary>
        /// PDF ファイルからイメージを抽出します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private KeyValuePair<int, int> ExtractImages(IProgress<ProgressEventArgs<string>> progress)
        {
            var query = new Query<string>(e => WhenPasswordRequired(e));
            using (var reader = new DocumentReader(Path, query, true, true, IO))
            {
                ExtractImages(reader, progress);
                return KeyValuePair.Create(reader.Pages.Count(), Items.Count);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ExtractImages
        ///
        /// <summary>
        /// PDF ファイルからイメージを抽出します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void ExtractImages(DocumentReader src, IProgress<ProgressEventArgs<string>> progress)
        {
            var count = src.Pages.Count();
            var name = IO.Get(Path).BaseName;

            for (var i = 0; i < count; ++i)
            {
                _source.Token.ThrowIfCancellationRequested();

                var pagenum = i + 1;
                progress.Report(Create(
                   (int)(i / (double)count * 100.0),
                   string.Format(Properties.Resources.MessageProcess, name, pagenum, count)
                ));

                var images = src.GetEmbeddedImages(pagenum);
                _source.Token.ThrowIfCancellationRequested();

                lock (_lock)
                {
                    foreach (var image in images)
                    {
                        _source.Token.ThrowIfCancellationRequested();
                        _allImages.Add(image);
                        Items.Add(image);
                    }
                }
            }
        }

        #endregion

        #region Others

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// イメージを保存します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Save(Image src, string directory, string basename, int index)
        {
            var path = Unique(directory, basename, index);
            src.Save(path, System.Drawing.Imaging.ImageFormat.Png);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Unique
        ///
        /// <summary>
        /// 一意のパス名を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string Unique(string directory, string basename, int index)
        {
            var digit = string.Format("D{0}", Items.Count.ToString("D").Length);
            for (var i = 1; i < 1000; ++i)
            {
                var filename = (i == 1) ?
                               string.Format("{0}-{1}.png", basename, index.ToString(digit)) :
                               string.Format("{0}-{1} ({2}).png", basename, index.ToString(digit), i);
                var dest = IO.Combine(directory, filename);
                if (!IO.Exists(dest)) return dest;
            }

            return IO.Combine(directory, System.IO.Path.GetRandomFileName());
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// ProgressEventArgs オブジェクトを生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ProgressEventArgs<string> Create(int percentage, string message)
            => new ProgressEventArgs<string>(percentage, message);

        #endregion

        #region Fields
        private readonly object _lock = new object();
        private bool _disposed = false;
        private CancellationTokenSource _source;
        private readonly IList<Image> _allImages = new List<Image>();
        #endregion
    }
}
