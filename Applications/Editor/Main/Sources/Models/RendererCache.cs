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
using System.Collections.Concurrent;
using System.Drawing;
using Cube.Pdf.Pdfium;
using Cube.Text.Extensions;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// RendererCache
    ///
    /// <summary>
    /// Represents a cache collection of renderer objects.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class RendererCache : DisposableBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// RendererCache
        ///
        /// <summary>
        /// Initializes a new instance of the RendererCache class with
        /// the specified arguments.
        /// </summary>
        ///
        /// <param name="query">Function to get the password query.</param>
        ///
        /* ----------------------------------------------------------------- */
        public RendererCache(Func<IQuery<string>> query) => _query = query;

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
        /// <returns>IDocumentReader object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public IDocumentRenderer GetOrAdd(string src) => GetOrAdd(src, string.Empty);

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
        /// <returns>IDocumentReader object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public IDocumentRenderer GetOrAdd(string src, string password)
        {
            if (Disposed) return null;
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
            foreach (var kv in _inner)
            {
                if (kv.Value is IDisposable e) e.Dispose();
            }
            _inner.Clear();
        }

        #endregion

        #region Implementations

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
        protected override void Dispose(bool disposing)
        {
            if (disposing) Clear();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Creates a new instance of the DocumentRenderer class with the
        /// specified arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private IDocumentRenderer Create(string src, string password) =>
            src.IsImageFile() ?
            CreateImageRenderer(src, password) :
            CreateDocumentRenderer(src, password);

        /* ----------------------------------------------------------------- */
        ///
        /// CreateDocumentRenderer
        ///
        /// <summary>
        /// Creates a new instance of the DocumentRenderer class with the
        /// specified arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private DocumentRenderer CreateDocumentRenderer(string src, string password)
        {
            var opt  = new OpenOption { FullAccess = true };
            var dest = password.HasValue() ?
                       new DocumentRenderer(src, password, opt) :
                       new DocumentRenderer(src, _query(), opt);

            dest.RenderOption.Background = Color.White;
            dest.RenderOption.Annotation = true;
            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CreateImageRenderer
        ///
        /// <summary>
        /// Creates a new instance of the ImageRenderer class with the
        /// specified arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ImageRenderer CreateImageRenderer(string src, string password) => new();

        #endregion

        #region Fields
        private readonly Func<IQuery<string>> _query;
        private readonly ConcurrentDictionary<string, IDocumentRenderer> _inner = new();
        #endregion
    }
}
