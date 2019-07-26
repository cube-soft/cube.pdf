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
using Cube.Mixin.String;
using System;
using System.Linq;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// SaveOption
    ///
    /// <summary>
    /// Represents the extract option.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class SaveOption : ObservableBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// SaveOption
        ///
        /// <summary>
        /// Initializes a new instance of the SaveOption class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="io">I/O handler.</param>
        ///
        /* ----------------------------------------------------------------- */
        public SaveOption(IO io) : this(io, Invoker.Vanilla) { }

        /* ----------------------------------------------------------------- */
        ///
        /// SaveOption
        ///
        /// <summary>
        /// Initializes a new instance of the SaveOption class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="io">I/O handler.</param>
        /// <param name="invoker">Invoker object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public SaveOption(IO io, Invoker invoker) : base(invoker) { IO = io; }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Destination
        ///
        /// <summary>
        /// Gets or sets the path to save the target pages.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Destination
        {
            get => GetProperty<string>();
            set { if (SetProperty(value)) SetFormat(); }
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
        public SaveFormat Format
        {
            get => GetProperty<SaveFormat>();
            set { if (SetProperty(value)) SetDestination(); }
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
        public SaveTarget Target
        {
            get => GetProperty<SaveTarget>();
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

        /* ----------------------------------------------------------------- */
        ///
        /// IO
        ///
        /// <summary>
        /// Gets the I/O handler.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IO IO { get; }

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

        /* ----------------------------------------------------------------- */
        ///
        /// SetFormat
        ///
        /// <summary>
        /// Sets the Format property according to the Destination.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SetFormat()
        {
            try
            {
                var fi = GetEntity(Destination);
                if (fi == null || !fi.Extension.HasValue()) return;

                Format = Enum.GetValues(typeof(SaveFormat))
                             .OfType<SaveFormat>()
                             .First(e => fi.Extension.FuzzyEquals($".{e}"));
            }
            catch { /* Not found */ }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetDestination
        ///
        /// <summary>
        /// Sets the Destination property according to the Format.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SetDestination()
        {
            var fi = GetEntity(Destination);
            if (fi == null || fi.Extension.FuzzyEquals($".{Format}")) return;

            var name = $"{fi.BaseName}.{Format.ToString().ToLowerInvariant()}";
            Destination = IO.Combine(fi.DirectoryName, name);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetEntity
        ///
        /// <summary>
        /// Creates a new instance of the Entity class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private Entity GetEntity(string src) => src.HasValue() ? IO.Get(src) : null;

        #endregion
    }

    /* --------------------------------------------------------------------- */
    ///
    /// SaveFormat
    ///
    /// <summary>
    /// Specifies the saving formats.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public enum SaveFormat
    {
        /// <summary>PDF</summary>
        Pdf,
        /// <summary>PNG</summary>
        Png,
    }

    /* --------------------------------------------------------------------- */
    ///
    /// SaveTarget
    ///
    /// <summary>
    /// Specifies the target pages to be saved.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public enum SaveTarget
    {
        /// <summary>All pages</summary>
        All,
        /// <summary>Selected pages</summary>
        Selected,
        /// <summary>Specified pages</summary>
        Range,
    }
}
