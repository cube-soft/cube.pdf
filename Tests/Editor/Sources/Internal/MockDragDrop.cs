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
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using GongSolutions.Wpf.DragDrop;

namespace Cube.Pdf.Editor.Tests
{
    #region IDragInfo

    /* --------------------------------------------------------------------- */
    ///
    /// MockDragInfo
    ///
    /// <summary>
    /// Mock class that implements the IDragInfo interface.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    class MockDragInfo : IDragInfo
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// MockDragInfo
        ///
        /// <summary>
        /// Initializes a new instance of the MockDragInfo class with the
        /// specified index.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public MockDragInfo(object data, int index)
        {
            Data        = data;
            SourceIndex = index;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Data
        ///
        /// <summary>
        /// Gets or sets the dragged data.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public object Data { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// SourceItem
        ///
        /// <summary>
        /// Gets or sets the dragged data.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public object SourceItem => Data;

        /* ----------------------------------------------------------------- */
        ///
        /// SourceIndex
        ///
        /// <summary>
        /// Gets the index of dragged item.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int SourceIndex { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Effects
        ///
        /// <summary>
        /// Gets or sets the available effects.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public DragDropEffects Effects { get; set; } = DragDropEffects.Move;

        #endregion

        #region NotImplemented
        public Point DragStartPosition => throw new NotImplementedException();
        public Point PositionInDraggedItem => throw new NotImplementedException();
        public MouseButton MouseButton => throw new NotImplementedException();
        public IEnumerable SourceCollection => throw new NotImplementedException();
        public IEnumerable SourceItems => throw new NotImplementedException();
        public CollectionViewGroup SourceGroup => throw new NotImplementedException();
        public UIElement VisualSource => throw new NotImplementedException();
        public UIElement VisualSourceItem => throw new NotImplementedException();
        public FlowDirection VisualSourceFlowDirection => throw new NotImplementedException();
        public DragDropKeyStates DragDropCopyKeyState => throw new NotImplementedException();
        public DataFormat DataFormat { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Func<DependencyObject, object, DragDropEffects, DragDropEffects> DragDropHandler { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        object IDragInfo.DataObject { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        #endregion
    }

    #endregion

    #region IDropInfo

    /* --------------------------------------------------------------------- */
    ///
    /// MockDropInfo
    ///
    /// <summary>
    /// Mock class that implements the IDropInfo interface.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    class MockDropInfo : IDropInfo
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Data
        ///
        /// <summary>
        /// Gets or sets the dragged data.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public object Data { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// DragInfo
        ///
        /// <summary>
        /// Gets the information of dragged item.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IDragInfo DragInfo { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// DropTargetAdorner
        ///
        /// <summary>
        /// Gets or sets the drop target adorner.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Type DropTargetAdorner { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Effects
        ///
        /// <summary>
        /// Gets or sets the drop effect.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public DragDropEffects Effects { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// InsertIndex
        ///
        /// <summary>
        /// Gets or sets the index of dropped item.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int InsertIndex { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// UnfilteredInsertIndex
        ///
        /// <summary>
        /// Gets the index of dropped item.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int UnfilteredInsertIndex => InsertIndex;

        /* ----------------------------------------------------------------- */
        ///
        /// TargetItem
        ///
        /// <summary>
        /// Gets or sets the dropped item.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public object TargetItem { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// DestinationText
        ///
        /// <summary>
        /// Gets or sets the destination text.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string DestinationText { get; set; } = string.Empty;

        /* ----------------------------------------------------------------- */
        ///
        /// NotHandled
        ///
        /// <summary>
        /// Gets or sets the value indicating whether the Drag&amp;Drop
        /// operation is not handled.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool NotHandled { get; set; }

        #endregion

        #region NotImplemented
        public EventType EventType => throw new NotImplementedException();
        public Point DropPosition => throw new NotImplementedException();
        public IEnumerable TargetCollection => throw new NotImplementedException();
        public CollectionViewGroup TargetGroup => throw new NotImplementedException();
        public ScrollViewer TargetScrollViewer => throw new NotImplementedException();
        public ScrollingMode TargetScrollingMode => throw new NotImplementedException();
        public UIElement VisualTarget => throw new NotImplementedException();
        public UIElement VisualTargetItem => throw new NotImplementedException();
        public Orientation VisualTargetOrientation => throw new NotImplementedException();
        public FlowDirection VisualTargetFlowDirection => throw new NotImplementedException();
        public RelativeInsertPosition InsertPosition => throw new NotImplementedException();
        public DragDropKeyStates KeyStates => throw new NotImplementedException();
        public bool IsSameDragDropContextAsSource => throw new NotImplementedException();
        public string EffectText { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        #endregion
    }

    #endregion
}
