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
    /// Port
    ///
    /// <summary>
    /// Provides functionality to install or uninstall a port.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class Port : IInstaller
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
        public Port(string name, string monitor)
        {
            Name        = name;
            MonitorName = monitor;
            Environment = this.GetEnvironment();
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
        /// Name
        ///
        /// <summary>
        /// Gets the name of the port monitor.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string MonitorName { get; }

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

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Install
        ///
        /// <summary>
        /// Installs the port.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Install() { }

        /* ----------------------------------------------------------------- */
        ///
        /// Uninstall
        ///
        /// <summary>
        /// Uninstalls the port.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Uninstall() { }

        #endregion
    }
}
