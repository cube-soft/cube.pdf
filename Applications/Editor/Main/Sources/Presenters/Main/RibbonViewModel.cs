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
using System.Threading;
using Cube.Mixin.Observable;
using Cube.Xui;

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
            GetDispatcher(false)
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
            GetDispatcher(false)
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
            GetDispatcher(false)
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
            GetDispatcher(false)
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
            GetDispatcher(false)
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
            GetDispatcher(false)
        ) { Command = IsOpen(() => Run(Facade.Overwrite, false)) });

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
            GetDispatcher(false)
        ) { Command = IsOpen(() => SendSave(Facade.Save)) });

        /* ----------------------------------------------------------------- */
        ///
        /// Close
        ///
        /// <summary>
        /// Gets a menu to close the current PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement Close => Get(() => new RibbonElement(
            nameof(Close),
            () => Properties.Resources.MenuClose,
            GetDispatcher(false)
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
            GetDispatcher(false)
        ) { Command = GetCommand(() => Send(new CloseMessage())) });

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
            GetDispatcher(false)
        ) { Command = IsUndoable(() => Run(Facade.Undo, true)) });

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
            GetDispatcher(false)
        ) { Command = IsRedoable(() => Run(Facade.Redo, true)) });

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
            GetDispatcher(false)
        ) { Command = IsOpen(() => Run(Facade.Select, true)) });

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
            GetDispatcher(false)
        ) { Command = IsOpen(() => Run(() => Facade.Select(true), true)) });

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
            GetDispatcher(false)
        ) { Command = IsOpen(() => Run(Facade.Flip, true)) });

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
            GetDispatcher(false)
        ) { Command = IsOpen(() => Run(() => Facade.Select(false), true)) });

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
            GetDispatcher(false)
        ) {
            Command = IsSelected(() => SendInsert(Facade.Value.Images.Selection.Last + 1))
        }.Hook(Facade.Value, nameof(MainBindableValue.Busy), nameof(MainBindableValue.Source)));

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
            GetDispatcher(false)
        ) { Command = IsOpen(() => SendInsert(0)) });

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
            GetDispatcher(false)
        ) { Command = IsOpen(() => SendInsert(int.MaxValue)) });

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
            GetDispatcher(false)
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
            GetDispatcher(false)
        ) {
            Command = IsSelected(() => SendSave(Facade.Extract))
        }.Hook(Facade.Value, nameof(MainBindableValue.Busy), nameof(MainBindableValue.Source)));

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
            GetDispatcher(false)
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
            GetDispatcher(false)
        ) {
            Command = IsSelected(() => Run(Facade.Remove, true))
        }.Hook(Facade.Value, nameof(MainBindableValue.Busy), nameof(MainBindableValue.Source)));

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
            GetDispatcher(false)
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
            GetDispatcher(false)
        ) { Command = IsSelected(() => Run(() => Facade.Move(1), true)) });

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
            GetDispatcher(false)
        ) { Command = IsSelected(() => Run(() => Facade.Move(-1), true)) });

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
            GetDispatcher(false)
        ) { Command = IsSelected(() => Run(() => Facade.Rotate(-90), true)) });

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
            GetDispatcher(false)
        ) { Command = IsSelected(() => Run(() => Facade.Rotate(90), true)) });

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
            GetDispatcher(false)
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
            GetDispatcher(false)
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
            GetDispatcher(false)
        ) { Command = IsOpen(() => Run(Facade.Redraw, true)) });

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
            GetDispatcher(false)
        ) { Command = GetCommand(() => Run(() => Facade.Zoom(1), true)) });

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
            GetDispatcher(false)
        ) { Command = GetCommand(() => Run(() => Facade.Zoom(-1), true)) });

        /* ----------------------------------------------------------------- */
        ///
        /// Help
        ///
        /// <summary>
        /// Gets a menu to show the help page.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement Help => Get(() => new RibbonElement(
            nameof(Help),
            () => Properties.Resources.MenuHelp,
            GetDispatcher(false)
        ) { Command = GetCommand(() => Send(Facade.Folder.DocumentUri)) });

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
            GetDispatcher(false)
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
            GetDispatcher(false)
        ));

        #endregion

        #endregion
    }
}
