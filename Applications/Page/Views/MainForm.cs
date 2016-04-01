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
using System.ComponentModel;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;

namespace Cube.Pdf.App.Page
{
    /* --------------------------------------------------------------------- */
    ///
    /// MainForm
    ///
    /// <summary>
    /// CubePDF Page メイン画面を表示するクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public partial class MainForm : Cube.Forms.Form
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
            InitializeEvents();
            InitializePresenters();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// MainForm
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public MainForm(string[] args)
            : this()
        {
            RaiseAddEvent(args);
        }

        #endregion

        #region Initialize methods

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
            var tips = new ToolTip();
            tips.InitialDelay = 200;
            tips.AutoPopDelay = 5000;
            tips.ReshowDelay = 1000;
            tips.SetToolTip(TitleButton, Properties.Resources.About);

            FileListView.SmallImageList = Icons.ImageList;
            FileListView.Converter = new FileConverter(Icons);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// InitializeEvents
        /// 
        /// <summary>
        /// イベントを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void InitializeEvents()
        {
            TitleButton.Click  += (s, e) => ShowVersion();
            FileButton.Click   += (s, e) => RaiseAddEvent();
            RemoveButton.Click += (s, e) => OnRemoveRequired(e);
            ClearButton.Click  += (s, e) => OnClearRequired(e);
            UpButton.Click     += (s, e) => OnMoveRequired(new ValueEventArgs<int>(-1));
            DownButton.Click   += (s, e) => OnMoveRequired(new ValueEventArgs<int>(1));
            MergeButton.Click  += (s, e) => RaiseMergeEvent();
            SplitButton.Click  += (s, e) => RaiseSplitEvent();
            ExitButton.Click   += (s, e) => Close();

            ButtonsPanel.DragEnter += Control_DragEnter;
            FooterPanel.DragEnter  += Control_DragEnter;
            FileListView.DragEnter += Control_DragEnter;

            ButtonsPanel.DragDrop += Control_DragDrop;
            FooterPanel.DragDrop  += Control_DragDrop;
            FileListView.DragDrop += Control_DragDrop;

            FileListView.ContextMenuStrip = CreateContextMenu();
            FileListView.SelectedIndexChanged += (s, e) => UpdateControls();
            FileListView.MouseDoubleClick += (s, e) => RaisePreviewEvent();
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
            new FileCollectionPresenter(this, Files);
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
        [Browsable(true)]
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
        [Browsable(false)]
        public IList<int> SelectedIndices
        {
            get
            {
                var dest = new List<int>();
                foreach (int index in FileListView.SelectedIndices) dest.Add(index);
                dest.Sort();
                return dest;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// AnyItemsSelected
        /// 
        /// <summary>
        /// 項目が一つでも選択されているかどうかを示す値を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Browsable(false)]
        public bool AnyItemsSelected
        {
            get
            {
                var items = FileListView.SelectedItems;
                return items != null && items.Count > 0;
            }
        }

        #endregion

        #region Events

        /* ----------------------------------------------------------------- */
        ///
        /// PreviewRequired
        /// 
        /// <summary>
        /// プレビュー要求時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public EventHandler<ValueEventArgs<int>> PreviewRequired;

        /* ----------------------------------------------------------------- */
        ///
        /// AddRequired
        /// 
        /// <summary>
        /// ファイルの追加要求時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public EventHandler<ValueEventArgs<string[]>> AddRequired;

        /* ----------------------------------------------------------------- */
        ///
        /// RemoveRequired
        /// 
        /// <summary>
        /// 項目を削除要求時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public EventHandler RemoveRequired;

        /* ----------------------------------------------------------------- */
        ///
        /// ClearRequired
        /// 
        /// <summary>
        /// 全項目の削除要求時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public EventHandler ClearRequired;

        /* ----------------------------------------------------------------- */
        ///
        /// MoveRequired
        /// 
        /// <summary>
        /// 項目の移動要求時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public EventHandler<ValueEventArgs<int>> MoveRequired;

        /* ----------------------------------------------------------------- */
        ///
        /// MergeRequired
        /// 
        /// <summary>
        /// 結合処理の実行時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public EventHandler<ValueEventArgs<string>> MergeRequired;

        /* ----------------------------------------------------------------- */
        ///
        /// SplitRequired
        /// 
        /// <summary>
        /// 分割処理の実行時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public EventHandler<ValueEventArgs<string>> SplitRequired;

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
        public void Add(FileBase item)
        {
            Execute(() => FileListView.Add(item));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Insert
        /// 
        /// <summary>
        /// 指定された位置に項目を挿入します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Insert(int index, FileBase item)
        {
            Execute(() =>
            {
                var i = Math.Max(Math.Min(index, FileListView.Items.Count), 0);
                FileListView.Insert(i, item);
            });
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Remove
        /// 
        /// <summary>
        /// 指定されたインデックスに対応する項目を削除します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Remove(int index)
        {
            Execute(() => FileListView.Items.RemoveAt(index));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ClearItems
        /// 
        /// <summary>
        /// 全ての項目を削除します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void ClearItems()
        {
            Execute(() => FileListView.Items.Clear());
        }

        /* ----------------------------------------------------------------- */
        ///
        /// MoveItem
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
                if (oldindex < 0 || oldindex >= FileListView.Items.Count) return;

                var item = FileListView.Items[oldindex];
                FileListView.Items.RemoveAt(oldindex);
                var result = FileListView.Items.Insert(newindex, item);
                if (result != null) result.Selected = true;
            });
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ShowVersion
        /// 
        /// <summary>
        /// バージョン情報を表示します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void ShowVersion()
        {
            var dialog = new Cube.Forms.VersionForm();
            dialog.Assembly = Assembly.GetExecutingAssembly();
            dialog.Version.Digit = 3;
            dialog.Logo = Properties.Resources.Logo;
            dialog.Description = string.Empty;
            dialog.Height = 250;
            dialog.ShowDialog();
        }

        #endregion

        #region Virtual methods

        /* ----------------------------------------------------------------- */
        ///
        /// OnPreviewRequired
        /// 
        /// <summary>
        /// 項目を開く時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnPreviewRequired(ValueEventArgs<int> e)
        {
            if (PreviewRequired != null) PreviewRequired(this, e);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnAddRequired
        /// 
        /// <summary>
        /// AddRequired イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnAddRequired(ValueEventArgs<string[]> e)
        {
            if (AddRequired != null) AddRequired(this, e);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnRemoveRequired
        /// 
        /// <summary>
        /// RemoveRequired イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnRemoveRequired(EventArgs e)
        {
            if (RemoveRequired != null) RemoveRequired(this, e);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnClearRequired
        /// 
        /// <summary>
        /// ClearRequired イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnClearRequired(EventArgs e)
        {
            if (ClearRequired != null) ClearRequired(this, e);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnMoveRequired
        /// 
        /// <summary>
        /// MoveRequired イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnMoveRequired(ValueEventArgs<int> e)
        {
            if (MoveRequired != null) MoveRequired(this, e);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnMergeRequired
        /// 
        /// <summary>
        /// MergeRequired イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnMergeRequired(ValueEventArgs<string> e)
        {
            if (MergeRequired != null) MergeRequired(this, e);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnSplitRequired
        /// 
        /// <summary>
        /// SplitRequired イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnSplitRequired(ValueEventArgs<string> e)
        {
            if (SplitRequired != null) SplitRequired(this, e);
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
            Execute(() =>
            {
                var arch = (IntPtr.Size == 4) ? "x86" : "x64";
                var asm = new AssemblyReader(Assembly.GetExecutingAssembly());
                Text = string.Format("{0} {1} ({2})", asm.Product, asm.Version.ToString(3), arch);
                ExitButton.Select();
            });
            base.OnLoad(e);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnReceived
        /// 
        /// <summary>
        /// 他プロセスからデータ受信時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnReceived(ValueEventArgs<object> e)
        {
            RaiseAddEvent(e.Value);
            base.OnReceived(e);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnKeyDown
        /// 
        /// <summary>
        /// キーボードのキーが押下された時に実行されるハンドラです。
        /// </summary>
        /// 
        /// <remarks>
        /// ショートカットキーは以下の通りです。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnKeyDown(KeyEventArgs e)
        {
            try
            {
                if (!e.Control) return;

                var results = true;

                switch (e.KeyCode)
                {
                    case Keys.A:
                        foreach (ListViewItem item in FileListView.Items) item.Selected = true;
                        break;
                    case Keys.D:
                        if (e.Alt) OnClearRequired(e);
                        else OnRemoveRequired(e);
                        break;
                    case Keys.H:
                        ShowVersion();
                        break;
                    case Keys.M:
                        RaiseMergeEvent();
                        break;
                    case Keys.O:
                        RaiseAddEvent();
                        break;
                    case Keys.S:
                        RaiseSplitEvent();
                        break;
                    case Keys.J:
                    case Keys.Up:
                        OnMoveRequired(new ValueEventArgs<int>(-1));
                        break;
                    case Keys.K:
                    case Keys.Down:
                        OnMoveRequired(new ValueEventArgs<int>(1));
                        break;
                    default:
                        results = false;
                        break;
                }

                e.Handled = results;
            }
            finally { base.OnKeyDown(e); }
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
                RaiseAddEvent(e.Data.GetData(DataFormats.FileDrop, false));
            }
        }

        #endregion

        #region RaiseXxxEvent methods

        /* ----------------------------------------------------------------- */
        ///
        /// RaisePreviewEvent
        /// 
        /// <summary>
        /// PreviewRequired イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void RaisePreviewEvent()
        {
            var indices = SelectedIndices;
            if (indices.Count <= 0) return;

            OnPreviewRequired(new ValueEventArgs<int>(indices[0]));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// RaiseAddEvent
        /// 
        /// <summary>
        /// AddRequired イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void RaiseAddEvent()
        {
            var dialog = new OpenFileDialog();
            dialog.CheckFileExists = true;
            dialog.Multiselect = true;
            dialog.Title = Properties.Resources.OpenFileTitle;
            dialog.Filter = Properties.Resources.OpenFileFilter;
            if (dialog.ShowDialog() == DialogResult.Cancel) return;

            RaiseAddEvent(dialog.FileNames);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// RaiseAddEvent
        /// 
        /// <summary>
        /// AddRequired イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void RaiseAddEvent(object obj)
        {
            var files = obj as string[];
            if (files == null) return;
            OnAddRequired(new ValueEventArgs<string[]>(files));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// RaiseMergeEvent
        /// 
        /// <summary>
        /// MergeRequired イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void RaiseMergeEvent()
        {
            var dialog = new SaveFileDialog();
            dialog.OverwritePrompt = true;
            dialog.Title = Properties.Resources.MergeTitle;
            dialog.Filter = Properties.Resources.SaveFileFilter;
            if (dialog.ShowDialog() == DialogResult.Cancel) return;

            OnMergeRequired(new ValueEventArgs<string>(dialog.FileName));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// RaiseSplitEvent
        /// 
        /// <summary>
        /// SplitRequired イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void RaiseSplitEvent()
        {
            var dialog = new FolderBrowserDialog();
            dialog.Description = Properties.Resources.SplitDescription;
            dialog.ShowNewFolderButton = true;
            if (dialog.ShowDialog() == DialogResult.Cancel) return;

            OnSplitRequired(new ValueEventArgs<string>(dialog.SelectedPath));
        }

        #endregion

        #region Others

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
            MergeButton.Enabled = FileListView.Items.Count > 1;
            SplitButton.Enabled = FileListView.Items.Count > 0;

            var selected = FileListView.SelectedIndices.Count > 0;
            UpButton.Enabled     = selected;
            DownButton.Enabled   = selected;
            RemoveButton.Enabled = selected;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CreateContextMenu
        /// 
        /// <summary>
        /// コンテキストメニューを生成します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private ContextMenuStrip CreateContextMenu()
        {
            var dest = new ContextMenuStrip();

            var open   = dest.Items.Add(Properties.Resources.OpenMenu, null, (s, e) => RaisePreviewEvent());
            var hr0    = dest.Items.Add("-");
            var up     = dest.Items.Add(Properties.Resources.UpMenu, null, (s, e) => OnMoveRequired(new ValueEventArgs<int>(-1)));
            var down   = dest.Items.Add(Properties.Resources.DownMenu, null, (s, e) => OnMoveRequired(new ValueEventArgs<int>(1)));
            var hr1    = dest.Items.Add("-");
            var remove = dest.Items.Add(Properties.Resources.RemoveMenu, null, (s, e) => OnRemoveRequired(e));

            Action action = () =>
            {
                open.Enabled   = AnyItemsSelected;
                up.Enabled     = AnyItemsSelected;
                down.Enabled   = AnyItemsSelected;
                remove.Enabled = AnyItemsSelected;
            };

            action();
            FileListView.SelectedIndexChanged += (s, e) => action();

            return dest;
        }

        #endregion

        #region Models
        private FileCollection Files = new FileCollection();
        private IconCollection Icons = new IconCollection();
        #endregion
    }
}
