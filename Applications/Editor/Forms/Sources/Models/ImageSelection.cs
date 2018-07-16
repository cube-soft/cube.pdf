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
        /// AnySelected
        ///
        /// <summary>
        /// Gets a value indicating whether any of images are selected.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool AnySelected => _selection.Count > 0;

        /* ----------------------------------------------------------------- */
        ///
        /// Value
        ///
        /// <summary>
        /// Gets the selection of images.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<ImageEntry> Value => _selection.Keys;

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
        public void Add(ImageEntry src)
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
        public void Remove(ImageEntry src)
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
            RaisePropertyChanged(nameof(AnySelected));
            RaisePropertyChanged(nameof(Value));
        }

        #endregion

        #region Fields
        private readonly ConcurrentDictionary<ImageEntry, byte> _selection = new ConcurrentDictionary<ImageEntry, byte>();
        #endregion
    }
}
