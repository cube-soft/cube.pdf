/* ------------------------------------------------------------------------- */
///
/// FileMenuControl.cs
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
using System.Windows.Forms;

namespace Cube.Pdf.App.Page
{
    /* --------------------------------------------------------------------- */
    ///
    /// FileMenuControl
    ///
    /// <summary>
    /// ファイルリスト上で表示されるコンテキストメニューを表すクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class FileMenuControl : ContextMenuStrip
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// FileMenuControl
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public FileMenuControl() : base()
        {
            PreviewMenu = new ToolStripMenuItem(Properties.Resources.MenuPreview);
            UpMenu      = new ToolStripMenuItem(Properties.Resources.MenuUp);
            DownMenu    = new ToolStripMenuItem(Properties.Resources.MenuDown);
            RemoveMenu  = new ToolStripMenuItem(Properties.Resources.MenuRemove);

            InitializeShortcutKeys();
            InitializeEvents();
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
        public EventAggregator Aggregator { get; set; }

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
        /// UpMenu
        /// 
        /// <summary>
        /// 上へメニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ToolStripItem UpMenu { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// DownMenu
        /// 
        /// <summary>
        /// 下へメニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ToolStripItem DownMenu { get; }

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
        /// InitializeShortcutKeys
        /// 
        /// <summary>
        /// ショートカットキーを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void InitializeShortcutKeys()
        {
            Menu(PreviewMenu).ShortcutKeys = Keys.Control | Keys.R;
            Menu(UpMenu).ShortcutKeys      = Keys.Control | Keys.Up;
            Menu(DownMenu).ShortcutKeys    = Keys.Control | Keys.Down;
            Menu(RemoveMenu).ShortcutKeys  = Keys.Control | Keys.D;
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
            PreviewMenu.Click += (s, e) => Aggregator?.Preview.Raise();
            UpMenu.Click      += (s, e) => Aggregator?.Move.Raise(ValueEventArgs.Create(-1));
            DownMenu.Click    += (s, e) => Aggregator?.Move.Raise(ValueEventArgs.Create(1));
            RemoveMenu.Click  += (s, e) => Aggregator?.Remove.Raise();
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
                UpMenu,
                DownMenu,
                new ToolStripSeparator(),
                RemoveMenu,
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
