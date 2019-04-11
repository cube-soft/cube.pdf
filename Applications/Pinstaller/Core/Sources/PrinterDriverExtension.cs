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
using Cube.FileSystem;
using Cube.Generics;
using Cube.Log;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cube.Pdf.Pinstaller
{
    /* --------------------------------------------------------------------- */
    ///
    /// PrinterDriverExtension
    ///
    /// <summary>
    /// Represents extended methods of PrinterDriver and PrinterDriverConfig
    /// classes.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class PrinterDriverExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Convert
        ///
        /// <summary>
        /// Creates a collection of the printer drivers from the specified
        /// configuration.
        /// </summary>
        ///
        /// <param name="src">Printer driver configuration.</param>
        ///
        /// <returns>Collection of printer drivers.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static IEnumerable<PrinterDriver> Convert(this IEnumerable<PrinterDriverConfig> src) =>
            src.Convert(PrinterDriver.GetElements());

        /* ----------------------------------------------------------------- */
        ///
        /// Convert
        ///
        /// <summary>
        /// Creates a collection of the printer drivers from the specified
        /// configuration.
        /// </summary>
        ///
        /// <param name="src">Printer driver configuration.</param>
        /// <param name="elements">
        /// Collection of installed printer drivers.
        /// </param>
        ///
        /// <returns>Collection of printer drivers.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static IEnumerable<PrinterDriver> Convert(this IEnumerable<PrinterDriverConfig> src,
            IEnumerable<PrinterDriver> elements) =>
            src.Select(e => e.Convert(elements));

        /* ----------------------------------------------------------------- */
        ///
        /// Convert
        ///
        /// <summary>
        /// Creates a new instance of the PrinterDriver class with the
        /// specified configuration.
        /// </summary>
        ///
        /// <param name="src">Printer driver configuration.</param>
        /// <param name="elements">
        /// Collection of installed printer drivers.
        /// </param>
        ///
        /// <returns>PrinterDriver object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static PrinterDriver Convert(this PrinterDriverConfig src,
            IEnumerable<PrinterDriver> elements) =>
            new PrinterDriver(src.Name, elements)
            {
                MonitorName  = src.MonitorName,
                FileName     = src.FileName,
                Config       = src.Config,
                Data         = src.Data,
                Help         = src.Help,
                Dependencies = src.Dependencies,
                Repository   = src.Repository,
            };

        /* ----------------------------------------------------------------- */
        ///
        /// Copy
        ///
        /// <summary>
        /// Copies resources from the specified directory.
        /// </summary>
        ///
        /// <param name="src">Printer driver object.</param>
        /// <param name="user">User resource directory.</param>
        /// <param name="io">I/O handler.</param>
        ///
        /// <remarks>
        /// データファイル (PPD) は、常にユーザが指定したディレクトリから
        /// コピーします。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static void Copy(this PrinterDriver src, string user, IO io)
        {
            var system = src.GetRepository(io);
            var from   = system.HasValue() && io.Exists(system) ? system : user;
            var to     = src.TargetDirectory;

            io.Copy(src.Data,     user, to); // see remarks.
            io.Copy(src.FileName, from, to);
            io.Copy(src.Config,   from, to);
            io.Copy(src.Help,     from, to);
            foreach (var f in src.Dependencies) io.Copy(f, from, to);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetRepository
        ///
        /// <summary>
        /// Gets the directory path from the specified configuration.
        /// </summary>
        ///
        /// <param name="src">Printer driver object.</param>
        /// <param name="io">I/O handler.</param>
        ///
        /// <returns>Path of the repository</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static string GetRepository(this PrinterDriver src, IO io)
        {
            if (!src.Repository.HasValue()) return string.Empty;

            var root = io.Combine(Environment.SpecialFolder.System.GetName(), @"DriverStore\FileRepository");
            var arch = IntPtr.Size == 4 ?  "x86" : "amd64";
            var sub  = IntPtr.Size == 4 ? "i386" : "amd64";
            var dest = io.GetDirectories(root, $"{src.Repository}.inf_{arch}*")
                         .SelectMany(e => new[] { io.Combine(e, sub), e })
                         .Where(e =>
                         {
                             var ok = io.Exists(e) && io.GetFiles(e, "*.dll").Length > 0;
                             src.LogDebug($"{e} ({ok})");
                             return ok;
                         })
                         .OrderByDescending(e => io.Get(e).LastWriteTime)
                         .FirstOrDefault();
            return dest.HasValue() ? dest : string.Empty;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Exists
        ///
        /// <summary>
        /// Determines whether the specified filename exists in the
        /// specified directory.
        /// </summary>
        ///
        /// <param name="src">Printer driver object.</param>
        /// <param name="filename">Target filename.</param>
        /// <param name="io">I/O handler.</param>
        ///
        /// <returns>true for exists; otherwise false.</returns>
        ///
        /* ----------------------------------------------------------------- */
        internal static bool Exists(this PrinterDriver src, string filename, IO io) =>
            io.Exists(io.Combine(src.TargetDirectory, filename));

        #endregion
    }
}
