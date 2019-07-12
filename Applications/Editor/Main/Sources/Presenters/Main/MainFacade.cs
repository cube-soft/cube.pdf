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
using Cube.Mixin.Collections;
using Cube.Mixin.Pdf;
using Cube.Mixin.String;
using System;
using System.Collections.Generic;
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
    public class MainFacade : DisposableBase
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
        /// <param name="src">User settings.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public MainFacade(SettingFolder src, SynchronizationContext context)
        {
            var images = new ImageCollection(e => _core?.GetOrAdd(e), new Dispatcher(context, true));
            var post   = new Dispatcher(context, false);

            _core    = new DocumentCollection(src.IO, () => Query);
            Backup   = new Backup(src.IO);
            Bindable = new MainBindable(images, src, post);

            Settings = src;
            Settings.Load();
            Settings.PropertyChanged += (s, e) => Update(e.PropertyName);

            var sizes = Bindable.Images.Preferences.ItemSizeOptions;
            var index = sizes.LastIndex(e => e <= Bindable.ItemSize.Value);

            Bindable.Images.Preferences.ItemSizeIndex = Math.Max(index, 0);
            Bindable.Images.Preferences.FrameOnly     = src.Value.FrameOnly;
            Bindable.Images.Preferences.TextHeight    = 25;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Bindable
        ///
        /// <summary>
        /// Gets bindable data related with PDF documents.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public MainBindable Bindable { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Setting
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
        /// Backup
        ///
        /// <summary>
        /// Gets or sets the password query.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IQuery<string> Query
        {
            get => Bindable.Query;
            set => Bindable.Query = value;
        }

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
            if (Bindable.IsOpen()) this.StartProcess(src.Quote());
            else Invoke(() =>
            {
                Bindable.SetMessage(Properties.Resources.MessageLoading, src);
                Bindable.Open(_core.GetOrAdd(src));
            }, "");
        }

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
        public void Close(bool save)
        {
            if (save) Save(Bindable.Source.Value.FullName, false);
            Invoke(() =>
            {
                _core.Clear();
                Bindable.Close();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }, "");
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// Saves the PDF document to the specified file path.
        /// </summary>
        ///
        /// <param name="dest">File path.</param>
        /// <param name="reopen">
        /// Value indicating whether restructuring some inner fields
        /// after saving.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        public void Save(string dest, bool reopen) => Invoke(() =>
        {
            Bindable.SetMessage(Properties.Resources.MessageSaving, dest);
            this.Save(Settings.IO.Get(dest), () => _core.Clear());
            if (reopen) this.Restruct(_core.GetOrAdd(dest, Bindable.Encryption.OwnerPassword));
        }, "");

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// Saves the selected PDF objects as the specified filename.
        /// </summary>
        ///
        /// <param name="dest">Save path.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Extract(string dest) => Invoke(() => Bindable.Images.Extract(dest),
            Properties.Resources.MessageSaved, dest);

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
        public void Select(bool selected) => Invoke(() => Bindable.Images.Select(selected), "");

        /* ----------------------------------------------------------------- */
        ///
        /// Flip
        ///
        /// <summary>
        /// Flips the IsSelected property of all items.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Flip() => Invoke(() => Bindable.Images.Flip(), "");

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
        public void Insert(int index, IEnumerable<string> src) => Invoke(() =>
            Bindable.Images.InsertAt(
                Math.Min(Math.Max(index, 0), Bindable.Images.Count),
                src.SelectMany(e =>
                {
                    Bindable.SetMessage(Properties.Resources.MessageLoading, e);
                    if (!this.IsInsertable(e)) return new Page[0];
                    else if (e.IsPdf()) return _core.GetOrAdd(e).Pages;
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
            Invoke(() => Bindable.Images.InsertAt(index, src), "");

        /* ----------------------------------------------------------------- */
        ///
        /// Remove
        ///
        /// <summary>
        /// Removes the selected objects.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Remove() => Invoke(() => Bindable.Images.Remove(), "");

        /* ----------------------------------------------------------------- */
        ///
        /// Remove
        ///
        /// <summary>
        /// Removes the specified objects.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Remove(IEnumerable<int> indices) => Invoke(() => Bindable.Images.RemoveAt(indices), "");

        /* ----------------------------------------------------------------- */
        ///
        /// Move
        ///
        /// <summary>
        /// Moves the selected objects at the specified distance.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Move(int delta) => Invoke(() => Bindable.Images.Move(delta), "");

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
        public void Rotate(int degree) => Invoke(() => Bindable.Images.Rotate(degree), "");

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
        public void Update(Metadata value) => Invoke(() => this.SetMetadata(value), "");

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
        public void Update(Encryption value) => Invoke(() => this.SetEncryption(value), "");

        /* ----------------------------------------------------------------- */
        ///
        /// Rotate
        ///
        /// <summary>
        /// Rotates the selected items with the specified value.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Undo() => Invoke(() => Bindable.History.Undo(), "");

        /* ----------------------------------------------------------------- */
        ///
        /// Rotate
        ///
        /// <summary>
        /// Rotates the selected items with the specified value.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Redo() => Invoke(() => Bindable.History.Redo(), "");

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
            Bindable.Images.Zoom(offset);
            Bindable.ItemSize.Value = Bindable.Images.Preferences.ItemSize;
        }, "");

        /* ----------------------------------------------------------------- */
        ///
        /// Refresh
        ///
        /// <summary>
        /// Clears all of images and regenerates them.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Refresh() => Invoke(() => Bindable.Images.Refresh(), "");

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
            Interlocked.Exchange(ref _core, null)?.Clear();
            Bindable.Close();
            if (disposing) Bindable.Images.Dispose();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Invokes the user action and registers the hisotry item.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Invoke(Func<HistoryItem> func, string format, params object[] args) =>
            Invoke(() => Bindable.History.Register(func()), format, args);

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Invokes the user action and sets the result message.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Invoke(Action action, string format, params object[] args) => Bindable.Invoke(() =>
        {
            if (_core == null) return;
            action();
            Bindable.SetMessage(format, args);
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Update
        ///
        /// <summary>
        /// Updates values corresponding to the specified name.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Update(string name)
        {
            var src = Settings.Value;
            var dic = new Dictionary<string, Action>
            {
                { nameof(src.ItemSize),  () => this.Zoom() },
                { nameof(src.FrameOnly), () => Bindable.Images.Preferences.FrameOnly = src.FrameOnly },
            };

            if (dic.TryGetValue(name, out var action)) action();
        }

        #endregion

        #region Fields
        private DocumentCollection _core;
        #endregion
    }
}
