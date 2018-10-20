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
using Cube.Collections;
using Cube.Collections.Mixin;
using Cube.Log;
using Cube.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Cube.Pdf.App.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// ImageCollection
    ///
    /// <summary>
    /// Provides a collection of images in which contents of Page
    /// objects are rendered.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ImageCollection : DisposableBase, IReadOnlyList<ImageItem>, INotifyCollectionChanged
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
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ImageCollection(Func<string, IDocumentRenderer> getter, SynchronizationContext context)
        {
            ImageSource create(ImageItem e) => getter(e.RawObject.File.FullName)?.Create(e);
            void update(string s) { if (s == nameof(Preferences.VisibleLast)) Reschedule(null); };

            _inner = new ObservableCollection<ImageItem>();
            _cache = new CacheCollection<ImageItem, ImageSource>(create);

            _inner.CollectionChanged += (s, e) => OnCollectionChanged(e);
            _cache.Created += (s, e) => e.Key.Refresh();
            _cache.Failed  += (s, e) => this.LogDebug($"[{e.Key.Index}] {e.Value.GetType().Name}");

            Selection   = new ImageSelection   { Context = context };
            Preferences = new ImagePreferences { Context = context };

            Create = (i, r) =>
            {
                if (i < 0 || i >= Count) return null;
                var src = _inner[i].RawObject;
                return getter(src.File.FullName)?.Create(src, r);
            };

            Convert = (e) => Preferences.FrameOnly ? null :
                             _cache.TryGetValue(e, out var dest) ? dest :
                             Preferences.Dummy;

            Preferences.PropertyChanged += (s, e) => update(e.PropertyName);
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
        public ImagePreferences Preferences { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Gets the function to create a new image with the specified
        /// arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Func<int, double, ImageSource> Create { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Convert
        ///
        /// <summary>
        /// Gets the function to convert from an ImageItem object to the
        /// ImageSource object.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Func<ImageItem, ImageSource> Convert { get; }

        #endregion

        #region Events

        /* ----------------------------------------------------------------- */
        ///
        /// CollectionChanged
        ///
        /// <summary>
        /// Occurs when an item is added, removed, changed, moved,
        /// or the entire list is refreshed.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public event NotifyCollectionChangedEventHandler CollectionChanged;

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
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            Reschedule(null);
            if (CollectionChanged == null) return;
            if (Preferences.Context == null) CollectionChanged(this, e);
            else Preferences.Context.Send(_ => CollectionChanged(this, e), null);
        }

        #endregion

        #region Methods

        #region IEnumerable

        /* ----------------------------------------------------------------- */
        ///
        /// GetEnumerator
        ///
        /// <summary>
        /// Returns an enumerator that iterates through this collection.
        /// </summary>
        ///
        /// <returns>
        /// An IEnumerator(ImageEntry) object for this collection.
        /// </returns>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerator<ImageItem> GetEnumerator() => _inner.GetEnumerator();

        /* ----------------------------------------------------------------- */
        ///
        /// IEnumerable.GetEnumerator
        ///
        /// <summary>
        /// Returns an enumerator that iterates through this collection.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

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
        public void Add(IEnumerable<Page> items)
        {
            foreach (var item in items) _inner.Add(this.CreateEntry(Count, item));
        }

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
                _inner.Insert(pos, this.CreateEntry(pos, item));
                ++pos;
            }
            return KeyValuePair.Create(pos, Count);
        });

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
                _cache.Remove(item);
                item.Rotate(degree);
            }
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Move
        ///
        /// <summary>
        /// Moves the specified items at the specfied distance.
        /// </summary>
        ///
        /// <param name="indices">Target items.</param>
        /// <param name="delta">Moving distance.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Move(IEnumerable<int> indices, int delta) => SetIndex(() =>
        {
            var tmp = indices.Within(Count);
            var src = delta < 0 ? tmp.OrderBy() : tmp.OrderByDescending();
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
            return KeyValuePair.Create(min, max + 1);
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
            var src = indices.Within(Count).OrderByDescending().ToList();
            foreach (var item in src.Select(i => _inner[i]))
            {
                _cache.Remove(item);
                _inner.Remove(item);
                item.Dispose();
            }
            return KeyValuePair.Create(src.LastOrDefault(), Count);
        });

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
        /// Refresh
        ///
        /// <summary>
        /// Removes all images and regenerates them.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Refresh() => Reschedule(() => _cache.Clear());

        #endregion

        #region Implementations

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
                    _cache.GetOrCreate(_inner[i]);
                }
            }).Forget();
        }

        #endregion

        #region Fields
        private readonly ObservableCollection<ImageItem> _inner;
        private readonly CacheCollection<ImageItem, ImageSource> _cache;
        private CancellationTokenSource _task;
        #endregion
    }
}
