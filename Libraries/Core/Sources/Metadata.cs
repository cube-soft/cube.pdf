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
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// Metadata
        ///
        /// <summary>
        /// Initializes a new instance of the Metadata class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Metadata() { Reset(); }

        #endregion

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
            get => GetProperty<PdfVersion>();
            set => SetProperty(value);
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
            get => GetProperty<string>();
            set => SetProperty(value);
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
            get => GetProperty<string>();
            set => SetProperty(value);
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
            get => GetProperty<string>();
            set => SetProperty(value);
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
            get => GetProperty<string>();
            set => SetProperty(value);
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
            get => GetProperty<string>();
            set => SetProperty(value);
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
            get => GetProperty<string>();
            set => SetProperty(value);
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
            get => GetProperty<ViewerOption>();
            set => SetProperty(value);
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
        private void OnDeserializing(StreamingContext context) => Reset();

        /* ----------------------------------------------------------------- */
        ///
        /// Reset
        ///
        /// <summary>
        /// Resets values.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Reset()
        {
            Version  = new PdfVersion(1, 7);
            Author   = string.Empty;
            Title    = string.Empty;
            Subject  = string.Empty;
            Keywords = string.Empty;
            Creator  = string.Empty;
            Producer = string.Empty;
            Options  = ViewerOption.OneColumn;
        }

        #endregion
    }
}
