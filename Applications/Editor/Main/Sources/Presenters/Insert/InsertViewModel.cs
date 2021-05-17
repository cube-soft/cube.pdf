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
using Cube.FileSystem;
using Cube.Mixin.Observing;
using Cube.Xui;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Input;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// InsertViewModel
    ///
    /// <summary>
    /// Represents the ViewModel associated with a InsertWindow object.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class InsertViewModel : DialogViewModel<InsertFacade>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// InsertViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the InsertViewModel with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="callback">Callback function.</param>
        /// <param name="index">Selected index.</param>
        /// <param name="count">Number of pages.</param>
        /// <param name="io">I/O handler.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public InsertViewModel(Action<int, IEnumerable<FileItem>> callback,
            int index,
            int count,
            IO io,
            SynchronizationContext context
        ) : base(new InsertFacade(index, count, io, new ContextDispatcher(context, false)),
            new Aggregator(),
            context
        ) {
            Position   = new PositionViewModel(Value, Aggregator, context);
            DragMove   = new InsertDropTarget((f, t) => Facade.Move(f, t));
            OK.Command = GetOkCommand(callback);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Value
        ///
        /// <summary>
        /// Gets data object associated with the ViewModel.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public InsertBindable Value => Facade.Value;

        /* ----------------------------------------------------------------- */
        ///
        /// Position
        ///
        /// <summary>
        /// Gets a ViewModel of the insertion position menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PositionViewModel Position { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// DragMove
        ///
        /// <summary>
        /// Gets the Drag&amp;Drop behavior.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public InsertDropTarget DragMove { get; }

        #region Elements

        /* ----------------------------------------------------------------- */
        ///
        /// Preview
        ///
        /// <summary>
        /// Gets a menu of the preview button.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IElement Preview => Get(() => new BindableElement(
            () => Properties.Resources.MenuPreview,
            GetDispatcher(false)
        ) { Command = IsSelected(() => Track(Facade.Preview)) });

        /* ----------------------------------------------------------------- */
        ///
        /// Add
        ///
        /// <summary>
        /// Gets an Add button.
        /// </summary>
        ///
        /// <remarks>
        /// TODO: Rename this property.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public IElement Add => Get(() => new BindableElement(
            () => Properties.Resources.MenuAdd,
            GetDispatcher(false)
        ) { Command = new DelegateCommand(SendOpen) });

        /* ----------------------------------------------------------------- */
        ///
        /// Remove
        ///
        /// <summary>
        /// Gets a Remove button.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IElement Remove => Get(() => new BindableElement(
            () => Properties.Resources.MenuRemove,
            GetDispatcher(false)
        ) { Command = IsSelected(() => Track(Facade.Remove, true)) });

        /* ----------------------------------------------------------------- */
        ///
        /// Clear
        ///
        /// <summary>
        /// Gets a Clear button.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IElement Clear => Get(() => new BindableElement(
            () => Properties.Resources.MenuClear,
            GetDispatcher(false)
        ) { Command = new DelegateCommand(() => Track(Facade.Clear, true)) });

        /* ----------------------------------------------------------------- */
        ///
        /// Up
        ///
        /// <summary>
        /// Gets an Up button.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IElement Up => Get(() => new BindableElement(
            () => Properties.Resources.MenuUp,
            GetDispatcher(false)
        ) { Command = IsSelected(() => Track(() => Facade.Move(-1), true)) });

        /* ----------------------------------------------------------------- */
        ///
        /// Down
        ///
        /// <summary>
        /// Gets a Down button.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IElement Down => Get(() => new BindableElement(
            () => Properties.Resources.MenuDown,
            GetDispatcher(false)
        ) { Command = IsSelected(() => Track(() => Facade.Move(1), true)) });

        /* ----------------------------------------------------------------- */
        ///
        /// FileName
        ///
        /// <summary>
        /// Gets a menu of the FileName column.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IElement FileName => Get(() => new BindableElement(
            () => Properties.Resources.MenuFilename,
            GetDispatcher(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// FileType
        ///
        /// <summary>
        /// Gets a menu of the FileType column.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IElement FileType => Get(() => new BindableElement(
            () => Properties.Resources.MenuFiletype,
            GetDispatcher(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// FileLength
        ///
        /// <summary>
        /// Gets a menu of the FileLength column.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IElement FileLength => Get(() => new BindableElement(
            () => Properties.Resources.MenuFilesize,
            GetDispatcher(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// LastWriteTime
        ///
        /// <summary>
        /// Gets a menu of the LastWriteTime column.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IElement LastWriteTime => Get(() => new BindableElement(
            () => Properties.Resources.MenuLastWriteTime,
            GetDispatcher(false)
        ));

        #endregion

        #region Commands

        /* ----------------------------------------------------------------- */
        ///
        /// DragAdd
        ///
        /// <summary>
        /// Gets the command when files are Drag&amp;Drop.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ICommand DragAdd => Get(() =>
            new DelegateCommand<string[]>(e => Track(() => Facade.Add(e), true))
        );

        /* ----------------------------------------------------------------- */
        ///
        /// SelectClear
        ///
        /// <summary>
        /// Gets the SelectClear command.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ICommand SelectClear => Get(() =>
            new DelegateCommand(() => Track(Facade.SelectClear, true))
        );

        #endregion

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetTitle
        ///
        /// <summary>
        /// Gets the title of the dialog.
        /// </summary>
        ///
        /// <returns>String value.</returns>
        ///
        /* ----------------------------------------------------------------- */
        protected override string GetTitle() => Properties.Resources.TitleInsert;

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases the unmanaged resources used by the object and
        /// optionally releases the managed resources.
        /// </summary>
        ///
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        protected override void Dispose(bool disposing)
        {
            try { if (disposing) Position.Dispose(); }
            finally { base.Dispose(disposing); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SendOpen
        ///
        /// <summary>
        /// Posts the message to show a dialog of the OpenFileDialog
        /// class, and executes the specified action as an asynchronous
        /// operation.
        /// </summary>
        ///
        /// <remarks>
        /// 複数ファイルを非同期で追加した際にエラーが発生する場合が確認
        /// されているため、暫定的に同期的に追加しています。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private void SendOpen()
        {
            var msg = MessageFactory.CreateForInsert();
            Send(msg);
            if (!msg.Cancel) Facade.Add(msg.Value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetOkCommand
        ///
        /// <summary>
        /// Gets the OK command.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ICommand GetOkCommand(Action<int, IEnumerable<FileItem>> callback)
        {
            var dest = new DelegateCommand(
                () => {
                    Send<CloseMessage>();
                    callback?.Invoke(Value.Index, Value.Files);
                },
                () => Value.Files.Count > 0
            );

            Value.Files.CollectionChanged += (s, e) => dest.Refresh();
            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// IsSelected
        ///
        /// <summary>
        /// Creates a command that can execute when any items are
        /// selected.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ICommand IsSelected(Action action) => new DelegateCommand(action,
            () => Value.Selection.Count > 0
        ).Associate(Value.Selection);

        #endregion
    }
}
