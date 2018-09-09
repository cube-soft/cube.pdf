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
        /// Initializes a new instance of the <c>RibbonViewModel</c>
        /// class with the specified arguments.
        /// </summary>
        ///
        /// <param name="getEnabled">
        /// Function to get value indicating whether some ribbon buttons
        /// are enabled.
        /// </param>
        ///
        /// <param name="messenger">Messenger object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonViewModel(Getter<bool> getEnabled, IMessenger messenger) : base(messenger)
        {
            Insert = new RibbonElement(
                () => Properties.Resources.MenuInsert,
                () => Properties.Resources.TooltipInsert,
                getEnabled,
                nameof(Insert)
            );

            Extract = new RibbonElement(
                () => Properties.Resources.MenuExtract,
                () => Properties.Resources.TooltipExtract,
                getEnabled,
                nameof(Extract)
            );

            Remove = new RibbonElement(
                () => Properties.Resources.MenuRemove,
                () => Properties.Resources.TooltipRemove,
                getEnabled,
                nameof(Remove)
            );
        }

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
        public RibbonElement File { get; } = new RibbonElement(
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
        public RibbonElement Edit { get; } = new RibbonElement(
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
        public RibbonElement View { get; } = new RibbonElement(
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
        public BindableElement Preview { get; } = new BindableElement(
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
        public RibbonElement Open { get; } = new RibbonElement(
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
        public RibbonElement Save { get; } = new RibbonElement(
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
        public RibbonElement SaveAs { get; } = new RibbonElement(
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
        public RibbonElement Close { get; } = new RibbonElement(
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
        public RibbonElement Exit { get; } = new RibbonElement(
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
        public RibbonElement Undo { get; } = new RibbonElement(
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
        public RibbonElement Redo { get; } = new RibbonElement(
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
        public RibbonElement Select { get; } = new RibbonElement(
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
        public RibbonElement SelectAll { get; } = new RibbonElement(
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
        public RibbonElement SelectFlip { get; } = new RibbonElement(
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
        public RibbonElement SelectClear { get; } = new RibbonElement(
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
        public RibbonElement InsertFront { get; } = new RibbonElement(
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
        public RibbonElement InsertBack { get; } = new RibbonElement(
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
        public RibbonElement InsertOthers { get; } = new RibbonElement(
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
        /// <remarks>
        /// コンストラクタで初期化します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement Extract { get; }

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
        public RibbonElement ExtractOthers { get; } = new RibbonElement(
            () => Properties.Resources.MenuExtractOthers
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Split
        ///
        /// <summary>
        /// Gets the ribbon menu of splitting the selected pages.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement Split { get; } = new RibbonElement(
            () => Properties.Resources.MenuSplit,
            () => Properties.Resources.MenuSplit,
            nameof(Extract)
        );

        /* ----------------------------------------------------------------- */
        ///
        /// SplitAll
        ///
        /// <summary>
        /// Gets the ribbon menu of splitting all pages.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement SplitAll { get; } = new RibbonElement(
            () => Properties.Resources.MenuSplitAll,
            () => Properties.Resources.MenuSplitAll,
            nameof(Extract)
        );

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
        public RibbonElement RemoveOthers { get; } = new RibbonElement(
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
        public RibbonElement MoveNext { get; } = new RibbonElement(
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
        public RibbonElement MovePrevious { get; } = new RibbonElement(
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
        public RibbonElement RotateLeft { get; } = new RibbonElement(
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
        public RibbonElement RotateRight { get; } = new RibbonElement(
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
        public RibbonElement Metadata { get; } = new RibbonElement(
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
        public RibbonElement Encryption { get; } = new RibbonElement(
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
        public RibbonElement Refresh { get; } = new RibbonElement(
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
        public RibbonElement ZoomIn { get; } = new RibbonElement(
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
        public RibbonElement ZoomOut { get; } = new RibbonElement(
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
        public RibbonElement Settings { get; } = new RibbonElement(
            () => Properties.Resources.MenuSettings
        );

        #endregion

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// RaiseEnabledChanged
        ///
        /// <summary>
        /// Raises the event that Enabled property is changed.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void RaiseEnabledChanged()
        {
            var name = nameof(RibbonElement.Enabled);

            Insert.RaisePropertyChanged(name);
            Extract.RaisePropertyChanged(name);
            Remove.RaisePropertyChanged(name);
        }

        #endregion
    }
}
