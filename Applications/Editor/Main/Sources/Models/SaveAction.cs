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
using Cube.Mixin.Syntax;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// SaveAction
    ///
    /// <summary>
    /// Provides functionality to save the document.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class SaveAction : DisposableBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// SaveAction
        ///
        /// <summary>
        /// Initializes a new instance of the SaveAction class with
        /// the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Source reader.</param>
        /// <param name="images">Image collection.</param>
        /// <param name="options">Save options.</param>
        ///
        /* ----------------------------------------------------------------- */
        public SaveAction(IDocumentReader src, ImageCollection images, SaveOption options)
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
        /// Invoke
        ///
        /// <summary>
        /// Invokes the save action.
        /// </summary>
        ///
        /// <param name="prev">Action to be invoked before saving.</param>
        /// <param name="next">Action to be invoked after saving.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Invoke(Action<Entity> prev, Action<Entity> next)
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
        private void SaveAsDocument(string dest,
            IDocumentReader src,
            IEnumerable<Page> pages,
            Action<Entity> prev,
            Action<Entity> next
        ) {
            var fi  = Options.IO.Get(dest);
            var tmp = Options.IO.Combine(fi.DirectoryName, Guid.NewGuid().ToString("D"));

            try
            {
                using (var writer = new Itext.DocumentWriter())
                {
                    if (Options.Attachments != null) writer.Add(Options.Attachments);
                    if (Options.Metadata    != null) writer.Set(Options.Metadata);
                    if (Options.Encryption  != null) writer.Set(Options.Encryption);
                    if (src != null) writer.Add(pages, Source);
                    else writer.Add(pages);
                    writer.Save(tmp);
                }

                prev(fi);
                Options.IO.Copy(tmp, fi.FullName, true);
                next(fi);
            }
            finally { _ = Options.IO.TryDelete(tmp); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SaveAsDocument
        ///
        /// <summary>
        /// Saves pages as a PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SaveAsDocument(Action<Entity> prev, Action<Entity> next) => SaveAsDocument(
            Options.Destination,
            Source,
            GetTarget(Options).Select(i => Images[i].RawObject),
            prev,
            next
        );

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
            var fi = Options.IO.Get(Options.Destination);
            GetTarget(Options).Each(i => SaveAsDocument(
                Convert(fi, i, Images.Count),
                null,
                new[] { Images[i].RawObject },
                prev,
                next
            ));
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
            var fi = Options.IO.Get(Options.Destination);
            foreach (var i in GetTarget(Options))
            {
                var ratio = Options.Resolution / Images[i].RawObject.Resolution.X;
                using (var image = Images.GetImage(i, ratio))
                {
                    var dest = Options.IO.Get(Convert(fi, i, Images.Count));
                    prev(dest);
                    image.Save(dest.FullName, ImageFormat.Png);
                    next(dest);
                }
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Convert
        ///
        /// <summary>
        /// Gets the output path with the specified arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string Convert(Entity src, int index, int count)
        {
            var digit = string.Format("D{0}", Math.Max(count.ToString("D").Length, 2));
            var name  = string.Format("{0}-{1}{2}", src.BaseName, (index + 1).ToString(digit), src.Extension);
            return Options.IO.Combine(src.DirectoryName, name);
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
            e.Target == SaveTarget.Selected ? Images.GetSelectedIndices().OrderBy(i => i) :
            new Range(e.Range, Images.Count).Select(i => i - 1);

        #endregion
    }
}
