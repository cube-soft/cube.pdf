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
        /// Clear
        ///
        /// <summary>
        /// Gets or sets the command to clear the selection.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ICommand Clear
        {
            get => (_clear as ICommandable)?.Command;
            set => Set(_clear, value, ClearProperty);
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
            get => (_move as ICommandable)?.Command;
            set => Set(_move, value, MoveProperty);
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
            get => (_preview as ICommandable)?.Command;
            set => Set(_preview, value, PreviewProperty);
        }

        #endregion

        #region Dependencies

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
        /// Set
        ///
        /// <summary>
        /// Sets the value to the specified component.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Set(Behavior<ListView> src, ICommand value, DependencyProperty dp)
        {
            if (src is ICommandable cb)
            {
                cb.Command = value;
                SetValue(dp, value);
            }
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
        private readonly Behavior<ListView> _clear = new MouseClear();
        private readonly Behavior<ListView> _move = new MouseMove();
        private readonly Behavior<ListView> _preview = new MousePreview();
        #endregion
    }

    /* --------------------------------------------------------------------- */
    ///
    /// ICommandable
    ///
    /// <summary>
    /// Represents the interface that has a Command property.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public interface ICommandable
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Command
        ///
        /// <summary>
        /// Gets or sets the command.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        ICommand Command { get; set; }
    }
}
