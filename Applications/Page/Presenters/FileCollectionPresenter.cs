/* ------------------------------------------------------------------------- */
///
/// FileCollectionPresenter.cs
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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.Linq;
using System.Windows.Forms;
using Cube.Log;
using Cube.Forms.Controls;

namespace Cube.Pdf.App.Page
{
    /* --------------------------------------------------------------------- */
    ///
    /// FileCollectionPresenter
    ///
    /// <summary>
    /// MainForm と FileCollection を対応付けるためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class FileCollectionPresenter : PresenterBase<FileListView, FileCollection>
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
        public FileCollectionPresenter(FileListView view, FileCollection model, EventAggregator events)
            : base(view, model, events)
        {
            Events.Preview.Handle += Preview_Handle;
            Events.Add.Handle     += Add_Handle;
            Events.Remove.Handle  += Remove_Handle;
            Events.Clear.Handle   += Clear_Handle;
            Events.Move.Handle    += View_MoveRequired;
            Events.Merge.Handle   += Merge_Handle;
            Events.Split.Handle   += Split_Handle;
            Events.Version.Handle += Version_Handle;

            View.Added                += (s, e) => Refresh();
            View.Removed              += (s, e) => Refresh();
            View.Cleared              += (s, e) => Refresh();
            View.SelectedIndexChanged += (s, e) => Refresh();
            View.MouseDoubleClick     += (s, e) => Events.Preview.Raise();

            Model.CollectionChanged += Model_CollectionChanged;
            Model.PasswordRequired  += Model_PasswordRequired;

            var reader = new AssemblyReader(Assembly.GetEntryAssembly());
            Model.Metadata.Version = new Version(1, 7);
            Model.Metadata.Creator = reader.Product;
        }

        #endregion

        #region Event handlers

        #region Events

        /* --------------------------------------------------------------------- */
        ///
        /// Preview_Handle
        /// 
        /// <summary>
        /// 選択項目のプレビュー要求が発生した時に実行されるハンドラです。
        /// 選択されている項目の内、最初の項目を既定のプログラムで開きます。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private void Preview_Handle(object sender, EventArgs e)
            => this.LogException(()
            => Sync(() =>
        {
            var indices = View.SelectedIndices;
            var index = (indices.Count > 0) ? indices[0] : -1;
            if (index < 0 || index >= Model.Count) return;
            System.Diagnostics.Process.Start(Model[index].FullName);
        }));

        /* --------------------------------------------------------------------- */
        ///
        /// Add_Handle
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
        private void Add_Handle(object sender, ValueEventArgs<string[]> e)
            => Execute(async () =>
        {
            var files = GetFiles(e.Value as string[]);
            if (files == null || files.Length == 0) return;
            await Async(() => Model.Add(files, 1));
        });

        /* --------------------------------------------------------------------- */
        ///
        /// Remove_Handle
        /// 
        /// <summary>
        /// 項目の削除要求が発生した時に実行されるハンドラです。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private void Remove_Handle(object sender, EventArgs e)
            => Execute(async () =>
        {
            int[] indices = null;
            SyncWait(() => indices = View.SelectedIndices.Descend().ToArray());
            if (indices == null || indices.Length == 0) return;
            await Async(() => { foreach (var index in indices) Model.RemoveAt(index); });
        });

        /* --------------------------------------------------------------------- */
        ///
        /// Clear_Handle
        /// 
        /// <summary>
        /// 全項目の削除要求が発生した時に実行されるハンドラです。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private void Clear_Handle(object sender, EventArgs e)
            => Execute(async () => await Async(() => Model.Clear()));

        /* --------------------------------------------------------------------- */
        ///
        /// View_MoveRequired
        /// 
        /// <summary>
        /// 項目の移動要求が発生した時に実行されるハンドラです。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private void View_MoveRequired(object sender, ValueEventArgs<int> e)
            => Execute(async () =>
        {
            int[] indices = null;
            SyncWait(() => indices = View.SelectedIndices.Ascend().ToArray());
            if (indices == null || indices.Length == 0) return;
            await Async(() => Model.Move(indices, e.Value));
        });

        /* --------------------------------------------------------------------- */
        ///
        /// Merge_Handle
        /// 
        /// <summary>
        /// ファイルの結合要求が発生した時に実行されるハンドラです。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private void Merge_Handle(object sender, EventArgs e)
            => Execute(async () =>
        {
            var dest = GetMergeFile();
            if (string.IsNullOrEmpty(dest)) return;

            await Async(() => Model.Merge(dest));

            var message = string.Format(Properties.Resources.MergeSuccess, Model.Count);
            Model.Clear();
            PostProcess(new string[] { dest }, message);
        });

        /* --------------------------------------------------------------------- */
        ///
        /// Split_Handle
        /// 
        /// <summary>
        /// ファイルの分割要求が発生した時に実行されるハンドラです。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private void Split_Handle(object sender, EventArgs e)
            => Execute(async () =>
        {
            var dest = GetSplitFolder();
            if (string.IsNullOrEmpty(dest)) return;

            var results = new List<string>();
            await Async(() => Model.Split(dest, results));

            var message = string.Format(Properties.Resources.SplitSuccess, Model.Count);
            Model.Clear();
            PostProcess(results.ToArray(), message);
        });

        /* --------------------------------------------------------------------- */
        ///
        /// Version_Handle
        /// 
        /// <summary>
        /// バージョン情報の表示要求が発生した時に実行されるハンドラです。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private void Version_Handle(object sender, EventArgs e)
            => SyncWait(() =>
        {
            var dialog = new Cube.Forms.VersionForm();
            dialog.Assembly = Assembly.GetExecutingAssembly();
            dialog.Version.Digit = 3;
            dialog.Logo = Properties.Resources.Logo;
            dialog.Description = string.Empty;
            dialog.Height = 250;
            dialog.ShowInTaskbar = false;
            dialog.ShowDialog();
        });

        #endregion

        #region Model

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
            => SyncWait(() =>
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    View.Insert(e.NewStartingIndex, Model[e.NewStartingIndex]);
                    break;
                case NotifyCollectionChangedAction.Move:
                    View.MoveItems(new int[] { e.OldStartingIndex }, e.NewStartingIndex - e.OldStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    View.RemoveItems(new int[] { e.OldStartingIndex });
                    break;
                case NotifyCollectionChangedAction.Reset:
                    View.ClearItems();
                    if (Model.Count == 0) break;
                    foreach (var item in Model) View.Add(item);
                    break;
                default:
                    break;
            }
        });

        /* --------------------------------------------------------------------- */
        ///
        /// Model_PasswordRequired
        /// 
        /// <summary>
        /// パスワードの要求が発生した時に実行されるハンドラです。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private void Model_PasswordRequired(object sender, QueryEventArgs<string, string> e)
            => SyncWait(() =>
        {
            var dialog = new PasswordForm();
            dialog.Path = e.Query;
            dialog.StartPosition = FormStartPosition.CenterParent;
            var result = dialog.ShowDialog(View);

            e.Cancel = (dialog.DialogResult == DialogResult.Cancel);
            if (!e.Cancel) e.Result = dialog.Password;
        });

        #endregion

        #endregion

        #region Others

        /* --------------------------------------------------------------------- */
        ///
        /// Execute
        /// 
        /// <summary>
        /// 処理を実行します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private void Execute(Action action)
        {
            if (action == null) return;

            try
            {
                SyncWait(() => View.AllowOperation = false);
                action();
            }
            catch (Exception err)
            {
                this.LogError(err.Message, err);
                ShowMessage(err);
            }
            finally { SyncWait(() => View.AllowOperation = true); }
        }

        /* --------------------------------------------------------------------- */
        ///
        /// GetFiles
        /// 
        /// <summary>
        /// 追加するファイルを取得します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private string[] GetFiles(string[] src)
        {
            if (src != null && src.Length > 0) return src;

            string[] dest = null;
            SyncWait(() =>
            {
                var dialog = new OpenFileDialog();
                dialog.CheckFileExists = true;
                dialog.Multiselect = true;
                dialog.Title = Properties.Resources.OpenFileTitle;
                dialog.Filter = Properties.Resources.OpenFileFilter;
                if (dialog.ShowDialog() == DialogResult.Cancel) return;
                dest = dialog.FileNames;
            });
            return dest;
        }

        /* --------------------------------------------------------------------- */
        ///
        /// GetMergeFile
        /// 
        /// <summary>
        /// 結合したファイルの保存先を取得します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private string GetMergeFile()
        {
            var dest = string.Empty;
            SyncWait(() =>
            {
                var dialog = new SaveFileDialog();
                dialog.OverwritePrompt = true;
                dialog.Title = Properties.Resources.MergeTitle;
                dialog.Filter = Properties.Resources.SaveFileFilter;
                if (dialog.ShowDialog() == DialogResult.Cancel) return;
                dest = dialog.FileName;
            });
            return dest;
        }

        /* --------------------------------------------------------------------- */
        ///
        /// GetSplitFolder
        /// 
        /// <summary>
        /// 分割したファイルの保存先を取得します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private string GetSplitFolder()
        {
            var dest = string.Empty;
            SyncWait(() =>
            {
                var dialog = new FolderBrowserDialog();
                dialog.Description = Properties.Resources.SplitDescription;
                dialog.ShowNewFolderButton = true;
                if (dialog.ShowDialog() == DialogResult.Cancel) return;
                dest = dialog.SelectedPath;
            });
            return dest;
        }

        /* --------------------------------------------------------------------- */
        ///
        /// Refresh
        /// 
        /// <summary>
        /// View を再描画します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private void Refresh()
            => Sync(() =>
        {
            var form = View.FindForm();
            if (form == null) return;
            form.Refresh();
        });

        /* --------------------------------------------------------------------- */
        ///
        /// PostProcess
        /// 
        /// <summary>
        /// 終了時に行う処理を UI スレッドで実行します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private void PostProcess(string[] files, string message)
            => SyncWait(() =>
        {
            var result = MessageBox.Show(message, Properties.Resources.MessageTitle,
            MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.No) return;

            Add_Handle(this, new ValueEventArgs<string[]>(files));
        });

        /* --------------------------------------------------------------------- */
        ///
        /// ShowMessage
        /// 
        /// <summary>
        /// メッセージをメッセージボックスに表示します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private void ShowMessage(string message, MessageBoxIcon icon)
            => SyncWait(() =>
        {
            MessageBox.Show(message, 
                Properties.Resources.MessageTitle,
                MessageBoxButtons.OK,
                icon
            );
        });

        /* --------------------------------------------------------------------- */
        ///
        /// ShowMessage
        /// 
        /// <summary>
        /// 例外メッセージをメッセージボックスに表示します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private void ShowMessage(Exception err)
            => SyncWait(() =>
        {
            MessageBox.Show(err.Message,
                Properties.Resources.ErrorMessageTitle,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        });

        #endregion
    }
}
