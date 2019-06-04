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
using Cube.Mixin.Logging;
using Cube.Mixin.String;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cube.Pdf.Ghostscript
{
    /* --------------------------------------------------------------------- */
    ///
    /// Converter
    ///
    /// <summary>
    /// Represents the base class that communicates with the Ghostscript API.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class Converter
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
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
        /* ----------------------------------------------------------------- */
        public Converter(Format format) : this(format, new IO()) { }

        /* ----------------------------------------------------------------- */
        ///
        /// Converter
        ///
        /// <summary>
        /// Initializes a new instance of the Converter class with the
        /// specified format.
        /// </summary>
        ///
        /// <param name="format">Target format.</param>
        /// <param name="io">I/O handler.</param>
        ///
        /* ----------------------------------------------------------------- */
        public Converter(Format format, IO io) : this(format, io, SupportedFormats) { }

        /* ----------------------------------------------------------------- */
        ///
        /// Converter
        ///
        /// <summary>
        /// Initializes a new instance of the Converter class with the
        /// specified format.
        /// </summary>
        ///
        /// <param name="format">Target format.</param>
        /// <param name="io">I/O handler.</param>
        /// <param name="supported">Collection of supported formats.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected Converter(Format format, IO io, IEnumerable<Format> supported)
        {
            if (!supported.Contains(format)) throw new NotSupportedException(format.ToString());

            IO = io;
            Format = format;
            Fonts.Add(@"C:\Windows\Fonts");
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Revision
        ///
        /// <summary>
        /// Gets the revision number of Ghostscript.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static int Revision => GsApi.Information.Revision;

        /* ----------------------------------------------------------------- */
        ///
        /// SupportedFormats
        ///
        /// <summary>
        /// Gets the collection of supported formats.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static IEnumerable<Format> SupportedFormats { get; } =
            new HashSet<Format>(Enum.GetValues(typeof(Format)).Cast<Format>());

        /* ----------------------------------------------------------------- */
        ///
        /// Format
        ///
        /// <summary>
        /// Gets the target format.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Format Format { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// IO
        ///
        /// <summary>
        /// Gets the I/O handler.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IO IO { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Paper
        ///
        /// <summary>
        /// Gets or sets the paper size.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Paper Paper { get; set; } = Paper.Auto;

        /* ----------------------------------------------------------------- */
        ///
        /// Orientation
        ///
        /// <summary>
        /// Gets or sets the orientation of the page.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Orientation Orientation { get; set; } = Orientation.Auto;

        /* ----------------------------------------------------------------- */
        ///
        /// Resolution
        ///
        /// <summary>
        /// Gets or sets the resolution of images.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Resolution { get; set; } = 600;

        /* ----------------------------------------------------------------- */
        ///
        /// Quiet
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to suppress some
        /// messages.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Quiet { get; set; } = true;

        /* ----------------------------------------------------------------- */
        ///
        /// Log
        ///
        /// <summary>
        /// Gets or sets the path to store log of Ghostscript API.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Log { get; set; } = string.Empty;

        /* ----------------------------------------------------------------- */
        ///
        /// Temp
        ///
        /// <summary>
        /// Gets or sets the path of the working directory.
        /// </summary>
        ///
        /// <remarks>
        /// このプロパティに値を設定した場合、変換処理の際に一時的に
        /// TEMP 環境変数が変更されます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public string Temp { get; set; } = string.Empty;

        /* ----------------------------------------------------------------- */
        ///
        /// Resources
        ///
        /// <summary>
        /// Gets the collection of resource directories.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ICollection<string> Resources { get; } = new List<string>();

        /* ----------------------------------------------------------------- */
        ///
        /// Fonts
        ///
        /// <summary>
        /// Gets the collection of font directories.
        /// </summary>
        ///
        /// <remarks>
        /// 初期値として C:\Windows\Fonts に相当するパスが設定されます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public ICollection<string> Fonts { get; } = new List<string>();

        /* ----------------------------------------------------------------- */
        ///
        /// Options
        ///
        /// <summary>
        /// Gets the collection of optional arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ICollection<Argument> Options { get; } = new List<Argument>();

        /* ----------------------------------------------------------------- */
        ///
        /// Codes
        ///
        /// <summary>
        /// Gets the collection of optional codes.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ICollection<Code> Codes { get; } = new List<Code>();

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
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
        /* ----------------------------------------------------------------- */
        public void Invoke(string src, string dest) => Invoke(new[] { src }, dest);

        /* ----------------------------------------------------------------- */
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
        /* ----------------------------------------------------------------- */
        public void Invoke(IEnumerable<string> sources, string dest) =>
            Invoke(() => GsApi.Invoke(Create()
                .Concat(new Argument('s', "OutputFile", dest))
                .Concat(OnCreateArguments())
                .Concat(CreateCodes())
                .Concat(new Argument('f'))
                .Select(e => e.ToString())
                .Concat(sources)
                .Where(e => { this.LogDebug(e); return true; }) // for debug
                .ToArray()
            , Temp, IO), dest);

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
        .Compact();

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
        protected virtual IEnumerable<Code> OnCreateCodes() =>
            new[] { Orientation.GetCode() }.Compact();

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Creates the collection of Ghostscript arguments.
        /// </summary>
        ///
        /// <remarks>
        /// Ghostscript API は最初の引数を無視するため、引数の先頭に
        /// ダミーオブジェクトを配置します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private IEnumerable<Argument> Create() { yield return Argument.Dummy; }

        /* ----------------------------------------------------------------- */
        ///
        /// CreateCodes
        ///
        /// <summary>
        /// Creates the collection of code to be executed with the
        /// Ghostscript API.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private IEnumerable<Argument> CreateCodes()
        {
            var dest = OnCreateCodes();
            return dest.Count() > 0 || Codes.Count > 0 ?
                   new[] { new Argument('c') }.Concat(dest.OfType<Argument>()).Concat(Codes.OfType<Argument>()) :
                   dest.OfType<Argument>();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CreateResources
        ///
        /// <summary>
        /// Creates a new instance of the Argument class representing
        /// the resource directories.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private Argument CreateResources() =>
            Resources.Count > 0 ?
            new Argument('I', string.Empty, string.Join(";", Resources.ToArray())) :
            null;

        /* ----------------------------------------------------------------- */
        ///
        /// CreateFonts
        ///
        /// <summary>
        /// Creates a new instance of the Argument class representing
        /// the font directories.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private Argument CreateFonts() =>
            Fonts.Count > 0 ?
            new Argument('s', "FONTPATH", string.Join(";", Fonts.ToArray())) :
            null;

        /* ----------------------------------------------------------------- */
        ///
        /// CreateLog
        ///
        /// <summary>
        /// Creates a new instance of the Argument class representing
        /// the path of the log file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private Argument CreateLog() =>
            Log.HasValue() ? new Argument('s', "stdout", Log) : null;

        /* ----------------------------------------------------------------- */
        ///
        /// CreateQuiet
        ///
        /// <summary>
        /// Creates a new instance of the Argument class representing
        /// the quiet mode.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private Argument CreateQuiet() =>
            Quiet ? new Argument('d', "QUIET") : null;

        /* ----------------------------------------------------------------- */
        ///
        /// CreateResolution
        ///
        /// <summary>
        /// Creates a new instance of the Argument class representing the
        /// resolution.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private Argument CreateResolution() => new Argument('r', Resolution);

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Sets the working directory and invokes the specified action.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Invoke(Action action, string dest)
        {
            var info = IO.Get(dest);
            if (!IO.Exists(info.DirectoryName)) IO.CreateDirectory(info.DirectoryName);
            action();
        }

        #endregion
    }
}
