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
using Cube.FileSystem;
using Cube.Pdf.Ghostscript;

/* ------------------------------------------------------------------------- */
///
/// FileTransfer
///
/// <summary>
/// Provides functionality to move or rename files.
/// </summary>
///
/* ------------------------------------------------------------------------- */
internal sealed class FileTransfer : DisposableBase
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// FileTransfer
    ///
    /// <summary>
    /// Initializes a new instance of the FileTransfer class with the
    /// specified arguments.
    /// </summary>
    ///
    /// <param name="src">User settings.</param>
    /// <param name="temp">Temp directory.</param>
    ///
    /* --------------------------------------------------------------------- */
    public FileTransfer(SettingFolder src, string temp)
    {
        Settings = src;
        Temp     = GetTempDirectory(temp);
        Target   = Io.Get(src.Value.Destination);
        Value    = Io.Combine(Temp, GetName());
    }

    #endregion

    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// Settings
    ///
    /// <summary>
    /// Gets the user settings.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public SettingFolder Settings { get; }

    /* --------------------------------------------------------------------- */
    ///
    /// Target
    ///
    /// <summary>
    /// Gets a value that represents the path to save the file.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public Entity Target { get; }

    /* --------------------------------------------------------------------- */
    ///
    /// Value
    ///
    /// <summary>
    /// Gets the path to save, which represents the temporary filename.
    /// </summary>
    ///
    /// <remarks>
    /// In the user program, save the file to the path specified by Value,
    /// and then use the Invoke method to move the file to the location
    /// where it should be saved.
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public string Value { get; }

    /* --------------------------------------------------------------------- */
    ///
    /// Temp
    ///
    /// <summary>
    /// Gets the path of the temp directory.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string Temp { get; }

    /* --------------------------------------------------------------------- */
    ///
    /// AutoRename
    ///
    /// <summary>
    /// Gets or a value indicating whether to rename files automatically
    /// when the specified file exists.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool AutoRename => Settings.Value.SaveOption == SaveOption.Rename;

    #endregion

    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// Invoke
    ///
    /// <summary>
    /// Invokes operations to move or rename files.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public void Invoke(IList<string> dest)
    {
        var src = Io.GetFiles(Temp);
        var n   = src.Count();
        var i   = 0;

        foreach (var e in src)
        {
            var path = GetDestination(i + 1, n);
            MoveOrCopy(e, path, true);
            dest.Add(path);
            GetType().LogDebug($"Save:{path}");
            ++i;
        }
    }

    #endregion

    #region Static methods

    /* --------------------------------------------------------------------- */
    ///
    /// MoveOrCopy
    ///
    /// <summary>
    /// Moves or copies the specified file.
    /// </summary>
    ///
    /// <param name="src">Source path.</param>
    /// <param name="dest">Path to move or copy.</param>
    /// <param name="overwrite">Overwrite or not.</param>
    ///
    /* --------------------------------------------------------------------- */
    public static void MoveOrCopy(string src, string dest, bool overwrite)
    {
        try { Io.Move(src, dest, overwrite); }
        catch { Io.Copy(src, dest, overwrite); }
    }

    #endregion

    #region Implementations

    /* --------------------------------------------------------------------- */
    ///
    /// Dispose
    ///
    /// <summary>
    /// Releases the unmanaged resources used by the
    /// FileTransfer and optionally releases the managed
    /// resources.
    /// </summary>
    ///
    /// <param name="disposing">
    /// true to release both managed and unmanaged resources;
    /// false to release only unmanaged resources.
    /// </param>
    ///
    /* --------------------------------------------------------------------- */
    protected override void Dispose(bool disposing) => GetType().LogWarn(() => Io.Delete(Temp));

    /* --------------------------------------------------------------------- */
    ///
    /// GetTempDirectory
    ///
    /// <summary>
    /// Gets the path of the working directory.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private string GetTempDirectory(string src) =>
        Enumerable.Range(1, int.MaxValue)
                  .Select(e => Io.Combine(src, e.ToString()))
                  .First(e => !Io.Exists(e));

    /* --------------------------------------------------------------------- */
    ///
    /// GetName
    ///
    /// <summary>
    /// Gets the value that represents the filename.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private string GetName() =>
        DocumentConverter.SupportedFormats.Contains(Settings.Value.Format) ?
        $"tmp{Target.Extension}" :
        $"tmp-%08d{Target.Extension}";

    /* --------------------------------------------------------------------- */
    ///
    /// GetDestination
    ///
    /// <summary>
    /// Gets the path to save the file from the specified arguments.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private string GetDestination(int index, int count)
    {
        var dest = GetDestinationCore(index, count);
        return (AutoRename && Io.Exists(dest)) ? IoEx.GetUniqueName(dest) : dest;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// GetDestinationCore
    ///
    /// <summary>
    /// Gets the path to save the file from the specified arguments.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private string GetDestinationCore(int index, int count)
    {
        if (count <= 1) return Target.FullName;

        var name  = Target.BaseName;
        var ext   = Target.Extension;
        var digit = string.Format("D{0}", Math.Max(count.ToString("D").Length, 2));
        var dest  = string.Format("{0}-{1}{2}", name, index.ToString(digit), ext);

        return Io.Combine(Target.DirectoryName, dest);
    }

    #endregion
}
