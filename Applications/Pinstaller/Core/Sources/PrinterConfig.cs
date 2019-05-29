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

namespace Cube.Pdf.Pinstaller
{
    /* --------------------------------------------------------------------- */
    ///
    /// PrinterConfig
    ///
    /// <summary>
    /// Represents the configuration to install a printer.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataContract]
    public class PrinterConfig : ObservableBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// PrinterConfig
        ///
        /// <summary>
        /// Initializes a new instance of the PrinterConfig class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PrinterConfig() { Reset(); }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Name
        ///
        /// <summary>
        /// Gets or sets the name of the printer.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
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
        [DataMember]
        public string ShareName
        {
            get => _shareName;
            set => SetProperty(ref _shareName, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// DriverName
        ///
        /// <summary>
        /// Gets or sets the name of the printer driver.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public string DriverName
        {
            get => _driverName;
            set => SetProperty(ref _driverName, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// PortName
        ///
        /// <summary>
        /// Gets or sets the port name.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public string PortName
        {
            get => _portName;
            set => SetProperty(ref _portName, value);
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
            _name       = string.Empty;
            _shareName  = string.Empty;
            _driverName = string.Empty;
            _portName   = string.Empty;
        }

        #endregion

        #region Fields
        private string _name;
        private string _shareName;
        private string _driverName;
        private string _portName;
        #endregion
    }
}
