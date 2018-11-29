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
using Cube.DataContract;
using Cube.FileSystem;
using Cube.FileSystem.Mixin;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cube.Pdf.App.Pinstaller
{
    /* --------------------------------------------------------------------- */
    ///
    /// Installer
    ///
    /// <summary>
    /// Provides functionality to install or uninstall the printer and
    /// related devices.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class Installer
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// Installer
        ///
        /// <summary>
        /// Initializes a new instance of the DeviceConfig class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="format">Data format.</param>
        /// <param name="src">Location of the serialized data.</param>
        ///
        /* ----------------------------------------------------------------- */
        public Installer(Format format, string src) : this(format, src, new IO()) { }

        /* ----------------------------------------------------------------- */
        ///
        /// Installer
        ///
        /// <summary>
        /// Initializes a new instance of the DeviceConfig class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="format">Data format.</param>
        /// <param name="src">Location of the serialized data.</param>
        /// <param name="io">I/O handler.</param>
        ///
        /* ----------------------------------------------------------------- */
        public Installer(Format format, string src, IO io)
        {
            Format   = Format;
            Location = src;
            IO       = io;
            Config   = Create(format, src, io);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Config
        ///
        /// <summary>
        /// Gets the configuration of the installing devices.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public DeviceConfig Config { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Format
        ///
        /// <summary>
        /// Gets the data format of serialized data.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Format Format { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Location
        ///
        /// <summary>
        /// Gets the value that the serialized data is located.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Location { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// IO
        ///
        /// <summary>
        /// Gets the I/O handler.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IO IO { get; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Install
        ///
        /// <summary>
        /// Installs devices according to the Config property.
        /// </summary>
        ///
        /// <param name="resource">Resource directory.</param>
        /// <param name="reinstall">
        /// Value indicating whether re-installing devices when provided
        /// devices have already been installed.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        public void Install(string resource, bool reinstall) => Invoke(service =>
        {
            var monitors = Config.PortMonitors.Select(e => e.Create()).ToList();
            var ports    = Config.Ports.Select(e => e.Create()).ToList();
            var drivers  = Config.PrinterDrivers.Select(e => e.Create()).ToList();
            var printers = Config.Printers.Select(e => e.Create()).ToList();

            // Uninstall
            if (reinstall) Uninstall(printers.OfType<IInstallable>(), drivers.OfType<IInstallable>(), ports.OfType<IInstallable>(), monitors.OfType<IInstallable>());

            // Copy
            service.Stop();
            foreach (var e in monitors) e.Copy(resource, IO);
            foreach (var e in drivers)  e.Copy(resource, IO);
            service.Start();

            // Install
            foreach (var e in monitors) e.Install();
            foreach (var e in ports)    e.Install();
            foreach (var e in drivers)  e.Install();
            foreach (var e in printers) e.Install();
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Uninstall
        ///
        /// <summary>
        /// Uninstalls devices according to the Config property.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Uninstall() => Invoke(_ => Uninstall(
            Config.Printers.Select(e => e.Create() as IInstallable),
            Config.PrinterDrivers.Select(e => e.Create() as IInstallable),
            Config.Ports.Select(e => e.Create() as IInstallable),
            Config.PortMonitors.Select(e => e.Create() as IInstallable)
        ));

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Creates a new instance of the DeviceConfig class with the
        /// specified arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static DeviceConfig Create(Format format, string src, IO io) =>
            format == Format.Registry ?
            format.Deserialize<DeviceConfig>(src) :
            io.Load(src, e => format.Deserialize<DeviceConfig>(e));

        /* ----------------------------------------------------------------- */
        ///
        /// Uninstall
        ///
        /// <summary>
        /// Uninstalls all of the specified devices.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Uninstall(params IEnumerable<IInstallable>[] devices)
        {
            foreach (var inner in devices)
            {
                foreach (var e in inner) e.Uninstall();
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Invokes the specified action.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Invoke(Action<SpoolerService> action)
        {
            var service = new SpoolerService();
            service.Reset();
            try { action(service); }
            finally { service.Start(); }
        }

        #endregion
    }
}
