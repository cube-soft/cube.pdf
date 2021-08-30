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
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Cube.FileSystem;
using Cube.Tests;
using Cube.Pdf.Itext;
using NUnit.Framework;

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
    class DragDropTest : FileFixture
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
            Assert.That(() => obj.DataFormat,                Throws.TypeOf<NotImplementedException>());
            Assert.That(() => obj.DataFormat = null,         Throws.TypeOf<NotImplementedException>());
            Assert.That(() => obj.DragDropHandler,           Throws.TypeOf<NotImplementedException>());
            Assert.That(() => obj.DragDropHandler = null,    Throws.TypeOf<NotImplementedException>());
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
            Assert.That(() => obj.EffectText,                    Throws.TypeOf<NotImplementedException>());
            Assert.That(() => obj.EffectText = null,             Throws.TypeOf<NotImplementedException>());
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Serialize
        ///
        /// <summary>
        /// Tests to serialize a DragDropObject object.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Serialize()
        {
            using var doc = new DocumentReader(GetSource("SampleRotation.pdf"));
            var src = new DragDropObject(4)
            {
                DropIndex = 2,
                Pages     = doc.Pages.ToList()
            };

            var dest = Get("DragDrop.bin");
            using (var fs = Io.Create(dest)) new BinaryFormatter().Serialize(fs, src);

            Assert.That(Io.Exists(dest), Is.True);
        }

        #endregion
    }
}
