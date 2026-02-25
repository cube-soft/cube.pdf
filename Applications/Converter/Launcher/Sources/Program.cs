/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
/* ------------------------------------------------------------------------- */
namespace Cube.Psa.DesktopBridge;

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
    static void Main()
    {
        var dir = ApplicationData.Current.GetPublisherCacheFolder("printing");
        if (dir is null) return;

        var src = Path.Combine(dir.Path, "source.ps");
        if (!File.Exists(src)) return;

        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = "CubePsaApp.exe",
                UseShellExecute = false,
            };

            psi.ArgumentList.Add(src);
            Process.Start(psi)?.WaitForExit();
        }
        finally { if (File.Exists(src)) File.Delete(src); }
    }
}
