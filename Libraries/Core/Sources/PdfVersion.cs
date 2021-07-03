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
using System.Runtime.Serialization;
using System.Text;
using Cube.Mixin.String;

namespace Cube.Pdf
{
    /* --------------------------------------------------------------------- */
    ///
    /// PdfVersion
    ///
    /// <summary>
    /// Represents PDF version.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Serializable]
    [DataContract]
    public class PdfVersion
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// PdfVersion
        ///
        /// <summary>
        /// Initializes a new instance of the PdfVersion class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PdfVersion() : this(1, 0) { }

        /* ----------------------------------------------------------------- */
        ///
        /// PdfVersion
        ///
        /// <summary>
        /// Initializes a new instance of the PdfVersion class with the
        /// specified values.
        /// </summary>
        ///
        /// <param name="major">Major version.</param>
        /// <param name="minor">Minor version.</param>
        ///
        /* ----------------------------------------------------------------- */
        public PdfVersion(int major, int minor) : this(major, minor, 0) { }

        /* ----------------------------------------------------------------- */
        ///
        /// PdfVersion
        ///
        /// <summary>
        /// Initializes a new instance of the PdfVersion class with the
        /// specified values.
        /// </summary>
        ///
        /// <param name="major">Major version.</param>
        /// <param name="minor">Minor version.</param>
        /// <param name="extension">Extension level.</param>
        ///
        /* ----------------------------------------------------------------- */
        public PdfVersion(int major, int minor, int extension) :
            this(string.Empty, major, minor, extension) { }

        /* ----------------------------------------------------------------- */
        ///
        /// PdfVersion
        ///
        /// <summary>
        /// Initializes a new instance of the PdfVersion class with the
        /// specified values.
        /// </summary>
        ///
        /// <param name="subset">PDF subset name.</param>
        /// <param name="major">Major version.</param>
        /// <param name="minor">Minor version.</param>
        /// <param name="extension">Extension level.</param>
        ///
        /* ----------------------------------------------------------------- */
        public PdfVersion(string subset, int major, int minor, int extension)
        {
            Subset         = subset;
            Major          = major;
            Minor          = minor;
            ExtensionLevel = extension;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Subset
        ///
        /// <summary>
        /// Gets or sets a value that represents PDF subset.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public string Subset { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Major
        ///
        /// <summary>
        /// Gets or sets the major number of PDF version.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public int Major { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Minor
        ///
        /// <summary>
        /// Gets or sets the minor number of PDF version.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public int Minor { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// ExtensionLevel
        ///
        /// <summary>
        /// Gets or sets the extension level of PDF version.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public int ExtensionLevel { get; set; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// ToString
        ///
        /// <summary>
        /// Gets a description of PDF version.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public override string ToString()
        {
            var sb = new StringBuilder("PDF");
            if (Subset.HasValue()) _ = sb.Append($"/{Subset}");
            _ = sb.Append(" ");
            _ = sb.Append($"{Major}.{Minor}");
            if (ExtensionLevel > 0) _ = sb.Append($" {nameof(ExtensionLevel)} {ExtensionLevel}");
            return sb.ToString();
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// OnDeserializing
        ///
        /// <summary>
        /// Occurs before deserializing.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [OnDeserializing]
        private void OnDeserializing(StreamingContext context)
        {
            Major          = 1;
            Minor          = 0;
            ExtensionLevel = 0;
            Subset         = string.Empty;
        }

        #endregion
    }
}
