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
using Cube.Mixin.Collections;

/* ------------------------------------------------------------------------- */
///
/// PdfConverter
///
/// <summary>
/// Provides functionality to convert to PDF format.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public class PdfConverter : DocumentConverter
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// PdfConverter
    ///
    /// <summary>
    /// Initializes a new instance of the PdfConverter class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public PdfConverter() : this(Format.Pdf, SupportedFormats) { }

    /* --------------------------------------------------------------------- */
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
    /* --------------------------------------------------------------------- */
    protected PdfConverter(Format format, IEnumerable<Format> supported) :
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
    public static new IEnumerable<Format> SupportedFormats { get; } = new[] { Format.Pdf };

    /* --------------------------------------------------------------------- */
    ///
    /// Version
    ///
    /// <summary>
    /// Gets or sets the version number of the converted document.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public PdfVersion Version { get; set; } = new PdfVersion(1, 7);

    /* --------------------------------------------------------------------- */
    ///
    /// Compression
    ///
    /// <summary>
    /// Gets or sets the compression encoding of embedded color or gray
    /// images.
    /// </summary>
    ///
    /// <remarks>
    /// Supported compressions are Flate, Lzw, and Jpeg.
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public Encoding Compression { get; set; } = Encoding.Flate;

    /* --------------------------------------------------------------------- */
    ///
    /// MonoCompression
    ///
    /// <summary>
    /// Gets or sets the compression encoding of embedded monochrome images.
    /// </summary>
    ///
    /// <remarks>
    /// Supported compressions are Flate, Lzw, and Fax.
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public Encoding MonoCompression { get; set; } = Encoding.G4Fax;

    /* --------------------------------------------------------------------- */
    ///
    /// Linearization
    ///
    /// <summary>
    /// Gets or sets a value indicating whether to enable linearization
    /// (a.k.a PDF Web optimization).
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool Linearization { get; set; } = false;

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
    /// <returns>Collection of Argument objects.</returns>
    ///
    /* --------------------------------------------------------------------- */
    protected override IEnumerable<Argument> OnCreateArguments() => base.OnCreateArguments()
        .Concat(CreateImages("Color", Compression))
        .Concat(CreateImages("Gray",  Compression))
        .Concat(CreateImages("Mono",  MonoCompression))
        .Concat(new Argument('d', "CompatibilityLevel", $"{Version.Major}.{Version.Minor}"))
        .Concat(Linearization ? new Argument('d', "FastWebView") : default);

    #endregion

    #region Implementations

    /* --------------------------------------------------------------------- */
    ///
    /// CreateImages
    ///
    /// <summary>
    /// Creates a collection of Argument objects for embedded images.
    /// </summary>
    ///
    /// <param name="kind">Color, Gray, or Mono.</param>
    /// <param name="value">Compression encoding.</param>
    /// 
    /// <returns>Collection of Argument objects.</returns>
    ///
    /* --------------------------------------------------------------------- */
    private IEnumerable<Argument> CreateImages(string kind, Encoding value) => new[]
    {
        new($"Encode{kind}Images", value != Encoding.None),
        new($"AutoFilter{kind}Images", false),
        CreateCompression($"{kind}ImageFilter", value),
    };

    /* --------------------------------------------------------------------- */
    ///
    /// CreateCompression
    ///
    /// <summary>
    /// Creates an Argument object for the compression settings.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private Argument CreateCompression(string name, Encoding value) => value switch
    {
        Encoding.Flate  => new(name, "FlateEncode"),
        Encoding.Lzw    => new(name, "LZWEncode"),
        Encoding.Jpeg   => new(name, "DCTEncode"),
        Encoding.G3Fax  => new(name, "CCITTFaxEncode"),
        Encoding.G4Fax  => new(name, "CCITTFaxEncode"),
        Encoding.Base85 => new(name, "ASCII85Encode"),
        _ => default,
    };

    #endregion
}
