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
    public sealed class SettingViewModel : Presentable<SettingFolder>
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
            SynchronizationContext context) : base(settings, aggregator, context)
        {
            Facade.Value.PropertyChanged += (s, e) => OnPropertyChanged(e);
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
            get => Facade.Value.Format;
            set
            {
                Facade.Value.Format = value;
                Refresh(nameof(IsPdf));
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
            get => Facade.Value.SaveOption;
            set => Facade.Value.SaveOption = value;
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
            get => Facade.Value.PostProcess;
            set
            {
                Facade.Value.PostProcess = value;
                Refresh(nameof(EnableUserProgram));
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
            get => Facade.Value.Source;
            set => Facade.Value.Source = value;
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
            get => Facade.Value.Destination;
            set => Facade.Value.Destination = value;
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
            get => Facade.Value.UserProgram;
            set => Facade.Value.UserProgram = value;
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
            get => Facade.Value.Resolution;
            set => Facade.Value.Resolution = value;
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
            get => Facade.Value.Orientation == Orientation.Auto;
            set
            {
                if (value)
                {
                    Facade.Value.Orientation = Orientation.Auto;
                    Refresh(nameof(IsAutoOrientation));
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
            get => Facade.Value.Orientation == Orientation.Portrait;
            set
            {
                if (value)
                {
                    Facade.Value.Orientation = Orientation.Portrait;
                    Refresh(nameof(IsPortrait));
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
            get => Facade.Value.Orientation == Orientation.Landscape;
            set
            {
                if (value)
                {
                    Facade.Value.Orientation = Orientation.Landscape;
                    Refresh(nameof(IsLandscape));
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
            get => Facade.Value.Grayscale;
            set => Facade.Value.Grayscale = value;
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
            get => Facade.Value.ImageFilter;
            set => Facade.Value.ImageFilter = value;
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
            get => Facade.Value.Linearization;
            set => Facade.Value.Linearization = value;
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
            get => Facade.Value.CheckUpdate;
            set => Facade.Value.CheckUpdate = value;
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
            get => Facade.Value.Language;
            set => Facade.Value.Language = value;
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
        public bool SourceVisible => Facade.Value.SourceVisible;

        /* ----------------------------------------------------------------- */
        ///
        /// SourceEditable
        ///
        /// <summary>
        /// Gets or sets a value indicating whether the input form of
        /// the source file is editable.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool SourceEditable => !Facade.Value.DeleteSource;

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
            if (!Facade.IO.Exists(Destination) || SaveOption == SaveOption.Rename) return true;
            else
            {
                var src = MessageFactory.Create(Destination, SaveOption);
                Send(src);
                return src.Value == DialogStatus.Yes;
            }
        }

        #endregion
    }
}
