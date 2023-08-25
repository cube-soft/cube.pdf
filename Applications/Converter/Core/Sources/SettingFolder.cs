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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cube.Collections;
using Cube.DataContract;
using Cube.FileSystem;
using Cube.Reflection.Extensions;
using Cube.Text.Extensions;

/* ------------------------------------------------------------------------- */
///
/// SettingFolder
///
/// <summary>
/// Represents the application and/or user settings.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public class SettingFolder : SettingFolder<SettingValue>
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// SettingFolder
    ///
    /// <summary>
    /// Initializes a new instance of the SettingFolder class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public SettingFolder() : this(Assembly.GetCallingAssembly()) { }

    /* --------------------------------------------------------------------- */
    ///
    /// SettingFolder
    ///
    /// <summary>
    /// Initializes a new instance of the SettingFolder class with the
    /// specified assembly.
    /// </summary>
    ///
    /// <param name="assembly">Assembly object.</param>
    ///
    /* --------------------------------------------------------------------- */
    public SettingFolder(Assembly assembly) : this(assembly, Format.Registry, @"CubeSoft\CubePDF\v3") { }

    /* --------------------------------------------------------------------- */
    ///
    /// SettingFolder
    ///
    /// <summary>
    /// Initializes a new instance of the SettingFolder class with the
    /// specified arguments.
    /// </summary>
    ///
    /// <param name="format">Serialization format.</param>
    /// <param name="location">Location to save settings.</param>
    ///
    /* --------------------------------------------------------------------- */
    public SettingFolder(Format format, string location) :
        this(Assembly.GetCallingAssembly(), format, location) { }

    /* --------------------------------------------------------------------- */
    ///
    /// SettingFolder
    ///
    /// <summary>
    /// Initializes a new instance of the SettingFolder class with the
    /// specified arguments.
    /// </summary>
    ///
    /// <param name="assembly">Assembly object.</param>
    /// <param name="format">Serialization format.</param>
    /// <param name="location">Location to save settings.</param>
    ///
    /// <remarks>
    /// The overloaded constructor will be removed in the future.
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public SettingFolder(Assembly assembly, Format format, string location) :
        base(format, location, assembly.GetSoftwareVersion())
    {
        AutoSave     = false;
        DocumentName = new(string.Empty);
    }

    #endregion

    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// Uid
    ///
    /// <summary>
    /// Gets the unique ID of the instance.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public Guid Uid { get; } = Guid.NewGuid();

    /* --------------------------------------------------------------------- */
    ///
    /// DocumentName
    ///
    /// <summary>
    /// Gets the normalized document name.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public DocumentName DocumentName { get; private set; }

    /* --------------------------------------------------------------------- */
    ///
    /// Digest
    ///
    /// <summary>
    /// Gets the SHA-256 message digest of the source file.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string Digest { get; private set; }

    #endregion

    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// Set
    ///
    /// <summary>
    /// Sets values based on the specified arguments.
    /// </summary>
    ///
    /// <param name="args">Program arguments.</param>
    ///
    /* --------------------------------------------------------------------- */
    public void Set(IEnumerable<string> args) => Set(new(args, Argument.Windows, true));

    /* --------------------------------------------------------------------- */
    ///
    /// Set
    ///
    /// <summary>
    /// Sets values based on the specified arguments.
    /// </summary>
    ///
    /// <param name="src">Program arguments.</param>
    ///
    /* --------------------------------------------------------------------- */
    public void Set(ArgumentCollection src)
    {
        var op = src.Options;
        if (op.TryGetValue("DocumentName", out var doc)) DocumentName = new(doc);
        if (op.TryGetValue("InputFile", out var input)) Value.Source = input;
        if (op.TryGetValue("Digest", out var digest)) Digest = digest;

        var dest = Io.Combine(PathExplorer.GetDirectoryName(Value.Destination), DocumentName.Value);
        var name = HasExtension(dest) ? Io.GetBaseName(dest) : Io.GetFileName(dest);
        var ext  = Value.Extensions.Get(Value.Format);

        Value.Destination  = Io.Combine(Io.GetDirectoryName(dest), $"{name}{ext}");
        Value.DeleteSource = op.ContainsKey("DeleteOnClose");
    }

    #endregion

    #region Implementations

    /* --------------------------------------------------------------------- */
    ///
    /// HasExtension
    ///
    /// <summary>
    /// Gets a value indicating whether the specified string has an extension.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private bool HasExtension(string src)
    {
        var ext = Io.GetExtension(src);
        if (!ext.HasValue() || ext.First() != '.' || ext.Length > 5) return false;

        var ok = false;
        foreach (var c in ext.Skip(1))
        {
            var alpha = ('A' <= c && c <= 'Z') || ('a' <= c && c <= 'z');
            var num   = ('0' <= c && c <= '9');

            if (!alpha && !num) return false;
            if (alpha) ok = true;
        }
        return ok;
    }

    #endregion
}
