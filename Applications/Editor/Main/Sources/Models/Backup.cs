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
namespace Cube.Pdf.Editor;

using System;
using System.Linq;
using Cube.FileSystem;
using Cube.Text.Extensions;

/* ------------------------------------------------------------------------- */
///
/// Backup
///
/// <summary>
/// Provides functionality to backup files.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public sealed class Backup
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// DirectoryMonitor
    ///
    /// <summary>
    /// Initializes a new instance of the Backup class with the specified
    /// arguments.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public Backup(SettingFolder settings)
    {
        _settings = settings;
        var v = _settings.Value;
        Logger.Debug($"Enable:{v.BackupEnabled}, Delete:{v.BackupAutoDelete}, Gen:{v.BackupDays}");

        var src = _settings.Value.Backup;
        var cvt = GetRootDirectory();
        Logger.Debug(cvt.Quote());
        if (src != cvt) Logger.Debug($"{src.Quote()} (Original)");
    }

    #endregion

    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// GetDefaultRootDirectory
    ///
    /// <summary>
    /// Gets the path of the default backup root directory.
    /// </summary>
    ///
    /// <returns>Path of the default backup root directory.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public static string GetDefaultRootDirectory() => Io.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "CubeSoft",
        ProductFolder,
        BackupFolder
    );

    /* --------------------------------------------------------------------- */
    ///
    /// GetRootDirectory
    ///
    /// <summary>
    /// Gets the path of the backup root directory.
    /// </summary>
    ///
    /// <returns>Path of the backup root directory.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public string GetRootDirectory()
    {
        var src = _settings.Value.Backup?.TrimEnd(
            System.IO.Path.DirectorySeparatorChar,
            System.IO.Path.AltDirectorySeparatorChar
        ) ?? string.Empty;

        if (!src.HasValue()) return GetDefaultRootDirectory();
        if (src.Length == 2 && src[1] == ':') return Io.Combine($@"{src}\", ProductFolder, BackupFolder);

        var dir = Io.GetFileName(src);
        if (!dir.HasValue()) return Io.Combine(src, ProductFolder, BackupFolder);
        if (dir.FuzzyEquals(ProductFolder)) return Io.Combine(src, BackupFolder);
        if (dir.FuzzyEquals(BackupFolder))
        {
            var parent = Io.GetFileName(Io.GetDirectoryName(src));
            return parent.HasValue() && parent.FuzzyEquals(ProductFolder) ?
                   src :
                   Io.Combine(src, ProductFolder, BackupFolder);
        }
        return Io.Combine(src, ProductFolder, BackupFolder);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Invoke
    ///
    /// <summary>
    /// Copies the specified file to the backup directory.
    /// </summary>
    ///
    /// <param name="src">Source file.</param>
    ///
    /* --------------------------------------------------------------------- */
    public void Invoke(Entity src)
    {
        try
        {
            if (!_settings.Value.BackupEnabled || !src.Exists) return;

            var dest = Io.Combine(GetRootDirectory(), DateTime.Today.ToString("yyyyMMdd"), src.Name);
            if (Io.Exists(dest)) return;

            Io.Copy(src.FullName, dest, false);
            Logger.Debug($"[Backup] {dest.Quote()}");
        }
        catch (Exception err) { throw new BackupException(err); }
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Cleanup
    ///
    /// <summary>
    /// Deletes expired files.
    /// </summary>
    ///
    /// <remarks>
    /// Up to the number of directories equal to the number of days
    /// retained will be retained without deleting them.
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public void Cleanup()
    {
        try
        {
            if (!_settings.Value.BackupEnabled || !_settings.Value.BackupAutoDelete)
            {
                Logger.Debug("[Clean] Skip");
                return;
            }

            var src = Io.GetDirectories(GetRootDirectory()).ToList();
            var cvt = src.Where(IsBackupFolder).ToList();
            if (src.Count != cvt.Count) Logger.Warn($"Folders:{cvt.Count}/{src.Count}");

            var n = cvt.Count - _settings.Value.BackupDays;
            if (n <= 0) return;

            foreach (var f in cvt.OrderBy(e => e).Take(n))
            {
                Io.Delete(f);
                Logger.Debug($"[Clean] {f.Quote()}");
            }
        }
        catch (Exception err) { throw new BackupException(err); }
    }

    #endregion

    #region Implementations

    /* --------------------------------------------------------------------- */
    ///
    /// IsBackupFolder
    ///
    /// <summary>
    /// Determines if the specified path is a target folder for cleaning up.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private static bool IsBackupFolder(string src)
    {
        if (!src.HasValue()) return false;
        if (!Io.IsDirectory(src)) return false;

        var s = Io.GetFileName(src);
        if (s.Length != 8) return false;
        if (!int.TryParse(s, out var ymd)) return false;

        var d = ymd % 100;
        if (d is < 1 or > 31) return false;

        ymd /= 100;
        var m = ymd % 100;
        if (m is < 1 or > 12) return false;

        ymd /= 100;
        var y = ymd % 10000;
        return y is >= 2000 and <= 2200;
    }

    #endregion

    #region Fields
    private readonly SettingFolder _settings;
    private const string ProductFolder = "CubePdfUtility2";
    private const string BackupFolder = "Backup";
    #endregion
}
