/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
/* ------------------------------------------------------------------------- */
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace Cube.Pdf.Page.App
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
    public partial class MainForm : Cube.Forms.StandardForm
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
            Aggregator.GetEvents()?.Add.Publish(args);
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
            Aggregator = new Aggregator();

            var tips = new ToolTip
            {
                InitialDelay = 200,
                AutoPopDelay = 5000,
                ReshowDelay  = 1000
            };
            tips.SetToolTip(TitleButton, Properties.Resources.About);

            //FileListView.SmallImageList = Icons.ImageList;

            Text = $"{ProductName} {ProductVersion} ({ProductPlatform})";
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
            TitleButton.Click  += (s, e) => Aggregator.GetEvents()?.Version.Publish();
            FileButton.Click   += (s, e) => Aggregator.GetEvents()?.Add.Publish(null);
            RemoveButton.Click += (s, e) => Aggregator.GetEvents()?.Remove.Publish();
            ClearButton.Click  += (s, e) => Aggregator.GetEvents()?.Clear.Publish();
            UpButton.Click     += (s, e) => Aggregator.GetEvents()?.Move.Publish(-1);
            DownButton.Click   += (s, e) => Aggregator.GetEvents()?.Move.Publish(1);
            MergeButton.Click  += (s, e) => Aggregator.GetEvents()?.Merge.Publish();
            SplitButton.Click  += (s, e) => Aggregator.GetEvents()?.Split.Publish();
            ExitButton.Click   += (s, e) => Close();

            FileMenu.Aggregator = Aggregator;
            //FileListView.ContextMenuStrip = FileMenu;
            //FileListView.DragEnter += (s, e) => OnDragEnter(e);
            //FileListView.DragDrop  += (s, e) => OnDragDrop(e);

            ButtonsPanel.DragEnter += (s, e) => OnDragEnter(e);
            ButtonsPanel.DragDrop  += (s, e) => OnDragDrop(e);

            FooterPanel.DragEnter += (s, e) => OnDragEnter(e);
            FooterPanel.DragDrop  += (s, e) => OnDragDrop(e);

            ShortcutKeys.Add(Keys.Control | Keys.A, SelectAll);
            ShortcutKeys.Add(Keys.Control | Keys.D, () => Aggregator?.GetEvents()?.Remove.Publish());
            ShortcutKeys.Add(Keys.Control | Keys.H, () => Aggregator?.GetEvents()?.Version.Publish());
            ShortcutKeys.Add(Keys.Control | Keys.J, () => Aggregator.GetEvents()?.Move.Publish(1));
            ShortcutKeys.Add(Keys.Control | Keys.K, () => Aggregator.GetEvents()?.Move.Publish(-1));
            ShortcutKeys.Add(Keys.Control | Keys.M, () => Aggregator?.GetEvents()?.Merge.Publish());
            ShortcutKeys.Add(Keys.Control | Keys.O, () => Aggregator.GetEvents()?.Add.Publish(null));
            ShortcutKeys.Add(Keys.Control | Keys.R, () => Aggregator.GetEvents()?.Preview.Publish());
            ShortcutKeys.Add(Keys.Control | Keys.S, () => Aggregator.GetEvents()?.Split.Publish());
            ShortcutKeys.Add(Keys.Control | Keys.Up, () => Aggregator.GetEvents()?.Move.Publish(-1));
            ShortcutKeys.Add(Keys.Control | Keys.Down, () => Aggregator.GetEvents()?.Move.Publish(1));
            ShortcutKeys.Add(Keys.Control | Keys.Shift | Keys.D, () => Aggregator?.GetEvents()?.Clear.Publish());
            ShortcutKeys.Add(Keys.Delete, () => Aggregator?.GetEvents()?.Remove.Publish());
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
            //new FileCollectionPresenter(FileListView, Files, Settings, Aggregator);
            new MenuPresenter(this, Settings, Aggregator);
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

                //MergeButton.Enabled = FileListView.Items.Count > 1;
                //SplitButton.Enabled = FileListView.Items.Count > 0;

                //UpButton.Enabled             =
                //DownButton.Enabled           =
                //RemoveButton.Enabled         =
                //FileMenu.PreviewMenu.Enabled =
                //FileMenu.UpMenu.Enabled      =
                //FileMenu.DownMenu.Enabled    =
                //FileMenu.RemoveMenu.Enabled  = FileListView.AnyItemsSelected;
            }
            finally
            {
                ResumeLayout();
                base.Refresh();
            }
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// OnReceived
        ///
        /// <summary>
        /// 他プロセスからデータ受信時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnReceived(EnumerableEventArgs<string> e)
        {
            try
            {
                if (e.Value == null) return;
                Aggregator.GetEvents()?.Add.Publish(e.Value.ToArray());
            }
            finally { base.OnReceived(e); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnDragEnter
        ///
        /// <summary>
        /// ファイルがドラッグされた時に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnDragEnter(DragEventArgs e)
        {
            var prev = e.Effect;
            base.OnDragEnter(e);
            if (e.Effect != prev || !e.Data.GetDataPresent(DataFormats.FileDrop)) return;

            e.Effect = DragDropEffects.Copy;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnDragDrop
        ///
        /// <summary>
        /// ファイルがドロップされた時に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnDragDrop(DragEventArgs e)
        {
            base.OnDragDrop(e);

            var files = e.Data.GetData(DataFormats.FileDrop, false) as string[];
            if (files == null) return;

            Aggregator.GetEvents()?.Add.Publish(files);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SelectAll
        ///
        /// <summary>
        /// 全ての項目を選択します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SelectAll()
        {
            //foreach (ListViewItem item in FileListView.Items) item.Selected = true;
        }

        #region Models
        private FileCollection Files = new FileCollection();
        private IconCollection Icons = new IconCollection();
        private Settings Settings = new Settings();
        #endregion

        #region Views
        private FileMenuControl FileMenu = new FileMenuControl();
        #endregion

        #endregion
    }
}
