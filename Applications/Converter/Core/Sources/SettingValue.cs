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
    /// SettingValue
    ///
    /// <summary>
    /// Represents the user settings.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataContract]
    public class SettingValue : SerializableBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// SettingValue
        ///
        /// <summary>
        /// Initializes a new instance of the SettingValue class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public SettingValue() { Reset(); }

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
            get => GetProperty<Format>();
            set => SetProperty(value);
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
            get => GetProperty<SaveOption>();
            set => SetProperty(value);
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
            get => GetProperty<Orientation>();
            set => SetProperty(value);
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
            get => GetProperty<Downsampling>();
            set => SetProperty(value);
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
            get => GetProperty<int>();
            set => SetProperty(value);
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
            get => GetProperty<bool>();
            set => SetProperty(value);
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
            get => GetProperty<bool>();
            set => SetProperty(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ImageFilter
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to compress embedded
        /// images as JPEG format.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public bool ImageFilter
        {
            get => GetProperty<bool>();
            set => SetProperty(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Linearization
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to apply the
        /// linearization option (a.k.a Web optimization).
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember(Name = "WebOptimize")]
        public bool Linearization
        {
            get => GetProperty<bool>();
            set => SetProperty(value);
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
        [DataMember]
        public bool SourceVisible
        {
            get => GetProperty<bool>();
            set => SetProperty(value);
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
            get => GetProperty<bool>();
            set => SetProperty(value);
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
            get => GetProperty<bool>();
            set => SetProperty(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// PlatformCompatible
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to ignore
        /// PlatformNotSupportedException exceptions as possible.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public bool PlatformCompatible
        {
            get => GetProperty<bool>();
            set => SetProperty(value);
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
            get => GetProperty<Language>();
            set => SetProperty(value);
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
            get => GetProperty<PostProcess>();
            set => SetProperty(value);
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
            get => GetProperty<string>();
            set => SetProperty(value);
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
            get => GetProperty<string>();
            set => SetProperty(value);
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
            get => GetProperty<string>();
            set => SetProperty(value);
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
            get => GetProperty<Metadata>();
            set => SetProperty(value);
        }

        #endregion

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
            get => GetProperty<Encryption>();
            set => SetProperty(value);
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
            get => GetProperty<string>();
            set => SetProperty(value);
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
            get => GetProperty<bool>();
            set => SetProperty(value);
        }

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
            get => GetProperty<bool>();
            set => SetProperty(value);
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
            Format             = Format.Pdf;
            SaveOption         = SaveOption.Overwrite;
            Orientation        = Orientation.Auto;
            Downsampling       = Downsampling.None;
            PostProcess        = PostProcess.None;
            Language           = Language.Auto;
            Resolution         = 600;
            Grayscale          = false;
            EmbedFonts         = true;
            ImageFilter        = true;
            Linearization      = false;
            SourceVisible      = false;
            CheckUpdate        = true;
            ExplicitDirectory  = false;
            PlatformCompatible = true;
            Temp               = $@"{Environment.SpecialFolder.CommonApplicationData.GetName()}\CubeSoft\CubePDF";
            Source             = string.Empty;
            Destination        = Environment.SpecialFolder.Desktop.GetName();
            UserProgram        = string.Empty;
            Metadata           = new Metadata();
            Encryption         = new Encryption();
            DeleteSource       = false;
            Busy               = false;
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
    }
}
