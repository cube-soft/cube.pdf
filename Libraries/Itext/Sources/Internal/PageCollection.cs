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
using System.Collections;
using System.Collections.Generic;
using Cube.Collections;
using iText.Kernel.Pdf;

namespace Cube.Pdf.Itext
{
    /* --------------------------------------------------------------------- */
    ///
    /// PageCollection
    ///
    /// <summary>
    /// Represents a read only collection of PDF pages.
    /// </summary>
    ///
    /// <remarks>
    /// IReadOnlyList(Page) implementations is for the GetPage extended
    /// method.
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    internal class PageCollection : EnumerableBase<Page2>, IReadOnlyList<Page2>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// PageCollection
        ///
        /// <summary>
        /// Initializes a new instance of the PageCollection class with
        /// the specified arguments.
        /// </summary>
        ///
        /// <param name="core">iText object.</param>
        /// <param name="file">PDF file information.</param>
        ///
        /* ----------------------------------------------------------------- */
        public PageCollection(PdfDocument core, PdfFile file)
        {
            File = file;
            _core = core;
            _cache = new((int)(file.Count / 0.72) + 1);
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
        public int Count => _core?.GetNumberOfPages() ?? 0;

        /* ----------------------------------------------------------------- */
        ///
        /// Item[int]
        ///
        /// <summary>
        /// Gets the Page object corresponding the specified index.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Page2 this[int index]
        {
            get
            {
                if (!_cache.ContainsKey(index))
                {
                    lock (_cache.SyncRoot)
                    {
                        _cache[index] = _core.GetPage(File, index + 1);
                    }
                }
                return new((Page)_cache[index]);
            }
        }

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
        public override IEnumerator<Page2> GetEnumerator()
        {
            for (var i = 0; i < Count; ++i) yield return this[i];
        }

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

        #region Fields
        private readonly PdfDocument _core;
        private readonly Hashtable _cache;
        #endregion
    }
}
