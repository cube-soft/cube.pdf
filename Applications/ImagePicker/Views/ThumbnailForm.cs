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
using System;
using System.Reflection;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Cube.Forms.Controls;

namespace Cube.Pdf.App.ImageEx
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

            ImageListView.ContextMenuStrip = CreateContextMenu();
            SaveButton.Enabled = AnyItemsSelected;

            TitleButton.Click    += (s, e) => ShowVersion();
            ExitButton.Click     += (s, e) => Close();
            SaveAllButton.Click  += (s, e) => OnSaveAll(e);
            SaveButton.Click     += (s, e) => OnSave(e);
            ImageListView.DoubleClick += (s, e) => OnPreview(e);

            ImageListView.SelectedIndexChanged += (s, e) => { SaveButton.Enabled = AnyItemsSelected; };
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// FileName
        /// 
        /// <summary>
        /// ファイル名を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
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
        public IList<int> SelectedIndices
        {
            get
            {
                var dest = new List<int>();
                foreach (int index in ImageListView.SelectedIndices) dest.Add(index);
                dest.Sort();
                return dest;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// AnyItemsSelected
        /// 
        /// <summary>
        /// サムネイルが一つでも選択されているかどうかを示す値を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool AnyItemsSelected
        {
            get
            {
                var items = ImageListView.SelectedItems;
                return items != null && items.Count > 0;
            }
        }

        #endregion

        #region Events

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        /// 
        /// <summary>
        /// 選択画像を保存する時に発生するイベントです。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public EventHandler Save;

        /* ----------------------------------------------------------------- */
        ///
        /// SaveAll
        /// 
        /// <summary>
        /// 全ての画像を保存する時に発生するイベントです。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public EventHandler SaveAll;

        /* ----------------------------------------------------------------- */
        ///
        /// Preview
        /// 
        /// <summary>
        /// 選択画像のプレビュー画面を表示する時に発生するイベントです。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public EventHandler Preview;

        /* ----------------------------------------------------------------- */
        ///
        /// Removed
        /// 
        /// <summary>
        /// 画像をが削除された時に発生するイベントです。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public EventHandler<ValueEventArgs<int>> Removed;

        #endregion

        #region Methods

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
            _images.Add(image);
            ImageListView.LargeImageList.Images.Add(image);
            ImageListView.Items.Add(new ListViewItem(
                string.Empty,
                ImageListView.LargeImageList.Images.Count - 1
            ));
            Debug.Assert(_images.Count == ImageListView.Items.Count);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Remove
        /// 
        /// <summary>
        /// 選択項目を削除します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public void Remove()
        {
            if (!AnyItemsSelected) return;
            foreach (var index in SelectedIndices.Reverse()) Remove(index);
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
        public void Remove(int index)
        {
            Debug.Assert(_images.Count == ImageListView.Items.Count);

            if (index < 0 || index >= _images.Count) return;
            _images.RemoveAt(index);
            ImageListView.Items.RemoveAt(index);

            OnRemoved(new ValueEventArgs<int>(index));
        }

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
            dialog.Logo = Properties.Resources.Logo;
            dialog.Description = string.Empty;
            dialog.Height = 320;
            dialog.ShowDialog();
        }

        #endregion

        #region Virtual methods

        /* ----------------------------------------------------------------- */
        ///
        /// OnSave
        /// 
        /// <summary>
        /// 選択画像を保存する時に実行されるハンドラです。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        protected virtual void OnSave(EventArgs e)
        {
            if (Save != null) Save(this, e);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnSaveAll
        /// 
        /// <summary>
        /// 全ての画像を保存する時に実行されるハンドラです。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        protected virtual void OnSaveAll(EventArgs e)
        {
            if (SaveAll != null) SaveAll(this, e);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnPreview
        /// 
        /// <summary>
        /// 選択画像のプレビュー画面を表示する時に実行されるハンドラです。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        protected virtual void OnPreview(EventArgs e)
        {
            if (Preview != null) Preview(this, e);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnRemove
        /// 
        /// <summary>
        /// 選択画像を削除する時に実行されるハンドラです。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        protected virtual void OnRemoved(ValueEventArgs<int> e)
        {
            if (Removed != null) Removed(this, e);
        }

        #endregion

        #region Override methods

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

            _images.Clear();
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
                var result = true;

                switch (e.KeyCode)
                {
                    case Keys.A:
                        if (e.Control) SelectAll();
                        break;
                    case Keys.D:
                        if (e.Control) Remove();
                        break;
                    case Keys.S:
                        if (e.Control)
                        {
                            if (e.Shift) OnSaveAll(e);
                            else if (AnyItemsSelected) OnSave(e);
                        }
                        break;
                    case Keys.Delete:
                        Remove();
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
            var tips = new ToolTip();
            tips.InitialDelay = 200;
            tips.AutoPopDelay = 5000;
            tips.ReshowDelay = 1000;
            tips.SetToolTip(TitleButton, Properties.Resources.About);

            ImageListView.LargeImageList = new ImageList();
            ImageListView.LargeImageList.ImageSize = new Size(128, 128);
            ImageListView.LargeImageList.ColorDepth = ColorDepth.Depth32Bit;
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

            var preview = dest.Items.Add(Properties.Resources.PreviewMenu, null, (s, e) => OnPreview(e));
            var hr0     = dest.Items.Add("-");
            var save    = dest.Items.Add(Properties.Resources.SaveMenu, null, (s, e) => OnSave(e));
            var remove  = dest.Items.Add(Properties.Resources.RemoveMenu, null, (s, e) => Remove());
            var hr1     = dest.Items.Add("-");
            var select  = dest.Items.Add(Properties.Resources.SelectAllMenu, null, (s, e) => SelectAll());

            Action action = () =>
            {
                preview.Enabled = AnyItemsSelected;
                save.Enabled    = AnyItemsSelected;
                remove.Enabled  = AnyItemsSelected;
            };

            action();
            ImageListView.SelectedIndexChanged += (s, e) => action();

            return dest;
        }

        #endregion

        #region Fields
        private string _filename = string.Empty;
        private IList<Image> _images = new List<Image>();
        #endregion
    }
}
