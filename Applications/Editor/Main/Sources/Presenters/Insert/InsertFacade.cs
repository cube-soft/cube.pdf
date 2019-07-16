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
using Cube.FileSystem;
using Cube.Mixin.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// InsertFacade
    ///
    /// <summary>
    /// Provides functionality to communicate with the InsertViewModel
    /// and other model classes.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class InsertFacade
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// InsertFacade
        ///
        /// <summary>
        /// Initializes a new instance of the InsertFacade with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="i">Selected index.</param>
        /// <param name="n">Number of pages.</param>
        /// <param name="io">I/O handler</param>
        /// <param name="dispatcher">Dispatcher object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public InsertFacade(int i, int n, IO io, IDispatcher dispatcher)
        {
            IO    = io;
            Value = new InsertBindableValue(i, n, dispatcher);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Value
        ///
        /// <summary>
        /// Gets the bindable value.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public InsertBindableValue Value { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// IO
        ///
        /// <summary>
        /// Gets the I/O handler.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected IO IO { get; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Preview
        ///
        /// <summary>
        /// Previews the selected file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Preview() => Process.Start(Value.Selection.First().FullName);

        /* ----------------------------------------------------------------- */
        ///
        /// Add
        ///
        /// <summary>
        /// Adds new files.
        /// </summary>
        ///
        /// <param name="src">Collection of files.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Add(IEnumerable<string> src)
        {
            foreach (var item in src)
            {
                Value.Files.Add(new FileItem(item, Value.Selection, IO));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Clear
        ///
        /// <summary>
        /// Clears all files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Clear()
        {
            foreach (var item in Value.Files) item.Dispose();
            Value.Files.Clear();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Remove
        ///
        /// <summary>
        /// Removes selected files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Remove()
        {
            foreach (var item in Value.Selection.ToList())
            {
                if (Value.Files.Remove(item)) item.Dispose();
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Move
        ///
        /// <summary>
        /// Moves selected items at the specified distance.
        /// </summary>
        ///
        /// <param name="delta">Moving distance.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Move(int delta)
        {
            if (delta != 0) Move(GetSelection(delta).ToList(), delta);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Move
        ///
        /// <summary>
        /// Moves selected items according to specified indices.
        /// </summary>
        ///
        /// <param name="from">Source index.</param>
        /// <param name="to">Target index.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Move(int from, int to)
        {
            if (from == to) return;
            if (from > to) MovePrevious(from, to);
            else MoveNext(from, to);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SelectClear
        ///
        /// <summary>
        /// Sets the IsSelected property of all files to false.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void SelectClear()
        {
            foreach (var item in Value.Selection.ToList()) item.IsSelected = false;
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Move
        ///
        /// <summary>
        /// Moves specified items at the specified distance.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Move(IEnumerable<int> src, int delta)
        {
            foreach (var index in src)
            {
                var inew = Math.Min(Math.Max(index + delta, 0), Value.Files.Count - 1);
                if (inew != index) Value.Files.Move(index, inew);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// MovePrevious
        ///
        /// <summary>
        /// Moves selected items according to specified indices.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void MovePrevious(int from, int to)
        {
            var delta = to - from;
            var src   = GetSelection(delta);
            var n     = src.Where(i => i < from && i >= to).Count();
            Move(src, delta + n);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// MoveNext
        ///
        /// <summary>
        /// Moves selected items according to specified indices.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void MoveNext(int from, int to)
        {
            var delta = to - from - 1;
            if (delta == 0) return;

            var src = GetSelection(delta);
            var n   = src.Where(i => i > from && i <= to).Count();
            Move(src, delta - n);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetSelection
        ///
        /// <summary>
        /// Gets the collection of selected items.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private IEnumerable<int> GetSelection(int delta)
        {
            var n    = Value.Files.Count;
            var dest = Value.Selection.Select(e => Value.Files.IndexOf(e)).Within(n);
            return delta > 0 ? dest.OrderByDescending() : dest.OrderBy();
        }

        #endregion
    }
}
