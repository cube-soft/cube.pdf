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

        try
        {
            var dir  = CacheFolder.Get() ?? throw new DirectoryNotFoundException(Metadata.DirectoryName);
            var src  = Io.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.dat");
            var meta = default(Metadata);

            try
            {
                var raw = Io.Combine(dir.Path, Metadata.SourceFileName);
                if (!Io.Exists(raw)) throw new FileNotFoundException(raw);

                Io.Move(raw, src, true);
                Logger.Debug(src);

                meta = await Metadata.LoadAsync(Io.Combine(dir.Path, Metadata.FileName));
                Io.Delete(Io.Combine(dir.Path, Metadata.FileName));
            }
            finally { Io.Delete(Io.Combine(dir.Path, Metadata.LockFileName)); }

            await (Process.Start(Create(src, meta))?.WaitForExitAsync() ?? Task.CompletedTask);
        }
        catch (Exception err) { Logger.Warn(err); }
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Create
    ///
    /// <summary>
    /// Creates a new instance of the ProcessStartInfo class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    static ProcessStartInfo Create(string src, Metadata? metadata)
    {
        var dest = new ProcessStartInfo
        {
            FileName = "CubePdf.exe",
            UseShellExecute = false,
        };

        dest.ArgumentList.Add("-DeleteOnClose");
        dest.ArgumentList.Add("-InputFile");
        dest.ArgumentList.Add(src);

        if (metadata is not null)
        {
            dest.ArgumentList.Add("-DocumentName");
            dest.ArgumentList.Add(metadata.JobTitle);
            dest.ArgumentList.Add("-SessionID");
            dest.ArgumentList.Add(metadata.SessionId);
            dest.ArgumentList.Add("-AppName");
            dest.ArgumentList.Add(metadata.AppName);
        }

        return dest;
    }

}
