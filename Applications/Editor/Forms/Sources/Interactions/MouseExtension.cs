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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Cube.Pdf.App.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// MouseExtension
    ///
    /// <summary>
    /// Provides extended methods for the ListView class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class MouseExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetBounds
        ///
        /// <summary>
        /// Gets the bound of the specified index.
        /// </summary>
        ///
        /// <param name="src">UI element.</param>
        /// <param name="index">Target index.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static Rect GetBounds(this ListView src, int index)
        {
            if (index < 0 || index >= src.Items.Count) return new Rect();

            var item = src.GetItem(index);
            if (item == null) return new Rect();

            var delta = item.TransformToVisual(src).Transform(new Point());
            var dest = VisualTreeHelper.GetDescendantBounds(item);
            dest.Offset(delta.X, delta.Y);
            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetItem
        ///
        /// <summary>
        /// Gets the item from the specified index.
        /// </summary>
        ///
        /// <param name="src">UI element.</param>
        /// <param name="index">Target index.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static ListViewItem GetItem(this ListView src, int index) =>
            src.ItemContainerGenerator.ContainerFromIndex(index) as ListViewItem;

        /* ----------------------------------------------------------------- */
        ///
        /// GetIndex
        ///
        /// <summary>
        /// Gets the item index located at the specified point.
        /// </summary>
        ///
        /// <param name="src">UI element.</param>
        /// <param name="pt">Target point.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static int GetIndex(this ListView src, Point pt)
        {
            var obj = src.GetObject<ListViewItem>(pt);
            return (obj != null) ? src.Items.IndexOf(obj.Content) : -1;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetObject
        ///
        /// <summary>
        /// Gets the object of type T at the specified point.
        /// </summary>
        ///
        /// <param name="src">UI element.</param>
        /// <param name="pt">Target point.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static T GetObject<T>(this ListView src, Point pt)
            where T : DependencyObject => GetParent<T>(
            VisualTreeHelper.HitTest(src, pt)?.VisualHit
        );

        /* ----------------------------------------------------------------- */
        ///
        /// GetParent
        ///
        /// <summary>
        /// Gets the parent object which type is T.
        /// </summary>
        ///
        /// <param name="src">UI element.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static T GetParent<T>(this DependencyObject src) where T : DependencyObject
        {
            for (var obj = src; obj != null; obj = VisualTreeHelper.GetParent(obj))
            {
                if (obj is T dest) return dest;
            }
            return default(T);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetChild
        ///
        /// <summary>
        /// Gets the child object which type is T.
        /// </summary>
        ///
        /// <param name="src">UI element.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static T GetChild<T>(this DependencyObject src) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(src); ++i)
            {
                var obj = VisualTreeHelper.GetChild(src, i);
                if (obj is T dest) return dest;
                else
                {
                    var gc = GetChild<T>(obj);
                    if (gc != null) return gc;
                }
            }
            return default(T);
        }

        #endregion
    }
}
