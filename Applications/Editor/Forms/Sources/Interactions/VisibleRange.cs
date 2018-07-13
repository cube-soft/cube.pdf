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
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Cube.Pdf.App.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// VisibleRange
    ///
    /// <summary>
    /// Provides the indices of currently visible items in the
    /// ScrollViewer control.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class VisibleRange : Behavior<ScrollViewer>
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// First
        ///
        /// <summary>
        /// Gets or sets the first index of currently visible items.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int First
        {
            get => _first;
            set => SetProperty(ref _first, value, () => SetValue(FirstProperty, value));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Last
        ///
        /// <summary>
        /// Gets or sets the last index of currently visible items.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Last
        {
            get => _last;
            set => SetProperty(ref _last, value, () => SetValue(LastProperty, value));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ItemWidth
        ///
        /// <summary>
        /// Gets or sets the width of each item.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int ItemWidth
        {
            get => _itemWidth;
            set => SetProperty(ref _itemWidth, value, () => SetRange());
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ItemHeight
        ///
        /// <summary>
        /// Gets or sets the height of each item.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int ItemHeight
        {
            get => _itemHeight;
            set => SetProperty(ref _itemHeight, value, () => SetRange());
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ItemMargin
        ///
        /// <summary>
        /// Gets or sets the margin of each item.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int ItemMargin
        {
            get => _itemMargin;
            set => SetProperty(ref _itemMargin, value, () => SetRange());
        }

        #endregion

        #region DependencyProperty

        /* ----------------------------------------------------------------- */
        ///
        /// FirstProperty
        ///
        /// <summary>
        /// DependencyProperty object for the First property.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static readonly DependencyProperty FirstProperty =
            CreateProperty<int>(nameof(First), (s, e) => s.First = e);

        /* ----------------------------------------------------------------- */
        ///
        /// LastProperty
        ///
        /// <summary>
        /// DependencyProperty object for the Last property.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static readonly DependencyProperty LastProperty =
            CreateProperty<int>(nameof(Last), (s, e) => s.Last = e);

        /* ----------------------------------------------------------------- */
        ///
        /// ItemWidthProperty
        ///
        /// <summary>
        /// DependencyProperty object for the ItemWidth property.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static readonly DependencyProperty ItemWidthProperty =
            CreateProperty<int>(nameof(ItemWidth), (s, e) => s.ItemWidth = e);

        /* ----------------------------------------------------------------- */
        ///
        /// ItemHeightProperty
        ///
        /// <summary>
        /// DependencyProperty object for the ItemHeight property.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static readonly DependencyProperty ItemHeightProperty =
            CreateProperty<int>(nameof(ItemHeight), (s, e) => s.ItemHeight = e);

        /* ----------------------------------------------------------------- */
        ///
        /// ItemMarginProperty
        ///
        /// <summary>
        /// DependencyProperty object for the ItemMargin property.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static readonly DependencyProperty ItemMarginProperty =
            CreateProperty<int>(nameof(ItemMargin), (s, e) => s.ItemMargin = e);

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// OnAttached
        ///
        /// <summary>
        /// Called after the action is attached to an AssociatedObject.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.SizeChanged   -= WhenSizeChanged;
            AssociatedObject.SizeChanged   += WhenSizeChanged;
            AssociatedObject.ScrollChanged -= WhenScrollChanged;
            AssociatedObject.ScrollChanged += WhenScrollChanged;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnDetaching
        ///
        /// <summary>
        /// Called when the action is being detached from its
        /// AssociatedObject, but before it has actually occurred.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnDetaching()
        {
            AssociatedObject.SizeChanged   -= WhenSizeChanged;
            AssociatedObject.ScrollChanged -= WhenScrollChanged;

            base.OnDetaching();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetRange
        ///
        /// <summary>
        /// Sets the visible range of the ScrollViewer control.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SetRange()
        {
            if (AssociatedObject == null) return;

            var width  = Math.Max(ItemWidth, 1.0);
            var height = Math.Max(ItemHeight, 1.0);
            var margin = Math.Max(ItemMargin * 2, 0.0);

            var column = (int)(AssociatedObject.ActualWidth / (width + margin));
            var row    = (int)(AssociatedObject.ActualHeight / (height + margin));

            var index  = (int)(AssociatedObject.VerticalOffset / height) * column;
            var first  = Math.Max(index - column, 0);
            var last   = index + column * (row + 1);

            SetRange(first, last);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetRange
        ///
        /// <summary>
        /// Sets the visible range of the ScrollViewer control with the
        /// specified parameters.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SetRange(int first, int last)
        {
            First = first;
            Last  = last;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CreateProperty
        ///
        /// <summary>
        /// Creates a new DependencyProperty instance with the specified
        /// parameters.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static DependencyProperty CreateProperty<T>(string name,
            Action<VisibleRange, T> callback) =>
            DependencyProperty.RegisterAttached(
                name,
                typeof(T),
                typeof(VisibleRange),
                new PropertyMetadata((s, e) =>
                {
                    if (s is VisibleRange src && e.NewValue is T value) callback(src, value);
                })
            );

        /* ----------------------------------------------------------------- */
        ///
        /// SetProperty
        ///
        /// <summary>
        /// Sets a new value to the specified property.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private bool SetProperty<T>(ref T field, T value)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            return true;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetProperty
        ///
        /// <summary>
        /// Sets a new value to the specified property and executes the
        /// action when setting complete.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SetProperty<T>(ref T field, T value, Action action)
        {
            if (SetProperty(ref field, value)) action();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// WhenSizeChanged
        ///
        /// <summary>
        /// Called after the size of the AssociatedObject is changed.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenSizeChanged(object sender, SizeChangedEventArgs e) => SetRange();

        /* ----------------------------------------------------------------- */
        ///
        /// WhenScrollChanged
        ///
        /// <summary>
        /// Called after the scroll condition of the AssociatedObject
        /// is changed.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenScrollChanged(object sender, ScrollChangedEventArgs e) => SetRange();

        #endregion

        #region Fields
        private int _first;
        private int _last;
        private int _itemWidth;
        private int _itemHeight;
        private int _itemMargin;
        #endregion
    }
}
