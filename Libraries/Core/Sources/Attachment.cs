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
using Cube.FileSystem;
using System.Security.Cryptography;

namespace Cube.Pdf
{
    /* --------------------------------------------------------------------- */
    ///
    /// Attachment
    ///
    /// <summary>
    /// Represents an attachment file in the PDF document.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class Attachment
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// Attachment
        ///
        /// <summary>
        /// Initializes a new instance of the Attachment class with the
        /// specified file path.
        /// </summary>
        ///
        /// <param name="path">Path of the attached file.</param>
        ///
        /* ----------------------------------------------------------------- */
        public Attachment(string path) : this(path, new IO()) { }

        /* ----------------------------------------------------------------- */
        ///
        /// Attachment
        ///
        /// <summary>
        /// Initializes a new instance of the Attachment class with the
        /// specified parameters.
        /// </summary>
        ///
        /// <param name="name">Display name.</param>
        /// <param name="path">Path of the attached file.</param>
        ///
        /* ----------------------------------------------------------------- */
        public Attachment(string name, string path) : this(name, path, new IO()) { }

        /* ----------------------------------------------------------------- */
        ///
        /// Attachment
        ///
        /// <summary>
        /// Initializes a new instance of the Attachment class with the
        /// specified parameters.
        /// </summary>
        ///
        /// <param name="path">Path of the attached file.</param>
        /// <param name="io">I/O handler.</param>
        ///
        /* ----------------------------------------------------------------- */
        public Attachment(string path, IO io) : this(io.Get(path).Name, path, io) { }

        /* ----------------------------------------------------------------- */
        ///
        /// Attachment
        ///
        /// <summary>
        /// Initializes a new instance of the Attachment class with the
        /// specified parameters.
        /// </summary>
        ///
        /// <param name="name">Display name.</param>
        /// <param name="path">Path of the attached file.</param>
        /// <param name="io">I/O handler.</param>
        ///
        /* ----------------------------------------------------------------- */
        public Attachment(string name, string path, IO io)
        {
            Name   = name;
            Source = path;
            IO     = io;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// IO
        ///
        /// <summary>
        /// Gets the I/O handler.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IO IO { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Name
        ///
        /// <summary>
        /// Gets the displayed name of the attached file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Name { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Source
        ///
        /// <summary>
        /// Gets the path of the attached file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Source { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Length
        ///
        /// <summary>
        /// Gets the data length of the attached file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public long Length => GetLength();

        /* ----------------------------------------------------------------- */
        ///
        /// Data
        ///
        /// <summary>
        /// Gets the data of the attached file in byte unit.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public byte[] Data => _data ?? (_data = GetData());

        /* ----------------------------------------------------------------- */
        ///
        /// Checksum
        ///
        /// <summary>
        /// Gets the checksum of attached file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public byte[] Checksum => _checksum ?? (_checksum = GetChecksum());

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetLength
        ///
        /// <summary>
        /// Gets the data length of the attached file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual long GetLength() => IO.Get(Source)?.Length ?? 0;

        /* ----------------------------------------------------------------- */
        ///
        /// GetBytes
        ///
        /// <summary>
        /// Gets the data of the attached file in byte unit.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual byte[] GetData()
        {
            if (!IO.Exists(Source)) return null;

            using (var src  = IO.OpenRead(Source))
            using (var dest = new System.IO.MemoryStream())
            {
                src.CopyTo(dest);
                return dest.ToArray();
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetChecksum
        ///
        /// <summary>
        /// Gets the checksum of attached file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual byte[] GetChecksum()
        {
            if (!IO.Exists(Source)) return null;
            using (var ss = IO.OpenRead(Source))
            {
                return new SHA256CryptoServiceProvider().ComputeHash(ss);
            }
        }

        #endregion

        #region Fields
        private byte[] _data;
        private byte[] _checksum;
        #endregion
    }
}
