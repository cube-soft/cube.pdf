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
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// Selection(T)
    ///
    /// <summary>
    /// Represents the selection of items.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class Selection<T> : ObservableBase, IEnumerable<T>
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Count
        ///
        /// <summary>
        /// Gets the number of selected items.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Count => RawObject.Count;

        /* ----------------------------------------------------------------- */
        ///
        /// RawObject
        ///
        /// <summary>
        /// Gets the raw object of the collection.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected ConcurrentDictionary<T, byte> RawObject { get; } = new ConcurrentDictionary<T, byte>();

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Add
        ///
        /// <summary>
        /// Adds the specified image to the selection list.
        /// </summary>
        ///
        /// <param name="src">Image entry.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Add(T src)
        {
            if (RawObject.TryAdd(src, 0)) Refresh(nameof(Count));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Remove
        ///
        /// <summary>
        /// Removes the specified image from the selection list.
        /// </summary>
        ///
        /// <param name="src">Image entry.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Remove(T src)
        {
            if (RawObject.TryRemove(src, out _)) Refresh(nameof(Count));
        }

        #region IEnumerable<T>

        /* ----------------------------------------------------------------- */
        ///
        /// GetEnumerator
        ///
        /// <summary>
        /// Returns an enumerator that iterates through this collection.
        /// </summary>
        ///
        /// <returns>
        /// An IEnumerator(T) object for this collection.
        /// </returns>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerator<T> GetEnumerator() => RawObject.Keys.GetEnumerator();

        /* ----------------------------------------------------------------- */
        ///
        /// IEnumerable.GetEnumerator
        ///
        /// <summary>
        /// Returns an enumerator that iterates through this collection.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

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
    }
}
