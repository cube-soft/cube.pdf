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
using System.Linq;

namespace Cube.Pdf.Itext
{
    /* --------------------------------------------------------------------- */
    ///
    /// DocumentWriter
    ///
    /// <summary>
    /// Provides functionality to create or modify a PDF document.
    /// </summary>
    ///
    /// <remarks>
    /// DocumentWriter realizes the page rotation information
    /// (Page.Rotation.Delta) by modifying the internal object of
    /// DocumentReader. However, if DocumentReader is generated with
    /// OpenOption.ReduceMemory enabled, this change will be disabled and
    /// the result of the page rotation change cannot be reflected.
    /// If you have rotated the page, please set the corresponding option
    /// to disabled.
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public class DocumentWriter : DocumentWriterBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// DocumentWriter
        ///
        /// <summary>
        /// Initializes a new instance of the DocumentWriter class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public DocumentWriter() : this(new()) { }

        /* ----------------------------------------------------------------- */
        ///
        /// DocumentWriter
        ///
        /// <summary>
        /// Initializes a new instance of the DocumentWriter class with
        /// the specified options.
        /// </summary>
        ///
        /// <param name="options">Saving options.</param>
        ///
        /* ----------------------------------------------------------------- */
        public DocumentWriter(SaveOption options) : base(options) { }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// OnSave
        ///
        /// <summary>
        /// Executes the save operation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnSave(string path)
        {
            try
            {
                using var dest = new Writer(path, Options, Metadata, Encryption);

                foreach (var chunk in Pages.ChunkBy(e => e.File))
                {
                    var src = Reader.From(GetRawReader(chunk.Key));
                    dest.Add(src, chunk);
                }
                dest.Add(Attachments);
                Release(); // Dispose all readers before save.
            }
            catch (Exception err)
            {
                var obj = err.Convert();
                if (obj != err) throw obj;
                else throw;
            }
            finally { Reset(); }
        }

        #endregion
    }
}
