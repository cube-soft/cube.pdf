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
    public class FileCollectionPresenter : Cube.Forms.PresenterBase<MainForm, FileCollection>
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
        public FileCollectionPresenter(MainForm view, FileCollection model)
            : base(view, model)
        {
            View.PreviewRequired += View_PreviewRequired;
            View.AddRequired     += View_AddRequired;
            View.RemoveRequired  += View_RemoveRequired;
            View.ClearRequired   += View_ClearRequired;
            View.MoveRequired    += View_MoveRequired;
            View.MergeRequired   += View_MergeRequired;
            View.SplitRequired   += View_SplitRequired;

            Model.CollectionChanged += Model_CollectionChanged;
            Model.PasswordRequired += Model_PasswordRequired;

            var reader = new AssemblyReader(Assembly.GetEntryAssembly());
            Model.Metadata.Version = new Version(1, 7);
            Model.Metadata.Creator = reader.Product;
        }

        #endregion

        #region Event handlers

        #region View

        /* --------------------------------------------------------------------- */
        ///
        /// View_PreviewRequired
        /// 
        /// <summary>
        /// 項目を既定のプログラムで開きます。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private void View_PreviewRequired(object sender, EventArgs e)
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
        /// View_AddRequired
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
        private void View_AddRequired(object sender, ValueEventArgs<string[]> e)
        {
            Execute(async () => await Async(() => Model.Add(e.Value, 1)));
        }

        /* --------------------------------------------------------------------- */
        ///
        /// View_RemoveRequired
        /// 
        /// <summary>
        /// 項目の削除要求が発生した時に実行されるハンドラです。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private void View_RemoveRequired(object sender, EventArgs e)
        {
            foreach (var index in View.SelectedIndices.Reverse()) Model.RemoveAt(index);
        }

        /* --------------------------------------------------------------------- */
        ///
        /// View_ClearRequired
        /// 
        /// <summary>
        /// 全項目の削除要求が発生した時に実行されるハンドラです。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private async void View_ClearRequired(object sender, EventArgs e)
        {
            await Async(() => Model.Clear());
        }

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
        {
            var indices = View.SelectedIndices;
            if (indices.Count == 0) return;
            Model.Move(indices, e.Value);
        }

        /* --------------------------------------------------------------------- */
        ///
        /// View_MergeRequired
        /// 
        /// <summary>
        /// ファイルの結合要求が発生した時に実行されるハンドラです。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private void View_MergeRequired(object sender, ValueEventArgs<string> e)
        {
            Execute(async () =>
            {
                await Async(() => Model.Merge(e.Value));

                var message = string.Format(Properties.Resources.MergeSuccess, Model.Count);
                Model.Clear();
                PostProcess(new string[] { e.Value }, message);
            });
        }

        /* --------------------------------------------------------------------- */
        ///
        /// View_SplitRequired
        /// 
        /// <summary>
        /// ファイルの分割要求が発生した時に実行されるハンドラです。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private void View_SplitRequired(object sender, ValueEventArgs<string> e)
        {
            Execute(async () =>
            {
                var results = new List<string>();
                await Async(() => Model.Split(e.Value, results));

                var message = string.Format(Properties.Resources.SplitSuccess, Model.Count);
                Model.Clear();
                PostProcess(results.ToArray(), message);
            });
        }

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
        {
            SyncWait(() =>
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        View.Insert(e.NewStartingIndex, Model[e.NewStartingIndex]);
                        break;
                    case NotifyCollectionChangedAction.Move:
                        View.MoveItem(e.OldStartingIndex, e.NewStartingIndex);
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        View.Remove(e.OldStartingIndex);
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
        private void Model_PasswordRequired(object sender, QueryEventArgs<string, string> e)
        {
            SyncWait(() =>
            {
                var dialog = new PasswordForm();
                dialog.Path = e.Query;
                dialog.StartPosition = FormStartPosition.CenterParent;
                var result = dialog.ShowDialog(View);

                e.Cancel = (dialog.DialogResult == DialogResult.Cancel);
                if (!e.Cancel) e.Result = dialog.Password;
            });
        }

        #endregion

        #endregion

        #region Other private methods

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
        /// PostProcess
        /// 
        /// <summary>
        /// 終了時に行う処理を UI スレッドで実行します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private void PostProcess(string[] files, string message)
        {
            SyncWait(() =>
            {
                var result = MessageBox.Show(message, Properties.Resources.MessageTitle,
                MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (result == DialogResult.No) return;

                View_AddRequired(this, new ValueEventArgs<string[]>(files));
            });
        }

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
        /// ShowMessage
        /// 
        /// <summary>
        /// 例外メッセージをメッセージボックスに表示します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private void ShowMessage(Exception err)
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
