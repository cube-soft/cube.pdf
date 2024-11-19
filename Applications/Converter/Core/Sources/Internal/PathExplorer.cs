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
using System.Linq;
using Cube.FileSystem;
using Cube.Text.Extensions;

/* ------------------------------------------------------------------------- */
///
/// PathExplorer
///
/// <summary>
/// Provides functionality to determine the path.
/// </summary>
///
/* ------------------------------------------------------------------------- */
internal static class PathExplorer
{
    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// HasExtension
    ///
    /// <summary>
    /// Gets a value indicating whether the specified string has an extension.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static bool HasExtension(string src)
    {
        if (!src.HasValue()) return false;
        var ext = Io.GetExtension(src);
        if (!ext.HasValue() || ext.First() != '.' || ext.Length > 6) return false;

        var ok = false;
        foreach (var c in ext.Skip(1))
        {
            var alpha = ('A' <= c && c <= 'Z') || ('a' <= c && c <= 'z');
            var num = ('0' <= c && c <= '9');

            if (!alpha && !num) return false;
            if (alpha) ok = true;
        }
        return ok;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// GetDirectoryName
    ///
    /// <summary>
    /// Gets the directory part of the specified path. If the specified value
    /// is empty or an exception occurs, the method returns the value of
    /// GetDesktopDirectoryName method instead.
    /// </summary>
    ///
    /// <returns>Path of the directory part.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public static string GetDirectoryName(string src)
    {
        var desktop = GetDesktopDirectoryName();

        try
        {
            if (!src.HasValue()) return desktop;
            return Io.IsDirectory(src) ? src : Io.GetDirectoryName(src);
        }
        catch (Exception e) { Logger.Warn(e); }

        return desktop;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// GetDesktopDirectoryName
    ///
    /// <summary>
    /// Gets the path of the user desktop directory. If an exception occurs,
    /// the method returns the value of GetDefaultDirectoryName method
    /// instead.
    /// </summary>
    ///
    /// <returns>Path of the user desktop.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public static string GetDesktopDirectoryName()
    {
        try { return Environment.GetFolderPath(Environment.SpecialFolder.Desktop); }
        catch (Exception e) { Logger.Warn(e); }
        return GetDefaultDirectoryName();
    }

    /* --------------------------------------------------------------------- */
    ///
    /// GetDefaultDirectoryName
    ///
    /// <summary>
    /// Gets the path of the default directory.
    /// </summary>
    ///
    /// <returns>Path of the default directory.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public static string GetDefaultDirectoryName() => Io.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
        "CubeSoft",
        "CubePDF"
    );

    #endregion
}
