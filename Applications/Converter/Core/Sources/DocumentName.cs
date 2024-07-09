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
namespace Cube.Pdf.Converter;

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Cube.Collections.Extensions;
using Cube.FileSystem;
using Cube.Text.Extensions;

/* ------------------------------------------------------------------------- */
///
/// DocumentName
///
/// <summary>
/// Provides functionality to convert the provided document name so that
/// it can be used as a filename. This class removes characters that
/// cannot be used as the filename.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public sealed class DocumentName
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// DocumentName
    ///
    /// <summary>
    /// Initializes a new instance of the DocumentName class with the
    /// specified arguments.
    /// </summary>
    ///
    /// <param name="src">Original document name.</param>
    ///
    /* --------------------------------------------------------------------- */
    public DocumentName(string src) : this(src, "CubePDF") { }

    /* --------------------------------------------------------------------- */
    ///
    /// DocumentName
    ///
    /// <summary>
    /// Initializes a new instance of the DocumentName class with the
    /// specified arguments.
    /// </summary>
    ///
    /// <param name="src">Original document name.</param>
    /// <param name="alternate">Default filename.</param>
    ///
    /* --------------------------------------------------------------------- */
    public DocumentName(string src, string alternate)
    {
        _path = new(src)
        {
            AllowCurrentDirectory = false,
            AllowDriveLetter      = false,
            AllowInactivation     = false,
            AllowParentDirectory  = false,
            AllowUnc              = false,
        };

        Value = GetValue(_path, alternate);
    }

    #endregion

    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// Source
    ///
    /// <summary>
    /// Gets the original document name.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string Source => _path.Source;

    /* --------------------------------------------------------------------- */
    ///
    /// Value
    ///
    /// <summary>
    /// Gets a name that can be used as a filename.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string Value { get; }

    #endregion

    #region Implementations

    /* --------------------------------------------------------------------- */
    ///
    /// GetValue
    ///
    /// <summary>
    /// Gets a name that is used as a filename.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private string GetValue(SafePath src, string alternate)
    {
        if (!Source.HasValue()) return alternate;

        var parts = Regex.Split(Io.GetFileName(src.Value), " - ")
                         .Select(e => e.Trim())
                         .Where(e => e.HasValue() && e != "-")
                         .ToList();

        if (parts.Count == 0) return alternate;
        if (parts.Count == 1) return parts.First();

        static string join(IEnumerable<string> e) => e.Join(" - ");
        if (_apps.Contains(parts.First())) return join(parts.Skip(1));
        if (_apps.Contains(parts.Last()))  return join(parts.Take(parts.Count - 1));

        var i = parts.FindIndex(PathExplorer.HasExtension);
        if (i == -1) return join(parts);
        if (i == parts.Count - 1) return join(parts.Skip(1));
        return join(parts.Take(i + 1));
    }

    #endregion

    #region Fields
    private readonly SafePath _path;
    private readonly string[] _apps = [
        "Microsoft Word",
        "Microsoft Excel",
        "Microsoft PowerPoint",
    ];
    #endregion
}
