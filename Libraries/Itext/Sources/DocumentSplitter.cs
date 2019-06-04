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
using Cube.Mixin.IO;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;

namespace Cube.Pdf.Itext
{
    /* --------------------------------------------------------------------- */
    ///
    /// DocumentSplitter
    ///
    /// <summary>
    /// Provides functionality to save the PDF document in page by page.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class DocumentSplitter : DocumentWriterBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// DocumentSplitter
        ///
        /// <summary>
        /// Initializes a new instance of the DocumentSplitter class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public DocumentSplitter() : this(new IO()) { }

        /* ----------------------------------------------------------------- */
        ///
        /// DocumentSplitter
        ///
        /// <summary>
        /// Initializes a new instance of the DocumentSplitter class with
        /// the specified arguments.
        /// </summary>
        ///
        /// <param name="io">I/O handler.</param>
        ///
        /* ----------------------------------------------------------------- */
        public DocumentSplitter(IO io) : base(io) { }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Results
        ///
        /// <summary>
        /// Gets the collection of saved paths.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IList<string> Results { get; } = new List<string>();

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// OnReset
        ///
        /// <summary>
        /// Executes the reset operation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnReset()
        {
            base.OnReset();
            Results.Clear();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnSave
        ///
        /// <summary>
        /// Executes the save operation.
        /// </summary>
        ///
        /// <remarks>
        /// Reset() を実行すると Results まで消去されてしまうため、
        /// base.OnReset() を代わりに実行しています。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnSave(string folder)
        {
            try
            {
                if (!IO.Exists(folder)) IO.CreateDirectory(folder);
                Results.Clear();
                foreach (var page in Pages) SaveCore(page, folder);
            }
            finally { base.OnReset(); } // see remarks
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// SaveCore
        ///
        /// <summary>
        /// Splits pages and saves them to the specified directory in
        /// page by page.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SaveCore(Page src, string directory)
        {
            var reader = GetRawReader(src);
            reader.Rotate(src);

            var dest = Unique(directory, src.File, src.Number);
            SaveOne(reader, src.Number, dest);
            Results.Add(dest);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SaveOne
        ///
        /// <summary>
        /// Saves the specified page.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SaveOne(PdfReader reader, int pagenum, string dest)
        {
            var kv = WriterFactory.Create(dest, Metadata, UseSmartCopy, IO);

            kv.Value.Set(Encryption);
            kv.Key.Open();
            kv.Value.AddPage(kv.Value.GetImportedPage(reader, pagenum));
            kv.Key.Close();
            kv.Value.Close();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Unique
        ///
        /// <summary>
        /// Gets a unique path.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string Unique(string dir, File src, int pagenum)
        {
            var digit = string.Format("D{0}", Math.Max(src.Count.ToString("D").Length, 2));
            var name  = string.Format("{0}-{1}", src.BaseName, pagenum.ToString(digit));
            var dest  = IO.Combine(dir, $"{name}.pdf");

            return IO.GetUniqueName(dest);
        }

        #endregion
    }
}
