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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cube.Collections;
using Cube.FileSystem;
using Cube.Mixin.Tasks;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// DirectoryMonitor
    ///
    /// <summary>
    /// Provides functionality to monitor and notify the change of files
    /// in the specified directory.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class DirectoryMonitor : ObservableBase<Entity>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// DirectoryMonitor
        ///
        /// <summary>
        /// Initializes a new instance of the DirectoryMonitor class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="directory">Target directory.</param>
        /// <param name="filter">Filter string.</param>
        /// <param name="dispatcher">Dispatcher object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public DirectoryMonitor(string directory, string filter, Dispatcher dispatcher) : base(dispatcher)
        {
            Directory  = directory;
            Filter     = filter;

            _core = new System.IO.FileSystemWatcher
            {
                Path                  = Directory,
                Filter                = Filter,
                IncludeSubdirectories = false,
                NotifyFilter          = System.IO.NotifyFilters.FileName |
                                        System.IO.NotifyFilters.LastWrite,
            };

            _core.Created += (s, e) => Refresh();
            _core.Renamed += (s, e) => Refresh();
            _core.Changed += (s, e) => Refresh();
            _core.Deleted += (s, e) => Refresh();
            _core.EnableRaisingEvents = true;

            Refresh();
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Directory
        ///
        /// <summary>
        /// Gets the target directory path.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Directory { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Filter
        ///
        /// <summary>
        /// Gets or the filter string used to determine what files are
        /// monitored in a directory.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Filter { get; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetEnumerator
        ///
        /// <summary>
        /// Returns an enumerator that iterates through this collection.
        /// </summary>
        ///
        /// <returns>
        /// An IEnumerator(ImageEntry) object for this collection.
        /// </returns>
        ///
        /* ----------------------------------------------------------------- */
        public override IEnumerator<Entity> GetEnumerator() => _items.GetEnumerator();

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases the unmanaged resources used by the object and
        /// optionally releases the managed resources.
        /// </summary>
        ///
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        protected override void Dispose(bool disposing) { }

        /* ----------------------------------------------------------------- */
        ///
        /// Refresh
        ///
        /// <summary>
        /// Refreshes the sequence of the recently used PDF files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Refresh() => Task.Run(() =>
        {
            var dest = Io.GetFiles(Directory, Filter)
                         .Select(e => Io.Get(e))
                         .OrderByDescending(e => e.LastWriteTime);
            _ = Interlocked.Exchange(ref _items, dest);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }).Forget();

        #endregion

        #region Fields
        private readonly System.IO.FileSystemWatcher _core;
        private IEnumerable<Entity> _items = Enumerable.Empty<Entity>();
        #endregion
    }
}
