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
        /// specified arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Printer(string name, string driver, string port)
        {
            Name        = name;
            DriverName  = driver;
            PortName    = port;
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
            get => _core.pPrinterName;
            private set => _core.pPrinterName = value;
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
            private set => _core.pDriverName = value;
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
            private set => _core.pPortName = value;
        }

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
        private PrinterInfo2 _core = new PrinterInfo2();
        #endregion
    }
}
