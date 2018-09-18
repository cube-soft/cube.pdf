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
using System.Reflection;
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
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public MainViewModel() : base(new Messenger())
        {
            var io       = new IO();
            var settings = new SettingsFolder(Assembly.GetExecutingAssembly(), io);
            var recent   = Environment.GetFolderPath(Environment.SpecialFolder.Recent);
            var mon      = new DirectoryMonitor(recent, "*.pdf.lnk", io);

            Model  = new MainFacade(settings, Context);
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
        /// Gets the command for Drag&amp;Drop.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ICommand Drop { get; private set; }

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
        /// Sets commands of the <c>MainWindow</c>.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SetCommands()
        {
            Drop                         = IsDrop();
            Recent.Open                  = IsLink();
            Ribbon.Open.Command          = Any(() => PostOpen(e => Model.Open(e)));
            Ribbon.Close.Command         = IsOpen(() => Send(() => Model.Close()));
            Ribbon.Save.Command          = IsOpen(() => Post(() => Model.Save()));
            Ribbon.SaveAs.Command        = IsOpen(() => PostSave(e => Model.Save(e)));
            Ribbon.Preview.Command       = IsItem(() => SendPreview());
            Ribbon.Select.Command        = IsOpen(() => Send(() => Model.Select()));
            Ribbon.SelectAll.Command     = IsOpen(() => Send(() => Model.Select(true)));
            Ribbon.SelectFlip.Command    = IsOpen(() => Send(() => Model.Flip()));
            Ribbon.SelectClear.Command   = IsOpen(() => Send(() => Model.Select(false)));
            Ribbon.Insert.Command        = IsItem(() => PostOpen(e => Model.Insert(e)));
            Ribbon.InsertFront.Command   = IsOpen(() => PostOpen(e => Model.Insert(0, e)));
            Ribbon.InsertBack.Command    = IsOpen(() => PostOpen(e => Model.Insert(int.MaxValue, e)));
            Ribbon.InsertOthers.Command  = IsOpen(() => SendInsert());
            Ribbon.Extract.Command       = IsItem(() => PostSave(e => Model.Extract(e)));
            Ribbon.Remove.Command        = IsItem(() => Send(() => Model.Remove()));
            Ribbon.RemoveOthers.Command  = IsOpen(() => SendRemove());
            Ribbon.MovePrevious.Command  = IsItem(() => Send(() => Model.Move(-1)));
            Ribbon.MoveNext.Command      = IsItem(() => Send(() => Model.Move(1)));
            Ribbon.RotateLeft.Command    = IsItem(() => Send(() => Model.Rotate(-90)));
            Ribbon.RotateRight.Command   = IsItem(() => Send(() => Model.Rotate(90)));
            Ribbon.Metadata.Command      = IsOpen(() => SendMetadata());
            Ribbon.Encryption.Command    = IsOpen(() => SendEncryption());
            Ribbon.Refresh.Command       = IsOpen(() => Send(() => Model.Refresh()));
            Ribbon.Undo.Command          = IsUndo();
            Ribbon.Redo.Command          = IsRedo();
            Ribbon.ZoomIn.Command        = Any(() => Send(() => Model.Zoom(1)));
            Ribbon.ZoomOut.Command       = Any(() => Send(() => Model.Zoom(-1)));
            Ribbon.Settings.Command      = Any(() => SendSettings());
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
            Data.Busy);

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
            Data.Busy, Data.Source);

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
            Data.Busy, Data.Source, Data.Selection);

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
            Data.Busy, Data.History);

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
            Data.Busy, Data.History);

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
            e => !Data.Busy.Value,
            Data.Busy);

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
            Data.Busy);

        #endregion

        #region Send or Post

        /* ----------------------------------------------------------------- */
        ///
        /// SendOpen
        ///
        /// <summary>
        /// Sends the message to show a dialog of the <c>OpenFileDialog</c>
        /// class, and executes the specified action as an asynchronous
        /// operation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void PostOpen(Action<string> action) => Send(MessageFactory.CreateSource(e =>
            Post(() => { if (e.Result) action(e.FileName); })
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// PostSave
        ///
        /// <summary>
        /// Sends the message to show a dialog of the <c>SaveFileDialog</c>
        /// class, and executes the specified action as an asynchronous
        /// operation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void PostSave(Action<string> action) => Send(MessageFactory.CreateDestination(e =>
            Post(() => { if (e.Result) action(e.FileName); })
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// SendPreview
        ///
        /// <summary>
        /// Sends the message to show a dialog of the <c>PreviewWindow</c>
        /// class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SendPreview() => Send(new PreviewViewModel(Data.Images, Data.Source.Value, Context));

        /* ----------------------------------------------------------------- */
        ///
        /// SendInsert
        ///
        /// <summary>
        /// Sends the message to show a dialog of the <c>InsertWindow</c>
        /// class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SendInsert() => Send(new InsertViewModel(Data.Count.Value, Context));

        /* ----------------------------------------------------------------- */
        ///
        /// SendRemove
        ///
        /// <summary>
        /// Sends the message to show a dialog of the <c>RemoveWindow</c>
        /// class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SendRemove() => Send(new RemoveViewModel(e => Model.Remove(e), Data.Count.Value, Context));

        /* ----------------------------------------------------------------- */
        ///
        /// SendMetadata
        ///
        /// <summary>
        /// Sends the message to show a dialog of the <c>MetadataWindow</c>
        /// class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SendMetadata() => Send(new MetadataViewModel(
            e => Model.Update(e),
            Data.Metadata.Value.Copy(),
            Data.Source.Value,
            Context
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// SendEncryption
        ///
        /// <summary>
        /// Sends the message to show a dialog of the <c>EncryptionWindow</c>
        /// class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SendEncryption() => Send(new EncryptionViewModel(
            e => Model.Update(e),
            Data.Encryption.Value.Copy(),
            Context
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// SendSettings
        ///
        /// <summary>
        /// Sends the message to show a dialog of the <c>SettingsWindow</c>
        /// class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SendSettings() => Send(new SettingsViewModel(
            Model.Settings,
            Context
        ));

        #endregion

        #endregion
    }
}
