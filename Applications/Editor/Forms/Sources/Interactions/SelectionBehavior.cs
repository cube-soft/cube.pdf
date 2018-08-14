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
using Fluent;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Cube.Pdf.App.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// SelectionBehavior
    ///
    /// <summary>
    /// Represents the behavior when a ListBoxItem is selected.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class SelectionBehavior : Behavior<ListBox>
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Command
        ///
        /// <summary>
        /// Gets or sets the command that executes when the selection
        /// is changed.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ICommand Command
        {
            get => GetValue(CommandProperty) as ICommand;
            set => SetValue(CommandProperty, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Popup
        ///
        /// <summary>
        /// Gets or sets a value indicating whether the AssociatedObject
        /// may be popped up.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Popup
        {
            get => (bool)GetValue(PopupProperty);
            set => SetValue(PopupProperty, value);
        }

        #endregion

        #region DependencyProperties

        /* ----------------------------------------------------------------- */
        ///
        /// CommandProperty
        ///
        /// <summary>
        /// Gets the DependencyProperty object for the Command property.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static readonly DependencyProperty CommandProperty =
            CreateProperty<ICommand>(nameof(Command), (s, e) => s.Command = e);

        /* ----------------------------------------------------------------- */
        ///
        /// PopupProperty
        ///
        /// <summary>
        /// Gets the DependencyProperty object for the Popup property.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static readonly DependencyProperty PopupProperty =
            CreateProperty<bool>(nameof(Popup), (s, e) => s.Popup = e);

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// CreateProperty
        ///
        /// <summary>
        /// Creates a new instance of the DependencyProperty class
        /// with the specified arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static DependencyProperty CreateProperty<T>(string name, Action<SelectionBehavior, T> action) =>
            DependencyProperty.RegisterAttached(
                name,
                typeof(T),
                typeof(SelectionBehavior),
                new PropertyMetadata(default(T), (s, e) =>
                {
                    if (s is SelectionBehavior sm && e.NewValue is T value) action(sm, value);
                })
            );

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
            AssociatedObject.SelectionChanged += WhenSelectionChanged;
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
            AssociatedObject.SelectionChanged -= WhenSelectionChanged;
            base.OnDetaching();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// WhenSelectionChanged
        ///
        /// <summary>
        /// Called when the selection of the ListBox is changed.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenSelectionChanged(object s, SelectionChangedEventArgs e)
        {
            try
            {
                var check = (e.AddedItems.Count > 0) &&
                            (Command?.CanExecute(e.AddedItems[0]) ?? false);
                if (check) Command.Execute(e.AddedItems[0]);
            }
            finally
            {
                AssociatedObject.UnselectAll();
                if (Popup) PopupService.RaiseDismissPopupEventAsync(AssociatedObject, DismissPopupMode.Always);
            }
        }

        #endregion
    }
}
