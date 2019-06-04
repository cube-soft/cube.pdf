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
using Cube.Mixin.Environment;
using Cube.Mixin.Generics;
using Cube.Mixin.Observing;
using Cube.Mixin.Pdf;
using Cube.Mixin.String;
using Cube.Xui;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Input;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// MainViewModel
    ///
    /// <summary>
    /// Provides binding properties and commands for the MainWindow class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class MainViewModel : ViewModelBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// MainViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public MainViewModel() : this (
            new SettingsFolder(Assembly.GetExecutingAssembly(), new IO()) { AutoSave = true },
            SynchronizationContext.Current
        ) { }

        /* ----------------------------------------------------------------- */
        ///
        /// MainViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the MainViewModel class
        /// with the specified settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public MainViewModel(SettingsFolder src, SynchronizationContext context) :
            base(new Aggregator(), context)
        {
            var recent   = Environment.SpecialFolder.Recent.GetName();
            var mon      = new DirectoryMonitor(recent, "*.pdf.lnk", src.IO, GetDispatcher(false));
            var password = new Query<string>(e => Send(new PasswordViewModel(e, src.IO, context)));

            Model  = new MainFacade(src, password, context);
            Ribbon = new RibbonViewModel(Model.Bindable, Aggregator, context);
            Recent = new RecentViewModel(mon, Aggregator, context);

            SetCommands();
            Track(() => Model.Setup(App.Arguments));
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Data
        ///
        /// <summary>
        /// Gets data for binding to the MainWindow.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public MainBindable Data => Model.Bindable;

        /* ----------------------------------------------------------------- */
        ///
        /// Ribbon
        ///
        /// <summary>
        /// Ribbon の ViewModel を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonViewModel Ribbon { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Recent
        ///
        /// <summary>
        /// Gets the ViewModel of the RecentCollection object.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RecentViewModel Recent { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Model
        ///
        /// <summary>
        /// Model オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected MainFacade Model { get; }

        #endregion

        #region Commands

        /* ----------------------------------------------------------------- */
        ///
        /// Open
        ///
        /// <summary>
        /// Gets the Drag&amp;Drop command to open a new PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ICommand Open => Get(() => new DelegateCommand<string[]>(
            e => Track(() => Model.Open(e)),
            e => !Data.Busy.Value && Model.GetFirst(e).HasValue()
        ).Associate(Data.Busy));

        /* ----------------------------------------------------------------- */
        ///
        /// InsertOrMove
        ///
        /// <summary>
        /// Gets the Drag&amp;Drop command to insert or move items.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ICommand InsertOrMove => Get(() => new DelegateCommand<DragDropObject>(
            e => Track(() => Model.InsertOrMove(e)),
            e => !Data.Busy.Value && Data.IsOpen() &&
                 (!e.IsCurrentProcess || e.DropIndex - e.DragIndex != 0)
        ).Associate(Data.Busy).Associate(Data.Source));

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
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Model.Dispose();
                Ribbon.Dispose();
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetCommands
        ///
        /// <summary>
        /// Sets commands of the MainWindow.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SetCommands()
        {
            Recent.Open                  = IsLink();
            Ribbon.Open.Command          = Any(() => PostOpen(e => Model.Open(e)));
            Ribbon.Close.Command         = Close();
            Ribbon.Save.Command          = IsOpen(() => Track(() => Model.Overwrite()));
            Ribbon.SaveAs.Command        = IsOpen(() => PostSave(e => Model.Save(e)));
            Ribbon.Preview.Command       = IsItem(() => PostPreview());
            Ribbon.Select.Command        = IsOpen(() => TrackSync(() => Model.Select()));
            Ribbon.SelectAll.Command     = IsOpen(() => TrackSync(() => Model.Select(true)));
            Ribbon.SelectFlip.Command    = IsOpen(() => TrackSync(() => Model.Flip()));
            Ribbon.SelectClear.Command   = IsOpen(() => TrackSync(() => Model.Select(false)));
            Ribbon.Insert.Command        = IsItem(() => PostInsert(e => Model.Insert(e)));
            Ribbon.InsertFront.Command   = IsOpen(() => PostInsert(e => Model.Insert(0, e)));
            Ribbon.InsertBack.Command    = IsOpen(() => PostInsert(e => Model.Insert(int.MaxValue, e)));
            Ribbon.InsertOthers.Command  = IsOpen(() => PostInsert());
            Ribbon.Extract.Command       = IsItem(() => PostSave(e => Model.Extract(e)));
            Ribbon.Remove.Command        = IsItem(() => TrackSync(() => Model.Remove()));
            Ribbon.RemoveOthers.Command  = IsOpen(() => PostRemove());
            Ribbon.MovePrevious.Command  = IsItem(() => TrackSync(() => Model.Move(-1)));
            Ribbon.MoveNext.Command      = IsItem(() => TrackSync(() => Model.Move(1)));
            Ribbon.RotateLeft.Command    = IsItem(() => TrackSync(() => Model.Rotate(-90)));
            Ribbon.RotateRight.Command   = IsItem(() => TrackSync(() => Model.Rotate(90)));
            Ribbon.Metadata.Command      = IsOpen(() => PostMetadata());
            Ribbon.Encryption.Command    = IsOpen(() => PostEncryption());
            Ribbon.Refresh.Command       = IsOpen(() => TrackSync(() => Model.Refresh()));
            Ribbon.Undo.Command          = IsUndo();
            Ribbon.Redo.Command          = IsRedo();
            Ribbon.ZoomIn.Command        = Any(() => TrackSync(() => Model.Zoom(1)));
            Ribbon.ZoomOut.Command       = Any(() => TrackSync(() => Model.Zoom(-1)));
            Ribbon.Settings.Command      = Any(() => PostSettings());
            Ribbon.Exit.Command          = Any(() => Send<CloseMessage>());
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
        private ICommand Any(Action action) => new DelegateCommand(action,
            () => !Data.Busy.Value
        ).Associate(Data.Busy);

        /* ----------------------------------------------------------------- */
        ///
        /// Close
        ///
        /// <summary>
        /// Creates a close command.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ICommand Close() => new DelegateCommand<CancelEventArgs>(
            e => {
                if (!Data.Modified.Value) TrackSync(() => Model.Close(false));
                else
                {
                    var msg = MessageFactory.CreateOverwriteWarn();
                    Send(msg);
                    PostClose(e, msg.Status);
                }
            },
            e => Data.IsOpen() && (e != null || !Data.Busy.Value)
        )
        .Associate(Data.Busy)
        .Associate(Data.Source);

        /* ----------------------------------------------------------------- */
        ///
        /// IsOpen
        ///
        /// <summary>
        /// Creates a command that can execute when a document is open.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ICommand IsOpen(Action action) => new DelegateCommand(action,
            () => !Data.Busy.Value && Data.IsOpen()
        )
        .Associate(Data.Busy)
        .Associate(Data.Source);

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
            () => !Data.Busy.Value && Data.IsOpen() && Data.Images.Selection.Count > 0
        )
        .Associate(Data.Busy)
        .Associate(Data.Source)
        .Associate(Data.Images.Selection);

        /* ----------------------------------------------------------------- */
        ///
        /// IsUndo
        ///
        /// <summary>
        /// Creates a command that can execute when undo-history items
        /// exist.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ICommand IsUndo() => new DelegateCommand(
            () => TrackSync(() => Model.Undo()),
            () => !Data.Busy.Value && Data.History.Undoable
        )
        .Associate(Data.Busy)
        .Associate(Data.History);

        /* ----------------------------------------------------------------- */
        ///
        /// IsRedo
        ///
        /// <summary>
        /// Creates a command that can execute when redo-history items
        /// exist.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ICommand IsRedo() => new DelegateCommand(
            () => TrackSync(() => Model.Redo()),
            () => !Data.Busy.Value && Data.History.Redoable
        )
        .Associate(Data.Busy)
        .Associate(Data.History);

        /* ----------------------------------------------------------------- */
        ///
        /// IsLink
        ///
        /// <summary>
        /// Creates a command that can execute when a link is selected.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ICommand IsLink() => new DelegateCommand<object>(
            e => Track(() => Model.OpenLink(e as Information)),
            e => !Data.Busy.Value && e is Information
        ).Associate(Data.Busy);

        #endregion

        #region Send or Post

        /* ----------------------------------------------------------------- */
        ///
        /// TrackSync
        ///
        /// <summary>
        /// Invokes the Track method as a synchronous manner.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void TrackSync(Action action) => Track(action, DialogMessage.Create, true);

        /* ----------------------------------------------------------------- */
        ///
        /// PostOpen
        ///
        /// <summary>
        /// Sends the message to show a dialog of the OpenFileDialog
        /// class, and executes the specified action as an asynchronous
        /// operation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void PostOpen(Action<string> action)
        {
            var msg = MessageFactory.CreateForOpen();
            Send(msg);
            Track(() => { if (!msg.Cancel) action(msg.Value.First()); });
        }

        /* ----------------------------------------------------------------- */
        ///
        /// PostSave
        ///
        /// <summary>
        /// Sends the message to show a dialog of the SaveFileDialog
        /// class, and executes the specified action as an asynchronous
        /// operation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void PostSave(Action<string> action)
        {
            var msg = MessageFactory.CreateForSave();
            Send(msg);
            Track(() => { if (!msg.Cancel) action(msg.Value); });
        }

        /* ----------------------------------------------------------------- */
        ///
        /// PostInsert
        ///
        /// <summary>
        /// Sends the message to show a dialog of the OpenFileDialog
        /// class, and executes the specified action as an asynchronous
        /// operation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void PostInsert(Action<IEnumerable<string>> action)
        {
            var msg = MessageFactory.CreateForInsert();
            Send(msg);
            Track(() => { if (!msg.Cancel) action(msg.Value); });
        }

        /* ----------------------------------------------------------------- */
        ///
        /// PostClose
        ///
        /// <summary>
        /// Posts the message to close the PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void PostClose(CancelEventArgs src, DialogStatus m)
        {
            var e = src ?? new CancelEventArgs();
            e.Cancel = m == DialogStatus.Cancel;
            if (e.Cancel) return;

            void close() => Model.Close(m == DialogStatus.Yes);
            if (src != null) Track(close, DialogMessage.Create, true);
            else Track(close);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// PostPreview
        ///
        /// <summary>
        /// Posts the message to show a dialog of the PreviewWindow
        /// class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void PostPreview() => Post(new PreviewViewModel(
            Data.Images, Data.Source.Value, Context
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// PostInsert
        ///
        /// <summary>
        /// Posts the message to show a dialog of the InsertWindow
        /// class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void PostInsert() => Post(new InsertViewModel(
            (i, v) => Track(() => Model.Insert(i + 1, v.Select(e => e.FullName))),
            Data.Images.Selection.First, Data.Count.Value, Data.IO, Context
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// PostRemove
        ///
        /// <summary>
        /// Sends the message to show a dialog of the RemoveWindow
        /// class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void PostRemove() => Post(new RemoveViewModel(
            e => Model.Remove(e), Data.Count.Value, Context
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// PostMetadata
        ///
        /// <summary>
        /// Posts the message to show a dialog of the MetadataWindow
        /// class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void PostMetadata() => Track(() =>
        {
            var m = Data.Metadata.Copy();
            Post(new MetadataViewModel(e => Model.Update(e), m, Data.Source.Value, Context));
        });

        /* ----------------------------------------------------------------- */
        ///
        /// PostEncryption
        ///
        /// <summary>
        /// Posts the message to show a dialog of the EncryptionWindow
        /// class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void PostEncryption() => Track(() =>
        {
            var m = Data.Encryption.Copy();
            Post(new EncryptionViewModel(e => Model.Update(e), m, Context));
        });

        /* ----------------------------------------------------------------- */
        ///
        /// PostSettings
        ///
        /// <summary>
        /// Posts the message to show a dialog of the SettingsWindow
        /// class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void PostSettings() => Post(new SettingsViewModel(Model.Settings, Context));

        #endregion

        #endregion
    }
}
