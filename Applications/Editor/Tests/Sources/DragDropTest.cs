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
using NUnit.Framework;
using System;

namespace Cube.Pdf.Editor.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// DragDropTest
    ///
    /// <summary>
    /// Tests the MockDragInfo and MockDropInfo classes.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class DragDropTest
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Drag
        ///
        /// <summary>
        /// Confirms unimplemented properties.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Drag()
        {
            var obj = new MockDragInfo(new object(), 0);
            Assert.That(() => obj.DragStartPosition,         Throws.TypeOf<NotImplementedException>());
            Assert.That(() => obj.PositionInDraggedItem,     Throws.TypeOf<NotImplementedException>());
            Assert.That(() => obj.MouseButton,               Throws.TypeOf<NotImplementedException>());
            Assert.That(() => obj.SourceCollection,          Throws.TypeOf<NotImplementedException>());
            Assert.That(() => obj.SourceItems,               Throws.TypeOf<NotImplementedException>());
            Assert.That(() => obj.SourceGroup,               Throws.TypeOf<NotImplementedException>());
            Assert.That(() => obj.VisualSource,              Throws.TypeOf<NotImplementedException>());
            Assert.That(() => obj.VisualSourceItem,          Throws.TypeOf<NotImplementedException>());
            Assert.That(() => obj.VisualSourceFlowDirection, Throws.TypeOf<NotImplementedException>());
            Assert.That(() => obj.DragDropCopyKeyState,      Throws.TypeOf<NotImplementedException>());
            Assert.That(() => obj.DataObject,                Throws.TypeOf<NotImplementedException>());
            Assert.That(() => obj.DataObject = null,         Throws.TypeOf<NotImplementedException>());
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Drop
        ///
        /// <summary>
        /// Confirms unimplemented properties.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Drop()
        {
            var obj = new MockDropInfo();
            Assert.That(() => obj.DropPosition,                  Throws.TypeOf<NotImplementedException>());
            Assert.That(() => obj.TargetCollection,              Throws.TypeOf<NotImplementedException>());
            Assert.That(() => obj.TargetGroup,                   Throws.TypeOf<NotImplementedException>());
            Assert.That(() => obj.VisualTarget,                  Throws.TypeOf<NotImplementedException>());
            Assert.That(() => obj.VisualTargetItem,              Throws.TypeOf<NotImplementedException>());
            Assert.That(() => obj.VisualTargetOrientation,       Throws.TypeOf<NotImplementedException>());
            Assert.That(() => obj.VisualTargetFlowDirection,     Throws.TypeOf<NotImplementedException>());
            Assert.That(() => obj.InsertPosition,                Throws.TypeOf<NotImplementedException>());
            Assert.That(() => obj.KeyStates,                     Throws.TypeOf<NotImplementedException>());
            Assert.That(() => obj.IsSameDragDropContextAsSource, Throws.TypeOf<NotImplementedException>());
        }

        #endregion
    }
}
