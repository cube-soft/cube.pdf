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
namespace Cube.Pdf.Ghostscript;

using System.Collections.Generic;
using System.Linq;

/* ------------------------------------------------------------------------- */
///
/// TiffConverter
///
/// <summary>
/// Provides functionality to convert to TIFF image.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public class TiffConverter : ImageConverter
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// TiffConverter
    ///
    /// <summary>
    /// Initializes a new instance of the TiffConverter class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public TiffConverter() : this(Format.Tiff) { }

    /* --------------------------------------------------------------------- */
    ///
    /// TiffConverter
    ///
    /// <summary>
    /// Initializes a new instance of the TiffConverter class with the
    /// specified format.
    /// </summary>
    ///
    /// <param name="format">Target format.</param>
    ///
    /* --------------------------------------------------------------------- */
    public TiffConverter(Format format) : this(format, SupportedFormats) { }

    /* --------------------------------------------------------------------- */
    ///
    /// TiffConverter
    ///
    /// <summary>
    /// Initializes a new instance of the TiffConverter class with the
    /// specified arguments.
    /// </summary>
    ///
    /// <param name="format">Target format.</param>
    /// <param name="supported">Collection of supported formats.</param>
    ///
    /* --------------------------------------------------------------------- */
    protected TiffConverter(Format format, IEnumerable<Format> supported) :
        base(format, supported) { }

    #endregion

    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// SupportedFormats
    ///
    /// <summary>
    /// Gets the collection of supported formats.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static new IEnumerable<Format> SupportedFormats { get; } = new HashSet<Format>
    {
        Format.Tiff,
        Format.Tiff12bppRgb,
        Format.Tiff24bppRgb,
        Format.Tiff48bppRgb,
        Format.Tiff32bppCmyk,
        Format.Tiff64bppCmyk,
        Format.Tiff8bppGrayscale,
        Format.Tiff1bppMonochrome,
    };

    /* --------------------------------------------------------------------- */
    ///
    /// Compression
    ///
    /// <summary>
    /// Gets or sets the compression encoding. The value is valid only if
    /// Format.Tiff1bppMohochrome is specified in the constructor.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public Encoding Compression { get; set; }

    #endregion

    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// OnCreateArguments
    ///
    /// <summary>
    /// Occurs when creating Ghostscript API arguments.
    /// </summary>
    ///
    /// <returns>Collection of arguments.</returns>
    ///
    /* --------------------------------------------------------------------- */
    protected override IEnumerable<Argument> OnCreateArguments() =>
        base.OnCreateArguments()
            .Concat(CreateCompression());

    #endregion

    #region Implementations

    /* --------------------------------------------------------------------- */
    ///
    /// CreateCompression
    ///
    /// <summary>
    /// Creates the argument sequence for compression.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private IEnumerable<Argument> CreateCompression() => Compression switch
    {
        Encoding.G3Fax    => new[] { new Argument('s', "Compression", "g3"  ) },
        Encoding.G4Fax    => new[] { new Argument('s', "Compression", "g4"  ) },
        Encoding.Lzw      => new[] { new Argument('s', "Compression", "lzw" ) },
        Encoding.PackBits => new[] { new Argument('s', "Compression", "pack") },
        _ => Enumerable.Empty<Argument>(),
    };

    #endregion
}
