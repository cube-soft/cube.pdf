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
using Cube.Mixin.Pdf;
using Cube.Pdf.Itext;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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
    public sealed class MainFacade : ObservableBase
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
        ///
        /* ----------------------------------------------------------------- */
        public MainFacade(IO io)
        {
            IO = io;
        }

        #endregion

        #region Properties

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
            get => _busy;
            private set => SetProperty(ref _busy, value);
        }

        #endregion

        #region Methods

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
        public void Add(string src) => Invoke(() =>
        {
            var ext = IO.Get(src).Extension.ToLower();
            if (ext == ".pdf") AddDocument(src);
            else _inner.Add(IO.GetImageFile(src));
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Contains
        ///
        /// <summary>
        /// Determines whether the specified PDF or image file has already
        /// been added.
        /// </summary>
        ///
        /// <param name="src">PDF or image file.</param>
        ///
        /* ----------------------------------------------------------------- */
        public bool Contains(string src) => _inner.Any(f => f.FullName == src);

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
        public void Move(IList<int> indices, int offset) => Invoke(() =>
        {
            if (offset == 0) return;
            var src = offset < 0 ? indices : indices.Reverse();
            MoveItems(src, offset);
        });

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
                var writer = new DocumentWriter();
                foreach (var file in _inner)
                {
                    if (file is PdfFile) AddDocument(file as PdfFile, writer);
                    else AddImage(file as ImageFile, writer);
                }
                writer.Set(Metadata);
                writer.Save(tmp);
                IO.Move(tmp, dest, true);
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
            using (var writer = new DocumentSplitter())
            {
                foreach (var item in _inner)
                {
                    if (item is PdfFile) AddDocument(item as PdfFile, writer);
                    else AddImage(item as ImageFile, writer);
                }
                writer.Set(Metadata);
                writer.Save(directory);

                foreach (var result in writer.Results) results.Add(result);
            }
        });

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
        /// AddDocument
        ///
        /// <summary>
        /// Adds the specified PDF file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void AddDocument(string path)
        {
            var query = new Query<string>(e => throw new NotSupportedException());
            using (var e = new DocumentReader(path, query, true, true, IO)) _inner.Add(e.File);
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
            var query = new Query<string>(e => e.Cancel = true);
            using (var e = new DocumentReader(src.FullName, query, true, true, IO)) dest.Add(e.Pages);
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
            foreach (var index in indices)
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
        private readonly object _lock = new object();
        private readonly ObservableCollection<File> _inner = new ObservableCollection<File>();
        private bool _busy = false;
        #endregion
    }
}
