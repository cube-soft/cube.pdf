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
using System.Windows.Forms;

namespace Cube.Pdf.ImageEx
{
    /* --------------------------------------------------------------------- */
    ///
    /// Cube.Pdf.ImageEx.ProgressPresenter
    ///
    /// <summary>
    /// ProgressForm をモデルを関連付けるためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ProgressPresenter : PresenterBase<ProgressForm, PickTask>
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
        public ProgressPresenter(ProgressForm view, PickTask model)
            : base(view, model)
        {
            View.FileName = System.IO.Path.GetFileNameWithoutExtension(Model.Path);
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
            var progress = new Progress<ProgressEventArgs>();
            progress.ProgressChanged += (s, e) => Update(e);

            try
            {
                View.AllowOperation = false;
                await Model.RunAsync(progress);
                if (Model.Images.Count > 0) View.AllowOperation = true;
            }
            catch (Exception err) { Update(0, err.Message); }
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
            task.Images = Model.Images;
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

        /* ----------------------------------------------------------------- */
        ///
        /// Update
        /// 
        /// <summary>
        /// View を更新します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private void Update(ProgressEventArgs e) { Update(e.Value, e.Message); }
        private void Update(int value, string message)
        {
            View.Value = value;
            View.Message = message;
        }
    }
}
