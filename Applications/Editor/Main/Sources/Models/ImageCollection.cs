﻿/* ------------------------------------------------------------------------- */
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
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using Cube.Collections;
using Cube.Collections.Extensions;
using Cube.Pdf.Extensions;
using Cube.Syntax.Extensions;
using Cube.Tasks.Extensions;
using Cube.Xui.Drawing.Extensions;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// ImageCollection
    ///
    /// <summary>
    /// Represents a collection of images in which contents of Page
    /// objects are rendered.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class ImageCollection : ObservableCollectionBase<ImageItem>, IReadOnlyList<ImageItem>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ImageCollection
        ///
        /// <summary>
        /// Initializes a new instance of the ImageCollection class with
        /// the specified arguments.
        /// </summary>
        ///
        /// <param name="getter">Function to get the renderer.</param>
        /// <param name="dispatcher">Dispatcher object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ImageCollection(Func<string, IDocumentRenderer> getter, Dispatcher dispatcher)
        {
            _getter = getter;

            _inner = new();
            _inner.CollectionChanged += (s, e) => OnCollectionChanged(e);

            _cache = new(e => getter(e.RawObject.File.FullName)
                ?.Render(e.RawObject, new(e.Width, e.Height))
                ?.ToBitmapImage(true)
            );
            _cache.Created += (s, e) => e.Key.Refresh();
            _cache.Failed  += (s, e) => Logger.Warn($"[{e.Key.Index}] {e.Value.GetType().Name}");

            Dispatcher  = dispatcher;
            Selection   = new ImageSelection(dispatcher);
            Preferences = new ImagePreference(dispatcher);
            Preferences.PropertyChanged += (s, e) => {
                if (e.PropertyName == nameof(Preferences.VisibleLast)) Reschedule(null);
            };
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Item(int)
        ///
        /// <summary>
        /// Gets the element at the specified index.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ImageItem this[int index] => _inner[index];

        /* ----------------------------------------------------------------- */
        ///
        /// Count
        ///
        /// <summary>
        /// Gets the number of elements contained in this collection.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Count => _inner.Count;

        /* ----------------------------------------------------------------- */
        ///
        /// Selection
        ///
        /// <summary>
        /// Gets the selection of elements.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ImageSelection Selection { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Preferences
        ///
        /// <summary>
        /// Gets the preferences for images.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ImagePreference Preferences { get; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Add
        ///
        /// <summary>
        /// Adds the Page object to be rendered.
        /// </summary>
        ///
        /// <param name="items">Page collection.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Add(IEnumerable<Page> items) => items.Each(e => _inner.Add(this.NewItem(Count, e)));

        /* ----------------------------------------------------------------- */
        ///
        /// Clear
        ///
        /// <summary>
        /// Clears all objects.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Clear()
        {
            Interlocked.Exchange(ref _task, null)?.Cancel();
            foreach (var item in _inner) item.Dispose();
            _inner.Clear();
            _cache.Clear();
        }

        #region SetIndex

        /* ----------------------------------------------------------------- */
        ///
        /// Insert
        ///
        /// <summary>
        /// Insert the objects at the specified index.
        /// </summary>
        ///
        /// <param name="index">Insertion index.</param>
        /// <param name="items">Page collection.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Insert(int index, IEnumerable<Page> items) => SetIndex(() =>
        {
            var pos = index;
            foreach (var item in items)
            {
                _inner.Insert(pos, this.NewItem(pos, item));
                ++pos;
            }
            return new(pos, Count);
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Move
        ///
        /// <summary>
        /// Moves the specified items at the specified distance.
        /// </summary>
        ///
        /// <param name="indices">Target items.</param>
        /// <param name="delta">Moving distance.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Move(IEnumerable<int> indices, int delta) => SetIndex(() =>
        {
            var tmp = indices.Within(Count);
            var src = delta < 0 ? tmp.OrderBy(i => i) : tmp.OrderByDescending(i => i);
            var min = int.MaxValue;
            var max = 0;

            foreach (var index in src)
            {
                var inew = Math.Min(Math.Max(index + delta, 0), Count - 1);
                if (inew != index)
                {
                    _inner.Move(index, inew);
                    min = Math.Min(Math.Min(min, index), inew);
                    max = Math.Max(Math.Max(max, index), inew);
                }
            }
            return new(min, max + 1);
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Remove
        ///
        /// <summary>
        /// Removes the specified images.
        /// </summary>
        ///
        /// <param name="indices">Indices for removal items.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Remove(IEnumerable<int> indices) => SetIndex(() =>
        {
            var src = indices.Within(Count).OrderByDescending(i => i).ToList();
            foreach (var item in src.Select(i => _inner[i]))
            {
                _ = _cache.Remove(item);
                _ = _inner.Remove(item);
                item.Dispose();
            }
            return new(src.LastOrDefault(), Count);
        });

        #endregion

        #region Reschedule

        /* ----------------------------------------------------------------- */
        ///
        /// Rotate
        ///
        /// <summary>
        /// Rotates the specified images and regenerates them.
        /// </summary>
        ///
        /// <param name="indices">Target items.</param>
        /// <param name="degree">Rotation angle in degree unit.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Rotate(IEnumerable<int> indices, int degree) => Reschedule(() =>
        {
            foreach (var item in indices.Within(Count).Select(i => _inner[i]))
            {
                _ = _cache.Remove(item);
                item.Rotate(degree);
            }
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Zoom
        ///
        /// <summary>
        /// Updates the scale ratio at the specified offset.
        /// </summary>
        ///
        /// <param name="offset">Offset for the ItemSizeIndex.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Zoom(int offset) => Reschedule(() =>
        {
            _cache.Clear();
            Preferences.ItemSizeIndex += offset;
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Redraw
        ///
        /// <summary>
        /// Removes all images and regenerates them.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Redraw() => Reschedule(_cache.Clear);

        #endregion

        /* ----------------------------------------------------------------- */
        ///
        /// GetImage
        ///
        /// <summary>
        /// Gets an Image object according to the specified arguments.
        /// </summary>
        ///
        /// <param name="index">Index of the image collection.</param>
        /// <param name="ratio">Image ratio.</param>
        ///
        /// <returns>Image object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public Image GetImage(int index, double ratio)
        {
            if (index < 0 || index >= Count) return null;
            var src = _inner[index].RawObject;
            return _getter(src.File.FullName)?.Render(src, src.GetViewSize(ratio));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetEnumerator
        ///
        /// <summary>
        /// Returns an enumerator that iterates through this collection.
        /// </summary>
        ///
        /// <returns>
        /// An IEnumerator(ImageItem) object for this collection.
        /// </returns>
        ///
        /* ----------------------------------------------------------------- */
        public override IEnumerator<ImageItem> GetEnumerator() => _inner.GetEnumerator();

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases the unmanaged resources used by the
        /// ImageCollection and optionally releases the managed
        /// resources.
        /// </summary>
        ///
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        protected override void Dispose(bool disposing) { if (disposing) Clear(); }

        /* ----------------------------------------------------------------- */
        ///
        /// OnCollectionChanged
        ///
        /// <summary>
        /// Raises the CollectionChanged event with the provided arguments.
        /// </summary>
        ///
        /// <param name="e">Arguments of the event being raised.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            Reschedule(null);
            base.OnCollectionChanged(e);
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// SetIndex
        ///
        /// <summary>
        /// Updates the index in the specified items.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SetIndex(Func<KeyValuePair<int, int>> before)
        {
            var pos = before();
            var min = Math.Max(pos.Key, 0);
            var max = Math.Min(pos.Value, Count);
            for (var i = min; i < max; ++i) _inner[i].Index = i;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Reschedule
        ///
        /// <summary>
        /// Restarts a task that creates images.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Reschedule(Action before)
        {
            if (Disposed) return;

            var cts = new CancellationTokenSource();
            Interlocked.Exchange(ref _task, cts)?.Cancel();
            before?.Invoke();

            var min = Math.Max(Preferences.VisibleFirst, 0);
            var max = Math.Min(Preferences.VisibleLast, Count);

            Task.Run(() =>
            {
                for (var i = min; i < max; ++i)
                {
                    if (Disposed || cts.Token.IsCancellationRequested) return;
                    _ = _cache.GetOrCreate(_inner[i]);
                }
            }).Forget();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetImageSource
        ///
        /// <summary>
        /// Gets the ImageSource object.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        internal ImageSource GetImageSource(ImageItem src) =>
            Preferences.FrameOnly ? null :
            _cache.TryGetValue(src, out var dest) ? dest :
            Preferences.Dummy;

        #endregion

        #region Fields
        private readonly Func<string, IDocumentRenderer> _getter;
        private readonly ObservableCollection<ImageItem> _inner;
        private readonly CacheCollection<ImageItem, ImageSource> _cache;
        private CancellationTokenSource _task;
        #endregion
    }
}
