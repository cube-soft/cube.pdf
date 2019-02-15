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
using Cube.Collections;
using Cube.FileSystem;
using Cube.Tasks;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cube.Pdf.App.Editor
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
    public class DirectoryMonitor : ObservableBase<Information>
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
        /// <param name="io">I/O handler</param>
        ///
        /* ----------------------------------------------------------------- */
        public DirectoryMonitor(string directory, string filter, IO io)
        {
            Context   = SynchronizationContext.Current;
            Directory = directory;
            Filter    = filter;
            IO        = io;

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

        /* ----------------------------------------------------------------- */
        ///
        /// IO
        ///
        /// <summary>
        /// Gets the I/O handler.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IO IO { get; }

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
        public override IEnumerator<Information> GetEnumerator() => _items.GetEnumerator();

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Refresh
        ///
        /// <summary>
        /// Refreshes the sequence of the recently used PDF files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Refresh() => TaskEx.Run(() =>
        {
            var dest = IO.GetFiles(Directory, Filter)
                         .Select(e => IO.Get(e))
                         .OrderByDescending(e => e.LastWriteTime);
            Interlocked.Exchange(ref _items, dest);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }).Forget();

        #endregion

        #region Fields
        private readonly System.IO.FileSystemWatcher _core;
        private IEnumerable<Information> _items = new Information[0];
        #endregion
    }
}
