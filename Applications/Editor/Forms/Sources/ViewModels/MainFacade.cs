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
using Cube.Pdf.Pdfium;
using System;
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
    public class MainFacade
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
            Settings = settings;
            Images   = new ImageCollection(context);
            Bindable = new MainBindableData(Images, settings);

            Images.Preferences.ItemSize = settings.Value.ViewSize;
            Images.Preferences.ItemMargin = 3;
            Images.Preferences.TextHeight = 25;
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
        public MainBindableData Bindable { get; }

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
        /// Core
        ///
        /// <summary>
        /// Gets or sets a core object.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected DocumentReader Core
        {
            get => _core;
            set
            {
                if (_core == value) return;
                _core = value;
                Bindable.IsOpen.Value = _core != null;
            }
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Open
        ///
        /// <summary>
        /// Open a PDF document with the specified file path.
        /// </summary>
        ///
        /// <param name="src">File path.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Open(string src) => Invoke(() =>
        {
            Core = new DocumentReader(src);
            Images.Renderer = Core;
            foreach (var page in Core.Pages) Images.Add(page);
        });

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
            Images.Clear();
            Core?.Dispose();
            Core = null;
        });

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
        /// Refresh
        ///
        /// <summary>
        /// Clears all of images and regenerates them.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Refresh() => Invoke(() => Bindable.Images.Refresh());

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Invokes the user action.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Invoke(Action action)
        {
            Bindable.IsBusy.Value = true;
            try { action(); }
            finally { Bindable.IsBusy.Value = false; }
        }

        #endregion

        #region Fields
        private DocumentReader _core;
        #endregion
    }
}
