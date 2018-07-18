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
using Cube.Pdf.Pdfium;
using System;
using System.Collections.Concurrent;

namespace Cube.Pdf.App.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// DocumentCollection
    ///
    /// <summary>
    /// Represents a colletion of PDF documents.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class DocumentCollection
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// DocumentCollection
        ///
        /// <summary>
        /// Initializes a new instance of the DocumentCollection class
        /// with the specified parameters.
        /// </summary>
        ///
        /// <param name="updated">
        /// Called after PDF documents is added or removed.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        public DocumentCollection(Action<DocumentCollection> updated)
        {
            _callback = updated;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Count
        ///
        /// <summary>
        /// Gets the number of DocumentReader objects contained in this
        /// class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Count => _core.Count;

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Get
        ///
        /// <summary>
        /// Gets the DocumentReader object of the specified file path.
        /// </summary>
        ///
        /// <param name="src">File path.</param>
        ///
        /// <returns>DocumentReader object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public DocumentReader Get(string src) =>
            _core.TryGetValue(src, out var dest) ? dest : null;

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
        public DocumentReader GetOrAdd(string src)
        {
            var created = false;
            var dest = _core.GetOrAdd(src, e =>
            {
                created = true;
                return new DocumentReader(e);
            });
            if (created) _callback(this);
            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Remove
        ///
        /// <summary>
        /// Attempts to remove the DocumentReader of the specified file path.
        /// </summary>
        ///
        /// <param name="src">File path.</param>
        ///
        /// <returns>
        /// true if the object was removed successfully; otherwise, false.
        /// </returns>
        ///
        /* ----------------------------------------------------------------- */
        public bool Remove(string src)
        {
            var dest = _core.TryRemove(src, out var removed);
            if (dest)
            {
                removed.Dispose();
                _callback(this);
            }
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
            foreach (var kv in _core) kv.Value.Dispose();
            _core.Clear();
            _callback(this);
        }

        #endregion

        #region Fields
        private readonly ConcurrentDictionary<string, DocumentReader> _core = new ConcurrentDictionary<string, DocumentReader>();
        private readonly Action<DocumentCollection> _callback;
        #endregion
    }
}
