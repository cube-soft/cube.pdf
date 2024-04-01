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
        Logger.Debug($"[Backup] Root:{v.Backup.Quote()}, Enable:{v.BackupEnabled}, Delete:{v.BackupAutoDelete}");
    }

    #endregion

    #region Methods

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

            var date = DateTime.Today.ToString("yyyyMMdd");
            var dest = Io.Combine(_settings.Value.Backup, date, src.Name);

            if (Io.Exists(dest)) return;

            Logger.Debug($"[Backup] {dest}");
            Io.Copy(src.FullName, dest, false);
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
            if (!_settings.Value.BackupEnabled || !_settings.Value.BackupAutoDelete) return;

            var src = Io.GetDirectories(_settings.Value.Backup).ToList();
            var cvt = src.Where(IsTarget).ToList();
            if (src.Count != cvt.Count) Logger.Warn($"[Cleanup] Folders:{cvt.Count}/{src.Count}");

            var n = cvt.Count - _settings.Value.BackupDays;
            if (n <= 0) return;

            foreach (var f in cvt.OrderBy(e => e).Take(n))
            {
                Io.Delete(f);
                Logger.Debug($"[Cleanup] {f}");
            }
        }
        catch (Exception err) { throw new BackupException(err); }
    }

    #endregion

    #region Implementations

    /* --------------------------------------------------------------------- */
    ///
    /// IsTarget
    ///
    /// <summary>
    /// Determines if the specified path is a target folder for backup.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private static bool IsTarget(string src)
    {
        if (!src.HasValue()) return false;
        if (!Io.IsDirectory(src)) return false;

        var name = Io.GetFileName(src);
        if (name.Length != 8) return false;

        if (!int.TryParse(name, out var dest)) return false;
        return dest is >= 20130520 and <= 21991231; // 2013/05/20 ~ 2199/12/31
    }

    #endregion

    #region Fields
    private readonly SettingFolder _settings;
    #endregion
}
