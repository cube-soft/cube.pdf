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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;

namespace Cube.Pdf.App.Pinstaller
{
    /* --------------------------------------------------------------------- */
    ///
    /// PrinterDriver
    ///
    /// <summary>
    /// Provides functionality to install or uninstall a printer driver.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class PrinterDriver : IInstaller
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// PrinterDriver
        ///
        /// <summary>
        /// Initializes a new instance of the PrinterDriver class with
        /// the specified arguments.
        /// </summary>
        ///
        /// <param name="name">Name of the printer driver.</param>
        /// <param name="monitor">Name of the port monitor.</param>
        ///
        /* ----------------------------------------------------------------- */
        public PrinterDriver(string name, string monitor) : this()
        {
            var opt = StringComparison.InvariantCultureIgnoreCase;
            var hit = GetElements().FirstOrDefault(e => e.Name.Equals(name, opt));

            Name        = name;
            MonitorName = monitor;
            Exists      = (hit != null);
            Environment = Exists ? hit.Environment : this.GetEnvironment();
            FileName    = Exists ? hit.FileName : string.Empty;
            Config      = Exists ? hit.Config : string.Empty;
            Data        = Exists ? hit.Data : string.Empty;
            Help        = Exists ? hit.Help : string.Empty;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// PrinterDriver
        ///
        /// <summary>
        /// Initializes a new instance of the PrinterDriver class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private PrinterDriver()
        {
            _core.cVersion         = 3;
            _core.pDefaultDataType = "RAW";
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Name
        ///
        /// <summary>
        /// Gets the name of the printer drvier.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Name
        {
            get => _core.pName;
            private set => _core.pName = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// FileName
        ///
        /// <summary>
        /// Gets or sets the name of the drvier file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string FileName
        {
            get => _core.pDriverPath;
            set => _core.pDriverPath = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// MonitorName
        ///
        /// <summary>
        /// Gets the name of the port monitor.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string MonitorName
        {
            get => _core.pMonitorName;
            private set => _core.pMonitorName = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Environment
        ///
        /// <summary>
        /// Gets the name of architecture (Windows NT x86 or Windows x64).
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Environment
        {
            get => _core.pEnvironment;
            private set => _core.pEnvironment = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Config
        ///
        /// <summary>
        /// Gets the name of config file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Config
        {
            get => _core.pConfigFile;
            set => _core.pConfigFile = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Data
        ///
        /// <summary>
        /// Gets the name of data file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Data
        {
            get => _core.pDataFile;
            set => _core.pDataFile = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Help
        ///
        /// <summary>
        /// Gets the name of help file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Help
        {
            get => _core.pHelpFile;
            set => _core.pHelpFile = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Dependents
        ///
        /// <summary>
        /// Gets the collection of dependent files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ICollection<string> Dependents { get; } = new List<string>();

        /* ----------------------------------------------------------------- */
        ///
        /// Exists
        ///
        /// <summary>
        /// Gets the value indicating whether the printer driver has been
        /// already installed.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Exists { get; private set; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetElements
        ///
        /// <summary>
        /// Gets the collection of currently installed port monitors.
        /// </summary>
        ///
        /// <returns>Collection of port monitors.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static IEnumerable<PrinterDriver> GetElements()
        {
            var bytes = 0u;
            var count = 0u;

            bool f(IntPtr p, uint n) => NativeMethods.EnumPrinterDrivers(null, "", 3, p, n, ref bytes, ref count);
            if (f(IntPtr.Zero, 0)) return new PrinterDriver[0];
            if (Marshal.GetLastWin32Error() != 122) throw new Win32Exception();

            var buffer = Marshal.AllocHGlobal((int)bytes);
            try
            {
                if (f(buffer, bytes)) return Convert(buffer, count);
                else throw new Win32Exception();
            }
            finally { Marshal.FreeHGlobal(buffer); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Install
        ///
        /// <summary>
        /// Installs the printer driver.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Install()
        {
            if (!NativeMethods.AddPrinterDriver(Name, 3, ref _core)) throw new Win32Exception();
            Exists = true;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Uninstall
        ///
        /// <summary>
        /// Uninstalls the printer driver.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Uninstall()
        {
            if (!NativeMethods.DeletePrinterDriver("", Environment, Name)) throw new Win32Exception();
            Exists = false;
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Convert
        ///
        /// <summary>
        /// Converts unmanaged resources to the PrinterDriver collection.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static IEnumerable<PrinterDriver> Convert(IntPtr src, uint n)
        {
            var dest = new List<PrinterDriver>();
            var ptr = src;

            for (var i = 0; i < n; ++i)
            {
                var e = (DriverInfo3)Marshal.PtrToStructure(ptr, typeof(DriverInfo3));
                dest.Add(new PrinterDriver
                {
                    Name        = e.pName,
                    MonitorName = e.pMonitorName,
                    Environment = e.pEnvironment,
                    FileName    = e.pDriverPath,
                    Config      = e.pConfigFile,
                    Data        = e.pDataFile,
                    Help        = e.pHelpFile,
                    Exists      = true,
                });
                ptr = IntPtr.Add(ptr, Marshal.SizeOf(typeof(DriverInfo3)));
            }

            return dest;
        }

        #endregion

        #region Fields
        private DriverInfo3 _core = new DriverInfo3();
        #endregion
    }
}
