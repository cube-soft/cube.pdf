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
using Cube.FileSystem;

namespace Cube.Pdf
{
    /* --------------------------------------------------------------------- */
    ///
    /// Metadata
    ///
    /// <summary>
    /// Represents a metadata in the PDF document.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Serializable]
    [DataContract]
    public class Metadata : SerializableBase
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Version
        ///
        /// <summary>
        /// Gets or sets a version of the PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public PdfVersion Version
        {
            get => Get(() => new PdfVersion(1, 7));
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Author
        ///
        /// <summary>
        /// Gets or sets an author.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public string Author
        {
            get => Get(() => string.Empty);
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Title
        ///
        /// <summary>
        /// Gets or sets a title.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public string Title
        {
            get => Get(() => string.Empty);
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Subject
        ///
        /// <summary>
        /// Gets or sets a subject.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public string Subject
        {
            get => Get(() => string.Empty);
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Keywords
        ///
        /// <summary>
        /// Gets or sets a keywords.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public string Keywords
        {
            get => Get(() => string.Empty);
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Creator
        ///
        /// <summary>
        /// Gets or sets a name of program that creates the PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public string Creator
        {
            get => Get(() => string.Empty);
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Producer
        ///
        /// <summary>
        /// Gets or sets a name of program that creates the PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public string Producer
        {
            get => Get(() => string.Empty);
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Options
        ///
        /// <summary>
        /// Gets or sets a value of viewer options.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public ViewerOption Options
        {
            get => Get(() => ViewerOption.OneColumn);
            set => Set(value);
        }

        #endregion
    }
}
