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
using System.Drawing;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// DocumentFolder
    ///
    /// <summary>
    /// Represents a collection of PDF documents.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class DocumentFolder
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// DocumentFolder
        ///
        /// <summary>
        /// Initializes a new instance of the DocumentFolder class with
        /// the specified arguments.
        /// </summary>
        ///
        /// <param name="io">I/O handler.</param>
        /// <param name="query">Function to get the password query.</param>
        ///
        /* ----------------------------------------------------------------- */
        public DocumentFolder(IO io, Func<IQuery<string>> query)
        {
            _io    = io;
            _query = query;
        }

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
        public DocumentRenderer GetOrAdd(string src) => GetOrAdd(src, string.Empty);

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
        public DocumentRenderer GetOrAdd(string src, string password)
        {
            if (!src.IsPdf()) return null;
            if (_inner.TryGetValue(src, out var value)) return value;
            return _inner.GetOrAdd(src, e => Create(e, password));
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

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Creates a new instance of the DocumentReader class with the
        /// specified arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private DocumentRenderer Create(string src, string password)
        {
            var opt  = new OpenOption { IO = _io, FullAccess = true };
            var dest = password.HasValue() ?
                       new DocumentRenderer(src, password, opt) :
                       new DocumentRenderer(src, _query(), opt);

            dest.RenderOption.Background = Color.White;
            return dest;
        }

        #endregion

        #region Fields
        private readonly IO _io;
        private readonly Func<IQuery<string>> _query;
        private readonly ConcurrentDictionary<string, DocumentRenderer> _inner =
            new ConcurrentDictionary<string, DocumentRenderer>();
        #endregion
    }
}
