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
using Cube.Pdf.App.Editor;
using NUnit.Framework;
using System.Collections.Generic;

namespace Cube.Pdf.Tests.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// HistoryTest
    ///
    /// <summary>
    /// Tests for the History class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class HistoryTest
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Undo
        ///
        /// <summary>
        /// Tests the Undo method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Undo()
        {
            var src = Create();
            var history = new History();

            Assert.That(src.Count, Is.EqualTo(10));
            Assert.That(history.Undoable, Is.False);
            Assert.That(history.Redoable, Is.False);

            Remove(src, 3, history);
            Remove(src, 5, history);
            Remove(src, 7, history);

            Assert.That(src.Count, Is.EqualTo(7));
            Assert.That(history.Undoable, Is.True);
            Assert.That(history.Redoable, Is.False);

            history.Undo();

            Assert.That(src.Count, Is.EqualTo(8));
            Assert.That(history.Undoable, Is.True);
            Assert.That(history.Redoable, Is.True);

            history.Undo();
            history.Undo();
            history.Undo(); // ignore

            Assert.That(src.Count, Is.EqualTo(10));
            Assert.That(history.Undoable, Is.False);
            Assert.That(history.Redoable, Is.True);

            for (var i = 0; i < 10; ++i) Assert.That(src[i].Value, Is.EqualTo(i));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Redo
        ///
        /// <summary>
        /// Tests the Redo method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Redo()
        {
            var src = Create();
            var history = new History();

            Remove(src, 0, history);
            Remove(src, 3, history);
            Remove(src, 6, history);

            history.Undo();
            history.Undo();

            Assert.That(src.Count, Is.EqualTo(9));
            Assert.That(history.Undoable, Is.True);
            Assert.That(history.Redoable, Is.True);

            history.Redo();
            history.Redo();
            history.Redo(); // ignore

            Assert.That(src.Count, Is.EqualTo(7));
            Assert.That(history.Undoable, Is.True);
            Assert.That(history.Redoable, Is.False);

            Assert.That(src[0].Value, Is.EqualTo(1));
            Assert.That(src[1].Value, Is.EqualTo(2));
            Assert.That(src[2].Value, Is.EqualTo(3));
            Assert.That(src[3].Value, Is.EqualTo(5));
            Assert.That(src[4].Value, Is.EqualTo(6));
            Assert.That(src[5].Value, Is.EqualTo(7));
            Assert.That(src[6].Value, Is.EqualTo(9));
        }

        #endregion

        #region Others

        /* ----------------------------------------------------------------- */
        ///
        /// Remove
        ///
        /// <summary>
        /// Removes the item at the specified index.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Remove(IList<Integer> src, int index, History history)
        {
            var item = src[index];
            void forward() => src.RemoveAt(index);
            void reverse() => src.Insert(index, item);
            history.Register(new HistoryItem { Undo = reverse, Redo = forward });
            forward(); // do
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Creates the dummy collection for some tests.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private IList<Integer> Create()
        {
            var dest = new List<Integer>();
            for (var i = 0; i < 10; ++i) dest.Add(new Integer { Value = i });
            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Integer
        ///
        /// <summary>
        /// Simplest class that has only an int property.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private class Integer
        {
            public int Value { get; set; }
        }

        #endregion
    }
}
