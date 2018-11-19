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
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cube.Pdf.Ghostscript
{
    /* --------------------------------------------------------------------- */
    ///
    /// PdfConverter
    ///
    /// <summary>
    /// Provides functionality to convert to PDF format.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class PdfConverter : DocumentConverter
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// PdfConverter
        ///
        /// <summary>
        /// Initializes a new instance of the PdfConverter class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PdfConverter() : this(new IO()) { }

        /* ----------------------------------------------------------------- */
        ///
        /// PdfConverter
        ///
        /// <summary>
        /// Initializes a new instance of the PdfConverter class.
        /// </summary>
        ///
        /// <param name="io">I/O handler.</param>
        ///
        /* ----------------------------------------------------------------- */
        public PdfConverter(IO io) : this(Format.Pdf, io, SupportedFormats) { }

        /* ----------------------------------------------------------------- */
        ///
        /// PdfConverter
        ///
        /// <summary>
        /// Initializes a new instance of the PdfConverter class with the
        /// specified parameters.
        /// </summary>
        ///
        /// <param name="format">Target format.</param>
        /// <param name="io">I/O handler.</param>
        /// <param name="supported">Collection of supported formats.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected PdfConverter(Format format, IO io, IEnumerable<Format> supported) :
            base(format, io, supported) { }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// SupportedFormats
        ///
        /// <summary>
        /// Gets the collection of supported formats.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static new IEnumerable<Format> SupportedFormats { get; } = new[] { Format.Pdf };

        /* ----------------------------------------------------------------- */
        ///
        /// Version
        ///
        /// <summary>
        /// Gets or sets the version number of the converted document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Version Version { get; set; } = new Version(1, 7);

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// OnCreateArguments
        ///
        /// <summary>
        /// Occurs when creating Ghostscript API arguments.
        /// </summary>
        ///
        /// <returns>Collection of arguments.</returns>
        ///
        /* ----------------------------------------------------------------- */
        protected override IEnumerable<Argument> OnCreateArguments() =>
            base.OnCreateArguments()
            .Concat(new[] { CreateVersion() });

        /* ----------------------------------------------------------------- */
        ///
        /// CreateVersion
        ///
        /// <summary>
        /// Creates a new instance of the Argument class representing
        /// version number.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private Argument CreateVersion() =>
            new Argument('d', "CompatibilityLevel", $"{Version.Major}.{Version.Minor}");

        #endregion
    }
}
