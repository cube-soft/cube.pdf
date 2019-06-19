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
    /// SettingViewModel
    ///
    /// <summary>
    /// Represents the ViewModel for the general and others tabs in
    /// the main window.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class SettingViewModel : ViewModelBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// SettingViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the SettingViewModel class with
        /// the specified arguments.
        /// </summary>
        ///
        /// <param name="settings">User settings.</param>
        /// <param name="aggregator">Message aggregator.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public SettingViewModel(SettingFolder settings, Aggregator aggregator,
            SynchronizationContext context) : base(aggregator, context)
        {
            _io    = settings.IO;
            _model = settings.Value;
            _model.PropertyChanged += (s, e) => OnPropertyChanged(e);
        }

        #endregion

        #region Properties

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
            get => _model.Format;
            set
            {
                _model.Format = value;
                RaisePropertyChanged(nameof(IsPdf));
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
            get => _model.SaveOption;
            set => _model.SaveOption = value;
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
            get => _model.PostProcess;
            set
            {
                _model.PostProcess = value;
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
            get => _model.Source;
            set => _model.Source = value;
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
            get => _model.Destination;
            set => _model.Destination = value;
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
            get => _model.UserProgram;
            set => _model.UserProgram = value;
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
            get => _model.Resolution;
            set => _model.Resolution = value;
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
            get => _model.Orientation == Orientation.Auto;
            set
            {
                if (value)
                {
                    _model.Orientation = Orientation.Auto;
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
            get => _model.Orientation == Orientation.Portrait;
            set
            {
                if (value)
                {
                    _model.Orientation = Orientation.Portrait;
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
            get => _model.Orientation == Orientation.Landscape;
            set
            {
                if (value)
                {
                    _model.Orientation = Orientation.Landscape;
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
            get => _model.Grayscale;
            set => _model.Grayscale = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ImageFilter
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to compress images
        /// embedded in the PDF.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool ImageFilter
        {
            get => _model.ImageFilter;
            set => _model.ImageFilter = value;
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
            get => _model.Linearization;
            set => _model.Linearization = value;
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
            get => _model.CheckUpdate;
            set => _model.CheckUpdate = value;
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
            get => _model.Language;
            set => _model.Language = value;
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
        public bool SourceVisible => _model.SourceVisible;

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
        /// 入力ファイルが指定された場合、変更不可能としています。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public bool SourceEditable => !_model.DeleteSource;

        /* ----------------------------------------------------------------- */
        ///
        /// IsPdf
        ///
        /// <summary>
        /// Gets or sets a value indicating whether the current format is
        /// PDF.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool IsPdf => Format == Format.Pdf;

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
            if (!_io.Exists(Destination) || SaveOption == SaveOption.Rename) return true;
            else return Confirm(MessageFactory.Create(Destination, SaveOption));
        }

        #endregion

        #region Fields
        private readonly SettingValue _model;
        private readonly IO _io;
        #endregion
    }
}
