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
using System;
using System.Collections.Generic;
using System.Linq;
using Cube.FileSystem;
using Cube.Text.Extensions;

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
        /// Initializes a new instance of the SaveOption class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public SaveOption() : this(Dispatcher.Vanilla) { }

        /* ----------------------------------------------------------------- */
        ///
        /// SaveOption
        ///
        /// <summary>
        /// Initializes a new instance of the SaveOption class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="dispatcher">Dispatcher object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public SaveOption(Dispatcher dispatcher) : base(dispatcher)
        {
            Resolution = 144;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Temp
        ///
        /// <summary>
        /// Gets or sets the path of the working directory. If this value is
        /// empty, the program will use the same directory as the path
        /// specified in Destination as its working directory.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Temp
        {
            get => Get(() => string.Empty);
            set => Set(value);
        }

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
            get => Get(() => string.Empty);
            set { if (Set(value)) SetFormat(); }
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
            get => Get(() => SaveFormat.Pdf);
            set { if (Set(value)) SetDestination(); }
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
            get => Get(() => SaveTarget.All);
            set => Set(value);
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
            get => Get(() => string.Empty);
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Resolution
        ///
        /// <summary>
        /// Gets or sets the resolution of saved image.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Resolution
        {
            get => Get(() => 72);
            set => Set(value);
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
            get => Get(() => false);
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ShrinkResources
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to shrink deduplicated
        /// resources when saving PDF files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool ShrinkResources
        {
            get => Get(() => false);
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// KeepOutlines
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to keep outline
        /// information when saving PDF files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool KeepOutlines
        {
            get => Get(() => true);
            set => Set(value);
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
        public Metadata Metadata
        {
            get => Get<Metadata>();
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Encryption
        ///
        /// <summary>
        /// Gets or sets the PDF encryption information.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Encryption Encryption
        {
            get => Get<Encryption>();
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Attachments
        ///
        /// <summary>
        /// Gets or sets the collection of objects to attach the PDF
        /// document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<Attachment> Attachments
        {
            get => Get(Enumerable.Empty<Attachment>);
            set => Set(value);
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// ToItext
        ///
        /// <summary>
        /// Converts to the Itext.SaveOption object.
        /// </summary>
        ///
        /// <returns>Itext.SaveOption object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public Itext.SaveOption ToItext() => new()
        {
            Temp            = Temp,
            ShrinkResources = ShrinkResources,
            KeepOutlines    = KeepOutlines,
        };

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
            Destination = Io.Combine(fi.DirectoryName, name);
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
        private Entity GetEntity(string src) => src.HasValue() ? Io.Get(src) : default;

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
