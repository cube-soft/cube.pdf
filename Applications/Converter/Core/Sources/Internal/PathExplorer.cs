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
            var dest = Io.Get(src);
            return dest.IsDirectory ? dest.FullName : dest.DirectoryName;
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
        return GetDeaultDirectoryName();
    }

    /* --------------------------------------------------------------------- */
    ///
    /// GetDeaultDirectoryName
    ///
    /// <summary>
    /// Gets the path of the default directory.
    /// </summary>
    ///
    /// <returns>Path of the default directory.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public static string GetDeaultDirectoryName() => Io.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
        "CubeSoft",
        "CubePDF"
    );
}
