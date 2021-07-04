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
using System.Collections.Generic;
using Cube.Collections;
using iText.Kernel.Pdf;

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
    internal class AttachmentCollection : EnumerableBase<Attachment>
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
        /// <param name="core">iText object.</param>
        /// <param name="file">Information of the PDF file.</param>
        ///
        /* ----------------------------------------------------------------- */
        public AttachmentCollection(PdfDocument core, PdfFile file)
        {
            File  = file;
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
        public override IEnumerator<Attachment> GetEnumerator() => _inner.GetEnumerator();

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases the unmanaged resources used by the object and
        /// optionally releases the managed resources.
        /// </summary>
        ///
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        protected override void Dispose(bool disposing) { }

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
            var root = _core.GetCatalog().GetPdfObject();
            var names = root.GetAsDictionary(PdfName.Names);
            var files = root.GetAsDictionary(PdfName.EmbeddedFiles);
            if (names == null || files == null) return;

            var items = files.GetAsArray(PdfName.Names);
            if (items == null) return;

            for (var i = 1; i < items.Size(); i += 2) // see remarks
            {
                var cur  = items.GetAsDictionary(i);
                var name = cur.GetAsString(PdfName.UF) ?? cur.GetAsString(PdfName.F);
                var dic  = cur.GetAsDictionary(PdfName.EF);
                if (name == null || dic == null) continue;

                foreach (var key in dic.KeySet())
                {
                    var stream = dic.GetAsStream(key);
                    if (stream is null) continue;
                    _inner.Add(new EmbeddedAttachment(name.ToUnicodeString(), File.FullName, stream));
                    break;
                }
            }
        }

        #endregion

        #region Fields
        private readonly PdfDocument _core;
        private readonly List<Attachment> _inner = new();
        #endregion
    }
}
