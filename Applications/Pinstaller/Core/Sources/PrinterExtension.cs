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
using System.Collections.Generic;
using System.Linq;

namespace Cube.Pdf.App.Pinstaller
{
    /* --------------------------------------------------------------------- */
    ///
    /// PrinterExtension
    ///
    /// <summary>
    /// Represents extended methods of Printer and PrinterConfig classes.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class PrinterExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Creates a collection of printers from the specified
        /// configuration.
        /// </summary>
        ///
        /// <param name="src">Printer configuration.</param>
        ///
        /// <returns>Collection of printers.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static IEnumerable<Printer> Create(this IEnumerable<PrinterConfig> src) =>
            src.Create(Printer.GetElements());

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Creates a collection of printers from the specified
        /// configuration.
        /// </summary>
        ///
        /// <param name="src">Printer configuration.</param>
        /// <param name="elements">Collection of printers.</param>
        ///
        /// <returns>Collection of printers.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static IEnumerable<Printer> Create(this IEnumerable<PrinterConfig> src,
            IEnumerable<Printer> elements) =>
            src.Select(e => new Printer(e.Name, elements)
            {
                ShareName  = e.ShareName,
                DriverName = e.DriverName,
                PortName   = e.PortName,
            });

        #endregion
    }
}
