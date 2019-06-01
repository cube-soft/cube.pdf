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
using Cube.Mixin.Observer;
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
    public sealed class InsertViewModel : DialogViewModel
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// InsertViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the InsertViewModel with the
        /// specified argumetns.
        /// </summary>
        ///
        /// <param name="callback">Callback function.</param>
        /// <param name="i">Selected index.</param>
        /// <param name="n">Number of pages.</param>
        /// <param name="io">I/O handler.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public InsertViewModel(Action<int, IEnumerable<FileItem>> callback,
            int i, int n, IO io, SynchronizationContext context) :
            base(() => Properties.Resources.TitleInsert, new Aggregator(), context)
        {
            _model = new InsertFacade(i, n, io, GetDispatcher(false));

            Position   = new InsertPosViewModel(Data, Aggregator, context);
            DragMove   = new InsertDropTarget((f, t) => _model.Move(f, t));
            OK.Command = new DelegateCommand(
                () =>
                {
                    Send<CloseMessage>();
                    callback?.Invoke(Data.Index.Value, Data.Files);
                },
                () => Data.Files.Count > 0
            ).Associate(Data.Files);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Position
        ///
        /// <summary>
        /// Gets a ViewModel of the insertion position menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public InsertPosViewModel Position { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Data
        ///
        /// <summary>
        /// Gets data object associated with the ViewModel.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public InsertBindable Data => _model.Bindable;

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

        /* ----------------------------------------------------------------- */
        ///
        /// DragAdd
        ///
        /// <summary>
        /// Gets the command when files are Drag&amp;Drop.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ICommand DragAdd => Get(() => new DelegateCommand<string[]>(e => _model.Add(e)));

        /* ----------------------------------------------------------------- */
        ///
        /// SelectClear
        ///
        /// <summary>
        /// Gets the SelectClear command.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ICommand SelectClear => Get(() => Any(() => TrackSync(() => _model.SelectClear())));

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
        public BindableElement Preview => Get(() => new BindableElement(
            () => Properties.Resources.MenuPreview,
            GetDispatcher(false)
        ) { Command = IsItem(() => Track(() => _model.Preview())) });

        /* ----------------------------------------------------------------- */
        ///
        /// Add
        ///
        /// <summary>
        /// Gets an Add button.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement Add => Get(() => new BindableElement(
            () => Properties.Resources.MenuAdd,
            GetDispatcher(false)
        ) { Command = Any(() => SendOpen()) });

        /* ----------------------------------------------------------------- */
        ///
        /// Remove
        ///
        /// <summary>
        /// Gets a Remove button.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement Remove => Get(() => new BindableElement(
            () => Properties.Resources.MenuRemove,
            GetDispatcher(false)
        ) { Command = IsItem(() => TrackSync(() => _model.Remove())) });

        /* ----------------------------------------------------------------- */
        ///
        /// Clear
        ///
        /// <summary>
        /// Gets a Clear button.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement Clear => Get(() => new BindableElement(
            () => Properties.Resources.MenuClear,
            GetDispatcher(false)
        ) { Command = Any(() => TrackSync(() => _model.Clear())) });

        /* ----------------------------------------------------------------- */
        ///
        /// Up
        ///
        /// <summary>
        /// Gets an Up button.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement Up => Get(() => new BindableElement(
            () => Properties.Resources.MenuUp,
            GetDispatcher(false)
        ) { Command = IsItem(() => TrackSync(() => _model.Move(-1))) });

        /* ----------------------------------------------------------------- */
        ///
        /// Down
        ///
        /// <summary>
        /// Gets a Down button.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement Down => Get(() => new BindableElement(
            () => Properties.Resources.MenuDown,
            GetDispatcher(false)
        ) { Command = IsItem(() => TrackSync(() => _model.Move(1))) });

        /* ----------------------------------------------------------------- */
        ///
        /// FileName
        ///
        /// <summary>
        /// Gets a menu of the FileName column.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement FileName => Get(() => new BindableElement(
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
        public BindableElement FileType => Get(() => new BindableElement(
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
        public BindableElement FileLength => Get(() => new BindableElement(
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
        public BindableElement LastWriteTime => Get(() => new BindableElement(
            () => Properties.Resources.MenuLastWriteTime,
            GetDispatcher(false)
        ));

        #endregion

        #endregion

        #region Implementations

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

        #region Factory

        /* ----------------------------------------------------------------- */
        ///
        /// Any
        ///
        /// <summary>
        /// Creates a command that can execute at any time.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ICommand Any(Action action) => new DelegateCommand(action);

        /* ----------------------------------------------------------------- */
        ///
        /// IsItem
        ///
        /// <summary>
        /// Creates a command that can execute when any items are
        /// selected.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ICommand IsItem(Action action) => new DelegateCommand(action,
            () => Data.Selection.Count > 0
        ).Associate(Data.Selection);

        #endregion

        #region Send or Post

        /* ----------------------------------------------------------------- */
        ///
        /// TrackSync
        ///
        /// <summary>
        /// Executes the Track method as a synchronous manner.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void TrackSync(Action action) => Track(action, DialogMessage.Create, true);

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
            if (!msg.Cancel) _model.Add(msg.Value);
        }

        #endregion

        #endregion

        #region Fields
        private readonly InsertFacade _model;
        #endregion
    }
}
