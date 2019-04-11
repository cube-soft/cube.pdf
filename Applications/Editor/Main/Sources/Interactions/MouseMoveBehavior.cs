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
using Cube.Xui.Behaviors;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// MouseMoveBehavior
    ///
    /// <summary>
    /// Represents the action to move items through the drag&amp;drop
    /// event.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class MouseMoveBehavior : CommandBehavior<ListView>
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
        public MouseMoveBehavior()
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
        /// Selection
        ///
        /// <summary>
        /// Gets or sets the collection of selected items.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ImageSelection Selection
        {
            get => GetValue(SelectionProperty) as ImageSelection;
            set => SetValue(SelectionProperty, value);
        }

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

        #region Dependencies

        /* ----------------------------------------------------------------- */
        ///
        /// SelectionProperty
        ///
        /// <summary>
        /// Gets a dependency object for the Selection property.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static DependencyProperty SelectionProperty =
            DependencyFactory.Create<MouseMoveBehavior, ImageSelection>(
                nameof(Selection), (s, e) => s.Selection = e
            );

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

            AssociatedObject.AllowDrop = true;
            AssociatedObject.PreviewMouseLeftButtonDown += WhenMouseDown;
            AssociatedObject.MouseMove += WhenMouseMove;
            AssociatedObject.MouseEnter += WhenMouseEnter;
            AssociatedObject.DragOver += WhenDragOver;
            AssociatedObject.Drop += WhenDrop;

            _attached = AssociatedObject.GetParent<Panel>();
            _attached?.Children.Add(DrawingCanvas);
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
            AssociatedObject.PreviewMouseLeftButtonDown -= WhenMouseDown;
            AssociatedObject.MouseMove -= WhenMouseMove;
            AssociatedObject.MouseEnter -= WhenMouseEnter;
            AssociatedObject.DragOver -= WhenDragOver;
            AssociatedObject.Drop -= WhenDrop;

            _attached?.Children.Remove(DrawingCanvas);
            base.OnDetaching();
        }

        #region EventHandler

        /* ----------------------------------------------------------------- */
        ///
        /// WhenMouseDown
        ///
        /// <summary>
        /// Occurs when the MouseDown event is fired.
        /// </summary>
        ///
        /// <remarks>
        /// 単項目が選択されている場合、ダブルクリックによるプレビュー機能
        /// などの他の操作を無効にする可能性があるので MouseMove イベント
        /// まで実行を遅延させます。
        ///
        /// 一方、複数項目が選択されている場合、MouseMove イベントまで
        /// 実行を遅延させると選択状況が解除されてしまうため、MouseDown の
        /// タイミングで Drag&amp;Drop を実行します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenMouseDown(object s, MouseEventArgs e)
        {
            if (Selection.Count > 1 && !Keys.ModifierKeys.IsPressed()) WhenDragStart(s, e);
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
        private void WhenMouseMove(object s, MouseEventArgs e)
        {
            if (e.LeftButton.IsPressed() && !Keys.ModifierKeys.IsPressed()) WhenDragStart(s, e);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// WhenMouseEnter
        ///
        /// <summary>
        /// Occurs when the MouseEnter event is fired.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenMouseEnter(object s, MouseEventArgs e)
        {
            if (!e.LeftButton.IsPressed()) DrawingCanvas.SetVisible(false);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// WhenDragStart
        ///
        /// <summary>
        /// Occurs when the Drag&amp;Drop operation starts.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenDragStart(object s, MouseEventArgs e)
        {
            Debug.Assert(AssociatedObject.Items != null);

            var pt = e.GetPosition(AssociatedObject);
            var item = AssociatedObject.GetObject<ListViewItem>(pt);
            var index = (item != null) ? AssociatedObject.Items.IndexOf(item.Content) : -1;

            e.Handled = item?.IsSelected ?? false;
            if (e.Handled) Drag(index);
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
            var obj = e.Data.GetData(DataFormats.Serializable) as DragDropObject;
            e.Handled = obj != null && obj.DragIndex >= 0;
            e.Effects = e.Handled ? DragDropEffects.Move : DragDropEffects.None;
            if (!e.Handled) return;

            var pt = e.GetPosition(AssociatedObject);
            var unit = AssociatedObject.GetBounds();
            Scroll(obj, pt, unit);
            Draw(obj, pt, unit);
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
            DrawingCanvas.SetVisible(false);

            var obj = e.Data.GetData(DataFormats.Serializable) as DragDropObject;
            e.Handled = obj != null;
            if (!e.Handled) return;

            var pt = e.GetPosition(AssociatedObject);
            var unit = AssociatedObject.GetBounds();
            obj.DropIndex = GetTargetIndex(obj, pt, unit);
            if (Command?.CanExecute(obj) ?? false) Command.Execute(obj);
        }

        #endregion

        /* ----------------------------------------------------------------- */
        ///
        /// Drag
        ///
        /// <summary>
        /// Starts the Drag&amp;Drop action to move items.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Drag(int index) => DragDrop.DoDragDrop(AssociatedObject,
            new DataObject(DataFormats.Serializable, new DragDropObject(index)
            {
                Pages = Selection.OrderBy(e => e.Index)
                                 .Select(e => e.RawObject)
                                 .ToList(),
            }),
            DragDropEffects.Move);

        /* ----------------------------------------------------------------- */
        ///
        /// Draw
        ///
        /// <summary>
        /// Draws the moving position.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Draw(DragDropObject src, Point pt, Rect unit)
        {
            var dest = AssociatedObject.GetIndex(pt, unit);
            var ok   = src.DragIndex >= 0 && dest >= 0;

            DrawingCanvas.SetVisible(ok);
            if (!ok) return;

            var n    = AssociatedObject.Items.Count;
            var rect = AssociatedObject.GetBounds(Math.Max(Math.Min(dest, n - 1), 0));
            var cvt  = Conver(pt, _attached);

            var w = rect.Width + 6;
            var h = rect.Height + 6;
            var o = Conver(new Point(rect.Left + w / 2, rect.Top + h / 6), _attached);
            var x = (dest == n || cvt.X > o.X) ? o.X : o.X - w;
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
        private void Scroll(DragDropObject src, Point pt, Rect unit)
        {
            var sv = AssociatedObject.GetChild<ScrollViewer>();
            if (sv == null) return;

            var height = AssociatedObject.ActualHeight;
            var margin = height / 5.0;
            var offset = Math.Max(unit.Height / 10, 50);

            if (pt.Y < margin) sv.ScrollToVerticalOffset(sv.VerticalOffset - offset);
            else if (pt.Y > height - margin) sv.ScrollToVerticalOffset(sv.VerticalOffset + offset);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetTargetIndex
        ///
        /// <summary>
        /// Gets the item index located at the specified point.
        /// </summary>
        ///
        /// <remarks>
        /// 指定位置がサムネイル上にある場合、その x 座標がサムネイル上の
        /// 右半分か左半分かで挿入位置をずらします。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private int GetTargetIndex(DragDropObject src, Point pt, Rect unit)
        {
            var drag = src.IsCurrentProcess ? src.DragIndex : -1;
            var drop = AssociatedObject.GetIndex(pt, unit);
            if (drop == -1 || drop == AssociatedObject.Items.Count) return drop;

            var rect = AssociatedObject.GetBounds(drop);
            var n    = AssociatedObject.Items.Count;
            var x    = rect.Left + rect.Width / 2;
            var y    = rect.Top + rect.Height / 2;
            var cmp  = Conver(new Point(x, y), _attached);
            var cvt  = Conver(pt, _attached);

            if (cvt.X <= cmp.X && drag < drop) return Math.Max(drop - 1, -1);
            else if (cvt.X > cmp.X && drag > drop) return Math.Min(drop + 1, n);
            else return drop;
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

        #endregion

        #region Fields
        private Panel _attached;
        #endregion
    }
}
