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
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Cube.Pdf.App.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// MouseMove
    ///
    /// <summary>
    /// Represents the action to move items through the drag&amp;drop
    /// event.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class MouseMove : Behavior<ListView>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// MouseMove
        ///
        /// <summary>
        /// Initializes a new instance of the MouseMove class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public MouseMove()
        {
            Drawing = new Border
            {
                BorderBrush     = SystemColors.HotTrackBrush.Clone(),
                BorderThickness = new Thickness(1),
                Background      = SystemColors.HotTrackBrush.Clone(),
                CornerRadius    = new CornerRadius(1),
            };

            Drawing.Background.Opacity = 0.1;
            Drawing.MouseEnter += WhenMouseEnter;
            Drawing.DragOver   += WhenDragOver;
            Drawing.Drop       += WhenDrop;

            DrawingCanvas = new Canvas { Visibility = Visibility.Collapsed };
            DrawingCanvas.Children.Add(Drawing);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Command
        ///
        /// <summary>
        /// Gets or sets the command.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ICommand Command { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Selection
        ///
        /// <summary>
        /// Gets or sets the collection of selected items.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ImageSelection Selection { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// DrawingCanvas
        ///
        /// <summary>
        /// Gets the canvas to draw the moving position.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Canvas DrawingCanvas { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Drawing
        ///
        /// <summary>
        /// Gets the drawing object.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Border Drawing { get; }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// OnAttached
        ///
        /// <summary>
        /// Called when the action is attached to an AssociatedObject.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnAttached()
        {
            base.OnAttached();
            Reset();

            AssociatedObject.AllowDrop = true;
            AssociatedObject.PreviewMouseLeftButtonDown += WhenMouseDown;
            AssociatedObject.MouseMove += WhenMouseMove;
            AssociatedObject.MouseEnter += WhenMouseEnter;
            AssociatedObject.DragOver += WhenDragOver;
            AssociatedObject.Drop += WhenDrop;

            _root = AssociatedObject.GetParent<Panel>();
            _root?.Children.Add(DrawingCanvas);
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
            Reset();

            AssociatedObject.PreviewMouseLeftButtonDown -= WhenMouseDown;
            AssociatedObject.MouseMove -= WhenMouseMove;
            AssociatedObject.DragOver -= WhenDragOver;
            AssociatedObject.Drop -= WhenDrop;

            _root?.Children.Remove(DrawingCanvas);
            base.OnDetaching();
        }

        #region EventHandler

        /* ----------------------------------------------------------------- */
        ///
        /// WhenMouseDown
        ///
        /// <summary>
        /// Occurs when the LeftMouseButtonDown event is fired.
        /// </summary>
        ///
        /// <remarks>
        /// TODO: 複数項目の Drag&amp;Drop 処理に問題があるため対応を
        /// 要検討。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenMouseDown(object s, MouseButtonEventArgs e)
        {
            Debug.Assert(AssociatedObject.Items != null);

            var pt   = e.GetPosition(AssociatedObject);
            var item = AssociatedObject.GetObject<ListViewItem>(pt);

            _core = new Core
            {
                Origin = pt,
                Source = (item != null) ? AssociatedObject.Items.IndexOf(item.Content) : -1,
                Bounds = AssociatedObject.GetBounds(AssociatedObject.Items.Count - 1),
            };

            e.Handled = item?.IsSelected ?? false;
            if (e.Handled) Drag();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// WhenMouseMove
        ///
        /// <summary>
        /// Occurs when the MouseMove event is fired.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && _core.Source >= 0) Drag();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// WhenMouseEnter
        ///
        /// <summary>
        /// Occurs when the MouseEnter event is fired.
        /// </summary>
        ///
        /// <remarks>
        /// Windows 外で Drop した場合、描画内容が残ったままとなります。
        /// そこで、MouseEnter イベントが発生したタイミングで非表示に
        /// します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenMouseEnter(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) return;
            DrawingCanvas.Visibility = Visibility.Collapsed;
            Reset();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// WhenDragOver
        ///
        /// <summary>
        /// Occurs when the DragOver event is fired.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenDragOver(object s, DragEventArgs e)
        {
            e.Handled = (_core.Source >= 0);
            if (e.Handled)
            {
                e.Effects = DragDropEffects.Move;

                var pt = e.GetPosition(AssociatedObject);
                Scroll(pt);
                Draw(pt);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// WhenDrop
        ///
        /// <summary>
        /// Occurs when the Drop event is fired.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenDrop(object s, DragEventArgs e)
        {
            try
            {
                DrawingCanvas.Visibility = Visibility.Collapsed;
                var index = GetTargetIndex(e.GetPosition(AssociatedObject));
                var delta = index - _core.Source;
                if (Command?.CanExecute(delta) ?? false) Command.Execute(delta);
            }
            finally { Reset(); }
        }

        #endregion

        /* ----------------------------------------------------------------- */
        ///
        /// GetTargetIndex
        ///
        /// <summary>
        /// Gets the item index located at the specified point.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private int GetTargetIndex(Point pt)
        {
            var index = GetIndex(pt);
            if (index == -1 || index == AssociatedObject.Items.Count) return index;

            var r = AssociatedObject.GetBounds(index);
            var cmp = Conver(new Point(r.Left + r.Width / 2, r.Top + r.Height / 2), _root);
            var cvt = Conver(pt, _root);

            var n = AssociatedObject.Items.Count;
            if (_core.Source < index && cvt.X < cmp.X) return Math.Max(index - 1, 0);
            else if (_core.Source > index && cvt.X >= cmp.X) return Math.Min(index + 1, n);
            else return index;
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
        private int GetIndex(Point pt)
        {
            var dest = AssociatedObject.GetIndex(pt);
            if (dest >= 0) return dest;

            // 最後の項目の右側
            if (pt.Y > _core.Bounds.Bottom ||
                pt.X > _core.Bounds.Right &&
                pt.Y > _core.Bounds.Top) return AssociatedObject.Items.Count;

            var w = AssociatedObject.ActualWidth;
            var m = AssociatedObject.GetItem(0)?.Margin.Right ?? 0;
            var x = (w - pt.X < _core.Bounds.Width) ? (w - _core.Bounds.Width) : (pt.X - m);
            return (x != pt.X) ? AssociatedObject.GetIndex(new Point(x, pt.Y)) : dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Drag
        ///
        /// <summary>
        /// Starts the Drag&amp;Drop action to move items.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Drag()
        {
            DrawingCanvas.Visibility = Visibility.Collapsed;
            DragDrop.DoDragDrop(AssociatedObject, _core.Source, DragDropEffects.Move);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Draw
        ///
        /// <summary>
        /// Draws the moving position.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Draw(Point pt)
        {
            var dest = GetIndex(pt);
            var ok   = _core.Source >= 0 && dest >= 0;

            DrawingCanvas.Visibility = ok ? Visibility.Visible : Visibility.Collapsed;
            if (!ok) return;

            var n    = AssociatedObject.Items.Count;
            var rect = AssociatedObject.GetBounds(Math.Max(Math.Min(dest, n - 1), 0));
            var cvt  = Conver(pt, _root);

            var w = rect.Width + 6;
            var h = rect.Height + 6;
            var o = Conver(new Point(rect.Left + w / 2, rect.Top + h / 6), _root);
            var x = (dest == n || cvt.X >= o.X) ? o.X : o.X - w;
            var y = o.Y;

            Canvas.SetLeft(Drawing, x);
            Canvas.SetTop(Drawing, y);

            Drawing.Width  = w;
            Drawing.Height = h * (2 / 3.0);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Scroll
        ///
        /// <summary>
        /// Moves the vertical scroll bar.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Scroll(Point pt)
        {
            var sv = AssociatedObject.GetChild<ScrollViewer>();
            if (sv == null) return;

            var height = AssociatedObject.ActualHeight;
            var margin = height / 5.0;
            var offset = Math.Max(_core.Bounds.Height / 10, 50);

            if (pt.Y < margin) sv.ScrollToVerticalOffset(sv.VerticalOffset - offset);
            else if (pt.Y > height - margin) sv.ScrollToVerticalOffset(sv.VerticalOffset + offset);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Convert
        ///
        /// <summary>
        /// Converts the specified point based on the specified control.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private Point Conver<T>(Point pt, T control) where T : UIElement =>
            control != null ?
            control.PointFromScreen(AssociatedObject.PointToScreen(pt)) :
            pt;

        /* ----------------------------------------------------------------- */
        ///
        /// Reset
        ///
        /// <summary>
        /// Resets the inner objects.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Reset() => _core = new Core();

        #endregion

        #region Fields
        private Panel _root;
        private Core _core;
        private class Core
        {
            public int Source { get; set; } = -1;
            public Point Origin { get; set; } = new Point();
            public Rect Bounds { get; set; } = new Rect();
        }
        #endregion
    }
}
