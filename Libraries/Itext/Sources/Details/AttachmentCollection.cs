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
using Cube.FileSystem;
using iTextSharp.text.pdf;
using System.Collections;
using System.Collections.Generic;

namespace Cube.Pdf.Itext
{
    /* --------------------------------------------------------------------- */
    ///
    /// AttachmentCollection
    ///
    /// <summary>
    /// Represents the collection of attached files.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal class AttachmentCollection : IEnumerable<Attachment>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// AttachmentCollection
        ///
        /// <summary>
        /// Initializes a new instance of the AttachmentCollection class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="core">PdfReader object.</param>
        /// <param name="file">Information of the PDF file.</param>
        /// <param name="io">I/O handler.</param>
        ///
        /* ----------------------------------------------------------------- */
        public AttachmentCollection(PdfReader core, PdfFile file, IO io)
        {
            File  = file;
            IO    = io;
            _core = core;

            Parse();
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// File
        ///
        /// <summary>
        /// Gets the file information of the PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected PdfFile File { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// IO
        ///
        /// <summary>
        /// Gets the I/O handler.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected IO IO { get; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetEnumerator
        ///
        /// <summary>
        /// Returns an enumerator that iterates through this collection.
        /// </summary>
        ///
        /// <returns>
        /// An IEnumerator(Attachment) object for this collection.
        /// </returns>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerator<Attachment> GetEnumerator() => _inner.GetEnumerator();

        /* ----------------------------------------------------------------- */
        ///
        /// IEnumerable.GetEnumerator
        ///
        /// <summary>
        /// Returns an enumerator that iterates through this collection.
        /// </summary>
        ///
        /// <returns>
        /// An IEnumerator object for this collection.
        /// </returns>
        ///
        /* ----------------------------------------------------------------- */
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Parse
        ///
        /// <summary>
        /// Gets the attachment objects from the PdfReader.
        /// </summary>
        ///
        /// <remarks>
        /// /EmbededFiles, /Names で取得できる配列は、以下のような構造に
        /// なっています。
        ///
        /// [String, Object, String, Object, ...]
        ///
        /// この内、各 Object が、添付ファイルに関する実際の情報を保持
        /// しています。そのため、間の String 情報をスキップする必要が
        /// あります。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private void Parse()
        {
            if (_core == null) return;

            if (!(PdfReader.GetPdfObject(_core.Catalog.Get(PdfName.NAMES)) is PdfDictionary c)) return;
            if (!(PdfReader.GetPdfObject(c.Get(PdfName.EMBEDDEDFILES)) is PdfDictionary e)) return;

            var files = e.GetAsArray(PdfName.NAMES);
            if (files == null) return;

            for (var i = 1; i < files.Size; i += 2) // see remarks
            {
                var item = files.GetAsDict(i);
                var name = item.GetAsString(PdfName.UF)?.ToUnicodeString();
                if (string.IsNullOrEmpty(name)) name = item.GetAsString(PdfName.F)?.ToString();
                if (string.IsNullOrEmpty(name)) continue;

                var ef = item.GetAsDict(PdfName.EF);
                if (ef == null) continue;

                foreach (var key in ef.Keys)
                {
                    if (!(PdfReader.GetPdfObject(ef.GetAsIndirectObject(key)) is PRStream stream)) continue;
                    _inner.Add(new EmbeddedAttachment(name, File.FullName, IO, stream));
                    break;
                }
            }
        }

        #endregion

        #region Fields
        private readonly PdfReader _core;
        private readonly IList<Attachment> _inner = new List<Attachment>();
        #endregion
    }
}
