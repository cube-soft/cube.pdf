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
using Cube.Xui.Mixin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        /// Initializes a new instance of the MainFacade class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="settings">User settings.</param>
        /// <param name="password">Password query.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public MainFacade(SettingsFolder settings, IQuery<string> password, SynchronizationContext context)
        {
            _dispose = new OnceAction<bool>(Dispose);
            _core    = new DocumentCollection(password);
            Bindable = new MainBindable(new ImageCollection(e => _core?.GetOrAdd(e), context), settings);

            Settings = settings;
            Settings.Load();
            Settings.PropertyChanged += SettingsChanged;

            var sizes = Bindable.Images.Preferences.ItemSizeOptions;
            var index = sizes.LastIndexOf(e => e <= Bindable.ItemSize.Value);

            Bindable.Images.Preferences.ItemSizeIndex = Math.Max(index, 0);
            Bindable.Images.Preferences.ItemMargin    = 3;
            Bindable.Images.Preferences.FrameOnly     = settings.Value.FrameOnly;
            Bindable.Images.Preferences.TextHeight    = 25;
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
        /// Setup
        ///
        /// <summary>
        /// Invokes some actions through the specified arguments.
        /// </summary>
        ///
        /// <param name="args">User arguments.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Setup(IEnumerable<string> args)
        {
            foreach (var ps in Settings.GetSplashProcesses()) ps.Kill();
            if (args.Count() <= 0) return;
            Open(args.First());
        }

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
            });
        }

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
        public void OpenLink(Information src)
        {
            try { Open(Shortcut.Resolve(src?.FullName)?.Target); }
            catch (Exception e)
            {
                var cancel = e is OperationCanceledException ||
                             e is TwiceException;
                if (!cancel) IO.TryDelete(src?.FullName);
                throw;
            }
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
            });
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
        ///
        /* ----------------------------------------------------------------- */
        public void Save(string dest) => Save(dest, true);

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
        private void Save(string dest, bool reopen) => Invoke(() =>
        {
            var file = IO.Get(dest);
            Bindable.SetMessage(Properties.Resources.MessageSaving, file.FullName);
            this.Save(file, () => _core.Clear());
            if (reopen) this.Restruct(_core.GetOrAdd(dest, Bindable.Encryption.Value.OwnerPassword));
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
        public void Extract(string dest) => Invoke(
            () => Bindable.Images.Extract(dest),
            Properties.Resources.MessageSaved, dest
        );

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
        public void Select(bool selected) => Invoke(() => Bindable.Images.Select(selected));

        /* ----------------------------------------------------------------- */
        ///
        /// Flip
        ///
        /// <summary>
        /// Flips the IsSelected property of all items.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Flip() => Invoke(() => Bindable.Images.Flip());

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
            Bindable.SetMessage(Properties.Resources.MessageLoading, src);
            var n = Math.Min(Math.Max(index, 0), Bindable.Images.Count);
            return Bindable.Images.InsertAt(n, _core.GetOrAdd(src).Pages);
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
        public void Remove() => Invoke(() => Bindable.Images.Remove());

        /* ----------------------------------------------------------------- */
        ///
        /// Remove
        ///
        /// <summary>
        /// Removes the specified objects.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Remove(IEnumerable<int> indices) => Invoke(() => Bindable.Images.RemoveAt(indices));

        /* ----------------------------------------------------------------- */
        ///
        /// Move
        ///
        /// <summary>
        /// Moves the selected objects at the specified distance.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Move(int delta) => Invoke(() => Bindable.Images.Move(delta));

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
        public void Rotate(int degree) => Invoke(() => Bindable.Images.Rotate(degree));

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
        public void Update(Metadata value) => Invoke(() => this.SetMetadata(value));

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
        public void Zoom(int offset) => Invoke(() =>
        {
            Bindable.Images.Zoom(offset);
            Bindable.ItemSize.Value = Bindable.Images.Preferences.ItemSize;
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Refresh
        ///
        /// <summary>
        /// Clears all of images and regenerates them.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Refresh() => this.Invoke(() => Bindable.Images.Refresh());

        #region IDisposable

        /* ----------------------------------------------------------------- */
        ///
        /// ~MainFacade
        ///
        /// <summary>
        /// Finalizes the MainFacade.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        ~MainFacade() { _dispose.Invoke(false); }

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases all resources used by the MainFacade.
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
        protected virtual void Dispose(bool disposing)
        {
            Interlocked.Exchange(ref _core, null)?.Clear();
            Bindable.Close();
            if (disposing) Bindable.Images.Dispose();
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
        private void Invoke(Func<HistoryItem> func) =>
            Invoke(() => Bindable.History.Register(func()));

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
            if (_core == null) return;

            try
            {
                Bindable.Busy.Value = true;
                action();
                Bindable.SetMessage(format, args);
            }
            catch (OperationCanceledException) { /* ignore user cancel */ }
            catch (Exception err) { Bindable.SetMessage(err.Message); throw; }
            finally
            {
                Bindable.Modified.Raise();
                Bindable.Count.Raise();
                Bindable.Busy.Value = false;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SettingsChanged
        ///
        /// <summary>
        /// Occurs when the PropertyChanged event is fired.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SettingsChanged(object s, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Settings.Value.ItemSize):
                    this.Zoom();
                    break;
                case nameof(Settings.Value.FrameOnly):
                    Bindable.Images.Preferences.FrameOnly = Settings.Value.FrameOnly;
                    break;
            }
        }

        #endregion

        #region Fields
        private readonly OnceAction<bool> _dispose;
        private DocumentCollection _core;
        #endregion
    }
}
