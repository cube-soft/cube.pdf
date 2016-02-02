/* ------------------------------------------------------------------------- */
///
/// ProgressPresenter.cs
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
using IoEx = System.IO;

namespace Cube.Pdf.App.ImageEx
{
    /* --------------------------------------------------------------------- */
    ///
    /// Cube.Pdf.App.ImageEx.ProgressPresenter
    ///
    /// <summary>
    /// ProgressForm をモデルを関連付けるためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ProgressPresenter : Cube.Forms.PresenterBase<ProgressForm, ImageCollection>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ProgressPresenter
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ProgressPresenter(ProgressForm view, ImageCollection model)
            : base(view, model)
        {
            View.FileName = IoEx.Path.GetFileName(Model.Path);
            View.Shown      += View_Shown;
            View.FormClosed += View_Closed;
            View.Save       += View_Save;
            View.Preview    += View_Preview;
        }

        #endregion

        #region Event handlers

        /* ----------------------------------------------------------------- */
        ///
        /// View_Shown
        /// 
        /// <summary>
        /// 画面が表示された時に実行されるハンドラです。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private async void View_Shown(object sender, EventArgs ev)
        {
            var progress = new Progress<ProgressEventArgs<string>>();
            progress.ProgressChanged += (s, e) => Report(e);

            try
            {
                SyncWait(() => View.AllowOperation = false);
                await Model.ExtractAsync(progress);

            }
            catch (Exception err) { Report(0, err.Message); }
            finally
            {
                if (Model.Count > 0) SyncWait(() => View.AllowOperation = true);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// View_Closed
        /// 
        /// <summary>
        /// 画面が閉じられた時に実行されるハンドラです。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private void View_Closed(object sender, EventArgs ev)
        {
            Model.Dispose();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// View_Save
        /// 
        /// <summary>
        /// 抽出画像が保存される時に実行されるハンドラです。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private async void View_Save(object sender, EventArgs ev)
        {
            var task = new SaveTask();
            if (string.IsNullOrEmpty(task.AskFolder(Model.Path))) return;

            var basename = System.IO.Path.GetFileNameWithoutExtension(Model.Path);
            task.Images = Model;
            await task.RunAsync(basename);

            View.Close();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// View_Preview
        /// 
        /// <summary>
        /// 抽出画像のプレビュー画面を表示する時に実行されるハンドラです。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private void View_Preview(object sender, EventArgs ev)
        {
            var preview   = new ThumbnailForm();
            var presenter = new ThumbnailPresenter(preview, Model);

            var completed = false;
            presenter.Completed += (s, e) => { completed = true; };

            var removed = false;
            preview.Removed += (s, e) => { removed = true; };
            preview.FormClosed += (s, e) =>
            {
                if (completed) View.Close();
                else
                {
                    View.Show();
                    if (removed) Model.Restore();
                }
            };

            View.Hide();
            preview.Show();
        }

        #endregion

        #region Others

        /* ----------------------------------------------------------------- */
        ///
        /// Report
        /// 
        /// <summary>
        /// 進捗状況を更新します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private void Report(ProgressEventArgs<string> e) { Report(e.Percentage, e.Value); }
        private void Report(double pecentage, string message)
        {
            View.Value = (int)pecentage;
            View.Message = message;
        }

        #endregion
    }
}
