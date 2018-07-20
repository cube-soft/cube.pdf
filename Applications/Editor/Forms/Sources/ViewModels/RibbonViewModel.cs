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
using Cube.Xui;
using GalaSoft.MvvmLight.Messaging;

namespace Cube.Pdf.App.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// RibbonViewModel
    ///
    /// <summary>
    /// Ribbon の ViewModel クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class RibbonViewModel : MessengerViewModel
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// RibbonViewModel
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="messenger">メッセージ伝搬用オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonViewModel(IMessenger messenger) : base(messenger) { }

        #endregion

        #region Properties

        #region Tabs

        /* ----------------------------------------------------------------- */
        ///
        /// File
        ///
        /// <summary>
        /// ファイルメニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry File { get; } = new RibbonEntry(
            () => Properties.Resources.MenuFile
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Edit
        ///
        /// <summary>
        /// 編集メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry Edit { get; } = new RibbonEntry(
            () => Properties.Resources.MenuEdit
        );

        /* ----------------------------------------------------------------- */
        ///
        /// View
        ///
        /// <summary>
        /// 表示メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry View { get; } = new RibbonEntry(
            () => Properties.Resources.MenuView
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Others
        ///
        /// <summary>
        /// その他メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry Others { get; } = new RibbonEntry(
            () => Properties.Resources.MenuOthers
        );

        #endregion

        #region Buttons

        /* ----------------------------------------------------------------- */
        ///
        /// Open
        ///
        /// <summary>
        /// 開くメニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry Open { get; } = new RibbonEntry(
            () => Properties.Resources.MenuOpen
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// 保存メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry Save { get; } = new RibbonEntry(
            () => Properties.Resources.MenuSave,
            () => Properties.Resources.TooltipSave
        );

        /* ----------------------------------------------------------------- */
        ///
        /// SaveAs
        ///
        /// <summary>
        /// 名前を付けて保存メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry SaveAs { get; } = new RibbonEntry(
            () => Properties.Resources.MenuSaveAs
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Close
        ///
        /// <summary>
        /// 閉じるメニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry Close { get; } = new RibbonEntry(
            () => Properties.Resources.MenuClose
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Exit
        ///
        /// <summary>
        /// 終了メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry Exit { get; } = new RibbonEntry(
            () => Properties.Resources.MenuExit
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Undo
        ///
        /// <summary>
        /// 元に戻すメニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry Undo { get; } = new RibbonEntry(
            () => Properties.Resources.MenuUndo
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Redo
        ///
        /// <summary>
        /// やり直しメニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry Redo { get; } = new RibbonEntry(
            () => Properties.Resources.MenuRedo
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Select
        ///
        /// <summary>
        /// 選択メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry Select { get; } = new RibbonEntry(
            () => Properties.Resources.MenuSelect
        );

        /* ----------------------------------------------------------------- */
        ///
        /// SelectAll
        ///
        /// <summary>
        /// すべて選択メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry SelectAll { get; } = new RibbonEntry(
            () => Properties.Resources.MenuSelectAll,
            nameof(Select)
        );

        /* ----------------------------------------------------------------- */
        ///
        /// SelectFlip
        ///
        /// <summary>
        /// 選択の切り替えメニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry SelectFlip { get; } = new RibbonEntry(
            () => Properties.Resources.MenuSelectFlip,
            nameof(Select)
        );

        /* ----------------------------------------------------------------- */
        ///
        /// SelectClear
        ///
        /// <summary>
        /// 選択を解除メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry SelectClear { get; } = new RibbonEntry(
            () => Properties.Resources.MenuSelectClear,
            nameof(Select)
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Insert
        ///
        /// <summary>
        /// 挿入メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry Insert { get; } = new RibbonEntry(
            () => Properties.Resources.MenuInsert,
            () => Properties.Resources.TooltipInsert
        );

        /* ----------------------------------------------------------------- */
        ///
        /// InsertFront
        ///
        /// <summary>
        /// 先頭に挿入メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry InsertFront { get; } = new RibbonEntry(
            () => Properties.Resources.MenuInsertFront,
            nameof(Insert)
        );

        /* ----------------------------------------------------------------- */
        ///
        /// InsertBack
        ///
        /// <summary>
        /// 末尾に挿入メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry InsertBack { get; } = new RibbonEntry(
            () => Properties.Resources.MenuInsertBack,
            nameof(Insert)
        );

        /* ----------------------------------------------------------------- */
        ///
        /// InsertOthers
        ///
        /// <summary>
        /// 詳細を設定して挿入メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry InsertOthers { get; } = new RibbonEntry(
            () => Properties.Resources.MenuInsertOthers
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// 抽出メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry Extract { get; } = new RibbonEntry(
            () => Properties.Resources.MenuExtract,
            () => Properties.Resources.TooltipExtract
        );

        /* ----------------------------------------------------------------- */
        ///
        /// ExtractOthers
        ///
        /// <summary>
        /// Gets the ribbon menu that provides functionality to show
        /// the settings dialog and extract items.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry ExtractOthers { get; } = new RibbonEntry(
            () => Properties.Resources.MenuExtractOthers
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Remove
        ///
        /// <summary>
        /// 削除メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry Remove { get; } = new RibbonEntry(
            () => Properties.Resources.MenuRemove,
            () => Properties.Resources.TooltipRemove
        );

        /* ----------------------------------------------------------------- */
        ///
        /// RemoveOthers
        ///
        /// <summary>
        /// 範囲を指定して削除メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry RemoveOthers { get; } = new RibbonEntry(
            () => Properties.Resources.MenuRemoveRange
        );

        /* ----------------------------------------------------------------- */
        ///
        /// MoveNext
        ///
        /// <summary>
        /// 後ろのページへ移動するメニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry MoveNext { get; } = new RibbonEntry(
            () => Properties.Resources.MenuMoveNext
        );

        /* ----------------------------------------------------------------- */
        ///
        /// MovePrevious
        ///
        /// <summary>
        /// 前のページへ移動するメニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry MovePrevious { get; } = new RibbonEntry(
            () => Properties.Resources.MenuMovePrevious
        );

        /* ----------------------------------------------------------------- */
        ///
        /// RotateLeft
        ///
        /// <summary>
        /// 左 90 度回転メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry RotateLeft { get; } = new RibbonEntry(
            () => Properties.Resources.MenuRotateLeft
        );

        /* ----------------------------------------------------------------- */
        ///
        /// RotateRight
        ///
        /// <summary>
        /// 右 90 度回転メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry RotateRight { get; } = new RibbonEntry(
            () => Properties.Resources.MenuRotateRight
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Metadata
        ///
        /// <summary>
        /// メタ情報メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry Metadata { get; } = new RibbonEntry(
            () => Properties.Resources.MenuMetadata
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Encryption
        ///
        /// <summary>
        /// セキュリティメニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry Encryption { get; } = new RibbonEntry(
            () => Properties.Resources.MenuEncryption
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Refresh
        ///
        /// <summary>
        /// 更新メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry Refresh { get; } = new RibbonEntry(
            () => Properties.Resources.MenuRefresh
        );

        /* ----------------------------------------------------------------- */
        ///
        /// ZoomIn
        ///
        /// <summary>
        /// 拡大メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry ZoomIn { get; } = new RibbonEntry(
            () => Properties.Resources.MenuZoomIn
        );

        /* ----------------------------------------------------------------- */
        ///
        /// ZoomOut
        ///
        /// <summary>
        /// 縮小メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry ZoomOut { get; } = new RibbonEntry(
            () => Properties.Resources.MenuZoomOut
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Settings
        ///
        /// <summary>
        /// Gets the settings ribbon menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry Settings { get; } = new RibbonEntry(
            () => Properties.Resources.MenuSettings
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Web
        ///
        /// <summary>
        /// Web メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry Web { get; } = new RibbonEntry(
            () => Properties.Resources.MenuWeb
        );

        #endregion

        #region Others

        /* ----------------------------------------------------------------- */
        ///
        /// Recent
        ///
        /// <summary>
        /// 最近開いたファイルメニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry Recent { get; } = new RibbonEntry(
            () => Properties.Resources.MenuRecent
        );

        #endregion

        #endregion
    }
}
