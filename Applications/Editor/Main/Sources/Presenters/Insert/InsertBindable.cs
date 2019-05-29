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
    public class InsertBindable
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
        /// <param name="i">Selected index.</param>
        /// <param name="n">Number of pages.</param>
        /// <param name="dispatcher">Dispatcher object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public InsertBindable(int i, int n, IDispatcher dispatcher)
        {
            Files              = new BindableCollection<FileItem>(dispatcher);
            Selection          = new Selection<FileItem> { Dispatcher = dispatcher };
            Count              = n;
            SelectedIndex      = i;
            Index              = new Bindable<int>(Math.Max(i, 0), dispatcher);
            UserSpecifiedIndex = new Bindable<int>(Math.Max(i, 0), dispatcher);
            UserSpecifiedIndex.PropertyChanged += (s, e) => Index.Value = UserSpecifiedIndex.Value;
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
        public BindableCollection<FileItem> Files { get; }

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
        /// Index
        ///
        /// <summary>
        /// Gets or sets the value that represents the insertion position.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Bindable<int> Index { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// UserSpecifiedIndex
        ///
        /// <summary>
        /// Gets or sets the value that represents the insertion position
        /// specified by users.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Bindable<int> UserSpecifiedIndex { get; }

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
        /// Count
        ///
        /// <summary>
        /// Gets the number of pages.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Count { get; }

        #endregion
    }
}
