/* ------------------------------------------------------------------------- */
///
/// ImagePicker.cs
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
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Cube.Pdf.Editing;
using IoEx = System.IO;

namespace Cube.Pdf.App.ImageEx
{
    /* --------------------------------------------------------------------- */
    ///
    /// ImagePicker
    ///
    /// <summary>
    /// 画像を抽出するクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ImagePicker : IDisposable
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ImagePicker
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public ImagePicker(string path)
        {
            Path = path;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ~ImagePicker
        /// 
        /// <summary>
        /// オブジェクトを解放します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        ~ImagePicker()
        {
            Dispose(false);
        }

        #endregion

        #region Properties

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

        /* ----------------------------------------------------------------- */
        ///
        /// Images
        /// 
        /// <summary>
        /// 抽出した画像一覧を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public ObservableCollection<Image> Images { get; } = new ObservableCollection<Image>();

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// RunAsync
        /// 
        /// <summary>
        /// 抽出処理を非同期で実行します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public async Task RunAsync(IProgress<ProgressEventArgs<string>> progress)
        {
            try
            {
                using (_source = new CancellationTokenSource())
                {
                    await Task.Run(() => Run(progress));
                }
            }
            finally { _source = null; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Cancel
        /// 
        /// <summary>
        /// 現在、実行中の処理をキャンセルします。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public void Cancel()
        {
            if (_source != null) _source.Cancel();
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
                Images.Clear();
                foreach (var image in _allImages) Images.Add(image);
            }
        }

        /* --------------------------------------------------------------------- */
        ///
        /// GetImage
        /// 
        /// <summary>
        /// 指定されたインデックスに対応する画像を upper に応じてリサイズして
        /// 返します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        public Image GetImage(int index, Size upperSize)
        {
            if (index < 0 || index >= Images.Count) return null;

            var image  = Images[index];
            var scale  = image.Width > image.Height ?
                         Math.Min(upperSize.Width / (double)image.Width, 1.0) :
                         Math.Min(upperSize.Height / (double)image.Height, 1.0);

            var width  = (int)(image.Width * scale);
            var height = (int)(image.Height * scale);

            var x = (upperSize.Width - width) / 2;
            var y = (upperSize.Height - height) / 2;

            var dest = new Bitmap(upperSize.Width, upperSize.Height);
            using (var gs = Graphics.FromImage(dest))
            {
                gs.DrawImage(image, x, y, width, height);
            }
            return dest;
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
                    Images.Clear();
                    foreach (var image in _allImages) image.Dispose();
                    _allImages.Clear();
                }
            }
        }

        #endregion

        #region Event handlers

        /* ----------------------------------------------------------------- */
        ///
        /// Reader_PasswordRequired
        /// 
        /// <summary>
        /// パスワードの要求が発生した時に実行されるハンドラです。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private void Reader_PasswordRequired(object sender, PasswordEventArgs e)
        {
            e.Cancel = true;
            throw new EncryptionException(string.Format(
                Properties.Resources.PasswordMessage,
                IoEx.Path.GetFileName(e.Path)
            ));
        }

        #endregion

        #region Other private methods

        /* ----------------------------------------------------------------- */
        ///
        /// Run
        /// 
        /// <summary>
        /// 処理を実行します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private void Run(IProgress<ProgressEventArgs<string>> progress)
        {
            try
            {
                var name = IoEx.Path.GetFileNameWithoutExtension(Path);
                progress.Report(Create(
                    -1,
                    string.Format(Properties.Resources.BeginMessage, name)
                ));

                var result = Pick(progress);

                progress.Report(Create(
                    100,
                    result.Value > 0 ?
                    string.Format(Properties.Resources.EndMessage, name, result.Key, result.Value) :
                    string.Format(Properties.Resources.NotFoundMessage, name, result.Key)
                ));
            }
            catch (OperationCanceledException /* err */)
            {
                progress.Report(Create(0, Properties.Resources.CancelMessage));
            }
            catch (Exception err)
            {
                progress.Report(Create(0, err.Message));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Pick
        /// 
        /// <summary>
        /// PDF ファイルから画像を抽出します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private KeyValuePair<int, int> Pick(IProgress<ProgressEventArgs<string>> progress)
        {
            using (var reader = new DocumentReader())
            {
                reader.PasswordRequired += Reader_PasswordRequired;
                reader.Open(Path, string.Empty, true);

                Pick(reader, progress);
                return new KeyValuePair<int, int>(reader.Pages.Count, Images.Count);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Pick
        /// 
        /// <summary>
        /// PDF ファイルから画像を抽出します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private void Pick(DocumentReader src, IProgress<ProgressEventArgs<string>> progress)
        {
            var name = IoEx.Path.GetFileNameWithoutExtension(Path);

            for (var i = 0; i < src.Pages.Count; ++i)
            {
                _source.Token.ThrowIfCancellationRequested();

                var pagenum = i + 1;
                progress.Report(Create(
                   (int)(i / (double)src.Pages.Count * 100.0),
                   string.Format(Properties.Resources.ProcessMessage, name, pagenum, src.Pages.Count)
                ));

                var images = src.GetImages(pagenum);
                _source.Token.ThrowIfCancellationRequested();

                lock (_lock)
                {
                    foreach (var image in images)
                    {
                        _source.Token.ThrowIfCancellationRequested();
                        _allImages.Add(image);
                        Images.Add(image);
                    }
                }
            }
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
        {
            return new ProgressEventArgs<string>(percentage, message);
        }

        #endregion

        #region Fields
        private bool _disposed = false;
        private object _lock = new object();
        private CancellationTokenSource _source;
        private IList<Image> _allImages = new List<Image>();
        #endregion
    }
}
