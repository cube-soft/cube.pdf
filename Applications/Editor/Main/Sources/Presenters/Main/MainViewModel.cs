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
    public class MainViewModel : ViewModelBase<MainFacade>
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
            new SettingFolder(Assembly.GetExecutingAssembly(), new IO()) { AutoSave = true },
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
        public MainViewModel(SettingFolder src, SynchronizationContext context) : base(
            new MainFacade(src, context),
            new Aggregator(),
            context
        ) {
            var recent = Environment.SpecialFolder.Recent.GetName();
            var mon    = new DirectoryMonitor(recent, "*.pdf.lnk", src.IO, GetDispatcher(false));

            Ribbon = new RibbonViewModel(Facade.Value, Aggregator, context);
            Recent = new RecentViewModel(mon, Aggregator, context);
            Facade.Query = new Query<string>(e => Send(new PasswordViewModel(e, context)));

            SetCommands();
            Track(() => Facade.Setup(App.Arguments));
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Value
        ///
        /// <summary>
        /// Gets data for binding to the MainWindow.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public MainBindable Value => Facade.Value;

        /* ----------------------------------------------------------------- */
        ///
        /// Ribbon
        ///
        /// <summary>
        /// Gets the ViewModel for the Ribbon components.
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
            e => Track(() => Facade.Open(e)),
            e => !Value.Busy && Facade.GetFirst(e).HasValue()
        ).Associate(Value, nameof(Value.Busy)));

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
            e => Track(() => Facade.InsertOrMove(e)),
            e => !Value.Busy && Value.Source != null &&
                 (!e.IsCurrentProcess || e.DropIndex - e.DragIndex != 0)
        ).Associate(Value, nameof(Value.Busy), nameof(Value.Source)));

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
                Facade.Dispose();
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
            Ribbon.Open.Command          = Any(() => PostOpen(e => Facade.Open(e)));
            Ribbon.Close.Command         = Close();
            Ribbon.Save.Command          = IsOpen(() => Track(() => Facade.Overwrite()));
            Ribbon.SaveAs.Command        = IsOpen(() => PostSave(e => Facade.Save(e)));
            Ribbon.Preview.Command       = IsItem(() => PostPreview());
            Ribbon.Select.Command        = IsOpen(() => Sync(() => Facade.Select()));
            Ribbon.SelectAll.Command     = IsOpen(() => Sync(() => Facade.Select(true)));
            Ribbon.SelectFlip.Command    = IsOpen(() => Sync(() => Facade.Flip()));
            Ribbon.SelectClear.Command   = IsOpen(() => Sync(() => Facade.Select(false)));
            Ribbon.Insert.Command        = IsItem(() => PostInsert(e => Facade.Insert(e)));
            Ribbon.InsertFront.Command   = IsOpen(() => PostInsert(e => Facade.Insert(0, e)));
            Ribbon.InsertBack.Command    = IsOpen(() => PostInsert(e => Facade.Insert(int.MaxValue, e)));
            Ribbon.InsertOthers.Command  = IsOpen(() => PostInsert());
            Ribbon.Extract.Command       = IsItem(() => PostSave(e => Facade.Extract(e)));
            Ribbon.Remove.Command        = IsItem(() => Sync(() => Facade.Remove()));
            Ribbon.RemoveOthers.Command  = IsOpen(() => PostRemove());
            Ribbon.MovePrevious.Command  = IsItem(() => Sync(() => Facade.Move(-1)));
            Ribbon.MoveNext.Command      = IsItem(() => Sync(() => Facade.Move(1)));
            Ribbon.RotateLeft.Command    = IsItem(() => Sync(() => Facade.Rotate(-90)));
            Ribbon.RotateRight.Command   = IsItem(() => Sync(() => Facade.Rotate(90)));
            Ribbon.Metadata.Command      = IsOpen(() => PostMetadata());
            Ribbon.Encryption.Command    = IsOpen(() => PostEncryption());
            Ribbon.Refresh.Command       = IsOpen(() => Sync(() => Facade.Refresh()));
            Ribbon.Undo.Command          = IsUndo();
            Ribbon.Redo.Command          = IsRedo();
            Ribbon.ZoomIn.Command        = Any(() => Sync(() => Facade.Zoom(1)));
            Ribbon.ZoomOut.Command       = Any(() => Sync(() => Facade.Zoom(-1)));
            Ribbon.Setting.Command       = Any(() => PostSetting());
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
            () => !Value.Busy
        ).Associate(Value, nameof(Value.Busy));

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
                if (!Value.Modified) Sync(() => Facade.Close(false));
                else
                {
                    var msg = MessageFactory.CreateOverwriteWarn();
                    Send(msg);
                    PostClose(e, msg.Value);
                }
            },
            e => Value.Source != null && (e != null || !Value.Busy)
        )
        .Associate(Value, nameof(Value.Busy), nameof(Value.Source));

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
            () => !Value.Busy && Value.Source != null
        )
        .Associate(Value, nameof(Value.Busy), nameof(Value.Source));

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
            () => !Value.Busy && Value.Source != null && Value.Images.Selection.Count > 0
        )
        .Associate(Value, nameof(Value.Busy), nameof(Value.Source))
        .Associate(Value.Images.Selection);

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
            () => Sync(() => Facade.Undo()),
            () => !Value.Busy && Value.Modified
        )
        .Associate(Value, nameof(Value.Busy), nameof(Value.Modified));

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
            () => Sync(() => Facade.Redo()),
            () => !Value.Busy && Value.History.Redoable
        )
        .Associate(Value, nameof(Value.Busy))
        .Associate(Value.History);

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
            e => Track(() => Facade.OpenLink(e as Entity)),
            e => !Value.Busy && e is Entity
        ).Associate(Value, nameof(Value.Busy));

        #endregion

        #region Send or Post

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
        private void PostOpen(Action<string> action) =>
            Send(MessageFactory.CreateForOpen(), e => action(e.First()));

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
        private void PostSave(Action<string> action) =>
            Send(MessageFactory.CreateForSave(), e => action(e));

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
        private void PostInsert(Action<IEnumerable<string>> action) =>
            Send(MessageFactory.CreateForInsert(), e => action(e));

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

            void close() => Facade.Close(m == DialogStatus.Yes);
            if (src != null) Sync(close);
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
            Value.Images, Value.Source, Context
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
            (i, v) => Track(() => Facade.Insert(i + 1, v.Select(e => e.FullName))),
            Value.Images.Selection.First, Value.Count, Value.IO, Context
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
            e => Facade.Remove(e), Value.Count, Context
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
            var m = Value.Metadata.Copy();
            Post(new MetadataViewModel(e => Facade.Update(e), m, Value.Source, Context));
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
            var m = Value.Encryption.Copy();
            Post(new EncryptionViewModel(e => Facade.Update(e), m, Context));
        });

        /* ----------------------------------------------------------------- */
        ///
        /// PostSetting
        ///
        /// <summary>
        /// Posts the message to show a dialog of the SettingWindow
        /// class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void PostSetting() => Post(new SettingViewModel(Facade.Settings, Context));

        #endregion

        #endregion
    }
}
