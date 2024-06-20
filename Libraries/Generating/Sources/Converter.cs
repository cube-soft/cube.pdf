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

using System;
using System.Collections.Generic;
using System.Linq;
using Cube.Collections.Extensions;
using Cube.FileSystem;
using Cube.Text.Extensions;

/* ------------------------------------------------------------------------- */
///
/// Converter
///
/// <summary>
/// Provides functionality to convert files via Ghostscript API.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public class Converter
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// Converter
    ///
    /// <summary>
    /// Initializes a new instance of the Converter class with the
    /// specified format.
    /// </summary>
    ///
    /// <param name="format">Target format.</param>
    ///
    /* --------------------------------------------------------------------- */
    public Converter(Format format) : this(format, SupportedFormats) { }

    /* --------------------------------------------------------------------- */
    ///
    /// Converter
    ///
    /// <summary>
    /// Initializes a new instance of the Converter class with the
    /// specified format.
    /// </summary>
    ///
    /// <param name="format">Target format.</param>
    /// <param name="supported">Collection of supported formats.</param>
    ///
    /* --------------------------------------------------------------------- */
    protected Converter(Format format, IEnumerable<Format> supported)
    {
        if (!supported.Contains(format)) throw new NotSupportedException(format.ToString());

        Format = format;
        Fonts.Add(@"C:\Windows\Fonts");
    }

    #endregion

    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// Revision
    ///
    /// <summary>
    /// Gets the revision number of Ghostscript.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static int Revision => GsApi.Information.Revision;

    /* --------------------------------------------------------------------- */
    ///
    /// SupportedFormats
    ///
    /// <summary>
    /// Gets the collection of supported formats.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static IEnumerable<Format> SupportedFormats { get; } =
        new HashSet<Format>(Enum.GetValues(typeof(Format)).Cast<Format>());

    /* --------------------------------------------------------------------- */
    ///
    /// Format
    ///
    /// <summary>
    /// Gets the target format.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public Format Format { get; }

    /* --------------------------------------------------------------------- */
    ///
    /// Paper
    ///
    /// <summary>
    /// Gets or sets the paper size.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public Paper Paper { get; set; } = Paper.Auto;

    /* --------------------------------------------------------------------- */
    ///
    /// Orientation
    ///
    /// <summary>
    /// Gets or sets the orientation of the page.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public Orientation Orientation { get; set; } = Orientation.Auto;

    /* --------------------------------------------------------------------- */
    ///
    /// Resolution
    ///
    /// <summary>
    /// Gets or sets the resolution of images.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public int Resolution { get; set; } = 600;

    /* --------------------------------------------------------------------- */
    ///
    /// Quiet
    ///
    /// <summary>
    /// Gets or sets a value indicating whether to suppress some
    /// messages.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool Quiet { get; set; } = true;

    /* --------------------------------------------------------------------- */
    ///
    /// Log
    ///
    /// <summary>
    /// Gets or sets the path to store log of Ghostscript API.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string Log { get; set; } = string.Empty;

    /* --------------------------------------------------------------------- */
    ///
    /// Temp
    ///
    /// <summary>
    /// Gets or sets the path of the working directory.
    /// </summary>
    ///
    /// <remarks>
    /// If you set a value for this property, the TEMP environment
    /// variable will be temporarily changed during the conversion.
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public string Temp { get; set; } = string.Empty;

    /* --------------------------------------------------------------------- */
    ///
    /// Resources
    ///
    /// <summary>
    /// Gets the collection of resource directories.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public ICollection<string> Resources { get; } = new List<string>();

    /* --------------------------------------------------------------------- */
    ///
    /// Fonts
    ///
    /// <summary>
    /// Gets the collection of font directories.
    /// </summary>
    ///
    /// <remarks>
    /// The path corresponding to C:\Windows\Fonts will be set as the
    /// default value.
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public ICollection<string> Fonts { get; } = new List<string>();

    /* --------------------------------------------------------------------- */
    ///
    /// Options
    ///
    /// <summary>
    /// Gets the collection of optional arguments.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public ICollection<Argument> Options { get; } = new List<Argument>();

    /* --------------------------------------------------------------------- */
    ///
    /// Codes
    ///
    /// <summary>
    /// Gets the collection of optional codes.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public ICollection<Code> Codes { get; } = new List<Code>();

    #endregion

    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// Invoke
    ///
    /// <summary>
    /// Invokes the conversion.
    /// </summary>
    ///
    /// <param name="src">Source file.</param>
    /// <param name="dest">Path to save the conversion result.</param>
    ///
    /* --------------------------------------------------------------------- */
    public void Invoke(string src, string dest) => Invoke(new[] { src }, dest);

    /* --------------------------------------------------------------------- */
    ///
    /// Invoke
    ///
    /// <summary>
    /// Invokes the conversion.
    /// </summary>
    ///
    /// <param name="sources">Collection of source files.</param>
    /// <param name="dest">Path to save the conversion result.</param>
    ///
    /* --------------------------------------------------------------------- */
    public void Invoke(IEnumerable<string> sources, string dest)
    {
        // OfType<T> methods are used to remove null objects.
        var args = new[] { Argument.Dummy }
            .Concat(new Argument('s', "OutputFile", dest))
            .Concat(OnCreateArguments().OfType<Argument>())
            .Concat(Adjust(OnCreateCodes().OfType<Code>()))
            .Concat(new Argument('f'))
            .Select(e => e.ToString())
            .Concat(sources)
            .ToArray();

        Logger.Debug(args.Join(" "));
        Io.CreateDirectory(Io.GetDirectoryName(dest));
        GsApi.Invoke(args, Temp);
    }

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
    protected virtual IEnumerable<Argument> OnCreateArguments() =>
        CreateBasicArgumetns().Concat(Options);

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
    protected virtual IEnumerable<Code> OnCreateCodes() =>
        CreateBasicCodes().Concat(Codes);

    #endregion

    #region Implementations

    /* --------------------------------------------------------------------- */
    ///
    /// CreateBasicArgumetns
    ///
    /// <summary>
    /// Creates a collection of Argument objects.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private IEnumerable<Argument> CreateBasicArgumetns()
    {
        yield return new('s', "DEVICE", GsDevice.From(Format));
        yield return new('d', "SAFER");
        yield return new('d', "BATCH");
        yield return new('d', "NOPAUSE");
        if (Quiet) yield return new('d', "QUIET");
        if (Log.HasValue()) yield return new('s', "stdout", Log);
        if (Resources.Count > 0) yield return new('I', "", Resources.Join(";"));
        if (Fonts.Count > 0) yield return new('s', "FONTPATH", Fonts.Join(";"));
        yield return new('r', Resolution);
        if (Paper != Paper.Auto) yield return new('s', "PAPERSIZE", Paper.ToString().ToLowerInvariant());
        yield return new("AutoRotatePages", Orientation == Orientation.Auto ? "PageByPage" : "None");
    }

    /* --------------------------------------------------------------------- */
    ///
    /// CreateBasicCodes
    ///
    /// <summary>
    /// Creates a collection of Code objects.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private IEnumerable<Code> CreateBasicCodes()
    {
        if (Orientation != Orientation.Auto)
        {
            yield return new($"<</Orientation {Orientation:d}>> setpagedevice");
        }
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Adjust
    ///
    /// <summary>
    /// Adjusts the specified Code collection so that it can be concatenated
    /// as Ghostscript arguments.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private IEnumerable<Argument> Adjust(IEnumerable<Code> src) =>
        src.Count() > 0 ? new[] { new Argument('c') }.Concat(src.OfType<Argument>()) : src.OfType<Argument>();

    #endregion
}
