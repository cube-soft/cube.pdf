/* ------------------------------------------------------------------------- */
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
using System.Threading.Tasks;
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
    /// FileListView と FileCollection を対応付けるためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class FileCollectionPresenter
        : Cube.Forms.PresenterBase<FileListView, FileCollection, Settings>
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
        public FileCollectionPresenter(FileListView view, FileCollection model,
            Settings settings, IEventAggregator events)
            : base(view, model, settings, events)
        {
            EventAggregator.GetEvents()?.Preview.Subscribe(Preview_Handle);
            EventAggregator.GetEvents()?.Add.Subscribe(Add_Handle);
            EventAggregator.GetEvents()?.Remove.Subscribe(Remove_Handle);
            EventAggregator.GetEvents()?.Clear.Subscribe(Clear_Handle);
            EventAggregator.GetEvents()?.Move.Subscribe(Move_Handle);
            EventAggregator.GetEvents()?.Merge.Subscribe(Merge_Handle);
            EventAggregator.GetEvents()?.Split.Subscribe(Split_Handle);

            View.Added                += (s, e) => EventAggregator.GetEvents()?.Refresh.Publish();
            View.Removed              += (s, e) => EventAggregator.GetEvents()?.Refresh.Publish();
            View.Cleared              += (s, e) => EventAggregator.GetEvents()?.Refresh.Publish();
            View.SelectedIndexChanged += (s, e) => EventAggregator.GetEvents()?.Refresh.Publish();
            View.MouseDoubleClick     += (s, e) => EventAggregator.GetEvents()?.Preview.Publish();

            Model.CollectionChanged += Model_CollectionChanged;
            Model.PasswordRequired  += Model_PasswordRequired;

            var reader = new AssemblyReader(Settings.Assembly);
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
        private void Preview_Handle()
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
        private async void Add_Handle(string[] value)
            => await ExecuteAsync(() =>
        {
            var files = GetFiles(value);
            if (files == null || files.Length == 0) return;
            Model.Add(files, 1);
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
        private async void Remove_Handle()
            => await ExecuteAsync(() =>
        {
            var indices = SyncWait(() => View.SelectedIndices.Descend().ToArray());
            if (indices == null || indices.Length == 0) return;
            foreach (var index in indices) Model.RemoveAt(index);
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
        private async void Clear_Handle()
            => await ExecuteAsync(() => Model.Clear());

        /* --------------------------------------------------------------------- */
        ///
        /// Move_Handle
        /// 
        /// <summary>
        /// 項目の移動要求が発生した時に実行されるハンドラです。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private async void Move_Handle(int value)
            => await ExecuteAsync(() =>
        {
            var indices = SyncWait(() => View.SelectedIndices.Ascend().ToArray());
            if (indices == null || indices.Length == 0) return;
            Model.Move(indices, value);
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
        private async void Merge_Handle()
            => await ExecuteAsync(() =>
        {
            var dest = GetMergeFile();
            if (string.IsNullOrEmpty(dest)) return;

            this.LogDebug($"Merge:{Model.Count}\tDest:{dest}");
            Model.Merge(dest);

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
        private async void Split_Handle()
            => await ExecuteAsync(() =>
        {
            var dest = GetSplitFolder();
            if (string.IsNullOrEmpty(dest)) return;

            this.LogDebug($"Split:{Model.Count}\tDest:{dest}");
            var results = new List<string>();
            Model.Split(dest, results);

            var message = string.Format(Properties.Resources.SplitSuccess, Model.Count);
            Model.Clear();
            PostProcess(results.ToArray(), message);
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
            var dialog = Views.CreatePasswordView(e.Query);
            var result = dialog.ShowDialog(View);
            e.Cancel = (dialog.DialogResult == DialogResult.Cancel);
            if (!e.Cancel) e.Result = dialog.Password;
        });

        #endregion

        #endregion

        #region Others

        /* --------------------------------------------------------------------- */
        ///
        /// ExecuteAsync
        /// 
        /// <summary>
        /// 処理を実行します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private async Task ExecuteAsync(Action action)
        {
            if (action == null) return;

            try
            {
                Settings.AllowOperation = false;
                await Async(() => action());
            }
            catch (Exception err)
            {
                this.LogError(err.Message, err);
                Views.ShowErrorMessage(err);
            }
            finally { Settings.AllowOperation = true; }
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
            return SyncWait(() =>
            {
                var dialog = Views.CreateAddView();
                if (dialog.ShowDialog() == DialogResult.Cancel) return null;
                return dialog.FileNames;
            });
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
            =>  SyncWait(() =>
        {
            var dialog = Views.CreateMergeView();
            if (dialog.ShowDialog() == DialogResult.Cancel) return string.Empty;
            return dialog.FileName;
        });

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
            => SyncWait(() =>
        {
            var dialog = Views.CreateSplitView();
            if (dialog.ShowDialog() == DialogResult.Cancel) return string.Empty;
            return dialog.SelectedPath;
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
            var result = Views.ShowConfirmMessage(message);
            if (result == DialogResult.No) return;
            Add_Handle(files);
        });

        #endregion
    }
}
