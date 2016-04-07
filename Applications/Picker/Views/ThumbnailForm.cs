/* ------------------------------------------------------------------------- */
///
/// ThumbnailForm.cs
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
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using Cube.Forms.Controls;
using System;

namespace Cube.Pdf.App.Picker
{
    /* --------------------------------------------------------------------- */
    ///
    /// ThumbnailForm
    ///
    /// <summary>
    /// サムネイル一覧を表示するクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public partial class ThumbnailForm : Cube.Forms.Form
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ThumbnailForm
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ThumbnailForm()
        {
            InitializeComponent();
            InitializeLayout();
            InitializeEvents();
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Aggregator
        /// 
        /// <summary>
        /// イベントを集約したオブジェクトを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public EventAggregator Aggregator { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// MenuControl
        /// 
        /// <summary>
        /// コンテキストメニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ThumbnailMenuControl MenuControl { get; } = new ThumbnailMenuControl();

        /* ----------------------------------------------------------------- */
        ///
        /// Complete
        /// 
        /// <summary>
        /// 何らかの保存操作が完了したかどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Browsable(true)]
        [DefaultValue(false)]
        public bool Complete { get; set; } = false;

        /* ----------------------------------------------------------------- */
        ///
        /// FileName
        /// 
        /// <summary>
        /// ファイル名を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Browsable(true)]
        public string FileName
        {
            get { return _filename; }
            set
            {
                if (_filename != value)
                {
                    _filename = value;
                    this.UpdateText(value);
                }
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ImageSize
        /// 
        /// <summary>
        /// サムネイルのサイズを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Browsable(false)]
        public Size ImageSize
        {
            get { return ImageListView.LargeImageList.ImageSize; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SelectedIndices
        /// 
        /// <summary>
        /// 選択されているサムネイルのインデックスを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Browsable(false)]
        public ListView.SelectedIndexCollection SelectedIndices
            => ImageListView.SelectedIndices;

        /* ----------------------------------------------------------------- */
        ///
        /// AnyItemsSelected
        /// 
        /// <summary>
        /// サムネイルが一つでも選択されているかどうかを示す値を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Browsable(false)]
        public bool AnyItemsSelected => ImageListView.AnyItemsSelected;

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
            SuspendLayout();

            SaveButton.Enabled              =
            MenuControl.PreviewMenu.Enabled =
            MenuControl.SaveMenu.Enabled    =
            MenuControl.RemoveMenu.Enabled  =
            AnyItemsSelected;

            ResumeLayout();
            base.Refresh();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Add
        /// 
        /// <summary>
        /// 新しいサムネイルを追加します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Add(Image image)
        {
            ImageListView.LargeImageList.Images.Add(image);
            ImageListView.Items.Add(new ListViewItem(
                string.Empty,
                ImageListView.LargeImageList.Images.Count - 1
            ));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// AddRange
        /// 
        /// <summary>
        /// 新しいサムネイルを追加します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void AddRange(IEnumerable<Image> images)
        {
            try
            {
                ImageListView.BeginUpdate();
                foreach (var image in images) Add(image);
            }
            finally { ImageListView.EndUpdate(); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Remove
        /// 
        /// <summary>
        /// 指定されたインデックスに対応するサムネイルを削除します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Remove(IEnumerable<int> indices)
            => ImageListView.RemoveItems(indices);

        /* ----------------------------------------------------------------- */
        ///
        /// SelectAll
        /// 
        /// <summary>
        /// 全てのサムネイルを選択します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void SelectAll()
        {
            foreach (ListViewItem item in ImageListView.Items) item.Selected = true;
        }

        #endregion

        #region Override methods

        /* ----------------------------------------------------------------- */
        ///
        /// OnShown
        /// 
        /// <summary>
        /// フォームの表示直後に実行されます。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        protected override void OnShown(EventArgs e)
        {
            Refresh();
            base.OnShown(e);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnFormClosed
        /// 
        /// <summary>
        /// フォームが閉じた時に実行されます。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);

            ImageListView.Items.Clear();
            ImageListView.LargeImageList.Images.Clear();
            ImageListView.LargeImageList.Dispose();
            ImageListView.LargeImageList = null;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnKeyDown
        /// 
        /// <summary>
        /// キーが押下された時に実行されます。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        protected override void OnKeyDown(KeyEventArgs e)
        {
            try
            {
                if (!e.Control) return;

                var result = true;
                switch (e.KeyCode)
                {
                    case Keys.A:
                        SelectAll();
                        break;
                    case Keys.D:
                        Aggregator?.Remove.Raise();
                        break;
                    case Keys.R:
                        Aggregator?.PreviewImage.Raise();
                        break;
                    case Keys.S:
                        if (e.Shift) Aggregator?.Save.Raise(EventAggregator.All);
                        else if (AnyItemsSelected) RaiseSave();
                        break;
                    default:
                        result = false;
                        break;
                }
                e.Handled = result;
            }
            finally { base.OnKeyDown(e); }
        }

        #endregion

        #region Others

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
            ImageListView.ContextMenuStrip = MenuControl;
            ImageListView.LargeImageList = new ImageList
            {
                ImageSize  = new Size(128, 128),
                ColorDepth = ColorDepth.Depth32Bit,
            };

            new ToolTip
            {
                InitialDelay =  200,
                AutoPopDelay = 5000,
                ReshowDelay  = 1000,
            }.SetToolTip(TitleButton, Properties.Resources.About);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// InitializeEvents
        /// 
        /// <summary>
        /// 各種イベントを初期化します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private void InitializeEvents()
        {
            ExitButton.Click    += (s, e) => Close();
            TitleButton.Click   += (s, e) => Aggregator?.Version.Raise();
            SaveAllButton.Click += (s, e) => Aggregator?.Save.Raise(EventAggregator.All);
            SaveButton.Click    += (s, e) => RaiseSave();

            MenuControl.PreviewMenu.Click   += (s, e) => Aggregator?.PreviewImage.Raise();
            MenuControl.RemoveMenu.Click    += (s, e) => Aggregator?.Remove.Raise();
            MenuControl.SaveMenu.Click      += (s, e) => RaiseSave();
            MenuControl.SelectAllMenu.Click += (s, e) => SelectAll();

            ImageListView.DoubleClick += (s, e) => Aggregator?.PreviewImage.Raise();
            ImageListView.SelectedIndexChanged += (s, e) => Refresh();
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
            => Aggregator?.Save.Raise(ValueEventArgs.Create(SelectedIndices.Ascend().ToArray()));

        #endregion

        #region Fields
        private string _filename = string.Empty;
        #endregion
    }
}
