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
using Cube.Xui;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Cube.Pdf.App.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// DragMoveBehavior
    ///
    /// <summary>
    /// Represents the mouse behavior of the ListView component.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class MouseBehavior : Behavior<ListView>
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Selection
        ///
        /// <summary>
        /// Gets or sets the collection of selected items.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ImageSelection Selection
        {
            get => _move.Selection;
            set
            {
                _move.Selection = value;
                SetValue(SelectionProperty, value);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Clear
        ///
        /// <summary>
        /// Gets or sets the command to clear the selection.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ICommand Clear
        {
            get => _clear.Command;
            set
            {
                _clear.Command = value;
                SetValue(ClearProperty, value);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Move
        ///
        /// <summary>
        /// Gets or sets the command to move selected items.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ICommand Move
        {
            get => _move.Command;
            set
            {
                _move.Command = value;
                SetValue(MoveProperty, value);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Preview
        ///
        /// <summary>
        /// Gets or sets the command to preview the selected item.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ICommand Preview
        {
            get => _preview.Command;
            set
            {
                _preview.Command = value;
                SetValue(PreviewProperty, value);
            }
        }

        #endregion

        #region Dependencies

        /* ----------------------------------------------------------------- */
        ///
        /// SelectionProperty
        ///
        /// <summary>
        /// Gets the DependencyProperty object for the Selection property.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static readonly DependencyProperty SelectionProperty =
            Create<ImageSelection>(nameof(Selection), (s, e) => s.Selection = e);

        /* ----------------------------------------------------------------- */
        ///
        /// ClearProperty
        ///
        /// <summary>
        /// Gets the DependencyProperty object for the Clear command.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static readonly DependencyProperty ClearProperty =
            Create<ICommand>(nameof(Clear), (s, e) => s.Clear = e);

        /* ----------------------------------------------------------------- */
        ///
        /// MoveProperty
        ///
        /// <summary>
        /// Gets the DependencyProperty object for the Move command.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static readonly DependencyProperty MoveProperty =
            Create<ICommand>(nameof(Move), (s, e) => s.Move = e);

        /* ----------------------------------------------------------------- */
        ///
        /// PreviewProperty
        ///
        /// <summary>
        /// Gets the DependencyProperty object for the Preview command.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static readonly DependencyProperty PreviewProperty =
            Create<ICommand>(nameof(Preview), (s, e) => s.Preview = e);

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
            _clear.Attach(AssociatedObject);
            _move.Attach(AssociatedObject);
            _preview.Attach(AssociatedObject);
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
            _clear.Detach();
            _move.Detach();
            _preview.Detach();
            base.OnDetaching();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Creates a new instance of the DependencyProperty class with
        /// the specified arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static DependencyProperty Create<T>(string name, Action<MouseBehavior, T> callback) =>
            DependencyFactory.Create(name, callback);

        #endregion

        #region Fields
        private readonly MouseClear _clear = new MouseClear();
        private readonly MouseMove _move = new MouseMove();
        private readonly MousePreview _preview = new MousePreview();
        #endregion
    }
}
