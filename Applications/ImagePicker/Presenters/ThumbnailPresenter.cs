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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskEx = System.Threading.Tasks.Task;

namespace Cube.Pdf.ImageEx
{
    /* --------------------------------------------------------------------- */
    ///
    /// Cube.Pdf.ImageEx.ThumbnailPresenter
    ///
    /// <summary>
    /// ThumbnailForm をモデルを関連付けるためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ThumbnailPresenter : PresenterBase<ThumbnailForm, PickTask>
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
        public ThumbnailPresenter(ThumbnailForm view, PickTask model)
            : base(view, model)
        {
            View.FileName = System.IO.Path.GetFileName(Model.Path);
            View.Save    += View_Save;
            View.SaveAll += View_SaveAll;
            View.Preview += View_Preview;
            View.Removed += View_Removed;

            AddImages();
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
            var folder = GetFolder();
            if (string.IsNullOrEmpty(folder)) return;

            var basename = System.IO.Path.GetFileNameWithoutExtension(Model.Path);
            var task = new SaveTask();
            task.Images = Model.Images;
            task.Folder = folder;
            await task.RunAsync(basename, View.SelectedIndices);

            View.Close();
        }

        /* --------------------------------------------------------------------- */
        ///
        /// View_Save
        /// 
        /// <summary>
        /// 全ての抽出画像を保存する時に実行されるハンドラです。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private async void View_SaveAll(object sender, EventArgs ev)
        {
            var folder = GetFolder();
            if (string.IsNullOrEmpty(folder)) return;

            var basename = System.IO.Path.GetFileNameWithoutExtension(Model.Path);
            var task = new SaveTask();
            task.Images = Model.Images;
            task.Folder = folder;
            await task.RunAsync(basename);

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

            var dialog = new PreviewForm();
            dialog.Image = Model.Images[indices[0]];
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
            Model.Images.RemoveAt(ev.Value);
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
            for (var i = 0; i < Model.Images.Count; ++i)
            {
                var image = Model.GetImage(i, upper);
                if (image != null) View.Add(image);
            }
        }

        /* --------------------------------------------------------------------- */
        ///
        /// GetFolder
        /// 
        /// <summary>
        /// 出力先フォルダを取得します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private string GetFolder()
        {
            var dialog = new FolderBrowserDialog();
            dialog.Description = Properties.Resources.SaveFolder;
            dialog.SelectedPath = System.IO.Path.GetDirectoryName(Model.Path);
            return (dialog.ShowDialog() == DialogResult.Cancel) ? string.Empty : dialog.SelectedPath;
        }

        #endregion
    }
}
