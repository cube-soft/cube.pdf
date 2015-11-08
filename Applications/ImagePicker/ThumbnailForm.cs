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
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Cube.Pdf.ImageEx.Extensions;

namespace Cube.Pdf.ImageEx
{
    /* --------------------------------------------------------------------- */
    ///
    /// Cube.Pdf.ImageEx.ThumbnailForm
    ///
    /// <summary>
    /// サムネイル一覧を表示するクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public partial class ThumbnailForm : Cube.Forms.NtsForm
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

            ListView.ContextMenuStrip = CreateContextMenu();
            SaveButton.UpdateStatus(false);

            ExitButton.Click     += (s, e) => Close();
            SaveAllButton.Click  += (s, e) => OnSaveAll(e);
            SaveButton.Click     += (s, e) => OnSave(e);
            ListView.DoubleClick += (s, e) => OnPreview(e);

            ListView.SelectedIndexChanged += ListView_SelectedIndexChanged;
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
                    this.UpdateTitle(value);
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
            get { return ListView.LargeImageList.ImageSize; }
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
                foreach (int index in ListView.SelectedIndices) dest.Add(index);
                dest.Sort();
                return dest;
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
            ListView.LargeImageList.Images.Add(image);
            ListView.Items.Add(new ListViewItem(
                string.Empty,
                ListView.LargeImageList.Images.Count - 1
            ));
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
            foreach (ListViewItem item in ListView.Items)
            {
                item.Selected = true;
            }
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

        #endregion

        #region Override methods

        /* ----------------------------------------------------------------- */
        ///
        /// OnKeyDown
        /// 
        /// <summary>
        /// キーボードのキーが押下された時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.A:
                    if (e.Control) SelectAll();
                    break;
                default:
                    break;
            }
            base.OnKeyDown(e);
        }

        #endregion

        #region Event handlers

        /* ----------------------------------------------------------------- */
        ///
        /// ListView_SelectedIndexChanged
        /// 
        /// <summary>
        /// サムネイル一覧の選択項目が変更された時に実行されるハンドラです。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private void ListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            var indices = ListView.SelectedIndices;
            SaveButton.UpdateStatus(indices != null && indices.Count > 0);
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
            ListView.LargeImageList = new ImageList();
            ListView.LargeImageList.ImageSize = new Size(128, 128);
            ListView.LargeImageList.ColorDepth = ColorDepth.Depth32Bit;

            UxTheme.SetWindowTheme(ListView.Handle, "Explorer", null);
        }

        private ContextMenuStrip CreateContextMenu()
        {
            var dest = new ContextMenuStrip();


            var preview = new ToolStripMenuItem
            {
                Name = "Preview",
                Text = "プレビュー"
            };
            preview.Click += (s, e) => OnPreview(e);
            dest.Items.Add(preview);

            dest.Items.Add("-");

            var save = new ToolStripMenuItem
            {
                Name = "Save",
                Text = "保存"
            };
            save.Click += (s, e) => OnSave(e);
            dest.Items.Add(save);

            var remove = new ToolStripMenuItem
            {
                Name = "Remove",
                Text = "削除"
            };
            dest.Items.Add(remove);

            dest.Items.Add("-");

            var selectall = new ToolStripMenuItem
            {
                Name = "SelectAll",
                Text = "全て選択"
            };
            selectall.Click += (s, e) => SelectAll();
            dest.Items.Add(selectall);

            return dest;
        }

        #endregion

        #region Fields
        private string _filename = string.Empty;
        #endregion
    }
}
