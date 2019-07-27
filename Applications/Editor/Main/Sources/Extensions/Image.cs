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
using Cube.Mixin.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// ImageExtension
    ///
    /// <summary>
    /// Provides functionality to handle the ImageEntry or related
    /// classes.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class ImageExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// NewItem
        ///
        /// <summary>
        /// Creates a new instance of the ImageItem class with the specified
        /// arguments.
        /// </summary>
        ///
        /// <param name="src">Source collection.</param>
        /// <param name="index">Index to be created.</param>
        /// <param name="item">Page information.</param>
        ///
        /// <returns>ImageItem object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static ImageItem NewItem(this ImageCollection src, int index, Page item) =>
            new ImageItem(src.GetImage, src.Selection, src.Preferences)
        {
            Index     = index,
            RawObject = item,
        };

        /* ----------------------------------------------------------------- */
        ///
        /// Select
        ///
        /// <summary>
        /// Sets the IsSelected property of all items to be the specified
        /// value.
        /// </summary>
        ///
        /// <param name="src">Source collection.</param>
        /// <param name="selected">true for selected.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Select(this ImageCollection src, bool selected)
        {
            foreach (var item in src) item.Selected = selected;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Flip
        ///
        /// <summary>
        /// Flips the section of items.
        /// </summary>
        ///
        /// <param name="src">Source collection.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Flip(this ImageCollection src)
        {
            foreach (var item in src) item.Selected = !item.Selected;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetSelectedIndices
        ///
        /// <summary>
        /// Gets the copied collection that represents the selected
        /// indices.
        /// </summary>
        ///
        /// <param name="src">Source collection.</param>
        ///
        /// <returns>Selected indices.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static IList<int> GetSelectedIndices(this ImageCollection src) =>
            src.Selection.Indices.Where(i => i >= 0 && i < src.Count).ToList();

        #region Undoable

        /* ----------------------------------------------------------------- */
        ///
        /// Insert
        ///
        /// <summary>
        /// Inserts the specified items behind the selected index.
        /// </summary>
        ///
        /// <param name="src">Source collection.</param>
        /// <param name="items">Insertion items.</param>
        ///
        /// <returns>
        /// History item to execute undo and redo actions.
        /// </returns>
        ///
        /* ----------------------------------------------------------------- */
        public static HistoryItem Insert(this ImageCollection src, IEnumerable<Page> items) =>
            src.InsertAt(src.Selection.Last + 1, items);

        /* ----------------------------------------------------------------- */
        ///
        /// InsertAt
        ///
        /// <summary>
        /// Inserts the specified items behind the specified index.
        /// </summary>
        ///
        /// <param name="src">Source collection.</param>
        /// <param name="index">Insertion index.</param>
        /// <param name="items">Insertion items.</param>
        ///
        /// <returns>
        /// History item to execute undo and redo actions.
        /// </returns>
        ///
        /* ----------------------------------------------------------------- */
        public static HistoryItem InsertAt(this ImageCollection src, int index, IEnumerable<Page> items)
        {
            var copy    = items.ToList();
            var indices = Enumerable.Range(index, copy.Count);
            return HistoryItem.Invoke(
                () => src.Insert(index, copy),
                () => src.Remove(indices)
            );
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Remove
        ///
        /// <summary>
        /// Removes the selected images.
        /// </summary>
        ///
        /// <param name="src">Source collection.</param>
        ///
        /// <returns>
        /// History item to execute undo and redo actions.
        /// </returns>
        ///
        /* ----------------------------------------------------------------- */
        public static HistoryItem Remove(this ImageCollection src) =>
            src.RemoveAt(src.GetSelectedIndices());

        /* ----------------------------------------------------------------- */
        ///
        /// RemoveAt
        ///
        /// <summary>
        /// Removes the specified images.
        /// </summary>
        ///
        /// <param name="src">Source collection.</param>
        /// <param name="indices">Collection to be removed.</param>
        ///
        /// <returns>
        /// History item to execute undo and redo actions.
        /// </returns>
        ///
        /* ----------------------------------------------------------------- */
        public static HistoryItem RemoveAt(this ImageCollection src, IEnumerable<int> indices)
        {
            var items = GetPair(src, indices.OrderBy(i => i));
            return HistoryItem.Invoke(
                () => src.Remove(indices),
                () => items.Each(e => src.Insert(e.Key, new[] { e.Value }))
            );
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Rotate
        ///
        /// <summary>
        /// Rotates the selected images and regenerates them.
        /// </summary>
        ///
        /// <param name="src">Source collection.</param>
        /// <param name="degree">Rotation angle in degree unit.</param>
        ///
        /// <returns>
        /// History item to execute undo and redo actions.
        /// </returns>
        ///
        /* ----------------------------------------------------------------- */
        public static HistoryItem Rotate(this ImageCollection src, int degree)
        {
            var indices = src.GetSelectedIndices();
            return HistoryItem.Invoke(
                () => src.Rotate(indices,  degree),
                () => src.Rotate(indices, -degree)
            );
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Move
        ///
        /// <summary>
        /// Moves the selected images at the specified distance.
        /// </summary>
        ///
        /// <param name="src">Source collection.</param>
        /// <param name="delta">Moving distance.</param>
        ///
        /// <returns>
        /// History item to execute undo and redo actions.
        /// </returns>
        ///
        /* ----------------------------------------------------------------- */
        public static HistoryItem Move(this ImageCollection src, int delta)
        {
            var indices = src.GetSelectedIndices();
            var cvt     = indices.Select(i => i + delta).ToList();
            return HistoryItem.Invoke(
                () => src.Move(indices, delta),
                () => src.Move(cvt,    -delta)
            );
        }

        #endregion

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetPair
        ///
        /// <summary>
        /// Gets the collection each element is a pair of index and Page
        /// object.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static IList<KeyValuePair<int, Page>> GetPair(ImageCollection src,
            IEnumerable<int> indices) =>
            indices.Select(i => KeyValuePair.Create(i, src[i].RawObject)).ToList();

        #endregion
    }
}
