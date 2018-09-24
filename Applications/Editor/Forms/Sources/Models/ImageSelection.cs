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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Cube.Pdf.App.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// ImageSelection
    ///
    /// <summary>
    /// Represents the selection of images.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ImageSelection : ObservableProperty
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Count
        ///
        /// <summary>
        /// Gets the number of selected images.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Count => _selection.Count;

        /* ----------------------------------------------------------------- */
        ///
        /// First
        ///
        /// <summary>
        /// Gets the first index that is maximum value in the selected
        /// images.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int First => _selection.Keys.OrderBy(i => i.Index).FirstOrDefault()?.Index ?? -1;

        /* ----------------------------------------------------------------- */
        ///
        /// Last
        ///
        /// <summary>
        /// Gets the last index that is maximum value in the selected
        /// images.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Last => _selection.Keys.OrderByDescending(i => i.Index).FirstOrDefault()?.Index ?? -1;

        /* ----------------------------------------------------------------- */
        ///
        /// Indices
        ///
        /// <summary>
        /// Gets the indices of the selected images.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<int> Indices => _selection.Keys.Select(e => e.Index);

        /* ----------------------------------------------------------------- */
        ///
        /// Items
        ///
        /// <summary>
        /// Gets the selection of images.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<ImageItem> Items => _selection.Keys;

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
        public void Add(ImageItem src)
        {
            if (_selection.TryAdd(src, 0)) RaiseEvents();
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
        public void Remove(ImageItem src)
        {
            if (_selection.TryRemove(src, out var _)) RaiseEvents();
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// RaiseEvents
        ///
        /// <summary>
        /// Raises some events.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void RaiseEvents()
        {
            RaisePropertyChanged(nameof(Count));
            RaisePropertyChanged(nameof(Last));
            RaisePropertyChanged(nameof(Indices));
            RaisePropertyChanged(nameof(Items));
        }

        #endregion

        #region Fields
        private readonly ConcurrentDictionary<ImageItem, byte> _selection = new ConcurrentDictionary<ImageItem, byte>();
        #endregion
    }
}
