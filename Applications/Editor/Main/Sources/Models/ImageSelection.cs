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
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Cube.Pdf.Editor
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
    public sealed class ImageSelection : Selection<ImageItem>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ImageSelection
        ///
        /// <summary>
        /// Initializes a new instance of the ImageSelection class with
        /// the specified dispatcher.
        /// </summary>
        ///
        /// <param name="dispatcher">Dispatcher object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ImageSelection(IDispatcher dispatcher) : base(dispatcher) { }

        #endregion

        #region Properties

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
        public int First => RawObject.Keys.OrderBy(i => i.Index).FirstOrDefault()?.Index ?? -1;

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
        public int Last => RawObject.Keys.OrderByDescending(i => i.Index).FirstOrDefault()?.Index ?? -1;

        /* ----------------------------------------------------------------- */
        ///
        /// Indices
        ///
        /// <summary>
        /// Gets the indices of the selected images.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<int> Indices => RawObject.Keys.Select(e => e.Index);

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// OnPropertyChanged
        ///
        /// <summary>
        /// Occurs when a property is changed.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.PropertyName != nameof(Count)) return;
            Refresh(nameof(Last), nameof(Indices));
        }

        #endregion
    }
}
