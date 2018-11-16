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
    /// JpegConverter
    ///
    /// <summary>
    /// Provides functionality to convert to JPEG format.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class JpegConverter : ImageConverter
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// JpegConverter
        ///
        /// <summary>
        /// Initializes a new instance of the JpegConverter class with the
        /// specified format.
        /// </summary>
        ///
        /// <param name="format">Target format.</param>
        ///
        /* ----------------------------------------------------------------- */
        public JpegConverter(Format format) : this(format, new IO()) { }

        /* ----------------------------------------------------------------- */
        ///
        /// JpegConverter
        ///
        /// <summary>
        /// Initializes a new instance of the JpegConverter class with the
        /// specified format.
        /// </summary>
        ///
        /// <param name="format">Target format.</param>
        /// <param name="io">I/O handler.</param>
        ///
        /* ----------------------------------------------------------------- */
        public JpegConverter(Format format, IO io) : this(format, io, GetSupportedFormats()) { }

        /* ----------------------------------------------------------------- */
        ///
        /// JpegConverter
        ///
        /// <summary>
        /// Initializes a new instance of the JpegConverter class with the
        /// specified format.
        /// </summary>
        ///
        /// <param name="format">Target format.</param>
        /// <param name="io">I/O handler.</param>
        /// <param name="supported">Collection of supported formats.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected JpegConverter(Format format, IO io, IEnumerable<Format> supported) :
            base(format, io, supported) { }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Quality
        ///
        /// <summary>
        /// Gets or sets the value of JPEG quality level.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Quality { get; set; } = 75;

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
            .Concat(new[] { new Argument("JPEGQ", Math.Min(Math.Max(Quality, 1), 100)) });

        /* ----------------------------------------------------------------- */
        ///
        /// GetSupportedFormats
        ///
        /// <summary>
        /// Gets the collection of supported formats.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static IEnumerable<Format> GetSupportedFormats() => new HashSet<Format>
        {
            Format.Jpeg,
            Format.Jpeg24bppRgb,
            Format.Jpeg32bppCmyk,
            Format.Jpeg8bppGrayscale,
        };

        #endregion
    }
}
