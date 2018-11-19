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
    /// PortMonitorExtension
    ///
    /// <summary>
    /// Represents extended methods of PortMonitor and PortMonitorConfig
    /// classes.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class PortMonitorExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Creates a new instance of the PortMonitor class from the
        /// specified configuration.
        /// </summary>
        ///
        /// <param name="src">Port monitor configuration.</param>
        ///
        /// <returns>Port monitor object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static PortMonitor Create(this PortMonitorConfig src) =>
            new PortMonitor(src.Name)
            {
                FileName = src.FileName,
                Config   = src.Config,
            };

        /* ----------------------------------------------------------------- */
        ///
        /// Copy
        ///
        /// <summary>
        /// Copies resources from the specified directory.
        /// </summary>
        ///
        /// <param name="src">Port monitor object.</param>
        /// <param name="from">Resource directory.</param>
        /// <param name="io">I/O handler.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Copy(this PortMonitor src, string from, IO io)
        {
            var to = src.DirectoryName;

            io.Copy(src.FileName, from, to);
            io.Copy(src.Config,   from, to);
        }

        #endregion
    }
}
