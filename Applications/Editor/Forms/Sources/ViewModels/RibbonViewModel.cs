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
        public ImageMenuEntry File { get; } = new ImageMenuEntry(
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
        public ImageMenuEntry Edit { get; } = new ImageMenuEntry(
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
        public ImageMenuEntry View { get; } = new ImageMenuEntry(
            () => Properties.Resources.MenuView
        );

        #endregion

        #region Buttons

        /* ----------------------------------------------------------------- */
        ///
        /// Preview
        ///
        /// <summary>
        /// Gets the menu that provides functionality to show the preview
        /// dialog.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public MenuEntry Preview { get; } = new MenuEntry(
            () => Properties.Resources.MenuPreview
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Open
        ///
        /// <summary>
        /// 開くメニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ImageMenuEntry Open { get; } = new ImageMenuEntry(
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
        public ImageMenuEntry Save { get; } = new ImageMenuEntry(
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
        public ImageMenuEntry SaveAs { get; } = new ImageMenuEntry(
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
        public ImageMenuEntry Close { get; } = new ImageMenuEntry(
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
        public ImageMenuEntry Exit { get; } = new ImageMenuEntry(
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
        public ImageMenuEntry Undo { get; } = new ImageMenuEntry(
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
        public ImageMenuEntry Redo { get; } = new ImageMenuEntry(
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
        public ImageMenuEntry Select { get; } = new ImageMenuEntry(
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
        public ImageMenuEntry SelectAll { get; } = new ImageMenuEntry(
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
        public ImageMenuEntry SelectFlip { get; } = new ImageMenuEntry(
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
        public ImageMenuEntry SelectClear { get; } = new ImageMenuEntry(
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
        public ImageMenuEntry Insert { get; } = new ImageMenuEntry(
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
        public ImageMenuEntry InsertFront { get; } = new ImageMenuEntry(
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
        public ImageMenuEntry InsertBack { get; } = new ImageMenuEntry(
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
        public ImageMenuEntry InsertOthers { get; } = new ImageMenuEntry(
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
        public ImageMenuEntry Extract { get; } = new ImageMenuEntry(
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
        public ImageMenuEntry ExtractOthers { get; } = new ImageMenuEntry(
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
        public ImageMenuEntry Remove { get; } = new ImageMenuEntry(
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
        public ImageMenuEntry RemoveOthers { get; } = new ImageMenuEntry(
            () => Properties.Resources.MenuRemoveOthers
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
        public ImageMenuEntry MoveNext { get; } = new ImageMenuEntry(
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
        public ImageMenuEntry MovePrevious { get; } = new ImageMenuEntry(
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
        public ImageMenuEntry RotateLeft { get; } = new ImageMenuEntry(
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
        public ImageMenuEntry RotateRight { get; } = new ImageMenuEntry(
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
        public ImageMenuEntry Metadata { get; } = new ImageMenuEntry(
            () => Properties.Resources.MenuMetadata,
            () => Properties.Resources.TooltipMetadata
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
        public ImageMenuEntry Encryption { get; } = new ImageMenuEntry(
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
        public ImageMenuEntry Refresh { get; } = new ImageMenuEntry(
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
        public ImageMenuEntry ZoomIn { get; } = new ImageMenuEntry(
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
        public ImageMenuEntry ZoomOut { get; } = new ImageMenuEntry(
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
        public ImageMenuEntry Settings { get; } = new ImageMenuEntry(
            () => Properties.Resources.MenuSettings
        );

        #endregion

        #endregion
    }
}
