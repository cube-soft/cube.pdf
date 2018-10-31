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
    /// Printer
    ///
    /// <summary>
    /// Provides functionality to install or uninstall a printer.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class Printer : IInstaller
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// Printer
        ///
        /// <summary>
        /// Initializes a new instance of the Printer class with the
        /// specified name.
        /// </summary>
        ///
        /// <param name="name">Printer name.</param>
        ///
        /* ----------------------------------------------------------------- */
        public Printer(string name)
        {
            Name        = name;
            ShareName   = name;
            Environment = this.GetEnvironment();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Printer
        ///
        /// <summary>
        /// Initializes a new instance of the Printer class with the
        /// specified arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private Printer(PrinterInfo2 core) { _core = core; }

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
            get => _core.pPrinterName;
            private set => _core.pPrinterName = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ShareName
        ///
        /// <summary>
        /// Gets or sets the name that identifies the share point for the
        /// printer.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string ShareName
        {
            get => _core.pShareName;
            set => _core.pShareName = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// DriverName
        ///
        /// <summary>
        /// Gets the name of the printer driver.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string DriverName
        {
            get => _core.pDriverName;
            set => _core.pDriverName = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// PortName
        ///
        /// <summary>
        /// Gets the port name.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string PortName
        {
            get => _core.pPortName;
            set => _core.pPortName = value;
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

        #region Fields
        private PrinterInfo2 _core;
        #endregion
    }
}
