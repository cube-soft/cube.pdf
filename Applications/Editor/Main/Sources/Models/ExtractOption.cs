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
namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// ExtractOption
    ///
    /// <summary>
    /// Represents the extract option.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class ExtractOption : ObservableBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ExtractOption
        ///
        /// <summary>
        /// Initializes a new instance of the ExtractOption class with
        /// the specified arguments.
        /// </summary>
        ///
        /// <param name="invoker">Invoker object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ExtractOption(Invoker invoker) : base(invoker) { }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Destination
        ///
        /// <summary>
        /// Gets or sets the path to save the extracted pages.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Destination
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Format
        ///
        /// <summary>
        /// Gets or sets the format to save.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ExtractFormat Format
        {
            get => GetProperty<ExtractFormat>();
            set => SetProperty(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Target
        ///
        /// <summary>
        /// Gets or sets the value of target pages.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ExtractTarget Target
        {
            get => GetProperty<ExtractTarget>();
            set => SetProperty(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Range
        ///
        /// <summary>
        /// Gets or sets the target range.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Range
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Split
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to save as a separate
        /// file per page.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Split
        {
            get => GetProperty<bool>();
            set => SetProperty(value);
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

    /* --------------------------------------------------------------------- */
    ///
    /// ExtractFormat
    ///
    /// <summary>
    /// Specifies the saving formats.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public enum ExtractFormat
    {
        /// <summary>PDF</summary>
        Pdf,
        /// <summary>PNG</summary>
        Png,
    }

    /* --------------------------------------------------------------------- */
    ///
    /// ExtractTarget
    ///
    /// <summary>
    /// Specifies the target to extract.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public enum ExtractTarget
    {
        /// <summary>All pages</summary>
        All,
        /// <summary>Selected pages</summary>
        Selected,
        /// <summary>Specified pages</summary>
        Range,
    }
}
