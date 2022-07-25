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
using Cube.FileSystem;
using Cube.Mixin.Collections;
using Cube.Mixin.String;

/* ------------------------------------------------------------------------- */
///
/// Converter
///
/// <summary>
/// Represents the base class that communicates with the Ghostscript API.
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
        Fonts.Add(Environment.GetFolderPath(Environment.SpecialFolder.Fonts));
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
    /// Executes to convert.
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
    /// Executes to convert.
    /// </summary>
    ///
    /// <param name="sources">Collection of source files.</param>
    /// <param name="dest">Path to save the conversion result.</param>
    ///
    /* --------------------------------------------------------------------- */
    public void Invoke(IEnumerable<string> sources, string dest) =>
        Invoke(() => GsApi.Invoke(Create()
            .Concat(new Argument('s', "OutputFile", dest))
            .Concat(OnCreateArguments())
            .Concat(CreateCodes())
            .Concat(new Argument('f'))
            .Select(e => e.ToString())
            .Concat(sources)
            .Where(e => { GetType().LogDebug(e); return true; }) // for debug
            .ToArray()
        , Temp), dest);

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
    protected virtual IEnumerable<Argument> OnCreateArguments() => new []
    {
        Format.GetArgument(),
        new Argument('d', "SAFER"),
        new Argument('d', "BATCH"),
        new Argument('d', "NOPAUSE"),
        CreateQuiet(),
        CreateLog(),
        CreateResources(),
        CreateFonts(),
        CreateResolution(),
        Paper.GetArgument(),
        Orientation.GetArgument(),
    }
    .Concat(Options)
    .OfType<Argument>();

    /* --------------------------------------------------------------------- */
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
    /* --------------------------------------------------------------------- */
    protected virtual IEnumerable<Code> OnCreateCodes() =>
        new[] { Orientation.GetCode() }.OfType<Code>();

    #endregion

    #region Implementations

    /* --------------------------------------------------------------------- */
    ///
    /// Create
    ///
    /// <summary>
    /// Creates the collection of Ghostscript arguments.
    /// </summary>
    ///
    /// <remarks>
    /// The Ghostscript API ignores the first argument, so it places
    /// a dummy object at the beginning of the argument.
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    private IEnumerable<Argument> Create() { yield return Argument.Dummy; }

    /* --------------------------------------------------------------------- */
    ///
    /// CreateCodes
    ///
    /// <summary>
    /// Creates the collection of code to be executed with the
    /// Ghostscript API.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private IEnumerable<Argument> CreateCodes()
    {
        var dest = OnCreateCodes();
        return dest.Count() > 0 || Codes.Count > 0 ?
               new[] { new Argument('c') }.Concat(dest).Concat(Codes) :
               dest;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// CreateResources
    ///
    /// <summary>
    /// Creates a new instance of the Argument class representing
    /// the resource directories.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private Argument CreateResources() =>
        Resources.Count > 0 ? new('I', string.Empty, string.Join(";", Resources)) : null;

    /* --------------------------------------------------------------------- */
    ///
    /// CreateFonts
    ///
    /// <summary>
    /// Creates a new instance of the Argument class representing
    /// the font directories.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private Argument CreateFonts() =>
        Fonts.Count > 0 ? new('s', "FONTPATH", string.Join(";", Fonts)) : default;

    /* --------------------------------------------------------------------- */
    ///
    /// CreateLog
    ///
    /// <summary>
    /// Creates a new instance of the Argument class representing
    /// the path of the log file.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private Argument CreateLog() => Log.HasValue() ? new('s', "stdout", Log) : default;

    /* --------------------------------------------------------------------- */
    ///
    /// CreateQuiet
    ///
    /// <summary>
    /// Creates a new instance of the Argument class representing
    /// the quiet mode.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private Argument CreateQuiet() => Quiet ? new('d', "QUIET") : default;

    /* --------------------------------------------------------------------- */
    ///
    /// CreateResolution
    ///
    /// <summary>
    /// Creates a new instance of the Argument class representing the
    /// resolution.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private Argument CreateResolution() => new('r', Resolution);

    /* --------------------------------------------------------------------- */
    ///
    /// Invoke
    ///
    /// <summary>
    /// Sets the working directory and invokes the specified action.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private void Invoke(Action action, string dest)
    {
        Io.CreateDirectory(Io.Get(dest).DirectoryName);
        action();
    }

    #endregion
}
