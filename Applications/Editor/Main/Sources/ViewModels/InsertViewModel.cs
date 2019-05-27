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
    /// Represents the ViewModel for a InsertWindow instance.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class InsertViewModel : DialogViewModel
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
            Model         = new InsertFacade(i, n, io, GetDispatcher(false));
            Position      = new InsertPosition(Data, GetDispatcher(false));
            DragMove      = new InsertDropTarget((f, t) => Model.Move(f, t));
            DragAdd       = new BindableCommand<string[]>(e => Model.Add(e), e => true);
            Preview       = new BindableElement(() => Properties.Resources.MenuPreview, GetDispatcher(false));
            Add           = new BindableElement(() => Properties.Resources.MenuAdd, GetDispatcher(false));
            Remove        = new BindableElement(() => Properties.Resources.MenuRemove, GetDispatcher(false));
            Clear         = new BindableElement(() => Properties.Resources.MenuClear, GetDispatcher(false));
            Up            = new BindableElement(() => Properties.Resources.MenuUp, GetDispatcher(false));
            Down          = new BindableElement(() => Properties.Resources.MenuDown, GetDispatcher(false));
            FileName      = new BindableElement(() => Properties.Resources.MenuFilename, GetDispatcher(false));
            FileType      = new BindableElement(() => Properties.Resources.MenuFiletype, GetDispatcher(false));
            FileLength    = new BindableElement(() => Properties.Resources.MenuFilesize, GetDispatcher(false));
            LastWriteTime = new BindableElement(() => Properties.Resources.MenuLastWriteTime, GetDispatcher(false));
            SetCommands(callback);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Model
        ///
        /// <summary>
        /// Gets the model object of the ViewModel.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected InsertFacade Model { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Data
        ///
        /// <summary>
        /// Gets data for binding to the InsertWindow.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public InsertBindable Data => Model.Bindable;

        /* ----------------------------------------------------------------- */
        ///
        /// Position
        ///
        /// <summary>
        /// Gets the label that represents the insert position.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public InsertPosition Position { get; }

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
        public ICommand DragAdd { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// SelectClear
        ///
        /// <summary>
        /// Gets the SelectClear command.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ICommand SelectClear { get; private set; }

        #region Buttons

        /* ----------------------------------------------------------------- */
        ///
        /// Preview
        ///
        /// <summary>
        /// Gets the menu that represents the preview button.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement Preview { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Add
        ///
        /// <summary>
        /// Gets the menu that represents the add button.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement Add { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Remove
        ///
        /// <summary>
        /// Gets the menu that represents the remove button.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement Remove { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Clear
        ///
        /// <summary>
        /// Gets the menu that represents the clear button.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement Clear { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Up
        ///
        /// <summary>
        /// Gets the menu that represents the up button.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement Up { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Down
        ///
        /// <summary>
        /// Gets the menu that represents the down button.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement Down { get; }

        #endregion

        #region Contents

        /* ----------------------------------------------------------------- */
        ///
        /// FileName
        ///
        /// <summary>
        /// Gets the menu that represents the FileName column.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement FileName { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// FileType
        ///
        /// <summary>
        /// Gets the menu that represents the FileType column.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement FileType { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// FileLength
        ///
        /// <summary>
        /// Gets the menu that represents the FileLength column.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement FileLength { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// LastWriteTime
        ///
        /// <summary>
        /// Gets the menu that represents the LastWriteTime column.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement LastWriteTime { get; }

        #endregion

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// SetCommands
        ///
        /// <summary>
        /// Sets commands of the InsertWindow.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SetCommands(Action<int, IEnumerable<FileItem>> callback)
        {
            OK.Command = new BindableCommand(() =>
            {
                Send<CloseMessage>();
                callback?.Invoke(Data.Index.Value, Data.Files);
            },
            () => Data.Files.Count > 0,
            Data.Files);

            SelectClear     = Any(() => TrackSync(() => Model.SelectClear()));
            Preview.Command = IsItem(() => Track(() => Model.Preview()));
            Add.Command     = Any(() => SendOpen());
            Clear.Command   = Any(() => TrackSync(() => Model.Clear()));
            Remove.Command  = IsItem(() => TrackSync(() => Model.Remove()));
            Up.Command      = IsItem(() => TrackSync(() => Model.Move(-1)));
            Down.Command    = IsItem(() => TrackSync(() => Model.Move(1)));
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
        private ICommand Any(Action action) => new BindableCommand(action, () => true);

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
        private ICommand IsItem(Action action) => new BindableCommand(action,
            () => Data.Selection.Count > 0,
            Data.Selection
        );

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
            var msg = Factory.InsertMessage();
            Send(msg);
            if (!msg.Cancel) Model.Add(msg.Value);
        }

        #endregion

        #endregion
    }
}
