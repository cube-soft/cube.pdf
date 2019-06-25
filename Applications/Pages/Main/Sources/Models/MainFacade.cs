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
using Cube.Mixin.Logging;
using Cube.Mixin.Pdf;
using Cube.Mixin.Syntax;
using Cube.Pdf.Itext;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Cube.Pdf.Pages
{
    /* --------------------------------------------------------------------- */
    ///
    /// FileCollection
    ///
    /// <summary>
    /// Represents the collection of PDF or image files.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class FileCollection : ObservableCollection<File>
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// IO
        ///
        /// <summary>
        /// Gets the I/O handler.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IO IO { get; } = new IO();

        /* ----------------------------------------------------------------- */
        ///
        /// Metadata
        ///
        /// <summary>
        /// Gets the PDF metadata.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Metadata Metadata { get; } = new Metadata();

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
        public void Add(string src)
        {
            var ext = IO.Get(src).Extension.ToLower();
            if (ext == ".pdf") AddDocument(src);
            else lock (_lock) Add(IO.GetImageFile(src));
        }

        /* --------------------------------------------------------------------- */
        ///
        /// Add
        ///
        /// <summary>
        /// Adds the specified PDF or image files.
        /// </summary>
        ///
        /// <param name="src">Collection of PDF or image files.</param>
        /// <param name="limit">
        /// Upper limit of recursively adding files of directories.
        /// </param>
        ///
        /// <remarks>Ignores unsupported files.</remarks>
        ///
        /* --------------------------------------------------------------------- */
        public void Add(IEnumerable<string> src, int limit)
        {
            foreach (var path in src)
            {
                if (Contains(path)) continue;
                if (IO.Get(path).IsDirectory)
                {
                    if (limit > 0) Add(IO.GetFiles(path), limit - 1);
                }
                else
                {
                    try { Add(path); }
                    catch (Exception err) { this.LogWarn($"Ignore:{path} ({err.Message})"); }
                }
            }
        }

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
        public bool Contains(string src) => Items.Any(f => f.FullName == src);

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
        public void Move(IList<int> indices, int offset) =>
            (offset != 0).Then(() => MoveItems(offset < 0 ? indices : indices.Reverse(), offset));

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
        public void Merge(string dest)
        {
            var dir = IO.Get(dest).DirectoryName;
            var tmp = IO.Combine(dir, Guid.NewGuid().ToString("N"));

            try
            {
                var writer = new DocumentWriter();
                foreach (var file in Items)
                {
                    if (file is PdfFile) AddDocument(file as PdfFile, writer);
                    else AddImage(file as ImageFile, writer);
                }
                writer.Set(Metadata);
                writer.Save(tmp);
                IO.Move(tmp, dest, true);
            }
            finally { _ = IO.TryDelete(tmp); }
        }

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
        public void Split(string directory, IList<string> results)
        {
            using (var writer = new DocumentSplitter())
            {
                foreach (var item in Items)
                {
                    if (item is PdfFile) AddDocument(item as PdfFile, writer);
                    else AddImage(item as ImageFile, writer);
                }
                writer.Set(Metadata);
                writer.Save(directory);

                foreach (var result in writer.Results) results.Add(result);
            }
        }

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
            using (var reader = new DocumentReader(path, query, true, true, IO))
            {
                lock (_lock) Add(reader.File);
            }
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
            using (var reader = new DocumentReader(src.FullName, query, true, true, IO))
            {
                dest.Add(reader.Pages);
            }
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
            lock (_lock)
            {
                var inserted = offset < 0 ? -1 : Count;
                foreach (var index in indices)
                {
                    var newindex = offset < 0 ?
                        Math.Max(index + offset, inserted + 1) :
                        Math.Min(index + offset, inserted - 1);
                    Move(index, newindex);
                    inserted = newindex;
                }
            }
        }

        #endregion

        #region Fields
        private readonly object _lock = new object();
        #endregion
    }
}
