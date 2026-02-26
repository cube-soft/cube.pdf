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
using System.Diagnostics;
using System.IO;
using Windows.Storage;

/* ------------------------------------------------------------------------- */
///
/// Program
///
/// <summary>
/// Represents the main program.
/// </summary>
///
/* ------------------------------------------------------------------------- */
internal class Program
{
    /* --------------------------------------------------------------------- */
    ///
    /// Main
    ///
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    static void Main() => Logger.Try(() =>
    {
        Logger.Configure(new Logging.NLog.LoggerSource());
        Logger.ObserveTaskException();
        Logger.Info(typeof(Program).Assembly);

        var dir = ApplicationData.Current.GetPublisherCacheFolder("printing") ??
                  throw new DirectoryNotFoundException("printing");
        var raw = Path.Combine(dir.Path, "source.ps");
        if (!File.Exists(raw)) throw new FileNotFoundException(raw);

        var src = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        File.Move(raw, src);
        Logger.Debug(src);

        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = "CubePdf.exe",
                UseShellExecute = false,
            };

            psi.ArgumentList.Add("-DeleteOnClose");
            psi.ArgumentList.Add("-DocumentName");
            psi.ArgumentList.Add("CubePDF PSA v4"); // TODO: How to get printing document name.
            psi.ArgumentList.Add("-InputFile");
            psi.ArgumentList.Add(src);

            Process.Start(psi)?.WaitForExit();
        }
        finally
        {
            if (File.Exists(raw)) File.Delete(raw);
            if (File.Exists(src)) File.Delete(src);
        }
    });
}
