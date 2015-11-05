/* ------------------------------------------------------------------------- */
///
/// PickTask.cs
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
using System.Threading;
using System.Threading.Tasks;
using DocumentReader = Cube.Pdf.Editing.DocumentReader;

namespace Cube.Pdf.ImageEx
{
    /* --------------------------------------------------------------------- */
    ///
    /// Cube.Pdf.ImageEx.PickTask
    ///
    /// <summary>
    /// 画像を抽出する処理を非同期で実行するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class PickTask
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// PickTask
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public PickTask(string path)
        {
            Path = path;
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
        public string Path { get; private set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Images
        /// 
        /// <summary>
        /// 抽出した画像一覧を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public ObservableCollection<System.Drawing.Image> Images { get; } = new ObservableCollection<System.Drawing.Image>();

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
        public async Task RunAsync(IProgress<ProgressEventArgs> progress)
        {
            using (_source = new CancellationTokenSource())
            {
                try
                {
                    var filename = System.IO.Path.GetFileName(Path);
                    var start = string.Format(Properties.Resources.BeginMessage, filename);
                    progress.Report(new ProgressEventArgs(-1, start));

                    var result = await PickImagesAsync(progress);
                    var done = string.Format(Properties.Resources.EndMessage, filename, result.Key, result.Value);
                    progress.Report(new ProgressEventArgs(100, done));
                }
                catch (OperationCanceledException /* err */)
                {
                    progress.Report(new ProgressEventArgs(0, Properties.Resources.CancelMessage));
                    return;
                }
            }
        }

        #endregion

        #region Other private methods

        /* ----------------------------------------------------------------- */
        ///
        /// PickImagesAsync
        /// 
        /// <summary>
        /// 非同期で PDF ファイルから画像を抽出します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private async Task<KeyValuePair<int, int>> PickImagesAsync(IProgress<ProgressEventArgs> progress)
        {
            using (var reader = new DocumentReader())
            {
                await reader.OpenAsync(Path, string.Empty);

                var filename = System.IO.Path.GetFileName(Path);
                var n = reader.Pages.Count;
                for (var i = 0; i < n; ++i)
                {
                    _source.Token.ThrowIfCancellationRequested();

                    var pagenum = i + 1;
                    var value = (int)(i / (double)reader.Pages.Count * 100.0);
                    var message = string.Format(Properties.Resources.ProcessMessage, filename, pagenum, n);
                    progress.Report(new ProgressEventArgs(value, message));

                    var src = await reader.GetImagesAsync(pagenum);
                    foreach (var image in src) Images.Add(image);
                }

                return new KeyValuePair<int, int>(n, Images.Count);
            }
        }

        #endregion

        #region Fields
        private CancellationTokenSource _source;
        #endregion
    }
}
