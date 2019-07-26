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
using Cube.Mixin.Iteration;
using System;
using System.Collections.Generic;
using System.Linq;
using Engine = Cube.Pdf.Itext.DocumentWriter;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// DocumentWriter
    ///
    /// <summary>
    /// Provides functionality to save the document.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class DocumentWriter : DisposableBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// DocumentWriter
        ///
        /// <summary>
        /// Initializes a new instance of the DocumentWriter class with
        /// the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Source reader.</param>
        /// <param name="images">Image collection.</param>
        /// <param name="options">Save options.</param>
        ///
        /* ----------------------------------------------------------------- */
        public DocumentWriter(IDocumentReader src, ImageCollection images, SaveOption options)
        {
            Source  = src;
            Images  = images;
            Options = options;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Source
        ///
        /// <summary>
        /// Gets the object to reader the source PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IDocumentReader Source { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Images
        ///
        /// <summary>
        /// Gets the collection of images.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ImageCollection Images { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Options
        ///
        /// <summary>
        /// Gets the save options.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public SaveOption Options { get; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// Saves PDF pages.
        /// </summary>
        ///
        /// <param name="prev">Action to be invoked before saving.</param>
        /// <param name="next">Action to be invoked after saving.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Save(Action<Entity> prev, Action<Entity> next)
        {
            if (Options.Format == SaveFormat.Pdf)
            {
                if (Options.Split) SplitAsDocument(prev, next);
                else SaveAsDocument(prev, next);
            }
            else SplitAsImage(prev, next);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases the unmanaged resources used by the object
        /// and optionally releases the managed resources.
        /// </summary>
        ///
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        protected override void Dispose(bool disposing)
        {
            if (disposing) Source.Dispose();
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// SaveAsDocument
        ///
        /// <summary>
        /// Saves pages as a PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SaveAsDocument(Action<Entity> prev, Action<Entity> next)
        {
            var pages = GetTarget(Options).Select(i => Images[i].RawObject);
            var dest  = Options.IO.Get(Options.Destination);
            var tmp   = Options.IO.Combine(dest.DirectoryName, Guid.NewGuid().ToString("D"));

            try
            {
                using (var writer = new Engine())
                {
                    if (Options.Attachments != null) writer.Add(Options.Attachments);
                    if (Options.Metadata    != null) writer.Set(Options.Metadata);
                    if (Options.Encryption  != null) writer.Set(Options.Encryption);

                    writer.Add(pages, Source);
                    writer.Save(tmp);
                }

                prev(dest);
                Options.IO.Copy(tmp, dest.FullName, true);
                next(dest);
            }
            finally { _ = Options.IO.TryDelete(tmp); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SplitAsDocument
        ///
        /// <summary>
        /// Saves as a PDF file per page.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SplitAsDocument(Action<Entity> prev, Action<Entity> next)
        {

        }

        /* ----------------------------------------------------------------- */
        ///
        /// SplitAsImage
        ///
        /// <summary>
        /// Saves as an image file per page.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SplitAsImage(Action<Entity> prev, Action<Entity> next)
        {

        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetTarget
        ///
        /// <summary>
        /// Gets the target indices according to the specified options.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private IEnumerable<int> GetTarget(SaveOption e) =>
            e.Target == SaveTarget.All      ? Images.Count.Make(i => i) :
            e.Target == SaveTarget.Selected ? Images.GetSelectedIndices() :
            new Range(e.Range, Images.Count).Select(i => i - 1);

        #endregion
    }
}
