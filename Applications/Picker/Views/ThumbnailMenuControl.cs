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
using System.Windows.Forms;

namespace Cube.Pdf.App.Picker
{
    /* --------------------------------------------------------------------- */
    ///
    /// ThumbnailMenuControl
    ///
    /// <summary>
    /// サムネイル画面で表示されるコンテキストメニューを表すクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ThumbnailMenuControl : ContextMenuStrip
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ThumbnailMenuControl
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ThumbnailMenuControl() : base()
        {
            PreviewMenu   = new ToolStripMenuItem(Properties.Resources.MenuPreviewImage);
            SelectAllMenu = new ToolStripMenuItem(Properties.Resources.MenuSelectAll);
            SaveMenu      = new ToolStripMenuItem(Properties.Resources.MenuSave);
            RemoveMenu    = new ToolStripMenuItem(Properties.Resources.MenuRemove);

            InitializeShortcutKeys();
            InitializeMenu();
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Aggregator
        ///
        /// <summary>
        /// イベントを集約するオブジェクトを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Aggregator Aggregator { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// PreviewMenu
        ///
        /// <summary>
        /// プレビューメニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ToolStripItem PreviewMenu { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// SelectAll
        ///
        /// <summary>
        /// 全て選択メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ToolStripItem SelectAllMenu { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// 保存メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ToolStripItem SaveMenu { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// RemoveMenu
        ///
        /// <summary>
        /// 削除メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ToolStripItem RemoveMenu { get; }

        #endregion

        #region Others

        /* ----------------------------------------------------------------- */
        ///
        /// InitialzieShortcutKeys
        ///
        /// <summary>
        /// ショートカットキーを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void InitializeShortcutKeys()
        {
            Menu(PreviewMenu).ShortcutKeys   = Keys.Control | Keys.R;
            Menu(SelectAllMenu).ShortcutKeys = Keys.Control | Keys.A;
            Menu(SaveMenu).ShortcutKeys      = Keys.Control | Keys.S;
            Menu(RemoveMenu).ShortcutKeys    = Keys.Control | Keys.D;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// InitializeMenu
        ///
        /// <summary>
        /// メニューを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void InitializeMenu()
        {
            Items.AddRange(new ToolStripItem[]
            {
                PreviewMenu,
                new ToolStripSeparator(),
                SaveMenu,
                RemoveMenu,
                new ToolStripSeparator(),
                SelectAllMenu,
            });
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Menu
        ///
        /// <summary>
        /// ToolStripMenuItem にキャストします。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ToolStripMenuItem Menu(ToolStripItem src)
            => src as ToolStripMenuItem;

        #endregion
    }
}
