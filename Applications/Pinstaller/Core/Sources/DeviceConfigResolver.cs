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

namespace Cube.Pdf.Pinstaller
{
    /* --------------------------------------------------------------------- */
    ///
    /// DeviceConfigResolver
    ///
    /// <summary>
    /// Provides functionality to resolve dependencies of the specified
    /// device configuration.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal class DeviceConfigResolver
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// DeviceConfigResolver
        ///
        /// <summary>
        /// Initializes a new instance of the DeviceConfigResolver class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Source device configuration.</param>
        /// <param name="printers">Installed printers.</param>
        /// <param name="drivers">Installed printer drivers.</param>
        /// <param name="monitors">Installled port monitors.</param>
        ///
        /* ----------------------------------------------------------------- */
        public DeviceConfigResolver(DeviceConfig src,
            IEnumerable<Printer> printers,
            IEnumerable<PrinterDriver> drivers,
            IEnumerable<PortMonitor> monitors)
        {
            Source         = src;
            Printers       = printers;
            PrinterDrivers = drivers;
            PortMonitors   = monitors;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Source
        ///
        /// <summary>
        /// Gets the source device configuration.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public DeviceConfig Source { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Printers
        ///
        /// <summary>
        /// Gets the collection of installed printers.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<Printer> Printers { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// PrinterDrivers
        ///
        /// <summary>
        /// Gets the collection of installed printer drivers.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<PrinterDriver> PrinterDrivers { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// PortMonitors
        ///
        /// <summary>
        /// Gets the collection of installed port monitors.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<PortMonitor> PortMonitors { get; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Resolves dependencies of the provided configuration.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Invoke()
        {
            ResolveDrivers();
            ResolvePrinters();
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// ResolveDrivers
        ///
        /// <summary>
        /// Resolves the dependency of printer driver settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void ResolveDrivers()
        {
            var monitors = new HashSet<string>(Source.PortMonitors.Select(e => e.Name));
            var drivers  = new HashSet<string>(Source.PrinterDrivers.Select(e => e.Name));
            var results  = PrinterDrivers.Where(e => !drivers.Contains(e.Name) && monitors.Contains(e.MonitorName));

            foreach (var e in results) Source.PrinterDrivers.Add(new PrinterDriverConfig
            {
                Name         = e.Name,
                MonitorName  = e.MonitorName,
                FileName     = e.FileName,
                Config       = e.Config,
                Data         = e.Data,
                Dependencies = e.Dependencies,
                Help         = e.Help,
            });
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ResolvePrinters
        ///
        /// <summary>
        /// Resolves the dependency of printer settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void ResolvePrinters()
        {
            var ports    = new HashSet<string>(Source.Ports.Select(e => e.Name));
            var drivers  = new HashSet<string>(Source.PrinterDrivers.Select(e => e.Name));
            var printers = new HashSet<string>(Source.Printers.Select(e => e.Name));
            var results  = Printers.Where(e => !printers.Contains(e.Name) && (drivers.Contains(e.DriverName) || ports.Contains(e.PortName)));

            foreach (var e in results) Source.Printers.Add(new PrinterConfig
            {
                Name       = e.Name,
                ShareName  = e.ShareName,
                DriverName = e.DriverName,
                PortName   = e.PortName,
            });
        }

        #endregion
    }
}
