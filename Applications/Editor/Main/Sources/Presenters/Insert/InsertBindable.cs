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

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// InsertBindable
    ///
    /// <summary>
    /// Provides values for binding to the InsertWindow.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class InsertBindable : ObservableBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// InsertBindable
        ///
        /// <summary>
        /// Initializes a new instance of the InsertBindable class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="index">Selected index.</param>
        /// <param name="count">Number of pages.</param>
        /// <param name="dispatcher">Dispatcher object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public InsertBindable(int index, int count, IDispatcher dispatcher) : base(dispatcher)
        {
            Files         = new FileCollection(dispatcher);
            Selection     = new Selection<FileItem>(dispatcher);
            Count         = count;
            SelectedIndex = index;
            Index         = Math.Max(index, 0);
            UserIndex     = Math.Max(index, 0);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Files
        ///
        /// <summary>
        /// Gets the collection of insertion files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public FileCollection Files { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Selection
        ///
        /// <summary>
        /// Gets the collection of selected items.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Selection<FileItem> Selection { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Count
        ///
        /// <summary>
        /// Gets the number of pages.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Count { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// SelectedIndex
        ///
        /// <summary>
        /// Gets the index of the selected item.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int SelectedIndex { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Index
        ///
        /// <summary>
        /// Gets or sets the value that represents the insertion position.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Index
        {
            get => _index;
            set => SetProperty(ref _index, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// UserIndex
        ///
        /// <summary>
        /// Gets or sets the value that represents the insertion position
        /// specified by users.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int UserIndex
        {
            get => _userIndex;
            set { if (SetProperty(ref _userIndex, value)) Index = value; }
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases the unmanaged resources used by the object
        /// and optionally releases the managed resources.
        /// </summary>
        ///
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        protected override void Dispose(bool disposing) { }

        #endregion

        #region Fields
        private int _index = -1;
        private int _userIndex = -1;
        #endregion
    }
}
