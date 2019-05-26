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
using System.Threading;

namespace Cube.Pdf.Editor
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
    public class RibbonViewModel : PresentableBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// RibbonViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the RibbonViewModel
        /// class with the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Bindable data.</param>
        /// <param name="aggregator">Message aggregator.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonViewModel(MainBindable src, Aggregator aggregator, SynchronizationContext context) :
            base(aggregator, context)
        {
            File = new RibbonElement(nameof(File),
                () => Properties.Resources.MenuFile,
                src.Dispatcher);

            Edit = new RibbonElement(nameof(Edit),
                () => Properties.Resources.MenuEdit,
                src.Dispatcher);

            Others = new RibbonElement(nameof(Others),
                () => Properties.Resources.MenuOthers,
                src.Dispatcher);

            Preview = new BindableElement(
                () => Properties.Resources.MenuPreview,
                src.Dispatcher);

            Open = new RibbonElement(nameof(Open),
                () => Properties.Resources.MenuOpen,
                src.Dispatcher);

            Save = new RibbonElement(nameof(Save),
                () => Properties.Resources.MenuSave,
                () => Properties.Resources.TooltipSave,
                src.Dispatcher);

            SaveAs = new RibbonElement(nameof(SaveAs),
                () => Properties.Resources.MenuSaveAs,
                src.Dispatcher);

            Close = new RibbonElement(nameof(Close),
                () => Properties.Resources.MenuClose,
                src.Dispatcher);

            Exit = new RibbonElement(nameof(Exit),
                () => Properties.Resources.MenuExit,
                src.Dispatcher);

            Undo = new RibbonElement(nameof(Undo),
                () => Properties.Resources.MenuUndo,
                src.Dispatcher);

            Redo = new RibbonElement(nameof(Redo),
                () => Properties.Resources.MenuRedo,
                src.Dispatcher);

            Select = new RibbonElement(nameof(Select),
                () => Properties.Resources.MenuSelect,
                src.Dispatcher);

            SelectAll = new RibbonElement(nameof(Select),
                () => Properties.Resources.MenuSelectAll,
                src.Dispatcher);

            SelectFlip = new RibbonElement(nameof(Select),
                () => Properties.Resources.MenuSelectFlip,
                src.Dispatcher);

            SelectClear = new RibbonElement(nameof(Select),
                () => Properties.Resources.MenuSelectClear,
                src.Dispatcher);

            Insert = new RibbonElement(nameof(Insert),
                () => Properties.Resources.MenuInsert,
                () => Properties.Resources.TooltipInsert,
                () => !src.Busy.Value && src.IsOpen(),
                src.Dispatcher);

            InsertFront = new RibbonElement(nameof(Insert),
                () => Properties.Resources.MenuInsertFront,
                src.Dispatcher);

            InsertBack = new RibbonElement(nameof(Insert),
                () => Properties.Resources.MenuInsertBack,
                src.Dispatcher);

            InsertOthers = new RibbonElement(nameof(InsertOthers),
                () => Properties.Resources.MenuInsertOthers,
                src.Dispatcher);

            Extract = new RibbonElement(nameof(Extract),
                () => Properties.Resources.MenuExtract,
                () => Properties.Resources.TooltipExtract,
                src.Dispatcher);

            Remove = new RibbonElement(nameof(Remove),
                () => Properties.Resources.MenuRemove,
                () => Properties.Resources.TooltipRemove,
                () => !src.Busy.Value && src.IsOpen(),
                src.Dispatcher);

            RemoveOthers = new RibbonElement(nameof(RemoveOthers),
                () => Properties.Resources.MenuRemoveOthers,
                src.Dispatcher);

            MoveNext = new RibbonElement(nameof(MoveNext),
                () => Properties.Resources.MenuMoveNext,
                src.Dispatcher);

            MovePrevious = new RibbonElement(nameof(MovePrevious),
                () => Properties.Resources.MenuMovePrevious,
                src.Dispatcher);

            RotateLeft = new RibbonElement(nameof(RotateLeft),
                () => Properties.Resources.MenuRotateLeft,
                src.Dispatcher);

            RotateRight = new RibbonElement(nameof(RotateRight),
               () => Properties.Resources.MenuRotateRight,
               src.Dispatcher);

            Metadata = new RibbonElement(nameof(Metadata),
                () => Properties.Resources.MenuMetadata,
                () => Properties.Resources.TooltipMetadata,
                src.Dispatcher);

            Encryption = new RibbonElement(nameof(Encryption),
                () => Properties.Resources.MenuEncryption,
                src.Dispatcher);

            Refresh = new RibbonElement(nameof(Refresh),
                () => Properties.Resources.MenuRefresh,
                src.Dispatcher);

            ZoomIn = new RibbonElement(nameof(ZoomIn),
                () => Properties.Resources.MenuZoomIn,
                src.Dispatcher);

            ZoomOut = new RibbonElement(nameof(ZoomOut),
                () => Properties.Resources.MenuZoomOut,
                src.Dispatcher);

            Settings = new RibbonElement(nameof(Settings),
                () => Properties.Resources.MenuSettings,
                src.Dispatcher);

            FrameOnly = new BindableElement<bool>(
                () => src.Settings.FrameOnly,
                e  => src.Settings.FrameOnly = e,
                () => Properties.Resources.MenuFrameOnly,
                src.Dispatcher);
        }

        #endregion

        #region Properties

        #region Tabs

        /* ----------------------------------------------------------------- */
        ///
        /// File
        ///
        /// <summary>
        /// Gets the file menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement File { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Edit
        ///
        /// <summary>
        /// Gets the edit menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement Edit { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Others
        ///
        /// <summary>
        /// Gets the others menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement Others { get; }

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
        public BindableElement Preview { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Open
        ///
        /// <summary>
        /// 開くメニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement Open { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// 保存メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement Save { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// SaveAs
        ///
        /// <summary>
        /// 名前を付けて保存メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement SaveAs { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Close
        ///
        /// <summary>
        /// 閉じるメニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement Close { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Exit
        ///
        /// <summary>
        /// 終了メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement Exit { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Undo
        ///
        /// <summary>
        /// 元に戻すメニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement Undo { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Redo
        ///
        /// <summary>
        /// やり直しメニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement Redo { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Select
        ///
        /// <summary>
        /// 選択メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement Select { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// SelectAll
        ///
        /// <summary>
        /// すべて選択メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement SelectAll { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// SelectFlip
        ///
        /// <summary>
        /// 選択の切り替えメニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement SelectFlip { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// SelectClear
        ///
        /// <summary>
        /// 選択を解除メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement SelectClear { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Insert
        ///
        /// <summary>
        /// 挿入メニューを取得します。
        /// </summary>
        ///
        /// <remarks>
        /// コンストラクタで初期化します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement Insert { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// InsertFront
        ///
        /// <summary>
        /// 先頭に挿入メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement InsertFront { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// InsertBack
        ///
        /// <summary>
        /// 末尾に挿入メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement InsertBack { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// InsertOthers
        ///
        /// <summary>
        /// 詳細を設定して挿入メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement InsertOthers { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// 抽出メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement Extract { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Remove
        ///
        /// <summary>
        /// 削除メニューを取得します。
        /// </summary>
        ///
        /// <remarks>
        /// コンストラクタで初期化します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement Remove { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// RemoveOthers
        ///
        /// <summary>
        /// 範囲を指定して削除メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement RemoveOthers { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// MoveNext
        ///
        /// <summary>
        /// 後ろのページへ移動するメニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement MoveNext { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// MovePrevious
        ///
        /// <summary>
        /// 前のページへ移動するメニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement MovePrevious { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// RotateLeft
        ///
        /// <summary>
        /// 左 90 度回転メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement RotateLeft { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// RotateRight
        ///
        /// <summary>
        /// 右 90 度回転メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement RotateRight { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Metadata
        ///
        /// <summary>
        /// メタ情報メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement Metadata { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Encryption
        ///
        /// <summary>
        /// セキュリティメニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement Encryption { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Refresh
        ///
        /// <summary>
        /// 更新メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement Refresh { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// ZoomIn
        ///
        /// <summary>
        /// 拡大メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement ZoomIn { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// ZoomOut
        ///
        /// <summary>
        /// 縮小メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement ZoomOut { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Settings
        ///
        /// <summary>
        /// Gets the settings ribbon menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement Settings { get; }

        #endregion

        /* ----------------------------------------------------------------- */
        ///
        /// FrameOnly
        ///
        /// <summary>
        /// Gets the frame only menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<bool> FrameOnly { get; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Raise
        ///
        /// <summary>
        /// Raises the event that Enabled property is changed.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Raise()
        {
            foreach (var e in new[] { Insert, Extract, Remove })
            {
                e.Refresh(nameof(RibbonElement.Enabled));
            }
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases the unmanaged resources used by the MainViewModel
        /// and optionally releases the managed resources.
        /// </summary>
        ///
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        protected override void Dispose(bool disposing) { }

        #endregion
    }
}
