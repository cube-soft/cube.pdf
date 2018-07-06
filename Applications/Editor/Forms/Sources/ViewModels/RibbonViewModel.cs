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
        public RibbonItem Open { get; } = new RibbonItem(nameof(Open));

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// 保存メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonItem Save { get; } = new RibbonItem(nameof(Save));

        /* ----------------------------------------------------------------- */
        ///
        /// Close
        ///
        /// <summary>
        /// 閉じるメニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonItem Close { get; } = new RibbonItem(nameof(Close));

        /* ----------------------------------------------------------------- */
        ///
        /// Undo
        ///
        /// <summary>
        /// 元に戻すメニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonItem Undo { get; } = new RibbonItem(nameof(Undo));

        /* ----------------------------------------------------------------- */
        ///
        /// Redo
        ///
        /// <summary>
        /// やり直しメニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonItem Redo { get; } = new RibbonItem(nameof(Redo));

        /* ----------------------------------------------------------------- */
        ///
        /// Select
        ///
        /// <summary>
        /// 選択メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonItem Select { get; } = new RibbonItem(nameof(Select));

        /* ----------------------------------------------------------------- */
        ///
        /// Insert
        ///
        /// <summary>
        /// 挿入メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonItem Insert { get; } = new RibbonItem(nameof(Insert));

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// 抽出メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonItem Extract { get; } = new RibbonItem(nameof(Extract), "Pick");

        /* ----------------------------------------------------------------- */
        ///
        /// Remove
        ///
        /// <summary>
        /// 削除メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonItem Remove { get; } = new RibbonItem(nameof(Remove));

        /* ----------------------------------------------------------------- */
        ///
        /// MoveNext
        ///
        /// <summary>
        /// 後ろのページへ移動するメニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonItem MoveNext { get; } = new RibbonItem(nameof(MoveNext), "Next");

        /* ----------------------------------------------------------------- */
        ///
        /// MovePrevious
        ///
        /// <summary>
        /// 前のページへ移動するメニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonItem MovePrevious { get; } = new RibbonItem(nameof(MovePrevious), "Prev");

        /* ----------------------------------------------------------------- */
        ///
        /// RotateLeft
        ///
        /// <summary>
        /// 左 90 度回転メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonItem RotateLeft { get; } = new RibbonItem(nameof(RotateLeft), "Left");

        /* ----------------------------------------------------------------- */
        ///
        /// RotateRight
        ///
        /// <summary>
        /// 右 90 度回転メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonItem RotateRight { get; } = new RibbonItem(nameof(RotateRight), "Right");

        /* ----------------------------------------------------------------- */
        ///
        /// Metadata
        ///
        /// <summary>
        /// メタ情報メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonItem Metadata { get; } = new RibbonItem(nameof(Metadata));

        /* ----------------------------------------------------------------- */
        ///
        /// Encryption
        ///
        /// <summary>
        /// セキュリティメニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonItem Encryption { get; } = new RibbonItem(nameof(Encryption), "Security");

        /* ----------------------------------------------------------------- */
        ///
        /// Refresh
        ///
        /// <summary>
        /// 更新メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonItem Refresh { get; } = new RibbonItem(nameof(Refresh));

        /* ----------------------------------------------------------------- */
        ///
        /// ZoomIn
        ///
        /// <summary>
        /// 拡大メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonItem ZoomIn { get; } = new RibbonItem(nameof(ZoomIn));

        /* ----------------------------------------------------------------- */
        ///
        /// ZoomOut
        ///
        /// <summary>
        /// 縮小メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonItem ZoomOut { get; } = new RibbonItem(nameof(ZoomOut));

        /* ----------------------------------------------------------------- */
        ///
        /// Version
        ///
        /// <summary>
        /// バージョンメニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonItem Version { get; } = new RibbonItem(nameof(Version));

        /* ----------------------------------------------------------------- */
        ///
        /// Web
        ///
        /// <summary>
        /// Web メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonItem Web { get; } = new RibbonItem(nameof(Web));

        #endregion
    }
}
