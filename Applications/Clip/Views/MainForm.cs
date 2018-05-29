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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace Cube.Pdf.Clip.App
{
    /* --------------------------------------------------------------------- */
    ///
    /// MainForm
    ///
    /// <summary>
    /// メイン画面を表すクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public partial class MainForm : Cube.Forms.StandardForm, IClipView
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

            // Event handlers
            ExitButton.Click           += (s, e) => Close();
            OpenButton.Click           += (s, e) => RaiseOpen();
            AttachButton.Click         += (s, e) => RaiseAttach();
            DetachButton.Click         += (s, e) => RaiseDetach();
            ResetButton.Click          += (s, e) => RaiseReset();
            SaveButton.Click           += (s, e) => RaiseSave();
            VersionButton.Click        += (s, e) => RaiseVersion();
            MyClipDataView.RowsAdded   += WhenRowCountChanged;
            MyClipDataView.RowsRemoved += WhenRowCountChanged;
            SourceTextBox.TextChanged  += WhenSourceChanged;

            // Shortcut keys
            ShortcutKeys.Add(Keys.Delete,           RaiseDetach);
            ShortcutKeys.Add(Keys.Control | Keys.D, RaiseDetach);
            ShortcutKeys.Add(Keys.Control | Keys.H, RaiseVersion);
            ShortcutKeys.Add(Keys.Control | Keys.N, RaiseAttach);
            ShortcutKeys.Add(Keys.Control | Keys.O, RaiseOpen);
            ShortcutKeys.Add(Keys.Control | Keys.R, RaiseReset);
            ShortcutKeys.Add(Keys.Control | Keys.S, RaiseSave);

            // Properties
            Source = "WhenSourceChanged(object, EventArgs)";
            Source = string.Empty;
            Text   = $"{ProductName} {ProductVersion} ({ProductPlatform})";

            // ToolTip
            var tips = new ToolTip
            {
                InitialDelay = 200,
                AutoPopDelay = 5000,
                ReshowDelay  = 1000
            };
            tips.SetToolTip(VersionButton, Properties.Resources.VersionTitle);
            tips.SetToolTip(OpenButton,    Properties.Resources.SourceTitle);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// MainForm
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="args">プログラム引数</param>
        ///
        /* ----------------------------------------------------------------- */
        public MainForm(string[] args) : this()
        {
            Load += (s, e) => Aggregator?.GetEvents()?.Open.Publish(args);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Source
        ///
        /// <summary>
        /// PDF ファイルのパスを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Source
        {
            get { return SourceTextBox.Text; }
            set { SourceTextBox.Text = value; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// DataSource
        ///
        /// <summary>
        /// View に関連付けられるデータを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object DataSource
        {
            get { return MyClipDataView.DataSource; }
            set { MyClipDataView.DataSource = value; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SelectedIndices
        ///
        /// <summary>
        /// 選択されているインデックスの一覧を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IEnumerable<int> SelectedIndices
            => MyClipDataView.SelectedRows
                             .Cast<DataGridViewRow>()
                             .Select(x => x.Index);

        /* ----------------------------------------------------------------- */
        ///
        /// IsBusy
        ///
        /// <summary>
        /// 処理中かどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;

                SourcePanel.Enabled =
                ClipPanel.Enabled   = !value;
                ToolsPanel.Enabled  =
                SaveButton.Enabled  = !value && !string.IsNullOrEmpty(SourceTextBox.Text);

                Cursor = value ? Cursors.WaitCursor : Cursors.Default;
            }
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// OnDragEnter
        ///
        /// <summary>
        /// ドラッグ時に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnDragEnter(DragEventArgs e)
        {
            base.OnDragEnter(e);
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ?
                       DragDropEffects.Copy :
                       DragDropEffects.None;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnDragDrop
        ///
        /// <summary>
        /// ドラッグ&ドロップ時に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnDragDrop(DragEventArgs e)
        {
            base.OnDragDrop(e);

            var files = e.Data.GetData(DataFormats.FileDrop, false) as string[];
            var open  = string.IsNullOrEmpty(SourceTextBox.Text);
            if (open) Aggregator?.GetEvents()?.Open.Publish(files);
            else Aggregator?.GetEvents()?.Attach.Publish(files);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// RaiseOpen
        ///
        /// <summary>
        /// Open イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void RaiseOpen()
        {
            var view = Views.CreateOpenView();
            if (view.ShowDialog() == DialogResult.Cancel) return;
            Aggregator?.GetEvents()?.Open.Publish(view.FileNames);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// RaiseAttach
        ///
        /// <summary>
        /// Attach イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void RaiseAttach()
        {
            if (!ToolsPanel.Enabled || !AttachButton.Enabled) return;
            var view = Views.CreateAttachView();
            if (view.ShowDialog() == DialogResult.Cancel) return;
            Aggregator?.GetEvents()?.Attach.Publish(view.FileNames);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// RaiseDetach
        ///
        /// <summary>
        /// Detach イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void RaiseDetach()
        {
            if (!ToolsPanel.Enabled || !DetachButton.Enabled) return;
            Aggregator?.GetEvents()?.Detach.Publish();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// RaiseReset
        ///
        /// <summary>
        /// Reset イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void RaiseReset()
        {
            if (!ToolsPanel.Enabled || !ResetButton.Enabled) return;
            Aggregator?.GetEvents()?.Reset.Publish();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// RaiseSave
        ///
        /// <summary>
        /// Save イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void RaiseSave()
        {
            if (!SaveButton.Enabled) return;
            Aggregator?.GetEvents()?.Save.Publish();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// RaiseVersion
        ///
        /// <summary>
        /// バージョン画面を表示します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void RaiseVersion()
        {
            var version = $"Version {ProductVersion} ({ProductPlatform})";
            var view = Views.CreateVersionView(version, Icon);
            view.ShowDialog();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// WhenRowCountChanged
        ///
        /// <summary>
        /// DataGridView の行数が変化した時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenRowCountChanged(object sender, EventArgs e)
            => DetachButton.Enabled = MyClipDataView.RowCount > 0;

        /* ----------------------------------------------------------------- */
        ///
        /// WhenSourceChanged
        ///
        /// <summary>
        /// PDF ファイル欄の内容が変化した時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenSourceChanged(object sender, EventArgs e)
        {
            ToolsPanel.Enabled =
            SaveButton.Enabled = !string.IsNullOrEmpty(SourceTextBox.Text);
        }

        #region Fields
        private bool _isBusy = false;
        #endregion

        #endregion
    }
}
