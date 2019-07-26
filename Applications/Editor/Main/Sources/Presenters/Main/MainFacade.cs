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
using Cube.Mixin.Pdf;
using Cube.Mixin.String;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// MainFacade
    ///
    /// <summary>
    /// Provides functionality to communicate with the MainViewModel and
    /// other model classes.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class MainFacade : DisposableBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// MainFacade
        ///
        /// <summary>
        /// Initializes a new instance of the MainFacade class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="settings">User settings.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public MainFacade(SettingFolder settings, SynchronizationContext context)
        {
            settings.Load();
            settings.PropertyChanged += WhenSettingChanged;

            Settings = settings;
            Cache    = new RendererCache(settings.IO, () => Value.Query);
            Backup   = new Backup(settings.IO);
            Value    = new MainBindable(
                new ImageCollection(e => Cache?.GetOrAdd(e), new ContextInvoker(context, true)),
                settings,
                new ContextInvoker(context, false)
            );
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Value
        ///
        /// <summary>
        /// Gets bindable value related with PDF documents.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public MainBindable Value { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Settings
        ///
        /// <summary>
        /// Gets user settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public SettingFolder Settings { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Backup
        ///
        /// <summary>
        /// Gets the backup handler.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Backup Backup { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Cache
        ///
        /// <summary>
        /// Gets the collection of renderer objects.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RendererCache Cache { get; private set; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Open
        ///
        /// <summary>
        /// Opens a PDF document with the specified file path.
        /// </summary>
        ///
        /// <param name="src">File path.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Open(string src)
        {
            if (!src.HasValue()) return;
            if (Value.Source != null) this.OpenProcess(src.Quote());
            else Invoke(() => {
                Value.Set(Properties.Resources.MessageLoading, src);
                this.Open(Cache.GetOrAdd(src));
            }, "");
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ReOpen
        ///
        /// <summary>
        /// Resets some inner fields.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void ReOpen(string src) => Invoke(() => {
            var doc   = Cache.GetOrAdd(src, Value.Encryption.OwnerPassword);
            var items = doc.Pages.Select((v, i) => new { Value = v, Index = i });
            foreach (var e in items) Value.Images[e.Index].RawObject = e.Value;
            Value.Source = doc.File;
            Value.History.Clear();
        }, "");

        /* ----------------------------------------------------------------- */
        ///
        /// Close
        ///
        /// <summary>
        /// Closes the current PDF document.
        /// </summary>
        ///
        /// <param name="save">Save before closing.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Close(bool save) => Invoke(() =>
        {
            if (save) this.Save(Value.Source.FullName, false);
            Value.Clear(Cache);
        }, "");

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// Saves the PDF document to the specified file path.
        /// </summary>
        ///
        /// <param name="src">Source reader.</param>
        /// <param name="options">Save options.</param>
        /// <param name="prev">Action to be invoked before saving.</param>
        /// <param name="next">Action to be invoked after saving.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Save(IDocumentReader src, SaveOption options, Action<Entity> prev, Action<Entity> next) => Invoke(() =>
        {
            Value.Set(Properties.Resources.MessageSaving, options.Destination);
            var itext = src ?? Value.Source.GetItexReader(Value.Query, Value.IO);
            Value.Set(itext.Metadata, itext.Encryption);
            using (var dest = new SaveAction(itext, Value.Images, options)) dest.Invoke(prev, next);
        }, "");

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
        public void Select(bool selected) => Invoke(() => Value.Images.Select(selected), "");

        /* ----------------------------------------------------------------- */
        ///
        /// Flip
        ///
        /// <summary>
        /// Flips the IsSelected property of all items.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Flip() => Invoke(() => Value.Images.Flip(), "");

        /* ----------------------------------------------------------------- */
        ///
        /// Insert
        ///
        /// <summary>
        /// Inserts the specified files at the specified index.
        /// </summary>
        ///
        /// <param name="index">Insertion index.</param>
        /// <param name="src">Inserting files.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Insert(int index, IEnumerable<string> src) => Invoke(() => Value.Images.InsertAt(
            Math.Min(Math.Max(index, 0), Value.Images.Count),
            src.SelectMany(e =>
            {
                Value.Set(Properties.Resources.MessageLoading, e);
                if (!this.CanInsert(e)) return new Page[0];
                else if (e.IsPdf()) return Cache.GetOrAdd(e).Pages;
                else return Settings.IO.GetImagePages(e);
            })
        ), "");

        /* ----------------------------------------------------------------- */
        ///
        /// Insert
        ///
        /// <summary>
        /// Inserts specified pages at the specified index.
        /// </summary>
        ///
        /// <param name="index">Insertion index.</param>
        /// <param name="src">Collection of pages.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Insert(int index, IEnumerable<Page> src) =>
            Invoke(() => Value.Images.InsertAt(index, src), "");

        /* ----------------------------------------------------------------- */
        ///
        /// Remove
        ///
        /// <summary>
        /// Removes the selected objects.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Remove() => Invoke(() => Value.Images.Remove(), "");

        /* ----------------------------------------------------------------- */
        ///
        /// Remove
        ///
        /// <summary>
        /// Removes the specified objects.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Remove(IEnumerable<int> indices) => Invoke(() => Value.Images.RemoveAt(indices), "");

        /* ----------------------------------------------------------------- */
        ///
        /// Move
        ///
        /// <summary>
        /// Moves the selected objects at the specified distance.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Move(int delta) => Invoke(() => Value.Images.Move(delta), "");

        /* ----------------------------------------------------------------- */
        ///
        /// Rotate
        ///
        /// <summary>
        /// Rotates the selected items with the specified value.
        /// </summary>
        ///
        /// <param name="degree">Angle in degree unit.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Rotate(int degree) => Invoke(() => Value.Images.Rotate(degree), "");

        /* ----------------------------------------------------------------- */
        ///
        /// Update
        ///
        /// <summary>
        /// Updates the Metadata object.
        /// </summary>
        ///
        /// <param name="value">Metadata object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Update(Metadata value) => Invoke(() => Value.SetMetadata(value), "");

        /* ----------------------------------------------------------------- */
        ///
        /// Update
        ///
        /// <summary>
        /// Updates the Encryption object.
        /// </summary>
        ///
        /// <param name="value">Encryption object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Update(Encryption value) => Invoke(() => Value.SetEncryption(value), "");

        /* ----------------------------------------------------------------- */
        ///
        /// Rotate
        ///
        /// <summary>
        /// Rotates the selected items with the specified value.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Undo() => Invoke(() => Value.History.Undo(), "");

        /* ----------------------------------------------------------------- */
        ///
        /// Rotate
        ///
        /// <summary>
        /// Rotates the selected items with the specified value.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Redo() => Invoke(() => Value.History.Redo(), "");

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
        public void Zoom(int offset) => Invoke(() =>
        {
            Value.Images.Zoom(offset);
            Value.ItemSize = Value.Images.Preferences.ItemSize;
        }, "");

        /* ----------------------------------------------------------------- */
        ///
        /// Redraw
        ///
        /// <summary>
        /// Clears all of images and regenerates them.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Redraw() => Invoke(() => Value.Images.Redraw(), "");

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases the unmanaged resources used by the MainFacade
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
            var cache = Cache;
            Cache = null;
            Value.Clear(cache);
            if (disposing) Value.Images.Dispose();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Invokes the user action and registers the history item.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Invoke(Func<HistoryItem> func, string format, params object[] args) =>
            Invoke(() => Value.History.Register(func()), format, args);

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Invokes the user action and sets the result message.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Invoke(Action action, string format, params object[] args) => Value.Invoke(() =>
        {
            if (Cache == null) return;
            action();
            Value.Set(format, args);
        });

        /* ----------------------------------------------------------------- */
        ///
        /// WhenSettingChanged
        ///
        /// <summary>
        /// Occurs when the PropertyChanged event is fired.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenSettingChanged(object s, PropertyChangedEventArgs e)
        {
            var src = Settings.Value;
            if (new Dictionary<string, Action> {
                { nameof(src.ItemSize),  () => this.Zoom() },
                { nameof(src.FrameOnly), () => Value.Images.Preferences.FrameOnly = src.FrameOnly },
            }.TryGetValue(e.PropertyName, out var action)) action();
        }

        #endregion
    }
}
