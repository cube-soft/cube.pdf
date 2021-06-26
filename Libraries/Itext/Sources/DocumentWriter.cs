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
using System;

namespace Cube.Pdf.Itext
{
    /* --------------------------------------------------------------------- */
    ///
    /// DocumentWriter
    ///
    /// <summary>
    /// Provides functionality to create or modify a PDF document.
    /// </summary>
    ///
    /// <remarks>
    /// DocumentWriter realizes the page rotation information
    /// (Page.Rotation.Delta) by modifying the internal object of
    /// DocumentReader. However, if DocumentReader is generated with
    /// OpenOption.ReduceMemory enabled, this change will be disabled and
    /// the result of the page rotation change cannot be reflected.
    /// If you have rotated the page, please set the corresponding option
    /// to disabled.
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public class DocumentWriter : DocumentWriterBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// DocumentWriter
        ///
        /// <summary>
        /// Initializes a new instance of the DocumentWriter class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public DocumentWriter() : this(new()) { }

        /* ----------------------------------------------------------------- */
        ///
        /// DocumentWriter
        ///
        /// <summary>
        /// Initializes a new instance of the DocumentWriter class with
        /// the specified options.
        /// </summary>
        ///
        /// <param name="options">Saving options.</param>
        ///
        /* ----------------------------------------------------------------- */
        public DocumentWriter(SaveOption options) : base(options) { }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// OnSave
        ///
        /// <summary>
        /// Executes the save operation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnSave(string path)
        {
            var dir = Options.IO.Get(path).DirectoryName;
            var tmp = Options.IO.Combine(dir, Guid.NewGuid().ToString("D"));

            try
            {
                var bk = new Bookmark();
                Merge(tmp, bk);
                Release();
                Writer.Stamp(path, Options, tmp, Metadata, Encryption, bk);
            }
            catch (Exception err) { throw err.Convert(); }
            finally
            {
                _ = Options.IO.TryDelete(tmp);
                Reset();
            }
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Merge
        ///
        /// <summary>
        /// Merges pages and save the document to the specified path.
        /// </summary>
        ///
        /// <remarks>
        /// To completely copy the page contents, including annotations,
        /// use the PdfCopy class to merge all pages together.
        /// PDF metadata and encryption settings will be added to the
        /// generated PDF.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private void Merge(string dest, Bookmark bookmark)
        {
            var e = Writer.Create(dest, Options, Metadata);

            e.Document.Open();

            foreach (var page in Pages)
            {
                var reader = Reader.From(GetRawReader(page));
                e.Writer.Set(reader, page, bookmark);
            }

            e.Writer.Set(Attachments);
            e.Close();
        }

        #endregion
    }
}
