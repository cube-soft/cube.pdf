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
using Cube.Mixin.Assembly;
using Cube.Mixin.String;
using System;
using System.ComponentModel;
using System.Threading;

namespace Cube.Pdf.Converter
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
    public sealed class MainViewModel : CommonViewModel
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
        public MainViewModel(SettingsFolder settings) :
            this(settings, SynchronizationContext.Current) { }

        /* ----------------------------------------------------------------- */
        ///
        /// MainViewModel
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="settings">設定情報</param>
        /// <param name="context">同期用コンテキスト</param>
        ///
        /* ----------------------------------------------------------------- */
        public MainViewModel(SettingsFolder settings, SynchronizationContext context) :
            base(new Aggregator(), context)
        {
            _model     = new Facade(settings);
            Settings   = new SettingsViewModel(settings, Aggregator, context);
            Metadata   = new MetadataViewModel(settings.Value.Metadata, Aggregator, context);
            Encryption = new EncryptionViewModel(settings.Value.Encryption, Aggregator, context);

            settings.PropertyChanged += WhenSettingsChanged;
        }

        #endregion

        #region Properties

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
            _model.Settings.Document.Value.HasValue() ?
            $"{_model.Settings.Document.Value} - {Product} {Version}" :
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
        public string Product => _model.Settings.Assembly.GetProduct();

        /* ----------------------------------------------------------------- */
        ///
        /// Version
        ///
        /// <summary>
        /// バージョンを表す文字列を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Version => _model.Settings.Version.ToString(true);

        /* ----------------------------------------------------------------- */
        ///
        /// Uri
        ///
        /// <summary>
        /// Web ページの URL を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Uri Uri => GlobalSettings.Uri;

        /* ----------------------------------------------------------------- */
        ///
        /// Busy
        ///
        /// <summary>
        /// 処理中かどうかを示す値を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Busy => _model.Settings.Value.Busy;

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

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Convert
        ///
        /// <summary>
        /// 変換処理を実行するコマンドです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Convert()
        {
            if (Encryption.Confirm() && Settings.Confirm()) TrackClose(() =>
            {
                _model.ChangeExtension();
                _model.Convert();
            });
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// 設定を保存するコマンドです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Save()
        {
            if (Metadata.ConfirmForSave()) _model.Save();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// BrowseSource
        ///
        /// <summary>
        /// 入力ファイルの選択画面を表示するコマンドです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void BrowseSource() =>
            Send(_model.Settings.CreateForSource(), e => _model.SetSource(e));


        /* ----------------------------------------------------------------- */
        ///
        /// BrowseDestination
        ///
        /// <summary>
        /// 保存パスの選択画面を表示するコマンドです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void BrowseDestination() =>
            Send(_model.Settings.CreateForDestination(), e => _model.SetDestination(e));

        /* ----------------------------------------------------------------- */
        ///
        /// BrowseUserProgram
        ///
        /// <summary>
        /// ユーザプログラムの選択画面を表示するコマンドです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void BrowseUserProgram() =>
            Send(_model.Settings.CreateForUserProgram(), e => _model.SetUserProgram(e));

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases the unmanaged resources used by the object and
        /// optionally releases the managed resources.
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
            try { if (disposing) _model.Dispose(); }
            finally { base.Dispose(disposing); }
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// WhenSettingsChanged
        ///
        /// <summary>
        /// Occurs when any settings are changed.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenSettingsChanged(object s, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Settings.Format):
                    _model.ChangeExtension();
                    break;
                case nameof(Settings.PostProcess):
                    if (Settings.PostProcess == PostProcess.Others) BrowseUserProgram();
                    break;
                case nameof(Settings.Language):
                    Locale.Set(Settings.Language);
                    break;
                default:
                    OnPropertyChanged(e);
                    break;
            }
        }

        #endregion

        #region Fields
        private readonly Facade _model;
        #endregion
    }
}
