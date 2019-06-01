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
            get => _version;
            set => SetProperty(ref _version, value);
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
            get => _author;
            set => SetProperty(ref _author, value);
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
            get => _title;
            set => SetProperty(ref _title, value);
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
            get => _subject;
            set => SetProperty(ref _subject, value);
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
            get => _keywords;
            set => SetProperty(ref _keywords, value);
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
            get => _creator;
            set => SetProperty(ref _creator, value);
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
            get => _producer;
            set => SetProperty(ref _producer, value);
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
        public ViewerOptions Options
        {
            get => _options;
            set => SetProperty(ref _options, value);
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
            _version  = new PdfVersion(1, 7);
            _author   = string.Empty;
            _title    = string.Empty;
            _subject  = string.Empty;
            _keywords = string.Empty;
            _creator  = string.Empty;
            _producer = string.Empty;
            _options  = ViewerOptions.OneColumn;
        }

        #endregion

        #region Fields
        private PdfVersion _version;
        private string _author;
        private string _title;
        private string _subject;
        private string _keywords;
        private string _creator;
        private string _producer;
        private ViewerOptions _options;
        #endregion
    }
}
