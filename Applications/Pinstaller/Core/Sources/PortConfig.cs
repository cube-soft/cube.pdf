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
    /// PortConfig
    ///
    /// <summary>
    /// Represents the configuration to install a port.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataContract]
    public class PortConfig : ObservableProperty
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// PortConfig
        ///
        /// <summary>
        /// Initializes a new instance of the PortConfig class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PortConfig() { Reset(); }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Name
        ///
        /// <summary>
        /// Gets or sets the port name.
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
        /// Name
        ///
        /// <summary>
        /// Gets the name of the port monitor.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public string MonitorName
        {
            get => _monitorName;
            set => SetProperty(ref _monitorName, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Application
        ///
        /// <summary>
        /// Gets or sets the application path that the port executes.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public string Application
        {
            get => _application;
            set => SetProperty(ref _application, value);
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
        [DataMember]
        public string Arguments
        {
            get => _arguments;
            set => SetProperty(ref _arguments, value);
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
        [DataMember]
        public string Temp
        {
            get => _temp;
            set => SetProperty(ref _temp, value);
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
        [DataMember]
        public bool WaitForExit
        {
            get => _wait;
            set => SetProperty(ref _wait, value);
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
            _name        = string.Empty;
            _monitorName = string.Empty;
            _application = string.Empty;
            _arguments   = string.Empty;
            _temp        = string.Empty;
            _wait        = false;
        }

        #endregion

        #region Fields
        private string _name;
        private string _monitorName;
        private string _application;
        private string _arguments;
        private string _temp;
        private bool _wait;
        #endregion
    }
}
