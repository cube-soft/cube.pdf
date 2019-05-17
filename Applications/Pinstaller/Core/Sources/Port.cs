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
using Cube.Mixin.Iteration;
using Cube.Mixin.String;
using Cube.Pdf.Pinstaller.Debug;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Cube.Pdf.Pinstaller
{
    /* --------------------------------------------------------------------- */
    ///
    /// Port
    ///
    /// <summary>
    /// Provides functionality to install or uninstall a port.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class Port : IInstallable
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// Port
        ///
        /// <summary>
        /// Initializes a new instance of the PortMonitor class with the
        /// specified arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Port(string name, string monitor) : this(name, monitor, Get(name, monitor)) { }

        /* ----------------------------------------------------------------- */
        ///
        /// Port
        ///
        /// <summary>
        /// Initializes a new instance of the PortMonitor class with the
        /// specified arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private Port(string name, string monitor, Core core)
        {
            Name        = name;
            MonitorName = monitor;
            Environment = this.GetEnvironment();
            Exists      = core != null;
            RetryCount  = 5;
            _core       = core ?? new Core();
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Name
        ///
        /// <summary>
        /// Gets the port name.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Name { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// MonitorName
        ///
        /// <summary>
        /// Gets the name of the port monitor.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string MonitorName { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Application
        ///
        /// <summary>
        /// Gets or sets the application path that the port executes.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Application
        {
            get => _core.AppPath;
            set => _core.AppPath = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Arguments
        ///
        /// <summary>
        /// Gets or sets the arguments for the program.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Arguments
        {
            get => _core.AppArgs;
            set => _core.AppArgs = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Proxy
        ///
        /// <summary>
        /// Gets or sets the path of the proxy application.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Proxy
        {
            get => _core.AppProxy;
            set => _core.AppProxy = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Temp
        ///
        /// <summary>
        /// Gets or sets the temporary directory of the port.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Temp
        {
            get => _core.TempDir;
            set => _core.TempDir = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// WaitForExit
        ///
        /// <summary>
        /// Gets or sets the value indicating whether the port wait for
        /// the termination of the executing program.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool WaitForExit
        {
            get => _core.WaitForApp;
            set => _core.WaitForApp = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// RunAsUser
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to run the provided
        /// application as the currently logged on user.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool RunAsUser
        {
            get => _core.UseAppProxy;
            set => _core.UseAppProxy = value;
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
        public string Environment { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Exists
        ///
        /// <summary>
        /// Gets the value indicating whether the port has been already
        /// installed.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Exists { get; private set; }

        /* ----------------------------------------------------------------- */
        ///
        /// RetryCount
        ///
        /// <summary>
        /// Gets or sets the maximum number of attempts for an installation
        /// or uninstallation operation to succeed.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int RetryCount { get; set; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetElements
        ///
        /// <summary>
        /// Gets the collection of currently installed ports from the
        /// specified port monitor name.
        /// </summary>
        ///
        /// <param name="monitor">Name of port monitor.</param>
        ///
        /// <returns>Collection of ports.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static IEnumerable<Port> GetElements(string monitor)
        {
            var dest = new List<Port>();

            using (var k = Open(GetName(monitor, "Ports"), false))
            {
                foreach (var name in k?.GetSubKeyNames() ?? new string[0])
                using (var kk = k.OpenSubKey(name, false))
                {
                    var core = kk.Deserialize<Core>();
                    dest.Add(new Port(name, monitor, core));
                }
            }

            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CanInstall
        ///
        /// <summary>
        /// Determines that the port can be installed.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool CanInstall() => Name.HasValue() && MonitorName.HasValue();

        /* ----------------------------------------------------------------- */
        ///
        /// Install
        ///
        /// <summary>
        /// Installs the port.
        /// </summary>
        ///
        /// <remarks>
        /// ポート設定の更新を考慮して、Exists の値に関わらずレジストリへの
        /// 書き込みは実行する。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public void Install()
        {
            this.Log();

            if (CanInstall()) RetryCount.Try(i =>
            {
                if (!Exists) Register(MonitorName, Name);
                Exists = true;
                using (var k = Open(GetName(MonitorName, "Ports", Name), true)) k.Serialize(_core);
            });
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Uninstall
        ///
        /// <summary>
        /// Uninstalls the port.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Uninstall()
        {
            this.Log();

            if (Exists) RetryCount.Try(i =>
            {
                using (var k = Open(GetName(MonitorName, "Ports"), true))
                {
                    var names = k.GetSubKeyNames();
                    if (names.Any(e => e.FuzzyEquals(Name))) k.DeleteSubKeyTree(Name);
                    Exists = false;
                }
            });
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Port.Core
        ///
        /// <summary>
        /// Represents core information of the Port class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataContract]
        private class Core
        {
            [DataMember] public string AppPath   { get; set; }
            [DataMember] public string AppArgs   { get; set; }
            [DataMember] public string AppProxy  { get; set; }
            [DataMember] public string TempDir   { get; set; }
            [DataMember] public bool WaitForApp  { get; set; }
            [DataMember] public bool UseAppProxy { get; set; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Get
        ///
        /// <summary>
        /// Gets a Core object from the registry.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static Core Get(string name, string monitor)
        {
            var key = GetName(monitor, "Ports", name);
            using (var k = Open(key, false)) return k?.Deserialize<Core>();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetName
        ///
        /// <summary>
        /// Gets the name of registry subkey from the specified names.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static string GetName(params string[] subkeys)
        {
            var root = @"System\CurrentControlSet\Control\Print\Monitors";
            return subkeys.Length > 0 ? $@"{root}\{string.Join("\\", subkeys)}" : root;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Open
        ///
        /// <summary>
        /// Opens the registry from the specified name.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static RegistryKey Open(string name, bool writable) => writable ?
            Registry.LocalMachine.CreateSubKey(name) :
            Registry.LocalMachine.OpenSubKey(name, false);

        /* ----------------------------------------------------------------- */
        ///
        /// Register
        ///
        /// <summary>
        /// Registers the specified port.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static void Register(string monitor, string port)
        {
            var xcv  = $",XcvMonitor {monitor}";
            var mask = AccessMask.ServerAccessAdminister.Create();
            if (!NativeMethods.OpenPrinter(xcv, out var h, ref mask)) throw new Win32Exception();

            var cn     = port + "\0";
            var size   = cn.Length * 2;
            var buffer = Marshal.AllocHGlobal(size);

            try
            {
                Marshal.Copy(cn.ToCharArray(), 0, buffer, cn.Length);
                if (!NativeMethods.XcvData(h, "AddPort", buffer, (uint)size, IntPtr.Zero, 0, out _, out var err))
                {
                    throw new Win32Exception((int)err);
                }
            }
            finally
            {
                NativeMethods.ClosePrinter(h);
                Marshal.FreeHGlobal(buffer);
            }
        }

        #endregion

        #region Fields
        private readonly Core _core;
        #endregion
    }
}
