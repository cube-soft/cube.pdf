/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2013 CubeSoft, Inc.
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
using System.Reflection;
using System.Threading;
using Cube.FileSystem;
using Cube.Mixin.Pdf;
using Cube.Mixin.Syntax;
using Cube.Pdf.Itext;

namespace Cube.Pdf.Pages
{
    /* --------------------------------------------------------------------- */
    ///
    /// MainFacade
    ///
    /// <summary>
    /// Represents the collection of PDF or image files.
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
        /// <param name="io">I/O handler.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public MainFacade(IO io, SynchronizationContext context) : base(new ContextDispatcher(context, false))
        {
            Settings = new SettingFolder(Assembly.GetExecutingAssembly(), io);
            IO = io;
            _inner.CollectionChanged += (s, e) => CollectionChanged?.Invoke(this, e);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Settings
        ///
        /// <summary>
        /// Gets the user settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public SettingFolder Settings { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Files
        ///
        /// <summary>
        /// Gets the collection of target files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IList<File> Files => _inner;

        /* ----------------------------------------------------------------- */
        ///
        /// Query
        ///
        /// <summary>
        /// Gets or sets the object to query password.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IQuery<string> Query { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Metadata
        ///
        /// <summary>
        /// Gets the PDF metadata.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Metadata Metadata { get; } = new Metadata
        {
            Version  = new PdfVersion(1, 7),
            Creator  = "CubePDF Page",
            Producer = "CubePDF Page",
        };

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
        /// Merge
        ///
        /// <summary>
        /// Merges the provided files and save to the specified path.
        /// </summary>
        ///
        /// <param name="dest">Path to save.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Merge(string dest) => Invoke(() =>
        {
            var dir = IO.Get(dest).DirectoryName;
            var tmp = IO.Combine(dir, Guid.NewGuid().ToString("N"));

            try
            {
                using (var writer = Make<DocumentWriter>()) writer.Save(tmp);
                IO.Move(tmp, dest, true);
                ClearItems();
            }
            finally { _ = IO.TryDelete(tmp); }
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Split
        ///
        /// <summary>
        /// Splits the provided files and save to the specified directory.
        /// </summary>
        ///
        /// <param name="directory">Directory to save.</param>
        /// <param name="results">Operation results.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Split(string directory, IList<string> results) => Invoke(() =>
        {
            using (var writer = Make<DocumentSplitter>())
            {
                writer.Save(directory);
                foreach (var f in writer.Results) results.Add(f);
            }
            ClearItems();
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Add
        ///
        /// <summary>
        /// Adds the specified PDF or image file.
        /// </summary>
        ///
        /// <param name="src">PDF or image file.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Add(IEnumerable<string> src) => Invoke(() =>
        {
            foreach (var f in new FileSelector(IO).Get(src))
            {
                if (_inner.Any(e => e.FullName == f)) continue;
                var ext = IO.Get(f).Extension.ToLower();
                if (ext == ".pdf") AddDocument(f);
                else _inner.Add(IO.GetImageFile(f));
            }
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Move
        ///
        /// <summary>
        /// Moves the specified items by the specified offset.
        /// </summary>
        ///
        /// <param name="indices">Indices of files.</param>
        /// <param name="offset">Offset to move.</param>
        ///
        /* ----------------------------------------------------------------- */
        public bool Move(IEnumerable<int> indices, int offset) => Invoke(() =>
        {
            if (offset == 0 || !indices.Any()) return false;

            var src = indices.OrderBy(i => i).ToArray();
            var min = src[0] + offset;
            var max = src[src.Length - 1] + offset;
            if (min < 0 || max >= Files.Count) return false;

            MoveItems(offset < 0 ? src : src.Reverse(), offset);
            return true;
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Remove
        ///
        /// <summary>
        /// Removes the specified indices.
        /// </summary>
        ///
        /// <param name="indices">
        /// Collection of indices to be removed.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        public void Remove(IEnumerable<int> indices) => Invoke(() =>
            indices.OrderByDescending(e => e).Each(i => _inner.RemoveAt(i)));

        /* ----------------------------------------------------------------- */
        ///
        /// Clear
        ///
        /// <summary>
        /// Clears the added files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Clear() => Invoke(() => ClearItems());

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

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Make
        ///
        /// <summary>
        /// Makes the specified writer with the provided settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private T Make<T>() where T : IDocumentWriter, new()
        {
            var dest = new T();
            foreach (var file in _inner)
            {
                if (file is PdfFile) AddDocument(file as PdfFile, dest);
                else AddImage(file as ImageFile, dest);
            }
            dest.Set(Metadata);
            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// AddDocument
        ///
        /// <summary>
        /// Adds the specified PDF file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void AddDocument(string path)
        {
            var options = new OpenOption { IO = IO, FullAccess = true };
            using var e = new DocumentReader(path, Query, options);
            _inner.Add(e.File);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// AddDocument
        ///
        /// <summary>
        /// Adds the specified PDF file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void AddDocument(PdfFile src, IDocumentWriter dest)
        {
            var options = new OpenOption { IO = IO, FullAccess = true };
            using var e = new DocumentReader(src.FullName, src.Password, options);
            dest.Add(e.Pages);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// AddImage
        ///
        /// <summary>
        /// Adds the specified image file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void AddImage(ImageFile src, IDocumentWriter dest)
        {
            var pages = IO.GetImagePages(src.FullName);
            if (pages != null) dest.Add(pages);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// MoveItems
        ///
        /// <summary>
        /// Moves the specified items by the specified offset.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void MoveItems(IEnumerable<int> indices, int offset)
        {
            var inserted = offset < 0 ? -1 : _inner.Count;
            foreach (var index in indices.ToArray())
            {
                var newindex = offset < 0 ?
                    Math.Max(index + offset, inserted + 1) :
                    Math.Min(index + offset, inserted - 1);
                if (index != newindex) _inner.Move(index, newindex);
                inserted = newindex;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Clear
        ///
        /// <summary>
        /// Clears the added files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void ClearItems() => _inner.Clear();

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Invokes the specified action.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Invoke(Action action) => Invoke(() =>
        {
            action();
            return true;
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Invokes the specified function.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private T Invoke<T>(Func<T> func)
        {
            lock (_lock)
            {
                try
                {
                    Busy = true;
                    return func();
                }
                finally { Busy = false; }
            }
        }

        #endregion

        #region Fields
        private readonly object _lock = new object();
        private readonly ObservableCollection<File> _inner = new ObservableCollection<File>();
        #endregion
    }
}
