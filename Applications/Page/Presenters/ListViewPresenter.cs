/* ------------------------------------------------------------------------- */
///
/// ListViewPresenter.cs
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
using System.IO;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cube.Pdf.App.Page
{
    /* --------------------------------------------------------------------- */
    ///
    /// Cube.Pdf.App.Page.ListViewPresenter
    ///
    /// <summary>
    /// MainForm とファイルリストを対応付けるためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ListViewPresenter : Cube.Forms.PresenterBase<MainForm, ItemCollection>
    {
        #region Constructors

        /* --------------------------------------------------------------------- */
        ///
        /// ListViewPresenter
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        public ListViewPresenter(MainForm view, ItemCollection model)
            : base(view, model)
        {
            View.Adding    += View_Adding;
            View.Removing  += View_Removing;
            View.Clearing  += View_Clearing;
            View.Moving    += View_Moving;
            View.Merging   += View_Merging;
            View.Splitting += View_Splitting;
            View.Opening   += View_Opening;

            Model.CollectionChanged += Model_CollectionChanged;
        }

        #endregion

        #region Event handlers

        /* --------------------------------------------------------------------- */
        ///
        /// View_Opening
        /// 
        /// <summary>
        /// 項目を開く時に実行されるハンドラです。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private void View_Opening(object sender, EventArgs e)
        {
            try
            {
                var indices = View.SelectedIndices;
                var index = (indices.Count > 0) ? indices[0] : -1;
                if (index < 0 || index >= Model.Count) return;
                System.Diagnostics.Process.Start(Model[index].FullName);
            }
            catch (Exception /* err */) { /* ignore errors */ }
        }

        /* --------------------------------------------------------------------- */
        ///
        /// View_Adding
        /// 
        /// <summary>
        /// ファイルの追加要求が発生した時に実行されるハンドラです。
        /// </summary>
        /// 
        /// <remarks>
        /// フォルダを指定された場合、直下のファイルのみを対象とします。
        /// </remarks>
        ///
        /* --------------------------------------------------------------------- */
        private async void View_Adding(object sender, DataEventArgs<string[]> e)
        {
            try
            {
                Sync(() => { View.AllowOperation = false; });
                await AddFileAsync(e.Value, 1); // 1 階層下のみ対象
            }
            finally { Sync(() => { View.AllowOperation = true; }); }
        }

        /* --------------------------------------------------------------------- */
        ///
        /// View_Removing
        /// 
        /// <summary>
        /// 項目からの削除要求が発生した時に実行されるハンドラです。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private void View_Removing(object sender, EventArgs e)
        {
            foreach (var index in View.SelectedIndices.Reverse()) Model.RemoveAt(index);
        }

        /* --------------------------------------------------------------------- */
        ///
        /// View_Clearing
        /// 
        /// <summary>
        /// 全項目の削除要求が発生した時に実行されるハンドラです。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private void View_Clearing(object sender, EventArgs e)
        {
            Model.Clear();
        }

        /* --------------------------------------------------------------------- */
        ///
        /// View_Moving
        /// 
        /// <summary>
        /// 項目の移動要求が発生した時に実行されるハンドラです。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private void View_Moving(object sender, DataEventArgs<int> e)
        {
            var indices = View.SelectedIndices;
            if (indices.Count == 0) return;
            Model.Move(indices, e.Value);
        }

        /* --------------------------------------------------------------------- */
        ///
        /// View_Merging
        /// 
        /// <summary>
        /// ファイルの結合要求が発生した時に実行されるハンドラです。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private async void View_Merging(object sender, DataEventArgs<string> e)
        {
            try
            {
                Sync(() => { View.AllowOperation = false; ; });

                var task = new Cube.Pdf.Editing.PageBinder();
                foreach (var item in Model)
                {
                    if (item.Type == PageType.Pdf) AddPdf(item, task);
                    else if (item.Type == PageType.Image) AddImage(item, task);
                }
                await task.SaveAsync(e.Value);

                var message = string.Format(Properties.Resources.MergeSuccess, Model.Count);
                Model.Clear();
                FinalizeSync(new string[] { e.Value }, message);
            }
            catch (Exception err) { ShowSync(err); }
            finally { Sync(() => { View.AllowOperation = true; }); }
        }

        /* --------------------------------------------------------------------- */
        ///
        /// View_Splitting
        /// 
        /// <summary>
        /// ファイルの分割要求が発生した時に実行されるハンドラです。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private async void View_Splitting(object sender, DataEventArgs<string> e)
        {
            try
            {
                Sync(() => { View.AllowOperation = false; });

                var results = new List<string>();
                var task = new Cube.Pdf.Editing.PageSplitter();
                foreach (var item in Model)
                {
                    if (item.Type == PageType.Pdf) AddPdf(item, task);
                    else if (item.Type == PageType.Image) AddImage(item, task);
                }
                await task.SaveAsync(e.Value, results);

                var message = string.Format(Properties.Resources.SplitSuccess, Model.Count);
                Model.Clear();
                FinalizeSync(results.ToArray(), message);
            }
            finally { Sync(() => { View.AllowOperation = true; }); }
        }

        /* --------------------------------------------------------------------- */
        ///
        /// Model_CollectionChanged
        /// 
        /// <summary>
        /// コレクションの内容に変更が発生した時に実行されるハンドラです。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private void Model_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Sync(() =>
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        View.InsertItem(e.NewStartingIndex, Model[e.NewStartingIndex]);
                        break;
                    case NotifyCollectionChangedAction.Move:
                        View.MoveItem(e.OldStartingIndex, e.NewStartingIndex);
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        View.RemoveItem(e.OldStartingIndex);
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        View.ClearItems();
                        if (Model.Count == 0) break;
                        foreach (var item in Model) View.AddItem(item);
                        break;
                    default:
                        break;
                }
            });
        }

        #endregion

        #region Other private methods

        /* --------------------------------------------------------------------- */
        ///
        /// AddFileAsync
        /// 
        /// <summary>
        /// ファイルを非同期で追加します。
        /// </summary>
        /// 
        /// <remarks>
        /// 追加不可能なファイルに関しては読み飛ばします。
        /// ただし、パスワード付の PDF ファイルに関しては要対応。
        /// </remarks>
        ///
        /* --------------------------------------------------------------------- */
        private async Task AddFileAsync(string[] files, int hierarchy)
        {
            foreach (var path in files)
            {
                if (Model.Contains(path)) continue;
                if (Directory.Exists(path))
                {
                    if (hierarchy > 0) await AddFileAsync(Directory.GetFiles(path), hierarchy - 1);
                    continue;
                }
                else if (!File.Exists(path)) continue;

                try { await Model.AddAsync(path); }
                catch (EncryptionException /* err */) { /* see remarks */ }
                catch (Exception /* err */) { /* see remarks */ }
            }
        }

        /* --------------------------------------------------------------------- */
        ///
        /// AddPdf
        /// 
        /// <summary>
        /// PDF ファイルを追加します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private void AddPdf(Item src, IDocumentWriter dest)
        {
            var reader = src.Value as Cube.Pdf.Editing.DocumentReader;
            if (reader == null) return;

            foreach (var page in reader.Pages) dest.Pages.Add(page);
        }

        /* --------------------------------------------------------------------- */
        ///
        /// AddImage
        /// 
        /// <summary>
        /// 画像ファイルを追加します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private void AddImage(Item src, IDocumentWriter dest)
        {
            var page = new ImagePage();
            page.Path = src.FullName;
            dest.Pages.Add(page);
        }

        /* --------------------------------------------------------------------- */
        ///
        /// FinalizeSync
        /// 
        /// <summary>
        /// 終了時に行う処理を UI スレッドで実行します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private void FinalizeSync(string[] files, string message)
        {
            Sync(() =>
            {
                View.AllowOperation = true;

                var result = MessageBox.Show(message, Properties.Resources.MessageTitle,
                MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (result == DialogResult.No) return;

                View_Adding(this, new DataEventArgs<string[]>(files));
            });
        }

        /* --------------------------------------------------------------------- */
        ///
        /// ShowSync
        /// 
        /// <summary>
        /// メッセージをメッセージボックスに表示します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private void ShowSync(string message, MessageBoxIcon icon)
        {
            Sync(() =>
            {
                MessageBox.Show(message, 
                    Properties.Resources.MessageTitle,
                    MessageBoxButtons.OK,
                    icon
                );
            });
        }

        /* --------------------------------------------------------------------- */
        ///
        /// ShowSync
        /// 
        /// <summary>
        /// 例外メッセージをメッセージボックスに表示します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private void ShowSync(Exception err)
        {
            Sync(() =>
            {
                MessageBox.Show(err.Message,
                    Properties.Resources.ErrorMessageTitle,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            });
        }

        #endregion
    }
}
