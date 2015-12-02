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
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using IoEx = System.IO;

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
    public class ListViewPresenter : PresenterBase<MainForm, ItemCollection>
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
            SynchronizationContext = SynchronizationContext.Current;
            
            View.Adding    += View_Adding;
            View.Removing  += View_Removing;
            View.Clearing  += View_Clearing;
            View.Moving    += View_Moving;
            View.Merging   += View_Merging;
            View.Splitting += View_Splitting;

            Model.CollectionChanged += Model_CollectionChanged;
        }

        #endregion

        #region Properties

        /* --------------------------------------------------------------------- */
        ///
        /// SynchronizationContext
        /// 
        /// <summary>
        /// オブジェクト初期化時のコンテキストを取得します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        public SynchronizationContext SynchronizationContext { get; }

        #endregion

        #region Event handlers

        /* --------------------------------------------------------------------- */
        ///
        /// View_Adding
        /// 
        /// <summary>
        /// ファイルの追加要求が発生した時に実行されるハンドラです。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private async void View_Adding(object sender, DataEventArgs<string[]> e)
        {
            try {
                Sync(() => { View.Cursor = Cursors.WaitCursor; });

                foreach (var path in e.Value)
                {
                    if (IoEx.Directory.Exists(path) || Model.Contains(path)) continue;
                    await Model.AddAsync(path);
                }
            }
            catch (Exception err) { ShowSync(err); }
            finally { Sync(() => { View.Cursor = Cursors.Default; }); }
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
            try {
                Sync(() => { View.Cursor = Cursors.WaitCursor; });

                var binder = new Cube.Pdf.Editing.PageBinder();
                foreach (var item in Model)
                {
                    if (item.Type == PageType.Pdf) AddPdf(item, binder);
                    else if (item.Type == PageType.Image) AddImage(item, binder);
                }
                await binder.SaveAsync(e.Value);

                Model.Clear();
            }
            catch (Exception err) { ShowSync(err); }
            finally { Sync(() => { View.Cursor = Cursors.Default; }); }
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
        private void View_Splitting(object sender, DataEventArgs<string> e)
        {
            try
            {
                Sync(() => { View.Cursor = Cursors.WaitCursor; });
            }
            finally { Sync(() => { View.Cursor = Cursors.Default; }); }
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
        /// AddPdf
        /// 
        /// <summary>
        /// PDF ファイルを追加します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private void AddPdf(Item src, Cube.Pdf.Editing.PageBinder dest)
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
        private void AddImage(Item src, Cube.Pdf.Editing.PageBinder dest)
        {
            var page = new ImagePage();
            page.Path = src.Path;
            page.Size = src.ViewSize;
            dest.Pages.Add(page);
        }

        /* --------------------------------------------------------------------- */
        ///
        /// Sync
        /// 
        /// <summary>
        /// 指定された Action を UI スレッドで実行します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private void Sync(Action action)
        {
            SynchronizationContext.Post(_ => action(), null);
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
                    Properties.Resources.ErrorTitle,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            });
        }

        #endregion
    }
}
