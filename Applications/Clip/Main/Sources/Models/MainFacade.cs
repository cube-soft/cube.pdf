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
using Cube.Mixin.String;
using Cube.Mixin.Syntax;
using Cube.Pdf.Itext;
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
        public string Source
        {
            get => _source?.File.FullName;
            set => Open(value);
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

        #endregion

        #region Methods

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
            _clips.Clear();
            if (_source == null) return;
            foreach (var item in _source.Attachments)
            {
                _clips.Add(new ClipItem(item) { Status = Properties.Resources.StatusEmbedded });
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
            var dest  = _source.File.FullName;
            var tmp   = System.IO.Path.GetTempFileName();
            var items = _clips.Select(e => e.RawObject).Where(e => IO.Exists(e.Source));

            using (var writer = new DocumentWriter())
            {
                writer.UseSmartCopy = true;
                writer.Set(_source.Metadata);
                writer.Set(_source.Encryption);
                writer.Add(_source.Pages);
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
        /// Attaches the specified file.
        /// </summary>
        ///
        /// <param name="file">File to be attached.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Attach(string file)
        {
            if (_clips.Any(e => e.RawObject.Source == file)) return;
            _clips.Insert(0, new ClipItem(new Attachment(file, IO))
            {
                Status = Properties.Resources.StatusNew
            });
        }

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
            (index >= 0 && index < _clips.Count).Then(() => _clips.RemoveAt(index));

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
        /// Open
        ///
        /// <summary>
        /// Opens the specified PDF file.
        /// </summary>
        ///
        /// <param name="src">Path of the PDF file.</param>
        ///
        /* ----------------------------------------------------------------- */
        private void Open(string src)
        {
            if (_source != null)
            {
                if (_source.File.FullName.FuzzyEquals(src)) return;
                else Close();
            }
            _source = new DocumentReader(src, "", true, IO);
            Reset();
        }

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

        #endregion

        #region Fields
        private readonly ObservableCollection<ClipItem> _clips = new ObservableCollection<ClipItem>();
        private IDocumentReader _source = null;
        #endregion
    }
}
