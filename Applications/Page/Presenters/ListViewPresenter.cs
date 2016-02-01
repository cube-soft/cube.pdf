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
    public class ListViewPresenter : Cube.Forms.PresenterBase<MainForm, FileCollection>
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
        public ListViewPresenter(MainForm view, FileCollection model)
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
            Model.PasswordRequired += Model_PasswordRequired;
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
        private void View_Adding(object sender, DataEventArgs<string[]> e)
        {
            try
            {
                SyncWait(() => { View.AllowOperation = false; });
                AddFile(e.Value, 1); // 1 階層下のみ対象
            }
            finally { SyncWait(() => { View.AllowOperation = true; }); }
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
        private void View_Merging(object sender, DataEventArgs<string> e)
        {
            try
            {
                SyncWait(() => { View.AllowOperation = false; ; });

                Metadata metadata = null;
                var writer = new Cube.Pdf.Editing.DocumentWriter();
                foreach (var item in Model)
                {
                    if (item is File)
                    {
                        AddPdf(item as File, writer);
                        //if (metadata == null) metadata = reader.Metadata;
                    }
                    else AddImage(item as ImageFile, writer);
                }
                
                if (metadata == null)
                {
                    metadata = new Metadata();
                    metadata.Version = new Version(1, 7);
                }
                writer.Metadata = metadata;
                writer.Metadata.Creator = Application.ProductName;
                writer.Save(e.Value);

                var message = string.Format(Properties.Resources.MergeSuccess, Model.Count);
                Model.Clear();
                FinalizeSync(new string[] { e.Value }, message);
            }
            catch (Exception err) { ShowSync(err); }
            finally { SyncWait(() => { View.AllowOperation = true; }); }
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
                SyncWait(() => { View.AllowOperation = false; });

                var results = new List<string>();
                var writer = new Cube.Pdf.Editing.PageSplitter();
                writer.Metadata.Version = new Version(1, 7);
                writer.Metadata.Creator = Application.ProductName;

                foreach (var item in Model)
                {
                    if (item is File) AddPdf(item as File, writer);
                    else AddImage(item as ImageFile, writer);
                }
                writer.Save(e.Value, results);

                var message = string.Format(Properties.Resources.SplitSuccess, Model.Count);
                Model.Clear();
                FinalizeSync(results.ToArray(), message);
            }
            catch (Exception err) { ShowSync(err); }
            finally { SyncWait(() => { View.AllowOperation = true; }); }
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
            SyncWait(() =>
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

        /* --------------------------------------------------------------------- */
        ///
        /// Model_PasswordRequired
        /// 
        /// <summary>
        /// パスワードの要求が発生した時に実行されるハンドラです。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private void Model_PasswordRequired(object sender, PasswordEventArgs e)
        {
            var dialog = new PasswordForm();
            dialog.Path = e.Path;
            dialog.StartPosition = FormStartPosition.CenterParent;
            var result = dialog.ShowDialog(View);

            e.Cancel = (dialog.DialogResult == DialogResult.Cancel);
            if (!e.Cancel) e.Password = dialog.Password;
        }

        #endregion

        #region Other private methods

        /* --------------------------------------------------------------------- */
        ///
        /// AddFile
        /// 
        /// <summary>
        /// ファイルを追加します。
        /// </summary>
        /// 
        /// <remarks>
        /// 追加不可能なファイルに関しては読み飛ばします。
        /// ただし、パスワード付の PDF ファイルに関しては要対応。
        /// </remarks>
        ///
        /* --------------------------------------------------------------------- */
        private void AddFile(string[] files, int hierarchy)
        {
            foreach (var path in files)
            {
                if (Model.Contains(path)) continue;
                if (Directory.Exists(path))
                {
                    if (hierarchy > 0) AddFile(Directory.GetFiles(path), hierarchy - 1);
                    continue;
                }
                else if (!IoEx.File.Exists(path)) continue;

                try { Model.Add(path); }
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
        private void AddPdf(File src, IDocumentWriter dest)
        {
            if (src == null) return;

            using (var reader = new Cube.Pdf.Editing.DocumentReader())
            {
                reader.PasswordRequired += (s, e) => { e.Cancel = true; };
                reader.Open(src.FullName, src.Password, true);
                foreach (var page in reader.Pages) dest.Pages.Add(page);
            }            
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
        private void AddImage(ImageFile src, IDocumentWriter dest)
        {
            if (src == null) return;
            foreach (var page in ImagePage.Create(src.FullName)) dest.Pages.Add(page);
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
            SyncWait(() =>
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
            SyncWait(() =>
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
            SyncWait(() =>
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
