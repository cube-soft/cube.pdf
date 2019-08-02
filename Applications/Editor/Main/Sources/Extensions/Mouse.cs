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

namespace Cube.Pdf.Editor
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
        /// <returns>Rect object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Rect GetBounds(this ListView src, int index)
        {
            if (index < 0 || index >= src.Items.Count) return new Rect();

            var obj = src.ItemContainerGenerator.ContainerFromIndex(index);
            if (obj is ListViewItem item)
            {
                var delta = item.TransformToVisual(src).Transform(new Point());
                var dest = VisualTreeHelper.GetDescendantBounds(item);
                dest.Offset(delta.X, delta.Y);
                return dest;
            }
            else return new Rect();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetIndex
        ///
        /// <summary>
        /// Gets the item index located at the specified point.
        /// </summary>
        ///
        /// <remarks>
        /// 右端のマージンを考慮してインデックスを決定します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static int GetIndex(this ListView obj, Point pt, Rect unit)
        {
            var dest = obj.GetIndex(pt);
            if (dest >= 0) return dest;

            // 最後の項目の右側
            if (pt.Y > unit.Bottom || (pt.X > unit.Right && pt.Y > unit.Top)) return obj.Items.Count;

            var item = obj.ItemContainerGenerator.ContainerFromIndex(0) as ListViewItem;
            var w    = obj.ActualWidth;
            var m    = item?.Margin.Right ?? 0;
            var x    = (w - pt.X < unit.Width) ? (w - unit.Width) : (pt.X - m);
            return (x != pt.X) ? obj.GetIndex(new Point(x, pt.Y)) : dest;
        }

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
        /// <returns>Index of the item.</returns>
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
        /// <returns>Found object.</returns>
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
        /// <returns>Found object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static T GetParent<T>(this DependencyObject src) where T : DependencyObject
        {
            for (var obj = src; obj != null; obj = VisualTreeHelper.GetParent(obj))
            {
                if (obj is T dest) return dest;
            }
            return default;
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
        /// <returns>Found object.</returns>
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
            return default;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetVisible
        ///
        /// <summary>
        /// Sets the specified visibility.
        /// </summary>
        ///
        /// <param name="src">UI element.</param>
        /// <param name="visible">
        /// true for Visiblity.Visible; otherwise Visiblity.Collapsed.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        public static void SetVisible(this UIElement src, bool visible)
        {
            var cvt = visible ? Visibility.Visible : Visibility.Collapsed;
            if (src.Visibility != cvt) src.Visibility = cvt;
        }

        #endregion
    }
}
