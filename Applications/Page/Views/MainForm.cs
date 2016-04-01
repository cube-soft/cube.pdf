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
            if (args == null || args.Length == 0) return;
            Aggregator.Add.Raise(new ValueEventArgs<string[]>(args));
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
            var tips = new ToolTip
            {
                InitialDelay = 200,
                AutoPopDelay = 5000,
                ReshowDelay  = 1000
            };
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
            TitleButton.Click  += (s, e) => Aggregator.Version.Raise();
            FileButton.Click   += (s, e) => Aggregator.Add.Raise(EventAggregator.Empty);
            RemoveButton.Click += (s, e) => Aggregator.Remove.Raise();
            ClearButton.Click  += (s, e) => Aggregator.Clear.Raise();
            UpButton.Click     += (s, e) => Aggregator.Move.Raise(ValueEventArgs.Create(-1));
            DownButton.Click   += (s, e) => Aggregator.Move.Raise(ValueEventArgs.Create(1));
            MergeButton.Click  += (s, e) => Aggregator.Merge.Raise();
            SplitButton.Click  += (s, e) => Aggregator.Split.Raise();
            ExitButton.Click   += (s, e) => Close();

            ButtonsPanel.DragEnter += Control_DragEnter;
            FooterPanel.DragEnter  += Control_DragEnter;
            FileListView.DragEnter += Control_DragEnter;

            ButtonsPanel.DragDrop += Control_DragDrop;
            FooterPanel.DragDrop  += Control_DragDrop;
            FileListView.DragDrop += Control_DragDrop;

            FileMenu.Aggregator = Aggregator;
            FileListView.ContextMenuStrip = FileMenu;
            FileListView.Added += (s, e) => Refresh();
            FileListView.Removed += (s, e) => Refresh();
            FileListView.Cleared += (s, e) => Refresh();
            FileListView.SelectedIndexChanged += (s, e) => Refresh();
            FileListView.MouseDoubleClick += (s, e) => Aggregator.Preview.Raise();
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
            new FileCollectionPresenter(FileListView, Files, Aggregator);
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

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Refresh
        /// 
        /// <summary>
        /// 再描画します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public override void Refresh()
        {
            try
            {
                SuspendLayout();

                MergeButton.Enabled = FileListView.Items.Count > 1;
                SplitButton.Enabled = FileListView.Items.Count > 0;

                UpButton.Enabled             =
                DownButton.Enabled           =
                RemoveButton.Enabled         =
                FileMenu.PreviewMenu.Enabled =
                FileMenu.UpMenu.Enabled      =
                FileMenu.DownMenu.Enabled    =
                FileMenu.RemoveMenu.Enabled  =
                FileListView.AnyItemsSelected;
            }
            finally
            {
                ResumeLayout();
                base.Refresh();
            }
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
            var asm = new AssemblyReader(Assembly.GetExecutingAssembly());
            var version = new SoftwareVersion(asm.Assembly);
            version.Digit = 3;
            Text = $"{asm.Product} {version}";
            base.OnLoad(e);
            Refresh();
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
            try
            {
                var args = e.Value as string[];
                if (args == null) return;
                Aggregator.Add.Raise(new ValueEventArgs<string[]>(args));
            }
            finally { base.OnReceived(e); }
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
                        if (e.Shift) Aggregator.Clear.Raise();
                        else Aggregator.Remove.Raise();
                        break;
                    case Keys.H:
                        Aggregator.Version.Raise();
                        break;
                    case Keys.M:
                        Aggregator.Merge.Raise();
                        break;
                    case Keys.O:
                        Aggregator.Add.Raise(EventAggregator.Empty);
                        break;
                    case Keys.R:
                        Aggregator.Preview.Raise();
                        break;
                    case Keys.S:
                        Aggregator.Split.Raise();
                        break;
                    case Keys.J:
                    case Keys.Up:
                        Aggregator.Move.Raise(ValueEventArgs.Create(-1));
                        break;
                    case Keys.K:
                    case Keys.Down:
                        Aggregator.Move.Raise(ValueEventArgs.Create(1));
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
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

            var files = e.Data.GetData(DataFormats.FileDrop, false) as string[];
            if (files == null) return;

            Aggregator.Add.Raise(ValueEventArgs.Create(files));
        }

        #endregion

        #region Models
        private FileCollection Files = new FileCollection();
        private IconCollection Icons = new IconCollection();
        private EventAggregator Aggregator = new EventAggregator();
        #endregion

        #region Views
        private FileMenuControl FileMenu = new FileMenuControl();
        #endregion
    }
}
