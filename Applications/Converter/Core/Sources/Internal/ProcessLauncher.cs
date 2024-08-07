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
using System.Diagnostics;
using System.Linq;
using Cube.FileSystem;
using Cube.Text.Extensions;

/* ------------------------------------------------------------------------- */
///
/// ProcessLauncher
///
/// <summary>
/// Provides functionality to execute the provided post process.
/// </summary>
///
/* ------------------------------------------------------------------------- */
internal sealed class ProcessLauncher
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// ProcessLauncher
    ///
    /// <summary>
    /// Initializes a new instance of the ProcessLauncher class with
    /// the specified settings.
    /// </summary>
    ///
    /// <param name="src">User settings.</param>
    ///
    /* --------------------------------------------------------------------- */
    public ProcessLauncher(SettingFolder src) => Settings = src;

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

    #endregion

    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// Invoke
    ///
    /// <summary>
    /// Executes the post process with the specified files.
    /// </summary>
    ///
    /// <param name="src">Source files.</param>
    ///
    /* --------------------------------------------------------------------- */
    public void Invoke(IEnumerable<string> src)
    {
        try
        {
            _ = Settings.Value.PostProcess switch
            {
                PostProcess.Open          => Open(src),
                PostProcess.OpenDirectory => OpenDirectory(src),
                PostProcess.Others        => RunUserProgram(src),
                _                         => default,
            };
        }
        catch (Exception err)
        {
            var key  = Settings.Value.PostProcess;
            var user = key == PostProcess.Others ? Settings.Value.UserProgram : string.Empty;
            throw new PostProcessException(key, user, err);
        }
    }

    #endregion

    #region Implementations

    /* --------------------------------------------------------------------- */
    ///
    /// Open
    ///
    /// <summary>
    /// Opens the specified files with the associated program.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private static Process Open(IEnumerable<string> src) => Start(src.First(), string.Empty);

    /* --------------------------------------------------------------------- */
    ///
    /// OpenDirectory
    ///
    /// <summary>
    /// Opens the directory at which the specified files are located.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private static Process OpenDirectory(IEnumerable<string> src) => Start(
        "explorer.exe",
        Io.GetDirectoryName(src.First()).Quote()
    );

    /* --------------------------------------------------------------------- */
    ///
    /// RunUserProgram
    ///
    /// <summary>
    /// Executes the specified program.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private Process RunUserProgram(IEnumerable<string> src) =>
        Settings.Value.UserProgram.HasValue() ?
        Start(Settings.Value.UserProgram, src.First().Quote()) :
        default;

    /* --------------------------------------------------------------------- */
    ///
    /// Start
    ///
    /// <summary>
    /// Executes the process with the specified arguments.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private static Process Start(string exec, string args) => Process.Start(new ProcessStartInfo
    {
        FileName        = exec,
        Arguments       = args,
        CreateNoWindow  = false,
        UseShellExecute = true,
        LoadUserProfile = false,
        WindowStyle     = ProcessWindowStyle.Normal,
    });

    #endregion
}
