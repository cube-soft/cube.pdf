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

namespace Cube.Pdf.App.Pinstaller
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
    internal static class PrinterDriverExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Creates a new instance of the PrinterDriver class from the
        /// specified configuration.
        /// </summary>
        ///
        /// <param name="src">Printer driver configuration.</param>
        ///
        /// <returns>Printer driver object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static PrinterDriver Create(this PrinterDriverConfig src) =>
            new PrinterDriver(src.Name)
            {
                MonitorName  = src.MonitorName,
                FileName     = src.FileName,
                Config       = src.Config,
                Data         = src.Data,
                Help         = src.Help,
                Dependencies = src.Dependencies,
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
        /// <param name="from">Resource directory.</param>
        /// <param name="io">I/O handler.</param>
        ///
        /// <remarks>
        /// Dependencies には複数のファイルが指定される可能性がある。
        /// その場合の処理方法を要検討。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static void Copy(this PrinterDriver src, string from, IO io)
        {
            var to = src.DirectoryName;

            io.Copy(src.FileName,     from, to);
            io.Copy(src.Config,       from, to);
            io.Copy(src.Data,         from, to);
            io.Copy(src.Help,         from, to);
            io.Copy(src.Dependencies, from, to); // see remarks
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Normalize
        ///
        /// <summary>
        /// Normalizes paths of resources.
        /// </summary>
        ///
        /// <param name="src">Printer driver object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Normalize(this PrinterDriver src, IO io)
        {
            src.FileName     = io.Combine(src.DirectoryName, src.FileName);
            src.Config       = io.Combine(src.DirectoryName, src.Config);
            src.Data         = io.Combine(src.DirectoryName, src.Data);
            src.Help         = io.Combine(src.DirectoryName, src.Help);
            src.Dependencies = io.Combine(src.DirectoryName, src.Dependencies);
        }

        #endregion
    }
}
