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
using Cube.Pdf.Mixin;
using Cube.Xui;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace Cube.Pdf.App.Editor
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
    public class MainViewModel : MessengerViewModel
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
            new SettingsFolder(Assembly.GetExecutingAssembly(), new IO()) { AutoSave = true }
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
        public MainViewModel(SettingsFolder src) : base(new Messenger())
        {
            var recent   = Environment.GetFolderPath(Environment.SpecialFolder.Recent);
            var mon      = new DirectoryMonitor(recent, "*.pdf.lnk", src.IO);
            var password = new Query<string>(e => Send(new PasswordViewModel(e, src.IO, Context)));

            Model  = new MainFacade(src, password, Context);
            Ribbon = new RibbonViewModel(Model.Bindable, MessengerInstance);
            Recent = new RecentViewModel(mon, MessengerInstance);

            Data.Source.PropertyChanged += (s, e) => Ribbon.Raise();
            Data.Busy.PropertyChanged   += (s, e) => Ribbon.Raise();

            SetCommands();
            Post(() => Model.Setup(App.Arguments));
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
        /// Drop
        ///
        /// <summary>
        /// Gets the Drag&amp;Drop command to open a new PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ICommand Drop { get; private set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Move
        ///
        /// <summary>
        /// Gets the Drag&amp;Drop command to move items.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ICommand Move { get; private set; }

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
            Drop                         = IsDrop();
            Move                         = IsDragMove();
            Recent.Open                  = IsLink();
            Ribbon.Open.Command          = Any(() => PostOpen(e => Model.Open(e)));
            Ribbon.Close.Command         = Close();
            Ribbon.Save.Command          = IsOpen(() => Post(() => Model.Overwrite()));
            Ribbon.SaveAs.Command        = IsOpen(() => PostSave(e => Model.Save(e)));
            Ribbon.Preview.Command       = IsItem(() => PostPreview());
            Ribbon.Select.Command        = IsOpen(() => Send(() => Model.Select()));
            Ribbon.SelectAll.Command     = IsOpen(() => Send(() => Model.Select(true)));
            Ribbon.SelectFlip.Command    = IsOpen(() => Send(() => Model.Flip()));
            Ribbon.SelectClear.Command   = IsOpen(() => Send(() => Model.Select(false)));
            Ribbon.Insert.Command        = IsItem(() => PostOpen(e => Model.Insert(e)));
            Ribbon.InsertFront.Command   = IsOpen(() => PostOpen(e => Model.Insert(0, e)));
            Ribbon.InsertBack.Command    = IsOpen(() => PostOpen(e => Model.Insert(int.MaxValue, e)));
            Ribbon.InsertOthers.Command  = IsOpen(() => PostInsert());
            Ribbon.Extract.Command       = IsItem(() => PostSave(e => Model.Extract(e)));
            Ribbon.Remove.Command        = IsItem(() => Send(() => Model.Remove()));
            Ribbon.RemoveOthers.Command  = IsOpen(() => PostRemove());
            Ribbon.MovePrevious.Command  = IsItem(() => Send(() => Model.Move(-1)));
            Ribbon.MoveNext.Command      = IsItem(() => Send(() => Model.Move(1)));
            Ribbon.RotateLeft.Command    = IsItem(() => Send(() => Model.Rotate(-90)));
            Ribbon.RotateRight.Command   = IsItem(() => Send(() => Model.Rotate(90)));
            Ribbon.Metadata.Command      = IsOpen(() => PostMetadata());
            Ribbon.Encryption.Command    = IsOpen(() => PostEncryption());
            Ribbon.Refresh.Command       = IsOpen(() => Send(() => Model.Refresh()));
            Ribbon.Undo.Command          = IsUndo();
            Ribbon.Redo.Command          = IsRedo();
            Ribbon.ZoomIn.Command        = Any(() => Send(() => Model.Zoom(1)));
            Ribbon.ZoomOut.Command       = Any(() => Send(() => Model.Zoom(-1)));
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
        private ICommand Any(Action action) => new BindableCommand(action,
            () => !Data.Busy.Value,
            Data.Busy
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Close
        ///
        /// <summary>
        /// Creates a close command.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ICommand Close() => new BindableCommand<CancelEventArgs>(
            e => {
                if (!Data.Modified.Value) Send(() => Model.Close(false));
                else Send(Factory.CloseMessage(m => PostClose(e, m.Result)));
            },
            e => Data.IsOpen() && (e != null || !Data.Busy.Value),
            Data.Busy, Data.Source
        );

        /* ----------------------------------------------------------------- */
        ///
        /// IsOpen
        ///
        /// <summary>
        /// Creates a command that can execute when a document is open.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ICommand IsOpen(Action action) => new BindableCommand(action,
            () => !Data.Busy.Value && Data.IsOpen(),
            Data.Busy, Data.Source
        );

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
            () => !Data.Busy.Value && Data.IsOpen() && Data.Selection.Count > 0,
            Data.Busy, Data.Source, Data.Selection
        );

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
        private ICommand IsUndo() => new BindableCommand(() => Model.Undo(),
            () => !Data.Busy.Value && Data.History.Undoable,
            Data.Busy, Data.History
        );

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
        private ICommand IsRedo() => new BindableCommand(() => Model.Redo(),
            () => !Data.Busy.Value && Data.History.Redoable,
            Data.Busy, Data.History
        );

        /* ----------------------------------------------------------------- */
        ///
        /// IsLink
        ///
        /// <summary>
        /// Creates a command that can execute when a link is selected.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ICommand IsLink() => new BindableCommand<object>(
            e => Post(() => Model.OpenLink(e as Information)),
            e => !Data.Busy.Value && e is Information,
            Data.Busy
        );

        /* ----------------------------------------------------------------- */
        ///
        /// IsDragMove
        ///
        /// <summary>
        /// Creates a Drag&amp;Drop command to move items.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ICommand IsDragMove() => new BindableCommand<DragDropObject>(
            e => Post(() => Model.InsertOrMove(e)),
            e => !Data.Busy.Value && Data.IsOpen() &&
                 (!e.IsCurrentProcess || e.DropIndex - e.DragIndex != 0),
            Data.Busy, Data.Source
        );

        /* ----------------------------------------------------------------- */
        ///
        /// IsDrop
        ///
        /// <summary>
        /// Creates a command that can execute when an item is dropped.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ICommand IsDrop() => new BindableCommand<string>(
            e => Post(() => Model.Open(e)),
            e => !Data.Busy.Value,
            Data.Busy
        );

        #endregion

        #region Send or Post

        /* ----------------------------------------------------------------- */
        ///
        /// PostOpen
        ///
        /// <summary>
        /// Posts the message to show a dialog of the OpenFileDialog
        /// class, and executes the specified action as an asynchronous
        /// operation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void PostOpen(Action<string> action) => Send(Factory.OpenMessage(e =>
            Post(() => { if (e.Result) action(e.FileName); })
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// PostSave
        ///
        /// <summary>
        /// Posts the message to show a dialog of the SaveFileDialog
        /// class, and executes the specified action as an asynchronous
        /// operation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void PostSave(Action<string> action) => Send(Factory.SaveMessage(e =>
            Post(() => { if (e.Result) action(e.FileName); })
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// PostClose
        ///
        /// <summary>
        /// Posts the message to close the PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void PostClose(CancelEventArgs src, MessageBoxResult m)
        {
            var e = src ?? new CancelEventArgs();
            e.Cancel = (m == MessageBoxResult.Cancel);
            if (e.Cancel) return;

            void close() => Model.Close(m == MessageBoxResult.Yes);
            if (src != null) Send(close);
            else Post(close);
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
            Data.Count.Value, Context
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
        private void PostMetadata() => Post(() =>
        {
            var m = Model.GetMetadata().Copy();
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
        private void PostEncryption() => Post(() =>
        {
            var m = Model.GetEncryption().Copy();
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
