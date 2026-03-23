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
using System.Threading.Tasks;
using Cube.FileSystem;
using Cube.Pdf.Converter.Psa;
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
    static async Task Main()
    {
        Logger.Configure(new Logging.NLog.LoggerSource());
        Logger.ObserveTaskException();
        Logger.Info(typeof(Program).Assembly);

        var dir = ApplicationData.Current.GetPublisherCacheFolder(Metadata.DirectoryName) ??
                  throw new DirectoryNotFoundException(Metadata.DirectoryName);
        var raw = Io.Combine(dir.Path, Metadata.SourceFileName);
        if (!Io.Exists(raw)) throw new FileNotFoundException(raw);

        var src = Io.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Io.Move(raw, src, true);
        Logger.Debug(src);

        var metadata = await Metadata.LoadAsync(Io.Combine(dir.Path, Metadata.FileName));
        Io.Delete(Io.Combine(dir.Path, Metadata.FileName));
        Io.Delete(Io.Combine(dir.Path, Metadata.LockFileName));

        var psi = new ProcessStartInfo
        {
            FileName = "CubePdf.exe",
            UseShellExecute = false,
        };

        psi.ArgumentList.Add("-DeleteOnClose");
        psi.ArgumentList.Add("-InputFile");
        psi.ArgumentList.Add(src);
        psi.ArgumentList.Add("-DocumentName");
        psi.ArgumentList.Add(metadata.JobTitle);
        psi.ArgumentList.Add("-SessionID");
        psi.ArgumentList.Add(metadata.SessionId);
        psi.ArgumentList.Add("-AppName");
        psi.ArgumentList.Add(metadata.AppName);

        await (Process.Start(psi)?.WaitForExitAsync() ?? Task.CompletedTask);
    }
}
