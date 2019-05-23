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
using Cube.Pdf.Ghostscript;
using System.Threading;

namespace Cube.Pdf.Converter
{
    /* --------------------------------------------------------------------- */
    ///
    /// SettingsViewModel
    ///
    /// <summary>
    /// Represents the viewmodel for the general and others tabs in
    /// the main window.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class SettingsViewModel : CommonViewModel
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// SettingsViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the SettingsViewModel class with
        /// the specified arguments.
        /// </summary>
        ///
        /// <param name="settings">User settings.</param>
        /// <param name="aggregator">Event aggregator.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public SettingsViewModel(SettingsFolder settings, Aggregator aggregator,
            SynchronizationContext context) : base(aggregator, context)
        {
            IO    = settings.IO;
            Model = settings.Value;
            Model.PropertyChanged += (s, e) => OnPropertyChanged(e);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Model
        ///
        /// <summary>
        /// Gets the model object.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected SettingsValue Model { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// IO
        ///
        /// <summary>
        /// Gets the I/O handler.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected IO IO { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Format
        ///
        /// <summary>
        /// Gets or sets the conversion format.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Format Format
        {
            get => Model.Format;
            set
            {
                Model.Format = value;
                RaisePropertyChanged(nameof(EnableFormatOption));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SaveOption
        ///
        /// <summary>
        /// Gets or sets the saving option.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public SaveOption SaveOption
        {
            get => Model.SaveOption;
            set => Model.SaveOption = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// PostProcess
        ///
        /// <summary>
        /// Gets or sets the kind of port-process.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PostProcess PostProcess
        {
            get => Model.PostProcess;
            set
            {
                Model.PostProcess = value;
                RaisePropertyChanged(nameof(EnableUserProgram));
            }
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
            get => Model.Source;
            set => Model.Source = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Destination
        ///
        /// <summary>
        /// Gets or sets the path to save.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Destination
        {
            get => Model.Destination;
            set => Model.Destination = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// UserProgram
        ///
        /// <summary>
        /// Gets or sets the path of the user program.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string UserProgram
        {
            get => Model.UserProgram;
            set => Model.UserProgram = value;
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
        public int Resolution
        {
            get => Model.Resolution;
            set => Model.Resolution = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// IsAutoOrientation
        ///
        /// <summary>
        /// Gets or sets a value indicating whether the orientation is
        /// equal to auto.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool IsAutoOrientation
        {
            get => Model.Orientation == Orientation.Auto;
            set
            {
                if (value)
                {
                    Model.Orientation = Orientation.Auto;
                    RaisePropertyChanged(nameof(IsAutoOrientation));
                }
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// IsPortrait
        ///
        /// <summary>
        /// Gets or sets a value indicating whether the orientation is
        /// equal to portrait.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool IsPortrait
        {
            get => Model.Orientation == Orientation.Portrait;
            set
            {
                if (value)
                {
                    Model.Orientation = Orientation.Portrait;
                    RaisePropertyChanged(nameof(IsPortrait));
                }
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// IsLandscape
        ///
        /// <summary>
        /// Gets or sets a value indicating whether the orientation is
        /// equal to landscape.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool IsLandscape
        {
            get => Model.Orientation == Orientation.Landscape;
            set
            {
                if (value)
                {
                    Model.Orientation = Orientation.Landscape;
                    RaisePropertyChanged(nameof(IsLandscape));
                }
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Grayscale
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to enable the
        /// grayscale option.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Grayscale
        {
            get => Model.Grayscale;
            set => Model.Grayscale = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ImageCompression
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to compress images
        /// embedded in the PDF.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool ImageCompression
        {
            get => Model.ImageCompression;
            set => Model.ImageCompression = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Linearization
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to linearize the
        /// PDF (a.k.a PDF web optimization).
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Linearization
        {
            get => Model.Linearization;
            set => Model.Linearization = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CheckUpdate
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to enable the checking
        /// CubePDF updates.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool CheckUpdate
        {
            get => Model.CheckUpdate;
            set => Model.CheckUpdate = value;
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
        public Language Language
        {
            get => Model.Language;
            set => Model.Language = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SourceVisible
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to display the input
        /// form of the source file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool SourceVisible => Model.SourceVisible;

        /* ----------------------------------------------------------------- */
        ///
        /// SourceEditable
        ///
        /// <summary>
        /// Gets or sets a value indicating whether the input form of
        /// the source file is editable.
        /// </summary>
        ///
        /// <remarks>
        /// 現在は、プログラム引数で /DeleteOnClose オプションとともに
        /// 入力ファイルが指定された場合、変更不可能な形となっています。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public bool SourceEditable => !Model.DeleteSource;

        /* ----------------------------------------------------------------- */
        ///
        /// EnableFormatOption
        ///
        /// <summary>
        /// Gets or sets a value indicating whether the FormatOption
        /// value is selectable.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool EnableFormatOption => Format == Format.Pdf;

        /* ----------------------------------------------------------------- */
        ///
        /// EnableUserProgram
        ///
        /// <summary>
        /// Gets or sets a value indicating whether the input form of the
        /// user program is editable.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool EnableUserProgram => PostProcess == PostProcess.Others;

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Confirm
        ///
        /// <summary>
        /// Confirms if the current settings are acceptable.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Confirm()
        {
            if (IO.Exists(Destination) && SaveOption != SaveOption.Rename) return true;
            else return Confirm(MessageFactory.Create(Destination, SaveOption));
        }

        #endregion

        #region Implementations

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
        protected override void Dispose(bool disposing) { }

        #endregion
    }
}
