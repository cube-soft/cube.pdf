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
using Cube.Log;
using Microsoft.Win32;
using System;
using System.Linq;

namespace Cube.Pdf.App.Pinstaller
{
    /* --------------------------------------------------------------------- */
    ///
    /// PortMonitor
    ///
    /// <summary>
    /// Provides functionality to install or uninstall a port monitor.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class PortMonitor : IInstaller
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// PortMonitor
        ///
        /// <summary>
        /// Initializes a new instance of the PortMonitor class with the
        /// specified name.
        /// </summary>
        ///
        /// <param name="name">Name of the port monitor.</param>
        ///
        /* ----------------------------------------------------------------- */
        public PortMonitor(string name)
        {
            var opt = StringComparison.InvariantCultureIgnoreCase;

            Name        = name;
            Environment = this.GetEnvironment();
            Exists      = GetOrDefault(k => k.GetSubKeyNames().Any(s => s.Equals(name, opt)));
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Name
        ///
        /// <summary>
        /// Gets the name of the port monitor.
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
        /// Gets or sets the DLL name of the port monitor.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string FileName
        {
            get => _core.pDLLName;
            set => _core.pDLLName = value;
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
        /// Exists
        ///
        /// <summary>
        /// Gets the value indicating whether the port monitor has been
        /// already installed.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Exists { get; private set; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Install
        ///
        /// <summary>
        /// Installs the port monitor.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Install() => this.Invoke(() =>
        {
            var status = WinSpool.NativeMethods.AddMonitor(Name, 2u, ref _core);
            if (status != 0) Exists = true;
            return status;
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Uninstall
        ///
        /// <summary>
        /// Uninstalls the port monitor.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Uninstall() => this.Invoke(() =>
        {
            var status = WinSpool.NativeMethods.DeleteMonitor("", "", Name);
            if (status != 0) Exists = false;
            return status;
        });

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Open
        ///
        /// <summary>
        /// Gets the root RegistryKey object of port monitors and executes
        /// the specified callback function.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private T GetOrDefault<T>(Func<RegistryKey, T> callback)
        {
            var s = @"System\CurrentControlSet\Control\Print\Monitors";

            try { using (var sk = Registry.LocalMachine.OpenSubKey(s, false)) return callback(sk); }
            catch (Exception err) { this.LogWarn(err.ToString(), err); }

            return default(T);
        }

        #endregion

        #region Fields
        private MonitorInfo2 _core = new MonitorInfo2();
        #endregion
    }
}
