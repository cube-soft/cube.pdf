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
using Cube.Pdf.Mixin;
using Cube.Xui.Converters;
using System.Collections.Generic;
using System.Drawing;
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
        /// Insert
        ///
        /// <summary>
        /// Inserts the specified items behind the selected index.
        /// </summary>
        ///
        /// <param name="src">Source collection.</param>
        /// <param name="items">Insertion items.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Insert(this ImageCollection src, IEnumerable<Page> items) =>
            src.Insert(src.Selection.Index + 1, items);

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
        /* ----------------------------------------------------------------- */
        public static void Rotate(this ImageCollection src, int degree) =>
            src.Rotate(src.Selection.Indices, degree);

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
        /* ----------------------------------------------------------------- */
        public static void Move(this ImageCollection src, int delta) =>
            src.Move(src.Selection.Indices, delta);

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
        /* ----------------------------------------------------------------- */
        public static void Remove(this ImageCollection src) => src.Remove(src.Selection.Indices);

        #endregion
    }
}
