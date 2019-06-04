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
using System.Runtime.Serialization;

namespace Cube.Pdf.Pinstaller
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
    public class DeviceConfig : SerializableBase
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
        /// PortMonitors
        ///
        /// <summary>
        /// Gets or sets the configuration of the installing port monitors.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public IList<PortMonitorConfig> PortMonitors
        {
            get => _portMonitors;
            set => SetProperty(ref _portMonitors, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Ports
        ///
        /// <summary>
        /// Gets or sets the configuration of the installing ports.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public IList<PortConfig> Ports
        {
            get => _ports;
            set => SetProperty(ref _ports, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// PrinterDrivers
        ///
        /// <summary>
        /// Gets or sets the configuration of the installing printer
        /// drivers.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public IList<PrinterDriverConfig> PrinterDrivers
        {
            get => _printerDrivers;
            set => SetProperty(ref _printerDrivers, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Printers
        ///
        /// <summary>
        /// Gets or sets the configuration of the installing printers.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public IList<PrinterConfig> Printers
        {
            get => _printers;
            set => SetProperty(ref _printers, value);
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
            _portMonitors   = new List<PortMonitorConfig>();
            _ports          = new List<PortConfig>();
            _printerDrivers = new List<PrinterDriverConfig>();
            _printers       = new List<PrinterConfig>();
        }

        #endregion

        #region Fields
        private IList<PortMonitorConfig> _portMonitors;
        private IList<PortConfig> _ports;
        private IList<PrinterDriverConfig> _printerDrivers;
        private IList<PrinterConfig> _printers;
        #endregion
    }
}
