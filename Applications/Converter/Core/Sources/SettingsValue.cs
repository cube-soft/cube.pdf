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
using Cube.Mixin.Environment;
using Cube.Pdf.Ghostscript;
using System;
using System.Runtime.Serialization;

namespace Cube.Pdf.Converter
{
    /* --------------------------------------------------------------------- */
    ///
    /// SettingsValue
    ///
    /// <summary>
    /// Represents the user settings.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataContract]
    public class SettingsValue : ObservableBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// SettingsValue
        ///
        /// <summary>
        /// Initializes a new instance of the Settings class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public SettingsValue() { Reset(); }

        #endregion

        #region Properties

        #region DataMember

        /* ----------------------------------------------------------------- */
        ///
        /// Format
        ///
        /// <summary>
        /// Gets or sets the converting format.
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
        /// SaveOption
        ///
        /// <summary>
        /// Gets or sets a value that represents save options.
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
        /// Gets or sets the page orientation.
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
        /// Gets or sets a value that represents the method of downsampling.
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
        /// Gets or sets the resolution.
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
        /// Gets or sets a value indicating whether to convert in grayscale.
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
        /// Gets or sets a value indicating whether to embed fonts.
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
        /// Gets or sets a value indicating whether to compress embedded
        /// images as JPEG format.
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
        /// Gets or sets a value indicating whether to apply the
        /// linearization option (aka: Web optimization).
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
        /// Gets or sets a value indicating whether to display the
        /// path of the source file.
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
        /// Gets or sets a value indicating whether to check the update
        /// of the application.
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
        /// Gets or sets the displayed language.
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
        /// Gets or sets a value that represents the post-process.
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
        /// Gets or sets the path of the user program that executes after
        /// converting.
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
        /// Gets or sets the path to save the converted file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember(Name = "LastAccess")]
        public string Destination
        {
            get => _destination;
            set => SetProperty(ref _destination, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Temp
        ///
        /// <summary>
        /// Gets or sets the path of the temp directory.
        /// </summary>
        ///
        /// <remarks>
        /// Ghostscript はパスにマルチバイト文字が含まれる場合、処理に
        /// 失敗する場合があります。そのため、マルチバイト文字の含まれない
        /// ディレクトリに移動して処理を実行します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public string Temp
        {
            get => _temp;
            set => SetProperty(ref _temp, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Metadata
        ///
        /// <summary>
        /// Gets or sets the PDF metadata.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public Metadata Metadata
        {
            get => _metadata;
            set => SetProperty(ref _metadata, value);
        }

        #endregion

        /* ----------------------------------------------------------------- */
        ///
        /// Busy
        ///
        /// <summary>
        /// Gets or sets a value indicating whether the application is busy.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Busy
        {
            get => _busy;
            set => SetProperty(ref _busy, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// DeleteSource
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to delete the source
        /// file after converting.
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
        /// Gets or sets the path of the source file.
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
        /// Encryption
        ///
        /// <summary>
        /// Gets or sets the encryption information.
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
        /// Resets values.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Reset()
        {
            _format           = Format.Pdf;
            _saveOption       = SaveOption.Overwrite;
            _orientation      = Orientation.Auto;
            _downsampling     = Downsampling.None;
            _postProcess      = PostProcess.None;
            _language         = Language.Auto;
            _resolution       = 600;
            _grayscale        = false;
            _embedFonts       = true;
            _imageCompression = true;
            _linearization    = false;
            _sourceVisible    = false;
            _checkUpdate      = true;
            _explicit         = false;
            _temp             = $@"{Environment.SpecialFolder.CommonApplicationData.GetName()}\CubeSoft\CubePDF";
            _source           = string.Empty;
            _destination      = Environment.SpecialFolder.Desktop.GetName();
            _userProgram      = string.Empty;
            _metadata         = new Metadata();
            _encryption       = new Encryption();
            _busy             = false;
            _deleteSource     = false;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnDeserializing
        ///
        /// <summary>
        /// Occurs before deserializing.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [OnDeserializing]
        private void OnDeserializing(StreamingContext context) => Reset();

        #endregion

        #region Fields
        private Format _format;
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
        private string _temp;
        private string _source;
        private string _destination;
        private string _userProgram;
        private Metadata _metadata;
        private Encryption _encryption;
        private bool _busy;
        private bool _deleteSource;
        #endregion
    }
}
