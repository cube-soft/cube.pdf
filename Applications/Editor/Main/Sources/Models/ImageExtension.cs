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
using Cube.FileSystem;
using Cube.Images.Icons;
using Cube.Mixin.Drawing;
using Cube.Mixin.Syntax;
using Cube.Pdf.Itext;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;

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
            new ImageItem(src.Convert, src.Selection, src.Preferences)
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
            foreach (var item in src) item.IsSelected = selected;
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
            foreach (var item in src) item.IsSelected = !item.IsSelected;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// Saves the selected PDF objects as the specified filename.
        /// </summary>
        ///
        /// <param name="src">Source collection.</param>
        /// <param name="dest">Save path.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Extract(this ImageCollection src, string dest)
        {
            var pages = GetCopiedIndices(src).OrderBy(i => i).Select(i => src[i].RawObject);
            using (var writer = new DocumentWriter())
            {
                writer.Add(pages);
                writer.Save(dest);
            }
        }

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
            return HistoryItem.CreateInvoke(
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
            src.RemoveAt(GetCopiedIndices(src));

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
            return HistoryItem.CreateInvoke(
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
            var indices = GetCopiedIndices(src);
            return HistoryItem.CreateInvoke(
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
            var indices = GetCopiedIndices(src);
            var cvt     = indices.Select(i => i + delta).ToList();
            return HistoryItem.CreateInvoke(
                () => src.Move(indices, delta),
                () => src.Move(cvt,    -delta)
            );
        }

        #endregion

        #region Icon

        /* ----------------------------------------------------------------- */
        ///
        /// IconImage
        ///
        /// <summary>
        /// Creates a icon from the specified arguments.
        /// </summary>
        ///
        /// <param name="src">File information.</param>
        /// <param name="size">Icon size.</param>
        ///
        /// <returns>Bitmap of the requested icon.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static BitmapImage GetIconImage(this Information src, IconSize size) =>
            src.GetIcon(size)?.ToBitmap().ToBitmapImage(true);

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

        /* ----------------------------------------------------------------- */
        ///
        /// GetCopiedIndices
        ///
        /// <summary>
        /// Gets the copied collection that represents the selected
        /// indices.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static IList<int> GetCopiedIndices(ImageCollection src) =>
            src.Selection.Indices.Where(i => i >= 0 && i < src.Count).ToList();

        #endregion
    }
}
