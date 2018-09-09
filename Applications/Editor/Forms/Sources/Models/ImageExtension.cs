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
using Cube.Pdf.Itext;
using Cube.Pdf.Mixin;
using Cube.Xui.Converters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Media;

namespace Cube.Pdf.App.Editor
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
    public static class ImageExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Create a new instance of the <c>ImageSource</c> class with the
        /// specified parameters.
        /// </summary>
        ///
        /// <param name="src">Renderer object.</param>
        /// <param name="page">Page object.</param>
        /// <param name="ratio">Scale ratio.</param>
        ///
        /// <returns>ImageSource object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static ImageSource Create(this IDocumentRenderer src, Page page, double ratio)
        {
            if (src == null || page == null) return null;

            var size = page.GetDisplaySize(ratio).Value;
            var dest = new Bitmap((int)size.Width, (int)size.Height);

            using (var gs = Graphics.FromImage(dest))
            {
                gs.Clear(System.Drawing.Color.White);
                src.Render(gs, page);
            }
            return dest.ToBitmapImage(true);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Create a new instance of the ImageSource class with the
        /// specified parameters.
        /// </summary>
        ///
        /// <param name="src">Renderer object.</param>
        /// <param name="entry">Information of the creating image.</param>
        ///
        /// <returns>ImageSource object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static ImageSource Create(this IDocumentRenderer src, ImageEntry entry)
        {
            if (src == null) return null;

            var dest = new Bitmap(entry.Width, entry.Height);
            using (var gs = Graphics.FromImage(dest))
            {
                gs.Clear(System.Drawing.Color.White);
                src.Render(gs, entry.RawObject);
            }
            return dest.ToBitmapImage(true);
        }

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
            var items = GetCopiedIndices(src).OrderBy(i => i).Select(i => src[i].RawObject);
            using (var writer = new DocumentWriter())
            {
                writer.Add(items);
                writer.Save(dest);
            }
        }

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

            return Invoke(
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
        public static HistoryItem Remove(this ImageCollection src)
        {
            var indices  = GetCopiedIndices(src);
            var preserve = GetPair(src, indices.OrderBy(i => i));

            void forward() => src.Remove(indices);
            void reverse()
            {
                foreach (var kv in preserve) src.Insert(kv.Key, new[] { kv.Value });
            }

            return Invoke(forward, reverse);
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
            return Invoke(
                () => src.Rotate(indices, degree),
                () => src.Rotate(indices, -degree)
            );
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Move
        ///
        /// <summary>
        /// Moves the selected images at the specfied distance.
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
            return Invoke(
                () => src.Move(indices, delta),
                () => src.Move(cvt,    -delta)
            );
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetPair
        ///
        /// <summary>
        /// Gets the collection each element contains a pair of index
        /// and Page object.
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

        /* ----------------------------------------------------------------- */
        ///
        /// Invokes
        ///
        /// <summary>
        /// Invokes the specified action and creates a history item.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static HistoryItem Invoke(Action forward, Action reverse)
        {
            forward(); // do
            return new HistoryItem { Undo = reverse, Redo = forward };
        }

        #endregion
    }
}
