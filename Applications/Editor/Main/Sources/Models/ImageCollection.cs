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
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Cube.Collections;
using Cube.Collections.Extensions;
using Cube.Pdf.Extensions;
using Cube.Syntax.Extensions;
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

            _inner = [];
            _inner.CollectionChanged += (_, e) => OnCollectionChanged(e);

            _cache = new(GetCreator, (_, e) => TrackReleasedCache(e));
            _cache.Created += (_, e) => e.Key.Refresh();
            _cache.Failed  += (_, e) => Logger.Warn($"[{e.Key.Index}] {e.Value.GetType().Name}");

            Dispatcher  = dispatcher;
            Selection   = new ImageSelection(dispatcher);
            Preferences = new ImagePreference(dispatcher);
            Preferences.PropertyChanged += WhenPreferencesChanged;
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
        public void Add(IEnumerable<Page> items) =>
            items.Each(e => Dispatcher.Invoke(() => _inner.Add(this.NewItem(Count, e))));

        /* ----------------------------------------------------------------- */
        ///
        /// Clear
        ///
        /// <summary>
        /// Clears all objects.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Clear() => Dispatcher.Invoke(() =>
        {
            _operation?.Abort();
            _operation = null;
            var items = _inner.ToList();
            _inner.Clear();
            _cache.Clear();
            foreach (var item in items) item.Dispose();
            ResetGcTimer(force: items.Count > 0);
        });

        #region ResetIndex required

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
        public void Insert(int index, IEnumerable<Page> items) => ResetIndex(() =>
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
        public void Move(IEnumerable<int> indices, int delta) => ResetIndex(() =>
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
        public void Remove(IEnumerable<int> indices) => ResetIndex(() =>
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

        #region Reschedule required

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
            ClearCache();
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
        public void Redraw() => Reschedule(() => ClearCache());

        #endregion

        /* ----------------------------------------------------------------- */
        ///
        /// Suspend
        ///
        /// <summary>
        /// Stops scheduling thumbnails and invalidates outstanding requests
        /// while the source document is being replaced.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        internal void Suspend() => Dispatcher.Invoke(() =>
        {
            _suspended = true;
            ClearCache();
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Resume
        ///
        /// <summary>
        /// Resumes thumbnail creation for the visible pages after a save.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        internal void Resume() => Dispatcher.Invoke(() =>
        {
            if (!_suspended) return;
            _suspended = false;
            Schedule();
        });

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
        protected override void Dispose(bool disposing)
        {
            if (!disposing) return;
            Clear();
            Dispatcher.Invoke(StopGcTimer);
        }

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
            if (!_editing && Count > 0) ResetVisibleRange();
            base.OnCollectionChanged(e);
        }

        #endregion

        #region Implementations

        #region Event handlers

        /* ----------------------------------------------------------------- */
        ///
        /// WhenPreferencesChanged
        ///
        /// <summary>
        /// Updates the thumbnails when the viewport or display mode changes.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenPreferencesChanged(object s, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Preferences.VisibleFirst):
                case nameof(Preferences.VisibleLast):
                    ResetVisibleRange();
                    break;
                case nameof(Preferences.FrameOnly):
                    Reschedule(() => ClearCache(all: true));
                    break;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// WhenIdle
        ///
        /// <summary>
        /// Requests garbage collection after thumbnail activity has settled.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenIdle(object s, EventArgs e)
        {
            StopGcTimer();
            Interlocked.Exchange(ref _released, 0);
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, false);
        }

        #endregion

        #region Getter

        /* ----------------------------------------------------------------- */
        ///
        /// GetImageSource
        ///
        /// <summary>
        /// Gets the ImageSource object from the specified image.
        /// </summary>
        ///
        /// <param name="src">Image item.</param>
        ///
        /// <returns>ImageSource object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        internal ImageSource GetImageSource(ImageItem src) =>
            Preferences.FrameOnly ? null :
            _cache.TryGetValue(src, out var dest) ? dest : Preferences.Dummy;

        /* ----------------------------------------------------------------- */
        ///
        /// GetCreator
        ///
        /// <summary>
        /// Captures the page, renderer, and size before background rendering starts.
        /// </summary>
        ///
        /// <param name="item">Item to render.</param>
        ///
        /// <returns>Operation that creates the thumbnail.</returns>
        ///
        /* ----------------------------------------------------------------- */
        private Func<ImageSource> GetCreator(ImageItem item)
        {
            var page = item.RawObject;
            var renderer = _getter(page.File.FullName);
            var size = new SizeF(item.Width, item.Height);
            return () => renderer?.Render(page, size)?.ToBitmapImage(true);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetUiDispatcher
        ///
        /// <summary>
        /// Gets the current WPF dispatcher if its synchronization context
        /// is active.
        /// </summary>
        ///
        /// <remarks>
        /// A worker thread can own a dispatcher without running a message loop.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private static System.Windows.Threading.Dispatcher GetUiDispatcher() =>
            SynchronizationContext.Current is DispatcherSynchronizationContext ?
            System.Windows.Threading.Dispatcher.FromThread(Thread.CurrentThread) : null;

        #endregion

        #region Cache operations

        /* ----------------------------------------------------------------- */
        ///
        /// TrimCache
        ///
        /// <summary>
        /// Drops cached images and pending requests outside the retained range.
        /// </summary>
        ///
        /// <param name="first">Inclusive start of the retained range.</param>
        /// <param name="last">Exclusive end of the retained range.</param>
        ///
        /* ----------------------------------------------------------------- */
        private void TrimCache(int first, int last)
        {
            foreach (var item in _cache.Keys)
            {
                if (item.Index >= first && item.Index < last) continue;
                if (_cache.Remove(item)) item.Refresh();
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ClearCache
        ///
        /// <summary>
        /// Invalidates cached images and refreshes the affected items.
        /// </summary>
        ///
        /// <param name="all">Refresh even uncached items when their display mode changes.</param>
        ///
        /* ----------------------------------------------------------------- */
        private void ClearCache(bool all = false)
        {
            IEnumerable<ImageItem> items = all ? _inner : _cache.Keys;
            _cache.Clear();
            foreach (var item in items) item.Refresh();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// TrackReleasedCache
        ///
        /// <summary>
        /// Accumulates the estimated pixel bytes of discarded thumbnails.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void TrackReleasedCache(ImageSource image)
        {
            if (image is BitmapSource bmp)
            {
                var bytes = (long)bmp.PixelWidth * bmp.PixelHeight * 4;
                Interlocked.Add(ref _released, bytes);
            }
        }

        #endregion

        #region GC Timer operations

        /* ----------------------------------------------------------------- */
        ///
        /// ResetGcTimer
        ///
        /// <summary>
        /// Postpones collection until thumbnail activity has settled.
        /// </summary>
        ///
        /// <param name="force">Schedule collection even below the byte threshold.</param>
        ///
        /* ----------------------------------------------------------------- */
        private void ResetGcTimer(bool force = false)
        {
            if (Disposed) return;
            if (!force && _gctimer == null && Interlocked.Read(ref _released) < GcThreshold) return;
            var ui = GetUiDispatcher();
            if (ui == null || ui.HasShutdownStarted) return;

            if (_gctimer == null)
            {
                _gctimer = new(DispatcherPriority.ApplicationIdle, ui) { Interval = TimeSpan.FromMilliseconds(500) };
                _gctimer.Tick += WhenIdle;
            }
            _gctimer.Stop();
            _gctimer.Start();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// StopGcTimer
        ///
        /// <summary>
        /// Cancels the pending collection and detaches the timer handler.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void StopGcTimer()
        {
            if (_gctimer == null) return;
            _gctimer.Stop();
            _gctimer.Tick -= WhenIdle;
            _gctimer = null;
        }

        #endregion

        /* ----------------------------------------------------------------- */
        ///
        /// ResetIndex
        ///
        /// <summary>
        /// Resets the index in the specified items.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void ResetIndex(Func<KeyValuePair<int, int>> before) => Dispatcher.Invoke(() =>
        {
            _editing = true;
            try
            {
                var pos = before();
                var min = Math.Max(pos.Key, 0);
                var max = Math.Min(pos.Value, Count);
                for (var i = min; i < max; ++i) _inner[i].Index = i;
            }
            finally
            {
                _editing = false;
                Reschedule();
            }
        });

        /* ----------------------------------------------------------------- */
        ///
        /// ResetVisibleRange
        ///
        /// <summary>
        /// Coalesces changes to both ends of the viewport into one update.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void ResetVisibleRange()
        {
            var ui = GetUiDispatcher();
            if (ui == null) { Reschedule(); return; }
            if (_operation != null) return;

            _operation = ui.BeginInvoke(DispatcherPriority.Background, new Action(() => {
                _operation = null;
                Reschedule();
            }));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Reschedule
        ///
        /// <summary>
        /// Updates the collection on its dispatcher before scheduling thumbnails.
        /// </summary>
        ///
        /// <param name="before">Action to run before scheduling.</param>
        ///
        /* ----------------------------------------------------------------- */
        private void Reschedule(Action before = null) => Dispatcher.Invoke(() =>
        {
            if (Disposed) return;
            before?.Invoke();
            Schedule();
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Schedule
        ///
        /// <summary>
        /// Queues thumbnails for the visible pages on the collection dispatcher.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Schedule()
        {
            if (Disposed) return;
            if (!_suspended && !Preferences.FrameOnly)
            {
                var first = Math.Max(Preferences.VisibleFirst, 0);
                var last  = Math.Min(Preferences.VisibleLast, Count);
                var span  = Math.Max(last - first, 0);
                TrimCache(first - span, last + span);
                for (var i = first; i < last; ++i) _ = _cache.GetOrCreate(_inner[i]);
            }
            ResetGcTimer();
        }

        #endregion

        #region Fields
        private const long GcThreshold = 16L * 1024 * 1024;
        private readonly Func<string, IDocumentRenderer> _getter;
        private readonly ObservableCollection<ImageItem> _inner;
        private readonly CacheCollection<ImageItem, ImageSource> _cache;
        private DispatcherOperation _operation;
        private DispatcherTimer _gctimer;
        private bool _suspended;
        private bool _editing;
        private long _released;
        #endregion
    }
}
