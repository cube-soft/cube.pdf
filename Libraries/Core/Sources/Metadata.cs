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
    public class Metadata : ObservableProperty
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
        /// <remarks>
        /// 現時点で有効な PDF バージョンは 1.0, 1.1, 1.2, 1.3, 1.4, 1.5,
        /// 1.6, 1.7, 1.7 Extension Level 3, 1.7 Extension Level 5 の
        /// 10 種類です。Adobe Extension Level の値は Build プロパティで
        /// 保持する事とします。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public Version Version
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
        public string Producer
        {
            get => _producer;
            set => SetProperty(ref _producer, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ViewOption
        ///
        /// <summary>
        /// Gets or sets a value of display options.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ViewOption ViewOption
        {
            get => _option;
            set => SetProperty(ref _option, value);
        }

        #endregion

        #region Fields
        private Version _version = new Version(1, 2);
        private string _author = string.Empty;
        private string _title = string.Empty;
        private string _subject = string.Empty;
        private string _keywords = string.Empty;
        private string _creator = string.Empty;
        private string _producer = string.Empty;
        private ViewOption _option = ViewOption.PageOnly;
        #endregion
    }
}
