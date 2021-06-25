/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
/* ------------------------------------------------------------------------- */
using System.Security.Cryptography;
using Cube.FileSystem;
using iTextSharp.text.pdf;

namespace Cube.Pdf.Itext
{
    /* --------------------------------------------------------------------- */
    ///
    /// EmbeddedAttachment
    ///
    /// <summary>
    /// Represents an file attached to a PDF document.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal class EmbeddedAttachment : Attachment
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// Attachment
        ///
        /// <summary>
        /// Initializes a new instance of the Attachment class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="io">I/O handler.</param>
        /// <param name="name">Name of attached file.</param>
        /// <param name="src">Path of the PDF document.</param>
        /// <param name="core">Core object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public EmbeddedAttachment(string name, string src, IO io, PRStream core) :
            base(name, src, io)
        {
            _core = core;
        }

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
        protected override long GetLength() =>
            _core?.GetAsDict(PdfName.PARAMS)?.GetAsNumber(PdfName.SIZE)?.LongValue ?? 0;

        /* ----------------------------------------------------------------- */
        ///
        /// GetData
        ///
        /// <summary>
        /// Gets the data of the attached file in byte unit.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override byte[] GetData() => PdfReader.GetStreamBytes(_core);

        /* ----------------------------------------------------------------- */
        ///
        /// GetChecksum
        ///
        /// <summary>
        /// Gets the checksum of attached file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override byte[] GetChecksum() =>
            new SHA256CryptoServiceProvider().ComputeHash(Data);

        #endregion

        #region Fields
        private readonly PRStream _core;
        #endregion
    }
}
