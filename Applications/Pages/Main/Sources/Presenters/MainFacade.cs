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
using Cube.FileSystem;
using Cube.Pdf.Itext;
using Cube.Syntax.Extensions;
using Cube.Text.Extensions;

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
        /// <param name="src">User settings.</param>
        ///
        /* ----------------------------------------------------------------- */
        public MainFacade(SettingFolder src)
        {
            Settings = src;
            Reset();
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
        public Metadata Metadata { get; private set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Encryption
        ///
        /// <summary>
        /// Gets the PDF encryption settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Encryption Encryption { get; private set; }

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
            get => Get(() => false);
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
        public void Merge(string dest) => Lock(() =>
        {
            var op  = Settings.ToSaveOption();
            var dir = op.Temp.HasValue() ? op.Temp : Io.GetDirectoryName(dest);
            var tmp = Io.Combine(dir, Guid.NewGuid().ToString("N"));

            try
            {
                using (var w = Make(new DocumentWriter(op))) w.Save(tmp);
                Io.Copy(tmp, dest, true);
                Reset();
            }
            finally { Logger.Try(() => Io.Delete(tmp)); }
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
        ///
        /* ----------------------------------------------------------------- */
        public void Split(string directory) => Lock(() =>
        {
            var op = Settings.ToSaveOption();
            using (var w = Make(new DocumentSplitter(op))) w.Save(directory);
            Reset();
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
        public void Add(IEnumerable<string> src) => Lock(() =>
        {
            var selector = new FileSelector { Sort = Settings.Value.AutoSort };
            foreach (var f in selector.Get(src))
            {
                if (_inner.Any(e => e.FullName == f)) continue;
                if (Io.GetExtension(f).ToLower() == ".pdf")
                {
                    var op = Settings.ToOpenOption();
                    using var e = new DocumentReader(f, Query, op);
                    _inner.Add(e.File);
                }
                else _inner.Add(new ImageFile(f));
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
        public bool Move(IEnumerable<int> indices, int offset) => Lock(() =>
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
        public void Remove(IEnumerable<int> indices) => Lock(() =>
            indices.OrderByDescending(e => e).Each(i => _inner.RemoveAt(i)));

        /* ----------------------------------------------------------------- */
        ///
        /// Reset
        ///
        /// <summary>
        /// Resets the settings of files, metada, and encryption.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Reset() => Lock(() =>
        {
            _inner.Clear();
            Encryption = new();
            Metadata   = new()
            {
                Version  = new(1, 7),
                Creator  = "CubePDF Page",
                Producer = "CubePDF Page",
            };
        });

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
        /// Make
        ///
        /// <summary>
        /// Makes the specified writer with the provided settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private DocumentWriterBase Make(DocumentWriterBase dest)
        {
            foreach (var f in _inner)
            {
                if (f is PdfFile pf)
                {
                    var op = Settings.ToOpenOption();
                    var r  = new DocumentReader(pf.FullName, pf.Password, op);
                    dest.Add(r.Pages, r);
                }
                else dest.Add(new ImagePageCollection(f.FullName));
            }

            var v = Metadata.Version.Minor;
            Encryption.Method = v >= 7 ? EncryptionMethod.Aes256 :
                                v >= 6 ? EncryptionMethod.Aes128 :
                                v >= 4 ? EncryptionMethod.Standard128 :
                                         EncryptionMethod.Standard40;
            dest.Set(Metadata);
            dest.Set(Encryption);

            return dest;
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
            foreach (var i in indices.ToArray())
            {
                var newindex = offset < 0 ?
                    Math.Max(i + offset, inserted + 1) :
                    Math.Min(i + offset, inserted - 1);
                if (i != newindex) _inner.Move(i, newindex);
                inserted = newindex;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Lock
        ///
        /// <summary>
        /// Locks and invokes the specified action.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Lock(Action action) => Lock(() =>
        {
            action();
            return true;
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Lock
        ///
        /// <summary>
        /// Locks and invokes the specified function.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private T Lock<T>(Func<T> func)
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
        private readonly object _lock = new();
        private readonly ObservableCollection<File> _inner = new();
        #endregion
    }
}
