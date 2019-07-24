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
using Cube.Mixin.Observing;
using Cube.Xui;
using System.Threading;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// RibbonViewModel
    ///
    /// <summary>
    /// Represents the ViewModel of Ribbon menu items.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class RibbonViewModel : MainViewModelBase
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
        /// <param name="src">Facade object.</param>
        /// <param name="aggregator">Message aggregator.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonViewModel(MainFacade src,
            Aggregator aggregator,
            SynchronizationContext context
        ) : base(src, aggregator, context) { }

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
        public RibbonElement File => Get(() => new RibbonElement(
            nameof(File),
            () => Properties.Resources.MenuFile,
            GetInvoker(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// Edit
        ///
        /// <summary>
        /// Gets the edit menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement Edit => Get(() => new RibbonElement(
            nameof(Edit),
            () => Properties.Resources.MenuEdit,
            GetInvoker(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// Others
        ///
        /// <summary>
        /// Gets the others menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement Others => Get(() => new RibbonElement(
            nameof(Others),
            () => Properties.Resources.MenuOthers,
            GetInvoker(false)
        ));

        #endregion

        #region Buttons

        /* ----------------------------------------------------------------- */
        ///
        /// Preview
        ///
        /// <summary>
        /// Gets a menu show a preview dialog.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement Preview => Get(() => new BindableElement(
            () => Properties.Resources.MenuPreview,
            GetInvoker(false)
        ) { Command = IsSelected(SendPreview) });

        /* ----------------------------------------------------------------- */
        ///
        /// Open
        ///
        /// <summary>
        /// Gets an Open menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement Open => Get(() => new RibbonElement(
            nameof(Open),
            () => Properties.Resources.MenuOpen,
            GetInvoker(false)
        ) { Command = GetCommand(() => SendOpen(e => Facade.Open(e))) });

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// Gets a Save menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement Save => Get(() => new RibbonElement(
            nameof(Save),
            () => Properties.Resources.MenuSave,
            () => Properties.Resources.TooltipSave,
            GetInvoker(false)
        ) { Command = IsOpen(() => Track(Facade.Overwrite)) });

        /* ----------------------------------------------------------------- */
        ///
        /// SaveAs
        ///
        /// <summary>
        /// Gets a SaveAs menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement SaveAs => Get(() => new RibbonElement(
            nameof(SaveAs),
            () => Properties.Resources.MenuSaveAs,
            GetInvoker(false)
        ) { Command = IsOpen(() => SendSave(Facade.Save)) });

        /* ----------------------------------------------------------------- */
        ///
        /// Close
        ///
        /// <summary>
        /// Gets a menu to closes the current PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement Close => Get(() => new RibbonElement(
            nameof(Close),
            () => Properties.Resources.MenuClose,
            GetInvoker(false)
        ) { Command = GetCloseCommand() });

        /* ----------------------------------------------------------------- */
        ///
        /// Exit
        ///
        /// <summary>
        /// Gets a menu to terminate the application.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement Exit => Get(() => new RibbonElement(
            nameof(Exit),
            () => Properties.Resources.MenuExit,
            GetInvoker(false)
        ) { Command = GetCommand(Send<CloseMessage>) });

        /* ----------------------------------------------------------------- */
        ///
        /// Undo
        ///
        /// <summary>
        /// Gets an Undo menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement Undo => Get(() => new RibbonElement(
            nameof(Undo),
            () => Properties.Resources.MenuUndo,
            GetInvoker(false)
        ) { Command = IsUndoable(() => Sync(Facade.Undo)) });

        /* ----------------------------------------------------------------- */
        ///
        /// Redo
        ///
        /// <summary>
        /// Gets a Redo menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement Redo => Get(() => new RibbonElement(
            nameof(Redo),
            () => Properties.Resources.MenuRedo,
            GetInvoker(false)
        ) { Command = IsRedoable(() => Sync(Facade.Redo)) });

        /* ----------------------------------------------------------------- */
        ///
        /// Select
        ///
        /// <summary>
        /// Gets a Select menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement Select => Get(() => new RibbonElement(
            nameof(Select),
            () => Properties.Resources.MenuSelect,
            GetInvoker(false)
        ) { Command = IsOpen(() => Sync(Facade.Select)) });

        /* ----------------------------------------------------------------- */
        ///
        /// SelectAll
        ///
        /// <summary>
        /// Gets a menu to select all items.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement SelectAll => Get(() => new RibbonElement(
            nameof(Select),
            () => Properties.Resources.MenuSelectAll,
            GetInvoker(false)
        ) { Command = IsOpen(() => Sync(() => Facade.Select(true))) });

        /* ----------------------------------------------------------------- */
        ///
        /// SelectFlip
        ///
        /// <summary>
        /// Gets a menu to flip the current selection.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement SelectFlip => Get(() => new RibbonElement(
            nameof(Select),
            () => Properties.Resources.MenuSelectFlip,
            GetInvoker(false)
        ) { Command = IsOpen(() => Sync(Facade.Flip)) });

        /* ----------------------------------------------------------------- */
        ///
        /// SelectClear
        ///
        /// <summary>
        /// Gets a menu to clear the current selection.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement SelectClear => Get(() => new RibbonElement(
            nameof(Select),
            () => Properties.Resources.MenuSelectClear,
            GetInvoker(false)
        ) { Command = IsOpen(() => Sync(() => Facade.Select(false))) });

        /* ----------------------------------------------------------------- */
        ///
        /// Insert
        ///
        /// <summary>
        /// Gets an Insert menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement Insert => Get(() => new RibbonElement(
            nameof(Insert),
            () => Properties.Resources.MenuInsert,
            () => Properties.Resources.TooltipInsert,
            () => !Facade.Value.Busy,
            GetInvoker(false)
        ) {
            Command = IsSelected(() => SendInsert(Facade.Insert))
        }.Associate(Facade.Value, nameof(MainBindable.Busy), nameof(MainBindable.Source)));

        /* ----------------------------------------------------------------- */
        ///
        /// InsertFront
        ///
        /// <summary>
        /// Gets a menu to insert other files at the beginning.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement InsertFront => Get(() => new RibbonElement(
            nameof(Insert),
            () => Properties.Resources.MenuInsertFront,
            GetInvoker(false)
        ) { Command = IsOpen(() => SendInsert(e => Facade.Insert(0, e))) });

        /* ----------------------------------------------------------------- */
        ///
        /// InsertBack
        ///
        /// <summary>
        /// Gets a menu to insert other files at the end.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement InsertBack => Get(() => new RibbonElement(
            nameof(Insert),
            () => Properties.Resources.MenuInsertBack,
            GetInvoker(false)
        ) { Command = IsOpen(() => SendInsert(e => Facade.Insert(int.MaxValue, e))) });

        /* ----------------------------------------------------------------- */
        ///
        /// InsertOthers
        ///
        /// <summary>
        /// Gets a menu to show an insert dialog.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement InsertOthers => Get(() => new RibbonElement(
            nameof(InsertOthers),
            () => Properties.Resources.MenuInsertOthers,
            GetInvoker(false)
        ) { Command = IsOpen(SendInsert) });

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// Gets an Extract menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement Extract => Get(() => new RibbonElement(
            nameof(Extract),
            () => Properties.Resources.MenuExtract,
            () => Properties.Resources.TooltipExtract,
            () => !Facade.Value.Busy,
            GetInvoker(false)
        ) {
            Command = IsSelected(() => SendSave(Facade.Extract))
        }.Associate(Facade.Value, nameof(MainBindable.Busy), nameof(MainBindable.Source)));

        /* ----------------------------------------------------------------- */
        ///
        /// ExtractOthers
        ///
        /// <summary>
        /// Gets a menu to show an extract dialog.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement ExtractOthers => Get(() => new RibbonElement(
            nameof(ExtractOthers),
            () => Properties.Resources.MenuExtractOthers,
            GetInvoker(false)
        ) { Command = IsOpen(SendExtract) });

        /* ----------------------------------------------------------------- */
        ///
        /// Remove
        ///
        /// <summary>
        /// Gets a Remove menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement Remove => Get(() => new RibbonElement(
            nameof(Remove),
            () => Properties.Resources.MenuRemove,
            () => Properties.Resources.TooltipRemove,
            () => !Facade.Value.Busy,
            GetInvoker(false)
        ) {
            Command = IsSelected(() => Sync(Facade.Remove))
        }.Associate(Facade.Value, nameof(MainBindable.Busy), nameof(MainBindable.Source)));

        /* ----------------------------------------------------------------- */
        ///
        /// RemoveOthers
        ///
        /// <summary>
        /// Gets a menu to show a remove dialog.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement RemoveOthers => Get(() => new RibbonElement(
            nameof(RemoveOthers),
            () => Properties.Resources.MenuRemoveOthers,
            GetInvoker(false)
        ) { Command = IsOpen(SendRemove) });

        /* ----------------------------------------------------------------- */
        ///
        /// MoveNext
        ///
        /// <summary>
        /// Gets a menu to move the selected page to the next.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement MoveNext => Get(() => new RibbonElement(
            nameof(MoveNext),
            () => Properties.Resources.MenuMoveNext,
            GetInvoker(false)
        ) { Command = IsSelected(() => Sync(() => Facade.Move(1))) });

        /* ----------------------------------------------------------------- */
        ///
        /// MovePrevious
        ///
        /// <summary>
        /// Gets a menu to move the selected page to the previous.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement MovePrevious => Get(() => new RibbonElement(
            nameof(MovePrevious),
            () => Properties.Resources.MenuMovePrevious,
            GetInvoker(false)
        ) { Command = IsSelected(() => Sync(() => Facade.Move(-1))) });

        /* ----------------------------------------------------------------- */
        ///
        /// RotateLeft
        ///
        /// <summary>
        /// Gets a menu to rotate 90 degrees left.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement RotateLeft => Get(() => new RibbonElement(
            nameof(RotateLeft),
            () => Properties.Resources.MenuRotateLeft,
            GetInvoker(false)
        ) { Command = IsSelected(() => Sync(() => Facade.Rotate(-90))) });

        /* ----------------------------------------------------------------- */
        ///
        /// RotateRight
        ///
        /// <summary>
        /// Gets a menu to rotate 90 degrees right.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement RotateRight => Get(() => new RibbonElement(
            nameof(RotateRight),
            () => Properties.Resources.MenuRotateRight,
            GetInvoker(false)
        ) { Command = IsSelected(() => Sync(() => Facade.Rotate(90))) });

        /* ----------------------------------------------------------------- */
        ///
        /// Metadata
        ///
        /// <summary>
        /// Gets a menu to show a metadata dialog.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement Metadata => Get(() => new RibbonElement(
            nameof(Metadata),
            () => Properties.Resources.MenuMetadata,
            () => Properties.Resources.TooltipMetadata,
            GetInvoker(false)
        ) { Command = IsOpen(SendMetadata) });

        /* ----------------------------------------------------------------- */
        ///
        /// Encryption
        ///
        /// <summary>
        /// Gets a menu to show an encryption dialog.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement Encryption => Get(() => new RibbonElement(
            nameof(Encryption),
            () => Properties.Resources.MenuEncryption,
            GetInvoker(false)
        ) { Command = IsOpen(SendEncryption) });

        /* ----------------------------------------------------------------- */
        ///
        /// Redraw
        ///
        /// <summary>
        /// Gets a Redraw menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement Redraw => Get(() => new RibbonElement(
            nameof(Redraw),
            () => Properties.Resources.MenuRedraw,
            GetInvoker(false)
        ) { Command = IsOpen(() => Sync(Facade.Redraw)) });

        /* ----------------------------------------------------------------- */
        ///
        /// ZoomIn
        ///
        /// <summary>
        /// Gets a ZoomIn menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement ZoomIn => Get(() => new RibbonElement(
            nameof(ZoomIn),
            () => Properties.Resources.MenuZoomIn,
            GetInvoker(false)
        ) { Command = GetCommand(() => Sync(() => Facade.Zoom(1))) });

        /* ----------------------------------------------------------------- */
        ///
        /// ZoomOut
        ///
        /// <summary>
        /// Gets a ZoomOut menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement ZoomOut => Get(() => new RibbonElement(
            nameof(ZoomOut),
            () => Properties.Resources.MenuZoomOut,
            GetInvoker(false)
        ) { Command = GetCommand(() => Sync(() => Facade.Zoom(-1))) });

        /* ----------------------------------------------------------------- */
        ///
        /// Setting
        ///
        /// <summary>
        /// Gets a menu to show a setting dialog.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement Setting => Get(() => new RibbonElement(
            nameof(Setting),
            () => Properties.Resources.MenuSetting,
            GetInvoker(false)
        ) { Command = GetCommand(SendSetting) });

        #endregion

        #region Others

        /* ----------------------------------------------------------------- */
        ///
        /// FrameOnly
        ///
        /// <summary>
        /// Gets a menu to determine whether to show only the frame.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IElement<bool> FrameOnly => Get(() => new BindableElement<bool>(
            () => Properties.Resources.MenuFrameOnly,
            () => Facade.Value.Settings.FrameOnly,
            e  => Facade.Value.Settings.FrameOnly = e,
            GetInvoker(false)
        ));

        #endregion

        #endregion
    }
}
