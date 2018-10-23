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
using Cube.Xui;
using Cube.Xui.Mixin;
using System;
using System.Diagnostics;

namespace Cube.Pdf.App.Editor
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
    public class MainBindable
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
        /// <param name="settings">Settings object.</param>
        /// <param name="query">Password query.</param>
        ///
        /* ----------------------------------------------------------------- */
        public MainBindable(ImageCollection images, SettingsFolder settings, IQuery<string> query)
        {
            _settings = settings;
            Images    = images;
            Query     = query;
            Busy      = new Bindable<bool>(() => _busy);
            Modified  = new Bindable<bool>(() => History.Undoable);
            Count     = new Bindable<int>(() => Images.Count);
            ItemSize  = new Bindable<int>(
                () => Settings.ItemSize,
                e =>
                {
                    if (Settings.ItemSize == e) return false;
                    Settings.ItemSize = e;
                    return true;
                }
            );
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
        /// Settings
        ///
        /// <summary>
        /// Gets an application settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Settings Settings => _settings.Value;

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
        public History History { get; } = new History();

        /* ----------------------------------------------------------------- */
        ///
        /// Query
        ///
        /// <summary>
        /// Gets the password query.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IQuery<string> Query { get; }

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
            set => _metadata = value;
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
            set => _encryption = value;
        }

        #region Bindable

        /* ----------------------------------------------------------------- */
        ///
        /// Source
        ///
        /// <summary>
        /// Gets a file path of the PDF file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Bindable<Information> Source { get; } = new Bindable<Information>();

        /* ----------------------------------------------------------------- */
        ///
        /// Busy
        ///
        /// <summary>
        /// Gets the value indicating whether models are busy.
        /// </summary>
        ///
        /// <remarks>
        /// この値は外部から変更する事はできません。Invoke メソッドの実行中
        /// のみ Busy.Value の値が true に設定されます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public Bindable<bool> Busy { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Modified
        ///
        /// <summary>
        /// Gets the value indicating whether the PDF document is modified.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Bindable<bool> Modified { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Count
        ///
        /// <summary>
        /// Gets the number of pages in the PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Bindable<int> Count { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// ItemSize
        ///
        /// <summary>
        /// Gets the displayed item size.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Bindable<int> ItemSize { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Message
        ///
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Bindable<string> Message { get; } = new Bindable<string>(string.Empty);

        #endregion

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// IsOpen
        ///
        /// <summary>
        /// Gets the value indicatint whether a PDF document is open.
        /// </summary>
        ///
        /// <returns>true for open.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public bool IsOpen() => Source.Value != null;

        /* ----------------------------------------------------------------- */
        ///
        /// Open
        ///
        /// <summary>
        /// Sets properties of the specified IDocumentReader.
        /// </summary>
        ///
        /// <param name="src">Document information.</param>
        ///
        /// <remarks>
        /// PDFium は Metadata や Encryption の情報取得が不完全なため、
        /// これらの情報は、必要になったタイミングで iTextSharp を用いて
        /// 取得します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public void Open(IDocumentReader src)
        {
            Source.Value = src.File;
            if (!src.Encryption.Enabled) Encryption = src.Encryption;
            Images.Add(src.Pages);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Close
        ///
        /// <summary>
        /// Clears properties of the current PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Close()
        {
            Source.Value = null;
            Metadata     = null;
            Encryption   = null;

            History.Clear();
            Images.Clear();
        }

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
                _busy = true;
                Busy.RaiseValueChanged();
                action();
            }
            catch (OperationCanceledException) { /* ignore user cancel */ }
            catch (Exception err) { SetMessage(err.Message); throw; }
            finally
            {
                _busy = false;
                Busy.RaiseValueChanged();
                Modified.RaiseValueChanged();
                Count.RaiseValueChanged();
            }
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
        public void Set(Metadata metadata, Encryption encryption)
        {
            if (_metadata   == null) _metadata   = metadata;
            if (_encryption == null) _encryption = encryption;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetMessage
        ///
        /// <summary>
        /// Sets the specified message.
        /// </summary>
        ///
        /// <param name="format">Format for the message.</param>
        /// <param name="args">Additional arguments.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void SetMessage(string format, params object[] args) =>
            Message.Value = string.Format(format, args);

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
            var src = Source.Value;
            Debug.Assert(src != null);
            using (var r = src.GetItexReader(Query, IO)) Set(r.Metadata, r.Encryption);
        }

        #endregion

        #region Fields
        private SettingsFolder _settings;
        private Metadata _metadata;
        private Encryption _encryption;
        private bool _busy = false;
        #endregion
    }
}
