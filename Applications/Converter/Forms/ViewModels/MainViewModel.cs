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
using Cube.Generics;
using Cube.Tasks;
using System;
using System.ComponentModel;

namespace Cube.Pdf.App.Converter
{
    /* --------------------------------------------------------------------- */
    ///
    /// MainViewModel
    ///
    /// <summary>
    /// Settings とメイン画面を関連付ける ViewModel を表すクラスです。
    /// </summary>
    ///
    /// <remarks>
    /// Convert 以外では、Messenger 経由でイベントを発生させる際に
    /// Sync を利用していません。これらのコマンドが非同期で実行される
    /// 可能性がある場合、Sync を利用する形に修正して下さい。
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public class MainViewModel : Cube.Forms.ViewModelBase<Messenger>
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
        /// <param name="settings">設定情報</param>
        ///
        /* ----------------------------------------------------------------- */
        public MainViewModel(SettingsFolder settings) : base(new Messenger())
        {
            Model      = new MainFacade(settings);
            Settings   = new SettingsViewModel(settings.Value, Messenger);
            Metadata   = new MetadataViewModel(settings.Value.Metadata, Messenger);
            Encryption = new EncryptionViewModel(settings.Value.Encryption, Messenger);

            settings.PropertyChanged += WhenPropertyChanged;
        }

        #endregion

        #region Properties

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
        /// IO
        ///
        /// <summary>
        /// I/O オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IO IO => Model.IO;

        /* ----------------------------------------------------------------- */
        ///
        /// Title
        ///
        /// <summary>
        /// タイトルを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Title =>
            Model.Settings.DocumentName.Value.HasValue() ?
            $"{Model.Settings.DocumentName.Value} - {Product} {Version}" :
            $"{Product} {Version}";

        /* ----------------------------------------------------------------- */
        ///
        /// Product
        ///
        /// <summary>
        /// 製品名を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Product => Model.Settings.Product;

        /* ----------------------------------------------------------------- */
        ///
        /// Version
        ///
        /// <summary>
        /// バージョンを表す文字列を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Version => Model.Settings.Version.ToString(true);

        /* ----------------------------------------------------------------- */
        ///
        /// Uri
        ///
        /// <summary>
        /// Web ページの URL を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Uri Uri => Model.Settings.Uri;

        /* ----------------------------------------------------------------- */
        ///
        /// IsBusy
        ///
        /// <summary>
        /// 処理中かどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool IsBusy
        {
            get => Model.Value.IsBusy;
            set => Model.Value.IsBusy = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Settings
        ///
        /// <summary>
        /// 一般およびその他タブを表す ViewModel を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public SettingsViewModel Settings { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Metadata
        ///
        /// <summary>
        /// 文書プロパティ・タブを表す ViewModel を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public MetadataViewModel Metadata { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Encryption
        ///
        /// <summary>
        /// セキュリティ・タブを表す ViewModel を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public EncryptionViewModel Encryption { get; }

        #endregion

        #region Commands

        /* ----------------------------------------------------------------- */
        ///
        /// Convert
        ///
        /// <summary>
        /// 変換処理を実行するコマンドです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Convert() =>
            Async(() => this.Invoke(() => Model.Convert())).Forget();

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// 設定を保存するコマンドです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Save() => Model.Save();

        /* ----------------------------------------------------------------- */
        ///
        /// BrowseSource
        ///
        /// <summary>
        /// 入力ファイルの選択画面を表示するコマンドです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void BrowseSource()
        {
            var e = MessageFactory.CreateSource(Settings.Source, IO);
            Messenger.OpenFileDialog.Publish(e);
            Model.UpdateSource(e);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// BrowseDestination
        ///
        /// <summary>
        /// 保存パスの選択画面を表示するコマンドです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void BrowseDestination()
        {
            var e = MessageFactory.CreateDestination(Settings.Destination, IO);
            Messenger.SaveFileDialog.Publish(e);
            Model.UpdateDestination(e);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// BrowseUserProgram
        ///
        /// <summary>
        /// ユーザプログラムの選択画面を表示するコマンドです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void BrowseUserProgram()
        {
            var e = MessageFactory.CreateUserProgram(Settings.UserProgram, IO);
            Messenger.OpenFileDialog.Publish(e);
            Model.UpdateUserProgram(e);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetCulture
        ///
        /// <summary>
        /// 表示言語を設定するコマンドです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void SetCulture() =>
            Messenger.SetCulture.Publish(Settings.Language.GetName());


        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// リソースを解放します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void Dispose(bool disposing)
        {
            if (disposing) Model.Dispose();
            base.Dispose(disposing);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// WhenPropertyChanged
        ///
        /// <summary>
        /// プロパティの変更時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Settings.Format):
                    Model.UpdateExtension();
                    break;
                case nameof(Settings.PostProcess):
                    if (Settings.PostProcess == PostProcess.Others) BrowseUserProgram();
                    break;
                case nameof(Settings.Language):
                    SetCulture();
                    break;
                default:
                    OnPropertyChanged(e);
                    break;
            }
        }

        #endregion
    }
}
