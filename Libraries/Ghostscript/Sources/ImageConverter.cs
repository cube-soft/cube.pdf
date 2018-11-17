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
using System.Collections.Generic;
using System.Linq;

namespace Cube.Pdf.Ghostscript
{
    /* --------------------------------------------------------------------- */
    ///
    /// ImageConverter
    ///
    /// <summary>
    /// Provides functionality to convert to raster image format such as
    /// PNG.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ImageConverter : Converter
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ImageConverter
        ///
        /// <summary>
        /// Initializes a new instance of the ImageConverter class with the
        /// specified format.
        /// </summary>
        ///
        /// <param name="format">Target format.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ImageConverter(Format format) : this(format, new IO()) { }

        /* ----------------------------------------------------------------- */
        ///
        /// ImageConverter
        ///
        /// <summary>
        /// Initializes a new instance of the ImageConverter class with the
        /// specified format.
        /// </summary>
        ///
        /// <param name="format">Target format.</param>
        /// <param name="io">I/O handler.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ImageConverter(Format format, IO io) : this(format, io, SupportedFormats) { }

        /* ----------------------------------------------------------------- */
        ///
        /// ImageConverter
        ///
        /// <summary>
        /// Initializes a new instance of the ImageConverter class with the
        /// specified format.
        /// </summary>
        ///
        /// <param name="format">Target format.</param>
        /// <param name="io">I/O handler.</param>
        /// <param name="supported">Collection of supported formats.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected ImageConverter(Format format, IO io, IEnumerable<Format> supported) :
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
        public static new IEnumerable<Format> SupportedFormats { get; } = new HashSet<Format>
        {
            Format.Psd,
            Format.PsdRgb,
            Format.PsdCmyk,
            Format.PsdCmykog,
            Format.Jpeg,
            Format.Jpeg24bppRgb,
            Format.Jpeg32bppCmyk,
            Format.Jpeg8bppGrayscale,
            Format.Png,
            Format.Png24bppRgb,
            Format.Png32bppArgb,
            Format.Png4bppIndexed,
            Format.Png8bppIndexed,
            Format.Png8bppGrayscale,
            Format.Png1bppMonochrome,
            Format.Bmp,
            Format.Bmp24bppRgb,
            Format.Bmp32bppArgb,
            Format.Bmp4bppIndexed,
            Format.Bmp8bppIndexed,
            Format.Bmp8bppGrayscale,
            Format.Bmp1bppMonochrome,
            Format.Tiff,
            Format.Tiff12bppRgb,
            Format.Tiff24bppRgb,
            Format.Tiff48bppRgb,
            Format.Tiff32bppCmyk,
            Format.Tiff64bppCmyk,
            Format.Tiff8bppGrayscale,
            Format.Tiff1bppMonochrome,
        };

        /* ----------------------------------------------------------------- */
        ///
        /// AntiAlias
        ///
        /// <summary>
        /// Gets or sets a value indicating whether anti-aliasing is
        /// enabled.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool AntiAlias { get; set; } = true;

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
            .Concat(CreateAntiAlias());

        /* ----------------------------------------------------------------- */
        ///
        /// CreateAntiAlias
        ///
        /// <summary>
        /// Creates the collection of arguments representing anti-aliasing.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private IEnumerable<Argument> CreateAntiAlias() =>
            AntiAlias ?
            new[]
            {
                new Argument("GraphicsAlphaBits", 4),
                new Argument("TextAlphaBits", 4),
            } :
            new Argument[0];

        #endregion
    }
}
