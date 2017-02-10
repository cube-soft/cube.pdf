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
using System.Reflection;
using System.Linq;
using System.Windows.Forms;
using Cube.Forms.Controls;
using IoEx = System.IO;

namespace Cube.Pdf.App.Picker
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
    public class ThumbnailPresenter : PresenterBase<ThumbnailForm, ImageCollection>
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
        public ThumbnailPresenter(ThumbnailForm view, ImageCollection model, EventAggregator events)
            : base(view, model, events)
        {
            View.Shown      += View_Shown;
            View.FormClosed += View_FormClosed;
            View.FileName = IoEx.Path.GetFileNameWithoutExtension(Model.Path);
        }

        #endregion

        #region Event handlers

        #region EventAggregator

        /* --------------------------------------------------------------------- */
        ///
        /// PreviewImage_Handle
        /// 
        /// <summary>
        /// 選択した抽出画像のプレビュー画面を表示する時に実行されるハンドラです。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private void PreviewImage_Handle(object sender, EventArgs e)
            => SyncWait(() =>
        {
            var indices = View.SelectedIndices;
            if (indices == null || indices.Count <= 0) return;

            var index = indices[0];
            var filename = IoEx.Path.GetFileNameWithoutExtension(Model.Path);
            var dialog = new PreviewForm();
            dialog.FileName = $"{filename} ({index}/{Model.Count})";
            dialog.Image = Model[index];
            dialog.ShowDialog();
        });

        /* --------------------------------------------------------------------- */
        ///
        /// Remove_Handle
        /// 
        /// <summary>
        /// 選択した抽出画像を削除する時に実行されるハンドラです。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private async void Remove_Handle(object sender, EventArgs e)
        {
            int[] indices = SyncWait(() => View.SelectedIndices.Descend().ToArray());
            if (indices == null || indices.Length <= 0) return;

            await Async(() => { foreach (var index in indices) Model.RemoveAt(index); });
        }

        /* --------------------------------------------------------------------- */
        ///
        /// SaveComplete_Handle
        /// 
        /// <summary>
        /// 保存処理が完了した時に実行されるハンドラです。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private void SaveComplete_Handle(object sender, EventArgs e)
           => SyncWait(() =>
        {
            View.Complete = true;
            View.Close();
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Version_Handle
        ///
        /// <summary>
        /// バージョン情報を表示するイベントが発生した時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Version_Handle(object sender, EventArgs e)
            => SyncWait(() => Dialogs.Version(Assembly.GetEntryAssembly()));

        #endregion

        #region View

        /* --------------------------------------------------------------------- */
        ///
        /// View_Shown
        /// 
        /// <summary>
        /// フォームの表示直後に実行されるハンドラです。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private void View_Shown(object sender, EventArgs e)
        {
            View.Aggregator = Events;
            Events.PreviewImage.Handle += PreviewImage_Handle;
            Events.Remove.Handle += Remove_Handle;
            Events.SaveComplete.Handle += SaveComplete_Handle;
            Events.Version.Handle += Version_Handle;

            View.Cursor = Cursors.WaitCursor;
            View.AddRange(Model.Select(x => Shrink(x, View.ImageSize)));
            View.Cursor = Cursors.Default;
        }

        /* --------------------------------------------------------------------- */
        ///
        /// View_FormClosed
        /// 
        /// <summary>
        /// フォームが閉じられた時に実行されるハンドラです。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private void View_FormClosed(object sender, FormClosedEventArgs e)
        {
            View.Aggregator = null;
            Events.PreviewImage.Handle -= PreviewImage_Handle;
            Events.Remove.Handle -= Remove_Handle;
            Events.SaveComplete.Handle -= SaveComplete_Handle;
            Events.Version.Handle -= Version_Handle;
        }

        #endregion

        #endregion

        #region Others

        /* --------------------------------------------------------------------- */
        ///
        /// Shrink
        /// 
        /// <summary>
        /// 画像を縮小します。
        /// </summary>
        /// 
        /// <remarks>
        /// TODO: モデルに移譲
        /// </remarks>
        ///
        /* --------------------------------------------------------------------- */
        private System.Drawing.Image Shrink(System.Drawing.Image src, System.Drawing.Size size)
            => new Cube.Images.ImageResizer(src)
            {
                PreserveAspectRatio = true,
                ShrinkOnly          = true,
                LongSide            = size.Width,
            }.Resized;

        #endregion
    }
}
