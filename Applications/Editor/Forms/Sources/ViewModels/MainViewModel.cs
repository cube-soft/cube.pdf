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
            SetRibbonCommands();
        }

        #endregion

        #region Properties

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
        /// Images
        ///
        /// <summary>
        /// PDF のサムネイル一覧を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ImageList Images => Model.Images;

        /* ----------------------------------------------------------------- */
        ///
        /// Settings
        ///
        /// <summary>
        /// 設定情報を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Settings Settings => Model.Settings.Value;

        /* ----------------------------------------------------------------- */
        ///
        /// Message
        ///
        /// <summary>
        /// メッセージを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Bindable<string> Message { get; } = new Bindable<string>("Ready");

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
        /// Default
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
        protected ICommand Default { get; } = new RelayCommand(() => { }, () => false);

        #endregion

        #region Commands

        /* ----------------------------------------------------------------- */
        ///
        /// SetRibbonCommands
        ///
        /// <summary>
        /// Ribbon メニューのコマンドを設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SetRibbonCommands()
        {
            Ribbon.Open.Command = new RelayCommand(() =>
            {
                Send(new OpenFileDialogMessage(e =>
                {
                    Task.Run(() => Model.Open(e.FileName)).Forget();
                }));
            });

            Ribbon.Save.Command = Default;
            Ribbon.SaveAs.Command = Default;
            Ribbon.Close.Command = Default;
            Ribbon.Undo.Command = Default;
            Ribbon.Redo.Command = Default;
            Ribbon.Select.Command = Default;
            Ribbon.SelectAll.Command = Default;
            Ribbon.SelectFlip.Command = Default;
            Ribbon.SelectCancel.Command = Default;
            Ribbon.Insert.Command = Default;
            Ribbon.InsertFront.Command = Default;
            Ribbon.InsertBack.Command = Default;
            Ribbon.InsertOthers.Command = Default;
            Ribbon.Extract.Command = Default;
            Ribbon.ExtractImages.Command = Default;
            Ribbon.ExtractOthers.Command = Default;
            Ribbon.Remove.Command = Default;
            Ribbon.RemoveRange.Command = Default;
            Ribbon.MovePrevious.Command = Default;
            Ribbon.MoveNext.Command = Default;
            Ribbon.RotateLeft.Command = Default;
            Ribbon.RotateRight.Command = Default;
            Ribbon.Metadata.Command = Default;
            Ribbon.Encryption.Command = Default;
            Ribbon.Refresh.Command = Default;
            Ribbon.ZoomIn.Command = Default;
            Ribbon.ZoomOut.Command = Default;
            Ribbon.Version.Command = Default;
            Ribbon.Exit.Command = new RelayCommand(() => Send<CloseMessage>());
            Ribbon.Web.Command = new RelayCommand(() => Send(Settings.Uri));
        }

        #endregion
    }
}
