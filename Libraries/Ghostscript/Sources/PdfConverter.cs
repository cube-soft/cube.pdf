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
using System.Collections.Generic;
using System.Linq;
using Cube.Mixin.Collections;

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
        public PdfConverter() : this(Format.Pdf, SupportedFormats) { }

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
        /// <param name="supported">Collection of supported formats.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected PdfConverter(Format format, IEnumerable<Format> supported) :
            base(format, supported) { }

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
        public PdfVersion Version { get; set; } = new PdfVersion(1, 7);

        /* ----------------------------------------------------------------- */
        ///
        /// Compression
        ///
        /// <summary>
        /// Gets or sets the compression method of embedded color or gray
        /// images.
        /// </summary>
        ///
        /// <remarks>
        /// Supported compressions are Flate, Lzw, and Jpeg.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public Encoding Compression { get; set; } = Encoding.Flate;

        /* ----------------------------------------------------------------- */
        ///
        /// MonoCompression
        ///
        /// <summary>
        /// Gets or sets the compression method of embedded mono images.
        /// </summary>
        ///
        /// <remarks>
        /// Supported compressions are Flate, Lzw, and Fax.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public Encoding MonoCompression { get; set; } = Encoding.Fax;

        /* ----------------------------------------------------------------- */
        ///
        /// Linearization
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to enable linearization
        /// (a.k.a PDF Web optimization).
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Linearization { get; set; } = false;

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
            .Concat(CreateImageArguments("Color", Compression))
            .Concat(CreateImageArguments("Gray",  Compression))
            .Concat(CreateImageArguments("Mono",  MonoCompression))
            .Concat(new[] { CreateVersion(), CreateFastWebView(), CreateNewPdf() })
            .Compact();

        /* ----------------------------------------------------------------- */
        ///
        /// CreateImageArguments
        ///
        /// <summary>
        /// Creates the collection of arguments representing information
        /// related to the images.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private IEnumerable<Argument> CreateImageArguments(string key, Encoding value) => new[]
        {
            new Argument($"Encode{key}Images", value != Encoding.None),
            new Argument($"AutoFilter{key}Images", false),
            value.GetArgument($"{key}ImageFilter"),
        };

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
            new('d', "CompatibilityLevel", $"{Version.Major}.{Version.Minor}");

        /* ----------------------------------------------------------------- */
        ///
        /// CreateFastWebView
        ///
        /// <summary>
        /// Creates a new instance of the Argument class representing
        /// the Linearized option.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private Argument CreateFastWebView() =>
            Linearization ? new('d', "FastWebView") : default;

        /* ----------------------------------------------------------------- */
        ///
        /// CreateNewPdf
        ///
        /// <summary>
        /// Creates a new instance of the Argument class to use the New PDF
        /// implementation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private Argument CreateNewPdf() =>
            GsApi.Information.Revision == 9550 ? new('d', "NEWPDF") : default;

        #endregion
    }
}
