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
using Cube.Mixin.String;
using Cube.Pdf.Pdfium;
using System;
using System.Collections.Concurrent;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// DocumentCollection
    ///
    /// <summary>
    /// Represents a collection of PDF documents.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal class DocumentCollection
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// DocumentCollection
        ///
        /// <summary>
        /// Initializes a new instance of the DocumentCollection class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="io">I/O handler.</param>
        /// <param name="query">Function to get the password query.</param>
        ///
        /* ----------------------------------------------------------------- */
        public DocumentCollection(IO io, Func<IQuery<string>> query)
        {
            IO = io;
            _query = query;
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

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetOrAdd
        ///
        /// <summary>
        /// Adds a DocumentReader if the specified path does not already
        /// exist.
        /// </summary>
        ///
        /// <param name="src">File path.</param>
        ///
        /// <returns>DocumentReader object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public DocumentReader GetOrAdd(string src) => GetOrAdd(src, string.Empty);

        /* ----------------------------------------------------------------- */
        ///
        /// GetOrAdd
        ///
        /// <summary>
        /// Adds a DocumentReader if the specified path does not already
        /// exist.
        /// </summary>
        ///
        /// <param name="src">File path.</param>
        /// <param name="password">Password of the source.</param>
        ///
        /// <returns>DocumentReader object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public DocumentReader GetOrAdd(string src, string password)
        {
            if (!src.IsPdf()) return null;
            if (_inner.TryGetValue(src, out var value)) return value;

            var dest = _inner.GetOrAdd(src, e =>
                password.HasValue() ?
                new DocumentReader(e, password, IO) :
                new DocumentReader(e, _query(), true, IO)
            );
            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Clear
        ///
        /// <summary>
        /// Removes all of the DocumentReader objects.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Clear()
        {
            foreach (var kv in _inner) kv.Value.Dispose();
            _inner.Clear();
        }

        #endregion

        #region Fields
        private readonly Func<IQuery<string>> _query;
        private readonly ConcurrentDictionary<string, DocumentReader> _inner =
            new ConcurrentDictionary<string, DocumentReader>();
        #endregion
    }
}
