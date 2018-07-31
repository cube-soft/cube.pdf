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
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Reflection;
using System.Threading;
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
            var settings = new SettingsFolder(Assembly.GetExecutingAssembly(), new IO());
            Model  = new MainFacade(settings, SynchronizationContext.Current);
            Ribbon = new RibbonViewModel(Messenger);
            SetRibbonEnabled();
            SetRibbonCommands();
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
        public MainBindableData Data => Model.Bindable;

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
        /// Model
        ///
        /// <summary>
        /// Model オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected MainFacade Model { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// None
        ///
        /// <summary>
        /// 既定動作を表すコマンドを取得します。
        /// </summary>
        ///
        /// <remarks>
        /// 未実装メニューに対して使用します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private ICommand None { get; } = new RelayCommand(() => { }, () => false);

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
        /// SetRibbonEnabled
        ///
        /// <summary>
        /// Sets a function object that determines the Ribbon button is
        /// enabled.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SetRibbonEnabled()
        {
            var open = IsOpenFunc();

            Ribbon.Insert.Enabled  = open;
            Ribbon.Extract.Enabled = open;
            Ribbon.Remove.Enabled  = open;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetRibbonCommands
        ///
        /// <summary>
        /// Sets commands of Ribbon items.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SetRibbonCommands()
        {
            Ribbon.Open.Command = new BindableCommand(
                () => SendOpen(e => Model.Open(e)),
                () => !Data.IsOpen.Value && !Data.IsBusy.Value,
                Data.IsOpen,
                Data.IsBusy
            );

            Ribbon.Close.Command         = WhenOpen(() => Model.Close());
            Ribbon.Save.Command          = WhenOpen(() => SendSave(e => Model.Save(e)));
            Ribbon.SaveAs.Command        = None;
            Ribbon.Undo.Command          = None;
            Ribbon.Redo.Command          = None;
            Ribbon.Select.Command        = WhenOpen(() => Model.Select());
            Ribbon.SelectAll.Command     = WhenOpen(() => Model.Select(true));
            Ribbon.SelectFlip.Command    = WhenOpen(() => Model.Flip());
            Ribbon.SelectClear.Command   = WhenOpen(() => Model.Select(false));
            Ribbon.Insert.Command        = WhenSelected(() => SendOpen(e => Model.Insert(e)));
            Ribbon.InsertFront.Command   = WhenOpen(() => SendOpen(e => Model.Insert(0, e)));
            Ribbon.InsertBack.Command    = WhenOpen(() => SendOpen(e => Model.Insert(int.MaxValue, e)));
            Ribbon.InsertOthers.Command  = None;
            Ribbon.Extract.Command       = None;
            Ribbon.ExtractOthers.Command = None;
            Ribbon.Remove.Command        = WhenSelected(() => Model.Remove());
            Ribbon.RemoveOthers.Command  = None;
            Ribbon.MovePrevious.Command  = WhenSelected(() => Model.Move(-1));
            Ribbon.MoveNext.Command      = WhenSelected(() => Model.Move(1));
            Ribbon.RotateLeft.Command    = WhenSelected(() => Model.Rotate(-90));
            Ribbon.RotateRight.Command   = WhenSelected(() => Model.Rotate(90));
            Ribbon.Metadata.Command      = None;
            Ribbon.Encryption.Command    = None;
            Ribbon.Refresh.Command       = WhenOpen(() => Model.Refresh());
            Ribbon.ZoomIn.Command        = WhenAny(() => Model.Zoom(1));
            Ribbon.ZoomOut.Command       = WhenAny(() => Model.Zoom(-1));
            Ribbon.Settings.Command      = WhenAny(() => Send(Data.Settings.Uri));
            Ribbon.Exit.Command          = WhenAny(() => Send<CloseMessage>());
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SendOpen
        ///
        /// <summary>
        /// Sends the message that shows OpenFileDialog and executes the
        /// specified action.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SendOpen(Action<string> action) =>
            Send(MessageFactory.CreateSource(e =>
                Async(() => { if (e.Result) action(e.FileName); })
            ));

        /* ----------------------------------------------------------------- */
        ///
        /// SendSave
        ///
        /// <summary>
        /// Sends the message that shows SaveFileDialog and executes the
        /// specified action.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SendSave(Action<string> action) =>
            Send(MessageFactory.CreateDestination(e =>
                Async(() => { if (e.Result) action(e.FileName); })
            ));

        #region Factory

        /* ----------------------------------------------------------------- */
        ///
        /// WhenAny
        ///
        /// <summary>
        /// Creates a command that can be executed at any time.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ICommand WhenAny(Action action) => new BindableCommand(
            action,
            () => !Data.IsBusy.Value,
            Data.IsBusy
        );

        /* ----------------------------------------------------------------- */
        ///
        /// WhenOpen
        ///
        /// <summary>
        /// Creates a command that can be executed when a document is open.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ICommand WhenOpen(Action action) => new BindableCommand(
            action,
            () => Data.IsOpen.Value && !Data.IsBusy.Value,
            Data.IsOpen,
            Data.IsBusy
        );

        /* ----------------------------------------------------------------- */
        ///
        /// WhenSelected
        ///
        /// <summary>
        /// Creates a command that can be executed when any items are
        /// selected.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ICommand WhenSelected(Action action) => new BindableCommand(
            action,
            () => Data.IsOpen.Value && Data.Selection.Count > 0 && !Data.IsBusy.Value,
            Data.IsOpen,
            Data.IsBusy,
            Data.Selection
        );

        /* ----------------------------------------------------------------- */
        ///
        /// IsOpenFunc
        ///
        /// <summary>
        /// Creates a BindableFunc(T) for the IsEnabled property of the
        /// RibbonEntry class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private BindableFunc<bool> IsOpenFunc()
        {
            var dest = new BindableFunc<bool>(() => Data.IsOpen.Value && !Data.IsBusy.Value);
            Data.IsOpen.PropertyChanged += (s, e) => dest.RaiseValueChanged();
            Data.IsBusy.PropertyChanged += (s, e) => dest.RaiseValueChanged();
            return dest;
        }

        #endregion

        #endregion
    }
}
