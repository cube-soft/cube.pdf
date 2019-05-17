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
using Cube.Mixin.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Cube.Pdf.Ghostscript
{
    /* --------------------------------------------------------------------- */
    ///
    /// DocumentConverter
    ///
    /// <summary>
    /// Provides functionality to convert to document format such as PDF.
    /// </summary>
    ///
    /// <see href="https://www.ghostscript.com/doc/9.25/VectorDevices.htm" />
    ///
    /* --------------------------------------------------------------------- */
    public class DocumentConverter : Converter
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// DocumentConverter
        ///
        /// <summary>
        /// Initializes a new instance of the DocumentConverter class with
        /// the specified format.
        /// </summary>
        ///
        /// <param name="format">Target format.</param>
        ///
        /* ----------------------------------------------------------------- */
        public DocumentConverter(Format format) : this(format, new IO()) { }

        /* ----------------------------------------------------------------- */
        ///
        /// DocumentConverter
        ///
        /// <summary>
        /// Initializes a new instance of the DocumentConverter class with
        /// the specified parameters.
        /// </summary>
        ///
        /// <param name="format">Target format.</param>
        /// <param name="io">I/O handler.</param>
        ///
        /* ----------------------------------------------------------------- */
        public DocumentConverter(Format format, IO io) : this(format, io, SupportedFormats) { }

        /* ----------------------------------------------------------------- */
        ///
        /// DocumentConverter
        ///
        /// <summary>
        /// Initializes a new instance of the DocumentConverter class with
        /// the specified parameters.
        /// </summary>
        ///
        /// <param name="format">Target format.</param>
        /// <param name="io">I/O handler.</param>
        /// <param name="supported">Collection of supported formats.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected DocumentConverter(Format format, IO io, IEnumerable<Format> supported) :
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
        public static new IEnumerable<Format> SupportedFormats { get; } =
            new HashSet<Format> { Format.Ps, Format.Eps, Format.Pdf };

        /* ----------------------------------------------------------------- */
        ///
        /// EmbedFonts
        ///
        /// <summary>
        /// Gets or sets a value indicating whether all used fonts are
        /// embedded in the converted document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool EmbedFonts { get; set; } = true;

        /* ----------------------------------------------------------------- */
        ///
        /// ColorMode
        ///
        /// <summary>
        /// Gets or sets the color mode.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ColorMode ColorMode { get; set; } = ColorMode.SameAsSource;

        /* ----------------------------------------------------------------- */
        ///
        /// Downsampling
        ///
        /// <summary>
        /// Gets or sets the downsampling method of embedded images.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Downsampling Downsampling { get; set; } = Downsampling.None;

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
            .Concat(new[] { ColorMode.GetArgument() })
            .Concat(CreateFontArguments())
            .Concat(CreateImageArguments());

        /* ----------------------------------------------------------------- */
        ///
        /// OnCreateCodes
        ///
        /// <summary>
        /// Occurs when creating code to be executed with the Ghostscript
        /// API.
        /// </summary>
        ///
        /// <returns>Collection of arguments.</returns>
        ///
        /* ----------------------------------------------------------------- */
        protected override IEnumerable<Code> OnCreateCodes() =>
            base.OnCreateCodes()
            .Concat(new[] { CreateEmbedFontsCode() }.Compact());

        /* ----------------------------------------------------------------- */
        ///
        /// CreateFontArguments
        ///
        /// <summary>
        /// Creates a new instance of the Argument class representing
        /// information related to the fonts.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private IEnumerable<Argument> CreateFontArguments()
        {
            var dest = new List<Argument> { new Argument("EmbedAllFonts", EmbedFonts) };
            if (EmbedFonts) dest.Add(new Argument("SubsetFonts", true));
            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CreateImageArguments
        ///
        /// <summary>
        /// Creates the collection of arguments representing information
        /// related to the images.
        /// </summary>
        ///
        /// <remarks>
        /// DownsampleXxxImages を false に設定すると Resolution 等の
        /// 設定も無視されるため、Downsampling の内容に関わらず true に
        /// 設定します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private IEnumerable<Argument> CreateImageArguments() => new[]
        {
            new Argument("ColorImageResolution",  Resolution),
            new Argument("GrayImageResolution",   Resolution),
            new Argument("MonoImageResolution",   GetMonoResolution()),
            new Argument("DownsampleColorImages", true),
            new Argument("DownsampleGrayImages",  true),
            new Argument("DownsampleMonoImages",  true),
            Downsampling.GetArgument("ColorImageDownsampleType"),
            Downsampling.GetArgument("GrayImageDownsampleType"),
            Downsampling.GetArgument("MonoImageDownsampleType"),
        }.Compact();

        /* ----------------------------------------------------------------- */
        ///
        /// CreateEmbedFontsCode
        ///
        /// <summary>
        /// Creates the code representing related to the fonts.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private Code CreateEmbedFontsCode() =>
            EmbedFonts ?
            new Code(".setpdfwrite <</NeverEmbed [ ]>> setdistillerparams") :
            null;

        /* ----------------------------------------------------------------- */
        ///
        /// GetMonoResolution
        ///
        /// <summary>
        /// Gets the resolution of monochrome images.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private int GetMonoResolution() =>
            Resolution <  300 ?  300 :
            Resolution < 1200 ? 1200 :
            Resolution;

        #endregion
    }
}
