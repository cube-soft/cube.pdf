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
using System.Windows.Forms;
using Cube.Collections;
using Cube.Collections.Extensions;
using Cube.DataContract;
using Cube.Pdf.Converter.Mixin;

/* ------------------------------------------------------------------------- */
///
/// Program
///
/// <summary>
/// Represents the main program.
/// </summary>
///
/* ------------------------------------------------------------------------- */
static class Program
{
    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// Main
    ///
    /// <summary>
    /// Executes the main program of the application.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [STAThread]
    static void Main(string[] raw) => Logger.Error(() =>
    {
        Logger.Configure(new Logging.NLog.LoggerSource());
        Logger.ObserveTaskException();
        Logger.Info(typeof(Program).Assembly);
        Logger.Info($"Ghostscript {GetGsVersion()}");
        Logger.Info($"[ {raw.Join(" ")} ]");

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        var args = new ArgumentCollection(raw, Argument.Windows, true);
        using var src = Create(args);
        src.Migrate(@"CubeSoft\CubePDF\v2");
        src.Normalize();
        src.Set(args);

        if (args.Options.ContainsKey("SkipUI")) Invoke(src);
        else Show(src);
    });

    #endregion

    #region Implementations

    /* --------------------------------------------------------------------- */
    ///
    /// Create
    ///
    /// <summary>
    /// Creates a new instance of the SettingFolder class with the
    /// specified arguments.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private static SettingFolder Create(ArgumentCollection src) =>
        src.Options.TryGetValue("Setting", out var subkey) ?
        new(Format.Registry, subkey) :
        new();

    /* --------------------------------------------------------------------- */
    ///
    /// Show
    ///
    /// <summary>
    /// Shows the main window.
    /// </summary>
    ///
    /// <param name="src">User settings.</param>
    ///
    /* --------------------------------------------------------------------- */
    private static void Show(SettingFolder src)
    {
        var view = new MainWindow();
        view.Bind(new MainViewModel(src));
        Application.Run(view);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Invoke
    ///
    /// <summary>
    /// Invokes the conversion directly.
    /// </summary>
    ///
    /// <param name="src">User settings.</param>
    ///
    /* --------------------------------------------------------------------- */
    private static void Invoke(SettingFolder src)
    {
        using var facade = new Facade(src);
        facade.Invoke();
    }

    /* --------------------------------------------------------------------- */
    ///
    /// GetGsVersion
    ///
    /// <summary>
    /// Gets a version number of the Ghostscript.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private static int GetGsVersion()
    {
        try { return Ghostscript.Converter.Revision; }
        catch (Exception err) { Logger.Warn(err); }
        return -1;
    }

    #endregion
}
