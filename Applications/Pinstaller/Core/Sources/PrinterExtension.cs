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
        /// Creates a new instance of the Printer class from the specified
        /// configuration.
        /// </summary>
        ///
        /// <param name="src">Printer configuration.</param>
        ///
        /// <returns>Printer object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Printer Create(this PrinterConfig src) =>
            new Printer(src.Name)
            {
                ShareName  = src.ShareName,
                DriverName = src.DriverName,
                PortName   = src.PortName,
            };

        #endregion
    }
}
