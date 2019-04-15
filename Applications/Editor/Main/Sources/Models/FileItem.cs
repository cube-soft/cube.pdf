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
using Cube.Images.Icons;
using Cube.Xui;
using System;
using System.Windows.Media;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// FileItem
    ///
    /// <summary>
    /// Represents information of a file.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class FileItem : ObservableProperty, IListItem, IDisposable
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// FileItem
        ///
        /// <summary>
        /// Initializes a new instance with the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Path of the source file.</param>
        /// <param name="selection">Shared object for selection.</param>
        /// <param name="io">I/O handler.</param>
        ///
        /* ----------------------------------------------------------------- */
        public FileItem(string src, Selection<FileItem> selection, IO io)
        {
            _dispose   = new OnceAction<bool>(Dispose);
            _selection = selection;

            var info = io.Get(src);
            Name          = info.Name;
            FullName      = info.FullName;
            Length        = info.Length;
            LastWriteTime = info.LastWriteTime;
            Icon          = info.IconImage(IconSize.Small);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Name
        ///
        /// <summary>
        /// Gets the filename.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Name { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// FullName
        ///
        /// <summary>
        /// Gets the full path of the file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string FullName { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Length
        ///
        /// <summary>
        /// Gets the filesize.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public long Length { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// LastWriteTime
        ///
        /// <summary>
        /// Gets the last written time.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public DateTime LastWriteTime { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Icon
        ///
        /// <summary>
        /// Gets the icon image.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ImageSource Icon { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// IsSelected
        ///
        /// <summary>
        /// Gets or sets the value indicating whether the item is selected.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool IsSelected
        {
            get => _selected;
            set
            {
                if (!SetProperty(ref _selected, value)) return;
                if (value) _selection.Add(this);
                else _selection.Remove(this);
            }
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// ~FileItem
        ///
        /// <summary>
        /// Finalizes the FileItem.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        ~FileItem() { _dispose.Invoke(false); }

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases all resources used by the FileItem.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Dispose()
        {
            _dispose.Invoke(true);
            GC.SuppressFinalize(this);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases the unmanaged resources used by the FileItem
        /// and optionally releases the managed resources.
        /// </summary>
        ///
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                IsSelected = false;
                _selection = null;
            }
        }

        #endregion

        #region Fields
        private readonly OnceAction<bool> _dispose;
        private Selection<FileItem> _selection;
        private bool _selected = false;
        #endregion
    }
}
