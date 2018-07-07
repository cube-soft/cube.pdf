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

        /* ----------------------------------------------------------------- */
        ///
        /// Open
        ///
        /// <summary>
        /// 開くメニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry Open { get; } =
            new RibbonEntry(nameof(Open), () => Properties.Resources.MenuOpen);

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// 保存メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry Save { get; } =
            new RibbonEntry(nameof(Save), () => Properties.Resources.MenuSave);

        /* ----------------------------------------------------------------- */
        ///
        /// Close
        ///
        /// <summary>
        /// 閉じるメニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry Close { get; } =
            new RibbonEntry(nameof(Close), () => Properties.Resources.MenuClose);

        /* ----------------------------------------------------------------- */
        ///
        /// Undo
        ///
        /// <summary>
        /// 元に戻すメニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry Undo { get; } =
            new RibbonEntry(nameof(Undo), () => Properties.Resources.MenuUndo);

        /* ----------------------------------------------------------------- */
        ///
        /// Redo
        ///
        /// <summary>
        /// やり直しメニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry Redo { get; } =
            new RibbonEntry(nameof(Redo), () => Properties.Resources.MenuRedo);

        /* ----------------------------------------------------------------- */
        ///
        /// Select
        ///
        /// <summary>
        /// 選択メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry Select { get; } =
            new RibbonEntry(nameof(Select), () => Properties.Resources.MenuSelect);

        /* ----------------------------------------------------------------- */
        ///
        /// Insert
        ///
        /// <summary>
        /// 挿入メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry Insert { get; } =
            new RibbonEntry(nameof(Insert), () => Properties.Resources.MenuInsert);

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// 抽出メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry Extract { get; } =
            new RibbonEntry(nameof(Extract), () => Properties.Resources.MenuExtract);

        /* ----------------------------------------------------------------- */
        ///
        /// Remove
        ///
        /// <summary>
        /// 削除メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry Remove { get; } =
            new RibbonEntry(nameof(Remove), () => Properties.Resources.MenuRemove);

        /* ----------------------------------------------------------------- */
        ///
        /// MoveNext
        ///
        /// <summary>
        /// 後ろのページへ移動するメニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry MoveNext { get; } =
            new RibbonEntry(nameof(MoveNext), () => Properties.Resources.MenuMoveNext);

        /* ----------------------------------------------------------------- */
        ///
        /// MovePrevious
        ///
        /// <summary>
        /// 前のページへ移動するメニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry MovePrevious { get; } =
            new RibbonEntry(nameof(MovePrevious), () => Properties.Resources.MenuMovePrevious);

        /* ----------------------------------------------------------------- */
        ///
        /// RotateLeft
        ///
        /// <summary>
        /// 左 90 度回転メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry RotateLeft { get; } =
            new RibbonEntry(nameof(RotateLeft), () => Properties.Resources.MenuRotateLeft);

        /* ----------------------------------------------------------------- */
        ///
        /// RotateRight
        ///
        /// <summary>
        /// 右 90 度回転メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry RotateRight { get; } =
            new RibbonEntry(nameof(RotateRight), () => Properties.Resources.MenuRotateRight);

        /* ----------------------------------------------------------------- */
        ///
        /// Metadata
        ///
        /// <summary>
        /// メタ情報メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry Metadata { get; } =
            new RibbonEntry(nameof(Metadata), () => Properties.Resources.MenuMetadata);

        /* ----------------------------------------------------------------- */
        ///
        /// Encryption
        ///
        /// <summary>
        /// セキュリティメニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry Encryption { get; } =
            new RibbonEntry(nameof(Encryption), () => Properties.Resources.MenuEncryption);

        /* ----------------------------------------------------------------- */
        ///
        /// Refresh
        ///
        /// <summary>
        /// 更新メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry Refresh { get; } =
            new RibbonEntry(nameof(Refresh), () => Properties.Resources.MenuRefresh);

        /* ----------------------------------------------------------------- */
        ///
        /// ZoomIn
        ///
        /// <summary>
        /// 拡大メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry ZoomIn { get; } =
            new RibbonEntry(nameof(ZoomIn), () => Properties.Resources.MenuZoomIn);

        /* ----------------------------------------------------------------- */
        ///
        /// ZoomOut
        ///
        /// <summary>
        /// 縮小メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry ZoomOut { get; } =
            new RibbonEntry(nameof(ZoomOut), () => Properties.Resources.MenuZoomOut);

        /* ----------------------------------------------------------------- */
        ///
        /// Version
        ///
        /// <summary>
        /// バージョンメニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry Version { get; } =
            new RibbonEntry(nameof(Version), () => Properties.Resources.MenuVersion);

        /* ----------------------------------------------------------------- */
        ///
        /// Web
        ///
        /// <summary>
        /// Web メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry Web { get; } =
            new RibbonEntry(nameof(Web), () => Properties.Resources.MenuWeb);

        #endregion
    }
}
