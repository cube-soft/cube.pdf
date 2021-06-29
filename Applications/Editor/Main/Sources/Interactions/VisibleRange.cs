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
using Cube.Xui;

namespace Cube.Pdf.Editor
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
            get => (int)GetValue(FirstProperty);
            set => SetValue(FirstProperty, value);
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
            get => (int)GetValue(LastProperty);
            set => SetValue(LastProperty, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Unit
        ///
        /// <summary>
        /// Gets or sets the size of each item.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Unit
        {
            get => _unit;
            set => Set(ref _unit, value, Update);
        }

        #endregion

        #region Dependencies

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
            Create<int>(nameof(First), (s, e) => s.First = e);

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
            Create<int>(nameof(Last), (s, e) => s.Last = e);

        /* ----------------------------------------------------------------- */
        ///
        /// ItemSizeProperty
        ///
        /// <summary>
        /// DependencyProperty object for the ItemWidth property.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static readonly DependencyProperty ItemSizeProperty =
            Create<int>(nameof(Unit), (s, e) => s.Unit = e);

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
            AssociatedObject.SizeChanged   += WhenChanged;
            AssociatedObject.ScrollChanged += WhenChanged;
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
            AssociatedObject.SizeChanged   -= WhenChanged;
            AssociatedObject.ScrollChanged -= WhenChanged;
            base.OnDetaching();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Update
        ///
        /// <summary>
        /// Updates the visible range of the ScrollViewer control.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Update()
        {
            if (AssociatedObject == null) return;

            var size   = Math.Max(Unit, 1.0);
            var column = Math.Max((int)(AssociatedObject.ActualWidth / size), 1);
            var row    = Math.Max((int)(AssociatedObject.ActualHeight / size), 1);

            var index  = (int)(AssociatedObject.VerticalOffset / size + 0.5) * column;
            var first  = Math.Max(index - column, 0);
            var last   = index + column * (row + 2);

            First = first;
            Last  = last;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Creates a new DependencyProperty instance with the specified
        /// parameters.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static DependencyProperty Create<T>(string name,
            Action<VisibleRange, T> callback) => DependencyFactory.Create(name, callback);

        /* ----------------------------------------------------------------- */
        ///
        /// Set
        ///
        /// <summary>
        /// Sets a new value to the specified property and executes the
        /// action when setting complete.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Set<T>(ref T field, T value, Action action)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return;
            field = value;
            action();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// WhenChanged
        ///
        /// <summary>
        /// Occurs when some condition of the AssociatedObject is changed.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenChanged(object s, EventArgs e) => Update();

        #endregion

        #region Fields
        private int _unit;
        #endregion
    }
}
