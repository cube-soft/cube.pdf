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
/// DocumentConverter
///
/// <summary>
/// Provides functionality to convert to document format such as PDF.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public class DocumentConverter : Converter
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// DocumentConverter
    ///
    /// <summary>
    /// Initializes a new instance of the DocumentConverter class with
    /// the specified parameters.
    /// </summary>
    ///
    /// <param name="format">Target format.</param>
    ///
    /* --------------------------------------------------------------------- */
    public DocumentConverter(Format format) : this(format, SupportedFormats) { }

    /* --------------------------------------------------------------------- */
    ///
    /// DocumentConverter
    ///
    /// <summary>
    /// Initializes a new instance of the DocumentConverter class with
    /// the specified parameters.
    /// </summary>
    ///
    /// <param name="format">Target format.</param>
    /// <param name="supported">Collection of supported formats.</param>
    ///
    /* --------------------------------------------------------------------- */
    protected DocumentConverter(Format format, IEnumerable<Format> supported) :
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
    public static new IEnumerable<Format> SupportedFormats { get; } =
        new HashSet<Format> { Format.Ps, Format.Eps, Format.Pdf };

    /* --------------------------------------------------------------------- */
    ///
    /// EmbedFonts
    ///
    /// <summary>
    /// Gets or sets a value indicating whether all used fonts are
    /// embedded in the converted document.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool EmbedFonts { get; set; } = true;

    /* --------------------------------------------------------------------- */
    ///
    /// ColorMode
    ///
    /// <summary>
    /// Gets or sets the color mode.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public ColorMode ColorMode { get; set; } = ColorMode.SameAsSource;

    /* --------------------------------------------------------------------- */
    ///
    /// Downsampling
    ///
    /// <summary>
    /// Gets or sets the downsampling method of embedded images.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public Downsampling Downsampling { get; set; } = Downsampling.None;

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
        .Concat(CreateColorMode("ColorConversionStrategy"))
        .Concat(CreateFonts())
        .Concat(CreateImages());

    /* --------------------------------------------------------------------- */
    ///
    /// OnCreateCodes
    ///
    /// <summary>
    /// Occurs when creating a collection of Code objects via Ghostscript
    /// API.
    /// </summary>
    ///
    /// <returns>Collection of Code objects.</returns>
    ///
    /* --------------------------------------------------------------------- */
    protected override IEnumerable<Code> OnCreateCodes() =>
        base.OnCreateCodes().Concat(CreateFontCodes());

    #endregion

    #region Implementations

    /* --------------------------------------------------------------------- */
    ///
    /// CreateColorMode
    ///
    /// <summary>
    /// Creates an Argument object for color mode.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private Argument CreateColorMode(string key) => ColorMode switch
    {
        ColorMode.Rgb               => new(key, "RGB"),
        ColorMode.Cmyk              => new(key, "CMYK"),
        ColorMode.Grayscale         => new(key, "Gray"),
        ColorMode.Monochrome        => new(key, "Gray"), // Monochrome currently not supported
        ColorMode.SameAsSource      => new(key, "LeaveColorUnchanged"),
        ColorMode.DeviceIndependent => new(key, "UseDeviceIndependentColor"),
        _ => default,
    };

    /* --------------------------------------------------------------------- */
    ///
    /// CreateDownsampling
    ///
    /// <summary>
    /// Creates an Argument object for downsampling.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private Argument CreateDownsampling(string key) =>
        Downsampling != Downsampling.None ? new(key, Downsampling.ToString()) : default;


    /* --------------------------------------------------------------------- */
    ///
    /// CreateFonts
    ///
    /// <summary>
    /// Creates a collection of Argument objects for fonts.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private IEnumerable<Argument> CreateFonts()
    {
        yield return new("EmbedAllFonts", EmbedFonts);
        if (EmbedFonts) yield return new("SubsetFonts", true);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// CreateImages
    ///
    /// <summary>
    /// Creates a collection of Argument objects for embedded images.
    /// </summary>
    ///
    /// <remarks>
    /// Set DownsampleXxxImages to true regardless of the contents of
    /// Downsampling, because if DownsampleXxxImages is set to false,
    /// the Resolution and other settings are also ignored.
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    private IEnumerable<Argument> CreateImages() => new[]
    {
        new("ColorImageResolution",  Resolution),
        new("GrayImageResolution",   Resolution),
        new("MonoImageResolution",   Resolution),
        new("DownsampleColorImages", true),
        new("DownsampleGrayImages",  true),
        new("DownsampleMonoImages",  true),
        CreateDownsampling("ColorImageDownsampleType"),
        CreateDownsampling("GrayImageDownsampleType"),
        CreateDownsampling("MonoImageDownsampleType"),
    };

    /* --------------------------------------------------------------------- */
    ///
    /// CreateFontCodes
    ///
    /// <summary>
    /// Creates a collection of Code objects for embedded fonts.
    /// </summary>
    ///
    /// <remarks>
    /// TODO: 3000000 setvmthreshold は旧来の .setpdfwrite だったもの。
    /// これは -c 以降で 1 度だけ記述すれば良いと予想されるので、要検討。
    /// 併せて setdistillerparams の意味も要調査。
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    private IEnumerable<Code> CreateFontCodes()
    {
        if (EmbedFonts)
        {
            yield return new("3000000 setvmthreshold");
            yield return new("<</NeverEmbed [ ]>> setdistillerparams");
        }
    }

    #endregion
}
