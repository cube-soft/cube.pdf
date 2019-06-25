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
using Cube.Mixin.Syntax;
using Cube.Pdf.Itext;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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
    public sealed class MainFacade : ObservableBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// MainFacade
        ///
        /// <summary>
        /// Initializes a new instance of the MainFacade class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public MainFacade() : this(new IO()) { }

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
        /// Source
        ///
        /// <summary>
        /// Gets the PDF to attach files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IDocumentReader Source
        {
            get => _source;
            set => SetProperty(ref _source, value);
        }

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
        /// Clips
        ///
        /// <summary>
        /// Gets the collection of attachments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ObservableCollection<ClipItem> Clips { get; } =
            new ObservableCollection<ClipItem>();

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
        public void Open(string src)
        {
            Close();
            Source = new DocumentReader(src, "", true, IO);
            Reset();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Reset
        ///
        /// <summary>
        /// Resets to the state when the provided PDF was loaded.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Reset()
        {
            Clips.Clear();
            var msg = Properties.Resources.StatusEmbedded;
            foreach (var item in Source.Attachments)
            {
                Clips.Add(new ClipItem(item) { Status = msg });
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// Overwrites the provided PDF file with the current condition.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Save()
        {
            var dest  = Source.File.FullName;
            var tmp   = System.IO.Path.GetTempFileName();
            var items = Clips.Select(e => e.RawObject).Where(e => IO.Exists(e.Source));

            using (var writer = new Cube.Pdf.Itext.DocumentWriter())
            {
                writer.UseSmartCopy = true;
                writer.Set(Source.Metadata);
                writer.Set(Source.Encryption);
                writer.Add(Source.Pages);
                writer.Add(items);

                _ = IO.TryDelete(tmp);
                writer.Save(tmp);
            }

            Close();
            IO.Copy(tmp, dest, true);
            _ = IO.TryDelete(tmp);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Attach
        ///
        /// <summary>
        /// Attaches the specified files.
        /// </summary>
        ///
        /// <param name="files">Files to be attached.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Attach(IEnumerable<string> files) => files.Each(f => Attach(f));

        /* ----------------------------------------------------------------- */
        ///
        /// Attach
        ///
        /// <summary>
        /// Attaches the specified file.
        /// </summary>
        ///
        /// <param name="file">File to be attached.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Attach(string file)
        {
            if (Clips.Any(e => e.RawObject.Source == file)) return;
            Clips.Insert(0, new ClipItem(new Attachment(file, IO))
            {
                Status = Properties.Resources.StatusNew
            });
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Detach
        ///
        /// <summary>
        /// Remove the specified files from the provided PDF.
        /// </summary>
        ///
        /// <param name="indices">Indices of files to be removed.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Detach(IEnumerable<int> indices) =>
            indices.OrderByDescending(x => x).Each(i => Detach(i));

        /* ----------------------------------------------------------------- */
        ///
        /// Detach
        ///
        /// <summary>
        /// Remove the specified file from the provided PDF.
        /// </summary>
        ///
        /// <param name="index">Index of file to be removed.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Detach(int index) =>
            (index >= 0 && index < Clips.Count).Then(() => Clips.RemoveAt(index));

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
            Clips.Clear();
            Source.Dispose();
            Source = null;
        }

        #endregion

        #region Fields
        private IDocumentReader _source = null;
        #endregion
    }
}
