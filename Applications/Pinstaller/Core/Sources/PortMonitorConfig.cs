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
    /// PortMonitorConfig
    ///
    /// <summary>
    /// Represents the configuration to install a port monitor.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataContract]
    public class PortMonitorConfig : ObservableProperty
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// PortMonitorConfig
        ///
        /// <summary>
        /// Initializes a new instance of the PortMonitorConfig class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PortMonitorConfig() { Reset(); }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Name
        ///
        /// <summary>
        /// Gets or sets the name of the port monitor.
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
        /// FileName
        ///
        /// <summary>
        /// Gets or sets the name of the port monitor file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public string FileName
        {
            get => _fileName;
            set => SetProperty(ref _fileName, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Config
        ///
        /// <summary>
        /// Gets or sets the name of UI config file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public string Config
        {
            get => _config;
            set => SetProperty(ref _config, value);
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
            _name     = string.Empty;
            _fileName = string.Empty;
            _config   = string.Empty;
        }

        #endregion

        #region Fields
        private string _name;
        private string _fileName;
        private string _config;
        #endregion
    }
}
