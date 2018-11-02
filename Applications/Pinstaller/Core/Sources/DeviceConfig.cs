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
using System.Runtime.Serialization;

namespace Cube.Pdf.App.Pinstaller
{
    /* --------------------------------------------------------------------- */
    ///
    /// DeviceConfig
    ///
    /// <summary>
    /// Represents the configuration to install devices.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataContract]
    public class DeviceConfig : ObservableProperty
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// DeviceConfig
        ///
        /// <summary>
        /// Initializes a new instance of the DeviceConfig class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public DeviceConfig() { Reset(); }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// PortMonitor
        ///
        /// <summary>
        /// Gets or sets the configuration of the installing port monitor.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public PortMonitorConfig PortMonitor
        {
            get => _portMonitor;
            set => SetProperty(ref _portMonitor, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Port
        ///
        /// <summary>
        /// Gets or sets the configuration of the installing port.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public PortConfig Port
        {
            get => _port;
            set => SetProperty(ref _port, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// PrinterDriver
        ///
        /// <summary>
        /// Gets or sets the configuration of the installing printer
        /// driver.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public PrinterDriverConfig PrinterDriver
        {
            get => _printerDriver;
            set => SetProperty(ref _printerDriver, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Printer
        ///
        /// <summary>
        /// Gets or sets the configuration of the installing printer.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public PrinterConfig Printer
        {
            get => _printer;
            set => SetProperty(ref _printer, value);
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// OnDeserializing
        ///
        /// <summary>
        /// Occurs bofore deserializing.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [OnDeserializing]
        private void OnDeserializing(StreamingContext context) => Reset();

        /* ----------------------------------------------------------------- */
        ///
        /// Reset
        ///
        /// <summary>
        /// Resets values of fields.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Reset()
        {
            _portMonitor   = new PortMonitorConfig();
            _port          = new PortConfig();
            _printerDriver = new PrinterDriverConfig();
            _printer       = new PrinterConfig();
        }

        #endregion

        #region Fields
        private PortMonitorConfig _portMonitor;
        private PortConfig _port;
        private PrinterDriverConfig _printerDriver;
        private PrinterConfig _printer;
        #endregion
    }
}
