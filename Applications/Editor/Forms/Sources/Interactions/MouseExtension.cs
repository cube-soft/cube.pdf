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
using System.Windows.Input;
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
        /// Gets the bound of the first item.
        /// </summary>
        ///
        /// <param name="src">UI element.</param>
        ///
        /// <returns>Rect object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Rect GetBounds(this ListView src) =>
            src.Items.Count > 0 ? src.GetBounds(0) : new Rect();

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
        /// <returns>ListViewItem object.</returns>
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

            var w = obj.ActualWidth;
            var m = obj.GetItem(0)?.Margin.Right ?? 0;
            var x = (w - pt.X < unit.Width) ? (w - unit.Width) : (pt.X - m);
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
            return default(T);
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

        /* ----------------------------------------------------------------- */
        ///
        /// IsPressed
        ///
        /// <summary>
        /// Gets the value indicating whether the specified mouse button
        /// is pressed.
        /// </summary>
        ///
        /// <param name="src">Mouse button.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static bool IsPressed(this MouseButtonState src) =>
            src == MouseButtonState.Pressed;

        #endregion
    }
}
