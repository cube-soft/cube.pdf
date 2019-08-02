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
using Cube.Mixin.Collections;
using System;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// MainBindable
    ///
    /// <summary>
    /// Provides values for binding to the MainWindow.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class MainBindable : ObservableBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// MainBindable
        ///
        /// <summary>
        /// Initializes a new instance of the MainBindable class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="images">Image collection.</param>
        /// <param name="settings">User settings.</param>
        /// <param name="invoker">Invoker object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public MainBindable(ImageCollection images, SettingFolder settings, Invoker invoker) : base(invoker)
        {
            _settings = settings;

            var index = images.Preferences.ItemSizeOptions.LastIndex(e => e <= ItemSize);
            images.Preferences.ItemSizeIndex = Math.Max(index, 0);
            images.Preferences.FrameOnly     = Settings.FrameOnly;
            images.Preferences.TextHeight    = 25;

            Images  = images;
            History = new History(invoker);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Images
        ///
        /// <summary>
        /// Gets an image collection of PDF documents.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ImageCollection Images { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Setting
        ///
        /// <summary>
        /// Gets a user settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public SettingValue Settings => _settings.Value;

        /* ----------------------------------------------------------------- */
        ///
        /// IO
        ///
        /// <summary>
        /// Gets the I/O handler.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IO IO => _settings.IO;

        /* ----------------------------------------------------------------- */
        ///
        /// History
        ///
        /// <summary>
        /// Gets a history to execute the undo and redo actions.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public History History { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Query
        ///
        /// <summary>
        /// Gets the password query.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IQuery<string> Query
        {
            get => _query;
            set => SetProperty(ref _query, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Metadata
        ///
        /// <summary>
        /// Gets the metadata of the PDF file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Metadata Metadata
        {
            get
            {
                if (_metadata == null) LazyLoad();
                return _metadata;
            }
            set => SetProperty(ref _metadata, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Encryption
        ///
        /// <summary>
        /// Gets the encryption information of the PDF file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Encryption Encryption
        {
            get
            {
                if (_encryption == null) LazyLoad();
                return _encryption;
            }
            set => SetProperty(ref _encryption, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Source
        ///
        /// <summary>
        /// Gets a file path of the PDF file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Entity Source
        {
            get => _source;
            set => SetProperty(ref _source, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Busy
        ///
        /// <summary>
        /// Gets the value indicating whether models are busy.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Busy
        {
            get => _busy;
            private set
            {
                if (SetProperty(ref _busy, value)) Refresh(nameof(Modified), nameof(Count));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Modified
        ///
        /// <summary>
        /// Gets the value indicating whether the PDF document is modified.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Modified => History.Undoable;

        /* ----------------------------------------------------------------- */
        ///
        /// Count
        ///
        /// <summary>
        /// Gets the number of pages in the PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Count => Images.Count;

        /* ----------------------------------------------------------------- */
        ///
        /// ItemSize
        ///
        /// <summary>
        /// Gets the displayed item size.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int ItemSize
        {
            get => Settings.ItemSize;
            set
            {
                if (Settings.ItemSize == value) return;
                Settings.ItemSize = value;
                Refresh(nameof(ItemSize));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Message
        ///
        /// <summary>
        /// Gets the message.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Message
        {
            get => _message;
            private set => SetProperty(ref _message, value);
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Invokes the specified action.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Invoke(Action action)
        {
            try
            {
                Busy = true;
                action();
            }
            catch (OperationCanceledException) { /* ignore user cancel */ }
            catch (Exception e) { Set(e.Message); throw; }
            finally { Busy = false; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Clear
        ///
        /// <summary>
        /// Clears the current properties.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Clear()
        {
            Source     = null;
            Metadata   = null;
            Encryption = null;

            History.Clear();
            Images.Clear();

            // for cleared images.
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Set
        ///
        /// <summary>
        /// Sets the specified message.
        /// </summary>
        ///
        /// <param name="format">Format for the message.</param>
        /// <param name="args">Additional arguments.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Set(string format, params object[] args) => Message = string.Format(format, args);

        /* ----------------------------------------------------------------- */
        ///
        /// Set
        ///
        /// <summary>
        /// Sets the Metadata object.
        /// </summary>
        ///
        /// <param name="src">Metadata object.</param>
        ///
        /// <returns>
        /// History item to execute undo and redo actions.
        /// </returns>
        ///
        /* ----------------------------------------------------------------- */
        public HistoryItem Set(Metadata src)
        {
            var m = Metadata;
            return HistoryItem.Invoke(() => Metadata = src, () => Metadata = m);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Set
        ///
        /// <summary>
        /// Sets the Encryption object.
        /// </summary>
        ///
        /// <param name="value">Encryption object.</param>
        ///
        /// <returns>
        /// History item to execute undo and redo actions.
        /// </returns>
        ///
        /* ----------------------------------------------------------------- */
        public HistoryItem Set(Encryption value)
        {
            var e = Encryption;
            return HistoryItem.Invoke(() => Encryption = value, () => Encryption = e);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Set
        ///
        /// <summary>
        /// Sets the metadata and encryption if needed.
        /// </summary>
        ///
        /// <param name="metadata">Metadata object.</param>
        /// <param name="encryption">Encryption object.</param>
        ///
        /* ----------------------------------------------------------------- */
        internal void Set(Metadata metadata, Encryption encryption)
        {
            if (_metadata   == null) _metadata   = metadata;
            if (_encryption == null) _encryption = encryption;
        }

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
            if (!disposing) return;
            Clear();
            Images.Dispose();
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// LazyLoad
        ///
        /// <summary>
        /// Loads metadata and encryption.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void LazyLoad()
        {
            if (Source == null) return;
            using (var r = Source.GetItext(Query, IO, true)) Set(r.Metadata, r.Encryption);
        }

        #endregion

        #region Fields
        private readonly SettingFolder _settings;
        private IQuery<string> _query;
        private Entity _source;
        private Metadata _metadata;
        private Encryption _encryption;
        private bool _busy = false;
        private string _message = string.Empty;
        #endregion
    }
}
