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
using Cube.Pdf.Mixin;
using Cube.Tasks;
using Cube.Xui.Converters;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Cube.Pdf.App.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// ImageList
    ///
    /// <summary>
    /// Provides a collection of images in which contents of Page
    /// objects are rendered.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ImageList : IReadOnlyList<ImageEntry>, INotifyCollectionChanged
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ImageCacheList
        ///
        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        ///
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ImageList(SynchronizationContext context)
        {
            _context = context;
            _inner   = new ObservableCollection<ImageEntry>();
            _cache   = new ConcurrentDictionary<ImageEntry, ImageSource>();
            _doing   = new ConcurrentDictionary<ImageEntry, byte>();

            _inner.CollectionChanged += WhenCollectionChanged;
            Preferences.PropertyChanged += WhenPreferenceChanged;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Items[int]
        ///
        /// <summary>
        /// Gets the element at the specified index.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ImageEntry this[int index] => _inner[index];

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
        /// Renderer
        ///
        /// <summary>
        /// Gets or sets the object to render Page contents.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IDocumentRenderer Renderer { get; set; }

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
        public IEnumerator<ImageEntry> GetEnumerator()
        {
            for (var i = 0; i < Count; ++i) yield return this[i];
        }

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

        /* ----------------------------------------------------------------- */
        ///
        /// Add
        ///
        /// <summary>
        /// Adds the Page object to be rendered.
        /// </summary>
        ///
        /// <param name="item">Page object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Add(Page item)
        {
            Preferences.Register(item.GetDisplaySize());
            _inner.Add(new ImageEntry(e => GetImage(e), Preferences)
            {
                Index     = _inner.Count,
                RawObject = item,
            });
        }

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

            Renderer = null;

            _inner.Clear();
            _cache.Clear();
            _doing.Clear();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Update
        ///
        /// <summary>
        /// Generates new items to show.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Update()
        {
            _task?.Cancel();
            _task = RunTask();
        }

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
        /// SetImage
        ///
        /// <summary>
        /// Stores an ImageSource to the Cache collection and raises
        /// the PropertyChanged event of the specified ImageEntry object.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SetImage(ImageEntry src)
        {
            if (_cache.ContainsKey(src)) return;
            if (!_doing.TryAdd(src, 0)) return;

            try
            {
                var image = new Bitmap(src.Width, src.Height);
                using (var gs = Graphics.FromImage(image))
                {
                    gs.Clear(System.Drawing.Color.White);
                    Renderer.Render(gs, src.RawObject);
                }
                _cache.TryAdd(src, image.ToBitmapImage(true));
                src.Update();
            }
            finally { _doing.TryRemove(src, out var _); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// RunTask
        ///
        /// <summary>
        /// Runs a task that creates ImageSource items.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private CancellationTokenSource RunTask()
        {
            var dest  = new CancellationTokenSource();
            var range = GetVisibleRange();

            Task.Run(() =>
            {
                for (var i = range.Key; i < range.Value; ++i)
                {
                    if (dest.Token.IsCancellationRequested) return;
                    SetImage(_inner[i]);
                }
            }).Forget();

            return dest;
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
            Update();
            OnCollectionChanged(e);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// WhenPropertyChanged
        ///
        /// <summary>
        /// Called when a property of the Preferences is changed.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenPreferenceChanged(object s, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Preferences.VisibleFirst)) Update();
        }

        #endregion

        #region Fields
        private readonly SynchronizationContext _context;
        private readonly ObservableCollection<ImageEntry> _inner;
        private readonly ConcurrentDictionary<ImageEntry, ImageSource> _cache;
        private readonly ConcurrentDictionary<ImageEntry, byte> _doing;
        private CancellationTokenSource _task;
        private ImageSource _dummy;
        #endregion
    }
}
