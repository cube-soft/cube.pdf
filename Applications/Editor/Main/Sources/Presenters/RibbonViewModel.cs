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
using System;
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
    public sealed class RibbonViewModel : ViewModelBase<MainBindable>
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
        public RibbonViewModel(MainBindable src,
            Aggregator aggregator,
            SynchronizationContext context
        ) : base(src, aggregator, context)
        {
            if (src != null)
            {
                src.Source.PropertyChanged += WhenPropertyChanged;
                src.Busy.PropertyChanged   += WhenPropertyChanged;
            }
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
        ));

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
        ));

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
        ));

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
        ));

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
            GetDispatcher(false)
        ));

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
        ));

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
        ));

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
        ));

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
        ));

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
        ));

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
        ));

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
        ));

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
            () => !Facade.Busy.Value && Facade.IsOpen(),
            GetDispatcher(false)
        ));

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
        ));

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
        ));

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
        ));

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
            GetDispatcher(false)
        ));

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
            () => !Facade.Busy.Value && Facade.IsOpen(),
            GetDispatcher(false)
        ));

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
        ));

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
        ));

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
        ));

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
        ));

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
        ));

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
        ));

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
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// Refresh
        ///
        /// <summary>
        /// Gets a Refresh menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonElement Refresh => Get(() => new RibbonElement(
            nameof(Refresh),
            () => Properties.Resources.MenuRefresh,
            GetDispatcher(false)
        ));

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
        ));

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
        ));

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
        ));

        #endregion

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
            () => Facade.Settings.FrameOnly,
            e  => Facade.Settings.FrameOnly = e,
            GetDispatcher(false)
        ));

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// WhenPropertyChanged
        ///
        /// <summary>
        /// Occurs when the PropertyChanged is fired.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenPropertyChanged(object s, EventArgs e)
        {
            var src = new[] { Insert, Extract, Remove };
            foreach (var re in src) re.Refresh(nameof(RibbonElement.Enabled));
        }

        #endregion
    }
}
