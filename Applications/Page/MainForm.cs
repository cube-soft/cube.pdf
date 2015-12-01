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
using System.Drawing;
using System.Windows.Forms;
using Cube.Extensions;
using IoEx = System.IO;

namespace Cube.Pdf.Page
{
    /* --------------------------------------------------------------------- */
    ///
    /// Cube.Pdf.Page.MainForm
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

            FileButton.Click  += (s, e) => RaiseRegisterEvent();
            MergeButton.Click += (s, e) => OnMerge(e);
            SplitButton.Click += (s, e) => OnSplit(e);

            ButtonsPanel.DragEnter += Control_DragEnter;
            FooterPanel.DragEnter  += Control_DragEnter;
            PageListView.DragEnter += Control_DragEnter;

            ButtonsPanel.DragDrop  += Control_DragDrop;
            FooterPanel.DragDrop   += Control_DragDrop;
            PageListView.DragDrop  += Control_DragDrop;
        }

        #endregion

        #region Events

        /* ----------------------------------------------------------------- */
        ///
        /// Register
        /// 
        /// <summary>
        /// 新しいファイルの追加時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public EventHandler<DataEventArgs<string[]>> Regsiter;

        /* ----------------------------------------------------------------- */
        ///
        /// Merge
        /// 
        /// <summary>
        /// 結合処理の実行時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public EventHandler Merge;

        /* ----------------------------------------------------------------- */
        ///
        /// Split
        /// 
        /// <summary>
        /// 分割処理の実行時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public EventHandler Split;

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
        public void Add(Item item)
        {
            PageListView.Items.Add(Convert(item));
        }

        #endregion

        #region Virtual methods

        /* ----------------------------------------------------------------- */
        ///
        /// OnRegister
        /// 
        /// <summary>
        /// 新しいファイルの追加時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnRegister(DataEventArgs<string[]> e)
        {
            if (Regsiter != null) Regsiter(this, e);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnMerge
        /// 
        /// <summary>
        /// 結合処理の実行時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnMerge(EventArgs e)
        {
            if (Merge != null) Merge(this, e);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnSplit
        /// 
        /// <summary>
        /// 分割処理の実行時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnSplit(EventArgs e)
        {
            if (Split != null) Split(this, e);
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
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.All;
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
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                RaiseRegisterEvent(e.Data.GetData(DataFormats.FileDrop, false));
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
        /// RaiseRegisterEvent
        /// 
        /// <summary>
        /// Register イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void RaiseRegisterEvent()
        {
            var dialog = new OpenFileDialog();
            dialog.CheckFileExists = true;
            dialog.Multiselect = true;
            dialog.Title = Properties.Resources.FileTitle;
            dialog.Filter = Properties.Resources.FileFilter;
            if (dialog.ShowDialog() == DialogResult.Cancel) return;

            RaiseRegisterEvent(dialog.FileNames);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// RaiseRegisterEvent
        /// 
        /// <summary>
        /// Register イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void RaiseRegisterEvent(object obj)
        {
            var files = obj as string[];
            if (files == null) return;
            OnRegister(new DataEventArgs<string[]>(files));
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
            var filename = IoEx.Path.GetFileNameWithoutExtension(item.Path);
            var type = item.Type == PageType.Pdf ? "PDF" : "Image";
            var pages = item.PageCount.ToString();
            var bytes = ((ulong)item.FileSize).ToPrettyBytes();
            var date = item.LastWriteTime.ToString("yyyy/mm/dd");

            var dest = new ListViewItem(new string[] { filename, type, pages, bytes, date });
            dest.ToolTipText = item.Path;
            return dest;
        }

        #endregion
    }
}
