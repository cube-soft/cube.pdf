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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using Cube.FileSystem;
using Cube.Mixin.String;
using Cube.Mixin.Syntax;
using Cube.Pdf.Itext;

namespace Cube.Pdf.Clip
{
    /* --------------------------------------------------------------------- */
    ///
    /// MainFacade
    ///
    /// <summary>
    /// Represents the facade model for the MainViewModel class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class MainFacade : ObservableBase, INotifyCollectionChanged
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// MainFacade
        ///
        /// <summary>
        /// Initializes a new instance of the MainFacade class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public MainFacade(SynchronizationContext context) : base(new ContextDispatcher(context, false))
        {
            _clips.CollectionChanged += (s, e) => CollectionChanged?.Invoke(this, e);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Source
        ///
        /// <summary>
        /// Gets the PDF to attach files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Source => _source?.File.FullName;

        /* ----------------------------------------------------------------- */
        ///
        /// Clips
        ///
        /// <summary>
        /// Gets the collection of attached files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<ClipItem> Clips => _clips;

        /* ----------------------------------------------------------------- */
        ///
        /// Busy
        ///
        /// <summary>
        /// Gets a value indicating whether the class is busy.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Busy
        {
            get => Get<bool>();
            private set => Set(value);
        }

        #endregion

        #region Events

        /* ----------------------------------------------------------------- */
        ///
        /// CollectionChanged
        ///
        /// <summary>
        /// Occurs when an item is added, removed, changed, moved, or the
        /// entire list is refreshed.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Open
        ///
        /// <summary>
        /// Opens the specified PDF file.
        /// </summary>
        ///
        /// <param name="src">Path of the PDF file.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Open(string src) => Invoke(() =>
        {
            if (_source != null)
            {
                if (_source.File.FullName.FuzzyEquals(src)) return;
                else Close();
            }

            var options = new OpenOption { SaveMemory = true };
            _source = new DocumentReader(src, "", options);
            Reset();
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Reset
        ///
        /// <summary>
        /// Resets to the state when the provided PDF was loaded.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Reset() => Invoke(() =>
        {
            _clips.Clear();
            if (_source == null) return;
            foreach (var item in _source.Attachments)
            {
                _clips.Add(new(item) { Status = Properties.Resources.StatusEmbedded });
            }
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// Overwrites the provided PDF file with the current condition.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Save() => Invoke(() =>
        {
            var dest  = _source.File.FullName;
            var tmp   = System.IO.Path.GetTempFileName();
            var items = _clips.Select(e => e.RawObject).Where(e => Io.Exists(e.Source));

            using (var writer = new DocumentWriter(new() { Smart = true }))
            {
                writer.Set(_source.Metadata);
                writer.Set(_source.Encryption);
                writer.Add(_source.Pages);
                writer.Add(items);

                GetType().LogWarn(() => Io.Delete(tmp));
                writer.Save(tmp);
            }

            Close();
            Io.Copy(tmp, dest, true);
            GetType().LogWarn(() => Io.Delete(tmp));
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Attach
        ///
        /// <summary>
        /// Attaches the specified files.
        /// </summary>
        ///
        /// <param name="src">Files to be attached.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Attach(IEnumerable<string> src) => Invoke(() =>
        {
            foreach (var f in src)
            {
                if (_clips.Any(e => e.RawObject.Source == f)) continue;
                var e = new ClipItem(new(f)) { Status = Properties.Resources.StatusNew };
                _clips.Insert(0, e);
            }
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Detach
        ///
        /// <summary>
        /// Remove the specified files from the provided PDF.
        /// </summary>
        ///
        /// <param name="indices">
        /// Collection of files to be removed.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        public void Detach(IEnumerable<int> indices) => Invoke(() =>
            indices.OrderByDescending(e => e).Each(i => _clips.RemoveAt(i)));

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
        protected override void Dispose(bool disposing) => Close();

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Close
        ///
        /// <summary>
        /// Closes the provided PDF.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Close()
        {
            _clips.Clear();
            _source?.Dispose();
            _source = null;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Invokes the specified action.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Invoke(Action action)
        {
            lock (_lock)
            {
                try
                {
                    Busy = true;
                    action();
                }
                finally { Busy = false; }
            }
        }

        #endregion

        #region Fields
        private readonly object _lock = new();
        private readonly ObservableCollection<ClipItem> _clips = new();
        private IDocumentReader _source = null;
        #endregion
    }
}
