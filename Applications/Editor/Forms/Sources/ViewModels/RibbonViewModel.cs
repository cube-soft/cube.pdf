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
using GalaSoft.MvvmLight;

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
    public class RibbonViewModel : ViewModelBase
    {
        #region Properties

        #region Tabs

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
        /// Version
        ///
        /// <summary>
        /// バージョンメニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry Version { get; } = new RibbonEntry(
            () => Properties.Resources.MenuVersion
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

        #endregion
    }
}
