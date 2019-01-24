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
using Cube.Pdf.Ghostscript;
using System;
using System.Runtime.Serialization;

namespace Cube.Pdf.App.Converter
{
    /* --------------------------------------------------------------------- */
    ///
    /// Settings
    ///
    /// <summary>
    /// ユーザ設定を保持するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataContract]
    public class Settings : ObservableProperty
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// Settings
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Settings()
        {
            Reset();
        }

        #endregion

        #region Properties

        #region DataMember

        /* ----------------------------------------------------------------- */
        ///
        /// Format
        ///
        /// <summary>
        /// 変換形式を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember(Name = "FileType")]
        public Format Format
        {
            get => _format;
            set => SetProperty(ref _format, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// FormatOption
        ///
        /// <summary>
        /// 変換形式に関するオプションを取得または設定します。
        /// </summary>
        ///
        /// <remarks>
        /// 旧 CubePDF で PDFVersion と呼んでいたものを汎用化した形で定義
        /// しています。PDF のバージョン以外のオプションが定義された段階で
        /// レジストリ等の設定項目名も変更する予定です。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember(Name = "PDFVersion")]
        public FormatOption FormatOption
        {
            get => _formatOption;
            set => SetProperty(ref _formatOption, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SaveOption
        ///
        /// <summary>
        /// 保存オプションを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember(Name = "ExistedFile")]
        public SaveOption SaveOption
        {
            get => _saveOption;
            set => SetProperty(ref _saveOption, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Orientation
        ///
        /// <summary>
        /// ページの向きを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public Orientation Orientation
        {
            get => _orientation;
            set => SetProperty(ref _orientation, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Downsampling
        ///
        /// <summary>
        /// ダウンサンプリング方式を取得または設定します。
        /// </summary>
        ///
        /// <remarks>
        /// 大きな効果が見込めないため、None で固定しユーザからは選択
        /// 不可能にしています。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public Downsampling Downsampling
        {
            get => _downsampling;
            set => SetProperty(ref _downsampling, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Resolution
        ///
        /// <summary>
        /// 解像度を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public int Resolution
        {
            get => _resolution;
            set => SetProperty(ref _resolution, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Grayscale
        ///
        /// <summary>
        /// グレースケールに変換するかどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public bool Grayscale
        {
            get => _grayscale;
            set => SetProperty(ref _grayscale, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// EmbedFonts
        ///
        /// <summary>
        /// フォント情報を埋め込むかどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /// <remarks>
        /// フォントを埋め込まない場合に文字化けする不都合が確認されている
        /// ため、GUI からは設定不可能にしています。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public bool EmbedFonts
        {
            get => _embedFonts;
            set => SetProperty(ref _embedFonts, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ImageCompression
        ///
        /// <summary>
        /// PDF 中の画像を JPEG 圧縮するかどうかを示す値を取得または
        /// 設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember(Name = "ImageFilter")]
        public bool ImageCompression
        {
            get => _imageCompression;
            set => SetProperty(ref _imageCompression, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Linearization
        ///
        /// <summary>
        /// PDF ファイルを Web 表示用に最適化するかどうかを示す値を取得
        /// または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember(Name = "WebOptimize")]
        public bool Linearization
        {
            get => _linearization;
            set => SetProperty(ref _linearization, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SourceVisible
        ///
        /// <summary>
        /// 入力ファイルのパスがユーザに確認できる形で表示するかどうかを
        /// 示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember(Name = "SelectInputFile")]
        public bool SourceVisible
        {
            get => _sourceVisible;
            set => SetProperty(ref _sourceVisible, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CheckUpdate
        ///
        /// <summary>
        /// アップデートの確認を実行するかどうかを示す値を取得または
        /// 設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public bool CheckUpdate
        {
            get => _checkUpdate;
            set => SetProperty(ref _checkUpdate, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ExplicitDirectory
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to set a value to the
        /// InitialDirectory property explicitly when showing a dialog
        /// that selects the file or directory name.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public bool ExplicitDirectory
        {
            get => _explicit;
            set => SetProperty(ref _explicit, value);
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
        [DataMember]
        public Language Language
        {
            get => _language;
            set => SetProperty(ref _language, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// PostProcess
        ///
        /// <summary>
        /// ポストプロセスを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public PostProcess PostProcess
        {
            get => _postProcess;
            set => SetProperty(ref _postProcess, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// UserProgram
        ///
        /// <summary>
        /// ユーザプログラムのパスを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public string UserProgram
        {
            get => _userProgram;
            set => SetProperty(ref _userProgram, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Destination
        ///
        /// <summary>
        /// 保存先のパスを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember(Name = "LastAccess")]
        public string Destination
        {
            get => _destination;
            set => SetProperty(ref _destination, value);
        }

        #endregion

        /* ----------------------------------------------------------------- */
        ///
        /// SkipUi
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to skip displaying the
        /// main window.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool SkipUi
        {
            get => _skipUi;
            set => SetProperty(ref _skipUi, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// IsBusy
        ///
        /// <summary>
        /// 変換処理中かどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool IsBusy
        {
            get => _busy;
            set => SetProperty(ref _busy, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// DeleteSource
        ///
        /// <summary>
        /// 変換処理完了後に入力ファイルを削除するかどうかを示す値を
        /// 取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool DeleteSource
        {
            get => _deleteSource;
            set => SetProperty(ref _deleteSource, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Source
        ///
        /// <summary>
        /// 入力ファイルのパスを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Source
        {
            get => _source;
            set => SetProperty(ref _source, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Metadata
        ///
        /// <summary>
        /// PDF のメタ情報を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Metadata Metadata
        {
            get => _metadata;
            set => SetProperty(ref _metadata, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Encryption
        ///
        /// <summary>
        /// PDF の暗号化情報を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Encryption Encryption
        {
            get => _encryption;
            set => SetProperty(ref _encryption, value);
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Reset
        ///
        /// <summary>
        /// 設定をリセットします。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Reset()
        {
            _format           = Format.Pdf;
            _formatOption     = FormatOption.Pdf17;
            _saveOption       = SaveOption.Overwrite;
            _orientation      = Orientation.Auto;
            _downsampling     = Downsampling.None;
            _postProcess      = PostProcess.Open;
            _language         = Language.Auto;
            _resolution       = 600;
            _grayscale        = false;
            _embedFonts       = true;
            _imageCompression = true;
            _linearization    = false;
            _sourceVisible    = false;
            _checkUpdate      = true;
            _explicit         = false;
            _source           = string.Empty;
            _destination      = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            _userProgram      = string.Empty;
            _metadata         = new Metadata();
            _encryption       = new Encryption();
            _skipUi           = false;
            _busy             = false;
            _deleteSource     = false;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnDeserializing
        ///
        /// <summary>
        /// デシリアライズ直前に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [OnDeserializing]
        private void OnDeserializing(StreamingContext context) => Reset();

        #endregion

        #region Fields
        private Format _format;
        private FormatOption _formatOption;
        private SaveOption _saveOption;
        private Orientation _orientation;
        private Downsampling _downsampling;
        private PostProcess _postProcess;
        private Language _language;
        private int _resolution;
        private bool _grayscale;
        private bool _embedFonts;
        private bool _imageCompression;
        private bool _linearization;
        private bool _sourceVisible;
        private bool _checkUpdate;
        private bool _explicit;
        private string _source;
        private string _destination;
        private string _userProgram;
        private Metadata _metadata;
        private Encryption _encryption;
        private bool _skipUi;
        private bool _busy;
        private bool _deleteSource;
        #endregion
    }
}
