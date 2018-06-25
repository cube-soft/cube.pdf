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
using System.Windows.Forms;
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
        /// <param name="view">View オブジェクト</param>
        /// <param name="model">Model オブジェクト</param>
        /// <param name="ea">イベント集約オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public ProgressPresenter(ProgressForm view, ImageCollection model, IAggregator ea) :
            base(view, model, ea)
        {
            View.FileName = IoEx.Path.GetFileName(Model.Path);
            View.Aggregator = Aggregator;

            Aggregator.GetEvents()?.Save.Subscribe(Save_Handle);
            Aggregator.GetEvents()?.Preview.Subscribe(Preview_Handle);

            View.Shown += View_Shown;
        }

        #endregion

        #region Event handlers

        #region EventHub

        /* ----------------------------------------------------------------- */
        ///
        /// Save_Handle
        ///
        /// <summary>
        /// 抽出画像が保存される時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private async void Save_Handle(int[] files)
        {
            var path = GetSaveFile();
            if (string.IsNullOrEmpty(path)) return;

            SyncWait(() => View.AllowOperation = false);

            try
            {
                await Async(() =>
                {
                    if (files == null) Model.Save(path);
                    else Model.Save(path, files);
                });
                Aggregator.GetEvents()?.SaveComplete.Publish();
            }
            catch (Exception err) { this.LogError(err.Message, err); }

            SyncWait(() =>
            {
                View.AllowOperation = true;
                if (View.Visible) View.Close();
            });
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Preview_Handle
        ///
        /// <summary>
        /// 抽出画像のプレビュー画面を表示する時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Preview_Handle()
            => Sync(() =>
        {
            Model.Restore();

            var sview = new ThumbnailForm();
            var _     = new ThumbnailPresenter(sview, Model, Aggregator);

            sview.FormClosed += (s, ev) =>
            {
                if (sview.Complete) View.Close();
                else View.Show();
            };

            View.Hide();
            sview.Show();
        });

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

            SyncWait(() => View.AllowOperation = false);

            try { await Model.ExtractAsync(progress); }
            catch (Exception err) { Report(0, err.Message); }

            if (Model.Count > 0) SyncWait(() => View.AllowOperation = true);
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
        private void Report(ProgressEventArgs<string> e) { Report(e.Ratio, e.Value); }
        private void Report(double pecentage, string message)
        {
            View.Value = (int)pecentage;
            View.Message = message;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetSaveFile
        ///
        /// <summary>
        /// 保存ファイル名を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string GetSaveFile()
            => SyncWait(() =>
        {
            var dialog = Dialogs.Save(Model.Path);
            return dialog.ShowDialog() == DialogResult.Cancel ?
                   string.Empty :
                   dialog.SelectedPath;
        });

        #endregion
    }
}
