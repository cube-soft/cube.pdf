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
using Cube.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
    public class ImageCollection : IReadOnlyCollection<ImageEntry>, INotifyCollectionChanged
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ImageCollection
        ///
        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        ///
        /// <param name="getter">
        /// Function for getting IDocumentRenderer objects.
        /// </param>
        ///
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ImageCollection(Func<string, IDocumentRenderer> getter, SynchronizationContext context)
        {
            _engine  = getter;
            _context = context;
            _cache   = new CacheCollection<ImageEntry, ImageSource>(e => CreateImage(e));

            _cache.Created += (s, e) => e.Key.Refresh();
            _inner.CollectionChanged += WhenCollectionChanged;
            Preferences.PropertyChanged += WhenPreferenceChanged;
        }

        #endregion

        #region Properties

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
        public ImageSelection Selection { get; } = new ImageSelection();

        /* ----------------------------------------------------------------- */
        ///
        /// Preferences
        ///
        /// <summary>
        /// Gets the preferences for images.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ImagePreferences Preferences { get; } = new ImagePreferences();

        /* ----------------------------------------------------------------- */
        ///
        /// LoadingImage
        ///
        /// <summary>
        /// Gets the image object representing loading.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ImageSource LoadingImage
        {
            get => _dummy ?? (_dummy = GetLoadingImage());
            set => _dummy = value;
        }

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
            if (CollectionChanged == null) return;
            if (_context != null) _context.Send(_ => CollectionChanged(this, e), null);
            else CollectionChanged(this, e);
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
        public IEnumerator<ImageEntry> GetEnumerator() => _inner.GetEnumerator();

        /* ----------------------------------------------------------------- */
        ///
        /// IEnumerable.GetEnumerator
        ///
        /// <summary>
        /// Returns an enumerator that iterates through this collection.
        /// </summary>
        ///
        /// <returns>
        /// An IEnumerator object for this collection.
        /// </returns>
        ///
        /* ----------------------------------------------------------------- */
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        #region Editing

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
            foreach (var item in items) _inner.Add(CreateEntry(_inner.Count, item));
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
        public void Insert(int index, IEnumerable<Page> items) => RestructIndex(() =>
        {
            var pos = index;
            foreach (var item in items)
            {
                _inner.Insert(pos, CreateEntry(pos, item));
                ++pos;
            }
            return KeyValuePair.Create(pos, _inner.Count);
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Remove
        ///
        /// <summary>
        /// Removes the selected images.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Remove() => Remove(Selection.Indices);

        /* ----------------------------------------------------------------- */
        ///
        /// Remove
        ///
        /// <summary>
        /// Removes the specified images.
        /// </summary>
        ///
        /// <param name="indecies">Indices for removal items.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Remove(IEnumerable<int> indecies) => RestructIndex(() =>
        {
            var n   = _inner.Count;
            var pos = int.MaxValue;
            var src = indecies.Where(i => i >= 0 && i < n).OrderByDescending(i => i);

            foreach (var index in src)
            {
                var item = _inner[index];
                _cache.Remove(item);
                _inner.Remove(item);
                item.Dispose();
                pos = index;
            }
            return KeyValuePair.Create(pos, n);
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Clear
        ///
        /// <summary>
        /// Clears all of images and related objects.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Clear()
        {
            _task?.Cancel();
            _task = null;

            foreach (var item in _inner) item.Dispose();

            _inner.Clear();
            _cache.Clear();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Move
        ///
        /// <summary>
        /// Moves the selected images at the specfied distance.
        /// </summary>
        ///
        /// <param name="delta">Moving distance.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Move(int delta) => RestructIndex(() =>
        {
            var n   = _inner.Count;
            var min = int.MaxValue;
            var max = 0;
            var tmp = Selection.Indices.Where(i => i >= 0 && i < n);
            var src = delta < 0 ? tmp.OrderBy(i => i) : tmp.OrderByDescending(i => i);

            foreach (var index in src)
            {
                var inew = Math.Min(Math.Max(index + delta, 0), n - 1);
                if (inew == index) continue;

                _inner.Move(index, inew);
                min = Math.Min(Math.Min(min, index), inew);
                max = Math.Max(Math.Max(max, index), inew);
            }
            return KeyValuePair.Create(min, max + 1);
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Rotate
        ///
        /// <summary>
        /// Rotates the selected images and regenerates them.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Rotate(int degree) => RestartTask(() =>
        {
            foreach (var item in Selection.Items)
            {
                _cache.Remove(item);
                item.Rotate(degree);
            }
        });

        #endregion

        #region Others

        /* ----------------------------------------------------------------- */
        ///
        /// Select
        ///
        /// <summary>
        /// Sets the IsSelected property of all items to be the specified
        /// value.
        /// </summary>
        ///
        /// <param name="selected">true for selected.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Select(bool selected)
        {
            foreach (var item in _inner) item.IsSelected = selected;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Flip
        ///
        /// <summary>
        /// Flips the section of items.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Flip()
        {
            foreach (var item in _inner) item.IsSelected = !item.IsSelected;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Zoom
        ///
        /// <summary>
        /// Updates the scale ratio at the specified offset.
        /// </summary>
        ///
        /// <param name="offset">
        /// Offset for the index in the item size collection.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        public void Zoom(int offset) => RestartTask(() =>
        {
            _cache.Clear();
            Preferences.ItemSizeIndex += offset;
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Refresh
        ///
        /// <summary>
        /// Removes all of images and regenerates them.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Refresh() => RestartTask(() => _cache.Clear());

        #endregion

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetVisibleRange
        ///
        /// <summary>
        /// Gets the current visible range of the image collection.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private KeyValuePair<int, int> GetVisibleRange() => KeyValuePair.Create(
            Math.Max(Preferences.VisibleFirst, 0),
            Math.Min(Preferences.VisibleLast, _inner.Count)
        );

        /* ----------------------------------------------------------------- */
        ///
        /// GetLoadingImage
        ///
        /// <summary>
        /// Gets a default ImageSource that notifies loading.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ImageSource GetLoadingImage() =>
            new BitmapImage(new Uri("pack://application:,,,/Assets/Medium/Loading.png"));

        /* ----------------------------------------------------------------- */
        ///
        /// GetImage
        ///
        /// <summary>
        /// Gets an ImageSource from the specified object.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ImageSource GetImage(ImageEntry src) =>
            _cache.TryGetValue(src, out var dest) ? dest : LoadingImage;

        /* ----------------------------------------------------------------- */
        ///
        /// CreateEntry
        ///
        /// <summary>
        /// Creats a new ImageEntry object.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ImageEntry CreateEntry(int index, Page item) =>
            new ImageEntry(e => GetImage(e), Selection, Preferences)
            {
                Index     = index,
                RawObject = item,
            };

        /* ----------------------------------------------------------------- */
        ///
        /// CreateImage
        ///
        /// <summary>
        /// Creates a new instance of the ImageSource class from the
        /// specified entry.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ImageSource CreateImage(ImageEntry src) =>
            _engine(src.RawObject.File.FullName).Create(src);

        /* ----------------------------------------------------------------- */
        ///
        /// RestartTask
        ///
        /// <summary>
        /// Runs a task that creates ImageSource items.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void RestartTask(Action before)
        {
            before?.Invoke();

            var range = GetVisibleRange();
            var cts   = new CancellationTokenSource();

            _task?.Cancel();
            _task = cts;

            Task.Run(() =>
            {
                for (var i = range.Key; i < range.Value; ++i)
                {
                    if (cts.Token.IsCancellationRequested) return;
                    _cache.GetOrCreate(_inner[i]);
                }
            }).Forget();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// RestructIndex
        ///
        /// <summary>
        /// Updtes the Index property of the specified items.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void RestructIndex(Func<KeyValuePair<int, int>> func)
        {
            var pos   = func();
            var begin = Math.Max(pos.Key, 0);
            var end   = Math.Min(pos.Value, _inner.Count);
            for (var i = begin; i < end; ++i) _inner[i].Index = i;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// WhenCollectionChanged
        ///
        /// <summary>
        /// Called when the collection is changed.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenCollectionChanged(object s, NotifyCollectionChangedEventArgs e)
        {
            RestartTask(null);
            OnCollectionChanged(e);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// WhenPreferenceChanged
        ///
        /// <summary>
        /// Called when a property of the Preferences is changed.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenPreferenceChanged(object s, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Preferences.VisibleLast)) RestartTask(null);
        }

        #endregion

        #region Fields
        private readonly Func<string, IDocumentRenderer> _engine;
        private readonly SynchronizationContext _context;
        private readonly CacheCollection<ImageEntry, ImageSource> _cache;
        private readonly ObservableCollection<ImageEntry> _inner = new ObservableCollection<ImageEntry>();
        private CancellationTokenSource _task;
        private ImageSource _dummy;
        #endregion
    }
}
