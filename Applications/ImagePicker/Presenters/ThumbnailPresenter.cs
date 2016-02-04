/* ------------------------------------------------------------------------- */
///
/// ThumbnailPresenter.cs
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
using IoEx = System.IO;

namespace Cube.Pdf.App.ImageEx
{
    /* --------------------------------------------------------------------- */
    ///
    /// ThumbnailPresenter
    ///
    /// <summary>
    /// ThumbnailForm をモデルを関連付けるためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ThumbnailPresenter : Cube.Forms.PresenterBase<ThumbnailForm, ImageCollection>
    {
        #region Constructors

        /* --------------------------------------------------------------------- */
        ///
        /// ThumbnailPresenter
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        public ThumbnailPresenter(ThumbnailForm view, ImageCollection model)
            : base(view, model)
        {
            View.FileName = IoEx.Path.GetFileNameWithoutExtension(Model.Path);
            View.Save    += View_Save;
            View.SaveAll += View_SaveAll;
            View.Preview += View_Preview;
            View.Removed += View_Removed;

            AddImages();
        }

        #endregion

        #region Events

        /* --------------------------------------------------------------------- */
        ///
        /// Completed
        /// 
        /// <summary>
        /// 操作が完了した時に発生するイベントです。
        /// </summary>
        /// 
        /// <remarks>
        /// このイベントは View.Save, View.SaveAll いずれかのイベントで
        /// 画像の保存処理が正常に完了した時に発生します。
        /// </remarks>
        ///
        /* --------------------------------------------------------------------- */
        public EventHandler Completed;

        #endregion

        #region Virtual methods

        /* --------------------------------------------------------------------- */
        ///
        /// OnCompleted
        /// 
        /// <summary>
        /// 操作が完了した時に実行されるハンドラです。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        protected virtual void OnCompleted(EventArgs e)
        {
            if (Completed != null) Completed(this, e);
        }

        #endregion

        #region Event handlers

        /* --------------------------------------------------------------------- */
        ///
        /// View_Save
        /// 
        /// <summary>
        /// 選択した抽出画像を保存する時に実行されるハンドラです。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private async void View_Save(object sender, EventArgs ev)
        {
            var dialog = new FolderBrowserDialog();
            dialog.Description = Properties.Resources.SaveFolder;
            dialog.SelectedPath = IoEx.Path.GetDirectoryName(Model.Path);
            if (dialog.ShowDialog() == DialogResult.Cancel) return;

            var indices = View.SelectedIndices;
            await Async(() => Model.Save(dialog.SelectedPath, indices));

            OnCompleted(new EventArgs());
            View.Close();
        }

        /* --------------------------------------------------------------------- */
        ///
        /// View_SaveAll
        /// 
        /// <summary>
        /// 全ての抽出画像を保存する時に実行されるハンドラです。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private async void View_SaveAll(object sender, EventArgs ev)
        {
            var dialog = new FolderBrowserDialog();
            dialog.Description = Properties.Resources.SaveFolder;
            dialog.SelectedPath = IoEx.Path.GetDirectoryName(Model.Path);
            if (dialog.ShowDialog() == DialogResult.Cancel) return;

            await Async(() => Model.Save(dialog.SelectedPath));

            OnCompleted(new EventArgs());
            View.Close();
        }

        /* --------------------------------------------------------------------- */
        ///
        /// View_Preview
        /// 
        /// <summary>
        /// 選択した抽出画像のプレビュー画面を表示する時に実行されるハンドラです。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private void View_Preview(object sender, EventArgs ev)
        {
            var indices = View.SelectedIndices;
            if (indices == null || indices.Count <= 0) return;

            var index = indices[0];
            var filename = IoEx.Path.GetFileNameWithoutExtension(Model.Path);
            var dialog = new PreviewForm();
            dialog.FileName = string.Format("{0} ({1}/{2})", filename, index, Model.Count);
            dialog.Image = Model[index];
            dialog.ShowDialog();
        }

        /* --------------------------------------------------------------------- */
        ///
        /// View_Removed
        /// 
        /// <summary>
        /// 画像が削除された時に実行されるハンドラです。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private void View_Removed(object sender, DataEventArgs<int> ev)
        {
            Model.RemoveAt(ev.Value);
        }

        #endregion

        #region Other private methods

        /* --------------------------------------------------------------------- */
        ///
        /// AddImagesAsync
        /// 
        /// <summary>
        /// 画像を View に追加します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private void AddImages()
        {
            var upper = View.ImageSize;
            for (var i = 0; i < Model.Count; ++i)
            {
                var image = Model.GetImage(i, upper);
                if (image != null) View.Add(image);
            }
        }

        #endregion
    }
}
