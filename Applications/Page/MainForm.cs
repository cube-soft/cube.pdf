/* ------------------------------------------------------------------------- */
///
/// MainForm.cs
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
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Windows.Forms;
using Cube.Extensions;
using IoEx = System.IO;

namespace Cube.Pdf.App.Page
{
    /* --------------------------------------------------------------------- */
    ///
    /// Cube.Pdf.App.Page.MainForm
    ///
    /// <summary>
    /// CubePDF Page メイン画面を表示するクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public partial class MainForm : Cube.Forms.NtsForm
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// MainForm
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public MainForm()
        {
            InitializeComponent();
            InitializeLayout();
            InitializePresenters();

            FileButton.Click   += (s, e) => RaiseAddingEvent();
            RemoveButton.Click += (s, e) => OnRemoving(e);
            ClearButton.Click  += (s, e) => OnClearing(e);
            UpButton.Click     += (s, e) => OnMoving(new DataEventArgs<int>(-1));
            DownButton.Click   += (s, e) => OnMoving(new DataEventArgs<int>(1));
            MergeButton.Click  += (s, e) => RaiseMergingEvent();
            SplitButton.Click  += (s, e) => RaiseSplittingEvent();

            ButtonsPanel.DragEnter += Control_DragEnter;
            FooterPanel.DragEnter  += Control_DragEnter;
            PageListView.DragEnter += Control_DragEnter;

            ButtonsPanel.DragDrop  += Control_DragDrop;
            FooterPanel.DragDrop   += Control_DragDrop;
            PageListView.DragDrop  += Control_DragDrop;

            PageListView.SelectedIndexChanged += (s, e) => UpdateControls();
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// AllowOperation
        /// 
        /// <summary>
        /// 各種操作を受け付けるかどうかを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool AllowOperation
        {
            get { return ButtonsPanel.Enabled && FooterPanel.Enabled; }
            set
            {
                ButtonsPanel.Enabled = value;
                FooterPanel.Enabled  = value;
                Cursor = value ? Cursors.Default : Cursors.WaitCursor;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SelectedIndices
        /// 
        /// <summary>
        /// 選択されている項目一覧を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IList<int> SelectedIndices
        {
            get
            {
                var dest = new List<int>();
                foreach (int index in PageListView.SelectedIndices) dest.Add(index);
                dest.Sort();
                return dest;
            }
        }

        #endregion

        #region Events

        /* ----------------------------------------------------------------- */
        ///
        /// Adding
        /// 
        /// <summary>
        /// 新しいファイルの追加時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public EventHandler<DataEventArgs<string[]>> Adding;

        /* ----------------------------------------------------------------- */
        ///
        /// Removing
        /// 
        /// <summary>
        /// 項目を削除する時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public EventHandler Removing;

        /* ----------------------------------------------------------------- */
        ///
        /// Clearing
        /// 
        /// <summary>
        /// 全項目を削除する時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public EventHandler Clearing;

        /* ----------------------------------------------------------------- */
        ///
        /// Moving
        /// 
        /// <summary>
        /// 項目を移動させる時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public EventHandler<DataEventArgs<int>> Moving;

        /* ----------------------------------------------------------------- */
        ///
        /// Merging
        /// 
        /// <summary>
        /// 結合処理の実行時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public EventHandler<DataEventArgs<string>> Merging;

        /* ----------------------------------------------------------------- */
        ///
        /// Splitting
        /// 
        /// <summary>
        /// 分割処理の実行時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public EventHandler<DataEventArgs<string>> Splitting;

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Add
        /// 
        /// <summary>
        /// 項目を追加します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void AddItem(Item item)
        {
            Execute(() => PageListView.Items.Add(Convert(item)));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Insert
        /// 
        /// <summary>
        /// 指定された位置に項目を追加します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void InsertItem(int index, Item item)
        {
            Execute(() =>
            {
                var i = Math.Max(Math.Min(index, PageListView.Items.Count), 0);
                if (i == PageListView.Items.Count) PageListView.Items.Add(Convert(item));
                else PageListView.Items.Insert(i, Convert(item));
            });
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Move
        /// 
        /// <summary>
        /// 項目を移動します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void MoveItem(int oldindex, int newindex)
        {
            Execute(() =>
            {
                if (oldindex < 0 || oldindex >= PageListView.Items.Count) return;

                var item = PageListView.Items[oldindex];
                PageListView.Items.RemoveAt(oldindex);
                var result = PageListView.Items.Insert(newindex, item);
                if (result != null) result.Selected = true;
            });
        }

        /* ----------------------------------------------------------------- */
        ///
        /// RemoveAt
        /// 
        /// <summary>
        /// 指定されたインデックスに対応する項目を削除します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void RemoveItem(int index)
        {
            Execute(() => PageListView.Items.RemoveAt(index));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Clear
        /// 
        /// <summary>
        /// 全ての項目を削除します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void ClearItems()
        {
            Execute(() => PageListView.Items.Clear());
        }

        #endregion

        #region Virtual methods

        /* ----------------------------------------------------------------- */
        ///
        /// OnAdding
        /// 
        /// <summary>
        /// 新しいファイルの追加時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnAdding(DataEventArgs<string[]> e)
        {
            if (Adding != null) Adding(this, e);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnRemoving
        /// 
        /// <summary>
        /// 項目を削除する時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnRemoving(EventArgs e)
        {
            if (Removing != null) Removing(this, e);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnClearing
        /// 
        /// <summary>
        /// 全項目を削除する時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnClearing(EventArgs e)
        {
            if (Clearing != null) Clearing(this, e);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnMoving
        /// 
        /// <summary>
        /// 項目を移動させる時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnMoving(DataEventArgs<int> e)
        {
            if (Moving != null) Moving(this, e);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnMerging
        /// 
        /// <summary>
        /// 結合処理の実行時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnMerging(DataEventArgs<string> e)
        {
            if (Merging != null) Merging(this, e);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnSplitting
        /// 
        /// <summary>
        /// 分割処理の実行時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnSplitting(DataEventArgs<string> e)
        {
            if (Splitting != null) Splitting(this, e);
        }

        #endregion

        #region Override methods

        /* ----------------------------------------------------------------- */
        ///
        /// OnLoad
        /// 
        /// <summary>
        /// フォームのロード時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnLoad(EventArgs e)
        {
            Execute(() => MergeButton.Select());
            base.OnLoad(e);
        }

        #endregion

        #region Event handlers

        /* ----------------------------------------------------------------- */
        ///
        /// Control_DragEnter
        /// 
        /// <summary>
        /// 何らかのコントロールでドラッグ操作が開始された時に実行される
        /// ハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Control_DragEnter(object sender, DragEventArgs e)
        {
            if (!AllowOperation) e.Effect = DragDropEffects.None;
            else if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.All;
            else if (e.Data.GetDataPresent(typeof(ListViewItem))) e.Effect = DragDropEffects.Move;
            else e.Effect = DragDropEffects.None;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Control_DragDrop
        /// 
        /// <summary>
        /// 何らかのコントロールでドロップ操作が行われた時に実行される
        /// ハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Control_DragDrop(object sender, DragEventArgs e)
        {
            if (!AllowOperation) return;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                RaiseAddingEvent(e.Data.GetData(DataFormats.FileDrop, false));
            }
        }

        #endregion

        #region Other private methods

        /* ----------------------------------------------------------------- */
        ///
        /// InitializeLayout
        /// 
        /// <summary>
        /// レイアウトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void InitializeLayout()
        {
            UxTheme.SetWindowTheme(PageListView.Handle, "Explorer", null);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// InitializePresenters
        /// 
        /// <summary>
        /// 各種 Presenter を初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void InitializePresenters()
        {
            new ListViewPresenter(this, new ItemCollection());
        }

        /* ----------------------------------------------------------------- */
        ///
        /// RaiseAddingEvent
        /// 
        /// <summary>
        /// Adding イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void RaiseAddingEvent()
        {
            var dialog = new OpenFileDialog();
            dialog.CheckFileExists = true;
            dialog.Multiselect = true;
            dialog.Title = Properties.Resources.OpenFileTitle;
            dialog.Filter = Properties.Resources.OpenFileFilter;
            if (dialog.ShowDialog() == DialogResult.Cancel) return;

            RaiseAddingEvent(dialog.FileNames);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// RaiseAddingEvent
        /// 
        /// <summary>
        /// Register イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void RaiseAddingEvent(object obj)
        {
            var files = obj as string[];
            if (files == null) return;
            OnAdding(new DataEventArgs<string[]>(files));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// RaiseMergingEvent
        /// 
        /// <summary>
        /// Merging イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void RaiseMergingEvent()
        {
            var dialog = new SaveFileDialog();
            dialog.OverwritePrompt = true;
            dialog.Title = Properties.Resources.SaveFileTitle;
            dialog.Filter = Properties.Resources.SaveFileFilter;
            if (dialog.ShowDialog() == DialogResult.Cancel) return;

            OnMerging(new DataEventArgs<string>(dialog.FileName));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// RaiseSplittingEvent
        /// 
        /// <summary>
        /// Splitting イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void RaiseSplittingEvent()
        {
            OnSplitting(new DataEventArgs<string>(string.Empty));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Execute
        /// 
        /// <summary>
        /// 各種操作を実行します。
        /// </summary>
        /// 
        /// <remarks>
        /// 各種操作の共通に行われる処理をここに記述します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private void Execute(Action op)
        {
            try { op(); }
            finally { UpdateControls(); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// UpdateControls
        /// 
        /// <summary>
        /// 各種コントロールの状態を更新します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void UpdateControls()
        {
            var some = PageListView.Items.Count > 0;
            MergeButton.Enabled = some;
            SplitButton.Enabled = some;

            var selected = PageListView.SelectedIndices.Count > 0;
            UpButton.Enabled     = selected;
            DownButton.Enabled   = selected;
            RemoveButton.Enabled = selected;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Convert
        /// 
        /// <summary>
        /// Item から ListViewItem オブジェクトへ変換します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ListViewItem Convert(Item item)
        {
            var filename = IoEx.Path.GetFileName(item.Path);
            var pages = item.PageCount.ToString();
            var bytes = item.FileSize.ToPrettyBytes();
            var date = item.LastWriteTime.ToString("yyyy/MM/dd hh:mm");

            var dest = new ListViewItem(new string[] { filename, pages, bytes, date });
            dest.ToolTipText = item.Path;
            return dest;
        }

        #endregion
    }
}
