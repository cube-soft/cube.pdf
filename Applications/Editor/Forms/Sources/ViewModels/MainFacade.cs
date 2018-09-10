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
using Cube.Collections.Mixin;
using Cube.FileSystem;
using Cube.Generics;
using Cube.Pdf.Itext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Cube.Pdf.App.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// MainFacade
    ///
    /// <summary>
    /// MainViewModel と各種 Model の窓口となるクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class MainFacade : IDisposable
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// MainFacade
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="settings">設定情報</param>
        /// <param name="context">同期用コンテキスト</param>
        ///
        /* ----------------------------------------------------------------- */
        public MainFacade(SettingsFolder settings, SynchronizationContext context)
        {
            _dispose = new OnceAction<bool>(Dispose);
            _core    = new DocumentCollection(e => Bindable.IsOpen.Value = e.Count > 0);
            Images   = new ImageCollection(e => _core.Get(e));
            Bindable = new MainBindable(Images, settings);
            Settings = settings;

            var sizes = Images.Preferences.ItemSizeOptions;
            var index = sizes.LastIndexOf(e => e <= settings.Value.ViewSize);
            Images.Preferences.ItemSizeIndex = Math.Max(index, 0);
            Images.Preferences.ItemMargin    = 3;
            Images.Preferences.TextHeight    = 25;
            Images.Context                   = context;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Settings
        ///
        /// <summary>
        /// 設定情報を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public SettingsFolder Settings { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Bindable
        ///
        /// <summary>
        /// Gets bindable data related with PDF docuemnts.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public MainBindable Bindable { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Images
        ///
        /// <summary>
        /// Gets the image collection.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected ImageCollection Images { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// IO
        ///
        /// <summary>
        /// Gets the I/O handler.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected IO IO => Settings.IO;

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
        public void Open(string src) => Invoke(() =>
        {
            if (!src.HasValue()) return;
            SetStatus(Properties.Resources.MessageLoading, src);

            var doc = _core.GetOrAdd(src);
            Bindable.Source.Value = IO.Get(src);
            Bindable.Metadata.Value = doc.Metadata;
            Bindable.Encryption.Value = doc.Encryption;
            Images.Add(doc.Pages);
        });

        /* ----------------------------------------------------------------- */
        ///
        /// OpenLink
        ///
        /// <summary>
        /// Opens a PDF document with the specified link.
        /// </summary>
        ///
        /// <param name="src">Information for the link.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void OpenLink(Information src) => Open(Shortcut.Resolve(src?.FullName)?.Target);

        /* ----------------------------------------------------------------- */
        ///
        /// Open
        ///
        /// <summary>
        /// Saves the PDF document to the specified file path.
        /// </summary>
        ///
        /// <param name="dest">File path.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Save(string dest) => Invoke(() =>
        {
            var name = IO.Get(dest).Name;
            SetStatus(Properties.Resources.MessageSaving, name);
            using (var writer = new DocumentWriter())
            {
                writer.Add(Images.Select(e => e.RawObject));
                writer.Set(Bindable.Metadata.Value);
                writer.Set(Bindable.Encryption.Value);
                writer.Save(dest);
            }
        });

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
        public void Extract(string dest) => Invoke(() => Images.Extract(dest));

        /* ----------------------------------------------------------------- */
        ///
        /// Select
        ///
        /// <summary>
        /// Sets or resets the IsSelected property of all items according
        /// to the current condition.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Select() => Select(Bindable.Selection.Count < Images.Count);

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
        public void Select(bool selected) => Invoke(() => Images.Select(selected));

        /* ----------------------------------------------------------------- */
        ///
        /// Flip
        ///
        /// <summary>
        /// Flips the IsSelected property of all items.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Flip() => Invoke(() => Images.Flip());

        /* ----------------------------------------------------------------- */
        ///
        /// Insert
        ///
        /// <summary>
        /// Inserts the page objects of the specified file path.
        /// </summary>
        ///
        /// <param name="src">File path.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Insert(string src) => Insert(Bindable.Selection.Last + 1, src);

        /* ----------------------------------------------------------------- */
        ///
        /// Insert
        ///
        /// <summary>
        /// Inserts the page objects at the specified index.
        /// </summary>
        ///
        /// <param name="index">Insertion index.</param>
        /// <param name="src">File path.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Insert(int index, string src) => Invoke(() =>
        {
            SetStatus(Properties.Resources.MessageLoading, IO.Get(src).Name);
            var n = Math.Min(Math.Max(index, 0), Images.Count);
            Images.InsertAt(n, _core.GetOrAdd(src).Pages);
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Remove
        ///
        /// <summary>
        /// Removes the selected objects.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Remove() => Invoke(() => Images.Remove());

        /* ----------------------------------------------------------------- */
        ///
        /// Remove
        ///
        /// <summary>
        /// Removes the specified objects.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Remove(IEnumerable<int> indices) => Invoke(() => Images.RemoveAt(indices));

        /* ----------------------------------------------------------------- */
        ///
        /// Move
        ///
        /// <summary>
        /// Moves the selected objects at the specified distance.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Move(int delta) => Invoke(() => Images.Move(delta));

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
        public void Rotate(int degree) => Invoke(() => Images.Rotate(degree));

        /* ----------------------------------------------------------------- */
        ///
        /// Update
        ///
        /// <summary>
        /// Updates the <c>Metadata</c> object.
        /// </summary>
        ///
        /// <param name="value"><c>Metadata</c> object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Update(Metadata value) => Invoke(() => this.SetMetadata(value));

        /* ----------------------------------------------------------------- */
        ///
        /// Update
        ///
        /// <summary>
        /// Updates the <c>Encryption</c> object.
        /// </summary>
        ///
        /// <param name="value"><c>Encryption</c> object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Update(Encryption value) => Invoke(() => this.SetEncryption(value));

        /* ----------------------------------------------------------------- */
        ///
        /// Rotate
        ///
        /// <summary>
        /// Rotates the selected items with the specified value.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Undo() => Invoke(() => Bindable.History.Undo());

        /* ----------------------------------------------------------------- */
        ///
        /// Rotate
        ///
        /// <summary>
        /// Rotates the selected items with the specified value.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Redo() => Invoke(() => Bindable.History.Redo());

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
        public void Zoom(int offset) => Invoke(() => Images.Zoom(offset));

        /* ----------------------------------------------------------------- */
        ///
        /// Refresh
        ///
        /// <summary>
        /// Clears all of images and regenerates them.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Refresh() => Invoke(() => Bindable.Images.Refresh());

        /* ----------------------------------------------------------------- */
        ///
        /// Close
        ///
        /// <summary>
        /// Closes the current PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Close() => Invoke(() =>
        {
            Bindable.Source.Value = null;
            Bindable.History.Clear();
            _core.Clear();
            Images.Clear();
        });

        #region IDisposable

        /* ----------------------------------------------------------------- */
        ///
        /// ~MainFacade
        ///
        /// <summary>
        /// Finalizes the <c>MainFacade</c>.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        ~MainFacade() { _dispose.Invoke(false); }

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases all resources used by the <c>MainFacade</c>.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Dispose()
        {
            _dispose.Invoke(true);
            GC.SuppressFinalize(this);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases the unmanaged resources used by the <c>MainFacade</c>
        /// and optionally releases the managed resources.
        /// </summary>
        ///
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void Dispose(bool disposing)
        {
            Close();
            if (disposing) Images.Dispose();
        }

        #endregion

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Invokes the user action and clears the message.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Invoke(Action action) => Invoke(action, string.Empty);

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Invokes the user action and registers the hisotry item.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Invoke(Func<HistoryItem> func) => Invoke(() => Bindable.History.Register(func()));

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Invokes the user action and sets the result message.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Invoke(Action action, string format, params object[] args)
        {
            try
            {
                Bindable.IsBusy.Value = true;
                action();
                SetStatus(format, args);
            }
            catch (Exception err) { SetStatus(err.Message); throw; }
            finally
            {
                Bindable.Count.Value = Images.Count;
                Bindable.IsBusy.Value = false;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetStatus
        ///
        /// <summary>
        /// Sets the specified message.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SetStatus(string format, params object[] args) =>
            Bindable.Message.Value = string.Format(format, args);

        #endregion

        #region Fields
        private readonly OnceAction<bool> _dispose;
        private readonly DocumentCollection _core;
        #endregion
    }
}
