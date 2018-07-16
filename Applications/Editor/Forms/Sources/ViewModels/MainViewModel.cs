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
using Cube.Tasks;
using Cube.Xui;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Cube.Pdf.App.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// MainViewModel
    ///
    /// <summary>
    /// メイン画面の ViewModel クラスです。
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
                () => Send(MessageFactory.CreateSource(e => Task.Run(() =>
                {
                    if (e.Result) Model.Open(e.FileName);
                }).Forget())),
                () => !Data.IsOpen.Value && !Data.IsBusy.Value,
                Data.IsOpen,
                Data.IsBusy
            );

            Ribbon.Close.Command         = WhenOpen(() => Model.Close());
            Ribbon.Save.Command          = WhenOpen(() => { });
            Ribbon.SaveAs.Command        = WhenOpen(() => { });
            Ribbon.Undo.Command          = None;
            Ribbon.Redo.Command          = None;
            Ribbon.Select.Command        = WhenOpen(() => { });
            Ribbon.SelectAll.Command     = WhenOpen(() => { });
            Ribbon.SelectFlip.Command    = WhenOpen(() => { });
            Ribbon.SelectCancel.Command  = WhenOpen(() => { });
            Ribbon.Insert.Command        = WhenSelected(() => { });
            Ribbon.InsertFront.Command   = WhenOpen(() => { });
            Ribbon.InsertBack.Command    = WhenOpen(() => { });
            Ribbon.InsertOthers.Command  = WhenOpen(() => { });
            Ribbon.Extract.Command       = WhenSelected(() => { });
            Ribbon.ExtractImages.Command = WhenOpen(() => { });
            Ribbon.ExtractOthers.Command = WhenOpen(() => { });
            Ribbon.Remove.Command        = WhenSelected(() => { });
            Ribbon.RemoveOthers.Command  = WhenOpen(() => { });
            Ribbon.MovePrevious.Command  = WhenSelected(() => { });
            Ribbon.MoveNext.Command      = WhenSelected(() => { });
            Ribbon.RotateLeft.Command    = WhenSelected(() => { });
            Ribbon.RotateRight.Command   = WhenSelected(() => { });
            Ribbon.Metadata.Command      = None;
            Ribbon.Encryption.Command    = None;
            Ribbon.Refresh.Command       = WhenOpen(() => Model.Refresh());
            Ribbon.ZoomIn.Command        = WhenAny(() => { });
            Ribbon.ZoomOut.Command       = WhenAny(() => { });
            Ribbon.Version.Command       = WhenAny(() => Send(Data.Settings.Uri));
            Ribbon.Exit.Command          = WhenAny(() => Send<CloseMessage>());
            Ribbon.Web.Command           = WhenAny(() => Send(Data.Settings.Uri));
        }

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
            () => Data.IsOpen.Value && Data.Selection.AnySelected && !Data.IsBusy.Value,
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
