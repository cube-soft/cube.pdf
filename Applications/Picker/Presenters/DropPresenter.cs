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
using IoEx = System.IO;

namespace Cube.Pdf.Picker.App
{
    /* --------------------------------------------------------------------- */
    ///
    /// DropPresenter
    ///
    /// <summary>
    /// DropForm とモデルを関連付けるクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class DropPresenter : Cube.Forms.PresenterBase<DropForm, object>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// DropPresenter
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public DropPresenter(DropForm view)
            : base(view, null)
        {
            View.Open += View_Open;
        }

        #endregion

        #region Event handlers

        /* ----------------------------------------------------------------- */
        ///
        /// Open_Handle
        ///
        /// <summary>
        /// ファイルを開くイベントが発生した時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void View_Open(object sender, ValueEventArgs<string[]> e)
        {
            if (e.Value == null) return;
            foreach (var path in e.Value) OpenProgress(path);
        }

        #endregion

        #region Others

        /* ----------------------------------------------------------------- */
        ///
        /// OpenProgress
        ///
        /// <summary>
        /// ProgressForm を表示します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void OpenProgress(string path) => Sync(() =>
        {
            var ext = IoEx.Path.GetExtension(path).ToLower();
            if (!ContainsExtension(ext) || !IoEx.File.Exists(path)) return;

            var view = new ProgressForm();
            new ProgressPresenter(
                view,
                new ImageCollection(path),
                new Aggregator()
            );
            view.Show();
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Open
        ///
        /// <summary>
        /// PDF ファイルを開いて解析するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private bool ContainsExtension(string ext)
            => SyncWait(() => View.AllowExtensions.Contains(ext));

        #endregion
    }
}
