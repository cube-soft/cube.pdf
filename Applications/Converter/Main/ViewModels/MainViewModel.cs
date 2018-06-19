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
using Cube.Log;
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
    /* --------------------------------------------------------------------- */
    public class MainViewModel : ObservableProperty
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
        public MainViewModel(SettingsFolder settings)
        {
            Model = new MainFacade(settings);
            settings.PropertyChanged += WhenPropertyChanged;

            Settings   = new SettingsViewModel(settings.Value);
            Metadata   = new MetadataViewModel(settings.Value.Metadata);
            Encryption = new EncryptionViewModel(settings.Value.Encryption);
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
        /// Messenger
        ///
        /// <summary>
        /// Messenger オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Messenger Messenger { get; } = new Messenger();

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
        public Uri Uri => new Uri("https://www.cube-soft.jp/cubepdf/");

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
            set => Model.Value.CheckUpdate = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CheckUpdate
        ///
        /// <summary>
        /// アップデートを確認するかどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool CheckUpdate
        {
            get => Model.Value.CheckUpdate;
            set => Model.Value.CheckUpdate = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Language
        ///
        /// <summary>
        /// 表示言語を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Language Language
        {
            get => Model.Value.Language;
            set => Model.Value.Language = value;
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
        /// Save
        ///
        /// <summary>
        /// 設定を保存するためのコマンドです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Save() => Model.Save();

        /* ----------------------------------------------------------------- */
        ///
        /// Convert
        ///
        /// <summary>
        /// 変換処理を実行するためのコマンドです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Convert()
        {
            if (!this.Validate()) return;

            try
            {
                Model.Convert();
                Messenger.Close.Publish();
            }
            catch (OperationCanceledException) { }
            catch (Exception err)
            {
                this.LogError(err.ToString(), err);
                Messenger.MessageBox.Publish(MessageFactory.CreateError(err.Message));
                Messenger.Close.Publish();
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// BrowseSource
        ///
        /// <summary>
        /// 入力ファイルの選択画面を表示するためのコマンドです。
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
        /// 保存パスの選択画面を表示するためのコマンドです。
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
        /// ユーザプログラムの選択画面を表示するためのコマンドです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void BrowseUserProgram()
        {
            var e = MessageFactory.CreateUserProgram(Settings.UserProgram, IO);
            Messenger.OpenFileDialog.Publish(e);
            Model.UpdateUserProgram(e);
        }

        #endregion

        #region Implementations

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
                case nameof(Settings.PostProcess):
                    if (Settings.PostProcess == PostProcess.Others) BrowseUserProgram();
                    break;
                case nameof(Language):
                    Messenger.SetCulture.Publish(Language.GetName());
                    break;
                default:
                    OnPropertyChanged(e);
                    break;
            }
        }

        #endregion
    }
}
