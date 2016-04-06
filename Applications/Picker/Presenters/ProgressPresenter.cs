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
using Cube.Log;
using IoEx = System.IO;

namespace Cube.Pdf.App.Picker
{
    /* --------------------------------------------------------------------- */
    ///
    /// ProgressPresenter
    ///
    /// <summary>
    /// ProgressForm をモデルを関連付けるためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ProgressPresenter : PresenterBase<ProgressForm, ImageCollection>
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
        public ProgressPresenter(ProgressForm view, ImageCollection model, EventAggregator events)
            : base(view, model, events)
        {
            Events.SaveAll.Handle += SaveAll_Handle;
            Events.Preview.Handle += Preview_Handle;

            View.FileName = IoEx.Path.GetFileName(Model.Path);
            View.Shown      += View_Shown;
            View.FormClosed += (s, e) => Model.Dispose();
        }

        #endregion

        #region Event handlers

        /* ----------------------------------------------------------------- */
        ///
        /// SaveAll_Handle
        /// 
        /// <summary>
        /// 抽出画像が保存される時に実行されるハンドラです。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private void SaveAll_Handle(object sender, EventArgs e)
            => SyncWait(async () =>
        {
            try
            {
                var dialog = Dialogs.Save(Model.Path);
                if (dialog.ShowDialog() == DialogResult.Cancel) return;

                View.AllowOperation = false;
                await Async(() => Model.Save(dialog.SelectedPath));
            }
            catch (Exception err) { this.LogError(err.Message, err); }
            finally { View.Close(); }
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Preview_Handle
        /// 
        /// <summary>
        /// 抽出画像のプレビュー画面を表示する時に実行されるハンドラです。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private void Preview_Handle(object sender, EventArgs e)
            => Sync(() =>
        {
            var sview = new ThumbnailForm();
            var presenter = new ThumbnailPresenter(sview, Model, Events);

            var completed = false;
            presenter.Completed += (s, ev) => { completed = true; };

            var removed = false;
            sview.Removed += (s, ev) => { removed = true; };
            sview.FormClosed += (s, ev) =>
            {
                if (completed) View.Close();
                else
                {
                    View.Show();
                    if (removed) Model.Restore();
                }
            };

            View.Hide();
            sview.Show();
        });

        #region EventAggregator

        #endregion

        #region Views

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

        #endregion

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
