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
using System.Windows.Forms;

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

            OnRegister(new DataEventArgs<string[]>(dialog.FileNames));
        }

        #endregion
    }
}
