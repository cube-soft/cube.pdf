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
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using Cube.FileSystem;
using Cube.Observable.Extensions;
using Cube.Pdf.Extensions;
using Cube.Xui;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// MainViewModelBase
    ///
    /// <summary>
    /// Provides functionality to communicate with the MainWindow.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public abstract class MainViewModelBase : PresentableBase<MainFacade>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// MainViewModelBase
        ///
        /// <summary>
        /// Initializes a new instance of the MainViewModel class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="facade">Facade object.</param>
        /// <param name="aggregator">Message aggregator.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected MainViewModelBase(MainFacade facade,
            Aggregator aggregator,
            SynchronizationContext context
        ) : base(facade, aggregator, context) { }

        #endregion

        #region Commands

        /* ----------------------------------------------------------------- */
        ///
        /// GetOpenLinkCommand
        ///
        /// <summary>
        /// Gets a new command to open the selected link.
        /// </summary>
        ///
        /// <returns>ICommand object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        protected ICommand GetOpenLinkCommand() => new DelegateCommand<object>(
            e => Run(() => Facade.OpenLink(e as Entity), false),
            e => !Facade.Value.Busy && e is Entity
        ).Hook(Facade.Value, nameof(MainBindableValue.Busy));

        /* ----------------------------------------------------------------- */
        ///
        /// GetCloseCommand
        ///
        /// <summary>
        /// Gets a new command to close the document.
        /// </summary>
        ///
        /// <returns>ICommand object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        protected ICommand GetCloseCommand() => new DelegateCommand<CancelEventArgs>(
            e => {
                if (!Facade.Value.Modified) Run(() => Facade.Close(false), true);
                else SendClose(e);
            },
            e => Facade.Value.Source != null && (e != null || !Facade.Value.Busy)
        ).Hook(Facade.Value, nameof(MainBindableValue.Busy), nameof(MainBindableValue.Source));

        /* ----------------------------------------------------------------- */
        ///
        /// GetCommand
        ///
        /// <summary>
        /// Gets a new command with the specified action.
        /// </summary>
        ///
        /// <param name="action">User action.</param>
        ///
        /// <returns>ICommand object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        protected ICommand GetCommand(Action action) => new DelegateCommand(
            action,
            () => !Facade.Value.Busy
        ).Hook(Facade.Value, nameof(MainBindableValue.Busy));

        /* ----------------------------------------------------------------- */
        ///
        /// IsOpen
        ///
        /// <summary>
        /// Gets a new command that the specified action is invoked when
        /// the document is open.
        /// </summary>
        ///
        /// <param name="action">User action.</param>
        ///
        /// <returns>ICommand object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        protected ICommand IsOpen(Action action) => new DelegateCommand(
            action,
            () => !Facade.Value.Busy && Facade.Value.Source != null
        ).Hook(Facade.Value, nameof(MainBindableValue.Busy), nameof(MainBindableValue.Source));

        /* ----------------------------------------------------------------- */
        ///
        /// IsSelected
        ///
        /// <summary>
        /// Gets a new command that the specified action is invoked when
        /// any items are selected.
        /// </summary>
        ///
        /// <param name="action">User action.</param>
        ///
        /// <returns>ICommand object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        protected ICommand IsSelected(Action action) => new DelegateCommand(
            action,
            () => !Facade.Value.Busy &&
                   Facade.Value.Source != null &&
                   Facade.Value.Images.Selection.Count > 0
        ).Hook(Facade.Value, nameof(MainBindableValue.Busy), nameof(MainBindableValue.Source))
         .Hook(Facade.Value.Images.Selection);

        /* ----------------------------------------------------------------- */
        ///
        /// IsUndoable
        ///
        /// <summary>
        /// Gets a new command that the specified action is invoked when
        /// any actions are undoable.
        /// </summary>
        ///
        /// <param name="action">User action.</param>
        ///
        /// <returns>ICommand object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        protected ICommand IsUndoable(Action action) => new DelegateCommand(
            action,
            () => !Facade.Value.Busy && Facade.Value.Modified
        ).Hook(Facade.Value, nameof(MainBindableValue.Busy), nameof(MainBindableValue.Modified));

        /* ----------------------------------------------------------------- */
        ///
        /// IsRedoable
        ///
        /// <summary>
        /// Gets a new command that the specified action is invoked when
        /// any actions are redoable.
        /// </summary>
        ///
        /// <param name="action">User action.</param>
        ///
        /// <returns>ICommand object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        protected ICommand IsRedoable(Action action) => new DelegateCommand(
            action,
            () => !Facade.Value.Busy && Facade.Value.History.Redoable
        ).Hook(Facade.Value, nameof(MainBindableValue.Busy))
         .Hook(Facade.Value.History);

        #endregion

        #region OnMessage

        /* ----------------------------------------------------------------- */
        ///
        /// OnMessage
        ///
        /// <summary>
        /// Converts the specified exception to a new instance of the
        /// DialogMessage class.
        /// </summary>
        ///
        /// <param name="src">Source exception.</param>
        ///
        /// <returns>DialogMessage object.</returns>
        ///
        /// <remarks>
        /// The Method is called from the Track methods.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        protected override DialogMessage OnMessage(Exception src) =>
            src is OperationCanceledException ? null : Message.From(src);

        #endregion

        #region Send

        /* ----------------------------------------------------------------- */
        ///
        /// SendClose
        ///
        /// <summary>
        /// Sends the message to close the PDF document.
        /// </summary>
        ///
        /// <param name="src">View event argument.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected void SendClose(CancelEventArgs src)
        {
            var e = src ?? new CancelEventArgs();
            var m = Message.ForOverwrite();

            Send(m);
            e.Cancel = m.Value == DialogStatus.Cancel;
            if (e.Cancel) return;

            void close() => Facade.Close(m.Value == DialogStatus.Yes);
            if (src != null) Run(close, true);
            else Run(close, false);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SendOpen
        ///
        /// <summary>
        /// Sends the message to show a dialog of the OpenFileDialog
        /// class, and executes the specified action as an asynchronous
        /// method.
        /// </summary>
        ///
        /// <param name="action">User action.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected void SendOpen(Action<string> action) =>
            Send(Message.ForOpen(), e => action(e.First()), false);

        /* ----------------------------------------------------------------- */
        ///
        /// SendSave
        ///
        /// <summary>
        /// Sends the message to show a dialog of the SaveFileDialog
        /// class, and executes the specified action as an asynchronous
        /// method.
        /// </summary>
        ///
        /// <param name="action">User action.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected void SendSave(Action<string> action) =>
            Send(Message.ForSave(), action, false);

        /* ----------------------------------------------------------------- */
        ///
        /// SendInsert
        ///
        /// <summary>
        /// Sends the message to show a dialog of the OpenFileDialog
        /// class, and executes the specified action as an asynchronous
        /// method.
        /// </summary>
        ///
        /// <param name="index">Insertion index.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected void SendInsert(int index) =>
            Send(Message.ForInsert(), e => Facade.Insert(index, e.Sort()), false);

        /* ----------------------------------------------------------------- */
        ///
        /// SendInsert
        ///
        /// <summary>
        /// Sends the message to show a dialog of the InsertWindow
        /// class, and executes the insert operation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected void SendInsert() => Send(new InsertViewModel(
            (i, v) => Run(() => Facade.Insert(i + 1, v.Select(e => e.FullName)), false),
            Facade.Value.Images.Selection.First,
            Facade.Value.Count,
            Context
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// SendRemove
        ///
        /// <summary>
        /// Sends the message to show a dialog of the RemoveWindow
        /// class, and executes the remove operation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected void SendRemove() => Send(new RemoveViewModel(
            Facade.Remove,
            Facade.Value.Count,
            Context
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// SendExtract
        ///
        /// <summary>
        /// Sends the message to show a dialog of the ExtractWindow
        /// class, and executes the remove operation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected void SendExtract() => Send(new ExtractViewModel(
            Facade.Extract,
            Facade.Value.Images.Selection,
            Facade.Value.Count,
            Context
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// SendMetadata
        ///
        /// <summary>
        /// Sends the message to show a dialog of the MetadataWindow
        /// class, and executes the operation to set PDF metadata.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected void SendMetadata() => Run(() =>Send(new MetadataViewModel(
            Facade.Update,
            Facade.Value.Metadata.Copy(),
            Facade.Value.Source,
            Context
        )), true);

        /* ----------------------------------------------------------------- */
        ///
        /// SendEncryption
        ///
        /// <summary>
        /// Sends the message to show a dialog of the EncryptionWindow
        /// class, and executes the operation to set encryption.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected void SendEncryption() => Run(() => Send(new EncryptionViewModel(
            Facade.Update,
            Facade.Value.Encryption.Copy(),
            Context
        )), true);

        /* ----------------------------------------------------------------- */
        ///
        /// SendPreview
        ///
        /// <summary>
        /// Sends the message to show a dialog of the PreviewWindow
        /// class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected void SendPreview() => Send(new PreviewViewModel(
            Facade.Value.Images,
            Facade.Value.Source,
            Context
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// SendSetting
        ///
        /// <summary>
        /// Sends the message to show a dialog of the SettingWindow
        /// class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected void SendSetting() => Send(new SettingViewModel(Facade.Folder, Context));

        #endregion
    }
}
