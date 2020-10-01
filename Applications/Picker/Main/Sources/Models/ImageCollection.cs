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
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cube.Collections;
using Cube.FileSystem;
using Cube.Mixin.IO;
using Cube.Mixin.Iteration;
using Cube.Pdf.Itext;

namespace Cube.Pdf.Picker
{
    /* --------------------------------------------------------------------- */
    ///
    /// ImageCollection
    ///
    /// <summary>
    /// Represents the collection of images that are extracted from the
    /// PDF document.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ImageCollection : EnumerableBase<Image>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ImageCollection
        ///
        /// <summary>
        /// Initializes a new instance of the ImageCollection class with
        /// the specified path.
        /// </summary>
        ///
        /// <param name="src">Path to extract images.</param>
        /// <param name="io">I/O handler.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ImageCollection(string src, IO io)
        {
            Source = src;
            IO     = io;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Source
        ///
        /// <summary>
        /// Gets the path of the PDF file to extract images.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Source { get; }

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
        /// ExtractAsync
        ///
        /// <summary>
        /// Extracts images as an asynchronous method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public async Task ExtractAsync(IProgress<Message<int>> progress)
        {
            try
            {
                using (_cts = new CancellationTokenSource())
                {
                    await Task.Run(() => Extract(progress)).ConfigureAwait(false);
                }
            }
            finally { _cts = null; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Cancel
        ///
        /// <summary>
        /// Invokes the cancellation of the current operation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Cancel() => _cts?.Cancel();

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// Saves all of extracted images to the specified directory.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Save(string directory) => Save(directory, _core.Count.Make(i => i));

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// Saves the selected images to the specified directory.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Save(string directory, IEnumerable<int> indices)
        {
            IO.CreateDirectory(directory);
            var basename = IO.Get(Source).BaseName;
            foreach (var index in indices)
            {
                if (index < 0 || index >= _core.Count) continue;
                Save(_core[index], directory, basename, index);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetEnumerator
        ///
        /// <summary>
        /// Gets the enumerator of the collection.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public override IEnumerator<Image> GetEnumerator() => _core.GetEnumerator();

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
        protected override void Dispose(bool disposing)
        {
            lock (_lock)
            {
                if (disposing)
                {
                    Cancel();
                    foreach (var image in _core) image.Dispose();
                    _core.Clear();
                }
            }
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// Extracts images from the specified PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Extract(IProgress<Message<int>> progress)
        {
            try
            {
                var name = IO.Get(Source).BaseName;
                progress.Report(Create(
                    -1,
                    string.Format(Properties.Resources.MessageBegin, name)
                ));

                var result = ExtractImages(progress);

                progress.Report(Create(
                    100,
                    result.Value > 0 ?
                    string.Format(Properties.Resources.MessageEnd, name, result.Key, result.Value) :
                    string.Format(Properties.Resources.MessageNotFound, name, result.Key)
                ));
            }
            catch (OperationCanceledException /* err */)
            {
                progress.Report(Create(0, Properties.Resources.MessageCancel));
            }
            catch (Exception err)
            {
                progress.Report(Create(0, err.Message));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ExtractImages
        ///
        /// <summary>
        /// Extracts images from the specified PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private KeyValuePair<int, int> ExtractImages(IProgress<Message<int>> progress)
        {
            var query   = new Query<string>(e => throw new NotSupportedException());
            var options = new OpenOption { IO = IO, FullAccess = true };
            using (var reader = new DocumentReader(Source, query, options))
            {
                ExtractImages(reader, progress);
                return KeyValuePair.Create(reader.Pages.Count(), _core.Count);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ExtractImages
        ///
        /// <summary>
        /// Extracts images from the specified PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void ExtractImages(DocumentReader src, IProgress<Message<int>> progress)
        {
            var count = src.Pages.Count();
            var name = IO.Get(Source).BaseName;

            for (var i = 0; i < count; ++i)
            {
                _cts.Token.ThrowIfCancellationRequested();

                var pagenum = i + 1;
                progress.Report(Create(
                   (int)(i / (double)count * 100.0),
                   string.Format(Properties.Resources.MessageProcess, name, pagenum, count)
                ));

                var images = src.GetEmbeddedImages(pagenum);
                _cts.Token.ThrowIfCancellationRequested();

                lock (_lock)
                {
                    foreach (var image in images)
                    {
                        _cts.Token.ThrowIfCancellationRequested();
                        _core.Add(image);
                    }
                }
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// Save the specified image to the specified directory.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Save(Image src, string directory, string basename, int index)
        {
            var path = Unique(directory, basename, index);
            src.Save(path, System.Drawing.Imaging.ImageFormat.Png);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Unique
        ///
        /// <summary>
        /// Gets the unique name.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string Unique(string directory, string basename, int index)
        {
            var digit = string.Format("D{0}", _core.Count.ToString("D").Length);
            for (var i = 1; i < 1000; ++i)
            {
                var filename = (i == 1) ?
                               string.Format("{0}-{1}.png", basename, index.ToString(digit)) :
                               string.Format("{0}-{1} ({2}).png", basename, index.ToString(digit), i);
                var dest = IO.Combine(directory, filename);
                if (!IO.Exists(dest)) return dest;
            }

            return IO.Combine(directory, System.IO.Path.GetRandomFileName());
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Creates a new instance of the ProgressMessage(string) class
        /// with the specified arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private Message<int> Create(int percentage, string message) => new Message<int>
        {
            Value = percentage,
            Text  = message,
        };

        #endregion

        #region Fields
        private readonly object _lock = new object();
        private readonly IList<Image> _core = new List<Image>();
        private CancellationTokenSource _cts;
        #endregion
    }
}
