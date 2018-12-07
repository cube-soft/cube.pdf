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
using Cube.Collections;
using iTextSharp.text.pdf;
using System.Collections.Generic;

namespace Cube.Pdf.Itext
{
    /* --------------------------------------------------------------------- */
    ///
    /// ReadOnlyPageList
    ///
    /// <summary>
    /// Represents a read only collection of PDF pages.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal class ReadOnlyPageList : EnumerableBase<Page>, IReadOnlyList<Page>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ReadOnlyPageList
        ///
        /// <summary>
        /// Initializes a new instance of the ReadOnlyPageList class with
        /// the specified arguments.
        /// </summary>
        ///
        /// <param name="core">PdfReader object.</param>
        /// <param name="file">PDF file information.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ReadOnlyPageList(PdfReader core, PdfFile file)
        {
            File  = file;
            _core = core;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// File
        ///
        /// <summary>
        /// Gets the PDF file information.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PdfFile File { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Count
        ///
        /// <summary>
        /// Gets the number of pages.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Count => _core?.NumberOfPages ?? 0;

        /* ----------------------------------------------------------------- */
        ///
        /// Item[int]
        ///
        /// <summary>
        /// Gets the Page object corresponding the specified index.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Page this[int index] => _core.GetPage(File, index + 1);

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
        /// An IEnumerator(Page) object for this collection.
        /// </returns>
        ///
        /* ----------------------------------------------------------------- */
        public override IEnumerator<Page> GetEnumerator()
        {
            for (var i = 0; i < Count; ++i) yield return this[i];
        }

        #endregion

        #region Fields
        private readonly PdfReader _core;
        #endregion
    }
}
