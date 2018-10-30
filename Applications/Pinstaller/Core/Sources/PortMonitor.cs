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
            var hit = GetElements().FirstOrDefault(e => e.Name.Equals(name, opt));

            Name        = name;
            Exists      = (hit != null);
            Environment = Exists ? hit.Environment : this.GetEnvironment();
            FileName    = Exists ? hit.FileName : string.Empty;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// PortMonitor
        ///
        /// <summary>
        /// Initializes a new instance of the PortMonitor class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private PortMonitor() { }

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
        /// GetElements
        ///
        /// <summary>
        /// Gets the collection of currently installed port monitors.
        /// </summary>
        ///
        /// <returns>Collection of port monitors.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static IEnumerable<PortMonitor> GetElements()
        {
            var bytes = 0u;
            var count = 0u;

            bool f(IntPtr p, uint n) => NativeMethods.EnumMonitors(null, 2, p, n, ref bytes, ref count);
            if (f(IntPtr.Zero, 0)) return new PortMonitor[0];
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
        /// Installs the port monitor.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Install()
        {
            if (!NativeMethods.AddMonitor(Name, 2u, ref _core)) throw new Win32Exception();
            Exists = true;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Uninstall
        ///
        /// <summary>
        /// Uninstalls the port monitor.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Uninstall()
        {
            if (!NativeMethods.DeleteMonitor("", "", Name)) throw new Win32Exception();
            Exists = false;
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Convert
        ///
        /// <summary>
        /// Converts unmanaged resources to the PortMonitor collection.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static IEnumerable<PortMonitor> Convert(IntPtr src, uint n)
        {
            var dest = new List<PortMonitor>();
            var ptr  = src;

            for (var i = 0; i < n; ++i)
            {
                var e = (MonitorInfo2)Marshal.PtrToStructure(ptr, typeof(MonitorInfo2));
                dest.Add(new PortMonitor
                {
                    Name = e.pName,
                    Environment = e.pEnvironment,
                    FileName = e.pDLLName,
                    Exists = true,
                });
                ptr = IntPtr.Add(ptr, Marshal.SizeOf(typeof(MonitorInfo2)));
            }

            return dest;
        }

        #endregion

        #region Fields
        private MonitorInfo2 _core = new MonitorInfo2();
        #endregion
    }
}
