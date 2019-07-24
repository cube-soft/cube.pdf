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
using Cube.Mixin.Generics;
using Cube.Mixin.Observing;
using Cube.Mixin.Pdf;
using Cube.Xui;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Input;

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
    public abstract class MainViewModelBase : GenericViewModel<MainFacade>
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
            e => Track(() => Facade.OpenLink(e as Entity)),
            e => !Facade.Value.Busy && e is Entity
        ).Associate(Facade.Value, nameof(MainBindable.Busy));

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
                if (!Facade.Value.Modified) Sync(() => Facade.Close(false));
                else SendClose(e);
            },
            e => Facade.Value.Source != null && (e != null || !Facade.Value.Busy)
        ).Associate(Facade.Value, nameof(MainBindable.Busy), nameof(MainBindable.Source));

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
        protected ICommand GetCommand(Action action) => new DelegateCommand(action,
            () => !Facade.Value.Busy
        ).Associate(Facade.Value, nameof(MainBindable.Busy));

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
        protected ICommand IsOpen(Action action) => new DelegateCommand(action,
            () => !Facade.Value.Busy && Facade.Value.Source != null
        ).Associate(Facade.Value, nameof(MainBindable.Busy), nameof(MainBindable.Source));

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
        protected ICommand IsSelected(Action action) => new DelegateCommand(action,
            () => !Facade.Value.Busy &&
                   Facade.Value.Source != null &&
                   Facade.Value.Images.Selection.Count > 0
        ).Associate(Facade.Value, nameof(MainBindable.Busy), nameof(MainBindable.Source))
         .Associate(Facade.Value.Images.Selection);

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
        protected ICommand IsUndoable(Action action) => new DelegateCommand(action,
            () => !Facade.Value.Busy && Facade.Value.Modified
        ).Associate(Facade.Value, nameof(MainBindable.Busy), nameof(MainBindable.Modified));

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
        protected ICommand IsRedoable(Action action) => new DelegateCommand(action,
            () => !Facade.Value.Busy && Facade.Value.History.Redoable
        ).Associate(Facade.Value, nameof(MainBindable.Busy))
         .Associate(Facade.Value.History);

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
            var m = MessageFactory.CreateOverwriteWarn();

            Send(m);
            e.Cancel = m.Value == DialogStatus.Cancel;
            if (e.Cancel) return;

            void close() => Facade.Close(m.Value == DialogStatus.Yes);
            if (src != null) Sync(close);
            else Track(close);
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
            Send(MessageFactory.CreateForOpen(), e => action(e.First()));

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
            Send(MessageFactory.CreateForSave(), e => action(e));

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
        /// <param name="action">User action.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected void SendInsert(Action<IEnumerable<string>> action) =>
            Send(MessageFactory.CreateForInsert(), e => action(e));

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
            (i, v) => Track(() => Facade.Insert(i + 1, v.Select(e => e.FullName))),
            Facade.Value.Images.Selection.First,
            Facade.Value.Count,
            Facade.Value.IO,
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
            e => Facade.Remove(e),
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
            e => Facade.Extract(e),
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
        protected void SendMetadata() => Send(new MetadataViewModel(
            e => Facade.Update(e),
            Facade.Value.Metadata.Copy(),
            Facade.Value.Source,
            Context
        ));

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
        protected void SendEncryption() => Send(new EncryptionViewModel(
            e => Facade.Update(e),
            Facade.Value.Encryption.Copy(),
            Context
        ));

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
        protected void SendPreview() =>
            Send(new PreviewViewModel(Facade.Value.Images, Facade.Value.Source, Context));

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
        protected void SendSetting() => Send(new SettingViewModel(Facade.Settings, Context));

        #endregion
    }
}
