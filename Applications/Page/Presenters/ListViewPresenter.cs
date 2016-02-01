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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
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
            View.AddRequired   += View_AddRequired;
            View.MergeRequired += View_MergeRequired;
            View.SplitRequired += View_SplitRequired;

            View.Removing  += View_Removing;
            View.Clearing  += View_Clearing;
            View.Moving    += View_Moving;
            View.Previewed += View_Previewed;

            Model.CollectionChanged += Model_CollectionChanged;
            Model.PasswordRequired += Model_PasswordRequired;
        }

        #endregion

        #region Event handlers

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
        private void View_AddRequired(object sender, DataEventArgs<string[]> e)
        {
            Execute(async () => await Async(() => Model.Add(e.Value, 1)));
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
        private void View_MergeRequired(object sender, DataEventArgs<string> e)
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
        private void View_SplitRequired(object sender, DataEventArgs<string> e)
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

        /* --------------------------------------------------------------------- */
        ///
        /// View_Previewed
        /// 
        /// <summary>
        /// 項目を既定のプログラムで開きます。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private void View_Previewed(object sender, EventArgs e)
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
            SyncWait(() =>
            {
                var dialog = new PasswordForm();
                dialog.Path = e.Path;
                dialog.StartPosition = FormStartPosition.CenterParent;
                var result = dialog.ShowDialog(View);

                e.Cancel = (dialog.DialogResult == DialogResult.Cancel);
                if (!e.Cancel) e.Password = dialog.Password;
            });
        }

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
            catch (Exception err) { ShowMessage(err); }
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

                View_AddRequired(this, new DataEventArgs<string[]>(files));
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
